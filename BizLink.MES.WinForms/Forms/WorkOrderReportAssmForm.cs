using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Helper;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Common.Views;
using BizLink.MES.WinForms.Infrastructure;
using Dm;
using Dm.util;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using SqlSugar;
using System;
using System.CodeDom;
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
    public partial class WorkOrderReportAssmForm : MesBaseForm
    {
        private readonly AssemblyModuleFacade _facade;
        private readonly IFormFactory _formFactory;

        private List<string> _workCenters = new List<string>();
        private List<MenuItem> _workStations = new List<MenuItem>();
        private List<V_WorkOrderInProgressView> _orderViews = new List<V_WorkOrderInProgressView>();
        private V_WorkOrderInProgress _currentOrderInPress;
        private List<WorkOrderTaskDto> _workOrderTaskDtos = new List<WorkOrderTaskDto>();
        private const string WorkCenterGroup_FQC = "FQC";

        // 2. 构造函数：只注入 Facade 和 Factory
        public WorkOrderReportAssmForm(
            AssemblyModuleFacade facade,
            IFormFactory formFactory)
        {
            _facade = facade;
            _formFactory = formFactory;

            InitializeComponent();
            InitializeTable();
        }

        // 3. 加载逻辑
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            workcenterSelect.PlaceholderText = "请先选择工作中心组...";
            currentWorkCenterSelect.PlaceholderText = "请先选择当前工作中心...";
            workstationSelect.PlaceholderText = "请先选择工位...";
            orderInput.PlaceholderText = "请扫描流转卡二维码";
            startDatePicker.Value = DateTime.Now.AddDays(-7).Date;
            endDatePicker.Value = DateTime.Now.AddDays(3).Date;

            executeButton.Enabled = false;
            saveButton.Enabled = false;
            suspendButton.Enabled = false;
            orderInput.Enabled = false;

            await RunAsync(async () =>
            {
                if (AppSession.CurrentFactoryId <= 0)
                {
                    throw new Exception("请先选择工厂！");
                }
                await LoadWorkCentersAndStationsAsync();
            });
        }

        private async System.Threading.Tasks.Task LoadWorkCentersAndStationsAsync()
        {

            var workcenterGroups = await _facade.WorkCenterGroup.GetListByGroupTypeAsync(AppSession.CurrentFactoryId,((int)WorkCenterGroupType.AssemblyGroup).ToString());
            if (workcenterGroups != null)
            {
                workcenterSelect.Items.AddRange(workcenterGroups.Select(s => new MenuItem
                {
                    Name = s.Id.ToString(),
                    Text = s.GroupCode + "-" + s.GroupName,
                }).ToArray());
            }


            // 加载工位
            var allStations = await _facade.WorkStation.GetAllAsync(AppSession.CurrentFactoryId);
            if (allStations != null)
            {


                var validStations = allStations.Where(x => x.WorkAreaId == 2 && x.WorkCenterId == 0).OrderByDescending(x => x.IsStartStep).OrderBy(x => x.IsEndStep).Select(s => new MenuItem
                {
                    Name = s.Id.ToString(),
                    Text = s.WorkStationName
                }).ToArray();

                workstationSelect.Items.AddRange(validStations);
            }
        }


        private void InitializeTable()
        {
            orderTable.Columns = new AntdUI.ColumnCollection
            {
               new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
               new AntdUI.Column("Operation", "工序", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenter", "工作中心", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("OrderNumber", "订单号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        string statusText = value as string;
                        return CellTagHelper.BuildWorkOrderStatusTag(statusText);
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "订单料号", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "订单数量", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Progress", "进度", AntdUI.ColumnAlign.Center).SetWidth("Auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("StartTime", "开工日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("DispatchDate", "排产日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("PlannerRemark", "计划备注", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

            };

            pickingTable.Columns = new AntdUI.ColumnCollection
            {
             new AntdUI.Column("OrderNumber", "订单号", AntdUI.ColumnAlign.Center).SetFixed().SetLocalizationTitleID("Table.Column."),
             new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetSortOrder().SetDefaultFilter().SetColAlign().SetLocalizationTitleID("Table.Column."),
             new AntdUI.Column("BomItem", "物料行号", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),
             new AntdUI.Column("ConsumeType", "物料类别", AntdUI.ColumnAlign.Center)
             {
                    Render = (value, record, index) =>
                    {
                        string text = value as string;
                        return CellTagHelper.BuildConsumeTypeTag(text);
                    }
             }.SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
             new AntdUI.Column("RequiredQuantity", "数量", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
             new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),
             //new AntdUI.Column("BatchCode", "批次", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
             //new AntdUI.Column("BarCode", "标签ID", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

            };


        }

        private async System.Threading.Tasks.Task LoadOrderDataAsync()
        {
            await RunAsync(async () =>
            {
                // 1. 获取数据
                var result = await _facade.View.GetListAsync(_workCenters, startDatePicker.Value, endDatePicker.Value);

                if (result != null && result.Count > 0)
                {
                    _orderViews = result
                        .Where(w => !w.Status.Equals("4"))
                        .Select(w => new V_WorkOrderInProgressView(w))
                        .ToList();

                    // 应用当前筛选
                    var filterText = orderInput.Text.Trim();
                    orderTable.DataSource = _orderViews
                        .WhereIF(!string.IsNullOrWhiteSpace(filterText), w => w.OrderNumber.Contains(filterText)).OrderBy(w => w.Operation).OrderBy(w => w.OrderNumber)
                        .ToList();

                    AntdUI.Message.success(this, $"成功查询出{result.Count}条待办订单");
                }
                else
                {
                    orderTable.DataSource = null;
                    executeButton.Enabled = false;
                    saveButton.Enabled = false;
                    suspendButton.Enabled = false;
                    throw new Exception("未查询到记录！");
                }
            });
        }

        // --- 工位确认 ---
        private async void stationConfirmButtom_Click(object sender, EventArgs e)
        {
            stationConfirmButtom.Enabled = false;
            await RunAsync(async () =>
            {
                _workStations.Clear();

                if (stationConfirmButtom.Text.Contains("工位确认"))
                {
                    if (startDatePicker.Value == null)
                        throw new Exception("请先选择开始日期！");
                    if (endDatePicker.Value == null)
                        throw new Exception("请先选择结束日期！");
                    if (string.IsNullOrWhiteSpace(workcenterSelect.Text))
                        throw new Exception("请先选择工作中心组！");

                    if (string.IsNullOrWhiteSpace(currentWorkCenterSelect.Text))
                        throw new Exception("请先选择当前工作中心！");
                    var selectedWorkCenter = currentWorkCenterSelect.SelectedValue;
                    if (selectedWorkCenter == null)
                        throw new Exception("请先选择当前工作中心！");

                    var selected = workstationSelect.SelectedValue;
                    // 注意：AntdUI 的 SelectedValue 如果是多选，通常是 List<object> 或 object[]
                    if (selected is IEnumerable<object> list)
                    {
                        _workStations.AddRange(list.Cast<MenuItem>());
                    }
                    else if (selected.FirstOrDefault() is MenuItem item)
                    {
                        _workStations.Add(item);
                    }

                    if (_workStations.Count == 0)
                        throw new Exception("请先选择工位！");

                    ToggleInputs(false);
                }
                else
                {
                    ToggleInputs(true);
                }

                // 这里的 LoadOrderData 其实是同步方法，但内部有 async void，
                // 在 RunAsync 中最好调用 Task 返回的方法。
                // 建议将 LoadOrderData 改为 async Task LoadOrderDataAsync
                // await LoadOrderDataAsync(); 
                await LoadOrderDataAsync(); // 暂时保持兼容
                stationConfirmButtom.Enabled = true;

            });

            stationConfirmButtom.Enabled = true;
        }

        // --- 核心功能：扫描/查询 ---
        private async void orderInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                await RunAsync(async () =>
                {
                    if (stationConfirmButtom.Text.Contains("工位确认"))
                        throw new Exception("请先进行工位确认！");
                    var orderNo = orderInput.Text.Trim().Split('-')[0];
                    // 内存过滤
                    var filtered = _orderViews.Where(w => w.OrderNumber.Contains(orderNo)).OrderBy(x => x.Operation).ToList();
                    orderTable.DataSource = filtered;

                    if (filtered.Count == 1)
                    {
                        orderTable.SelectedIndex = 1;
                        var temp = filtered.First();
                        var orders = await _facade.View.GetByOrderNoAsync(temp.OrderNumber);
                        _currentOrderInPress = orders.FirstOrDefault(x => x.OrderProcessId == temp.OrderProcessId);

                        if (_currentOrderInPress != null)
                        {
                            var bom = await _facade.WorkOrderBomItemService.GetListByOrderIdAync(_currentOrderInPress.OrderId);
                            if (bom != null)
                            {
                                pickingTable.DataSource = bom.Where(x => x.ConsumeType != null).Select(x => new
                                {
                                    OrderNumber = _currentOrderInPress.OrderNumber,
                                    MaterialCode = x.MaterialCode,
                                    BomItem = x.BomItem,
                                    ConsumeType = ((ConsumeType)x.ConsumeType).GetDescription(), // 简化转换
                                    RequiredQuantity = x.RequiredQuantity,
                                    MaterialDesc = x.MaterialDesc,
                                }).ToList();
                            }

                            executeButton.Enabled = true;
                            saveButton.Enabled = true;
                            suspendButton.Enabled = true;
                        }
                        else
                        {
                            throw new Exception("未查询到订单号，请检查订单号是否正确或订单是否已完工！");
                        }
                    }
                });
                orderInput.SelectAll();
            }
        }

        private async void workcenterSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            _workCenters.Clear();
            //workstationSelect.SelectedValue = null;
            if (e.Value == null)
                return;

            var workcenterGroup = await _facade.WorkCenterGroup.GetByIdAsync(int.Parse(((MenuItem)e.Value).Name));
            if (workcenterGroup == null)
                return;

            var workcenters = await _facade.WorkCenter.GetListByGroupCodeAsync(workcenterGroup.GroupCode);
            if (workcenters == null) 
                return;
            _workCenters.AddRange(workcenters.Select(x => x.WorkCenterCode));
            
            currentWorkCenterSelect.Items.Clear();
            currentWorkCenterSelect.Items.AddRange(workcenters.Select(x => new MenuItem()
            {
                Name = x.Id.ToString(),
                Text = x.WorkCenterCode + "-" + x.WorkCenterName,
            }).ToArray());

        }

        // --- 核心功能：执行/打开详情 ---
        private async void executeButton_Click(object sender, EventArgs e)
        {
            await RunAsync(executeButton, async () =>
            {
                if (orderTable.DataSource == null)
                    return;

                // 1. 获取选中行
                var viewList = orderTable.DataSource as List<V_WorkOrderInProgressView>;
                if (viewList == null)
                    return;
                var view = viewList[orderTable.SelectedIndex-1];
                if (view == null)
                    return;


                if (view.Status == ((int)WorkOrderStatus.UnKnow).ToString())
                {
                    if (AntdUI.Modal.open(new AntdUI.Modal.Config(this.ParentForm, "确认", "前道工序未完工，是否继续执行？", AntdUI.TType.Warn)) != DialogResult.OK)
                        return;
                }


                var taskNo = view.OrderNumber + "-" + view.Operation;

                var workcenters = await _facade.WorkCenter.GetListByGroupCodeAsync(WorkCenterGroup_FQC);
                //如果当前订单工作中心属于检验工作中心，只创建装配完工任务
                //var selected = workstationSelect.SelectedValue;
                //if (selected is IEnumerable<object> list)
                //{
                //    _workStations.AddRange(list.Cast<MenuItem>());
                //}
                //else if (selected.FirstOrDefault() is MenuItem item)
                //{
                //    _workStations.Add(item);
                //}

                //var tempStations = _workStations;
                bool fqcFlag = false;
                if (workcenters.Any(x => x.WorkCenterCode == view.WorkCenter))
                {
                    var allStations = await _facade.WorkStation.GetAllAsync(AppSession.CurrentFactoryId);
                    if (allStations != null)
                    {
                        _workStations.clear();

                        var endStation = allStations.Where(x => x.WorkAreaId == 2 && x.WorkCenterId == 0 && x.IsEndStep).Select(s => new MenuItem
                        {
                            Name = s.Id.ToString(),
                            Text = s.WorkStationName
                        }).First();

                        _workStations.Add(endStation);
                        fqcFlag = true;
                    }
                }

                // 2. 准备创建 DTO 列表
                var createDtos = _workStations.Select(station => new WorkOrderTaskCreateDto
                {
                    OrderProcessId = view.OrderProcessId,
                    OrderId = view.OrderId,
                    OrderNumber = view.OrderNumber,
                    TaskNumber = taskNo,
                    WorkStationId = int.Parse(station.Name),
                    MaterialCode = view.MaterialCode,
                    MaterialDesc = view.MaterialDesc,
                    MaterialItem = "",
                    Status = "1",
                    Quantity = view.Quantity,
                    CompletedQty = 0,
                    NextWorkCenter = view.NextWorkCenter,
                    Operation = view.Operation,
                    ProfitCenter = view.ProfitCenter,
                    StartTime = view.StartTime,
                    DispatchDate = view.DispatchDate,
                    WorkCenter = ((MenuItem)currentWorkCenterSelect.SelectedValue).Text.split("-")[0] ?? view.WorkCenter,
                    Remark = view.PlannerRemark,
                    CreateBy = AppSession.CurrentUser.EmployeeId,
                }).ToList();


                // 3. 检查任务是否存在
                var tasks = await _facade.Task.GetByTaskNoAsync(taskNo);
                if (tasks != null && tasks.Any())
                {
                    // ... (保留原有的复杂逻辑：检查工位是否一致) ...
                    // 简单来说：如果工位已存在任务，检查完成数量是否一致；如果有新工位，报错提示

                    var existingStationIds = tasks.Select(t => t.WorkStationId).Where(id => id != null).Select(id => (int)id).ToList();
                    var selectedStationIds = _workStations.Select(s => int.Parse(s.Name)).ToList();

                    if (selectedStationIds.Except(existingStationIds).Any() && !selectedStationIds.Intersect(existingStationIds).Any())
                    {
                        // 创建新任务
                        var rtn = await _facade.Task.BatchCreateAsync(createDtos);
                        if (rtn)
                        {
                            var temp = await _facade.Task.GetByTaskNoAsync(taskNo);
                            _workOrderTaskDtos = temp.Where(x => _workStations.Select(s => int.Parse(s.Name)).Contains((int)x.WorkStationId)).ToList();
                        }
                    }
                    else if (selectedStationIds.Except(existingStationIds).Any() && selectedStationIds.Intersect(existingStationIds).Any())
                    {
                        //如果已选择的工位在已有任务中不存在，则提示错误
                        var newStationIds = selectedStationIds.Except(existingStationIds).ToList();
                        var newStations = await _facade.WorkStation.GetByIdAsync(newStationIds);
                        throw new Exception($"当前任务任务已存在，且新增[{string.Join(",", newStations.Select(s => s.WorkStationName))}]未创建任务，请单独对[{string.Join(",", newStations.Select(s => s.WorkStationName))}]报工！");
                    }
                    else
                    {
                        var targetTasks = tasks.Where(x => x.WorkStationId.HasValue && selectedStationIds.Contains(x.WorkStationId.Value)).ToList();

                        var uniqueQuantities = targetTasks.Select(x => x.CompletedQty ?? 0).Distinct().ToList();
                        //判断当前选择的工位与已有任务的工位完成数量是否一致
                        if (uniqueQuantities.Count > 1)
                        {
                            throw new Exception("当前所选工步报工进度不一致，无法同时报工，请重新选择！");
                        }
                        // 逻辑简化示例：
                        _workOrderTaskDtos = tasks.Where(x => selectedStationIds.Contains((int)x.WorkStationId)).ToList();
                    }

                }
                else
                {
                    //非检验工作中心验证材料准备
                    if (!fqcFlag)
                    {
                        //验证当前是否选择材料准备，如果没有选择材料准备，则不允许创建任务
                        var workStations = await _facade.WorkStation.GetByIdAsync(_workStations.Select(s => int.Parse(s.Name)).ToList());
                        if (!workStations.Any(x => x.IsStartStep))
                        {
                            throw new Exception("当前订单首次创建任务未勾选材料准备，请选择材料准备后重试！");
                        }
                    }


                    // 创建新任务
                    var rtn = await _facade.Task.BatchCreateAsync(createDtos);
                    if (rtn)
                    {
                        var temp = await _facade.Task.GetByTaskNoAsync(taskNo);
                        _workOrderTaskDtos = temp.Where(x => _workStations.Select(s => int.Parse(s.Name)).Contains((int)x.WorkStationId)).ToList();
                    }
                }

                // 4. 打开详情页 (使用 Factory)
                _formFactory.OpenDrawer<WorkOrderReportAssmDetailForm>(this.ParentForm, form =>
                {
                    // 使用 InitData 传参
                    // 注意：需要重构 WorkOrderReportAssmDetailForm 支持 InitData
                    form.InitData(_currentOrderInPress, _workOrderTaskDtos);

                    form.LoadOrderData += LoadOrderDataAsync;
                });
            });
        }

        private void ToggleInputs(bool enable)
        {
            startDatePicker.Enabled = enable;
            endDatePicker.Enabled = enable;
            workcenterSelect.Enabled = enable;
            workstationSelect.Enabled = enable;
            currentWorkCenterSelect.Enabled = enable;

            if (!enable)
            {
                stationConfirmButtom.Text = "解除确认";
                stationConfirmButtom.Type = AntdUI.TTypeMini.Error;
                orderInput.Enabled = true;
            }
            else
            {
                executeButton.Enabled = false;
                saveButton.Enabled = false;
                suspendButton.Enabled = false;
                stationConfirmButtom.Text = "工位确认";
                stationConfirmButtom.Type = AntdUI.TTypeMini.Primary;
                orderInput.Enabled = false;
            }
        }

        private async void orderTable_CellClick(object sender, TableClickEventArgs e)
        {
            await RunAsync(async () => 
            {
                if (stationConfirmButtom.Text.Contains("工位确认"))
                    throw new Exception("请先进行工位确认！");
                if (e.Record is V_WorkOrderInProgressView data)
                {
                    _currentOrderInPress = (await _facade.View.GetListByProcessIdAsync(new List<int>() 
                    {
                    data.OrderProcessId
                    })).FirstOrDefault();

                    if (_currentOrderInPress != null)
                    {
                        var bom = await _facade.WorkOrderBomItemService.GetListByOrderIdAync(_currentOrderInPress.OrderId);
                        if (bom != null)
                        {
                            pickingTable.DataSource = bom.Where(x => x.ConsumeType != null).Select(x => new
                            {
                                OrderNumber = _currentOrderInPress.OrderNumber,
                                MaterialCode = x.MaterialCode,
                                BomItem = x.BomItem,
                                ConsumeType = ((ConsumeType)x.ConsumeType).GetDescription(), // 简化转换
                                RequiredQuantity = x.RequiredQuantity,
                                MaterialDesc = x.MaterialDesc,
                            }).ToList();
                        }

                        executeButton.Enabled = true;
                        saveButton.Enabled = true;
                        suspendButton.Enabled = true;
                    }
                    else
                    {
                        throw new Exception("未查询到所选择的订单工序，请检查订单号！");
                    }
                }
                   
            });
        }
    }
}
