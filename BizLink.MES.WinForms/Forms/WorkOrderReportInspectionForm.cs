using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using Dm.util;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class WorkOrderReportInspectionForm : MesBaseForm
    {
        private readonly AssemblyModuleFacade _facade;
        private readonly IPendingOperationTaskService _opTaskService;
        private readonly IWorkCenterService _workCenterService;
        private readonly IFormFactory _formFactory;
        private readonly IWorkOrderStepTaskService _stepTaskService;
        private readonly IWorkTaskCategoryService _workTaskCategoryService;
        private readonly IWorkOrderTaskExecuteLogService _workOrderTaskExecuteLogService;
        private readonly ITaskExecutionService _taskExecutionService;
        private readonly IServiceScopeFactory _scopeFactory; // 【新增】用于创建临时事务 Scope


        private WorkOrderStepTaskDto? _currentWorkOrderStepTask = null;
        private WorkOrderDto? _currentWorkOrder = null;
        public WorkOrderReportInspectionForm(AssemblyModuleFacade facade, IPendingOperationTaskService taskService, IWorkCenterService workCenterService, IWorkOrderStepTaskService stepTaskService, IWorkTaskCategoryService workTaskCategoryService, ITaskExecutionService taskExecutionService, IFormFactory formFactory, IWorkOrderTaskExecuteLogService workOrderTaskExecuteLogService, IServiceScopeFactory scopeFactory)
        {
            InitializeComponent();
            InitializeTable();
            _facade = facade;
            _opTaskService = taskService;
            _workCenterService = workCenterService;
            _stepTaskService = stepTaskService;
            _formFactory = formFactory;
            _workTaskCategoryService = workTaskCategoryService;
            _workOrderTaskExecuteLogService = workOrderTaskExecuteLogService;
            _taskExecutionService = taskExecutionService;
            _scopeFactory = scopeFactory;
        }


        private void InitializeTable()
        {
            OrderTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("WorkOrderNo", "订单/日期", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is PendingOperationTaskDto model)
                        {
                            return model.WorkOrderNo + "\r\n" + model.DispatchDate?.ToString("yyyy-MM-dd");
                        }
                        return value;
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Quantity", "数量", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is PendingOperationTaskDto model)
                        {
                            return $"订单数量：{(model.Quantity ?? 0).ToString("F0")}\r\n完成数量：{(model.CompletedQuantity ?? 0).ToString("F0")}";
                        }
                        return value;
                    }
                }.SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is PendingOperationTaskDto model)
                        {
                            if (model.TaskId == null)
                                return new AntdUI.CellTag("未同步", AntdUI.TTypeMini.Error);
                            if((model.PrevCompletedQuantity ?? 0) <= 0)
                                return new AntdUI.CellTag("未准备", AntdUI.TTypeMini.Error);
                            if((model.PrevCompletedQuantity ?? 0) > 0  && (model.CompletedQuantity ?? 0) <= 0)
                                return new AntdUI.CellTag("已排产", AntdUI.TTypeMini.Primary);
                            if((model.CompletedQuantity ?? 0) > 0)
                            {
                              if((model.CompletedQuantity ?? 0) < model.Quantity)
                                  return new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Default);
                              else
                                  return new AntdUI.CellTag("已完成", AntdUI.TTypeMini.Success);
                            }
                        }
                        return null;
                    }
                }.SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialCode", "物料信息", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is PendingOperationTaskDto model)
                        {
                            return $"订单物料：{model.MaterialCode}\r\n{model.MaterialDesc}\r\n成品物料：{model.LeadingOrderMaterial}";
                        }
                        return value;
                    }
                }.SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("WorkCenter", "工作中心/BU", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is PendingOperationTaskDto model)
                        {
                            return model.WorkCenter + "\r\n" + model.ProfitCenter;
                        }
                        return value;
                    }
                }.SetLocalizationTitleID("Table.Column.")
            };

            ConfTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("BatchCode", "报工序号", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CompletedQuantity", "报工数量", AntdUI.ColumnAlign.Center).SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("EmployerCode", "报工人员", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CreatedOn", "报工时间", AntdUI.ColumnAlign.Left).SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value?.ToString() switch
                        {
                            "50" => new AntdUI.CellTag("完成", AntdUI.TTypeMini.Success),
                            _    => null,
                        };
                    }
                }.SetLocalizationTitleID("Table.Column.")
            };
        }


        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DispathchDatePickerRange.PlaceholderStart = "请选择开始日期...";
            DispathchDatePickerRange.PlaceholderEnd = "请选择结束日期...";
            OrderScanInput.PlaceholderText = "请扫描或输入订单号...";
            WorkcenterGroupSelect.PlaceholderText = "请选择工作中心组...";
            WorkcenterSelect.PlaceholderText = "请选择工作中心...";

            ToggleInputs(true);
            await LoadWorkCenterGroupAsync();
        }


        public async Task<int> LoadOrderDataAsync()
        {
            OrderTable.DataSource = null;

            if (DispathchDatePickerRange.Value == null)
                throw new Exception("请先选择订单排产日期！");
            var dateStart = DispathchDatePickerRange.Value.FirstOrDefault();
            var dateEnd = DispathchDatePickerRange.Value.LastOrDefault();
            if (dateStart == null || dateEnd == null)
                throw new Exception("请先选择订单排产日期！");

            var workcenterGroup = (MenuItem)WorkcenterGroupSelect.SelectedValue;
            int GroupId = 0;
            if (workcenterGroup == null || !int.TryParse(workcenterGroup.Name, out GroupId))
                throw new Exception("请先选择工作中心组！");

            int workcenterId = 0;
            var workcenter = (MenuItem)WorkcenterSelect.SelectedValue;
            if (workcenter == null || !int.TryParse(workcenter.Name, out workcenterId))
                throw new Exception("请先选择工作中心！");
            var workcenters = await _workCenterService.GetBygroupIdAsync(GroupId);
            var result = await _opTaskService.GetListByWorkCentersAsync(
                AppSession.CurrentFactoryId,
                workcenters.Select(x => x.WorkCenterCode).Distinct().ToList(),
                dateStart,
                dateEnd);

            if (result != null && result.Any())
            {
                OrderTable.DataSource = result.OrderBy(x => x.WorkOrderNo).ThenByDescending(x => x.DispatchDate).ToList();
                return result.Count;
            }
            else
            {
                throw new Exception("未查询到待办订单信息！");
            }
        }

        private async Task QueryExecuteLogsAsync()
        {
            ConfTable.DataSource = null;
            if (_currentWorkOrderStepTask != null)
            {
                var result = await _workOrderTaskExecuteLogService.GetListByTaskIdAsync(_currentWorkOrderStepTask.Id, TaskLevel.Step);
                if (result != null && result.Any())
                {
                    ConfTable.DataSource = result.OrderByDescending(x => x.Id).ToList();
                }

            }
        }


        private async Task LoadWorkCenterGroupAsync()
        {
            var workcentergroup = await _facade.WorkCenterGroup.GetListByGroupTypeAsync(AppSession.CurrentFactoryId, ((int)WorkCenterGroupType.InspectionGroup).ToString());
            if (workcentergroup != null)
            {
                WorkcenterGroupSelect.Items.Clear();
                WorkcenterGroupSelect.Items.AddRange(workcentergroup.Select(g => new MenuItem()
                {
                    Name = g.Id.ToString(),
                    Text = $"{g.GroupCode}-{g.GroupName}"
                }).ToArray());

            }
        }

        private async Task LoadWorkCenterAsync(int groupid)
        {
            var workcenters = await _facade.WorkCenter.GetBygroupIdAsync(groupid);
            if (workcenters != null)
            {
                WorkcenterSelect.Items.Clear();
                WorkcenterSelect.Items.AddRange(workcenters.Select(w => new MenuItem()
                {
                    Name = w.Id.ToString(),
                    Text = $"{w.WorkCenterCode}-{w.WorkCenterName}"
                }).ToArray());
            }
        }


        private async void WorkcenterGroupSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            if (e.Value is MenuItem item && int.TryParse(item.Name, out int groupid))
            {
                await RunAsync(async () =>
                {
                    await LoadWorkCenterAsync(groupid);
                });
            }
        }

        private void WorkcenterSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            WorkcenterSelect.Select(0, 0);
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {

            await RunAsync(SearchButton, async () =>
            {

                if (SearchButton.Text.Contains("工位确认"))
                {
 
                    var count = await LoadOrderDataAsync();
                    AntdUI.Message.success(this.ParentForm, $"查询成功：共查询出{count}条记录！");

                    ToggleInputs(false);
                }
                else
                    ToggleInputs(true);


            });
        }


        private void SetLabel(AntdUI.Label lbl, string? value)
        {
            string prefix = lbl.Text.Contains("：") ? lbl.Text.Split('：')[0] : lbl.Text;
            lbl.Text = $"{prefix}：{value ?? "-"}";
        }
        private async void OrderScanInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r')
                return;
            var orderNo = OrderScanInput.Text.Trim();
            if (string.IsNullOrEmpty(orderNo))
                return;
            await RunAsync(async () =>
            {
                //var processId = 0;
                var item = MoveOrderToTop(orderNo);
                if (item == null)
                    throw new Exception($"未找到订单号 {orderNo} 的信息！");
                if(item.TaskId == null)
                    throw new Exception($"订单号 {orderNo} 同步异常，请重新同步SAP订单！");

                var workorderProcess = await _facade.WorkOrderProcessService.GetByIdAsync(item.WorkOrderProcessId);
                var workorder = await _facade.WorkOrderService.GetByIdAsync(item.WorkOrderId);
                _currentWorkOrder = workorder;
                var workorderProcesses = await _facade.WorkOrderProcessService.GetListByOrderIdAync(item.WorkOrderId);

                var lastProcess = workorderProcesses.Where(x => x.Id < item.WorkOrderProcessId).OrderByDescending(x => x.Id).FirstOrDefault();
                if (lastProcess != null && lastProcess.CompletedQuantity <= 0) 
                {
                    AntdUI.Modal.open(new AntdUI.Modal.Config(this.ParentForm, "错误", "订单前道工序未报工，本道不允许开工！", AntdUI.TType.Error));
                     //return;
                }

                //获取当前工序任务下的工步任务
                
                var inspectCategory = await _workTaskCategoryService.GetByCategoryCodeAsync(TaskCategories.InspectionEnd);

                var stepTasks = await _stepTaskService.GetListByOperationIdAsync((int)item.TaskId);
                if (stepTasks.Any(x => x.TaskCategory == inspectCategory.CategoryCode))
                    _currentWorkOrderStepTask = stepTasks.Where(x => x.TaskCategory == inspectCategory.CategoryCode).First();
                else 
                {
                    var existingMaxCode = 0;
                    var codes = stepTasks.Select(s => int.TryParse(s.StepCode, out int c) ? c : 0);
                    if (codes.Any()) existingMaxCode = codes.Max();
                    _currentWorkOrderStepTask = await _stepTaskService.CreateAsync(new WorkOrderStepTaskCreateDto()
                    {
                        OperationTaskId = item.TaskId,
                        WorkCenterGroupId = int.Parse(((MenuItem)WorkcenterGroupSelect.SelectedValue).Name),
                        WorkCenterGroupCode = ((MenuItem)WorkcenterGroupSelect.SelectedValue).Text.Split("-")[0],
                        ActualWorkCenterId = int.Parse(((MenuItem)WorkcenterSelect.SelectedValue).Name),
                        ActualWorkCenterCode = ((MenuItem)WorkcenterSelect.SelectedValue).Text.Split("-")[0],
                        ActualStartTime = DateTime.Now,
                        Quantity = workorderProcess.Quantity,
                        CompletedQuantity = 0,
                        StepCode = (existingMaxCode + 10).ToString(),
                        Status = WorkTaskStatus.New,
                        TaskCategory = inspectCategory.CategoryCode,
                        CreatedBy = AppSession.CurrentUser.EmployeeId,
                    });

                    if (workorderProcess.ActStartTime == null) 
                    {
                        await _facade.WorkOrderProcessService.UpdateAsync(new WorkOrderProcessUpdateDto()
                        {
                            Id = workorderProcess.Id,
                            ActStartTime = _currentWorkOrderStepTask.ActualStartTime
                        });
                    }
  
                }

                await QueryExecuteLogsAsync();
                SubmitButton.Enabled = !(_currentWorkOrderStepTask.Quantity > 0 && _currentWorkOrderStepTask.Quantity == _currentWorkOrderStepTask.CompletedQuantity);

                SetLabel(Orderlabel, workorder.OrderNumber);
                SetLabel(Matlabel, workorder.MaterialCode);
                SetLabel(Desclabel, workorder.MaterialDesc);
                SetLabel(LeadMatlabel, workorder.LeadingOrderMaterial);
                SetLabel(confLabel, $"{_currentWorkOrderStepTask.CompletedQuantity ?? 0:F0}/{_currentWorkOrderStepTask.Quantity ?? 1:F0}");

                // 安全计算进度条 (防止 Quantity 为 0 导致除零错误)
                decimal safeQty = (_currentWorkOrderStepTask.Quantity ?? 1) == 0 ? 1 : (_currentWorkOrderStepTask.Quantity ?? 1);
                progressControl.Value = (float)((_currentWorkOrderStepTask.CompletedQuantity ?? 0) / safeQty);

                if (progressControl.Value == 0)
                {
                    progressControl.State = TType.Error;
                }

            });

        }

        /// <summary>
        /// 功能：扫码置顶
        /// Table 的置顶非常简单，只需要操作数据源 List，然后重新赋值 DataSource 即可
        /// </summary>
        public PendingOperationTaskDto? MoveOrderToTop(string orderNo)
        {
            var orderDatas = OrderTable.DataSource as List<PendingOperationTaskDto>;
            var target = orderDatas.FirstOrDefault(x => x.WorkOrderNo == orderNo);
            if (target != null)
            {
                // 1. 修改数据源顺序
                orderDatas.Remove(target);
                orderDatas.Insert(0, target);

                // 2. 刷新表格 (AntdUI Table 重新赋值 DataSource 会触发重绘，非常快)
                OrderTable.DataSource = null; // 有时候需要重置一下触发刷新，视版本而定
                OrderTable.DataSource = orderDatas;

                // 3. 选中第一行并滚动
                OrderTable.SelectedIndex = 1;
                OrderTable.ScrollLine(1, true);
            }
            return target;
        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SubmitButton, async () =>
            {
                var task = await _stepTaskService.GetByIdAsync(_currentWorkOrderStepTask.Id);

                if (int.TryParse(task.Status, out int status))
                {
                    if (status >= int.Parse(WorkTaskStatus.Completed))
                        throw new Exception("该订单工序已完成，无法报工！");
                }
                if (ConfInputNumber.Value + task.CompletedQuantity > task.Quantity)
                {
                    throw new Exception("报工数量超出订单数量，无法报工！");
                }

                await _taskExecutionService.ReportStepProductionAsync(_currentWorkOrderStepTask.Id, ConfInputNumber.Value, AppSession.CurrentUser.EmployeeId);

                await QueryExecuteLogsAsync();
                await LoadOrderDataAsync();
                _currentWorkOrderStepTask = await _stepTaskService.GetByIdAsync(_currentWorkOrderStepTask.Id);

                SetLabel(confLabel, $"{(_currentWorkOrderStepTask.CompletedQuantity ?? 0):F0}/{(_currentWorkOrderStepTask.Quantity ?? 1):F0}");

                // 安全计算进度条 (防止 Quantity 为 0 导致除零错误)
                progressControl.Value = (float)((_currentWorkOrderStepTask.CompletedQuantity ?? 0) / (_currentWorkOrderStepTask.Quantity ?? 1));

                if (progressControl.Value == 0)
                    progressControl.State = TType.Error;
                else
                    progressControl.State = TType.Success;

                ConfInputNumber.Value = 0;
                ConfInputNumber.Text = string.Empty;
                ScrapInputNumber.Value = 0;
                ScrapInputNumber.Text = string.Empty;
                if (_currentWorkOrderStepTask.Quantity <= _currentWorkOrderStepTask.CompletedQuantity)
                {
                    SubmitButton.Enabled = false;
                    _currentWorkOrder = null;
                    _currentWorkOrderStepTask = null;
                }
                else 
                {
                    MoveOrderToTop(_currentWorkOrder.OrderNumber);
                }

            }, confirmMsg: $"即将对订单{_currentWorkOrder.OrderNumber}进行报工，是否继续？", successMsg: "报工成功！");
        }

        private void ToggleInputs(bool enable)
        {
            DispathchDatePickerRange.Enabled = enable;
            WorkcenterGroupSelect.Enabled = enable;
            WorkcenterSelect.Enabled = enable;

            if (!enable)
            {
                SearchButton.Text = "解除确认";
                SearchButton.Type = AntdUI.TTypeMini.Error;
                OrderScanInput.Enabled = true;
            }
            else
            {
                SubmitButton.Enabled = false;
                SearchButton.Text = "工位确认";
                SearchButton.Type = AntdUI.TTypeMini.Primary;
                OrderScanInput.Enabled = false;
            }
        }

    }
}
