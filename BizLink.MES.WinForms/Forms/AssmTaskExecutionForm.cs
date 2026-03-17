using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using Dm.util;
using Microsoft.Data.SqlClient;
using SqlSugar;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class AssmTaskExecutionForm : MesBaseForm
    {
        private readonly AssemblyModuleFacade _facade;
        private readonly IPendingOperationTaskService _pendingTaskService;
        private readonly IWorkOrderOperationTaskService _opTaskService;
        private readonly IWorkOrderStepTaskService _stepTaskService;
        private readonly IWorkOrderKittingItemService _kittingService;
        private readonly ITaskExecutionService _taskExecutionService;

        private const string WorkStepType_Assm = "PROCESS";
        private const string WorkStepType_Inspe = "INSPECT";

        private PendingOperationTaskDto? _currentPendingTask = null;
        private List<string> _currentTaskCategory = new List<string>();

        public AssmTaskExecutionForm(AssemblyModuleFacade facade, IPendingOperationTaskService pendingTaskService, IWorkOrderKittingItemService kittingService, IWorkOrderOperationTaskService opTaskService, IWorkOrderStepTaskService stepTaskService, ITaskExecutionService taskExecutionService)
        {
            InitializeComponent();
            InitializeTable();
            _facade = facade;
            _pendingTaskService = pendingTaskService;
            _stepTaskService = stepTaskService;
            _opTaskService = opTaskService;
            _kittingService = kittingService;
            _taskExecutionService = taskExecutionService;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WorkCenterGroupSelect.PlaceholderText = "请选择工作中心组";

            TaskCategorySelect.PlaceholderText = "请选择工步";
            DispathchDatePickerRange.PlaceholderStart = "请选择开始日期...";
            DispathchDatePickerRange.PlaceholderEnd = "请选择结束日期...";

            WorkCenterSelect.PlaceholderText = "请选择工作中心";
            DispathchDatePickerRange.Value = new[] { DateTime.Now.AddDays(-7).Date, DateTime.Now.AddDays(3).Date };


            //ToggleInputs(false);
            await LoadWorkCenterGroupSelectAsync();
            await LoadTaskCategorySelectAsync();
            await LoadUserAsync();
            OrderNoLabel.Text = string.Empty;
            OpLabel.Text = string.Empty;
            MatcLabel.Text = string.Empty;
            MatdLabel.Text = string.Empty;
            ProgressLabel.Text = string.Empty;
            ReportInputNumber.Value = 0;
            ReportInputNumber.Text = string.Empty;
            ScrapLenInputNumber.Value = 0;
            ScrapLenInputNumber.Text = string.Empty;
            SubmitButton.Enabled = false;

        }

        private async Task LoadUserAsync()
        {
            EmpSelect.Items.Clear();
            var users = await _facade.UserService.GetListByFactoryIdAsync(AppSession.CurrentFactoryId);
            if (users.Any())
            {
                //var index = users.OrderBy(x => x.EmployeeId).ToList().FindIndex(x => x.EmployeeId == AppSession.CurrentUser.EmployeeId);
                EmpSelect.Items.AddRange(users.OrderBy(x => x.EmployeeId).Select(g => new MenuItem()
                {
                    Name = g.EmployeeId.ToString(),
                    Text = $"{g.EmployeeId}-{g.UserName}"
                }).ToArray());
                EmpSelect.SelectedValue = new MenuItem()
                {
                    Name = AppSession.CurrentUser.EmployeeId,
                    Text = $"{AppSession.CurrentUser.EmployeeId}-{AppSession.CurrentUser.UserName}"
                };
            }


        }

        private async Task LoadWorkCenterGroupSelectAsync()
        {
            WorkCenterGroupSelect.Items.Clear();
            var group = await _facade.WorkCenterGroup.GetListByGroupTypeAsync(AppSession.CurrentFactoryId, ((int)WorkCenterGroupType.AssemblyGroup).ToString());
            if (group != null)
            {
                WorkCenterGroupSelect.Items.AddRange(group.Select(g => new MenuItem()
                {
                    Name = g.Id.ToString(),
                    Text = $"{g.GroupCode}-{g.GroupName}"
                }).ToArray());

            }
        }

        private async Task LoadTaskCategorySelectAsync()
        {
            TaskCategorySelect.Items.Clear();
            var taskCategories = await _facade.WorkTaskCategory.GetListByStepTypeAsync(WorkStepType_Assm);
            if (taskCategories.Any())
            {
                TaskCategorySelect.Items.AddRange(taskCategories.Select(g => new MenuItem()
                {
                    Name = g.CategoryCode,
                    Text = g.CategoryDescription,
                }).ToArray());
            }
        }

        private async Task LoadWorkOrderBomAsync(int processId)
        {
            BomTable.DataSource = null;
            var boms = await _facade.WorkOrderBomItemService.GetListByProcessIdsAsync(new List<int>() { processId });
            if (boms.Any(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed && x.ConsumeType != null))
            {
                var activeBoms = boms.Where(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed && x.ConsumeType != null).OrderBy(x => x.ConsumeType).ThenBy(x => x.MaterialCode).ToList();
                var summaryBoms = activeBoms.GroupBy(b => (b.ConsumeType, b.MaterialCode)).Select(g => new WorkOrderBomItemDto()
                {
                    ConsumeType = g.Key.ConsumeType,
                    MaterialCode = g.Key.MaterialCode,
                    MaterialDesc = g.FirstOrDefault()?.MaterialDesc,
                    Unit = g.FirstOrDefault()?.Unit,
                    RequiredQuantity = g.Sum(x => x.RequiredQuantity),
                }).ToList();
                var kittingItems = (await _kittingService.GetListByProcessIdAsync(processId)).GroupBy(k => k.MaterialCode).ToDictionary(g => g.Key, g => g.Sum(x => x.Quantity));
                BomTable.DataSource = summaryBoms.Select(b =>
                {
                    kittingItems.TryGetValue(b.MaterialCode, out decimal pickingQuantity);

                    return new WorkOrderBomKittingViewModel
                    {
                        MaterialCode = b.MaterialCode,
                        MaterialDesc = b.MaterialDesc,
                        Unit = b.Unit,
                        RequiredQuantity = (decimal)b.RequiredQuantity,
                        ConsumeType = Enum.IsDefined(typeof(ConsumeType), b.ConsumeType) ? ((ConsumeType)b.ConsumeType).GetDescription() : string.Empty,
                        PickingQuantity = pickingQuantity,
                        Status = pickingQuantity > 0 ? "已拣配" : "未拣配"
                    };
                }).OrderBy(x => x.Status).ThenBy(x => x.ConsumeType).ToList();
            }

        }

        public async Task<int> LoadOrderDataAsync()
        {
            TaskTable.DataSource = null;
            if (DispathchDatePickerRange.Value == null)
                throw new Exception("请先选择订单排产日期！");
            var dateStart = DispathchDatePickerRange.Value.FirstOrDefault();
            var dateEnd = DispathchDatePickerRange.Value.LastOrDefault();
            if (dateStart == null || dateEnd == null)
                throw new Exception("请先选择订单排产日期！");

            var workcenterGroup = (MenuItem)WorkCenterGroupSelect.SelectedValue;
            int GroupId = 0;
            if (workcenterGroup == null || !int.TryParse(workcenterGroup.Name, out GroupId))
                throw new Exception("请先选择工作中心组！");

            int workcenterId = 0;
            var workcenter = (MenuItem)WorkCenterSelect.SelectedValue;
            if (workcenter == null || !int.TryParse(workcenter.Name, out workcenterId))
                throw new Exception("请先选择工作中心！");
            var taskCategories = TaskCategorySelect.SelectedValue;
            if (!taskCategories.Any())
                throw new Exception("请先选择工步！");
            var workcenters = await _facade.WorkCenter.GetBygroupIdAsync(GroupId);
            var result = await _pendingTaskService.GetListByWorkCentersAsync(
                AppSession.CurrentFactoryId,
                workcenters.Select(x => x.WorkCenterCode).Distinct().ToList(),
                dateStart,
                dateEnd);


            if (result != null && result.Any())
            {

                var kittingTasks = await _stepTaskService.GetListByWorkOrderProcessIdAsync(result.Where(x => x.PrevProcessId != null).Select(x => (int)x.PrevProcessId).ToList(), TaskCategories.Kitting);

                var kittingStatusDict = kittingTasks
                .GroupBy(t => (int)t.WorkOrderProcessId)
                .ToDictionary(
                    g => g.Key,
                    g => g.First()
                );
                var activeBoms = await _facade.WorkOrderBomItemService.GetListByProcessIdsAsync(result.Where(x => x.PrevProcessId != null).Select(x => (int)x.PrevProcessId).ToList());
                var activeBomsDict = activeBoms.Where(x => x.RequiredQuantity> 0  && x.MovementAllowed == true && (x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial))
                .GroupBy(t => (int)t.WorkOrderProcessId)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                );

                foreach (var item in result)
                {
                    // 如果有 PrevProcessId，并且在字典里找到了对应的任务状态
                    if (item.PrevProcessId.HasValue && activeBomsDict.TryGetValue(item.PrevProcessId.Value,out var boms) &&
                        kittingStatusDict.TryGetValue(item.PrevProcessId.Value, out WorkOrderStepTaskDto kittingTask))
                    {
                        // 如果齐套任务(Kitting)已完成，更新当前 result item 的状态
                        if (kittingTask == null || int.Parse(kittingTask.Status) >= int.Parse(WorkTaskStatus.Completed))
                        {
                            item.PrevProcessStatus = WorkTaskStatus.Completed;
                            item.PrevCompletedQuantity = item.Quantity;
                        }
                    }
                }
                TaskTable.DataSource = result.OrderBy(x => x.WorkOrderNo).ThenByDescending(x => x.DispatchDate).ThenBy(x => x.Operation).ToList();
                return result.Count;
            }
            else
            {
                throw new Exception("未查询到待办订单信息！");
            }
        }

        private void ToggleInputs(bool enable)
        {
            DispathchDatePickerRange.Enabled = enable;
            WorkCenterGroupSelect.Enabled = enable;
            WorkCenterSelect.Enabled = enable;
            TaskCategorySelect.Enabled = enable;

            if (!enable)
            {
                SearchButton.Text = "重置";
                SearchButton.Type = AntdUI.TTypeMini.Error;
                OrderScanInput.Enabled = true;
            }
            else
            {
                SubmitButton.Enabled = false;
                SearchButton.Text = "查询";
                SearchButton.Type = AntdUI.TTypeMini.Primary;
                OrderScanInput.Enabled = false;
            }
        }

        private void SetTaskStatusTag(string status)
        {
            switch (status)
            {
                case WorkTaskStatus.New:
                case WorkTaskStatus.Released:
                case WorkTaskStatus.Scheduled:
                    TaskStatusTag.Type = TTypeMini.Success;
                    TaskStatusTag.Text = "已排产";
                    break;

                case WorkTaskStatus.InProgress:
                    TaskStatusTag.Type = TTypeMini.Primary;
                    TaskStatusTag.Text = "执行中";
                    break;

                case WorkTaskStatus.Suspended:
                    TaskStatusTag.Type = TTypeMini.Warn;
                    TaskStatusTag.Text = "已挂起";
                    break;
                case WorkTaskStatus.Completed:
                    TaskStatusTag.Type = TTypeMini.Success;
                    TaskStatusTag.Text = "已完成";
                    break;
                case WorkTaskStatus.Closed:
                    TaskStatusTag.Type = TTypeMini.Info;
                    TaskStatusTag.Text = "已关闭";
                    break;
                case WorkTaskStatus.Cancelled:
                    TaskStatusTag.Type = TTypeMini.Info;
                    TaskStatusTag.Text = "已取消";
                    break;
                default:
                    TaskStatusTag.Type = TTypeMini.Error;
                    TaskStatusTag.Text = "未准备";
                    break;
            }
        }


        /// <summary>
        /// 功能：扫码置顶
        /// Table 的置顶非常简单，只需要操作数据源 List，然后重新赋值 DataSource 即可
        /// </summary>
        public PendingOperationTaskDto? MoveOrderToTop(string orderNo,string? operation = null)
        {
            var orderDatas = TaskTable.DataSource as List<PendingOperationTaskDto>;
            var target = orderDatas.Where(x => x.WorkOrderNo == orderNo)
                .WhereIF(!string.IsNullOrEmpty(operation), x => x.Operation == operation)
                .OrderByDescending(x => x.Operation);
            if (target != null && target.Count() > 0)
            {
                foreach (var item in target)
                {
                    // 1. 修改数据源顺序
                    orderDatas.Remove(item);
                    orderDatas.Insert(0, item);

                    // 2. 刷新表格 (AntdUI Table 重新赋值 DataSource 会触发重绘，非常快)
                    TaskTable.DataSource = null; // 有时候需要重置一下触发刷新，视版本而定
                    TaskTable.DataSource = orderDatas;

                    // 3. 选中第一行并滚动
                    TaskTable.SelectedIndex = 1;
                    TaskTable.ScrollLine(1, true);
                }

            }


            return target?.Last();
        }


        private void InitializeTable()
        {
            TaskTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("WorkOrderNo", "订单/日期", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is PendingOperationTaskDto model)
                        {
                            return model.WorkOrderNo + "\r\n" + model.DispatchDate?.ToString("yyyy-MM-dd")+"\r\n" + model.Operation;
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
                                return new AntdUI.CellTag("未合箱", AntdUI.TTypeMini.Error);
                            if((model.PrevCompletedQuantity ?? 0) > 0  && (model.CompletedQuantity ?? 0) <= 0)
                                return new AntdUI.CellTag("已排产", AntdUI.TTypeMini.Primary);
                            if((model.CompletedQuantity ?? 0) > 0)
                            {
                              if((model.CompletedQuantity ?? 0) < model.Quantity)
                                  return new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Primary);
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
                }.SetWidth("auto").SetLocalizationTitleID("Table.Column."),

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

            BomTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialCode", "物料信息", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is WorkOrderBomKittingViewModel model)
                        {
                            return $"订单物料：{model.MaterialCode}\r\n单       位：{model.Unit}";
                        }
                        return value;
                    }
                }.SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("RequiredQuantity", "数量", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is WorkOrderBomKittingViewModel model)
                        {
                            return $"需求数量：{model.RequiredQuantity:F3}\r\n拣配数量：{model.PickingQuantity:F3}";
                        }
                        return value;
                    }
                }.SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("ConsumeType", "物料属性", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "电缆" => new AntdUI.CellTag("电缆", AntdUI.TTypeMini.Primary),
                            "按单领料" => new AntdUI.CellTag("按单领料", AntdUI.TTypeMini.Success),
                            "一次性领料" => new AntdUI.CellTag("一次性领料", AntdUI.TTypeMini.Error),
    _                       => new AntdUI.CellTag("未知", AntdUI.TTypeMini.Error),
                        };
                    }
                }.SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "已拣配" => new AntdUI.CellTag("已拣配", AntdUI.TTypeMini.Success),
                            "未拣配" => new AntdUI.CellTag("未拣配", AntdUI.TTypeMini.Error),
    _                       => new AntdUI.CellTag("未拣配", AntdUI.TTypeMini.Error),
                        };
                    }
                }.SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
            };
        }

        private async void WorkCenterGroupSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            WorkCenterSelect.SelectedValue = null;
            WorkCenterSelect.Items.Clear();
            var groupSelect = ((MenuItem)WorkCenterGroupSelect.SelectedValue).Name;
            if (groupSelect != null && int.TryParse(groupSelect, out int groupId))
            {
                var workcenters = await _facade.WorkCenter.GetBygroupIdAsync(groupId);
                if (workcenters.Any())
                {
                    WorkCenterSelect.Items.AddRange(workcenters.Select(g => new MenuItem()
                    {
                        Name = g.Id.ToString(),
                        Text = $"{g.WorkCenterCode}-{g.WorkCenterName}",
                    }).ToArray());
                }
            }
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SearchButton, async () =>
            {
                if (SearchButton.Text.Contains("查询"))
                {
                    var count = await LoadOrderDataAsync();
                    AntdUI.Message.success(this.ParentForm, $"查询成功：共查询出{count}条记录！");
                    ToggleInputs(false);
                }
                else
                    ToggleInputs(true);

            });
        }

        private async void OrderScanInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                await RunAsync(async () =>
                {
                    SubmitButton.Enabled = false;
                    _currentTaskCategory.Clear();
                    var workorder = OrderScanInput.Text.Trim();
                    if (string.IsNullOrEmpty(workorder)) return;
                    var data = TaskTable.DataSource as List<PendingOperationTaskDto>;
                    if (data == null || !data.Any(x => x.WorkOrderNo == workorder))
                        throw new Exception($"未查询到订单{workorder}的信息，请检查！");
                    var currentTask = data.OrderBy(x => x.Operation).First(x => x.WorkOrderNo == workorder);
                    //更新Label
                    OpLabel.Text = currentTask.Operation;
                    OrderNoLabel.Text = currentTask.WorkOrderNo;
                    MatcLabel.Text = currentTask.MaterialCode;
                    MatdLabel.Text = currentTask.MaterialDesc;
                    ProgressLabel.Text = $"{(currentTask.CompletedQuantity ?? 0).ToString("F0")} / {(currentTask.Quantity ?? 0).ToString("F0")}";
                    _currentPendingTask = currentTask;
                    SetTaskStatusTag(currentTask.Status);
                    if (currentTask.TaskId == null)
                        throw new Exception($"订单出错，请重新同步SAP订单{currentTask.WorkOrderNo}");
                    MoveOrderToTop(currentTask.WorkOrderNo);
                    if (currentTask.PrevProcessId != null && currentTask.PrevProcessId > 0)
                    {
                        var opTasks = await _opTaskService.GetListByProcessIdAsync(new List<int>() { (int)currentTask.PrevProcessId });
                        if (opTasks.Any())
                        {
                            //判断当前订单是否合箱
                            var kitTasks = await _stepTaskService.GetListByOperationIdAsync(opTasks.Select(x => x.Id).ToList());
                            if (kitTasks.Any(x => x.TaskCategory == TaskCategories.Kitting && int.Parse(x.Status) < int.Parse(WorkTaskStatus.Completed)))
                            {
                                throw new Exception($"订单{currentTask.WorkOrderNo}存在未合箱的记录，请先合箱！");
                            }
                        }
                        else
                            throw new Exception($"订单出错，请重新同步SAP订单{currentTask.WorkOrderNo}");

                        await LoadWorkOrderBomAsync((int)currentTask.PrevProcessId);
                        //创建工步任务
                        var selected = TaskCategorySelect.SelectedValue;

                        var processes = await _facade.WorkOrderProcessService.GetListByOrderIdAync(currentTask.WorkOrderId);
                        //判断当前工序是否为第二道工序
                        int index = processes.OrderBy(x => x.Operation).ToList().FindIndex(u => u.Id == currentTask.WorkOrderProcessId);

                        if (selected is IEnumerable<object> list && index == 1)
                        {
                            _currentTaskCategory.AddRange(list.Cast<MenuItem>().Select(x => x.Name).ToList());
                        }
                        else
                        {
                            //第三道工序只创建完成任务
                            var taskCategories = await _facade.WorkTaskCategory.GetListByStepTypeAsync(WorkStepType_Inspe);
                            if (taskCategories.Any())
                            {
                                _currentTaskCategory.Add(taskCategories.Where(x => x.IsSapCurrentTrigger).Last().CategoryCode);
                            }
                        }

                        var existStepTasks = await _stepTaskService.GetListByOperationIdAsync((int)currentTask.TaskId);
                        var existingMaxCode = 0;
                        var codes = existStepTasks.Select(s => int.TryParse(s.StepCode, out int c) ? c : 0);
                        if (codes.Any()) existingMaxCode = codes.Max();
                        var missStepTasks = _currentTaskCategory.Except(existStepTasks.Select(x => x.TaskCategory));

                        if (missStepTasks.Any())
                        {
                            var stepIds = await _stepTaskService.CreateBatchAsync(missStepTasks.Select(item => new WorkOrderStepTaskCreateDto()
                            {
                                OperationTaskId = currentTask.TaskId,
                                WorkCenterGroupId = int.Parse(((MenuItem)WorkCenterGroupSelect.SelectedValue).Name),
                                WorkCenterGroupCode = ((MenuItem)WorkCenterGroupSelect.SelectedValue).Text.Split("-")[0],
                                ActualWorkCenterId = int.Parse(((MenuItem)WorkCenterSelect.SelectedValue).Name),
                                ActualWorkCenterCode = ((MenuItem)WorkCenterSelect.SelectedValue).Text.Split("-")[0],
                                ActualStartTime = DateTime.Now,
                                Quantity = currentTask.Quantity,
                                CompletedQuantity = 0,
                                StepCode = (existingMaxCode + 10).ToString(),
                                Status = WorkTaskStatus.New,
                                TaskCategory = item,
                                CreatedBy = AppSession.CurrentUser.EmployeeId,
                            }).ToList());

                        }

                        SubmitButton.Enabled = true;
                    }
                });
            }
        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SubmitButton, async () =>
            {

                if (_currentPendingTask == null)
                    throw new Exception("请先扫描或输入订单！");
                if (_currentPendingTask.TaskId == null)
                    throw new Exception($"{_currentPendingTask.WorkOrderNo}未同步最新SAP订单，请重新同步！");
                if (ReportInputNumber.Value <= 0)
                    throw new Exception("报工数量输入有误，请检查！");
                var stepTasks = await _stepTaskService.GetListByOperationIdAsync((int)_currentPendingTask.TaskId);
                if (!stepTasks.Any(x => _currentTaskCategory.Contains(x.TaskCategory)) || stepTasks.Where(x => _currentTaskCategory.Contains(x.TaskCategory)).Count() != _currentTaskCategory.Count())
                    throw new Exception("任务数量与工步数量未匹配，请重新扫描订单！");
                if (EmpSelect.SelectedValue == null)
                    throw new Exception("未选择报工人员，请先选择！");
                var currentProcess = await _facade.WorkOrderProcessService.GetByIdAsync(_currentPendingTask.WorkOrderProcessId);
                if (currentProcess != null && currentProcess.Quantity < (currentProcess.CompletedQuantity ?? 0) + ReportInputNumber.Value)
                {
                    throw new Exception("本次报工已超出订单数量，请检查！");
                }
                await _taskExecutionService.ReportStepProductionAsync(stepTasks.Where(x => _currentTaskCategory.Contains(x.TaskCategory)).Select(X => X.Id).ToList(), ReportInputNumber.Value, ((MenuItem)EmpSelect.SelectedValue).Name, remark: RemarkInput.Text);

                ReportInputNumber.Value = 0;
                ReportInputNumber.Text = string.Empty;
                ScrapLenInputNumber.Value = 0;
                ScrapLenInputNumber.Text = string.Empty;
                RemarkInput.Text = string.Empty;

                var opTask = await _opTaskService.GetByIdAsync((int)_currentPendingTask.TaskId);
                ProgressLabel.Text = $"{(opTask.CompletedQuantity ?? 0).ToString("F0")} / {(opTask.Quantity ?? 0).ToString("F0")}";
                SetTaskStatusTag(opTask.Status);
                if (opTask.Status != WorkTaskStatus.InProgress)
                {
                    _currentPendingTask = null;
                }

                await LoadOrderDataAsync();
                if (_currentPendingTask != null)
                    MoveOrderToTop(_currentPendingTask.WorkOrderNo);


            }, confirmMsg: $"即将对订单{_currentPendingTask?.WorkOrderNo}进行报工，是否继续？", successMsg: "报工成功！");
        }

        private void EmpSelect_DoubleClick(object sender, EventArgs e)
        {
            EmpSelect.ReadOnly = false;
        }

        private async void TaskTable_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is PendingOperationTaskDto data) 
            {
                await RunAsync(async () =>
                {
                    _currentTaskCategory.Clear();
                    var currentTask = data;
                    //更新Label
                    OrderNoLabel.Text = currentTask.WorkOrderNo;
                    MatcLabel.Text = currentTask.MaterialCode;
                    MatdLabel.Text = currentTask.MaterialDesc;
                    OpLabel.Text = currentTask.Operation;
                    ProgressLabel.Text = $"{(currentTask.CompletedQuantity ?? 0).ToString("F0")} / {(currentTask.Quantity ?? 0).ToString("F0")}";
                    _currentPendingTask = currentTask;
                    SetTaskStatusTag(currentTask.Status);
                    if (currentTask.TaskId == null)
                        throw new Exception($"订单出错，请重新同步SAP订单{currentTask.WorkOrderNo}");
                    MoveOrderToTop(data.WorkOrderNo,data.Operation);
                    if (currentTask.PrevProcessId != null && currentTask.PrevProcessId > 0)
                    {
                        var opTasks = await _opTaskService.GetListByProcessIdAsync(new List<int>() { (int)currentTask.PrevProcessId });
                        if (opTasks.Any())
                        {
                            //判断当前订单是否合箱
                            var kitTasks = await _stepTaskService.GetListByOperationIdAsync(opTasks.Select(x => x.Id).ToList());
                            if (kitTasks.Any(x => x.TaskCategory == TaskCategories.Kitting && int.Parse(x.Status) < int.Parse(WorkTaskStatus.Completed)))
                            {
                                throw new Exception($"订单{currentTask.WorkOrderNo}存在未合箱的记录，请先合箱！");
                            }
                        }
                        else
                            throw new Exception($"订单出错，请重新同步SAP订单{currentTask.WorkOrderNo}");

                        await LoadWorkOrderBomAsync((int)currentTask.PrevProcessId);
                        //创建工步任务
                        var selected = TaskCategorySelect.SelectedValue;

                        var processes = await _facade.WorkOrderProcessService.GetListByOrderIdAync(currentTask.WorkOrderId);
                        //判断当前工序是否为第二道工序
                        int index = processes.OrderBy(x => x.Operation).ToList().FindIndex(u => u.Id == currentTask.WorkOrderProcessId);

                        if (selected is IEnumerable<object> list && index == 1)
                        {
                            _currentTaskCategory.AddRange(list.Cast<MenuItem>().Select(x => x.Name).ToList());
                        }
                        else
                        {
                            //第三道工序只创建完成任务
                            var taskCategories = await _facade.WorkTaskCategory.GetListByStepTypeAsync(WorkStepType_Inspe);
                            if (taskCategories.Any())
                            {
                                _currentTaskCategory.Add(taskCategories.Where(x => x.IsSapCurrentTrigger).Last().CategoryCode);
                            }
                        }

                        var existStepTasks = await _stepTaskService.GetListByOperationIdAsync((int)currentTask.TaskId);
                        var existingMaxCode = 0;
                        var codes = existStepTasks.Select(s => int.TryParse(s.StepCode, out int c) ? c : 0);
                        if (codes.Any()) existingMaxCode = codes.Max();
                        var missStepTasks = _currentTaskCategory.Except(existStepTasks.Select(x => x.TaskCategory));

                        if (missStepTasks.Any())
                        {
                            var stepIds = await _stepTaskService.CreateBatchAsync(missStepTasks.Select(item => new WorkOrderStepTaskCreateDto()
                            {
                                OperationTaskId = currentTask.TaskId,
                                WorkCenterGroupId = int.Parse(((MenuItem)WorkCenterGroupSelect.SelectedValue).Name),
                                WorkCenterGroupCode = ((MenuItem)WorkCenterGroupSelect.SelectedValue).Text.Split("-")[0],
                                ActualWorkCenterId = int.Parse(((MenuItem)WorkCenterSelect.SelectedValue).Name),
                                ActualWorkCenterCode = ((MenuItem)WorkCenterSelect.SelectedValue).Text.Split("-")[0],
                                ActualStartTime = DateTime.Now,
                                Quantity = currentTask.Quantity,
                                CompletedQuantity = 0,
                                StepCode = (existingMaxCode + 10).ToString(),
                                Status = WorkTaskStatus.New,
                                TaskCategory = item,
                                CreatedBy = AppSession.CurrentUser.EmployeeId,
                            }).ToList());

                        }

                        SubmitButton.Enabled = true;
                    }

                });
            }
        }
    }

    public class WorkOrderBomKittingViewModel : AntdUI.NotifyProperty
    {


        string _materialCode;
        public string MaterialCode
        {
            get => _materialCode;
            set
            {
                if (_materialCode == value)
                    return;
                _materialCode = value;
                OnPropertyChanged();
            }
        }

        string _materialDesc;
        public string MaterialDesc
        {
            get => _materialDesc;
            set
            {
                if (_materialDesc == value)
                    return;
                _materialDesc = value;
                OnPropertyChanged();
            }
        }

        string? _unit;
        public string? Unit
        {
            get => _unit;
            set
            {
                if (_unit == value)
                    return;
                _unit = value;
                OnPropertyChanged();
            }
        }

        string _consumeType;
        public string ConsumeType
        {
            get => _consumeType;
            set
            {
                if (_consumeType == value)
                    return;
                _consumeType = value;
                OnPropertyChanged();
            }
        }

        decimal _requiredQuantity;
        public decimal RequiredQuantity
        {
            get => _requiredQuantity;
            set
            {
                if (_requiredQuantity == value)
                    return;
                _requiredQuantity = value;
                OnPropertyChanged();
            }
        }

        decimal _pickingQuantity;
        public decimal PickingQuantity
        {
            get => _pickingQuantity;
            set
            {
                if (_pickingQuantity == value)
                    return;
                _pickingQuantity = value;
                OnPropertyChanged();
            }
        }

        string? _status;
        public string? Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;
                _status = value;
                OnPropertyChanged();
            }
        }

        //AntdUI.CellLink[] _btns = new AntdUI.CellLink[]
        //    {
        //            new AntdUI.CellButton("unload", "下料", AntdUI.TTypeMini.Primary)
        //    };
        //public AntdUI.CellLink[] btns
        //{
        //    get => _btns;
        //    set
        //    {
        //        _btns = value;
        //        OnPropertyChanged();
        //    }
        //}
    }
}
