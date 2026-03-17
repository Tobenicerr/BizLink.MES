using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Helper;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using Dm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BizLink.MES.Application.Services
{
    internal class WorkOrderProcessApiService : IWorkOrderProcessApiService
    {
        private readonly IWorkOrderService _workOrderService;
        private readonly IWorkOrderProcessService _workOrderProcessService;
        private readonly IWorkOrderBomItemService _workOrderBomItemService;
        private readonly IWorkOrderTaskConfirmService _workOrderTaskConfirmService;
        private readonly ISerialHelperService _serialHelperService;
        private readonly IMaterialViewService _materialViewService;
        private readonly IWorkOrderOperationConfirmService _confirmService;
        private readonly IWorkOrderOperationConsumpService _consumpService;
        private readonly IWorkOrderOperationConsumptionRecordService _consumpRecordService;
        private readonly IParameterGroupService _parameterGroupService;
        private readonly ISapRfcService _sapRfcService;
        private readonly IFinishedGoodsReceiptService _finishedGoodsReceiptService;

        private const string PARAM_GROUP_SAP = "TransferSAP";
        private const string PARAM_KEY_ENABLED = "IsEnabled";
        private const string SapParamGroup = "CN11SAPStockLocation";
        private const string SapLineStockKey = "SAPLineStock";

        public WorkOrderProcessApiService(
            IWorkOrderService workOrderService,
            IWorkOrderProcessService workOrderProcessService,
            IWorkOrderTaskConfirmService workOrderTaskConfirmService,
            ISerialHelperService serialHelperService,
            IMaterialViewService materialViewService,
            IWorkOrderOperationConfirmService confirmService,
            IWorkOrderOperationConsumpService consumpService,
            IWorkOrderOperationConsumptionRecordService consumpRecordService,
            IParameterGroupService parameterGroupService,
            ISapRfcService sapRfcService, IWorkOrderBomItemService workOrderBomItemService, IFinishedGoodsReceiptService finishedGoodsReceiptService)
        {
            _workOrderService = workOrderService;
            _workOrderProcessService = workOrderProcessService;
            _workOrderTaskConfirmService = workOrderTaskConfirmService;
            _serialHelperService = serialHelperService;
            _materialViewService = materialViewService;
            _confirmService = confirmService;
            _consumpService = consumpService;
            //_consumpRecordService = consumpRecordService;
            _parameterGroupService = parameterGroupService;
            _sapRfcService = sapRfcService;
            _workOrderBomItemService = workOrderBomItemService;
            _finishedGoodsReceiptService = finishedGoodsReceiptService;
        }
        public async Task<string> ReportWorkOrderOperationToSapAsync(WorkOrderReportRequest request)
        {
            // 1. 基础数据校验与获取
            var process = await _workOrderProcessService.GetByIdAsync(request.ProcessId)
                          ?? throw new Exception("未查询到工序信息");



            var order = await _workOrderService.GetByIdAsync((int)process.WorkOrderId)
                        ?? throw new Exception("未查询到工单信息");

            var material = await _materialViewService.GetByCodeAsync(request.FactoryCode, order.MaterialCode)
                           ?? throw new Exception("未查询到物料信息");

            // 2. 准备报工数据 (提取逻辑)
            var reportData = await PrepareReportDataAsync(request, process, order);

            var confirmLogs = await _confirmService.GetListByProcessIdAsync(request.ProcessId);
            if (confirmLogs != null && confirmLogs.Count() > 0)
            {
                var confirmedQty = confirmLogs.Sum(x => x.YieldQuantity).Value;
                if (process.Quantity < confirmedQty + reportData.YieldQuantity)
                    throw new Exception("确认数量超出计划数量，无法报工！");

                if (request.ConfirmId != null && confirmLogs.Exists(c => c.TaskConfirmId == request.ConfirmId))
                    throw new Exception("当前报工已存在SAP报工记录，无需重复报工！");
            }


            //if (process.Quantity < process.CompletedQuantity + reportData.YieldQuantity)
            //    throw new Exception("确认数量超出计划数量，无法报工！");

            // 3. 创建本地报工记录
            var sapConfirm = await CreateLocalConfirmationAsync(request, order, process, material, reportData);

            // 4. 创建消耗记录
            await CreateConsumptionRecordsAsync(request.FactoryCode,request.EmployeeId, process.Id, sapConfirm, order);

            // 5. 调用 SAP 接口 (如果启用)
            if (await IsSapTransferEnabledAsync())
            {
                return await ExecuteSapRfcCallAsync(sapConfirm.Id, request.EmployeeId);
            }

            return "报工成功 (SAP接口未启用)";
        }

        public async Task<string> ReSendConfirmationToSapAsync(int confirmId)
        {
            if (!await IsSapTransferEnabledAsync())
                throw new Exception("参数配置未启用推送SAP功能！");

            var confirm = await _confirmService.GetConfirmWitemConsumeptionAsync(confirmId)
                          ?? throw new Exception("未查询到报工信息，无法重新推送！");



            if (confirm.Consumps != null && confirm.Consumps.Count() > 0)
            {
                await _consumpService.UpdateAsync(confirm.Consumps.Select(x => new WorkOrderOperationConsumpUpdateDto()
                {
                    Id = x.Id,
                    Status = "0",
                    UpdatedBy = confirm.UpdatedBy,
                    UpdatedAt = DateTime.Now
                }).ToList());
            }
            var workorder = await _workOrderService.GetByIdAsync((int)confirm.WorkOrderId);
            await CreateConsumptionRecordsAsync(confirm.FactoryCode, confirm.CreatedBy, (int)confirm.ProcessId, confirm, workorder);
            confirm = await _confirmService.GetConfirmWitemConsumeptionAsync(confirmId);
            //获取最新的bom
            var saporder = await _sapRfcService.GetWorkOrdersAsync(confirm.FactoryCode, null, new List<string>() { confirm.WorkOrderNo });
            if (saporder != null && saporder.sapOrderBoms != null && saporder.sapOrderBoms.Count() > 0)
            {
                var keysInB = saporder.sapOrderBoms.Select(x => (x.ReservationItem, x.MaterialCode)).ToHashSet();

                // 步骤 2: 查找在 List A 中，但 Key 不在 List B 中的元素
                var notInB = confirm.Consumps.Where(x => !keysInB.Contains((x.ReservationItem, x.MaterialCode.TrimStart('0')))).ToList();

                await _consumpService.UpdateAsync(notInB.Select(x => new WorkOrderOperationConsumpUpdateDto()
                {
                    Id = x.Id,
                    Status = "-1"
                }).ToList());

            }


            return await ExecuteSapRfcCallAsync(confirm.Id, confirm.UpdatedBy ?? confirm.CreatedBy);
        }

        #region Private Helper Methods

        private async Task<(string CompletedFlag, string ActFinishDate, string ActFinishTime, decimal? YieldQuantity)>
            PrepareReportDataAsync(WorkOrderReportRequest request, WorkOrderProcessDto process, WorkOrderDto order)
        {
            if (request.ConfirmId == null || request.ConfirmId == 0)
            {
                // 全部报工模式
                var isFinished = process.Status == ((int)WorkOrderStatus.Finished).ToString();
                return (
                    isFinished ? "X" : "",
                    process.ActEndTime?.ToString("yyyyMMdd") ?? DateTime.Now.ToString("yyyyMMdd"),
                    process.ActEndTime?.ToString("HH:mm:ss") ?? DateTime.Now.ToString("HH:mm:ss"),
                    order.Quantity
                );
            }
            else
            {
                // 指定 MES 报工记录模式
                var mesConfirm = await _workOrderTaskConfirmService.GetByIdAsync((int)request.ConfirmId)
                                 ?? throw new Exception($"未查询订单{order.OrderNumber}工序{process.Operation}的相关完工报工记录");


                return (
                    mesConfirm.Status == "1" ? "X" : "",
                    mesConfirm.ConfirmDate?.ToString("yyyyMMdd"),
                    mesConfirm.ConfirmDate?.ToString("HH:mm:ss"),
                    mesConfirm.ConfirmQuantity
                );
            }
        }

        private async Task<WorkOrderOperationConfirmDto> CreateLocalConfirmationAsync(
            WorkOrderReportRequest request, WorkOrderDto order, WorkOrderProcessDto process,
            MaterialViewDto material, (string? Flag, string? Date, string? Time, decimal? Qty) data)
        {
            await _workOrderProcessService.UpdateAsync(new WorkOrderProcessUpdateDto()
            {
                Id = process.Id,
                CompletedQuantity = process.CompletedQuantity + data.Qty,
                Status = data.Flag == "X" ? ((int)WorkOrderStatus.Finished).ToString() : process.Status,
                ActStartTime = process.ActStartTime == null ? DateTime.Now: process.ActStartTime,
                ActEndTime = data.Flag == "X" ? DateTime.Now : null
            });
            return await _confirmService.CreateAsync(new WorkOrderOperationConfirmCreateDto
            {
                WorkOrderId = order.Id,
                ProcessId = process.Id,
                TaskConfirmId = request.ConfirmId,
                SapConfirmationNo = process.ConfirmNo,
                WorkOrderNo = order.OrderNumber.PadLeft(12, '0'),
                OperationNo = process.Operation,
                CompletedFlag = data.Flag,
                ConfirmSequence = _serialHelperService.GenerateNext($"{request.FactoryCode}ConfirmReportToSAP"),
                WorkCenterCode = process.WorkCenter,
                PostingDate = DateTime.Now.Date,
                EmployeeId = request.EmployeeId,
                FactoryCode = request.FactoryCode,
                BaseUnit = material.BaseUnit,
                YieldQuantity = data.Qty,
                ScrapQuantity = 0,
                ActFinishDate = data.Date,
                ActFinishTime = data.Time,
                CreatedBy = request.EmployeeId,
            });
        }

        private async Task CreateConsumptionRecordsAsync(string factoryCode,string employeeId, int processId, WorkOrderOperationConfirmDto sapConfirm, WorkOrderDto order)
        {
            var rawRecords = await _consumpRecordService.GetListByProcessIdAsync(processId);
            if (!rawRecords.Any())
                return;
            var boms = await _workOrderBomItemService.GetListByProcessIdsAsync(new List<int>() { processId });
            if (boms != null && boms.Count() > 0) 
            {
                //var activeboms = boms.Where(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed).ToList();
                //rawRecords = rawRecords.Where(r => activeboms.Select(x => x.ReservationItem).Contains(r.ReservationItem) && activeboms.Select(x => x.MaterialCode).Contains(r.MaterialCode)).ToList();

                var activeBoms = boms.Where(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed).ToList();
                // 使用 HashSet 提高查找性能
                var activeResItems = activeBoms.Select(x => x.ReservationItem).ToHashSet();
                var activeMaterials = activeBoms.Select(x => x.MaterialCode).ToHashSet();

                rawRecords = rawRecords.Where(r => activeResItems.Contains(r.ReservationItem) && activeMaterials.Contains(r.MaterialCode)).ToList();
            }

            if (!rawRecords.Any()) return; // 再次检查
            var currentProcess = await _workOrderProcessService.GetByIdAsync(processId);
            var processConfirms = await _confirmService.GetListByProcessIdAsync(processId);
            var processCompletedQty = processConfirms.Where(s => s.Status != "0").Sum(s => s.YieldQuantity ?? 0);
            decimal remainingOrderQty = (currentProcess.Quantity ?? 0) - processCompletedQty;
            // --- 优化 2: 比例计算逻辑 ---
            decimal ratio = 0;
            bool isLastStepOrOverProduction = false;
            // 如果剩余数量 <= 本次报工数量，视为最后一次报工或超产，需清空线边库
            if (remainingOrderQty <= (sapConfirm.YieldQuantity ?? 0))
            {
                ratio = 1;
                isLastStepOrOverProduction = true;
            }
            else
            {
                // remainingOrderQty 肯定 > 0
                ratio = (sapConfirm.YieldQuantity ?? 0) / remainingOrderQty;
                if (ratio > 1) ratio = 1; // 双重保险
            }
            if (remainingOrderQty <= 0)
                remainingOrderQty = 1; // 防止除零

            //decimal ratio = (sapConfirm.YieldQuantity ?? 0) / remainingOrderQty;
            //if (ratio > 1)
            //    ratio = 1;
            //获取当前工序所有的消耗记录
            var existsConsumptions = await _consumpService.GetListByProcessIdAsync(processId);

            var consumptionList = new List<WorkOrderOperationConsumpCreateDto>();
            // 4. 按物料/批次分组处理
            var groupedRecords = rawRecords.GroupBy(g => new { g.ReservationItem, g.MaterialCode, g.BatchCode, g.BaseUnit, g.ConsumptionType, g.ConsumptionRemark });
            foreach (var group in groupedRecords)
            {
                // A. 计算该物料/批次的总投入量 (Total In)
                decimal totalIn = group.Sum(x => (decimal)x.Quantity);

                // B. 计算该物料/批次的已消耗量 (Total Out)
                decimal totalOut = 0;
                if (existsConsumptions != null && existsConsumptions.Count() > 0)
                {
                    totalOut = existsConsumptions
                        .Where(x => x.ReservationItem == group.Key.ReservationItem
                                 && x.MaterialCode.TrimStart('0') == group.Key.MaterialCode
                                 && x.BatchCode == group.Key.BatchCode
                                 && x.MovementType == group.Key.ConsumptionType.ToString()
                                 && x.MovementReason == (group.Key.ConsumptionRemark ?? "")
                                 )
                        .Sum(x => x.Quantity ?? 0);
                }

                // C. 计算剩余可用库存 (Remaining Stock)
                decimal totalAvailableStock = totalIn - totalOut;
                if (totalAvailableStock < 0)
                    totalAvailableStock = 0;

                decimal consumeQty = 0;

                // --- 优化 3: 消耗量计算与精度控制 ---
                if (isLastStepOrOverProduction)
                {
                    // 逻辑 1: 完工/超产，清空库存
                    consumeQty = totalAvailableStock;
                }
                else
                {
                    // 【逻辑 2】非完工时，按比例消耗剩余库存
                    consumeQty = totalAvailableStock * ratio;

                    // ★ 核心修复：根据单位判断舍入规则 ★
                    if (IsDiscreteUnit(group.Key.BaseUnit)) // 需要你实现这个辅助方法，判断是否为 PC, EA, 个 等
                    {
                        // 离散单位：向下取整，或四舍五入，取决于业务要求。原代码是 Floor。
                        // 只有离散单位才应用 "如果<1且库存为整数则扣1" 的逻辑
                        consumeQty = Math.Floor(consumeQty);

                        // 原代码的特殊逻辑：库存是整数，算出来 < 1 (比如0.1)，强扣 1
                        // 只有离散单位这样操作才合理
                        if (consumeQty == 0 && consumeQty < 1 && totalIn % 1 == 0 && totalAvailableStock >= 1)
                        {
                            consumeQty = 1;
                        }
                    }
                    else
                    {
                        // 连续单位 (KG, M)：保留3位或4位小数，不要 Floor！
                        consumeQty = Math.Round(consumeQty, 3);
                    }

                }

                // --- 兜底约束：不能超过剩余库存 ---
                if (consumeQty > totalAvailableStock)
                {
                    consumeQty = totalAvailableStock;
                }

                // --- 兜底约束：不能小于0 ---
                if (consumeQty < 0)
                    consumeQty = 0;
                var locationGroup = await _parameterGroupService.GetGroupWithItemsAsync(SapParamGroup);
                var sapLineLocation = locationGroup.Items.Where(p => p.Key == SapLineStockKey).First().Value;
                if (string.IsNullOrEmpty(order.PlannerRemark) || !order.PlannerRemark.Contains(sapLineLocation))
                    sapLineLocation = "2100";



                // 创建消耗记录
                var consumDto = new WorkOrderOperationConsumpCreateDto
                {
                    OperationConfirmId = sapConfirm.Id,
                    SapConfirmationNo = sapConfirm.SapConfirmationNo,
                    WorkOrderNo = order.OrderNumber.PadLeft(12, '0'),
                    ConfirmSequence = sapConfirm.ConfirmSequence,
                    ReservationNo = order.ReservationNo,
                    ReservationItem = group.Key.ReservationItem,
                    MaterialCode = group.Key.MaterialCode.StartsWith("E")
                                   ? group.Key.MaterialCode
                                   : group.Key.MaterialCode.PadLeft(18, '0'),
                    FactoryCode = factoryCode,
                    FromLocationCode = sapLineLocation,
                    BatchCode = group.Key.BatchCode,
                    MovementType = ((int)group.Key.ConsumptionType).ToString(),
                    MovementReason = group.Key.ConsumptionRemark ?? "",
                    Quantity = consumeQty,
                    BaseUnit = group.Key.BaseUnit,
                    CreatedBy = employeeId,
                };

                consumptionList.Add(consumDto);
            }
                //var groupedRecords = rawRecords
                //.GroupBy(g => new { g.ReservationItem, g.MaterialCode, g.BatchCode, g.BaseUnit, g.ConsumptionType, g.ConsumptionRemark })
                //.Select(s => new WorkOrderOperationConsumpCreateDto
                //{
                //    OperationConfirmId = sapConfirm.Id,
                //    SapConfirmationNo = sapConfirm.SapConfirmationNo,
                //    WorkOrderNo = order.OrderNumber.PadLeft(12, '0'),
                //    ConfirmSequence = sapConfirm.ConfirmSequence,
                //    ReservationNo = order.ReservationNo,
                //    ReservationItem = s.Key.ReservationItem,
                //    MaterialCode = s.Key.MaterialCode.StartsWith("E") ? s.Key.MaterialCode : s.Key.MaterialCode.PadLeft(18, '0'),
                //    FactoryCode = request.FactoryCode,
                //    FromLocationCode = SAP_LOCATION_CODE,
                //    BatchCode = s.Key.BatchCode,
                //    MovementType = ((int)s.Key.ConsumptionType).ToString(),
                //    MovementReason = s.Key.ConsumptionRemark ?? "",
                //    Quantity = s.Sum(ss => ss.Quantity),
                //    BaseUnit = s.Key.BaseUnit,
                //    CreatedBy = request.EmployeeId,
                //})
                //.ToList();

            if (consumptionList.Any())
            {
                await _consumpService.CreateAsync(consumptionList);
            }
        }

        /// <summary>
        /// ★ 批量创建消耗记录 (支持多条 Confirm 一次性运算)
        /// 融合了 Left Join 逻辑和内存滚动计算，彻底消除并发导致的多扣/少扣问题
        /// </summary>
        private bool IsDiscreteUnit(string unit)
        {
            var discreteUnits = new[] { "PC", "EA", "UNT", "个", "PCS","ST" }; // 根据实际业务配置
            return discreteUnits.Contains(unit?.ToUpper());
        }
        private async Task<bool> IsSapTransferEnabledAsync()
        {
            var group = await _parameterGroupService.GetGroupWithItemsAsync(PARAM_GROUP_SAP);
            var item = group?.Items.FirstOrDefault(x => x.Key == PARAM_KEY_ENABLED);
            // 默认启用 (如果未配置或配置为 true)
            return item == null || bool.Parse(item.Value);
        }

        private async Task<string> ExecuteSapRfcCallAsync(int confirmId, string employeeId)
        {
            var result = await _sapRfcService.ConfirmOrderCompletionToSAPAsync(confirmId);

            if (result == null)
                throw new Exception("调用SAP接口失败，返回结果为空");

            if (result.MessageType?.ToUpper() == "S")
            {
                return "报工成功！";
            }
            else
            {
                throw new Exception(result.Message);
            }
        }

        public async Task<(bool, string)> FinishedGoodsReceiptToSapAsync(int receiptId)
        {
            if (!await IsSapTransferEnabledAsync())
                throw new Exception("参数配置未启用推送SAP功能！");
            var receiptDto = await _finishedGoodsReceiptService.GetByIdAsync(receiptId);
            var result = await _sapRfcService.FinishedGoodsReceiptToSapAsync(receiptDto);
            if (result == null)
                throw new Exception("调用SAP接口失败，返回结果为空");
            else if (result.SapMessageType == "S")
                return (true, "入库成功");
            else
                return (false, result.SapMessage??"入库失败！");
        }


        #endregion
    }
}
