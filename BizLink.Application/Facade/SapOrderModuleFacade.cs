using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using Dm.util;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Facade
{
    public class SapOrderModuleFacade : BaseAppFacade
    {
        public IMaterialViewService MaterialView
        {
            get;
        }
        public ICableCutParamService CableCutParam
        {
            get;
        }

        public SapOrderModuleFacade(
            IMaterialViewService materialView,
            ICableCutParamService cableCutParam,

            // 2. 基础服务 (透传给基类 BaseAppFacade)
            IParameterGroupService paramsService,
            IMesApiClient mesApi,
            IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings,
            IFactoryService factoryService,
            IWorkCenterGroupService workCenterGroupService,
            IWorkCenterService workCenter,
            IWorkStationService workStation,
            IWorkOrderService workOrderService,
            IWorkOrderProcessService workOrderProcessService,
            IWorkOrderBomItemService workOrderBomItemService,
            IUserService userService,
            IMenuService menuService,
            IPermissionService permissionService,
            IRoleService roleService,
            IActivityLogService activityLogService
            ) : base(
                paramsService,
                mesApi,
                apiSettings, // 这里如果不需要 ApiSettings 可以传 null，或者补充注入
                factoryService,
                workCenterGroupService,
                workCenter,
                workStation,
                workOrderService,
                workOrderProcessService,
                workOrderBomItemService,
                userService,
                menuService,
                permissionService,
                roleService,
                activityLogService)
        {
            MaterialView = materialView;
            CableCutParam = cableCutParam;
        }

        /// <summary>
        /// 获取并处理 SAP 订单数据 (包含与本地 DB 的比对逻辑)
        /// </summary>
        public async Task<SapOrderDto> GetProcessedSapOrdersAsync(string factoryCode, DateTime dispatchDate, List<string> orderNos = null)
        {
            string requestUrl;
            ApiResponse<SapOrderDto> result;

            // 1. 调用 API 获取原始数据
            if (orderNos == null || !orderNos.Any())
            {
                string endpoint = ApiSettings["MesApi"].Endpoints["GetWorkOrdersByDispatchDate"];
                requestUrl = $"{endpoint}?factoryCode={factoryCode}&dispatchDate={dispatchDate:yyyy-MM-dd}";
                result = await MesApi.GetAsync<SapOrderDto>(requestUrl);
            }
            else
            {
                requestUrl = ApiSettings["MesApi"].Endpoints["GetWorkOrderByOrderNos"];
                result = await MesApi.PostAsync<object, SapOrderDto>(requestUrl, new
                {
                    factoryCode,
                    OrderNos = orderNos
                });
            }

            if (!result.IsSuccess || result.Data == null)
            {
                throw new Exception(result.Message ?? "未查询到 SAP 数据");
            }

            var sapData = result.Data;
            var operations = sapData.sapOrderOperations;
            var boms = sapData.sapOrderBoms?.Where(x => x.RequireQuantity > 0).ToList() ?? new List<SapOrderBom>();

            // 2. 业务逻辑处理：与本地 DB 比对，移除已同步的条目
            if (operations != null && operations.Any())
            {
                var orderNumbers = operations.Select(x => x.OrderNo).Distinct().ToList();

                // 获取本地已存在的 BOM 和 工艺
                var dbBoms = await WorkOrderBomItemService.GetListByOrderNoAync(orderNumbers);
                var existingBomOrderNos = dbBoms.Where(x => x.SyncWMSStatus > 0).Select(x => x.WorkOrderNo).ToHashSet();


                //// 获取本地已存在的工单ID对应的工艺
                //// 注意：原逻辑是通过 BOM 反查 Process，这里简化逻辑，直接查已存在的工单
                //var dbOps = await WorkOrderProcessService.GetListByOrderNos(orderNumbers);
                //var existingOpKeys = dbOps.Select(x => (x.WorkOrderNo, x.Operation)).ToHashSet();

                // 过滤掉本地已存在的
                //operations.RemoveAll(p => existingBomOrderNos.Contains(p.OrderNo));
                //boms.RemoveAll(b => existingBomOrderNos.Contains(b.OrderNo));
                operations.Where(p => existingBomOrderNos.Contains(p.OrderNo)).ToList().ForEach(p => p.MesStatus = "修改");
                // 标记状态
                operations.Where(p => !existingBomOrderNos.Contains(p.OrderNo)).ToList().ForEach(p => p.MesStatus = "新增");

                // 3. 计算 NextWorkCenter (链表逻辑)
                var groupedByOrder = operations.GroupBy(step => step.OrderNo);
                foreach (var orderGroup in groupedByOrder)
                {
                    var sortedSteps = orderGroup.OrderBy(step => step.OperationNo).ToList();
                    for (int i = 0; i < sortedSteps.Count - 1; i++)
                    {
                        sortedSteps[i].NextWorkCenter = sortedSteps[i + 1].WorkCenter;
                    }
                }
            }

            sapData.sapOrderOperations = operations;
            sapData.sapOrderBoms = boms;
            return sapData;
        }

        /// <summary>
        /// 执行同步逻辑：物料 -> 工单 -> 断线参数
        /// </summary>
        public async Task SyncSapDataToMesAsync(
            SapOrderDto sapData,
            int factoryId,
            DateTime startDate,
            DateTime endDate,
            string currentUser,
            IProgress<float> progress = null)
        {
            // 1. 同步物料信息
            var materialList = sapData.sapOrderOperations.Select(x => x.MaterialCode)
                .Concat(sapData.sapOrderBoms.Select(x => x.MaterialCode))
                .Distinct().ToList();
            var factory = await FactoryService.GetByIdAsync(factoryId);

            var matSyncUrl = ApiSettings["MesApi"].Endpoints["GetListByMaterialCodeFromSap"];
            await MesApi.PostAsync<object, object>(matSyncUrl, new
            {
                FactoryCode = factory.FactoryCode,
                MaterialCodes = materialList
            });

            // 2. 获取 SAP 数据中的所有订单号
            var orderNos = sapData.sapOrderOperations.Select(x => x.OrderNo).Distinct().ToList();
            if (!orderNos.Any())
                return;
            // 3. 【关键步骤】查询 MES 中已存在的 订单、工序、BOM
            // 需要在对应的 Service 中支持这些查询方法，如果不支持，需要先扩充 Service
            var existingOrders = await WorkOrderService.GetByOrdrNoAsync(orderNos);
            var existingProcesses = await WorkOrderProcessService.GetListByOrderNos(orderNos);
            var existingBoms = await WorkOrderBomItemService.GetListByOrderNoAync(orderNos);

            // --- 准备增量/更新列表 ---
            var ordersToInsert = new List<WorkOrderCreateDto>();
            var ordersToUpdate = new List<WorkOrderUpdateDto>(); // 如果需要更新工单头信息

            var processesToInsert = new List<WorkOrderProcessCreateDto>();
            var processesToUpdate = new List<WorkOrderProcessUpdateDto>();

            var bomsToInsert = new List<WorkOrderBomItemCreateDto>();
            var bomsToUpdate = new List<WorkOrderBomItemUpdateDto>();

            var minOpDict = sapData.sapOrderOperations
                    .GroupBy(x => x.OrderNo).ToDictionary(g => g.Key,g => g.Min(x => x.OperationNo));
            // 4. 处理工单 (WorkOrder)
            var sapOrderGroups = sapData.sapOrderOperations.GroupBy(op => op.OrderNo);

            var orderMaterialCodes = sapData.sapOrderOperations.Select(x => x.MaterialCode.TrimStart('0')).Distinct().ToList();
            var orderMaterials = await MaterialView.GetListByCodesAsync(factory.FactoryCode, orderMaterialCodes);
            var orderMaterialMap = orderMaterials.ToDictionary(x => x.MaterialCode, x => x);

            foreach (var group in sapOrderGroups)
            {
                var first = group.First();
                var existingOrder = existingOrders.FirstOrDefault(x => x.OrderNumber == first.OrderNo);

                if (existingOrder == null)
                {
                    // 新增
                    string materialDesc = string.Empty;
                    if (orderMaterialMap.TryGetValue(first.MaterialCode.TrimStart('0'), out var matView))
                    {
                        materialDesc = matView.MaterialName; // 假设 View DTO 里是 int
                    }

                    ordersToInsert.Add(new WorkOrderCreateDto
                    {
                        FactoryId = factoryId,
                        OrderNumber = first.OrderNo,
                        MaterialCode = first.MaterialCode,
                        MaterialDesc = materialDesc,
                        Quantity = first.TargetQuantity,
                        DispatchDate = endDate,
                        Status = ((int)WorkOrderStatus.New).ToString(),
                        ScheduledStartDate = startDate,
                        ScheduledFinishDate = endDate,
                        BasicStartDate = startDate,
                        BasicFinishDate = endDate,
                        CollectiveOrder = first.CollectiveOrder,
                        SuperiorOrder = first.SuperiorOrder,
                        LeadingOrder = first.LeadingOrder,
                        LeadingOrderMaterial = first.LeadingMaterial,
                        ComponentMainMaterial = first.LeadingMaterial,
                        ReservationNo = first.ReservationNo,
                        PlanningPeriod = first.PlanPeriod,
                        StorageLocation = first.StorageLocation,
                        ProfitCenter = first.ProfitCenter,
                        LabelCount = first.LabelCount,
                        CreateBy = currentUser
                    });
                }
                else
                {
                    ordersToUpdate.Add(new WorkOrderUpdateDto()
                    {
                        Id = existingOrder.Id,
                        Quantity = first.TargetQuantity,
                        Status = ((int)WorkOrderStatus.New).ToString(),
                        DispatchDate = endDate,
                        ScheduledStartDate = startDate,
                        ScheduledFinishDate = endDate,
                        BasicStartDate = startDate,
                        BasicFinishDate = endDate,
                        CollectiveOrder = first.CollectiveOrder,
                        SuperiorOrder = first.SuperiorOrder,
                        LeadingOrder = first.LeadingOrder,
                        LeadingOrderMaterial = first.LeadingMaterial,
                        ComponentMainMaterial = first.LeadingMaterial,
                        ReservationNo = first.ReservationNo,
                        PlanningPeriod = first.PlanPeriod,
                        StorageLocation = first.StorageLocation,
                        ProfitCenter = first.ProfitCenter,
                        LabelCount = first.LabelCount,
                        UpdateBy = currentUser,
                        UpdateOn = DateTime.Now
                    });
                }
            }

            if (ordersToInsert.Any())
            {
                await WorkOrderService.CreateBatchAsync(ordersToInsert);
                // 【关键】重新查询以获取新创建工单的 ID
                var newOrderNos = ordersToInsert.Select(x => x.OrderNumber).ToList();
                var newOrders = await WorkOrderService.GetByOrdrNoAsync(newOrderNos);
                existingOrders.AddRange(newOrders);
            }

            if (ordersToUpdate.Any())
            {
                await WorkOrderService.UpdateBatchAsync(ordersToUpdate);
            }

            // 创建工单号 -> ID 映射
            var orderIdMap = existingOrders.ToDictionary(x => x.OrderNumber, x => x.Id);

            // 5. 处理工序 (Process)
            foreach (var op in sapData.sapOrderOperations)
            {
                if (!orderIdMap.ContainsKey(op.OrderNo))
                    continue; // 安全检查
                var existingOp = existingProcesses.FirstOrDefault(p => p.WorkOrderNo == op.OrderNo && p.Operation == op.OperationNo);

                if (existingOp == null)
                {
                    // 新增
                    processesToInsert.Add(new WorkOrderProcessCreateDto
                    {
                        WorkOrderId = orderIdMap[op.OrderNo], // 注入 ID
                        WorkOrderNo = op.OrderNo,
                        ConfirmNo = op.ConfirmNo,
                        Status = ((int)WorkOrderStatus.New).ToString(),
                        Quantity = op.TargetQuantity,
                        StartTime = op.OperationNo == minOpDict[op.OrderNo] ? startDate : DateTime.Parse(op.ProductStart),
                        EndTime = DateTime.Parse(op.ProductFinish),
                        WorkCenter = op.WorkCenter,
                        Operation = op.OperationNo,
                        ControlKey = op.ControlKey,
                        NextWorkCenter = op.NextWorkCenter,
                        SetupTime = op.SetupTime1,
                        SetupTimeUnit = op.ConfActiUnit1,
                        MachineTime = op.MachineTime1,
                        MachineTimeUnit = op.ConfActiUnit3,
                        CreateBy = currentUser
                    });
                }
                else
                {
                    // 更新 (工序通常允许覆盖)
                    processesToUpdate.Add(new WorkOrderProcessUpdateDto
                    {
                        Id = existingOp.Id,
                        Quantity = op.TargetQuantity,
                        StartTime = op.OperationNo == minOpDict[op.OrderNo] ? startDate : DateTime.Parse(op.ProductStart),
                        EndTime = DateTime.Parse(op.ProductFinish),
                        WorkCenter = op.WorkCenter, // 可能换线
                        SetupTime = op.SetupTime1,
                        SetupTimeUnit = op.ConfActiUnit1,
                        MachineTime = op.MachineTime1,
                        MachineTimeUnit = op.ConfActiUnit3,
                        ControlKey = op.ControlKey,
                        ConfirmNo = op.ConfirmNo,
                        NextWorkCenter = op.NextWorkCenter,
                        UpdateBy = currentUser,
                        UpdateOn = DateTime.Now
                    });
                }
            }

            if (processesToUpdate.Any())
                await WorkOrderProcessService.UpdateBatchAsync(processesToUpdate);
            if (processesToInsert.Any())
            {
                await WorkOrderProcessService.CreateBatchAsync(processesToInsert);
                // 【关键】重新查询以获取新创建工序的 ID (用于 BOM 关联)
                // 注意：这里简单起见，重新查询所有相关工单的工序
                var allProcesses = await WorkOrderProcessService.GetListByOrderNos(orderNos);
                existingProcesses = allProcesses;
            }

            // 创建 (OrderNo, Operation) -> ProcessId 映射
            var processIdMap = existingProcesses.ToDictionary(x => (x.WorkOrderNo, x.Operation), x => x.Id);

            // 6. 处理 BOM (核心逻辑：Check SyncWMSStatus)
            // 先获取物料属性用于ConsumeType
            var bomMaterialCodes = sapData.sapOrderBoms.Select(x => x.MaterialCode.TrimStart('0')).Distinct().ToList();
            var bomMaterials = await MaterialView.GetListByCodesAsync(factory.FactoryCode, bomMaterialCodes);
            var materialMap = bomMaterials.ToDictionary(x => x.MaterialCode, x => x);
            // 用于记录 SAP 中存在的 BOM，以便后续比对找出 MES 中多余的 BOM
            var sapBomKeys = new HashSet<(string OrderNo, int ReservationItem, string MaterialCode)>();
            int i = 0;
            foreach (var ob in sapData.sapOrderBoms)
            {
                sapBomKeys.Add((ob.OrderNo, (int)ob.ReservationItem, ob.MaterialCode));


                if (!orderIdMap.ContainsKey(ob.OrderNo))
                    continue;

                int? processId = null;
                if (processIdMap.TryGetValue((ob.OrderNo, ob.Operation), out var pid))
                    processId = pid;
                // 确定 ConsumeType
                int? consumeType = ob.ConsumeType;
                if (materialMap.TryGetValue(ob.MaterialCode.TrimStart('0'), out var matView))
                {
                    consumeType = matView.ConsumeType; // 假设 View DTO 里是 int
                }

                // 查找现有 BOM
                var existingBom = existingBoms.FirstOrDefault(b =>
                    b.WorkOrderNo == ob.OrderNo &&
                    b.ReservationItem == ob.ReservationItem && // 假设 BomItem 是唯一行号标识
                    b.MaterialCode == ob.MaterialCode);

                if (existingBom == null)
                {
                    // [新增]：数据库中没有，插入
                    bomsToInsert.Add(new WorkOrderBomItemCreateDto
                    {
                        WorkOrderId = orderIdMap[ob.OrderNo],
                        WorkOrderProcessId = (int)processId,
                        WorkOrderNo = ob.OrderNo,
                        MaterialCode = ob.MaterialCode,
                        MaterialDesc = ob.MaterialDesc,
                        RequiredQuantity = ob.RequireQuantity,
                        Unit = ob.BaseUnit,
                        ReservationItem = ob.ReservationItem,
                        ComponentScrap = ob.ComponentScrap,
                        BomItem = ob.BomItem,
                        Operation = ob.Operation,
                        MovementAllowed = ob.AllowedMovement.Contains("X"),
                        QuantityIsFixed = ob.FixIndicator.Contains("X"),
                        ConsumeType = consumeType,
                        SyncWMSStatus = 0, // 默认为 0
                        SuperMaterialCode = ob.SuperMaterialCode,
                        CreateBy = currentUser
                    });
                }
                else
                {
                    // [修改]：数据库中有
                    // 核心条件：只有当 SyncWMSStatus == 0 (未推送到 WMS) 时，才允许 SAP 数据覆盖本地
                    if (existingBom.SyncWMSStatus == 0)
                    {
                        bomsToUpdate.Add(new WorkOrderBomItemUpdateDto
                        {
                            Id = existingBom.Id,
                            RequiredQuantity = ob.RequireQuantity, // 允许更新数量
                            Unit = ob.BaseUnit,
                            ComponentScrap = ob.ComponentScrap,
                            BomItem = ob.BomItem,
                            ReservationItem = ob.ReservationItem,
                            QuantityIsFixed = ob.FixIndicator.Contains("X"),
                            MovementAllowed = ob.AllowedMovement.Contains("X"),
                            ConsumeType = consumeType,
                            SuperMaterialCode = ob.SuperMaterialCode,
                            UpdateBy = currentUser,
                            UpdateOn = DateTime.Now
                        });
                    }
                    // else { Log: BOM已推送WMS，忽略SAP更新 }
                }

            }

            foreach (var dbBom in existingBoms)
            {
                // 检查该 BOM 是否在 SAP 数据中存在
                if (!sapBomKeys.Contains((dbBom.WorkOrderNo, (int)dbBom.ReservationItem, dbBom.MaterialCode)))
                {
                    // SAP 中不存在 -> 说明被删除了
                    // 如果未推送 WMS，则将其需求量置为 0
                    if (dbBom.RequiredQuantity > 0)
                    {
                        bomsToUpdate.Add(new WorkOrderBomItemUpdateDto
                        {
                            Id = dbBom.Id,
                            RequiredQuantity = 0, // 置为 0，代表该行已失效
                            MovementAllowed  = false,
                            UpdateBy = currentUser,
                            UpdateOn = DateTime.Now
                        });
                    }
                }
            }

            // 7. 执行数据库操作
            // 建议在 Service 层提供 BatchCreate / BatchUpdate 方法

            // 9. 执行 BOM DB 操作
            if (bomsToUpdate.Any())
                await WorkOrderBomItemService.UpdateBatchAsync(bomsToUpdate);
            if (bomsToInsert.Any())
                await WorkOrderBomItemService.CreateBatchAsync(bomsToInsert);

            // 8. 同步断线参数 (逻辑保持不变)
            if (sapData.sapOrderOperations.Any())
            {
                var semimaterials = sapData.sapOrderOperations.Select(x => x.MaterialCode.TrimStart('0')).Distinct().ToList();
                // ... 获取并 CreateBatchAsync ...
                // (调用 API 获取断线参数并保存)
                var cutParamUrl = ApiSettings["MesApi"].Endpoints["GetCableCutParamByMaterial"];
                var cutResult = await MesApi.PostAsync<object, List<CableCutParamCreateDto>>(cutParamUrl, new
                {
                    semiMaterialCode = semimaterials
                });
                if (cutResult.IsSuccess && cutResult.Data.Any())
                {
                    await CableCutParam.CreateBatchAsync(cutResult.Data);
                }
            }

            // (此处保留原有的复杂 GroupBy 和 Select 逻辑，为了篇幅简化展示，实际应完整复制原逻辑)
            //var workOrders = sapData.sapOrderOperations
            //    .GroupBy(op => op.OrderNo) // 简化分组
            //    .Select(g =>
            //    {
            //        var first = g.First();
            //        return new WorkOrderCreateDto
            //        {
            //            FactoryId = factory.Id,
            //            OrderNumber = first.OrderNo,
            //            MaterialCode = first.MaterialCode,
            //            Quantity = first.TargetQuantity,
            //            DispatchDate = endDate,
            //            Status = ((int)WorkOrderStatus.New).ToString(),
            //            ScheduledStartDate = startDate,
            //            ScheduledFinishDate = endDate,
            //            BasicStartDate = startDate,
            //            BasicFinishDate = endDate,
            //            CollectiveOrder = first.CollectiveOrder,
            //            SuperiorOrder = first.SuperiorOrder,
            //            LeadingOrder = first.LeadingOrder,
            //            LeadingOrderMaterial = first.LeadingMaterial,
            //            ComponentMainMaterial = first.LeadingMaterial,
            //            ReservationNo = first.ReservationNo,
            //            PlanningPeriod = first.PlanPeriod,
            //            StorageLocation = first.StorageLocation,
            //            ProfitCenter = first.ProfitCenter,
            //            LabelCount = first.LabelCount,
            //            CreateBy = currentUser
            //        };
            //    }).ToList();

            //var processes = sapData.sapOrderOperations.Select(op => new WorkOrderProcessCreateDto
            //{
            //    WorkOrderNo = op.OrderNo,
            //    ConfirmNo = op.ConfirmNo,
            //    Status = ((int)WorkOrderStatus.New).toString(),
            //    Quantity = op.TargetQuantity,
            //    StartTime = op.OperationNo == minOpDict[op.OrderNo]
            //            ? startDate : DateTime.Parse(op.ProductStart),
            //    EndTime = DateTime.Parse(op.ProductFinish),
            //    WorkCenter = op.WorkCenter,
            //    Operation = op.OperationNo,
            //    ControlKey = op.ControlKey,
            //    NextWorkCenter = op.NextWorkCenter,
            //    SetupTime = op.SetupTime1,
            //    SetupTimeUnit = op.ConfActiUnit1,
            //    MachineTime = op.MachineTime1,
            //    MachineTimeUnit = op.ConfActiUnit3,
            //    CreateBy = currentUser
            //}).ToList();

            //// 3. 获取 BOM 物料属性以更新 ConsumeType
            //var bomMaterialCodes = sapData.sapOrderBoms.Select(x => x.MaterialCode.TrimStart('0')).Distinct().ToList();
            //var bomMaterials = await MaterialView.GetListByCodesAsync(factory.FactoryCode, bomMaterialCodes);
            //var materialMap = bomMaterials.ToDictionary(x => x.MaterialCode, x => x);

            //foreach (var item in sapData.sapOrderBoms)
            //{
            //    if (materialMap.TryGetValue(item.MaterialCode.TrimStart('0'), out var matView))
            //    {
            //        item.ConsumeType = matView.ConsumeType;
            //    }
            //}

            //var bomItems = sapData.sapOrderBoms.Select(ob => new WorkOrderBomItemCreateDto
            //{
            //    WorkOrderNo = ob.OrderNo,
            //    MaterialCode = ob.MaterialCode,
            //    MaterialDesc = ob.MaterialDesc,
            //    RequiredQuantity = ob.RequireQuantity,
            //    Unit = ob.BaseUnit,
            //    ReservationItem = ob.ReservationItem,
            //    ComponentScrap = ob.ComponentScrap,
            //    BomItem = ob.BomItem,
            //    Operation = ob.Operation,
            //    MovementAllowed = ob.AllowedMovement.contains("X") ? true : false,
            //    QuantityIsFixed = ob.FixIndicator.contains("X") ? true : false,
            //    ConsumeType = ob.ConsumeType,
            //    SyncWMSStatus = (int)RequistStatus.No,
            //    SuperMaterialCode = ob.SuperMaterialCode,
            //    CreateBy = currentUser
            //}).ToList();

            //// 4. 执行批量创建
            //await WorkOrderService.CreateBySAPAsync(factory.FactoryCode, workOrders, processes, bomItems, progress);

            //// 5. 同步断线参数
            //if (sapData.sapOrderOperations.Any())
            //{
            //    var semimaterials = sapData.sapOrderOperations.Select(x => x.MaterialCode.TrimStart('0')).Distinct().ToList();
            //    var cutParamUrl = ApiSettings["MesApi"].Endpoints["GetCableCutParamByMaterial"];
            //    var cutResult = await MesApi.PostAsync<object, List<CableCutParamCreateDto>>(cutParamUrl, new
            //    {
            //        semiMaterialCode = semimaterials
            //    });

            //    if (cutResult.IsSuccess && cutResult.Data.Any())
            //    {
            //        await CableCutParam.CreateBatchAsync(cutResult.Data);
            //    }
            //}
        }
    }
}

