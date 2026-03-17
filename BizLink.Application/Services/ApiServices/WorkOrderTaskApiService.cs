using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Helper;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.WinForms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services.ApiServices
{
    public class WorkOrderTaskApiService : IWorkOrderTaskApiService
    {
        private readonly IWorkOrderTaskService _workOrderTaskService;
        private readonly IWorkCenterService _workCenterService;
        private readonly IWorkStationService _workStationService;
        private readonly IWorkOrderService _workOrderService;
        private readonly IWorkOrderProcessService _workOrderProcessService;
        private readonly IUserService _userService;
        private readonly ISerialHelperService _serialHelperService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWorkOrderTaskConfirmService _workOrderTaskConfirmService;
        private readonly IWorkOrderProcessApiService _workOrderProcessApiService;
        private readonly IFactoryService _factoryService;



        private const string InspectionWorkCenterGroup = "FQC";
        public WorkOrderTaskApiService(IWorkOrderTaskService workOrderTaskService, IWorkCenterService workCenterService, IWorkStationService workStationService, IWorkOrderService workOrderService, IWorkOrderProcessService workOrderProcessService, IUserService userService, ISerialHelperService serialHelperService, IUnitOfWork unitOfWork, IWorkOrderTaskConfirmService workOrderTaskConfirmService, IWorkOrderProcessApiService workOrderProcessApiService, IFactoryService factoryService) 
        {
            _workOrderTaskService = workOrderTaskService;
            _workCenterService = workCenterService;
            _workStationService = workStationService;
            _workOrderService = workOrderService;
            _workOrderProcessService = workOrderProcessService;
            _userService = userService;
            _serialHelperService = serialHelperService;
            _unitOfWork = unitOfWork;
            _workOrderTaskConfirmService = workOrderTaskConfirmService;
            _workOrderProcessApiService = workOrderProcessApiService;
            _factoryService = factoryService;
        }

        public async Task<WorkOrderTaskDto> CreateInspectionTaskByWorkOrderAsync(string userCode, string workOrderNo)
        {
            var User = await _userService.GetByEmployeeIdAsync(userCode);
            if (User == null)
                throw new Exception($"未查询到当前人员工号{userCode}，请检查工号是否正确！");
            var WorkOrder = await _workOrderService.GetByOrdrNoAsync(workOrderNo);
            if (WorkOrder == null || WorkOrder.Status == ((int)WorkOrderStatus.Deleted).ToString())
                throw new Exception($"当前订单号{workOrderNo}不存在或已被删除！");
            var WorkOrderProcesses = await _workOrderProcessService.GetListByOrderIdAync(WorkOrder.Id);
            if (WorkOrderProcesses == null)
                throw new Exception($"未查询到当前订单号{workOrderNo}的工序信息，请重新同步工序信息！");
            var WorkCenters = await _workCenterService.GetListByGroupCodeAsync(InspectionWorkCenterGroup);
            if (WorkCenters == null || WorkCenters.Count() == 0)
                throw new Exception("未配置质检工作中心，请联系IT进行配置！");
            if (!WorkOrderProcesses.Any(p => WorkCenters.Select(w => w.WorkCenterCode).ToList().Contains(p.WorkCenter)))
                throw new Exception($"未查询到当前订单号{workOrderNo}的质检工序，请检查订单工序！");
            var WorkStations = await _workStationService.GetAllAsync(WorkOrder.FactoryId);
            if (WorkStations == null || WorkStations.Count() == 0)
                throw new Exception("未查询到工位信息，请联系IT进行配置！");
            var endStation = WorkStations.Where(w => w.WorkAreaId == 2 && w.IsEndStep).FirstOrDefault();
            if (endStation == null)
                throw new Exception("未查询到当前工作中心结束工位，请联系IT进行配置！");

            var InspectionProcess = WorkOrderProcesses.Where(p => WorkCenters.Select(w => w.WorkCenterCode).ToList().Contains(p.WorkCenter)).OrderByDescending(p => p.Operation).First();

            var InspectionTasks = await _workOrderTaskService.GetByProcessIdsAsync(new List<int>() { InspectionProcess.Id });
            if (InspectionTasks != null && InspectionTasks.Any(i => i.WorkStationId == endStation.Id)) 
            {
                return InspectionTasks.Where(i => i.WorkStationId == endStation.Id).First();
            }
            return await _workOrderTaskService.CreateAsync(new WorkOrderTaskCreateDto()
            {
                OrderProcessId = InspectionProcess.Id,
                OrderId = WorkOrder.Id,
                OrderNumber = WorkOrder.OrderNumber,
                TaskNumber = $"{InspectionProcess.WorkOrderNo}-{InspectionProcess.Operation}",
                WorkStationId = endStation.Id,
                MaterialCode = WorkOrder.MaterialCode,
                MaterialDesc = WorkOrder.MaterialDesc,
                Status = ((int)WorkOrderStatus.New).ToString(),
                Quantity = WorkOrder.Quantity,
                CompletedQty = 0,
                NextWorkCenter = InspectionProcess.NextWorkCenter,
                Operation = InspectionProcess.Operation,
                ProfitCenter = WorkOrder.ProfitCenter,
                StartTime = InspectionProcess.StartTime,
                DispatchDate = InspectionProcess.EndTime,
                WorkCenter = InspectionProcess.WorkCenter,
                Remark = WorkOrder.PlannerRemark,
                CreateBy = User.EmployeeId,
            });


        }

        public async Task<string> ConfirmInspectionTaskAsync(string userCode, int taskId, decimal completedQty, bool InspectResult)
        {
            string message = "报工成功！";
            try
            {
                if (InspectResult)
                {
                    var taskDto = await _workOrderTaskService.GetByIdAsync(taskId);
                    if (taskDto == null)
                        throw new Exception("未查询到当前任务信息，请重试！");
                    var WorkOrder = await _workOrderService.GetByIdAsync(taskDto.OrderId);
                    if (WorkOrder == null)
                        throw new Exception("未查询到当前任务对应的订单信息！");
                    var factory = await _factoryService.GetByIdAsync(WorkOrder.FactoryId);
                    if (taskDto.Quantity <= taskDto.CompletedQty || taskDto.Status == ((int)WorkOrderStatus.Finished).ToString())
                        throw new Exception("当前任务已完成，无法继续报工！");
                    int taskConfirmId = 0;
                    //await _unitOfWork.BeginTransactionAsync();
                    //try
                    //{
                    //    var taskUpdateDto = new WorkOrderTaskUpdateDto
                    //    {
                    //        Id = taskDto.Id,
                    //        Status = completedQty + taskDto.CompletedQty >= taskDto.Quantity ? "4" : "2", // 4=Finished, 2=Processing
                    //        CompletedQty = completedQty + taskDto.CompletedQty,
                    //        UpdateBy = userCode,
                    //        UpdateOn = DateTime.Now,
                    //    };
                    //    await _workOrderTaskService.UpdateAsync(taskUpdateDto);
                    //    var taskConfirmDto = await _workOrderTaskConfirmService.CreateAsync(new WorkOrderTaskConfirmCreateDto
                    //    {
                    //        TaskId = taskDto.Id,
                    //        WorkStationId = (int)taskDto.WorkStationId,
                    //        ConfirmNumber = _serialHelperService.GenerateNext("ReportLabelSerial"),
                    //        ConfirmDate = DateTime.Now,
                    //        ConfirmQuantity = completedQty,
                    //        EmployerCode = userCode,
                    //        Status = taskUpdateDto.Status == ((int)WorkOrderStatus.Finished).ToString() ? "1" : "0",
                    //        //Remark = reportRemarkInput.Text.Trim(),
                    //    });
                    //    taskConfirmId = taskConfirmDto.Id;
                    //    await _unitOfWork.CommitAsync();

                    //}
                    //catch (Exception ex)
                    //{
                    //    try
                    //    {
                    //        await _unitOfWork.RollbackAsync();
                    //    }
                    //    catch
                    //    {
                    //    }
                    //    throw new Exception($"报工记录创建失败：{ex.Message}", ex);
                    //}

                    //message = await _workOrderProcessApiService.ReportWorkOrderOperationToSapAsync(new WorkOrderReportRequest()
                    //{
                    //    FactoryCode = factory.FactoryCode,
                    //    ProcessId = taskDto.OrderProcessId,
                    //    EmployeeId = userCode,
                    //    ConfirmId = taskConfirmId
                    //});
                }
                return message;

            }
            catch (Exception ex)
            {

                return ex.Message;
            }
           
        }
    }
}
