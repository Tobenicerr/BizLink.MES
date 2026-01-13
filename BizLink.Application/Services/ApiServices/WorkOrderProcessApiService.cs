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
        private readonly IWorkOrderTaskConfirmService _workOrderTaskConfirmService;
        private readonly ISerialHelperService _serialHelperService;
        private readonly IMaterialViewService _materialViewService;
        private readonly IWorkOrderOperationConfirmService _confirmService;
        private readonly IWorkOrderOperationConsumpService _consumpService;
        private readonly IWorkOrderOperationConsumptionRecordService _consumpRecordService;
        private readonly IParameterGroupService _parameterGroupService;
        private readonly ISapRfcService _sapRfcService;

        private const string SAP_LOCATION_CODE = "2100";
        private const string PARAM_GROUP_SAP = "TransferSAP";
        private const string PARAM_KEY_ENABLED = "IsEnabled";

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
            ISapRfcService sapRfcService)
        {
            _workOrderService = workOrderService;
            _workOrderProcessService = workOrderProcessService;
            _workOrderTaskConfirmService = workOrderTaskConfirmService;
            _serialHelperService = serialHelperService;
            _materialViewService = materialViewService;
            _confirmService = confirmService;
            _consumpService = consumpService;
            _consumpRecordService = consumpRecordService;
            _parameterGroupService = parameterGroupService;
            _sapRfcService = sapRfcService;
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
            var currentProcess = await _workOrderProcessService.GetByIdAsync(processId);
            var processConfirms = await _confirmService.GetListByProcessIdAsync(processId);
            var processCompletedQty = processConfirms.Where(s => s.Status != "0").Sum(s => s.YieldQuantity ?? 0);
            decimal remainingOrderQty = (currentProcess.Quantity ?? 0) - processCompletedQty;
            if (remainingOrderQty <= 0)
                remainingOrderQty = 1; // 防止除零

            decimal ratio = (sapConfirm.YieldQuantity ?? 0) / remainingOrderQty;
            if (ratio > 1)
                ratio = 1;
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

                if (remainingOrderQty <= (sapConfirm.YieldQuantity ?? 0))
                {
                    // 【逻辑 1】完工时，强制消耗剩余所有库存 (清空库存)
                    consumeQty = totalAvailableStock;
                }
                else
                {
                    // 【逻辑 2】非完工时，按比例消耗剩余库存
                    consumeQty = totalAvailableStock * ratio;

                    // 【核心修改】全部向下取整 (舍弃小数)，不区分单位
                    if (consumeQty >= 1)
                    {
                        consumeQty = Math.Floor(consumeQty);
                    }

                    else if (consumeQty > 0 && consumeQty < 1)
                    {
                        //如果小于1大于0，则判断总投入量是否为整数，是则消耗1,否则消耗原数量
                        if (totalIn % 1 == 0)
                        {
                            consumeQty = 1;
                        }
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
                    FromLocationCode = SAP_LOCATION_CODE,
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

            var isSuccess = result.MessageType?.ToUpper() == "S" ||
                            (result.Message?.Contains("该报工盘号已处理完成") ?? false);

            // 更新状态
            await _confirmService.UpdateAsync(new WorkOrderOperationConfirmUpdateDto
            {
                Id = confirmId,
                Message = result.Message?.Length > 100 ? result.Message.Substring(0, 100) : result.Message,
                MessageType = result.MessageType,
                Status = isSuccess ? "1" : "-1",
                UpdatedAt = DateTime.Now,
                UpdatedBy = employeeId // 记录更新人
            });

            if (isSuccess)
            {
                return "报工成功！";
            }
            else
            {
                throw new Exception(result.Message);
            }
        }


        #endregion
    }
}
