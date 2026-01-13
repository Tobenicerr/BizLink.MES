using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.Shared.Helpers;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Infrastructure;
using Dm.util;
using DocumentFormat.OpenXml.Wordprocessing;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace BizLink.MES.WinForms.Forms
{
    public partial class CableTaskReportForm : MesBaseForm
    {
        private readonly CableTaskModuleFacade _facade;
        private readonly IFormFactory _formFactory;
        private bool _isProgrammaticPageChange = false;


        public CableTaskReportForm(
           CableTaskModuleFacade facade,
           IFormFactory formFactory)
        {
            _facade = facade;
            _formFactory = formFactory;

            InitializeComponent();
            InitializeTable();
        }

        // 3. 窗体加载
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 初始化 UI 占位符
            keywordInput.PlaceholderText = "请输入关键字...";
            OrderInput.PlaceholderText = "请输入订单号...";
            startDatePicker.PlaceholderText = "请选择开工日期...";
            workcenterSelect.PlaceholderText = "请选择工作中心...";
            workstationSelect.PlaceholderText = "请选择工位...";
            statusSelect.PlaceholderText = "请选择订单状态...";

            InitStatusSelect();

            // 异步加载下拉框数据
            await RunAsync(async () =>
            {
                await LoadWorkCentersAndStationsAsync();
            });
        }


        #region Data Loading

        private async void queryButton_Click(object sender, EventArgs e)
        {
            // 使用 RunAsync 托管 Loading 和 异常
            await RunAsync(SearchButton, async () =>
            {
                SpinControl.Visible = true;
                _isProgrammaticPageChange = true;
                try
                {
                    // 这里的赋值可能会触发 ValueChanged 事件
                    // 但因为标志位为 true，事件内部会直接 return，不会执行查询
                    PaginationControl.Current = 1;
                }
                finally
                {
                    // 2. 无论如何，必须关闭锁，恢复正常状态
                    _isProgrammaticPageChange = false;
                }
                // 校验
                //if (string.IsNullOrEmpty(OrderInput.Text) && startDatePicker.Value == null &&
                //    workcenterSelect.SelectedValue == null && workstationSelect.SelectedValue == null)
                //{
                //    throw new Exception("请至少输入一个查询条件！");
                //}

                // 加载数据
                var count = await LoadDataAsync();
                AntdUI.Message.success(this, $"查询成功：共查询出{count}条记录");
                SpinControl.Visible = false;
            });
        }

        private async Task<int> LoadDataAsync()
        {
            TableControl.DataSource = null;

            var pageIndex = PaginationControl.Current;
            var pageSize = PaginationControl.PageSize;
            var orders = OrderInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            int? wcId = workcenterSelect.SelectedValue == null ? null : int.Parse(((MenuItem)workcenterSelect.SelectedValue).Name);
            int? wsId = workstationSelect.SelectedValue == null ? null : int.Parse(((MenuItem)workstationSelect.SelectedValue).Name);
            string? status = statusSelect.SelectedValue == null ? null : ((MenuItem)statusSelect.SelectedValue).Name;
            // 调用 Facade
            var result = await _facade.View.GetCableTaskPageListAsync(
                pageIndex,
                pageSize,
                keywordInput.Text.Trim(),
                orders,
                startDatePicker.Value,
                wcId,
                wsId,
                status);

            if (result != null && result.TotalCount > 0)
            {
                // 绑定数据
                TableControl.DataSource = result.Items.Select(x => new CableTaskReportView
                {
                    OrderId = x.OrderId,
                    OrderProcessId = x.OrderProcessId,
                    ProfitCenter = x.ProfitCenter,
                    OrderNumber = x.OrderNumber,
                    MaterialCode = x.MaterialCode,
                    Operation = x.Operation,
                    WorkcenterCode = x.WorkCenter,
                    WorkSationCode = x.WorkStationCode,
                    StartTime = x.StartTime,
                    FactoryCode = x.FactoryCode,
                    Status = x.TaskCompletedQty == null ? "未开始":(x.TaskQuantity == x.TaskCompletedQty ? "已完成":"执行中"),
                    DispatchDate = x.DispatchDate,
                    ActStartTime = x.ActStartTime,
                    TaskId = x.TaskId,
                    CableItem = x.CableItem,
                    CableMaterialCode = x.CableMaterial,
                    Quantity = x.TaskQuantity,
                    CableCompleteQuantity = x.TaskCompletedQty
                }).ToList();

                PaginationControl.Total = result.TotalCount; // 别忘了更新分页控件的总数
                return result.TotalCount;
            }

            return 0;
        }

        private async Task LoadWorkCentersAndStationsAsync()
        {
            // 加载工作中心
            var workcenters = await _facade.WorkCenter.GetAllAsync(AppSession.CurrentFactoryId);
            if (workcenters != null && workcenters.Count > 0)
            {
                workcenterSelect.Items.AddRange(workcenters.Where(x => x.WorkAreaId == 1).Select(s => new MenuItem
                {
                    Name = s.Id.ToString(),
                    Text = $"{s.WorkCenterCode}-{s.WorkCenterName}"
                }).ToArray());
            }

            // 加载工位
            var workstations = await _facade.WorkStation.GetByWorkcenterGroupCodeAsync("CABLECUT");
            if (workstations != null && workstations.Count > 0)
            {
                workstationSelect.Items.AddRange(workstations.OrderBy(s => s.WorkStationCode.SafeSubstring(1, 6)).Select(s => new MenuItem
                {
                    Name = s.Id.ToString(),
                    Text = $"{s.WorkStationCode}-{s.WorkStationName}"
                }).ToArray());
            }
        }

        // --- 展开详情 ---
        private async void orderTable_ExpandChanged(object sender, TableExpandEventArgs e)
        {
            if (e.Expand && e.Record is CableTaskReportView data)
            {
                try
                {
                    // 1. 获取报工记录
                    var taskConfirms = await _facade.Confirm.GetListByTaskIdAsync((int)data.TaskId);

                    if (taskConfirms != null)
                    {
                        var detailList = taskConfirms.OrderByDescending(x => x.ConfirmNumber)
                       .Select(x => new CableTaskReportDetailView(x)).ToList();

                        // 2. 补充信息 (上料时间、人员姓名)
                        foreach (var detailView in detailList)
                        {
                            // 获取最早的上料记录时间作为开始时间
                            var materials = await _facade.MaterialAdd.GetByTaskIdAsync(detailView.TaskId);
                            var firstMaterial = materials.OrderBy(x => x.CreateOn).FirstOrDefault();
                            if (firstMaterial != null)
                            {
                                detailView.StartDate = firstMaterial.CreateOn;
                            }

                            // 获取人员姓名
                            var user = await _facade.UserService.GetByEmployeeIdAsync(detailView.EmployerCode);
                            if (user != null)
                            {
                                detailView.EmployerCode += $" ({user.UserName})";
                            }
                        }

                        data.Children = detailList.ToArray();
                    }
                    else
                        throw new Exception("未查询到相关的报工记录！");
                   
                }
                catch (Exception ex)
                {
                    AntdUI.Message.error(this, "加载详情失败: " + ex.Message);
                }
            }
        }

        #endregion

        #region Operations (Restart & Reprint)

        private async void orderTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            // A. 主表操作：重启订单
            if (e.Record is CableTaskReportView main && e.Btn.Id == "restart")
            {
                if (e.Btn.Id == "restart")
                {
                    await RunAsync(async () =>
                    {
                        var task = await _facade.Task.GetByIdAsync((int)main.TaskId);
                        if (task == null)
                            throw new Exception("任务不存在！");

                        if (task.Quantity <= task.CompletedQty)
                            throw new Exception("订单已完成，无法重启！");

                        if (task.Status != ((int)WorkOrderStatus.Finished).ToString())
                            throw new Exception("订单未完成，无需重启！");

                        // 更新任务状态
                        bool result = await _facade.Task.UpdateAsync(new WorkOrderTaskUpdateDto
                        {
                            Id = task.Id,
                            Status = ((int)WorkOrderStatus.OnGoing).ToString(),
                            UpdateBy = AppSession.CurrentUser.EmployeeId,
                            UpdateOn = DateTime.Now
                        });

                        if (result)
                        {
                            // 将“完工”状态的 Confirm 记录回退为“正常”状态
                            var confirms = await _facade.Confirm.GetListByTaskIdAsync(task.Id);
                            var finishedConfirm = confirms.FirstOrDefault(x => x.Status == "1");

                            if (finishedConfirm != null)
                            {
                                await _facade.Confirm.UpdateAsync(new WorkOrderTaskConfirmUpdateDto
                                {
                                    Id = finishedConfirm.Id,
                                    Status = "0"
                                });
                            }
                            // 可选：刷新列表 await LoadDataAsync();
                        }

                    }, successMsg: "重启成功，请查看断线报工界面！", confirmMsg: "即将重启断线任务，是否继续？");
                }

                else if (e.Btn.Id == "delete")
                {
                    await RunAsync(async () =>
                    {
                        var task = await _facade.Task.GetByIdAsync((int)main.TaskId);
                        if (task == null)
                            throw new Exception("任务不存在！");

                        if (task.Status == ((int)WorkOrderStatus.Finished).ToString())
                            throw new Exception("断线任务完成，无需强制关闭！");
                        // 更新任务状态
                        bool result = await _facade.Task.UpdateAsync(new WorkOrderTaskUpdateDto
                        {
                            Id = task.Id,
                            Status = ((int)WorkOrderStatus.Finished).ToString(),
                            Remark = "强制关闭",
                            UpdateBy = AppSession.CurrentUser.EmployeeId,
                            UpdateOn = DateTime.Now
                            
                        });

                        if (result)
                        {
                            // 将“完工”状态的 Confirm 记录回退为“正常”状态
                            var confirms = await _facade.Confirm.GetListByTaskIdAsync(task.Id);
                            var finishedConfirm = confirms.OrderByDescending(x => x.Id).FirstOrDefault();

                            if (finishedConfirm != null)
                            {
                                await _facade.Confirm.UpdateAsync(new WorkOrderTaskConfirmUpdateDto
                                {
                                    Id = finishedConfirm.Id,
                                    Status = "1"
                                });
                            }
                            // 可选：刷新列表 await LoadDataAsync();
                        }

                    }, successMsg: "关闭成功，当前断线任务已完成！", confirmMsg: "即将关闭断线任务，是否继续？");
                }

            }

            // B. 子表操作：补打标签
            else if (e.Record is CableTaskReportDetailView data && e.Btn.Id == "reprint")
            {
                await RunAsync(async () =>
                {
                    // 1. 选择打印机
                    string chosenPrinter = null;
                    _formFactory.Show<PrinterSelectForm>(form =>
                    {
                        form.FormClosed += (s, args) =>
                        {
                            if (form.DialogResult == DialogResult.OK)
                                chosenPrinter = form.SelectedPrinterName;
                        };
                    }, isModal: true);

                    if (string.IsNullOrWhiteSpace(chosenPrinter))
                        throw new Exception("打印已取消或未选择打印机！");

                    var printType = chosenPrinter.Contains(PrinterType.NetworkPrinter.GetDescription())
                        ? PrinterType.NetworkPrinter : PrinterType.LocalPrinter;

                    // 2. 获取打印所需数据
                    var workOrderTask = await _facade.Task.GetByIdAsync(data.TaskId);
                    var workOrder = await _facade.WorkOrderService.GetByIdAsync(workOrderTask.OrderId);
                    var confirm = await _facade.Confirm.GetByIdAsync(data.Id);

                    // 获取消耗批次
                    var consumList = await _facade.Consum.GetListByConfirmIdAsync(data.Id);
                    var batchCode = consumList.FirstOrDefault(x => x.MovementType == "261")?.BatchCode;

                    if (string.IsNullOrEmpty(batchCode))
                        throw new Exception("未找到关联的物料消耗批次！");

                    // 3. 执行打印
                    bool rtn = LabelPrintHelper.CuttingReportPrinter(
                        workOrderTask,
                        confirm,
                        workOrder.ProfitCenter,
                        batchCode,
                        printType,
                        chosenPrinter.Split('-')[0]);

                    if (!rtn)
                        throw new Exception("打印失败，请检查打印机或网络状态！");

                }, successMsg: "标签补打成功！", confirmMsg: "即将补打过账标签，是否继续？");
            }
        }

        #endregion



        #region UI Helpers

        private void InitStatusSelect()
        {
            statusSelect.Items.AddRange(new MenuItem[] {
                new MenuItem{ Name=string.Empty, Text=string.Empty},
                new MenuItem{ Name="1", Text="未开始"},
                new MenuItem{ Name="2", Text="执行中"},
                new MenuItem{ Name="4", Text="已完成"},
            });
        }

        private void InitializeTable()
        {

            TableControl.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("OrderNumber", "订单号", AntdUI.ColumnAlign.Center)
                {
                   KeyTree = "Children"

                }.SetFixed().SetWidth("auto").SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("StartTime", "开工日期", AntdUI.ColumnAlign.Center).SetFixed().SetDisplayFormat("yyyy-MM-dd").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "订单物料", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CableItem", "BOM行", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CableMaterialCode", "断线物料", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "计划数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CableCompleteQuantity", "完成数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "未开始" => new AntdUI.CellTag("未开始", AntdUI.TTypeMini.Error),
                            "已完成" => new AntdUI.CellTag("已完成", AntdUI.TTypeMini.Success),
                            "执行中" => new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Primary),
                            _ => null
                        };
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConfirmNo", "标签ID", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("StartDate", "上料时间", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConfirmDate", "报工时间", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConfirmQty", "报工数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("EmployerCode", "报工人员", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CompletedStatus", "完工标志", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                        {
                            switch (value as string)
                            {

                                case "完工":
                                    return new AntdUI.CellTag("完工", AntdUI.TTypeMini.Success);
                                default:
                                    return null;
                            }
                        }
                }.SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Remark", "备注", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Operation", "工序", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkcenterCode", "工作中心", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkSationCode", "工位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                  new AntdUI.Column("FactoryCode", "工厂", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
   
                new AntdUI.Column("DispatchDate", "装配日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),

                //{
                //    LocalizationTitle ="Table.Column.{id}",
                //    Call = (value, record, i_row, i_col) => {
                //        System.Threading.Thread.Sleep(2000);
                //        return value;
                //    }
                //}
               new AntdUI.Column("Btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),

             };

        }

        private async void PaginationControl_ValueChanged(object sender, PagePageEventArgs e)
        {
            if (_isProgrammaticPageChange)
                return;
            await LoadDataAsync();
        }

        private void OrderInput_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion



        //private async void CableTaskReportForm_Load(object sender, EventArgs e)
        //{
        //    keywordInput.PlaceholderText = "请输入关键字...";
        //    OrderInput.PlaceholderText = "请输入订单号...";
        //    startDatePicker.PlaceholderText = "请选择开工日期...";
        //    workcenterSelect.PlaceholderText = "请选择工作中心...";
        //    workstationSelect.PlaceholderText = "请选择工位...";
        //    statusSelect.PlaceholderText = "请选择订单状态...";

        //    var workcenters = await _workCenterService.GetAllAsync(Common.AppSession.CurrentFactoryId);
        //    if (workcenters != null && workcenters.Count() > 0)
        //    {
        //        workcenterSelect.Items.AddRange(workcenters.Where(x => x.WorkAreaId == 1).Select(s => new MenuItem
        //        {
        //            Name = s.Id.ToString(),
        //            Text = s.WorkCenterCode + "-" + s.WorkCenterName
        //        }).ToArray());
        //    }



        //    ///获取断线组下所有工作中心的工位
        //    var workstations = await _workStationService.GetByWorkcenterGroupCodeAsync("CABLECUT");
        //    if (workstations != null && workstations.Count() > 0)
        //    {
        //        workstationSelect.Items.AddRange(workstations.OrderBy(s => s.WorkStationCode.SafeSubstring(1, 6)).Select(s => new MenuItem
        //        {
        //            Name = s.Id.ToString(),
        //            Text = s.WorkStationCode + "-" + s.WorkStationName
        //        }).ToArray());
        //    }

        //    statusSelect.Items.AddRange(new MenuItem[] {
        //        new MenuItem{ Name=string.Empty, Text=string.Empty},
        //        new MenuItem{ Name="未开始", Text="未开始"},
        //        new MenuItem{ Name="执行中", Text="执行中"},
        //        new MenuItem{ Name="已完成", Text="已完成"},
        //    });
        //}

        //private async void queryButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        SearchButton.Loading = true;
        //        SpinControl.Visible = true;
        //        if (string.IsNullOrEmpty(OrderInput.Text) && startDatePicker.Value == null && workcenterSelect.SelectedValue == null && workstationSelect.SelectedValue == null)
        //            throw new Exception("请至少输入一个查询条件！");
        //        await LoadData();
        //    }
        //    catch (Exception ex)
        //    {

        //        AntdUI.Message.error(this, $"查询失败：{ex.Message}");
        //    }
        //    finally
        //    {
        //        SearchButton.Loading = false;
        //        SpinControl.Visible = false;

        //    }
        //}

        //private async void orderTable_ExpandChanged(object sender, TableExpandEventArgs e)
        //{
        //    if (e.Expand && e.Record is CableTaskReportView data)
        //    {
        //        var taskConfirms = await _workOrderTaskConfirmService.GetListByTaskIdAsync(data.TaskId);

        //        data.Children = taskConfirms.OrderByDescending(x => x.ConfirmNumber).Select(x => new CableTaskReportDetailView(x)).ToArray();
        //        var detailViews = data.Children as CableTaskReportDetailView[];
        //        foreach ( var detailView in detailViews ) 
        //        {
        //            var startDate = (await _workOrderTaskMaterialAddService.GetByTaskIdAsync(detailView.TaskId)).OrderBy(x => x.CreateOn).FirstOrDefault();
        //            if(startDate != null)
        //                detailView.StartDate = startDate.CreateOn;
        //            var user = await _userService.GetByEmployeeIdAsync(detailView.EmployerCode);
        //            if (user != null)
        //                detailView.EmployerCode += user.UserName;
        //        }
        //    }
        //}

        //private async void orderTable_CellButtonClick(object sender, TableButtonEventArgs e)
        //{
        //    try
        //    {
        //        if (e.Record is CableTaskReportView main)
        //        {
        //            if (e.Btn.Id == "restart")
        //            {
        //                var task = await _workOrderTaskService.GetByIdAsync(main.TaskId);
        //                if (task != null)
        //                {
        //                    if (task.Quantity <= task.CompletedQty)
        //                        throw new Exception("订单已完成，无法重启！");
        //                    else if (task.Status != ((int)WorkOrderStatus.Finished).ToString())
        //                        throw new Exception("订单未完成，无需重启！");
        //                    else
        //                    {
        //                        if (AntdUI.Modal.open(this.ParentForm, "订单重开", "即将重新打开订单允许继续报工，是否继续？") == DialogResult.OK)
        //                        {
        //                            var result = await _workOrderTaskService.UpdateAsync(new WorkOrderTaskUpdateDto()
        //                            {
        //                                Id = task.Id,
        //                                Status = ((int)WorkOrderStatus.OnGoing).ToString()
        //                            });
        //                            if (result)
        //                            {
        //                                var confirm = (await _workOrderTaskConfirmService.GetListByTaskIdAsync(task.Id)).Where(x => x.Status == "1").FirstOrDefault();
        //                                if (confirm != null)
        //                                {
        //                                    result = await _workOrderTaskConfirmService.UpdateAsync(new WorkOrderTaskConfirmUpdateDto()
        //                                    {
        //                                        Id = confirm.Id,
        //                                        Status = "0",
        //                                    });
        //                                }

        //                                AntdUI.Message.success(this.ParentForm, "操作成功，请查看断线报工界面！");
        //                            }
        //                        }

        //                    }
        //                }
        //                else
        //                    throw new Exception("订单未完成，无需重启！");

        //            }
        //        }
        //        else if (e.Record is CableTaskReportDetailView data)
        //        {

        //            if (e.Btn.Id == "reprint")
        //            {
        //                try
        //                {
        //                    if (AntdUI.Modal.open(this.ParentForm, "过账标签补打", "即将补打过账标签，是否继续？") == DialogResult.OK)
        //                    {
        //                        string chosenPrinter = string.Empty;
        //                        using (PrinterSelectForm form = new PrinterSelectForm())
        //                        {
        //                            DialogResult result = form.ShowDialog();
        //                            if (result == DialogResult.OK)
        //                            {
        //                                chosenPrinter = form.SelectedPrinterName;
        //                                if (string.IsNullOrWhiteSpace(chosenPrinter))
        //                                {
        //                                    throw new Exception("未查询到打印机，请重新选择！");
        //                                }
        //                            }
        //                            else
        //                            {
        //                                throw new Exception("已取消打印！");
        //                            }

        //                        }

        //                        PrinterType printType;
        //                        if (chosenPrinter.Split("-")[1] == PrinterType.NetworkPrinter.GetDescription())
        //                        {
        //                            printType = PrinterType.NetworkPrinter;
        //                        }
        //                        else
        //                            printType = PrinterType.LocalPrinter;

        //                        var workOrderTask = await _workOrderTaskService.GetByIdAsync(data.TaskId);
        //                        var workorder = await _workOrderService.GetByIdAsync(workOrderTask.OrderId);


        //                        var confirm = await _workOrderTaskConfirmService.GetByIdAsync(data.Id);
        //                        var consumList = await _workOrderTaskConsumService.GetListByConfirmIdAsync(data.Id);
        //                        var batchcode = consumList.Where(x => x.MovementType == "261").Select(x => x.BatchCode).First();
        //                        var rtn = LabelPrintHelper.CuttingReportPrinter(workOrderTask, confirm, workorder.ProfitCenter, batchcode, printType, chosenPrinter.Replace("-" + printType.GetDescription(), string.Empty));

        //                        if (!rtn)
        //                            throw new Exception("请检查打印机或网络状态！");
        //                    }
        //                    AntdUI.Message.success(this, "标签打印成功！");
        //                }
        //                catch (Exception ex)
        //                {
        //                    AntdUI.Message.error(this, $"标签打印失败：{ex.Message}");
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        AntdUI.Message.error(this.ParentForm,$"操作失败：{ex.Message}");
        //    }

        //}

    }
    public class CableTaskReportView : AntdUI.NotifyProperty
    {
        int _orderId;
        public int OrderId
        {
            get => _orderId;
            set
            {
                if (_orderId == value)
                    return;
                _orderId = value;
                OnPropertyChanged();
            }
        }

        int _orderProcessId;
        public int OrderProcessId
        {
            get => _orderProcessId;
            set
            {
                if (_orderProcessId == value)
                    return;
                _orderProcessId = value;
                OnPropertyChanged();
            }
        }

        string? _profitCenter;
        public string? ProfitCenter
        {
            get => _profitCenter;
            set
            {
                if (_profitCenter == value)
                    return;
                _profitCenter = value;
                OnPropertyChanged();
            }
        }
        string? _orderNumber;
        public string? OrderNumber
        {
            get => _orderNumber;
            set
            {
                if (_orderNumber == value)
                    return;
                _orderNumber = value;
                OnPropertyChanged();
            }
        }
        string? _materialCode;
        public string? MaterialCode
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

        string? _operation;
        public string? Operation
        {
            get => _operation;
            set
            {
                if (_operation == value)
                    return;
                _operation = value;
                OnPropertyChanged();
            }
        }

        string? _workcenterCode;
        public string? WorkcenterCode
        {
            get => _workcenterCode;
            set
            {
                if (_workcenterCode == value)
                    return;
                _workcenterCode = value;
                OnPropertyChanged();
            }
        }

        decimal? _quantity;
        public decimal? Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                    return;
                _quantity = value;
                OnPropertyChanged();
            }
        }

        int _cableCount;
        public int CableCount
        {
            get => _cableCount;
            set
            {
                if (_cableCount == value)
                    return;
                _cableCount = value;
                OnPropertyChanged();
            }
        }

        DateTime? _dispatchDate;
        public DateTime? DispatchDate
        {
            get => _dispatchDate;
            set
            {
                if (_dispatchDate == value)
                    return;
                _dispatchDate = value;
                OnPropertyChanged();
            }
        }
        DateTime? _startTime;
        public DateTime? StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime == value)
                    return;
                _startTime = value;
                OnPropertyChanged();
            }
        }

        DateTime? _actstartTime;
        public DateTime? ActStartTime
        {
            get => _actstartTime;
            set
            {
                if (_actstartTime == value)
                    return;
                _actstartTime = value;
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
        string? _factoryCode;
        public string? FactoryCode
        {
            get => _factoryCode;
            set
            {
                if (_factoryCode == value)
                    return;
                _factoryCode = value;
                OnPropertyChanged();
            }
        }
        int? _taskId;
        public int? TaskId
        {
            get => _taskId;
            set
            {
                if (_taskId == value)
                    return;
                _taskId = value;
                OnPropertyChanged();
            }
        }
        string? _cableMaterialCode;
        public string? CableMaterialCode
        {
            get => _cableMaterialCode;
            set
            {
                if (_cableMaterialCode == value)
                    return;
                _cableMaterialCode = value;
                OnPropertyChanged();
            }
        }

        decimal? _cableQuantity;
        public decimal? CableQuantity
        {
            get => _cableQuantity;
            set
            {
                if (_cableQuantity == value)
                    return;
                _cableQuantity = value;
                OnPropertyChanged();
            }
        }

        decimal? _cableCompleteQuantity;
        public decimal? CableCompleteQuantity
        {
            get => _cableCompleteQuantity;
            set
            {
                if (_cableCompleteQuantity == value)
                    return;
                _cableCompleteQuantity = value;
                OnPropertyChanged();
            }
        }

        string? _workSationCode;
        public string? WorkSationCode
        {
            get => _workSationCode;
            set
            {
                if (_workSationCode == value)
                    return;
                _workSationCode = value;
                OnPropertyChanged();
            }
        }

        string? _cableItem;
        public string? CableItem
        {
            get => _cableItem;
            set
            {
                if (_cableItem == value)
                    return;
                _cableItem = value;
                OnPropertyChanged();
            }
        }

        CableTaskReportDetailView[] _children = new CableTaskReportDetailView[1];
        public CableTaskReportDetailView[] Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }

        AntdUI.CellLink[] _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("restart", "任务重启", AntdUI.TTypeMini.Primary),
                        new AntdUI.CellButton("delete", "任务关闭", AntdUI.TTypeMini.Error),
                    };
    public AntdUI.CellLink[] Btns
        {
            get => _btns;
            set
            {
                if (_btns == value)
                    return;
                _btns = value;
                OnPropertyChanged();
            }
        }

    }

    public class CableTaskReportDetailView : AntdUI.NotifyProperty
    {
        public CableTaskReportDetailView(WorkOrderTaskConfirmDto dto)
        {
            _id = dto.Id;
            _taskId = dto.TaskId;
            _confirmNo = dto.ConfirmNumber;
            _confirmDate = dto.ConfirmDate;
            _confirmQty = dto.ConfirmQuantity;
            _employerCode = dto.EmployerCode;
            _completedStatus = dto.Status == "1" ?"完工":string.Empty;
            _remark = dto.Remark;
            _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("reprint", "标签补打", AntdUI.TTypeMini.Primary),
                    };
        }

        int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;
                _id = value;
                OnPropertyChanged();
            }
        }
        int _taskId;
        public int TaskId
        {
            get => _taskId;
            set
            {
                if (_taskId == value)
                    return;
                _taskId = value;
                OnPropertyChanged();
            }
        }

        string? _confirmNo;
        public string? ConfirmNo
        {
            get => _confirmNo;
            set
            {
                if (_confirmNo == value)
                    return;
                _confirmNo = value;
                OnPropertyChanged();
            }
        }

        DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value)
                    return;
                _startDate = value;
                OnPropertyChanged();
            }
        }
        DateTime? _confirmDate;
        public DateTime? ConfirmDate
        {
            get => _confirmDate;
            set
            {
                if (_confirmDate == value)
                    return;
                _confirmDate = value;
                OnPropertyChanged();
            }
        }
        decimal? _confirmQty;
        public decimal? ConfirmQty
        {
            get => _confirmQty;
            set
            {
                if (_confirmQty == value)
                    return;
                _confirmQty = value;
                OnPropertyChanged();
            }
        }
        string? _employerCode;
        public string? EmployerCode
        {
            get => _employerCode;
            set
            {
                if (_employerCode == value)
                    return;
                _employerCode = value;
                OnPropertyChanged();
            }
        }
        string? _completedStatus;
        public string? CompletedStatus
        {
            get => _completedStatus;
            set
            {
                if (_completedStatus == value)
                    return;
                _completedStatus = value;
                OnPropertyChanged();
            }
        }
        string? _remark;
        public string? Remark
        {
            get => _remark;
            set
            {
                if (_remark == value)
                    return;
                _remark = value;
                OnPropertyChanged();
            }
        }
        AntdUI.CellLink[] _btns;
        public AntdUI.CellLink[] Btns
        {
            get => _btns;
            set
            {
                if (_btns == value)
                    return;
                _btns = value;
                OnPropertyChanged();
            }
        }
    }
}
