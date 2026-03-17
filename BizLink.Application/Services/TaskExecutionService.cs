using Azure.Core;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Response;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using Dm.util;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace BizLink.MES.Application.Services
{

    public class TaskExecutionService : ITaskExecutionService
    {

        // 核心任务仓储
        private readonly IWorkOrderMaterialTaskRepository _taskRepo;
        private readonly IWorkOrderStepTaskRepository _stepRepo;
        private readonly IWorkOrderOperationTaskRepository _opTaskRepo;
        private readonly IWorkOrderKittingItemRepository _kittingItemRepo;

        // 物料与库存仓储
        private readonly IStationMaterialLoadingRepository _loadingRepo;
        private readonly IRawLinesideStockRepository _inventoryRepo;

        // 报工记录仓储
        private readonly IWorkOrderTaskExecuteLogRepository _exeLogRepo;
        private readonly IWorkOrderTaskExecuteConsumpRepository _exeConsumpRepo;

        // 基础信息仓储 (用于补充 ActualWorkCenterId)
        private readonly IWorkStationRepository _stationRepo;
        private readonly IWorkCenterRepository _centerRepo;
        private readonly ISerialSequenceRepository _serialRepo;
        private readonly IFactoryRepository _factoryRepository;
        private readonly IParameterGroupRepository _parameterGroupRepo;
        private readonly IWorkTaskCategoryRepository _categoryRepository;


        // --- 工单相关仓储 (Kitting 逻辑需要) ---
        private readonly IWorkOrderRepository _workOrderRepo;
        private readonly IWorkOrderBomItemRepository _bomRepo;
        private readonly IWorkOrderProcessRepository _processRepo;
        private readonly IMaterialViewService _materialService;
        private readonly IWmsWorkOrderPickingLogRepository _pickingLogRepo;

        private readonly IUnitOfWork _unitOfWork;
        // ★ 新增：用于在 WinForms 长生命周期中创建短生命周期的 Scope
        private readonly IServiceScopeFactory _scopeFactory;

        //sap移库
        private readonly ISapIntegrationService _sapIntegrationService;
        private readonly ISapRfcService _sapRfcService;

        private readonly IWorkOrderOperationConfirmService _opConfirmService;
        private readonly IWorkOrderOperationConsumpService _opConsumpService;

        //入库
        private readonly IFinishedGoodsReceiptService _finishedGoodsReceiptService;


        //WMS出库
        private readonly IJyApiClient _jyApiClient;
        private readonly Dictionary<string, ServiceEndpointSettings> _apiSettings;
        private readonly IActivityLogRepository _activityLogRepo;

        private const string REPORT_SEQUENCE_CODE = "CN11ConfirmReportToSAP";
        private const string MOVEMENT_PROD = "261"; // 生产投料
        private const string MOVEMENT_SCRAP = "903"; // 生产报废

        private const string TASKCATEGORY_PROCESS = "PROCESS";


        private const string CONTROL_KEY = "CN06"; // 生产报废

        private const string Group_SapLocation = "CN11SAPStockLocation";
        private const string Group_BagMaterialList = "BagMaterialList";

        private const string SAP_TRANSFER_CONFIG_GROUP = "TransferSAP";
        private const string SAP_CONFIG_KEY_ENABLED = "IsEnabled";

        private const string Key_CableRawLineStock = "CableRawLineStock";
        private const string Key_CableProLineStock = "CableProLineStock";
        private const string Key_SAPRawMtrStock = "SAPRawMtrStock";

        // ★ 新增：里程碑配置的参数键名
        private const string Group_MilestoneConfig = "MilestoneConfig";
        private const string Key_MilestoneSteps = "MilestoneTaskCategory";
        private const string Key_SapPrevTriggers = "SapPrevTriggers";
        private const string Key_SapCurrentTriggers = "SapCurrentTriggers";




        public TaskExecutionService(
            IWorkOrderMaterialTaskRepository taskRepo,
            IWorkOrderStepTaskRepository stepRepo,
            IWorkOrderOperationTaskRepository opTaskRepo,
            IStationMaterialLoadingRepository loadingRepo,
            IRawLinesideStockRepository inventoryRepo,
            IWorkOrderTaskExecuteLogRepository exeLogRepo,
            IWorkOrderTaskExecuteConsumpRepository exeConsumpRepo,
            IWorkStationRepository stationRepo,
            IWorkCenterRepository centerRepo,
            IUnitOfWork unitOfWork,
            IServiceScopeFactory scopeFactory,
            ISerialSequenceRepository serialRepo,
            IParameterGroupRepository parameterGroupRepo,
            IWorkTaskCategoryRepository categoryRepository,
            IWorkOrderRepository workOrderRepo,
            IWorkOrderBomItemRepository bomRepo,
            IWorkOrderProcessRepository processRepo,
            IMaterialViewService materialService,
            IFactoryRepository factoryRepository,
            IWmsWorkOrderPickingLogRepository pickingLogRepo,
            IWorkOrderKittingItemRepository kittingItemRepo,
            ISapIntegrationService sapIntegrationService,
            ISapRfcService sapRfcService,
            IWorkOrderOperationConfirmService confirmService,
            IWorkOrderOperationConsumpService consumpService,
            IJyApiClient jyApiClient, 
            IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings,
            IActivityLogRepository activityLog,
            IFinishedGoodsReceiptService finishedGoodsReceiptService
            )
        {
            _taskRepo = taskRepo;
            _stepRepo = stepRepo;
            _opTaskRepo = opTaskRepo;
            _loadingRepo = loadingRepo;
            _inventoryRepo = inventoryRepo;
            _exeLogRepo = exeLogRepo;
            _exeConsumpRepo = exeConsumpRepo;
            _stationRepo = stationRepo;
            _centerRepo = centerRepo;
            _unitOfWork = unitOfWork;
            _scopeFactory = scopeFactory;
            _serialRepo = serialRepo;
            _parameterGroupRepo = parameterGroupRepo;
            _categoryRepository = categoryRepository;
            _workOrderRepo = workOrderRepo;
            _bomRepo = bomRepo;
            _processRepo = processRepo;
            _materialService = materialService;
            _factoryRepository = factoryRepository;
            _pickingLogRepo = pickingLogRepo;
            _kittingItemRepo = kittingItemRepo;
            _sapIntegrationService = sapIntegrationService;
            _sapRfcService = sapRfcService;
            _opConfirmService = confirmService;
            _opConsumpService = consumpService;
            _jyApiClient = jyApiClient;
            _apiSettings = apiSettings.Value;
            _activityLogRepo = activityLog;
            _finishedGoodsReceiptService = finishedGoodsReceiptService;
        }


        #region Kitting 业务逻辑 (齐套查询)

        /// <summary>
        /// 获取工单 Kitting (齐套/合箱) 状态
        /// 供 PDA 扫描订单号时调用
        /// </summary>
        public async Task<KittingResponse> GetKittingListAsync(string orderNoRaw)
        {
            if (string.IsNullOrWhiteSpace(orderNoRaw))
                throw new Exception("订单号不能为空");

            string orderNo = orderNoRaw.Split("-")[0];

            // 1. 获取工单及基础信息
            var workOrder = await _workOrderRepo.GetByOrderNoAsync(orderNo);
            if (workOrder == null) throw new Exception($"未查询到订单信息: {orderNo}");

            var processes = await _processRepo.GetListByOrderIdAync(workOrder.Id);
            var factory = await _factoryRepository.GetByIdAsync(workOrder.FactoryId);
            if (factory == null) throw new Exception("未查询到工厂信息");
            // 2. 获取 BOM (一次性过滤)
            var allBomItems = await _bomRepo.GetListByProcessIdsAsync(processes.Select(p => p.Id).ToList());
            var activeBomItems = allBomItems
                .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true)
                .Where(x => x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial)
                .ToList();

            if (!activeBomItems.Any()) throw new Exception("未查询到订单待合箱 BOM 信息");

            // 3. 批量获取所有相关数据 (并行优化)
            var materialCodes = activeBomItems.Select(x => x.MaterialCode).Distinct().ToList();

            // Task A: 获取物料属性
            var materialMap = (await _materialService.GetListByCodesAsync(factory.FactoryCode, materialCodes)).ToDictionary(x => x.MaterialCode);

            // Task B: 获取已拣配物料
            var stocks = await _pickingLogRepo.GetListByWorkOrderAsync(orderNo);
            // 或者根据您的业务规则，可能是通过 BatchCode 或其他字段关联

            // Task C: 获取断线任务 (Cable Task) - 通过 StepTaskId (ProcessId) 关联
            var processIds = activeBomItems.Select(x => x.WorkOrderProcessId).Distinct().ToList();
            //var tasks = await  _taskRepo.GetListByBomIdAsync(activeBomItems.Where(x => x.ConsumeType == (int)ConsumeType.CableMaterial).Select(x => x.Id).ToList(), TaskCategories.CableCut, TaskReasonCodes.BomRequirement);
            
            var tasks = await _taskRepo.GetCuttingViewByProcessIdAsync(activeBomItems.Select(x => x.WorkOrderProcessId).First());

            //await Task.WhenAll(materials, stockTask, tasksTask);

            //var materialMap = materials.Result.ToDictionary(x => x.MaterialCode);
            //var stocks = stockTask.Result;
            //var tasks = tasksTask.Result;

            // 4. 构建 BOM 需求视图
            var bomViewList = activeBomItems.Select(bom =>
            {
                var matName = materialMap.TryGetValue(bom.MaterialCode, out var mat) ? (string.IsNullOrWhiteSpace(mat.LabelName) ? "ROH原材料" : mat.LabelName) : mat.LabelName;
                return new KittingItemDto
                {
                    BomId = bom.Id,
                    ItemNo = bom.BomItem,
                    MaterialCode = bom.MaterialCode,
                    MaterialDesc = bom.MaterialDesc,
                    Quantity = (decimal)bom.RequiredQuantity,
                    ConsumeType = matName,
                    CompletedQuantity = 0 // 初始为0，后续计算
                };
            }).ToList();

            // 5. 执行分配计算
            foreach (var item in bomViewList)
            {
                if (!string.IsNullOrEmpty(item.ConsumeType)) 
                {
                    // === 分支 A: 断线任务 (从 Task 表取进度) ===
                    if (item.ConsumeType.Contains("断线") || item.ConsumeType.Contains("Cable"))
                    {
                        // 在内存中查找对应的任务
                        // 假设 MaterialTask 可以通过 MaterialCode 唯一确定，或者需要结合 ProcessId
                        var matchedTask = tasks.FirstOrDefault(t => t.RefSourceId == item.BomId);

                        if (matchedTask != null)
                        {
                            // 覆盖 BOM 需求量为任务量 (以实际排产为准)
                            item.Quantity = matchedTask.TargetQuantity ?? 0;
                            item.CompletedQuantity = matchedTask.CompletedQuantity ?? 0;
                        }
                    }
                    // === 分支 B: 原材料 (从线边库存/拣配记录取进度) ===
                    else
                    {
                        // 汇总该工单下该物料的所有库存量
                        var totalStockQty = stocks
                            .Where(s => s.MaterialCode == item.MaterialCode)
                            .Sum(s => s.Quantity);

                        // 计算逻辑：已拣配量不能超过需求量
                        item.CompletedQuantity = Math.Min(item.Quantity, (decimal)totalStockQty);
                    }
                }

            }

            // 6. 组装返回值
            //var processes = await _processRepo.GetListAsync(x => x.WorkOrderId == workOrder.Id);
            var firstProcess = processes.OrderBy(x => x.Operation).FirstOrDefault();

            return new KittingResponse
            {
                OrderId = workOrder.Id,
                OrderNo = workOrder.OrderNumber,
                OperationNo = firstProcess?.Operation,
                OperationStatus = firstProcess?.Status,
                LabelCount = (int)workOrder.LabelCount,

                // 统计
                CableItemCount = bomViewList.Count(x => x.ConsumeType.Contains("断线")),
                RawItemCount = bomViewList.Count(x => !x.ConsumeType.Contains("断线")),
                RawMtrBatchCount = stocks.Count, // 批次数量

                // 分组列表
                CableItems = bomViewList.Where(x => x.ConsumeType.Contains("断线")).ToList(),
                CenterStockItems = bomViewList.Where(x => x.ConsumeType == "ROH原材料").ToList(), // 需根据实际 LabelName 调整
                AutoStockItems = bomViewList.Where(x => x.ConsumeType == "自动仓物料").ToList()
            };
        }

        /// <summary>
        /// 确认 Kitting 完成
        /// 验证齐套情况，更新 Picking 和 Kitting 任务状态，触发 SAP 移库
        /// </summary>
        public async Task ConfirmKittingAsync(int workorderId,int? processId = null, string employeeId = "PDA")
        {
            if (workorderId <= 0 ) throw new Exception("订单号不能为空");

            // 1. 获取工单及基础信息
            var workOrder = await _workOrderRepo.GetByIdAsync(workorderId);
            //if (workOrder == null) throw new Exception($"未查询到订单信息: {orderNo}");

            var factory = await _factoryRepository.GetByIdAsync(workOrder.FactoryId);
            if (factory == null) throw new Exception("未查询到工厂信息");

            var processes = await _processRepo.GetListByOrderIdAync(workOrder.Id);
            if (processId != null) 
            {
                if(!processes.Any(x => x.Id == processId))
                    throw new Exception("未查询到订单工序信息");
                else
                    processes = processes.Where(x => x.Id == processId).ToList();
            } 
                
            // 2. 获取 BOM (一次性过滤)
            var allBomItems = await _bomRepo.GetListByProcessIdsAsync(processes.Select(p => p.Id).ToList());
            var activeBomItems = allBomItems
                .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true)
                .Where(x => x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial)
                .ToList();

            if (!activeBomItems.Any()) throw new Exception("未查询到订单待合箱 BOM 信息");

            var currentProcess = processes.Find(x => x.Id == activeBomItems.Select(x => x.WorkOrderProcessId).Distinct().First());


            //判断当前kitting任务是否完成
            var opTasks = await _opTaskRepo.GetListByProcessIdAsync(new List<int>() { currentProcess.Id });
            if (opTasks.Any()) 
            {
                var kittingTask = await _stepRepo.GetByOperationIdAsync(opTasks.Select(x => x.Id).First(), TaskCategories.Kitting);
                if (kittingTask.Status == WorkTaskStatus.Completed || kittingTask.CompletedQuantity == kittingTask.Quantity)
                    throw new Exception("当前订单已合箱，请勿重复合箱！");
            }
           

            // 3. 批量获取所有相关数据 (并行优化)
            // 3. 批量获取所有相关数据 (并行优化)
            var cableBomItems = activeBomItems.Where(x => x.ConsumeType == (int)ConsumeType.CableMaterial).ToList();
            var pickByOrderBomItems = activeBomItems.Where(x => x.ConsumeType == (int)ConsumeType.OrderBasedMaterial).ToList();
            
            // Task A: 获取已拣配物料
            var stocks = await _pickingLogRepo.GetListByWorkOrderAsync(workOrder.OrderNumber);

            // Task B: 获取断线任务
            var cableBomIds = cableBomItems.Select(x => x.Id).ToList();
            var tasks = await _taskRepo.GetListByBomIdAsync(cableBomIds, TaskCategories.CableCut, TaskReasonCodes.BomRequirement);
            var taskIds = tasks.Select(t => t.Id).ToList();

            // Task C: ★ 优化：批量获取断线任务消耗记录 (消除 N+1 查询)
            List<WorkOrderTaskExecuteConsump> allCableConsumptions = new List<WorkOrderTaskExecuteConsump>();
            //if (taskIds.Any())
            //{
            //    // 假设仓储层有 GetListAsync 或类似的泛型查询方法，这里模拟批量查询逻辑
            //    // 如果没有 GetListByTaskIdsAsync，请使用 generic repo 的 Where(x => taskIds.Contains(x.TaskId))
            //    allCableConsumptions = await _exeConsumpRepo.GetListByTaskIdAsync(TaskLevel.Material, taskIds);
            //}

            var kittingCreates = new List<WorkOrderKittingItem>();
            // 检查是否有未完成的项
            var uncompletedItems = new List<string>();

            foreach (var item in cableBomItems)
            {
                var task = tasks.First(t => t.RefSourceId == item.Id);
                if (task != null) 
                {
                    if (int.TryParse(task.Status, out int status) && status < int.Parse(WorkTaskStatus.Suspended))
                        uncompletedItems.Add($"{item.BomItem}[断线] {item.MaterialCode}缺料:  需求 【{task.TargetQuantity}】 PCS，已断线 【{task.CompletedQuantity??0}】 PCS");
                    else 
                    {
                        //验证断线任务完成后，将消耗数据添加进Kitting表中
                        var exeConsumeption = await _exeConsumpRepo.GetListByTaskIdAsync(TaskLevel.Material, task.Id);
                        if (exeConsumeption.Any())
                        {
                            kittingCreates.AddRange(exeConsumeption.GroupBy(x => new { x.MovementType, x.MovementReason, x.MaterialCode, x.BatchCode, x.BarCode })
                                .Select(g => new WorkOrderKittingItem()
                                {
                                    WorkOrderId = (int)currentProcess.WorkOrderId,
                                    WorkOrderNo = currentProcess.WorkOrderNo,
                                    WorkOrderProcessId = currentProcess.Id,
                                    Operation = currentProcess.Operation,
                                    BomItem = item.BomItem,
                                    ReservationItem = (int)item.ReservationItem,
                                    MaterialCode = item.MaterialCode,
                                    MaterialDesc = item.MaterialDesc,
                                    BaseUnit = item.Unit,
                                    BatchCode = g.Key.BatchCode,
                                    BarCode = g.Key.BarCode,
                                    Quantity = g.Sum(x => (decimal)x.ConsumedQuantity),
                                    ConsumptionType = g.Key.MovementType,
                                    MaterialConsumeType = item.ConsumeType,
                                    ConsumptionRemark = g.Key.MovementReason,
                                    CreatedBy = employeeId

                                }
                            ));

                        }
                    }
                }
                else
                    uncompletedItems.Add($"{item.BomItem}[断线] {item.MaterialCode}: 未查询到断线报工记录！");
            }

            // --- 4.2 处理拣配物料 (OrderBasedMaterial) ---

            // 按物料汇总 BOM 需求
            var pickRequirements = pickByOrderBomItems.GroupBy(x => x.MaterialCode)
                .ToDictionary(g => g.Key, g => new {
                    Required = g.Sum(x => x.RequiredQuantity),
                    Desc = g.First().MaterialDesc,
                    BomItems = g.ToList()
                });

            // 按物料汇总库存 (Queue 用于 FIFO 分配)
            var stockQueues = stocks.GroupBy(x => x.MaterialCode)
                .ToDictionary(g => g.Key, g => new Queue<V_WmsWorkOrderPickingLog>(g.OrderBy(s => s.WmsSourceId))); // 假设 PickingLog 有 CreatedOn

            // 汇总库存总数用于快速检查
            var stockSummary = stocks.GroupBy(p => p.MaterialCode)
               .ToDictionary(g => g.Key, g => g.Sum(item => item.Quantity));

            // ★ 新增逻辑：找出 WMS 中状态为 "60" (已关闭/短缺关闭) 的物料
            // 如果 WMS 认为该物料已经拣配结束(尽管没齐)，我们就不再阻断 MES 合箱
            var wmsClosedMaterials = stocks
                .Where(x => x.TaskStatus == WorkTaskStatus.Closed) // 根据你的实际类型，如果是整型请用 == 60
                .Select(x => x.MaterialCode)
                .ToHashSet();

            //拣配记录是合并拣配的，需要拆分到每个BOM行上，如果对应多个barcode，那么将增加多行
            foreach (var req in pickRequirements)
            {
                string matCode = req.Key;
                decimal neededTotal = (decimal)req.Value.Required;
                if (!stockSummary.TryGetValue(matCode, out decimal pickedQuantity))
                {
                    pickedQuantity = 0;
                }

                // ★ 判断该物料是否拥有 WMS 的 "免死金牌" (TaskStatus == 60)
                bool isWmsClosed = wmsClosedMaterials.Contains(matCode);

                // ★ 核心修改：如果没有强制关闭，且拣配数量不足，则报错阻断
                if (!isWmsClosed && neededTotal > pickedQuantity)
                {
                    uncompletedItems.Add($"[按单物料]{matCode}缺料: 需求 {neededTotal} PCS，已拣配 {pickedQuantity} PCS");
                }
                else
                {
                    // ★ 核心逻辑：将库存批次分配给 BOM Item (Kitting Item)
                    // 注意：由于是合并拣配，这里的逻辑是将实际库存转换为 KittingItem。
                    // 我们可以将库存记录按顺序分配给 BOM Item，或者简化为只生成 KittingItem 而不强绑定具体的 ReservationItem (视 SAP 接口要求而定)
                    // 这里采用：遍历 BOM Item，依次填入库存批次

                    if (!stockQueues.TryGetValue(matCode, out var stockQueue)) continue;

                    foreach (var bomItem in req.Value.BomItems)
                    {
                        decimal remainingBomQty = (decimal)bomItem.RequiredQuantity;

                        while (remainingBomQty > 0 && stockQueue.Count > 0)
                        {
                            var currentStock = stockQueue.Peek();

                            // 本次分配数量：取 Min(BOM剩余需求, 当前库存批次剩余量)
                            // 注意：需要克隆或跟踪库存批次的剩余量，这里简单起见假设 PickingLog 是只读的，我们在内存中计算剩余
                            // 由于 PickingLog 对象在 Dictionary 里是引用类型，我们可以直接修改内存中的 Quantity 作为临时计算
                            // 但为了不影响原始数据，建议不要直接修改，而是用局部变量。
                            // 简化逻辑：这里假设 PickingLog.Quantity 就是该批次的可用量。
                            // 如果一个库存批次要分给多个 BOM Item，我们需要追踪它的剩余量。

                            // 这里我们做一个简化的假设：KittingItem 记录的是“实际投料”，
                            // 对于 SAP 261 报文，重要的是 物料、批次、数量、预留行号。

                            decimal allocQty = Math.Min(remainingBomQty, (decimal)currentStock.Quantity);

                            kittingCreates.Add(new WorkOrderKittingItem()
                            {
                                WorkOrderId = (int)currentProcess.WorkOrderId,
                                WorkOrderNo = currentProcess.WorkOrderNo,
                                WorkOrderProcessId = currentProcess.Id,
                                Operation = currentProcess.Operation,
                                BomItem = bomItem.BomItem,
                                ReservationItem = (int)bomItem.ReservationItem,
                                MaterialCode = bomItem.MaterialCode,
                                MaterialDesc = bomItem.MaterialDesc,
                                BaseUnit = bomItem.Unit,
                                BatchCode = currentStock.BatchCode,
                                BarCode = currentStock.BarCode,
                                Quantity = allocQty,
                                ConsumptionType = ((int)ConsumptionType.Consumption).ToString(), // 投料
                                MaterialConsumeType = bomItem.ConsumeType,
                                CreatedBy = employeeId
                            });

                            remainingBomQty -= allocQty;
                            currentStock.Quantity -= allocQty; // 内存扣减

                            if (currentStock.Quantity <= 0)
                            {
                                stockQueue.Dequeue(); // 耗尽，移出队列
                            }
                        }
                    }
                }
            }

            if (uncompletedItems.Any())
            {
                throw new InvalidOperationException("齐套验证失败：\r\n" + string.Join("\r\n", uncompletedItems));
            }

            List<int> kittingIds = new List<int>();
            // 3. 事务更新状态
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // A. 保存 Kitting Items

                if (kittingCreates.Any())
                    kittingIds = await _kittingItemRepo.AddBulkAsync(kittingCreates);


                // B. 更新 Picking & Kitting 任务状态
                //var opTasks = await _opTaskRepo.GetListByProcessIdAsync(new List<int>() { currentProcess.Id});
                var opTaskIds = opTasks.Select(x => x.Id).ToList();
                var steps = await _stepRepo.GetListByOperationIdAsync(opTaskIds);

                var pickingSteps = steps.Where(x => x.TaskCategory == TaskCategories.Picking).ToList();
                var kittingSteps = steps.Where(x => x.TaskCategory == TaskCategories.Kitting).ToList();

                var now = DateTime.Now;

                // 更新 Picking 任务为完成 (如果尚未完成)
                foreach (var step in pickingSteps.Concat(kittingSteps))
                {
                    if (step.Status != WorkTaskStatus.Completed)
                    {
                        step.Status = WorkTaskStatus.Completed;
                        step.CompletedQuantity = step.Quantity; // 视为全额完成
                        step.UpdateBy = employeeId;
                        step.UpdatedOn = now;
                        if (step.ActualStartTime == null) step.ActualStartTime = now;
                        step.ActualEndTime = now;
                        await _stepRepo.UpdateAsync(step);
                    }
                }

                // 更新 Kitting 任务为完成
                foreach (var task in kittingSteps)
                {
                    // 仅更新未完成的
                    if (task.Status != WorkTaskStatus.Completed)
                    {
                        task.Status = WorkTaskStatus.Completed;
                        task.CompletedQuantity = task.Quantity;
                        task.UpdateBy = employeeId;
                        task.UpdatedOn = now;
                        // 记录实际时间
                        if (task.ActualStartTime == null) task.ActualStartTime = now;
                        task.ActualEndTime = now;

                        await _stepRepo.UpdateAsync(task);
                    }
                }


                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            // 4. 触发 SAP&WMS 移库 (非阻塞，或记录错误但不回滚)

            var kittings = await _kittingItemRepo.GetByIdsAsync(kittingIds);
            if (kittings.Any(x => x.MaterialConsumeType == (int)ConsumeType.OrderBasedMaterial && x.Quantity > 0)) 
            {
                var parameterGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_SapLocation);

                string fromLocation = parameterGroup?.Items.FirstOrDefault(x => x.Key == Key_SAPRawMtrStock)?.Value ?? "1100";
                var toLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == Key_CableProLineStock)?.Value ?? "2102";
                if (string.IsNullOrEmpty(workOrder.PlannerRemark) || !workOrder.PlannerRemark.Contains(toLocation))
                    toLocation = "2100";
                var transfers = kittings.Where(x => x.MaterialConsumeType == (int)ConsumeType.OrderBasedMaterial && x.Quantity > 0).GroupBy(x => (x.MaterialCode, x.BatchCode, x.BaseUnit))
                      .Select(g => new MaterialTransferLogCreateDto()
                      {
                          TransferNo = _serialRepo.GenerateNext($"{factory.FactoryCode}MaterialTransferToSAPSerial"),
                          TransferType = MaterialTransferType.GoodsIssue.GetDescription(),
                          PostingDate = DateTime.Today,
                          DocumentDate = DateTime.Today,
                          FactoryCode = factory.FactoryCode,
                          MaterialCode = g.Key.MaterialCode,
                          FromLocationCode = fromLocation,
                          BatchCode = g.Key.BatchCode,
                          WorkOrderNo = workOrder.OrderNumber,
                          WorkOrderId = workOrder.Id,
                          MovementType = ((int)ConsumptionType.MaterialTransfer).ToString(),
                          Quantity = g.Sum(x => x.Quantity),
                          BaseUnit = g.Key.BaseUnit,
                          //MovementReason = movementReason,
                          //Remark = remark,
                          ReceivingMaterialCode = g.Key.MaterialCode,
                          ReceivingBatchCode = g.Key.BatchCode,
                          ToFactoryCode = factory.FactoryCode,
                          ToLocationCode = toLocation,
                          CreatedBy = employeeId,
                      }).ToList();


                // ★ 核心修复：安全地将耗时的 SAP 移库放入后台独立线程 (Fire-and-Forget)
                // 这样可以立即返回给 UI，防止用户因等待过长而强退程序
                _ = Task.Run(async () =>
                {
                    try
                    {
                        // 必须通过 _scopeFactory 创建全新作用域，防止宿主上下文销毁
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var sapService = scope.ServiceProvider.GetRequiredService<ISapIntegrationService>();
                            await sapService.TransferToSapAsync(transfers, employeeId);
                        }
                    }
                    catch (Exception ex)
                    {
                        // 记录后台运行崩溃日志
                        Console.WriteLine($"[Background Task] SAP 移库发送失败: {ex.Message}");
                    }
                });

                // ★ 新增：触发 WMS 出库审核 (异步非阻塞)
                try
                {
                    await ApproveWmsPickTaskAsync(stocks, employeeId);
                }
                catch (Exception ex)
                {
                    // 仅记录日志，不阻断流程，因为本地业务已完成
                    // _logger.LogError(ex, "WMS出库审核失败");
                }
            }
        }


        public async Task ApproveWmsPickTaskAsync(List<string> wmsPickNos) 
        {
            List<V_WmsWorkOrderPickingLog> pickLogs = wmsPickNos.Select(x => new V_WmsWorkOrderPickingLog() 
            {
                WmsTaskCode = x
            }).ToList();
            await ApproveWmsPickTaskAsync(pickLogs, string.Empty);
        }

        /// <summary>
        /// 内部方法：调用 WMS 接口进行出库审核
        /// </summary>
        private async Task ApproveWmsPickTaskAsync(List<V_WmsWorkOrderPickingLog> pickLogs, string updateBy)
        {
            if (pickLogs == null || !pickLogs.Any()) return;

            // 筛选需要审核的单据：BomItem 为空 且 Remark 不为空
            var targetBillCodes = pickLogs.Select(x => x.WmsTaskCode)
                .Distinct()
                .ToList();

            if (!targetBillCodes.Any()) return;

            var requestUrl = _apiSettings["JyApi"].Endpoints["PickTaskApprove"];

            // 定义重试策略
            var retryPolicy = Policy
                .Handle<Exception>()
                .OrResult<WmsApprovedResponse>(r => r.Result != "S")
                .WaitAndRetryAsync(
                    2,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                );

            var logs = new List<ActivityLog>();

            foreach (var billCode in targetBillCodes)
            {
                try
                {
                    // 执行策略请求
                    var finalResult = await retryPolicy.ExecuteAndCaptureAsync(async () =>
                    {
                        var response = await _jyApiClient.WmsPostAsync<WmsApprovedResponse>(requestUrl,
                            JsonConvert.SerializeObject(
                            new
                            {
                                gid = Guid.NewGuid(),
                                billcode = billCode,
                                User = "admin", // 或使用 updateBy
                                plant = "CN11"
                            }));

                        // 确保返回对象不为空
                        if (response == null || !response.IsSuccess || response.Data == null)
                        {
                            return new WmsApprovedResponse { Result = "F", Message = response?.Message ?? "API 无响应" };
                        }

                        // 如果 ApiResponse 包裹了实际数据，这里需根据实际结构调整
                        // 假设 PostAsync 返回的是 ApiResponse<T>，这里需要取 Data
                        return response.Data;
                    });

                    // 记录失败日志
                    if (finalResult.Outcome == OutcomeType.Failure || finalResult.Result?.Result != "S")
                    {
                        string errorMsg = finalResult.FinalException?.Message ?? finalResult.Result?.Message ?? "未知错误";

                        logs.Add(new ActivityLog
                        {
                            LogType = "API_ERROR",
                            LogContent = "WMSPickTaskApprove",
                            Details = $"单据号: {billCode}，错误: {errorMsg}",
                            UserName = updateBy,
                            Timestamp = DateTime.Now
                        });
                    }
                }
                catch (Exception ex)
                {
                    // 兜底异常捕获
                    logs.Add(new ActivityLog
                    {
                        LogType = "API_CRASH",
                        LogContent = "WMSPickTaskApprove",
                        Details = $"单据号: {billCode}，异常: {ex.Message}",
                        UserName = updateBy,
                        Timestamp = DateTime.Now
                    });
                }

                await Task.Delay(1500);
            }

            // 批量保存日志
            if (logs.Any())
            {
                await _activityLogRepo.AddBulkAsync(logs);
            }
        }
        #endregion

        #region 0. 物料上料 (Material Loading - 库存转移)

        /// <summary>
        /// ★ 新增：执行上料动作
        /// 时机：操作员扫描物料条码，将其挂载到设备上时调用。
        /// 作用：将库存从 [线边库] 转移至 [机台]，实现扣减线边库存。
        /// </summary>
        public async Task LoadMaterialAsync(int factoryId, string barcode, int stationId, string employeeId, SupplyMode supplyMode = SupplyMode.Local)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<TaskExecutionService>();
                await scopedService.ExecuteLoadMaterialAsync(factoryId, barcode, stationId, employeeId, supplyMode);
            }
        }

        private async Task ExecuteLoadMaterialAsync(int factoryId, string barcode, int stationId, string employeeId, SupplyMode supplyMode)
        {
            // 1. 校验线边库存
            var inventory = await _inventoryRepo.GetByBarCodeAsync(factoryId, barcode);
            if (inventory == null)
                throw new Exception($"标签ID不存在：{barcode}");

            if (inventory.Status == LineInventoryStatus.Loaded) throw new Exception($"条码 [{barcode}] 已处于上料状态，请勿重复扫描。");

            if (inventory.SapStatus != "2")
                throw new Exception($"标签ID：[{barcode}] 未收货，无法上料。");

            if (inventory.Quantity <= 0)
                throw new Exception($"标签ID [{barcode}] 库存为 0，无法上料。");

            var parameterGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_SapLocation);
            var lineLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == Key_CableRawLineStock)?.Value ?? "2200";
            if (!(bool)inventory.LocationCode?.Contains(lineLocation))
                throw new Exception("当前标签ID未发料至断线线边，无法上料。");

            // 2. 获取工位信息
            var station = await _stationRepo.GetByIdAsync(stationId);
            if (station == null) throw new Exception("工位不存在");

            await _unitOfWork.BeginTransactionAsync();
            try
            {

                // 如果是独立供料(Local)，且机台已有同类物料正在使用，则自动卸载旧料
                if (supplyMode == SupplyMode.Local)
                {
                    var activeLoadings = await _loadingRepo.GetListByStationIdAsync(stationId, inventory.MaterialCode, MaterialLoadingStatus.Active);
                    if (activeLoadings.Any())
                    {
                        await InternalBatchUnloadLogicAsync(activeLoadings, employeeId, "Auto-Replace");
                    }
                }
                // 3. 创建机台上料记录 (增加机台库存)
                var loading = new StationMaterialLoading
                {
                    FactoryId = factoryId,
                    WorkStationId = stationId,
                    WorkStationCode = station.WorkStationCode,
                    LineSideInventoryId = inventory.Id,
                    MaterialCode = inventory.MaterialCode,
                    MaterialDesc = inventory.MaterialDesc,
                    BatchCode = inventory.BatchCode, // 继承批次
                    BarCode = barcode, // 如果Loading表有BarCode字段建议存入
                    Quantity = (decimal)inventory.LastQuantity,
                    LastQuantity = (decimal)inventory.LastQuantity, // 初始余量等于整盘数量
                    BaseUnit = inventory.BaseUnit,
                    Status = MaterialLoadingStatus.Active, // Active
                    SupplyMode = supplyMode,
                    LoadedBy = employeeId,
                    LoadedTime = DateTime.Now
                };
                await _loadingRepo.AddAsync(loading);

                // 4. 扣减线边库存 (减少/清空线边库存)
                // 逻辑：上料通常是整盘/整箱移动，所以直接扣为0或标记为已上机
                //inventory.Quantity = 0;
                inventory.Status = LineInventoryStatus.Loaded; // 假设有状态字段，标记为已上机
                inventory.UpdateBy = employeeId;
                inventory.UpdatedAt = DateTime.Now;

                await _inventoryRepo.UpdateAsync(inventory);

                // 如果业务要求保留线边记录但数量为0，用Update；如果要求物理移除，用Delete
                // await _inventoryRepo.DeleteAsync(inventory); 

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// 手动卸料/余料退回
        /// </summary>
        public async Task UnloadMaterialAsync(int loadingId, string employeeId, string reason = "Manual-Unload")
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<TaskExecutionService>();
                await scopedService.ExecuteUnloadMaterialAsync(loadingId, employeeId, reason);
            }
        }

        private async Task ExecuteUnloadMaterialAsync(int loadingId, string employeeId, string reason)
        {
            var loading = await _loadingRepo.GetByIdAsync(loadingId);
            if (loading == null) throw new Exception("上料记录不存在");
            if (loading.Status != MaterialLoadingStatus.Active) throw new Exception("该物料已卸载或耗尽，无需重复卸料");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await InternalBatchUnloadLogicAsync(new List<StationMaterialLoading>() { loading }, employeeId, reason);
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        private async Task ExecuteUnloadMaterialAsync(int loadingId, string employeeId)
        {
            var loading = await _loadingRepo.GetByIdAsync(loadingId);
            if (loading == null) throw new Exception("上料记录不存在");
            if (loading.Status != MaterialLoadingStatus.Active) throw new Exception("该物料已卸载或耗尽，无需重复卸料");

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                await InternalBatchUnloadLogicAsync(new List<StationMaterialLoading>() { loading }, employeeId, "Manual-Unload");
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// 内部通用卸料逻辑：更新上料表 -> 恢复线边库存
        /// ★ 优化：内部通用批量卸料逻辑
        /// </summary>
        private async Task InternalBatchUnloadLogicAsync(List<StationMaterialLoading> loadings, string employeeId, string reason)
        {
            if (loadings == null || !loadings.Any()) return;

            var now = DateTime.Now;
            var inventoryIds = new List<int>();

            // 1. 批量更新上料记录状态
            foreach (var loading in loadings)
            {
                loading.Status = MaterialLoadingStatus.Unloaded; // 3=Unloaded
                loading.UnloadedTime = now;
                loading.Remark = reason;
                if (loading.LineSideInventoryId > 0)
                {
                    inventoryIds.Add(loading.LineSideInventoryId);
                }
            }
            await _loadingRepo.UpdateBulkAsync(loadings);

            // 2. 批量更新线边库存状态 (一次性查询和更新)
            if (inventoryIds.Any())
            {
                var inventories = await _inventoryRepo.GetByIdsAsync(inventoryIds); // 假设仓储支持批量获取
                if (inventories != null && inventories.Any())
                {
                    foreach (var inventory in inventories)
                    {
                        inventory.Status =LineInventoryStatus.Available; // 标记为退回/可用
                        inventory.UpdateBy = employeeId;
                        inventory.UpdatedAt = now;
                    }
                    await _inventoryRepo.UpdateBulkAsync(inventories);
                }
            }
        }

        #endregion


        #region 1. 开工前校验与认领 (Claim & Start)

        /// <summary>
        /// 防错校验：验证扫描的物料标签是否是当前任务所需的物料
        /// 场景：操作员在手持终端选中任务后，扫描手中的物料标签，系统判断是否匹配
        /// </summary>
        public async Task VerifyScannedMaterialMatchAsync(int factoryId ,string barcode, List<int> taskIds)
        {
            // 1. 解析条码获取物料信息
            var inventoryItem = await _inventoryRepo.GetByBarCodeAsync(factoryId, barcode);
            if (inventoryItem == null)
                throw new Exception($"无效标签ID：[{barcode}] 在线边库中不存在。");

            var parameterGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_SapLocation);

            var LineLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == Key_CableRawLineStock)?.Value ?? "2200";
            if (!(bool)inventoryItem.LocationCode?.Contains(LineLocation))
                throw new Exception("当前标签ID未发料至断线线边，无法上料！");

            string scannedMaterialCode = inventoryItem.MaterialCode;

            var currentTasks = await _taskRepo.GetByIdsAsync(taskIds);
            // 2. 获取选中任务的需求物料清单
            var tasks = await _taskRepo.GetListByStepTaskIdAsync(currentTasks.Select(x => (int)x.StepTaskId).ToList(), TaskCategories.CableCut);
            if (!tasks.Any()) throw new Exception("未选择任何任务");

            // 筛选出需要上料的任务类型 (切线、投料)
            var requiredMaterials = tasks
                .Where(t => t.TaskSubType == TaskCategories.CableCut || t.TaskSubType == TaskCategories.Feeding)
                .Select(t => t.MaterialCode)
                .ToHashSet();

            // 3. 执行比对
            if (!requiredMaterials.Contains(scannedMaterialCode))
            {
                throw new Exception($"扫描出错：[{barcode}]对应的物料[{scannedMaterialCode}] 不在上料范围中，请检查bom信息！");
            }
        }

        /// <summary>
        /// 核心校验方法：判断机台物料是否齐备、数量是否充足
        /// </summary>
        public async Task ValidateMaterialReadinessAsync(List<int> taskIds, int stationId)
        {
            // 1. 获取选中的所有任务
            var tasks = await _taskRepo.GetByIdsAsync(taskIds);
            if (!tasks.Any()) return;

            // 2. 获取当前机台所有“使用中”的上料记录
            var loadedMaterials = await _loadingRepo.GetListByStationIdAsync(stationId);
            var stationInventory = loadedMaterials
                .GroupBy(x => x.MaterialCode)
                .ToDictionary(g => g.Key, g => g.Sum(l => l.LastQuantity));

            // 3. 计算任务的总需求
            var requirements = new Dictionary<string, decimal>();

            foreach (var task in tasks)
            {
                bool isMaterialTask = task.TaskSubType == TaskCategories.CableCut || task.TaskSubType == TaskCategories.Feeding;
                if (!isMaterialTask) continue;

                decimal neededQty = 0;
                if (task.TaskSubType == TaskCategories.CableCut)
                {
                    // 需求量 = 根数 * 单根长度
                    neededQty = (task.TargetQuantity ?? 0) * (task.TargetValue ?? 0);
                }
                else if (task.TaskSubType == TaskCategories.Feeding)
                {
                    neededQty = task.TargetQuantity ?? 0;
                }

                if (requirements.ContainsKey(task.MaterialCode))
                    requirements[task.MaterialCode] += neededQty;
                else
                    requirements.Add(task.MaterialCode, neededQty);
            }

            // 4. 执行比对
            var errorMessages = new List<string>();
            foreach (var req in requirements)
            {
                string matCode = req.Key;
                decimal qtyNeeded = req.Value;

                if (!stationInventory.ContainsKey(matCode))
                {
                    errorMessages.Add($"缺料：当前任务需要 [{matCode}]，但机台未扫描上料。");
                }
                else if (stationInventory[matCode] < qtyNeeded)
                {
                    // 数量不足通常作为警告，视业务规则是否阻断
                    // errorMessages.Add($"量不足：[{matCode}] 需求 {qtyNeeded}，机台仅剩 {stationInventory[matCode]}。");
                }
            }

            if (errorMessages.Any())
                throw new Exception("开工校验失败：\n" + string.Join("\n", errorMessages));
        }

        /// <summary>
        /// 认领并开始任务 (拉动模式核心逻辑)
        /// </summary>
        public async Task ClaimAndStartTasksAsync(List<int> taskIds, int stationId, string employeeId)
        {

            // 1. 获取任务
            var tasks = await _taskRepo.GetByIdsAsync(taskIds);

            // 2. 并发控制
            if (tasks.Any(x => x.Status == WorkTaskStatus.InProgress && x.ActualWorkStationId != stationId))
            {
                throw new Exception("当前任务正在其他工位执行，请刷新列表！");
            }

            // 3. 补全 WorkCenter 信息 (用于报表统计)
            var station = await _stationRepo.GetByIdAsync(stationId);
            var center = await _centerRepo.GetByIdAsync(station.WorkCenterId);
            // 4. 绑定与状态更新
            foreach (var task in tasks)
            {

                task.ActualWorkStationId = stationId;
                task.ActualWorkStationCode = station.WorkStationCode;
                task.ActualWorkCenterId = center.Id;
                task.ActualWorkCenterCode = center.WorkCenterCode;

                task.Status = WorkTaskStatus.InProgress;
                task.ActualStartTime = DateTime.Now;
                task.UpdateBy = employeeId;
                task.UpdatedOn = DateTime.Now;
            }

            await _taskRepo.UpdateBulkAsync(tasks);
        }

        public async Task ReleaseTaskAsync(int taskId, string employeeId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null || task.Status != WorkTaskStatus.InProgress)
                throw new Exception("任务不存在或状态不在执行中！");

            task.ActualWorkStationId = null;
            task.ActualWorkStationCode = null;
            task.Status = WorkTaskStatus.Released;
            task.UpdateBy = employeeId;
            task.UpdatedOn = DateTime.Now;

            await _taskRepo.UpdateAsync(task);
        }

        public async Task SuspendTaskAsync(int taskId, string employeeId)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null )
                throw new Exception("任务不存在！");

            task.ActualWorkStationId = null;
            task.ActualWorkStationCode = null;
            task.Status = WorkTaskStatus.Suspended;
            task.UpdateBy = employeeId;
            task.UpdatedOn = DateTime.Now;

            await _taskRepo.UpdateAsync(task);
        }


        public async Task<bool> VerifyCuttingStepTaskFirstConfirmAsync(int materialTaskId) 
        {
            var materialTask = await _taskRepo.GetByIdAsync(materialTaskId);
            var exeLogs = await _exeLogRepo.GetListByStepTaskIdAsync((int)materialTask.StepTaskId);
            return exeLogs.Count() == 0;

        }

        #endregion


        #region 2. 报工与消耗 (Report & Consumption)

        // 场景 A：断线报工 (MaterialTask)
        public async Task ReportMaterialTaskProductionAsync(int taskId, decimal outputQty, decimal scrapQty,
 int stationId, string stationCode, string employeeId , string? remark = null, string? movementReason = null, DateTime? startTime = null,DateTime? endTime = null)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null) throw new Exception("物料任务不存在");

            var stepTask = await _stepRepo.GetByIdAsync((int)task.StepTaskId);
            if (stepTask == null) throw new Exception("工步任务不存在");

            var kittingTask = await _stepRepo.GetByOperationIdAsync((int)stepTask.OperationTaskId, TaskCategories.Kitting);
            if (kittingTask == null) throw new Exception("合箱任务不存在");
            if (int.TryParse(kittingTask.Status, out int status) && status >= int.Parse(WorkTaskStatus.Completed))
                throw new Exception("合箱任务已完成，无法继续报工");

            // 1. 计算需求 (正常消耗 + 报废消耗)
            var productionReqs = new Dictionary<string, decimal>();
            var scrapReqs = new Dictionary<string, decimal>();
            var totalReqs = new Dictionary<string, decimal>();
            if (task.TaskSubType == TaskCategories.CableCut)
            {
                //将mm转换成m
                decimal prodLen = outputQty * (task.TargetValue ?? 0) / 1000;
                if (prodLen > 0) productionReqs.Add(task.MaterialCode, prodLen);

                // 报废消耗 = 直接传入的长度 (如 250mm)
                scrapQty /= 1000;
                if (scrapQty > 0) scrapReqs.Add(task.MaterialCode, scrapQty);

                // 合并总需求用于预校验
                decimal totalLen = prodLen + scrapQty;
                if (totalLen > 0) totalReqs.Add(task.MaterialCode, totalLen);
            }

            // 2. ★ 预校验 (Pre-Check) ★
            await PreCheckMaterialAvailabilityAsync(stationId, totalReqs);

            int executeLogId = 0;
            // 3. 执行事务
            await _unitOfWork.BeginTransactionAsync();

            try
            {

                // ★ 核心修复 1：提取绝对唯一且安全的布尔标识 (算一次，管全局)
                decimal targetQty = task.TargetQuantity ?? 0m;
                decimal completedQty = (task.CompletedQuantity ?? 0m) + outputQty;
                bool isFinished = targetQty > 0 && completedQty >= targetQty;
                // A. 记录报工主日志
                var executeLog = await CreateExecuteLogAsync(TaskLevel.Material, task.Id, outputQty, stationId, stationCode, employeeId, remark, isFinished, startTime, endTime);


                // B.1 执行正常扣料 (261)
                if (productionReqs.Count() > 0)
                {
                    await DeductBatchMaterialAndRecordAsync(stationId, productionReqs, executeLog.Id, employeeId, MOVEMENT_PROD, string.Empty);
                }

                // B.2 执行报废扣料 (903)
                if (scrapReqs.Count() > 0)
                {
                    // 报废原因可以硬编码或由前端传入，这里暂用 Scrap Sample
                    await DeductBatchMaterialAndRecordAsync(stationId, scrapReqs, executeLog.Id, employeeId, MOVEMENT_SCRAP, movementReason);
                }

                // C. 更新进度
                task.CompletedQuantity = completedQty;
                if (isFinished)
                {
                    task.Status = WorkTaskStatus.Completed;
                    task.ActualEndTime = endTime ?? DateTime.Now;
                }
                task.UpdateBy = employeeId;
                task.UpdatedOn = DateTime.Now;
                await _taskRepo.UpdateAsync(task);

                await _unitOfWork.CommitAsync();

                executeLogId = executeLog.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            var bom = await _bomRepo.GetByIdAsync((int)task.RefSourceId);
            var workOrder = await _workOrderRepo.GetByIdAsync(bom.WorkOrderId);
            var factory = await _factoryRepository.GetByIdAsync(workOrder.FactoryId);

            var taskConsumps = await _exeConsumpRepo.GetListByExelogIdAsync(executeLogId);
            if (taskConsumps.Any())
            {
                var parameterGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_SapLocation);

                var fromLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == Key_CableRawLineStock)?.Value ?? "2200";
                var toLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == Key_CableProLineStock)?.Value ?? "2102";
                if (string.IsNullOrEmpty(workOrder.PlannerRemark) || !workOrder.PlannerRemark.Contains(toLocation))
                    toLocation = "2100";
                var transfers = taskConsumps.GroupBy(x => (x.MaterialCode, x.BatchCode, x.BaseUnit))
                      .Select(g => new MaterialTransferLogCreateDto()
                      {
                          TransferNo = _serialRepo.GenerateNext($"{factory.FactoryCode}MaterialTransferToSAPSerial"),
                          TransferType = MaterialTransferType.GoodsIssue.GetDescription(),
                          PostingDate = DateTime.Today,
                          DocumentDate = DateTime.Today,
                          FactoryCode = factory.FactoryCode,
                          MaterialCode = g.Key.MaterialCode,
                          FromLocationCode = fromLocation,
                          BatchCode = g.Key.BatchCode,
                          WorkOrderNo = workOrder.OrderNumber,
                          WorkOrderId = workOrder.Id,
                          MovementType = ((int)ConsumptionType.MaterialTransfer).ToString(),
                          Quantity = g.Sum(x => x.ConsumedQuantity),
                          BaseUnit = g.Key.BaseUnit,
                          //MovementReason = movementReason,
                          //Remark = remark,
                          ReceivingMaterialCode = g.Key.MaterialCode,
                          ReceivingBatchCode = g.Key.BatchCode,
                          ToFactoryCode = factory.FactoryCode,
                          ToLocationCode = toLocation,
                          CreatedBy = employeeId,
                      }).ToList();

                if (transfers.Any())
                {
                    // ★ 核心修复：安全地将耗时的 SAP 移库放入后台独立线程 (Fire-and-Forget)
                    // 这样可以立即返回给 UI，防止用户因等待过长而强退程序
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            // 必须通过 _scopeFactory 创建全新作用域，防止宿主上下文销毁
                            using (var scope = _scopeFactory.CreateScope())
                            {
                                var sapService = scope.ServiceProvider.GetRequiredService<ISapIntegrationService>();
                                await sapService.TransferToSapAsync(transfers, employeeId);
                            }
                        }
                        catch (Exception ex)
                        {
                            // 记录后台运行崩溃日志
                            Console.WriteLine($"[Background Task] SAP 移库发送失败: {ex.Message}");
                        }
                    });
                }
            }
        }

        // ★ 新增：私有方法，包含原有的核心业务逻辑
        // 因为是在新 Scope 的实例上调用的，这里的 _unitOfWork 是全新的
        private async Task ExecuteReportMaterialTaskProductionAsync(int taskId, decimal outputQty, decimal scrapQty, int stationId, string stationCode, string employeeId,string remark,string? movementReason,DateTime? startTime,DateTime? endTime)
        {
            var task = await _taskRepo.GetByIdAsync(taskId);
            if (task == null) throw new Exception("物料任务不存在");

            var stepTask = await _stepRepo.GetByIdAsync((int)task.StepTaskId);
            if (stepTask == null) throw new Exception("工步任务不存在");

            var kittingTask = await _stepRepo.GetByOperationIdAsync((int)stepTask.OperationTaskId, TaskCategories.Kitting);
            if (kittingTask == null) throw new Exception("合箱任务不存在");
            if (int.TryParse(kittingTask.Status, out int status) && status >= int.Parse(WorkTaskStatus.Completed))
                throw new Exception("合箱任务已完成，无法继续报工");

            // 1. 计算需求 (正常消耗 + 报废消耗)
            var productionReqs = new Dictionary<string, decimal>();
            var scrapReqs = new Dictionary<string, decimal>();
            var totalReqs = new Dictionary<string, decimal>();
            if (task.TaskSubType == TaskCategories.CableCut)
            {
                //将mm转换成m
                decimal prodLen = outputQty * (task.TargetValue ?? 0) / 1000;
                if (prodLen > 0) productionReqs.Add(task.MaterialCode, prodLen);

                // 报废消耗 = 直接传入的长度 (如 250mm)
                scrapQty /= 1000;
                if (scrapQty > 0) scrapReqs.Add(task.MaterialCode, scrapQty);

                // 合并总需求用于预校验
                decimal totalLen = prodLen + scrapQty;
                if (totalLen > 0) totalReqs.Add(task.MaterialCode, totalLen);
            }

            // 2. ★ 预校验 (Pre-Check) ★
            await PreCheckMaterialAvailabilityAsync(stationId, totalReqs);

            int executeLogId = 0;
            // 3. 执行事务
            await _unitOfWork.BeginTransactionAsync();

            try
            {

                // ★ 核心修复 1：提取绝对唯一且安全的布尔标识 (算一次，管全局)
                decimal targetQty = task.TargetQuantity ?? 0m;
                decimal completedQty = (task.CompletedQuantity ?? 0m) + outputQty;
                bool isFinished = targetQty > 0 && completedQty >= targetQty;
                // A. 记录报工主日志
                var executeLog = await CreateExecuteLogAsync(TaskLevel.Material, task.Id, outputQty, stationId, stationCode, employeeId, remark, isFinished, startTime, endTime);


                // B.1 执行正常扣料 (261)
                if (productionReqs.Count() > 0)
                {
                    await DeductBatchMaterialAndRecordAsync(stationId, productionReqs, executeLog.Id, employeeId, MOVEMENT_PROD, string.Empty);
                }

                // B.2 执行报废扣料 (903)
                if (scrapReqs.Count() > 0)
                {
                    // 报废原因可以硬编码或由前端传入，这里暂用 Scrap Sample
                    await DeductBatchMaterialAndRecordAsync(stationId, scrapReqs, executeLog.Id, employeeId, MOVEMENT_SCRAP, movementReason);
                }

                // C. 更新进度
                task.CompletedQuantity = completedQty;
                if (isFinished) 
                {
                    task.Status = WorkTaskStatus.Completed;
                    task.ActualEndTime = endTime ?? DateTime.Now;
                }
                task.UpdateBy = employeeId;
                task.UpdatedOn = DateTime.Now;
                await _taskRepo.UpdateAsync(task);

                await _unitOfWork.CommitAsync();

                executeLogId = executeLog.Id;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            var bom =await _bomRepo.GetByIdAsync((int)task.RefSourceId);
            var workOrder = await _workOrderRepo.GetByIdAsync(bom.WorkOrderId);
            var factory = await _factoryRepository.GetByIdAsync(workOrder.FactoryId);

            var taskConsumps = await _exeConsumpRepo.GetListByExelogIdAsync(executeLogId);
            if (taskConsumps.Any()) 
            {
                var parameterGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_SapLocation);

                var fromLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == Key_CableRawLineStock)?.Value ?? "2200";
                var toLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == Key_CableProLineStock)?.Value ?? "2102";
                if (string.IsNullOrEmpty(workOrder.PlannerRemark) || !workOrder.PlannerRemark.Contains(toLocation))
                    toLocation = "2100";
                var transfers = taskConsumps.GroupBy(x => (x.MaterialCode, x.BatchCode, x.BaseUnit))
                      .Select(g => new MaterialTransferLogCreateDto()
                      {
                          TransferNo = _serialRepo.GenerateNext($"{factory.FactoryCode}MaterialTransferToSAPSerial"),
                          TransferType = MaterialTransferType.GoodsIssue.GetDescription(),
                          PostingDate = DateTime.Today,
                          DocumentDate = DateTime.Today,
                          FactoryCode = factory.FactoryCode,
                          MaterialCode = g.Key.MaterialCode,
                          FromLocationCode = fromLocation,
                          BatchCode = g.Key.BatchCode,
                          WorkOrderNo = workOrder.OrderNumber,
                          WorkOrderId = workOrder.Id,
                          MovementType = ((int)ConsumptionType.MaterialTransfer).ToString(),
                          Quantity = g.Sum(x => x.ConsumedQuantity),
                          BaseUnit = g.Key.BaseUnit,
                          //MovementReason = movementReason,
                          //Remark = remark,
                          ReceivingMaterialCode = g.Key.MaterialCode,
                          ReceivingBatchCode = g.Key.BatchCode,
                          ToFactoryCode = factory.FactoryCode,
                          ToLocationCode = toLocation,
                          CreatedBy = employeeId,
                      }).ToList();

                if (transfers.Any())
                {
                    // ★ 核心修复：安全地将耗时的 SAP 移库放入后台独立线程 (Fire-and-Forget)
                    // 这样可以立即返回给 UI，防止用户因等待过长而强退程序
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            // 必须通过 _scopeFactory 创建全新作用域，防止宿主上下文销毁
                            using (var scope = _scopeFactory.CreateScope())
                            {
                                var sapService = scope.ServiceProvider.GetRequiredService<ISapIntegrationService>();
                                await sapService.TransferToSapAsync(transfers, employeeId);
                            }
                        }
                        catch (Exception ex)
                        {
                            // 记录后台运行崩溃日志
                            Console.WriteLine($"[Background Task] SAP 移库发送失败: {ex.Message}");
                        }
                    });
                }
            }
        }


        public async Task ReportStepProductionAsync(int stepId, decimal outputQty, int stationId, string stationCode, string employeeId, string? remark = null, string movementType = "261", string movementReason = null, DateTime? startTime = null, DateTime? endTime = null)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<TaskExecutionService>();
                await scopedService.ExecuteReportStepProductionAsync(stepId, outputQty, stationId, stationCode, employeeId, remark, movementType, movementReason, startTime, endTime);
            }
        }

        public async Task ReportStepProductionAsync(int stepId, decimal outputQty, string employeeId, string? remark = null, DateTime? startTime = null, DateTime? endTime = null)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<TaskExecutionService>();
                await scopedService.ExecuteReportStepProductionAsync(stepId, outputQty, employeeId, remark, startTime, endTime);
            }
        }

        private async Task ExecuteReportStepProductionAsync(int stepId, decimal outputQty, string employeeId, string? remark, DateTime? startTime, DateTime? endTime)
        {
            var step = await _stepRepo.GetByIdAsync(stepId);
            if (step == null) throw new Exception("工步任务不存在");
            if ((step.CompletedQuantity ?? 0) + outputQty > step.Quantity)
                throw new Exception("报工数量已超出任务数量");
            int exeLogId = 0;
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var executeLog = await CreateExecuteLogAsync(TaskLevel.Step, step.Id, outputQty, null, null, employeeId, remark, step.CompletedQuantity + outputQty >= step.Quantity, startTime, endTime);
                exeLogId = executeLog.Id;
                // 更新任务进度 (此处假设 StepTask 有 CompletedQuantity)
                step.CompletedQuantity = (step.CompletedQuantity ?? 0) + outputQty;
                if (step.CompletedQuantity >= step.Quantity) step.Status = WorkTaskStatus.Completed;
                step.UpdatedOn = DateTime.Now;
                await _stepRepo.UpdateAsync(step);

                // 向上汇总工序进度
                await RollupOperationProgressAsync(step, outputQty, employeeId, endTime);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
            if (exeLogId > 0) 
            {
                await DispatchSapIntegrationForStepTaskAsync(step, exeLogId, outputQty, employeeId);
            }
        }

        /// <summary>
        /// ★ 批量工步报工入口 (支持 List<int> stepIds)
        /// </summary>
        public async Task ReportStepProductionAsync(List<int> stepIds, decimal outputQty, string employeeId, string? remark = null, DateTime? startTime = null, DateTime? endTime = null)
        {
            if (stepIds == null || !stepIds.Any()) return;

            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<TaskExecutionService>();
                await scopedService.ExecuteReportStepProductionBatchAsync(stepIds, outputQty, employeeId, remark, startTime, endTime);
            }
        }

        /// <summary>
        /// ★ 批量工步报工核心逻辑
        /// 确保所有工步更新在同一个事务中完成
        /// </summary>
        private async Task ExecuteReportStepProductionBatchAsync(List<int> stepIds, decimal outputQty, string employeeId, string? remark, DateTime? startTime, DateTime? endTime)
        {
            // 1. 获取所有涉及的工步任务
            // 这里假设 _stepRepo.GetListAsync 支持表达式树查询。如果是仓储特定方法如 GetByIdsAsync，请替换。
            var steps = await _stepRepo.GetSortListByIdsAsync(stepIds);

            if (steps == null || steps.Count != stepIds.Count)
                throw new Exception("部分工步任务不存在，请刷新页面后重试");

            if (steps.Select(x => x.OperationTaskId).Distinct().Count() > 1)
                throw new Exception("当前工步不属于同一工序任务，无法继续报工");

            var taskCategories = await _categoryRepository.GetByCategoryCodeAsync(steps.Select(x => x.TaskCategory).ToList());
            if (taskCategories == null || taskCategories.Count() == 0)
                throw new Exception("未查询到工步分类信息，请刷新页面后重试");
            if (taskCategories.Select(x => x.StepType).Distinct().Count() != 1)
                throw new Exception("不同类别的工步无法同时报工，请重新选择工步");

            


            //var preTriggerStep = new WorkOrderStepTask();
            // 预校验所有任务的数量
            foreach (var step in steps)
            {

                if ((step.CompletedQuantity ?? 0) + outputQty > step.Quantity)
                    throw new Exception($"工步 [{step.StepCode}] 报工数量已超出任务数量");
            }
            //
            if (taskCategories.Select(x => x.StepType).First() == TASKCATEGORY_PROCESS) 
            {

                var processCategories = await _categoryRepository.GetListByStepTypeAsync(TASKCATEGORY_PROCESS);
                var preTriggerStep = processCategories.Where(x => x.IsSapPrevTrigger).FirstOrDefault();
                if (preTriggerStep != null) 
                {
                    var preStepTask = await _stepRepo.GetByOperationIdAsync(steps.Select(x => (int)x.OperationTaskId).First(), preTriggerStep.CategoryCode);

                    if (preStepTask == null || preStepTask.Id <= 0)
                        throw new Exception("请先选择材料准备工步进行开工");
                    if (steps.Any(x => x.CompletedQuantity > preStepTask.CompletedQuantity))
                        throw new Exception("部分工步进度已超过材料准备工步进度，请先报工材料准备工步");
                }

            }


            var executeLogs = new List<WorkOrderTaskExecuteLog>();
            var now = DateTime.Now;
            var actualEndTime = endTime ?? now;
            var actualStartTime = startTime ?? actualEndTime;

            // 批量操作通常共享一个批次号
            string batchCode = _serialRepo.GenerateNext(REPORT_SEQUENCE_CODE);

            // 记录生成的日志和对应的工步，用于后续分发 SAP
            var sapDispatchQueue = new List<(WorkOrderStepTask Step, int ExeLogId)>();

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 2. 内存中更新数据并构建批量插入的日志
                foreach (var step in steps)
                {
                    bool isFinished = (step.CompletedQuantity ?? 0) + outputQty >= step.Quantity;

                    // A. 构建日志对象
                    var log = new WorkOrderTaskExecuteLog
                    {
                        TaskLevel = TaskLevel.Step,
                        TaskId = step.Id,
                        BatchCode = batchCode,
                        CompletedQuantity = outputQty,
                        WorkStationId = null, // 或根据业务需要传入
                        WorkStationCode = null,
                        EmployerCode = employeeId,
                        StartTime = actualStartTime,
                        EndTime = actualEndTime,
                        Status = isFinished ? WorkTaskStatus.Completed : WorkTaskStatus.InProgress,
                        Remark = remark,
                        CreatedBy = employeeId,
                        CreatedOn = now
                    };
                    executeLogs.Add(log);

                    // B. 更新工步实体
                    step.CompletedQuantity = (step.CompletedQuantity ?? 0) + outputQty;
                    if (isFinished) step.Status = WorkTaskStatus.Completed;
                    step.UpdatedOn = now;
                    step.UpdateBy = employeeId;

                    // C. 向上汇总工序进度 (内部已包含 IsMilestone 判断)
                    await RollupOperationProgressAsync(step, outputQty, employeeId, actualEndTime);
                }

                // 3. 批量执行数据库操作
                if (executeLogs.Any())
                {
                    // 假设仓储层有 AddBulkAsync 或 InsertRangeAsync，并且插入后能保留 Id
                    // 如果框架不支持在 Bulk 插入后回填 Id，这里可能需要退回到 foreach 单独 AddAsync
                    // 或者先生成，然后再查询出来。这里采用最稳妥的逐条 Add 以获取 ExeLogId
                    foreach (var log in executeLogs)
                    {
                        var savedLog = await _exeLogRepo.AddAsync(log);

                        // 找到这个日志对应的 Step
                        var matchedStep = steps.First(s => s.Id == log.TaskId);
                        sapDispatchQueue.Add((matchedStep, savedLog.Id));
                    }
                }

                if (steps.Any())
                {
                    await _stepRepo.UpdateBulkAsync(steps);
                }

                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            // 4. 事务提交成功后，循环分发 SAP 报工逻辑
            // DispatchSapIntegrationForStepTaskAsync 内部有 IsMilestone/SAP 触发器判定
            foreach (var dispatchItem in sapDispatchQueue)
            {
                if (dispatchItem.ExeLogId > 0)
                {
                    await DispatchSapIntegrationForStepTaskAsync(dispatchItem.Step, dispatchItem.ExeLogId, outputQty, employeeId);
                }
            }
        }

        private async Task ExecuteReportStepProductionAsync(int stepId,decimal outputQty,int stationId,string stationCode,string employeeId, string remark,string movementType,string movementReason,DateTime? startTime,DateTime? endTime)
        {
            var step = await _stepRepo.GetByIdAsync(stepId);
            if (step == null) throw new Exception("工步任务不存在");

            var feedingTasks = await _taskRepo.GetListByStepTaskIdAsync(stepId, TaskCategories.Feeding);
            if (!feedingTasks.Any()) throw new Exception("未配置投料任务");

            decimal ratio = (step.Quantity == 0) ? 0 : outputQty / ((decimal)step.Quantity);
            var requirements = new Dictionary<string, decimal>();

            foreach (var feedTask in feedingTasks)
            {
                decimal deductQty = (feedTask.TargetQuantity ?? 0) * ratio;
                if (deductQty > 0)
                {
                    if (requirements.ContainsKey(feedTask.MaterialCode))
                        requirements[feedTask.MaterialCode] += deductQty;
                    else
                        requirements.Add(feedTask.MaterialCode, deductQty);
                }
            }

            await PreCheckMaterialAvailabilityAsync(stationId, requirements);
            int exeLogId = 0;
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                var executeLog = await CreateExecuteLogAsync(TaskLevel.Step, step.Id, outputQty, stationId, stationCode, employeeId,  remark, step.CompletedQuantity + outputQty >= step.Quantity, startTime, endTime);
                exeLogId = executeLog.Id;
                foreach (var req in requirements)
                {
                    await DeductMaterialAndRecordAsync(stationId, req.Key, req.Value, executeLog.Id, employeeId, movementType, movementReason);
                }

                // 更新任务进度 (此处假设 StepTask 有 CompletedQuantity)
                step.CompletedQuantity = (step.CompletedQuantity ?? 0) + outputQty;
                if (step.CompletedQuantity >= step.Quantity) step.Status = WorkTaskStatus.Completed;
                step.UpdatedOn = DateTime.Now;
                await _stepRepo.UpdateAsync(step);
                // 向上汇总工序进度
                await RollupOperationProgressAsync(step, outputQty, employeeId, endTime);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

            if (exeLogId > 0)
            {
                await DispatchSapIntegrationForStepTaskAsync(step, exeLogId, outputQty, employeeId);
            }
        }


        public async Task ReEntryPushSapConfirmAsync(int sapConfirmId) 
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<TaskExecutionService>();
                await scopedService.CreateConsumptionRecordsAsync(sapConfirmId);
            }
        }

        public async Task BatchReEntryPushSapConfirmAsync(List<int> sapConfirmIds)
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedService = scope.ServiceProvider.GetRequiredService<TaskExecutionService>();
                await scopedService.CreateConsumptionRecordsBatchAsync(sapConfirmIds);
            }
        }

        //成品入库
        public async Task<FinishedGoodsReceiptDto> FinishedGoodsReceiptToSapAsync(FinishedGoodsReceiptDto dto)
        {
            // 4. 正式向 SAP 发送报工指令 (支持针对返回值的自动重试)
            if (await CheckSapEnabledAsync())
            {
                // 定义重试策略：捕获因锁定引发的主动抛出异常
                var sapRetryPolicy = Policy
                    .Handle<Exception>(ex =>
                        ex.Message.ToLower().Contains("already being processed") ||
                        ex.Message.Contains("锁定") ||
                        ex.Message.ToLower().Contains("lock"))
                    .WaitAndRetryAsync(
                        3, // 重试 3 次
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                    );

                await sapRetryPolicy.ExecuteAsync(async () =>
                {
                    var result = await _sapRfcService.FinishedGoodsReceiptToSapAsync(dto);

                    // ★ 核心技巧：由于该接口调用报错不会抛出 Exception，而是存放在返回值中，
                    // 所以我们在委托内部手动检测 result.Message，若命中锁定关键词则主动 throw，
                    // 以此来激活外层配置的 Polly 重试策略。
                    if (result != null && !string.IsNullOrEmpty(result.SapMessageType))
                    {
                        string msgLower = result.SapMessageType.ToLower();
                        if (msgLower.Contains("already being processed") ||
                            msgLower.Contains("lock") ||
                            msgLower.Contains("锁定"))
                        {
                            throw new Exception(result.SapMessageType); // 主动抛出，触发 Polly 指数退避重试
                        }

                        // (如果存在其他不可重试的业务错误，您可以选择是否抛出异常来阻断流转，或者记录告警日志)
                    }
                });
            }

            return await _finishedGoodsReceiptService.GetByIdAsync(dto.Id);

        }

        #endregion


        #region 3. 内部核心方法 (Core Helpers)

        /// <summary>
        /// ★ 重构：SAP 报工策略分发 (触发器解耦设计)
        /// 针对“材料准备报上一道，装配完工报本道”的业务场景进行解耦
        /// </summary>
        private async Task DispatchSapIntegrationForStepTaskAsync(WorkOrderStepTask step,int exeLogId, decimal outputQty, string employeeId)
        {
            // 如果是 Kitting 工步，通常有专门的移库逻辑，不参与标准报工
            if (step.TaskCategory == TaskCategories.Kitting) return;

            var currentOpTask = await _opTaskRepo.GetByIdAsync((int)step.OperationTaskId);
            if (currentOpTask == null) return;

            var allOpTasks = await _opTaskRepo.GetListByOrderIdAsync((int)currentOpTask.WorkOrderId);
            var sortedOps = allOpTasks.OrderBy(x => x.Operation).ToList();
            var currentIndex = sortedOps.FindIndex(x => x.Operation == currentOpTask.Operation);

            bool hasPushedPrev = false;
            // 1. 判断是否触发【前置工序】报工 (如 "材料准备")
            if (await IsSapPrevOpTriggerAsync(step.TaskCategory))
            {
                if (currentIndex > 0)
                {
                    var prevOpTask = sortedOps[currentIndex - 1];
                    try
                    {
                        // ★ 先在本地数据库创建报工确认日志，再推送 SAP
                        await RecordAndPushSapConfirmAsync(prevOpTask, exeLogId, outputQty, employeeId);
                        hasPushedPrev = true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"前置工序({prevOpTask.Operation}) 报工失败，操作已阻断: {ex.Message}");
                    }
                }
            }

            // 2. 判断是否触发【当前工序】报工 (如 "装配完工", "绝缘")
            if (await IsSapCurrentOpTriggerAsync(step.TaskCategory))
            {
                // ★ 主动缓冲：如果刚刚推送了前置报工，这里强行延时，给 SAP 留出异步释放锁的时间
                if (hasPushedPrev)
                {
                    await Task.Delay(2000);
                }
                // ★ 先在本地数据库创建报工确认日志，再推送 SAP
                await RecordAndPushSapConfirmAsync(currentOpTask, exeLogId, outputQty, employeeId);
            }

            // 注意：那些纯本地记件的工步（如焊接、压接），如果既不配为 Prev 也不配为 Current，这里就会自动跳过，不报 SAP。
        }


        /// <summary>
        /// ★ 核心封装：在推送 SAP 报工前，先将数据存入本地数据库，并记录消耗清单
        /// </summary>
        private async Task RecordAndPushSapConfirmAsync(WorkOrderOperationTask opTask, int exeLogId, decimal qty, string employeeId)
        {
            // 1. 补充基础信息用于记录
            var process = await _processRepo.GetByIdAsync((int)opTask.WorkOrderProcessId);
            var workOrder = await _workOrderRepo.GetByIdAsync((int)opTask.WorkOrderId);
            var factory = await _factoryRepository.GetByIdAsync(workOrder.FactoryId);

            // 2. 评估当前报工是否为最后一次报工
            bool isFinal = (opTask.CompletedQuantity ?? 0) >= opTask.Quantity;
            string completedFlag = isFinal ? "X" : "";

            // 生成 ConfirmSequence
            string confirmSeq = _serialRepo.GenerateNext($"{factory.FactoryCode}ConfirmReportToSAP");

            // 3. 将本地记录持久化至数据库
            var confirmCreateDto = new WorkOrderOperationConfirmCreateDto
            {
                WorkOrderId = workOrder.Id,
                ProcessId = process.Id,
                TaskConfirmId = exeLogId,
                SapConfirmationNo = process.ConfirmNo,
                WorkOrderNo = workOrder.OrderNumber.PadLeft(12, '0'),
                OperationNo = process.Operation,
                CompletedFlag = completedFlag,
                ConfirmSequence = confirmSeq,
                WorkCenterCode = process.WorkCenter,
                PostingDate = DateTime.Now.Date,
                EmployeeId = employeeId,
                FactoryCode = factory.FactoryCode,
                BaseUnit = "ST",
                YieldQuantity = qty,
                ScrapQuantity = 0,
                ActFinishDate = DateTime.Now.ToString("yyyyMMdd"),
                ActFinishTime = DateTime.Now.ToString("HH:mm:ss"),
                CreatedBy = employeeId,
            };

            // 创建 Confirm 记录
            var createdConfirm = await _opConfirmService.CreateAsync(confirmCreateDto);

            // ★ 核心追加：针对物料消耗（Kitting 投入）生成并推入操作消耗记录库
            if (createdConfirm != null)
            {
                await CreateConsumptionRecordsAsync(factory.FactoryCode, employeeId, process.Id, createdConfirm, workOrder, qty);

                // 4. 正式向 SAP 发送报工指令 (支持针对返回值的自动重试)
                if (await CheckSapEnabledAsync())
                {
                    // 定义重试策略：捕获因锁定引发的主动抛出异常
                    var sapRetryPolicy = Policy
                        .Handle<Exception>(ex =>
                            ex.Message.ToLower().Contains("already being processed") ||
                            ex.Message.Contains("锁定") ||
                            ex.Message.ToLower().Contains("lock"))
                        .WaitAndRetryAsync(
                            3, // 重试 3 次
                            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                        );

                    await sapRetryPolicy.ExecuteAsync(async () =>
                    {
                        var result = await _sapRfcService.ConfirmOrderCompletionToSAPAsync(createdConfirm.Id);

                        // ★ 核心技巧：由于该接口调用报错不会抛出 Exception，而是存放在返回值中，
                        // 所以我们在委托内部手动检测 result.Message，若命中锁定关键词则主动 throw，
                        // 以此来激活外层配置的 Polly 重试策略。
                        if (result != null && !string.IsNullOrEmpty(result.Message))
                        {
                            string msgLower = result.Message.ToLower();
                            if (msgLower.Contains("already being processed") ||
                                msgLower.Contains("lock") ||
                                msgLower.Contains("锁定"))
                            {
                                throw new Exception(result.Message); // 主动抛出，触发 Polly 指数退避重试
                            }

                            // (如果存在其他不可重试的业务错误，您可以选择是否抛出异常来阻断流转，或者记录告警日志)
                        }
                    });
                }

                if (createdConfirm.MessageType?.ToUpper() == "E")
                    throw new Exception(createdConfirm.Message);
            }

       
        }

        /// <summary>
        /// ★ 基于比例分摊，将 Kitting 表中的记录同步创建为工序报工消耗记录 (Consump)
        /// </summary>
        private async Task CreateConsumptionRecordsAsync(string factoryCode, string employeeId, int processId, dynamic sapConfirm, WorkOrder order, decimal yieldQty)
        {

            // 1. 获取有效的 BOM 行 (作为驱动主表)
            var boms = await _bomRepo.GetListByProcessIdsAsync(new List<int>() { processId });
            if (boms == null || !boms.Any()) return;

            var activeBoms = boms.Where(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed &&
                                            (x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial)).ToList();
            if (!activeBoms.Any()) return;

            // 2. 获取 Kitting 表中的记录
            // 移除原来 !rawRecords.Any() 的拦截，因为我们要处理没有齐套记录的情况
            var rawRecords = await _kittingItemRepo.GetListByProcessIdAsync(processId);
            // 3. 计算本道报工占比及剩余数量
            var currentProcess = await _processRepo.GetByIdAsync(processId);

            // 获取当前工序的历史确认总和 (注意此时刚刚创建的 sapConfirm 已经被统计在内了)
            var processConfirms = await _opConfirmService.GetListByProcessIdAsync(processId);
            var historicalCompletedQty = processConfirms.Where(s => s.Id != sapConfirm.Id && s.Status != "0").Sum(s => s.YieldQuantity ?? 0);

            // 判定计算逻辑（采用经典的剩余回冲算法）
            // 剩余数量 = 订单总量 - 截止目前的总完工数量 
            decimal remainingOrderQty = (currentProcess.Quantity ?? 0) - historicalCompletedQty;

            decimal ratio = 0;
            bool isLastStepOrOverProduction = false;

            if (remainingOrderQty <= yieldQty)
            {
                ratio = 1;
                isLastStepOrOverProduction = true; // 最后一次或超产：清空线边库剩余全部
            }
            else
            {
                if (remainingOrderQty <= 0) remainingOrderQty = 1; // 容错除零
                ratio = yieldQty / remainingOrderQty;
                if (ratio > 1) ratio = 1;
            }

            // 读取历史已存在的消耗记录
            var existsConsumptions = await _opConsumpService.GetListByProcessIdAsync(processId);
            var consumptionList = new List<WorkOrderOperationConsumpCreateDto>();

            // 获取读取 SAP 的库位配置
            var locationGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_SapLocation);
            var sapLineLocation = locationGroup?.Items?.FirstOrDefault(p => p.Key == Key_CableProLineStock)?.Value ?? "2102";
            if (string.IsNullOrEmpty(order.PlannerRemark) || !order.PlannerRemark.Contains(sapLineLocation))
                sapLineLocation = "2100";

            // 4. ★ 核心修改：以 BOM 为主循环，实现 Left Join 逻辑
            foreach (var bom in activeBoms)
            {
                // 查找当前 BOM 对应的 Kitting 记录
                var matchingKittings = rawRecords.Where(r => r.ReservationItem == bom.ReservationItem && r.MaterialCode == bom.MaterialCode).ToList();

                if (!matchingKittings.Any())
                {
                    // ★ 需求实现：BOM中需要，但 Kitting 表中不存在时，生成一条 Batch 为空、数量为 0 的记录
                    consumptionList.Add(new WorkOrderOperationConsumpCreateDto
                    {
                        OperationConfirmId = sapConfirm.Id,
                        SapConfirmationNo = sapConfirm.SapConfirmationNo,
                        WorkOrderNo = order.OrderNumber.PadLeft(12, '0'),
                        ConfirmSequence = sapConfirm.ConfirmSequence,
                        ReservationNo = order.ReservationNo,
                        ReservationItem = bom.ReservationItem,
                        MaterialCode = bom.MaterialCode.StartsWith("E") ? bom.MaterialCode : bom.MaterialCode.PadLeft(18, '0'),
                        FactoryCode = factoryCode,
                        FromLocationCode = sapLineLocation,
                        BatchCode = string.Empty, // 空批次
                        MovementType = MOVEMENT_PROD, // 默认使用生产投料类型 261
                        MovementReason = string.Empty,
                        Quantity = 0m,            // 数量为 0
                        BaseUnit = bom.Unit ?? "ST",
                        CreatedBy = employeeId,
                    });
                }
                else
                {
                    // 有对应的 Kitting 记录，按批次正常计算消耗
                    var groupedRecords = matchingKittings.GroupBy(g => new {
                        g.ReservationItem,
                        g.MaterialCode,
                        g.BatchCode,
                        g.BaseUnit,
                        g.ConsumptionType,
                        ConsumptionRemark = g.ConsumptionRemark ?? ""
                    });

                    foreach (var group in groupedRecords)
                    {
                        decimal totalIn = group.Sum(x => x.Quantity);

                        decimal totalOut = 0;
                        if (existsConsumptions != null && existsConsumptions.Any())
                        {
                            totalOut = existsConsumptions
                                .Where(x => x.ReservationItem == group.Key.ReservationItem
                                         && x.MaterialCode.TrimStart('0') == group.Key.MaterialCode.TrimStart('0')
                                         && x.BatchCode == group.Key.BatchCode
                                         && x.MovementType == group.Key.ConsumptionType
                                         && x.MovementReason == group.Key.ConsumptionRemark)
                                .Sum(x => x.Quantity ?? 0);
                        }

                        decimal totalAvailableStock = totalIn - totalOut;
                        if (totalAvailableStock < 0) totalAvailableStock = 0;

                        decimal consumeQty = 0;

                        if (isLastStepOrOverProduction)
                        {
                            consumeQty = totalAvailableStock;
                        }
                        else
                        {
                            consumeQty = totalAvailableStock * ratio;

                            if (IsDiscreteUnit(group.Key.BaseUnit))
                            {
                                consumeQty = Math.Floor(consumeQty);

                                if (consumeQty == 0 && (totalAvailableStock * ratio) > 0 && totalIn % 1 == 0 && totalAvailableStock >= 1)
                                {
                                    consumeQty = 1;
                                }
                            }
                            else
                            {
                                consumeQty = Math.Round(consumeQty, 3);
                            }
                        }

                        if (consumeQty > totalAvailableStock) consumeQty = totalAvailableStock;
                        if (consumeQty < 0) consumeQty = 0;

                        // 已有 Kitting 记录且消耗完时不需再写入 0
                        if (consumeQty == 0) continue;

                        var consumDto = new WorkOrderOperationConsumpCreateDto
                        {
                            OperationConfirmId = sapConfirm.Id,
                            SapConfirmationNo = sapConfirm.SapConfirmationNo,
                            WorkOrderNo = order.OrderNumber.PadLeft(12, '0'),
                            ConfirmSequence = sapConfirm.ConfirmSequence,
                            ReservationNo = order.ReservationNo,
                            ReservationItem = group.Key.ReservationItem,
                            MaterialCode = group.Key.MaterialCode.StartsWith("E") ? group.Key.MaterialCode : group.Key.MaterialCode.PadLeft(18, '0'),
                            FactoryCode = factoryCode,
                            FromLocationCode = sapLineLocation,
                            BatchCode = group.Key.BatchCode,
                            MovementType = group.Key.ConsumptionType,
                            MovementReason = group.Key.ConsumptionRemark,
                            Quantity = consumeQty,
                            BaseUnit = group.Key.BaseUnit,
                            CreatedBy = employeeId,
                        };


                        consumptionList.Add(consumDto);
                    }
                }
            }

            if (consumptionList.Any())
            {
                await _opConsumpService.CreateAsync(consumptionList);
            }
        }


        private async Task CreateConsumptionRecordsAsync(int sapConfirmId)
        {
            WorkOrderOperationConfirmDto sapConfirm = await _opConfirmService.GetConfirmWitemConsumeptionAsync(sapConfirmId);
            if (sapConfirm == null || sapConfirm.Status == "1")
                throw new Exception("当前报工记录不可重推，请重试！");
            //将报工记录中的消耗记录状态重置
            if (sapConfirm.Consumps.Where(x => x.Status == "1").Count() > 0)
            {
                await _opConsumpService.UpdateAsync(sapConfirm.Consumps.Where(x => x.Status == "1")
                    .Select(x => new WorkOrderOperationConsumpUpdateDto()
                    {
                        Id = x.Id,
                        Status = "0"
                    }).ToList());
            }

            // 1. 获取有效的 BOM 行 (作为驱动主表)
            var boms = await _bomRepo.GetListByProcessIdsAsync(new List<int>() { (int)sapConfirm.ProcessId });
            if (boms.Any()) 
            {
                var activeBoms = boms.Where(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed &&
                                (x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial)).ToList();
                if (activeBoms.Any())
                {


                    // 2. 获取 Kitting 表中的记录
                    // 移除原来 !rawRecords.Any() 的拦截，因为我们要处理没有齐套记录的情况
                    var rawRecords = await _kittingItemRepo.GetListByProcessIdAsync((int)sapConfirm.ProcessId);
                    // 3. 计算本道报工占比及剩余数量
                    var currentProcess = await _processRepo.GetByIdAsync((int)sapConfirm.ProcessId);
                    var currentWorkOrder = await _workOrderRepo.GetByIdAsync((int)sapConfirm.WorkOrderId);

                    // 获取当前工序的历史确认总和 (注意此时刚刚创建的 sapConfirm 已经被统计在内了)
                    var processConfirms = await _opConfirmService.GetListByProcessIdAsync((int)sapConfirm.ProcessId);
                    var historicalCompletedQty = processConfirms.Where(s => s.Id != sapConfirm.Id && s.Status != "0").Sum(s => s.YieldQuantity ?? 0);

                    // 判定计算逻辑（采用经典的剩余回冲算法）
                    // 剩余数量 = 订单总量 - 截止目前的总完工数量 
                    decimal remainingOrderQty = (currentProcess.Quantity ?? 0) - historicalCompletedQty;

                    decimal ratio = 0;
                    bool isLastStepOrOverProduction = false;

                    if (remainingOrderQty <= (decimal)sapConfirm.YieldQuantity)
                    {
                        ratio = 1;
                        isLastStepOrOverProduction = true; // 最后一次或超产：清空线边库剩余全部
                    }
                    else
                    {
                        if (remainingOrderQty <= 0) remainingOrderQty = 1; // 容错除零
                        ratio = (decimal)sapConfirm.YieldQuantity / remainingOrderQty;
                        if (ratio > 1) ratio = 1;
                    }

                    // 读取历史已存在的消耗记录
                    var existsConsumptions = await _opConsumpService.GetListByProcessIdAsync((int)sapConfirm.ProcessId);
                    var consumptionList = new List<WorkOrderOperationConsumpCreateDto>();

                    // 获取读取 SAP 的库位配置
                    var locationGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_SapLocation);
                    var sapLineLocation = locationGroup?.Items?.FirstOrDefault(p => p.Key == Key_CableProLineStock)?.Value ?? "2102";
                    if (string.IsNullOrEmpty(currentWorkOrder.PlannerRemark) || !currentWorkOrder.PlannerRemark.Contains(sapLineLocation))
                        sapLineLocation = "2100";

                    // 4. ★ 核心修改：以 BOM 为主循环，实现 Left Join 逻辑
                    foreach (var bom in activeBoms)
                    {
                        // 查找当前 BOM 对应的 Kitting 记录
                        var matchingKittings = rawRecords.Where(r => r.ReservationItem == bom.ReservationItem && r.MaterialCode == bom.MaterialCode).ToList();

                        if (!matchingKittings.Any())
                        {
                            // ★ 需求实现：BOM中需要，但 Kitting 表中不存在时，生成一条 Batch 为空、数量为 0 的记录
                            consumptionList.Add(new WorkOrderOperationConsumpCreateDto
                            {
                                OperationConfirmId = sapConfirm.Id,
                                SapConfirmationNo = sapConfirm.SapConfirmationNo,
                                WorkOrderNo = currentWorkOrder.OrderNumber.PadLeft(12, '0'),
                                ConfirmSequence = sapConfirm.ConfirmSequence,
                                ReservationNo = currentWorkOrder.ReservationNo,
                                ReservationItem = bom.ReservationItem,
                                MaterialCode = bom.MaterialCode.StartsWith("E") ? bom.MaterialCode : bom.MaterialCode.PadLeft(18, '0'),
                                FactoryCode = sapConfirm.FactoryCode,
                                FromLocationCode = sapLineLocation,
                                BatchCode = string.Empty, // 空批次
                                MovementType = MOVEMENT_PROD, // 默认使用生产投料类型 261
                                MovementReason = string.Empty,
                                Quantity = 0m,            // 数量为 0
                                BaseUnit = bom.Unit ?? "ST",
                                CreatedBy = sapConfirm.CreatedBy,
                            });
                        }
                        else
                        {
                            // 有对应的 Kitting 记录，按批次正常计算消耗
                            var groupedRecords = matchingKittings.GroupBy(g => new
                            {
                                g.ReservationItem,
                                g.MaterialCode,
                                g.BatchCode,
                                g.BaseUnit,
                                g.ConsumptionType,
                                ConsumptionRemark = g.ConsumptionRemark ?? ""
                            });

                            foreach (var group in groupedRecords)
                            {
                                decimal totalIn = group.Sum(x => x.Quantity);

                                decimal totalOut = 0;
                                if (existsConsumptions != null && existsConsumptions.Any())
                                {
                                    totalOut = existsConsumptions
                                        .Where(x => x.ReservationItem == group.Key.ReservationItem
                                                 && x.MaterialCode.TrimStart('0') == group.Key.MaterialCode.TrimStart('0')
                                                 && x.BatchCode == group.Key.BatchCode
                                                 && x.MovementType == group.Key.ConsumptionType
                                                 && x.MovementReason == group.Key.ConsumptionRemark)
                                        .Sum(x => x.Quantity ?? 0);
                                }

                                decimal totalAvailableStock = totalIn - totalOut;
                                if (totalAvailableStock < 0) totalAvailableStock = 0;

                                decimal consumeQty = 0;

                                if (isLastStepOrOverProduction)
                                {
                                    consumeQty = totalAvailableStock;
                                }
                                else
                                {
                                    consumeQty = totalAvailableStock * ratio;

                                    if (IsDiscreteUnit(group.Key.BaseUnit))
                                    {
                                        consumeQty = Math.Floor(consumeQty);

                                        if (consumeQty == 0 && (totalAvailableStock * ratio) > 0 && totalIn % 1 == 0 && totalAvailableStock >= 1)
                                        {
                                            consumeQty = 1;
                                        }
                                    }
                                    else
                                    {
                                        consumeQty = Math.Round(consumeQty, 3);
                                    }
                                }

                                if (consumeQty > totalAvailableStock) consumeQty = totalAvailableStock;
                                if (consumeQty < 0) consumeQty = 0;

                                // 已有 Kitting 记录且消耗完时不需再写入 0
                                if (consumeQty == 0) continue;

                                var consumDto = new WorkOrderOperationConsumpCreateDto
                                {
                                    OperationConfirmId = sapConfirm.Id,
                                    SapConfirmationNo = sapConfirm.SapConfirmationNo,
                                    WorkOrderNo = currentWorkOrder.OrderNumber.PadLeft(12, '0'),
                                    ConfirmSequence = sapConfirm.ConfirmSequence,
                                    ReservationNo = currentWorkOrder.ReservationNo,
                                    ReservationItem = group.Key.ReservationItem,
                                    MaterialCode = group.Key.MaterialCode.StartsWith("E") ? group.Key.MaterialCode : group.Key.MaterialCode.PadLeft(18, '0'),
                                    FactoryCode = sapConfirm.FactoryCode,
                                    FromLocationCode = sapLineLocation,
                                    BatchCode = group.Key.BatchCode,
                                    MovementType = group.Key.ConsumptionType,
                                    MovementReason = group.Key.ConsumptionRemark,
                                    Quantity = consumeQty,
                                    BaseUnit = group.Key.BaseUnit,
                                    CreatedBy = sapConfirm.CreatedBy,
                                };


                                consumptionList.Add(consumDto);
                            }
                        }
                    }

                    if (consumptionList.Any())
                    {
                        await _opConsumpService.CreateAsync(consumptionList);
                    }
                }
            }

            if (await CheckSapEnabledAsync())
            {
                var result = await _sapRfcService.ConfirmOrderCompletionToSAPAsync(sapConfirm.Id);
                if (result.MessageType?.ToUpper() == "E") 
                {
                    throw new Exception(result.Message);
                }
            }
        }

        /// <summary>
        /// ★ 终极优化：批量重推/创建消耗记录，并推送 SAP
        /// 融合旧消耗作废、Left Join 空记录补齐、O(1) 滚动内存运算、Polly 自动重试
        /// </summary>
        private async Task CreateConsumptionRecordsBatchAsync(List<int> sapConfirmIds)
        {
            if (sapConfirmIds == null || !sapConfirmIds.Any()) return;

            // 1. 获取所有待处理的报工记录及其关联的消耗明细
            //var sapConfirmsTasks = sapConfirmIds.Select(id => _opConfirmService.GET(id));
            var sapConfirms = await _opConfirmService.GetConfirmWitemConsumeptionAsync(sapConfirmIds);

            if (!sapConfirms.Any()) throw new Exception("未找到对应的报工记录！");
            if (sapConfirms.Any(c => c.Status == "1"))
                throw new Exception("部分报工记录已成功推送到 SAP，不可重复推送，请刷新后重试！");

            // 2. ★ 状态重置：将这些报工记录之前关联的旧消耗记录作废 (Status = "0")
            var consumpToReset = sapConfirms
                .Where(c => c.Consumps != null)
                .SelectMany(c => c.Consumps)
                .Where(x => x.Status == "1")
                .Select(x => new WorkOrderOperationConsumpUpdateDto { Id = x.Id, Status = "0" })
                .ToList();

            if (consumpToReset.Any())
            {
                await _opConsumpService.UpdateAsync(consumpToReset);
            }

            var allConsumptionList = new List<WorkOrderOperationConsumpCreateDto>();

            // 提取去重后的 ProcessId 用于批量加载数据
            var processIds = sapConfirms.Where(c => c.ProcessId.HasValue).Select(c => c.ProcessId.Value).Distinct().ToList();
            if (!processIds.Any()) return;

            // 3. 【极速优化】并发获取所有依赖数据，彻底消除循环内 N+1 查询
            var locationGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_SapLocation);
            var processesList = await _processRepo.GetByIdsAsync(processIds);
            var rawRecordsList = await _kittingItemRepo.GetListByProcessIdAsync(processIds);
            var bomsList = await _bomRepo.GetListByProcessIdsAsync(processIds);
            var historicalConfirms = await _opConfirmService.GetListByProcessIdAsync(processIds);
            var existsConsumptions = await _opConsumpService.GetListByProcessIdAsync(processIds);

            // 解析任务结果并建立查找表
            // 解析任务结果并建立查找表
            var defaultSapLineLocation = locationGroup?.Items?.FirstOrDefault(p => p.Key == Key_CableProLineStock)?.Value ?? "2102";
            var processes = processesList.ToDictionary(p => p.Id);

            var orderIds = processes.Values.Select(p => p.WorkOrderId ?? 0).Where(id => id > 0).Distinct().ToList();
            var ordersList = await _workOrderRepo.GetByIdsAsync(orderIds);
            var orderDict = ordersList.ToDictionary(o => o.Id);

            var rawRecordsLookup = rawRecordsList.ToLookup(r => r.WorkOrderProcessId);
            var bomsLookup = bomsList
                .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true &&
                           (x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial))
                .ToLookup(b => b.WorkOrderProcessId);

            // 4. 构建滚动库存追踪字典 (O(1) 复杂度)
            var consumedStockMap = new Dictionary<string, decimal>();
            foreach (var c in existsConsumptions)
            {
                // ★ 极其关键：必须排除掉刚才被作废(Status="0")的记录，确保可用库存被正确"退回"
                if (c.Status != "1") continue;
                string safeMaterialCode = string.IsNullOrEmpty(c.MaterialCode) ? "" : c.MaterialCode.TrimStart('0');

                string key = $"{c.WorkOrderProcessId}_{c.ReservationItem}_{safeMaterialCode}_{c.BatchCode}_{c.MovementType}_{c.MovementReason}";
                if (!consumedStockMap.ContainsKey(key)) consumedStockMap[key] = 0;
                consumedStockMap[key] += (c.Quantity ?? 0);
            }

            // 构建历史已报工数量字典 (排除当前批次传入的 Confirm)
            var batchConfirmIds = sapConfirms.Select(c => c.Id).ToHashSet();
            var runningCompletedQtyMap = historicalConfirms
                .Where(s => s.Status != "0" && !batchConfirmIds.Contains(s.Id))
                .GroupBy(s => s.ProcessId ?? 0)
                .ToDictionary(g => g.Key, g => g.Sum(s => s.YieldQuantity ?? 0));

            // 5. 内存中极速分组滚动计算
            var confirmsByProcess = sapConfirms.GroupBy(c => c.ProcessId).ToList();
            foreach (var processGroup in confirmsByProcess)
            {
                int processId = processGroup.Key ?? 0;
                if (!processes.TryGetValue(processId, out var currentProcess)) continue;
                if (!orderDict.TryGetValue(currentProcess.WorkOrderId ?? 0, out var currentWorkOrder)) continue;

                string currentSapLocation = defaultSapLineLocation;
                if (string.IsNullOrEmpty(currentWorkOrder.PlannerRemark) || !currentWorkOrder.PlannerRemark.Contains(currentSapLocation))
                    currentSapLocation = "2100";

                var activeBoms = bomsLookup[processId].ToList();
                var processRawRecords = rawRecordsLookup[processId].ToList();

                if (!runningCompletedQtyMap.TryGetValue(processId, out decimal runningCompletedQty))
                    runningCompletedQty = 0;

                foreach (var sapConfirm in processGroup.OrderBy(c => c.Id))
                {
                    decimal yieldQty = sapConfirm.YieldQuantity ?? 0;
                    decimal remainingOrderQty = (currentProcess.Quantity ?? 0) - runningCompletedQty;

                    decimal ratio = 0;
                    bool isLastStepOrOverProduction = false;

                    if (remainingOrderQty <= yieldQty || sapConfirm.CompletedFlag == "X")
                    {
                        ratio = 1;
                        isLastStepOrOverProduction = true;
                    }
                    else
                    {
                        if (remainingOrderQty <= 0) remainingOrderQty = 1;
                        ratio = yieldQty / remainingOrderQty;
                        if (ratio > 1) ratio = 1;
                    }

                    foreach (var bom in activeBoms)
                    {
                        var matchingKittings = processRawRecords.Where(r => r.ReservationItem == bom.ReservationItem && r.MaterialCode == bom.MaterialCode).ToList();

                        if (!matchingKittings.Any())
                        {
                            allConsumptionList.Add(new WorkOrderOperationConsumpCreateDto
                            {
                                OperationConfirmId = sapConfirm.Id,
                                SapConfirmationNo = sapConfirm.SapConfirmationNo,
                                WorkOrderNo = currentWorkOrder.OrderNumber.PadLeft(12, '0'),
                                ConfirmSequence = sapConfirm.ConfirmSequence,
                                ReservationNo = currentWorkOrder.ReservationNo,
                                ReservationItem = bom.ReservationItem,
                                MaterialCode = bom.MaterialCode.StartsWith("E") ? bom.MaterialCode : bom.MaterialCode.PadLeft(18, '0'),
                                FactoryCode = sapConfirm.FactoryCode,
                                FromLocationCode = currentSapLocation,
                                BatchCode = string.Empty,
                                MovementType = MOVEMENT_PROD,
                                MovementReason = string.Empty,
                                Quantity = 0m,
                                BaseUnit = bom.Unit ?? "ST",
                                CreatedBy = sapConfirm.CreatedBy,
                            });
                        }
                        else
                        {
                            var groupedRecords = matchingKittings.GroupBy(g => new {
                                g.ReservationItem,
                                g.MaterialCode,
                                g.BatchCode,
                                g.BaseUnit,
                                g.ConsumptionType,
                                ConsumptionRemark = g.ConsumptionRemark ?? ""
                            });

                            foreach (var group in groupedRecords)
                            {
                                decimal totalIn = group.Sum(x => x.Quantity);

                                string stockKey = $"{processId}_{group.Key.ReservationItem}_{group.Key.MaterialCode.TrimStart('0')}_{group.Key.BatchCode}_{group.Key.ConsumptionType}_{group.Key.ConsumptionRemark}";
                                consumedStockMap.TryGetValue(stockKey, out decimal totalOut);

                                decimal totalAvailableStock = totalIn - totalOut;
                                if (totalAvailableStock < 0) totalAvailableStock = 0;

                                decimal consumeQty = 0;

                                if (isLastStepOrOverProduction)
                                {
                                    consumeQty = totalAvailableStock;
                                }
                                else
                                {
                                    consumeQty = totalAvailableStock * ratio;
                                    if (IsDiscreteUnit(group.Key.BaseUnit))
                                    {
                                        consumeQty = Math.Floor(consumeQty);
                                        if (consumeQty == 0 && (totalAvailableStock * ratio) > 0 && totalIn % 1 == 0 && totalAvailableStock >= 1)
                                        {
                                            consumeQty = 1;
                                        }
                                    }
                                    else
                                    {
                                        consumeQty = Math.Round(consumeQty, 3);
                                    }
                                }

                                if (consumeQty > totalAvailableStock) consumeQty = totalAvailableStock;
                                if (consumeQty < 0) consumeQty = 0;

                                if (consumeQty == 0) continue;

                                allConsumptionList.Add(new WorkOrderOperationConsumpCreateDto
                                {
                                    OperationConfirmId = sapConfirm.Id,
                                    SapConfirmationNo = sapConfirm.SapConfirmationNo,
                                    WorkOrderNo = currentWorkOrder.OrderNumber.PadLeft(12, '0'),
                                    ConfirmSequence = sapConfirm.ConfirmSequence,
                                    ReservationNo = currentWorkOrder.ReservationNo,
                                    ReservationItem = group.Key.ReservationItem,
                                    MaterialCode = group.Key.MaterialCode.StartsWith("E") ? group.Key.MaterialCode : group.Key.MaterialCode.PadLeft(18, '0'),
                                    FactoryCode = sapConfirm.FactoryCode,
                                    FromLocationCode = currentSapLocation,
                                    BatchCode = group.Key.BatchCode,
                                    MovementType = group.Key.ConsumptionType?.ToString(),
                                    MovementReason = group.Key.ConsumptionRemark,
                                    Quantity = consumeQty,
                                    BaseUnit = group.Key.BaseUnit,
                                    CreatedBy = sapConfirm.CreatedBy,
                                });

                                // 更新字典的值，为下一条 Confirm 做减法准备
                                consumedStockMap[stockKey] = totalOut + consumeQty;
                            }
                        }
                    }

                    // 更新已完成产量供下一笔记录使用
                    runningCompletedQty += yieldQty;
                    runningCompletedQtyMap[processId] = runningCompletedQty;
                }
            }

            // 6. 批量写入全新的消耗记录
            if (allConsumptionList.Any())
            {
                await _opConsumpService.CreateAsync(allConsumptionList);
            }

            // 7. ★ 向 SAP 批量重推指令 (带有 Polly 重试与主动缓冲机制)
            if (await CheckSapEnabledAsync())
            {

                var result = await _sapRfcService.ConfirmBatchOrderCompletionToSAPAsync(sapConfirms.Select(x => x.Id).ToList());

            }
        }

        /// <summary>
        /// 辅助方法：判定是否为离散单位 (PC, EA, PCS, ST, 个, 件)
        /// 离散单位在按比例计算物料消耗时需向下取整 (Floor)，不能保留小数点
        /// </summary>
        private bool IsDiscreteUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit)) return false;
            var discreteUnits = new[] { "PC", "PCS", "EA", "ST", "个", "件" };
            return discreteUnits.Contains(unit.ToUpper());
        }

        /// <summary>
        /// 辅助方法：安全读取配置开关
        /// </summary>
        private async Task<bool> CheckSapEnabledAsync()
        {
            try
            {
                var group = await _parameterGroupRepo.GetGroupWithItemsAsync(SAP_TRANSFER_CONFIG_GROUP);
                var param = group?.Items?.FirstOrDefault(x => x.Key == SAP_CONFIG_KEY_ENABLED);

                // 默认策略：如果没配置，默认开启还是关闭？这里假设没配置则开启(与原代码逻辑一致)
                if (param == null) return true;

                // 安全解析 bool
                if (bool.TryParse(param.Value, out bool result))
                {
                    return result;
                }

                // 兼容 "1"/"0"
                if (param.Value == "1") return true;

                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        /// <summary>
        /// ★ 向上汇总更新工序(OperationTask)和基础工序(WorkOrderProcess)进度：基于触发器解耦的工序进度汇总与特定任务生成逻辑
        /// </summary>
        private async Task RollupOperationProgressAsync(WorkOrderStepTask currentStep, decimal outputQty, string employeeId, DateTime? endTime)
        {
            var currentOpTask = await _opTaskRepo.GetByIdAsync((int)currentStep.OperationTaskId);
            if (currentOpTask == null) return;

            var allOpTasks = await _opTaskRepo.GetListByOrderIdAsync((int)currentOpTask.WorkOrderId);
            var sortedOps = allOpTasks.OrderBy(x => x.Operation).ToList();
            var currentIndex = sortedOps.FindIndex(x => x.Operation == currentOpTask.Operation);

            // 1. 检查当前工步是否是【前道工序】的触发器 (例如：材料准备 MAT_CHECK)
            // 如果是，需要汇总【前道工序】的进度，并可能为前道工序生成入库任务
            if (await IsSapPrevOpTriggerAsync(currentStep.TaskCategory) && currentIndex > 0)
            {
                var prevOpTask = sortedOps[currentIndex - 1];
                await ExecuteRollupToSpecificOperationAsync(prevOpTask, outputQty, employeeId, endTime, currentStep);
            }

            // 2. 检查当前工步是否是【本道工序】的里程碑 (例如：装配完工 ASSY_END)
            // 如果是，需要汇总【本道工序】的进度，并可能为本道工序生成入库任务
            if (await IsMilestoneStepAsync(currentStep.TaskCategory))
            {
                await ExecuteRollupToSpecificOperationAsync(currentOpTask, outputQty, employeeId, endTime, currentStep);
            }
        }

        /// <summary>
        /// 核心：将数量汇总到特定的目标工序(OperationTask)，并根据需要创建入库任务
        /// </summary>
        private async Task ExecuteRollupToSpecificOperationAsync(WorkOrderOperationTask targetOpTask, decimal outputQty, string employeeId, DateTime? endTime, WorkOrderStepTask triggerStep)
        {
            // 1. 更新目标工序任务 (Operation Task)
            targetOpTask.CompletedQuantity = (targetOpTask.CompletedQuantity ?? 0) + outputQty;

            // 状态流转控制
            if (targetOpTask.CompletedQuantity >= targetOpTask.Quantity)
            {
                targetOpTask.Status = WorkTaskStatus.Completed;
                targetOpTask.ActualEndTime = endTime ?? DateTime.Now;
            }
            else if (targetOpTask.Status == WorkTaskStatus.New || targetOpTask.Status == WorkTaskStatus.Released)
            {
                targetOpTask.Status = WorkTaskStatus.InProgress;
                targetOpTask.ActualStartTime = targetOpTask.ActualStartTime ?? DateTime.Now;
            }

            targetOpTask.UpdateBy = employeeId;
            targetOpTask.UpdatedOn = DateTime.Now;
            await _opTaskRepo.UpdateAsync(targetOpTask);

            // 2. 更新目标工单基础工序 (WorkOrder Process)
            var process = await _processRepo.GetByIdAsync((int)targetOpTask.WorkOrderProcessId);
            if (process != null)
            {
                process.CompletedQuantity = (process.CompletedQuantity ?? 0) + outputQty;

                process.Status = ((int)WorkOrderStatus.OnGoing).ToString();
                if (process.CompletedQuantity >= process.Quantity)
                {
                    process.Status = ((int)WorkOrderStatus.Finished).ToString();
                }

                process.UpdateBy = employeeId;
                process.UpdateOn = DateTime.Now;

                await _processRepo.UpdateAsync(process);

                // 3. 判断是否需要为目标工序生成入库任务 (GoodsReceipt)
                if (process.ControlKey == CONTROL_KEY)
                {
                    var workOrder = await _workOrderRepo.GetByIdAsync((int)process.WorkOrderId);
                    if (workOrder != null && workOrder.MaterialCode == workOrder.LeadingOrderMaterial)
                    {
                        // 必须检查的是目标工序(targetOpTask)下是否已生成过入库任务
                        var receiptTask = await _stepRepo.GetByOperationIdAsync((int)targetOpTask.Id, TaskCategories.GoodsReceipt);
                        if (receiptTask == null)
                        {
                            var existingMaxCode = 0;
                            var stepTasks = await _stepRepo.GetListByOperationIdAsync((int)targetOpTask.Id);
                            if (stepTasks.Any())
                            {
                                var codes = stepTasks.Select(s => int.TryParse(s.StepCode, out int c) ? c : 0);
                                if (codes.Any()) existingMaxCode = codes.Max();
                            }

                            // 这里的入库任务是挂在目标工序(targetOpTask)下的
                            await _stepRepo.AddAsync(new WorkOrderStepTask()
                            {
                                StepCode = (existingMaxCode + 10).ToString(),
                                OperationTaskId = targetOpTask.Id, // ★ 关键修复
                                WorkCenterGroupId = triggerStep.WorkCenterGroupId,
                                WorkCenterGroupCode = triggerStep.WorkCenterGroupCode,
                                ActualWorkCenterId = triggerStep.ActualWorkCenterId,
                                ActualWorkCenterCode = triggerStep.ActualWorkCenterCode,
                                Quantity = targetOpTask.Quantity,
                                CompletedQuantity = 0,
                                TaskCategory = TaskCategories.GoodsReceipt,
                                PreStepIds = triggerStep.Id.ToString(),
                                Status = WorkTaskStatus.New,
                                CreatedBy = employeeId
                            });
                        }
                    }
                }
            }
        }

        #region 参数与策略配置读取

        /// <summary>
        /// 是否是当前工序进度的里程碑 (控制 MES 本地数量向上汇总)
        /// </summary>
        private async Task<bool> IsMilestoneStepAsync(string taskCategory)
        {
            var currentTaskCategory = await _categoryRepository.GetByCategoryCodeAsync(taskCategory);
            if (currentTaskCategory != null )
            {

                return currentTaskCategory.IsSapCurrentTrigger;
            }
            else 
            {
                var paramGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_MilestoneConfig);
                var milestoneStr = paramGroup?.Items?.FirstOrDefault(x => x.Key == Key_MilestoneSteps)?.Value;

                if (!string.IsNullOrEmpty(milestoneStr))
                {
                    return milestoneStr.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Contains(taskCategory, StringComparer.OrdinalIgnoreCase);
                }

                // 默认兜底：只有产生最终成品的步骤才算本地完工
                var defaultMilestones = new[] { TaskCategories.MaterialCheck, TaskCategories.AssemblyEnd, TaskCategories.Insulation, TaskCategories.LooseTube, TaskCategories.PairTwisting, TaskCategories.SingleTwisting, TaskCategories.Braiding, TaskCategories.Rewinding, TaskCategories.Sheathing, TaskCategories.FinalTest };
                return defaultMilestones.Contains(taskCategory, StringComparer.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// 是否触发【前置工序】的 SAP 报工
        /// 例如："材料准备" (STEP19 或 MAT_CHECK)
        /// </summary>
        private async Task<bool> IsSapPrevOpTriggerAsync(string taskCategory)
        {

            var currentTaskCategory = await _categoryRepository.GetByCategoryCodeAsync(taskCategory);
            if (currentTaskCategory != null)
            {
                return currentTaskCategory.IsSapPrevTrigger;
            }
            else 
            {
                var paramGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_MilestoneConfig);
                var triggerStr = paramGroup?.Items?.FirstOrDefault(x => x.Key == Key_SapPrevTriggers)?.Value;

                if (!string.IsNullOrEmpty(triggerStr))
                {
                    return triggerStr.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                                     .Contains(taskCategory, StringComparer.OrdinalIgnoreCase);
                }

                // 默认兜底：假设 "材料准备(MAT_CHECK)" 会报前一道
                var defaults = new[] { TaskCategories.MaterialCheck };
                return defaults.Contains(taskCategory, StringComparer.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// 是否触发【本道工序】的 SAP 报工
        /// 例如："装配完工" (STEP13 或 ASSY_END)
        /// </summary>
        private async Task<bool> IsSapCurrentOpTriggerAsync(string taskCategory)
        {

            var currentTaskCategory = await _categoryRepository.GetByCategoryCodeAsync(taskCategory);
            if (currentTaskCategory != null)
            {
                return currentTaskCategory.IsSapCurrentTrigger;
            }
            else 
            {
                var paramGroup = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_MilestoneConfig);
                var triggerStr = paramGroup?.Items?.FirstOrDefault(x => x.Key == Key_SapCurrentTriggers)?.Value;

                if (!string.IsNullOrEmpty(triggerStr))
                {
                    return triggerStr.Split(new[] { ',', ';', '|' }, StringSplitOptions.RemoveEmptyEntries)
                                     .Contains(taskCategory, StringComparer.OrdinalIgnoreCase);
                }

                // 默认兜底
                var defaults = new[] { TaskCategories.AssemblyEnd, TaskCategories.InspectionEnd, TaskCategories.Insulation, TaskCategories.LooseTube, TaskCategories.PairTwisting, TaskCategories.SingleTwisting, TaskCategories.Braiding, TaskCategories.Rewinding, TaskCategories.Sheathing, TaskCategories.FinalTest };
                return defaults.Contains(taskCategory, StringComparer.OrdinalIgnoreCase);
            }
               
        }

        #endregion

        private async Task<WorkOrderTaskExecuteLog> CreateExecuteLogAsync(string level, int taskId, decimal qty, int? stationId, string? stationCode, string user,string? remark,bool isFinished, DateTime? startTime, DateTime? endTime)
        {
            var now = DateTime.Now;

            // 修正时间逻辑：
            // 如果前端未传结束时间，默认当前时间
            var actualEndTime = endTime ?? now;
            // 如果前端未传开始时间，默认等于结束时间 (即瞬时完成)。
            // 提示：这会导致工时为0，建议前端传入真实的 StartTime
            var actualStartTime = startTime ?? actualEndTime;

            return await _exeLogRepo.AddAsync(new WorkOrderTaskExecuteLog
            {
                TaskLevel = level,
                TaskId = taskId,
                BatchCode = _serialRepo.GenerateNext(REPORT_SEQUENCE_CODE),
                CompletedQuantity = qty,
                WorkStationId = stationId,
                WorkStationCode = stationCode,
                EmployerCode = user,
                StartTime = actualStartTime,
                EndTime = actualEndTime,
                Status = isFinished ? WorkTaskStatus.Completed : WorkTaskStatus.InProgress,
                Remark = remark,
                CreatedBy = user,
                CreatedOn = DateTime.Now
            });
        }

        private async Task PreCheckMaterialAvailabilityAsync(int stationId, Dictionary<string, decimal> requirements)
        {
            if (requirements.Count == 0) return;

            var activeLoadings = await _loadingRepo.GetListByStationIdAsync(stationId, status: MaterialLoadingStatus.Active);
            var stationInventory = activeLoadings.Where(x => x.LastQuantity > 0)
                .GroupBy(x => x.MaterialCode)
                .ToDictionary(g => g.Key, g => g.Sum(x => x.LastQuantity));

            var errors = new List<string>();
            foreach (var req in requirements)
            {
                if (!stationInventory.TryGetValue(req.Key, out decimal available))
                    errors.Add($"物料 [{req.Key}] 未上料或余量为0，需要 {req.Value} M。");
                else if (available < req.Value)
                    errors.Add($"物料 [{req.Key}] 余量不足！当前: {available}，需要: {req.Value} M。");
            }

            if (errors.Any()) throw new Exception("物料校验失败，无法报工：\n" + string.Join("\n", errors));
        }

        private async Task DeductMaterialAndRecordAsync(int stationId, string materialCode, decimal qtyToDeduct, int exeLogId, string employeeId, string movementType, string movementReason)
        {
            decimal remaining = qtyToDeduct;
            var loadings = await _loadingRepo.GetListByStationIdAsync(stationId, status: MaterialLoadingStatus.Active);
            var sortedLoadings = loadings.Where(x => x.MaterialCode == materialCode && x.LastQuantity > 0).OrderBy(x => x.LoadedTime).ToList(); // FIFO
            var inventories = await _inventoryRepo.GetByIdsAsync(sortedLoadings.Select(x => x.LineSideInventoryId).ToList());
            var inventoryMap = inventories.ToDictionary(x => x.Id);
            var consumpLogs = new List<WorkOrderTaskExecuteConsump>();
            foreach (var loading in sortedLoadings)
            {
                if (remaining <= 0) break;

                decimal currentDeduct = Math.Min(loading.LastQuantity, remaining);

                // 更新上料记录 (考虑并发，可引入 Version 乐观锁)
                loading.LastQuantity -= currentDeduct;
                if (loading.LastQuantity == 0) loading.Status = MaterialLoadingStatus.Depleted; // 耗尽
                loading.UnloadedTime = DateTime.Now;

                // 2. 同步扣减线边库存 (使用新增的 ID 字段)
                // 只有当有关联的库存记录时才扣减
                if (loading.LineSideInventoryId > 0) 
                {
                    if (inventoryMap.TryGetValue(loading.LineSideInventoryId, out var inventory))
                    {
                        inventory.Quantity -= currentDeduct;
                        if (inventory.Quantity < 0) inventory.Quantity = 0; // 防御性归零
                        if (inventory.Quantity == 0) inventory.Status = LineInventoryStatus.Available;
                        inventory.UpdateBy = employeeId;
                        inventory.UpdatedAt = DateTime.Now;
                    }
                }

                // 记录消耗明细
                var consumpLog = new WorkOrderTaskExecuteConsump
                {
                    ExeLogId = exeLogId,
                    MaterialCode = materialCode,
                    BatchCode = loading.BatchCode,
                    BarCode = loading.BarCode,
                    ConsumedQuantity = currentDeduct,
                    BaseUnit = loading.BaseUnit,
                    MovementType = movementType,
                    MovementReason = movementReason,
                    Status = "1",
                    CreatedBy = employeeId,
                    CreatedOn = DateTime.Now
                };
                consumpLogs.Add(consumpLog);

                remaining -= currentDeduct;
            }
            if (remaining > 0)
                throw new Exception($"并发错误：物料 [{materialCode}] 余额在校验后被扣减，操作失败。");
            if(sortedLoadings.Any()) await _loadingRepo.UpdateWithOptLockAsync(sortedLoadings);
            if(consumpLogs.Any()) await _exeConsumpRepo.AddBulkAsync(consumpLogs);
            if(inventoryMap.Any()) await _inventoryRepo.UpdateBulkAsync(inventoryMap.Values.ToList());
        }


        /// <summary>
        /// ★ 重构：批量扣减物料逻辑 (支持一次性处理多个物料需求)
        /// </summary>
        private async Task DeductBatchMaterialAndRecordAsync(
            int stationId,
            Dictionary<string, decimal> requirements, // 接收所有需求
            int exeLogId,
            string employeeId,
            string movementType,
            string? movementReason)
        {
            if (!requirements.Any()) return;

            // 1. 获取机台所有活跃上料
            var allActiveLoadings = await _loadingRepo.GetListByStationIdAsync(stationId, status: MaterialLoadingStatus.Active);

            // 2. 收集涉及的 InventoryId (去重)
            var relatedInventoryIds = allActiveLoadings
                .Where(x => requirements.ContainsKey(x.MaterialCode) && x.LineSideInventoryId > 0)
                .Select(x => x.LineSideInventoryId)
                .Distinct()
                .ToList();

            // 3. 获取线边库存
            var inventories = await _inventoryRepo.GetByIdsAsync(relatedInventoryIds);
            var inventoryMap = inventories.ToDictionary(x => x.Id);

            var modifiedLoadings = new List<StationMaterialLoading>();
            var consumpLogs = new List<WorkOrderTaskExecuteConsump>();
            // 注意：库存对象是引用的，直接收集 Values 即可

            // 4. 遍历需求，执行内存扣减
            foreach (var req in requirements)
            {
                string matCode = req.Key;
                decimal remaining = req.Value;

                // 筛选该物料的批次并排序 (FIFO)
                var sortedLoadings = allActiveLoadings
                    .Where(x => x.MaterialCode == matCode && x.LastQuantity > 0)
                    .OrderBy(x => x.LoadedTime)
                    .ToList();

                foreach (var loading in sortedLoadings)
                {
                    if (remaining <= 0) break;

                    decimal currentDeduct = Math.Min(loading.LastQuantity, remaining);

                    // A. 更新上料记录
                    loading.LastQuantity -= currentDeduct;
                    if (loading.LastQuantity == 0) 
                    {
                        loading.Status = MaterialLoadingStatus.Depleted;
                        loading.UnloadedTime = DateTime.Now;
                    }
     

                    modifiedLoadings.Add(loading);

                    // B. 更新线边库存
                    if (loading.LineSideInventoryId > 0 && inventoryMap.TryGetValue(loading.LineSideInventoryId, out var inventory))
                    {
                        inventory.LastQuantity -= currentDeduct;
                        if (inventory.LastQuantity < 0) inventory.LastQuantity = 0;
                        if (inventory.LastQuantity == 0) inventory.Status = LineInventoryStatus.Available;
                        inventory.UpdateBy = employeeId;
                        inventory.UpdatedAt = DateTime.Now;
                    }

                    // C. 创建日志
                    consumpLogs.Add(new WorkOrderTaskExecuteConsump
                    {
                        ExeLogId = exeLogId,
                        MaterialCode = matCode,
                        BatchCode = loading.BatchCode,
                        BarCode = loading.BarCode,
                        ConsumedQuantity = currentDeduct,
                        BaseUnit = loading.BaseUnit,
                        MovementType = movementType,
                        MovementReason = movementReason,
                        Status = "1",
                        CreatedBy = employeeId,
                        CreatedOn = DateTime.Now
                    });

                    remaining -= currentDeduct;
                }

                if (remaining > 0)
                    throw new Exception($"并发错误：物料 [{matCode}] 余额不足 (Pre-Check Passed but Deduction Failed)");
            }

            // 5. 批量提交
            if (modifiedLoadings.Any())
                await _loadingRepo.UpdateWithOptLockAsync(modifiedLoadings); // 需确保去重，或仓储层处理

            if (consumpLogs.Any())
                await _exeConsumpRepo.AddBulkAsync(consumpLogs);

            if (inventoryMap.Any())
                await _inventoryRepo.UpdateBulkAsync(inventoryMap.Values.ToList());
        }

        #endregion


        #region 4. 打印

        public async Task<string> DetermineMaterialFlagToProcessLabelAsync(int orderId)
        {
            var boms = (await _bomRepo.GetListByOrderIdAync(orderId))
                .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true).ToList();

            if (!boms.Any() || boms.Where(x => x.ConsumeType != null && (new List<int> { 0, 2 }).Contains((int)x.ConsumeType)).Count() == 0)
                return "空";

            if (boms.Any(x => x.ConsumeType == 2))
            {
                if (boms.Any(x => x.MaterialCode.StartsWith('E') && x.ConsumeType == 2))
                    return "超";

                if (!boms.Any(x => x.ConsumeType == 2 && !x.MaterialCode.StartsWith('8')))
                {
                    var bagParams = await _parameterGroupRepo.GetGroupWithItemsAsync(Group_BagMaterialList);
                    var bagMaterials = bagParams.Items.FirstOrDefault()?.Value.Split(',').ToList() ?? new List<string>();
                    var bomMaterials = boms.Where(x => x.ConsumeType == 2).Select(x => x.MaterialCode).ToList();

                    if (!bomMaterials.Except(bagMaterials).Any())
                        return "袋";
                }
            }
            return string.Empty;
        }




        #endregion

    }
}
