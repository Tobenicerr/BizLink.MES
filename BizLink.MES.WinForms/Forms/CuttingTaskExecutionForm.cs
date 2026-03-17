using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Helpers;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using Dm.util;
using DocumentFormat.OpenXml.Bibliography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class CuttingTaskExecutionForm : MesBaseForm
    {
        private readonly IWorkCenterService _workCenterService;
        private readonly IWorkStationService _workStationService;
        private readonly IFactoryService _factoryService;
        private readonly IPendingMaterialTaskCuttingService _pendingMaterialTaskCuttingService;
        private readonly ITaskGenerationService _taskGenerationService;
        private readonly ITaskExecutionService _taskExecutionService;
        private readonly IStationMaterialLoadingService _stationMaterialLoadingService;
        private readonly IWorkOrderMaterialTaskService _workOrderMaterialTaskService;
        private readonly IParameterGroupService _parameterGroupService;
        private readonly IWorkOrderBomItemService _workOrderBomItemService;
        private readonly IWorkOrderService _workOrderService;
        private readonly IWorkOrderProcessService _workOrderProcessService;
        private readonly IWarehouseLocationService _warehouseLocationService;
        private readonly IWorkOrderTaskExecuteLogService _workOrderTaskExecuteLogService;
        private readonly IWorkOrderTaskExecuteConsumpService _workOrderTaskExecuteConsumpService;
        private readonly IFormFactory _formFactory;



        private const string CUTTING_WORKCENT_GROUP = "CABLECUT";
        private const string Group_CableCutScrapReason = "CableCutScrapReason";
        private const string Group_CableSampleRunScrapLen = "CableSampleRunScrapLen";
        private const string Key_DefaultLength = "DefaultLength";
        private Dictionary<int, string> _stationPrinters = new Dictionary<int, string>();

        private PendingMaterialTaskCuttingDto? _currentPendingTask = null;
        private WorkOrderMaterialTaskDto? _currentStartTask = null;
        private WorkStationDto? _currentStation = null;
        //private string? _currentPrinter = null;
        private DateTime? _workStartTime = null;
        private StationMaterialLoadingDto? _currentStationMaterialLoading = null;


        public CuttingTaskExecutionForm(IWorkCenterService workCenterService, IWorkStationService workStationService, IFactoryService factoryService, IPendingMaterialTaskCuttingService pendingMaterialTaskCuttingService, ITaskGenerationService taskGenerationService, ITaskExecutionService taskExecutionService, IStationMaterialLoadingService stationMaterialLoadingService, IWorkOrderMaterialTaskService workOrderMaterialTaskService, IParameterGroupService parameterGroupService, IWorkOrderBomItemService workOrderBomItemService, IWorkOrderService workOrderService, IWorkOrderProcessService workOrderProcessService, IWarehouseLocationService warehouseLocationService, IWorkOrderTaskExecuteLogService workOrderTaskExecuteLogService, IWorkOrderTaskExecuteConsumpService workOrderTaskExecuteConsumpService, IFormFactory formFactory)
        {
            InitializeComponent();
            InitializeTable();
            _workCenterService = workCenterService;
            _workStationService = workStationService;
            _factoryService = factoryService;
            _pendingMaterialTaskCuttingService = pendingMaterialTaskCuttingService;
            _taskGenerationService = taskGenerationService;
            _taskExecutionService = taskExecutionService;
            _stationMaterialLoadingService = stationMaterialLoadingService;
            _workOrderMaterialTaskService = workOrderMaterialTaskService;
            _parameterGroupService = parameterGroupService;
            _workOrderBomItemService = workOrderBomItemService;
            _workOrderService = workOrderService;
            _workOrderProcessService = workOrderProcessService;
            _warehouseLocationService = warehouseLocationService;
            _workOrderTaskExecuteLogService = workOrderTaskExecuteLogService;
            _workOrderTaskExecuteConsumpService = workOrderTaskExecuteConsumpService;
            _formFactory = formFactory;
        }


        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WorkCenterSelect.PlaceholderText = "请选择工作中心";
            PrinterSelect.PlaceholderText = "请选择打印机";
            StartDatePicker.PlaceholderText = "请选择仓库日期";
            WorkStationSelect.PlaceholderText = "请选择工位";

            ToggleInputs(false);
            await LoadWorkCenterSelectAsync();
            await LoadWorkStationSelectAsync();
            await LoadCuttingScrapReasonAsync();
            LoadPrinters();

            OrderNoLabel.Text = string.Empty;
            MatcLabel.Text = string.Empty;
            MatdLabel.Text = string.Empty;
            ProgressLabel.Text = string.Empty;
            CutLendLabel.Text = string.Empty;
            CutLenuLabel.Text = string.Empty;
            CutLenLabel.Text = string.Empty;

        }

        private void InitializeTable()
        {
            TaskTable.Columns = new AntdUI.ColumnCollection()
            {
                new AntdUI.Column("WorkOrderNo", "订单/工序", AntdUI.ColumnAlign.Left)
                    {
                        Render = (value, record, index) =>
                        {
                            if (record is PendingMaterialTaskCuttingDto model)
                                {
                                    // 2. 自由组合多个字段
                                    // 这里演示将 "订单号" 和 "工序" 拼接，中间换行
                                    return model.WorkOrderNo + "\r\n" + model.Operation;
        
                                    // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                    // return new CellText(model.OrderNo, model.Process); 
                                }
                                return value;
                        }
                    }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                 new AntdUI.Column("MaterialCode", "物料", AntdUI.ColumnAlign.Left)
                 {
                        Render = (value, record, index) =>
                        {
                            if (record is PendingMaterialTaskCuttingDto model)
                                {
                                    // 2. 自由组合多个字段
                                    // 这里演示将 "订单号" 和 "工序" 拼接，中间换行
                                    return $"物料代码：{model.MaterialCode}\r\n订单数量：{model.Quantity.ToString("F0")}\r\n断线数量：{(model.CompletedQuantity??0).ToString("F0")} / {(model.TargetQuantity??0).ToString("F0")}";
        
                                    // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                    // return new CellText(model.OrderNo, model.Process); 
                                }
                                return value;
                        }
                 }.SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                 new AntdUI.Column("TargetValue", "断线参数mm", AntdUI.ColumnAlign.Left)
                 {
                        Render = (value, record, index) =>
                        {
                            if (record is PendingMaterialTaskCuttingDto model)
                                {
                                    if(model.CuttingParam == null)
                                        return "断线长度：- \r\n最大长度：- \r\n最小长度：-";
                                    return $"断线长度：{model.CuttingParam?.CuttingLength.Value.ToString("0.0")}\r\n最大长度：{(model.CuttingParam?.BomLength / model.CuttingParam?.CablePcs + (decimal)(model.CuttingParam.UpTol * 0.8m)).Value.ToString("0.0")}\r\n最小长度：{(model.CuttingParam.BomLength / model.CuttingParam.CablePcs -model.CuttingParam.DownTol).Value.ToString("0.0") }";
        
                                    // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                    // return new CellText(model.OrderNo, model.Process); 
                                }
                                return value;
                        }
                 }.SetLocalizationTitleID("Table.Column."),


                  new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),

                  new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                  {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "10" or "20" or "30" => new AntdUI.CellTag("已排产", AntdUI.TTypeMini.Success),
                            "40" => new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Primary),
                            "45" => new AntdUI.CellTag("已挂起", AntdUI.TTypeMini.Warn),
    _                       => new AntdUI.CellTag("未准备", AntdUI.TTypeMini.Error),
                        };
                    }
                  }.SetFixed().SetLocalizationTitleID("Table.Column."),
            };

            LoadingTable.Columns = new AntdUI.ColumnCollection()
            {
                new AntdUI.Column("MaterialCode", "物料代码", AntdUI.ColumnAlign.Left).SetFixed().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BatchCode", "物料批次", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BarCode", "标签ID", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LastQuantity", "可用数量", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetLocalizationTitleID("Table.Column."),

            };

            AllLoadingTable.Columns = new AntdUI.ColumnCollection()
            {
                new AntdUI.Column("WorkStationCode", "工位代码", AntdUI.ColumnAlign.Left).SetFixed().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialCode", "物料代码", AntdUI.ColumnAlign.Left).SetFixed().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BatchCode", "物料批次", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BarCode", "标签ID", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LastQuantity", "可用数量", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),

                //new AntdUI.Column("btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetLocalizationTitleID("Table.Column."),

            };

            BomTable.Columns = new ColumnCollection()
            {
                new AntdUI.Column("MaterialCode", "物料代码", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("RequiredQuantity", "BOM数量", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Unit", "单位", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("TheoreticalConsumption", "理论用量", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "库存不足" => new AntdUI.CellTag("库存不足", AntdUI.TTypeMini.Error),
                            "库存充足" => new AntdUI.CellTag("库存充足", AntdUI.TTypeMini.Success),
    _                       => new AntdUI.CellTag("库存不足", AntdUI.TTypeMini.Error),
                        };
                    }
                }.SetFixed().SetLocalizationTitleID("Table.Column."),
            };

            ExecuteTable.Columns = new ColumnCollection()
            {
                new AntdUI.Column("BatchCode", "报工序号", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CompletedQuantity", "报工数量", AntdUI.ColumnAlign.Center).SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("EmployerCode", "报工人员", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CreatedOn", "报工时间", AntdUI.ColumnAlign.Left).SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "50" => new AntdUI.CellTag("完成", AntdUI.TTypeMini.Success),
    _                       => null,
                        };
                    }
                }.SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetLocalizationTitleID("Table.Column."),

            };
        }

        private void ToggleInputs(bool enable)
        {

            SubmitButton.Enabled = enable;
            SuspendButton.Enabled = enable;
            WorkStartButton.Enabled = enable;
            AdjustButton.Enabled = enable;
            ProcessCardPrintButton.Enabled = enable;
            InspectLabelButton.Enabled = enable;
            SubmitButton.Enabled = enable;


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

        private async Task LoadWorkCenterSelectAsync()
        {
            WorkCenterSelect.Items.Clear();
            var workcenters = await _workCenterService.GetListByGroupCodeAsync(CUTTING_WORKCENT_GROUP);
            if (workcenters.Any())
            {
                WorkCenterSelect.Items.AddRange(workcenters.Select(x => new AntdUI.MenuItem()
                {
                    Name = x.Id.ToString(),
                    Text = $"{x.WorkCenterCode}-{x.WorkCenterName}"
                }).ToArray());
            }

        }

        private async Task LoadWorkStationSelectAsync()
        {
            WorkStationSelect.Items.Clear();
            var workcenters = await _workCenterService.GetListByGroupCodeAsync(CUTTING_WORKCENT_GROUP);
            if (workcenters.Any())
            {
                var stations = await _workStationService.GetListByWorkcenterIdAsync(workcenters.Select(x => x.Id).ToList());
                if (stations.Any())
                {
                    WorkStationSelect.Items.AddRange(stations.Select(x => new AntdUI.MenuItem()
                    {
                        Name = x.Id.ToString(),
                        Text = $"{x.WorkStationCode}-{x.WorkStationName}"
                    }).ToArray());

                    _stationPrinters = stations.Where(x => !string.IsNullOrEmpty(x.PrinterName)).ToDictionary(x => x.Id, x => x.PrinterName);
                }
            }
        }

        private async Task LoadStationMaterialLoadingAsync()
        {
            LoadingTable.DataSource = null;
            AllLoadingTable.DataSource = null;
            var allLoadings = await _stationMaterialLoadingService.GetListByFactoryIdAsync(AppSession.CurrentFactoryId);
            if (allLoadings.Any())
            {
                AllLoadingTable.DataSource = allLoadings.Select(x => new StationMaterialLoadingViewModel(x)).ToList();

            }
            var station = WorkStationSelect.SelectedValue as AntdUI.MenuItem;
            if (station == null || string.IsNullOrEmpty(station.Name))
                throw new Exception("未选择有效工位，请重新选择！");
            if (int.TryParse(station.Name, out int stationId))
            {
                var loadingLogs = await _stationMaterialLoadingService.GetListByStationIdAsync(stationId);
                if (loadingLogs.Any())
                    LoadingTable.DataSource = loadingLogs.Select(x => new StationMaterialLoadingViewModel(x)).ToList();
            }
            else
                throw new Exception("工位数据加载出错，请关闭界面后重新打开！");


        }

        private async Task LoadBomInfoByTaskIdAsync(List<int> materialTaskId)
        {
            var tasks = await _workOrderMaterialTaskService.GetByIdAsync(materialTaskId);
            if (tasks.Any())
            {
                var boms = await _workOrderBomItemService.GetByIdAsync(tasks.Select(x => (int)x.RefSourceId).ToList());
                if (boms.Any())
                {
                    var loadingLogs = await _stationMaterialLoadingService.GetListByStationIdAsync(_currentStation.Id);

                    var stockSummary = loadingLogs.GroupBy(s => s.MaterialCode).ToDictionary(g => g.Key, g => g.Sum(s => s.LastQuantity));

                    // 2. 计算 BOM 和 Task，并按 MaterialCode 汇总
                    var bomSummary = boms
                        .Select(b => new
                        {
                            b.MaterialCode,
                            b.MaterialDesc,
                            b.Unit,
                            b.RequiredQuantity,
                            // 先在内部把当前 BOM 的 Task 消耗算出来 (相当于一次安全匹配，防止空指针)
                            TaskConsumption = tasks.Where(t => t.RefSourceId == b.Id)
                                                   .Sum(t => (t.TargetQuantity - (t.CompletedQuantity ?? 0)) * t.TargetValue / 1000m)
                        })
                        .GroupBy(x => new { x.MaterialCode, x.MaterialDesc, x.Unit })
                        .Select(g => new
                        {
                            g.Key.MaterialCode,
                            g.Key.MaterialDesc,
                            g.Key.Unit,
                            TotalRequiredQty = g.Sum(x => x.RequiredQuantity),
                            TotalConsumption = g.Sum(x => x.TaskConsumption)
                        }).ToList();

                    // 3. 将汇总后的 Bom 与 库存字典进行匹配生成最终结果
                    //bool allowReport = true;
                    BomTable.DataSource = bomSummary.Select(b =>
                    {
                        // 尝试获取库存，如果没有相关记录则默认为 0
                        stockSummary.TryGetValue(b.MaterialCode, out decimal currentStock);
                        //allowReport = allowReport && b.TotalConsumption <= currentStock;

                        return new
                        {
                            b.MaterialCode,
                            b.MaterialDesc,
                            RequiredQuantity = b.TotalRequiredQty?.ToString("F3") ?? "0.000",
                            b.Unit,
                            TheoreticalConsumption = b.TotalConsumption?.ToString("F3") ?? "0.000",
                            // 修正了你的逻辑：消耗量 > 库存量 才是库存不足
                            Status = b.TotalConsumption > currentStock ? "库存不足" : "库存充足"
                        };
                    }).ToList();
                    //if (!allowReport)
                    //    SubmitButton.Enabled = false;
                    //else
                    //    SubmitButton.Enabled = true;

                }
            }

        }

        private async Task LoadCuttingScrapReasonAsync()
        {
            var scrapReasonGroup = await _parameterGroupService.GetGroupWithItemsAsync(Group_CableCutScrapReason);
            ScrapReasonSelect.Items.Clear();
            if (scrapReasonGroup != null)
            {
                foreach (var item in scrapReasonGroup.Items)
                {
                    ScrapReasonSelect.Items.Add(new MenuItem
                    {
                        Name = item.Value,
                        Text = $"{item.Value}-{item.Name}"
                    });
                }
            }

            var scrapLenGroup = await _parameterGroupService.GetGroupWithItemsAsync(Group_CableSampleRunScrapLen);
            var defaultLengthItem = scrapLenGroup?.Items.FirstOrDefault(x => x.Key == Key_DefaultLength);
            var items = ScrapReasonSelect.Items.ToArray<MenuItem>();
            ScrapReasonSelect.SelectedIndex = ScrapReasonSelect.Items.IndexOf(items.First(x => x.Name == defaultLengthItem.Name));
            ScrapLenInputNumber.Value = int.Parse(defaultLengthItem?.Value ?? "250");
        }

        private void LoadPrinters()
        {
            PrinterSelect.Items.Clear();

            // 获取本地安装的所有打印机
            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                var typeDesc = printerName.StartsWith(@"\\") ? PrinterType.NetworkPrinter : PrinterType.LocalPrinter;
                string displayName = printerName.StartsWith(@"\\") ? printerName.Substring(printerName.LastIndexOf('\\') + 1) : printerName;
                PrinterSelect.Items.Add($"{displayName}-{typeDesc.GetDescription()}");
            }
        }

        private async void WorkStationSelect_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            await RunAsync(async () =>
            {
                if (e.Value is AntdUI.MenuItem item && int.TryParse(item.Name, out int stationId) && _stationPrinters.TryGetValue(stationId, out string printerName))
                {
                    //PrinterSelect.SelectedValue = PrinterSelect.Items.ToList<string>().FirstOrDefault(x => x.Contains(printerName));
                    PrinterSelect.SelectedValue = PrinterSelect.Items.Cast<string>().FirstOrDefault(x => x != null && x.StartsWith($"{printerName}-"));

                    WorkStationSelect.SelectionStart = 0;
                }
            });
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SearchButton, async () =>
            {
                int count = await QueryOrdersAsync();
                await LoadStationMaterialLoadingAsync();
                if (count > 0)
                {
                    if (SearchButton.Text == "查询" && int.TryParse(((MenuItem)WorkStationSelect.SelectedValue).Name, out int stationId))
                    {
                        _currentStation = await _workStationService.GetByIdAsync(stationId);
                        //_currentPrinter = PrinterSelect.SelectedValue.toString().split("-")[0];
                        ResetSearchButton(false);

                        AntdUI.Message.success(this.ParentForm, $"查询成功：共有{count}条结果");
                    }
                    else
                    {
                        _currentStation = null;
                        //_currentPrinter = null;
                        ResetSearchButton(true);
                    }


                }
                else
                    throw new Exception("未查询到订单记录！");

            });
        }

        private async Task<int> QueryOrdersAsync()
        {
            var startdate = StartDatePicker.Value;
            if (startdate == null)
                throw new Exception("未选择日期，请先选择仓库日期！");
            var workcenter = (AntdUI.MenuItem)WorkCenterSelect.SelectedValue;
            if (workcenter == null)
                throw new Exception("未选择工作中心，请先选择工作中心！");
            var workstation = (AntdUI.MenuItem)WorkStationSelect.SelectedValue;
            if (workstation == null)
                throw new Exception("未选择工位，请先选择工位！");
            var printername = PrinterSelect.SelectedValue as string;
            if (string.IsNullOrEmpty(printername))
                throw new Exception("未选择打印机，请先选择打印机！");
            var result = await _pendingMaterialTaskCuttingService.GetListByExecutionAsync((DateTime)startdate, workcenter.Text.Split('-')[0]);
            if (result.Any())
            {
                TaskTable.DataSource = result.OrderByDescending(x => x.Status).ToList();
                return result.Count;
            }
            return 0;
        }

        private async Task QueryExecuteLogsAsync()
        {
            ExecuteTable.DataSource = null;
            if (_currentStartTask != null)
            {
                var result = await _workOrderTaskExecuteLogService.GetListByTaskIdAsync(_currentStartTask.Id, TaskLevel.Material);
                if (result != null && result.Any())
                {
                    ExecuteTable.DataSource = result.OrderByDescending(x => x.Id).Select(x => new ExecuteLogViewModel(x)).ToList();
                }

            }
        }

        private void WorkCenterSelect_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e) => WorkCenterSelect.SelectionStart = 0;

        private void PrinterSelect_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e) => PrinterSelect.SelectionStart = 0;

        private async void OrderScanInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                await RunAsync(null, async () =>
                {
                    var data = TaskTable.DataSource as List<PendingMaterialTaskCuttingDto>;
                    if (data == null || !data.Any())
                        throw new Exception("未查询到待执行的断线任务，请重新查询！");
                    string filter = OrderScanInput.Text.Trim();
                    OrderScanInput.SelectAll();
                    if (!string.IsNullOrEmpty(filter))
                    {
                        TaskTable.DataSource = data.Where(t => t.WorkOrderNo.Contains(filter) || t.MaterialCode.contains(filter)).ToList();
                        MoveOrderToTop(filter);
                    }
                    else
                        await QueryOrdersAsync();

                });

            }

        }

        private void TaskTable_FilterDataChanged(object sender, AntdUI.TableFilterDataChangedEventArgs e)
        {
            var data = TaskTable.DataSource as List<PendingMaterialTaskCuttingDto>;
            if (data != null && data.Count() > 0)
                AntdUI.Message.success(this.ParentForm, $"查询成功：共有{data.Count()}条结果");
        }

        private async void TaskTable_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is PendingMaterialTaskCuttingDto data)
            {
                await RunAsync(null, async () =>
                {
                    SubmitButton.Enabled = false;
                    WorkStartButton.Enabled = false;
                    SuspendButton.Enabled = false;
                    ToggleReportButtons(false);
                    if (_currentPendingTask != null) _currentPendingTask = null;
                    if (data.MaterialTaskId <= 0)
                        throw new Exception($"查询出错：请重新同步Sap订单{data.WorkOrderNo}！");
                    var task = await _taskGenerationService.UpdateCuttingMaterialTaskAsync(data.MaterialTaskId, AppSession.CurrentUser.EmployeeId);
                    if (task != null)
                    {
                        if (task.TargetValue == null)
                            throw new Exception("未查询到最新的断线参数信息，请重新同步断线参数！");
                        data.TargetQuantity = task.TargetQuantity;
                        data.TargetValue = task.TargetValue;
                        data.TargetUnit = task.TargetUnit;
                        if (task.ExtAttributes == null)
                            throw new Exception("未查询到断线参数信息，请重新同步订单！");
                        data.CuttingParam = JsonConvert.DeserializeObject<CableCutParamDto>(task.ExtAttributes);
                        TaskTable.Refresh();

                        OrderNoLabel.Text = data.WorkOrderNo;
                        MatcLabel.Text = data.MaterialCode;
                        MatdLabel.Text = data.MaterialDesc;
                        ProgressLabel.Text = $"{(task.CompletedQuantity ?? 0).ToString("F0")} / {(task.TargetQuantity ?? 0).ToString("F0")}";
                        CutLendLabel.Text = $"{Math.Round((decimal)data.CuttingParam.BomLength / (decimal)data.CuttingParam.CablePcs - (decimal)data.CuttingParam.DownTol, 1)} mm";
                        CutLenuLabel.Text = $"{Math.Round((decimal)data.CuttingParam.BomLength / (decimal)data.CuttingParam.CablePcs + (decimal)data.CuttingParam.UpTol * 0.8m, 1)} mm";
                        CutLenLabel.Text = $"{(data.CuttingParam.CuttingLength ?? 0).ToString("F1")} mm";


                        if (task.Status != WorkTaskStatus.InProgress)
                            WorkStartButton.Enabled = true;
                        if (task.Status != WorkTaskStatus.Suspended)
                            SuspendButton.Enabled = true;

                        _currentPendingTask = data;
                        if (task.Status == WorkTaskStatus.InProgress)
                        {
                            _currentStartTask = await _workOrderMaterialTaskService.GetByIdAsync(_currentPendingTask.MaterialTaskId);
                            ToggleReportButtons(true);
                        }
                        AdjustButton.Enabled = true;
                        SetTaskStatusTag(task.Status);

                        MoveOrderToTop(data.WorkOrderNo);

                        if (_currentPendingTask != null)
                        {
                            await LoadBomInfoByTaskIdAsync(new List<int>() { _currentPendingTask.MaterialTaskId });
                            await QueryExecuteLogsAsync();
                        }


                    }
                });

            }
        }


        /// <summary>
        /// 功能：扫码置顶
        /// Table 的置顶非常简单，只需要操作数据源 List，然后重新赋值 DataSource 即可
        /// </summary>
        private void MoveOrderToTop(string keyword)
        {
            var orderDatas = TaskTable.DataSource as List<PendingMaterialTaskCuttingDto>;
            var target = orderDatas.Where(x => x.WorkOrderNo == keyword || x.MaterialCode == keyword).OrderBy(x => x.WorkOrderNo).ToArray();
            if (target != null && target.Any())
            {

                for (int i = 0; i < target.Count(); i++)
                {
                    // 1. 修改数据源顺序
                    orderDatas.Remove(target[i]);
                    orderDatas.Insert(i, target[i]);
                }


                // 2. 刷新表格 (AntdUI Table 重新赋值 DataSource 会触发重绘，非常快)
                TaskTable.DataSource = null; // 有时候需要重置一下触发刷新，视版本而定
                TaskTable.DataSource = orderDatas;

                // 3. 选中第一行并滚动
                TaskTable.SelectedIndex = 1;
                TaskTable.ScrollLine(1, true);
            }
        }

        private async void WorkStartButton_Click(object sender, EventArgs e)
        {
            await RunAsync(WorkStartButton, async () =>
            {
                ToggleReportButtons(false);
                await _taskExecutionService.ValidateMaterialReadinessAsync(new List<int>() { _currentPendingTask.MaterialTaskId }, _currentStation.Id);
                await _taskExecutionService.ClaimAndStartTasksAsync(new List<int>() { _currentPendingTask.MaterialTaskId }, _currentStation.Id, AppSession.CurrentUser.EmployeeId);
                _workStartTime = DateTime.Now;
                _currentStartTask = await _workOrderMaterialTaskService.GetByIdAsync(_currentPendingTask.MaterialTaskId);
                ToggleReportButtons(true);
                SetTaskStatusTag(WorkTaskStatus.InProgress);
                WorkStartButton.Enabled = false;
                SuspendButton.Enabled = true;
                SubmitButton.Enabled = true;
                await QueryOrdersAsync();
                MoveOrderToTop(_currentPendingTask.WorkOrderNo);
            }, confirmMsg: $"即将执行订单{_currentPendingTask.WorkOrderNo}物料{_currentPendingTask.MaterialCode},是否继续？");
        }

        private async void BarCodeScanInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                await RunAsync(null, async () =>
                {
                    var station = WorkStationSelect.SelectedValue as AntdUI.MenuItem;
                    if (station == null || string.IsNullOrEmpty(station.Name))
                        throw new Exception("未选择有效工位，请重新选择！");
                    var barcode = BarCodeScanInput.Text.Trim();
                    if (string.IsNullOrEmpty(barcode))
                        throw new Exception("未扫描有效标签ID，请重新扫描！");
                    var loading = await _stationMaterialLoadingService.GetListByStationIdAsync(int.Parse(station.Name));
                    if (loading.Any())
                        throw new Exception("当前工位已扫描上料，请先将物料下料后再次扫描！");
                    if (_currentStartTask != null || _currentPendingTask.MaterialTaskId > 0)
                    {
                        await _taskExecutionService.VerifyScannedMaterialMatchAsync(AppSession.CurrentFactoryId, barcode, new List<int>() { _currentStartTask != null ? _currentStartTask.Id : _currentPendingTask.MaterialTaskId });

                    }
                    await _taskExecutionService.LoadMaterialAsync(AppSession.CurrentFactoryId, barcode, int.Parse(station.Name), AppSession.CurrentUser.EmployeeId);

                    await LoadStationMaterialLoadingAsync();

                    BarCodeScanInput.Text = string.Empty;


                    if (_currentPendingTask != null)
                        LoadBomInfoByTaskIdAsync(new List<int>() { _currentPendingTask.MaterialTaskId });

                }, successMsg: "标签ID扫描成功！");
            }
        }

        private async void LoadingTable_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is StationMaterialLoadingViewModel data)
            {
                if (e.Btn.Id == "unload")
                {
                    await RunAsync(null, async () =>
                    {
                        var loadingId = data.Id;
                        if (data.Id <= 0)
                            throw new Exception($"当前标签ID{data.BarCode}无效，请重新扫描标签！");
                        await _taskExecutionService.UnloadMaterialAsync(data.Id, AppSession.CurrentUser.EmployeeId);
                        await LoadStationMaterialLoadingAsync();
                        BarCodeScanInput.Text = data.BarCode;
                        if (_currentPendingTask != null)
                            LoadBomInfoByTaskIdAsync(new List<int>() { _currentPendingTask.MaterialTaskId });

                    }, confirmMsg: $"即将下料标签{data.BarCode}释放线边库存，是否继续？", successMsg: "下料已完成！");

                }

            }

        }


        private void ToggleReportButtons(bool enable)
        {
            ProcessCardPrintButton.Enabled = enable;
            InspectLabelButton.Enabled = enable;
            SubmitButton.Enabled = enable;

            ReportInputNumber.Value = 0;
            ReportInputNumber.Text = string.Empty;
            ScrapReasonSelect.ReadOnly = true;
            ScrapLenInputNumber.ReadOnly = true;
        }

        private void ResetSearchButton(bool enable)
        {
            StartDatePicker.Enabled = enable;
            WorkCenterSelect.Enabled = enable;
            WorkStationSelect.Enabled = enable;
            PrinterSelect.Enabled = enable;
            if (enable)
            {
                TaskTable.DataSource = null;
                LoadingTable.DataSource = null;
                ToggleReportButtons(false);

                SearchButton.Type = TTypeMini.Primary;
                SearchButton.Text = "查询";
            }
            else
            {
                SearchButton.Type = TTypeMini.Error;
                SearchButton.Text = "重置";
            }

        }


        private async Task ReflashLabelInfoAsync(int taskId)
        {
            var task = await _workOrderMaterialTaskService.GetByIdAsync(taskId);
            if (task != null)
            {
                if (task.TargetValue == null)
                    throw new Exception("未查询到最新的断线参数信息，请重新同步断线参数！");
                ProgressLabel.Text = $"{(task.CompletedQuantity ?? 0).ToString("F0")} / {(task.TargetQuantity ?? 0).ToString("F0")}";

                if (int.TryParse(task.Status, out int status))
                {
                    if (status < int.Parse(WorkTaskStatus.InProgress))
                        WorkStartButton.Enabled = true;
                    if (status < int.Parse(WorkTaskStatus.Suspended))
                    {
                        SuspendButton.Enabled = true;
                        ToggleReportButtons(true);
                    }
                }
                SetTaskStatusTag(task.Status);

            }
        }

        private async void SuspendButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SuspendButton, async () =>
            {
                if (_currentStartTask == null)
                {
                    if (_currentPendingTask == null)
                    {
                        throw new Exception("未查询到可暂停的工单，请重新选择工单！");
                    }
                    else
                        await _taskExecutionService.SuspendTaskAsync(_currentPendingTask.MaterialTaskId, AppSession.CurrentUser.EmployeeId);
                }
                else
                {
                    await _taskExecutionService.SuspendTaskAsync(_currentStartTask.Id, AppSession.CurrentUser.EmployeeId);
                    _currentStartTask = null;
                }

                SetTaskStatusTag(WorkTaskStatus.Suspended);
                ToggleReportButtons(false);
                SuspendButton.Enabled = false;
                QueryOrdersAsync();
            }, confirmMsg: "即将挂起工单，是否继续？", successMsg: "挂起完成！");
        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SubmitButton, async () =>
            {
                if (_currentStartTask == null)
                    throw new Exception("当前订单未开工，请先点击开工按钮！");
                if (ReportInputNumber.Value <= 0)
                    throw new Exception("报工数量必须大于0，请修改报工数量！");
                if (ScrapLenInputNumber.Value < 0)
                    throw new Exception("报废数量不能小于0，请修改报废数量！");
                if (ScrapLenInputNumber.Value > 0 && ScrapReasonSelect.SelectedValue == null)
                    throw new Exception("报废原因不能为空，请重新选择！");

                var task = await _workOrderMaterialTaskService.GetByIdAsync(_currentStartTask.Id);
                if (task == null)
                    throw new Exception("未查询到当前任务信息！");

                if (((task.CompletedQuantity) ?? 0) + ReportInputNumber.Value > task.TargetQuantity)
                    throw new Exception("当前报工数量已超过任务可报工数量，无法继续报工！");
                bool isFinished = false;
                if (((task.CompletedQuantity) ?? 0) + ReportInputNumber.Value == task.TargetQuantity)
                    isFinished = true;
                decimal scrapqty = 0;
                var scrapreason = ((MenuItem)ScrapReasonSelect.SelectedValue).Name;

                //
                bool printflag = false;
                if (await _taskExecutionService.VerifyCuttingStepTaskFirstConfirmAsync(_currentStartTask.Id))
                    printflag = true;


                var scrapLenGroup = await _parameterGroupService.GetGroupWithItemsAsync(Group_CableSampleRunScrapLen);
                var defaultLengthItem = scrapLenGroup?.Items.FirstOrDefault(x => x.Key == Key_DefaultLength);
                if (((task.CompletedQuantity) ?? 0) == 0 && scrapreason == defaultLengthItem.Name)
                {
                    scrapqty = ScrapLenInputNumber.Value;
                }
                var loadings = await _stationMaterialLoadingService.GetListByStationIdAsync(_currentStation.Id, _currentStartTask.MaterialCode);
                if (loadings == null || !loadings.Any() || loadings.Where(x => x.LastQuantity > 0).Count() == 0)
                    throw new Exception("未查询到可用的上料记录，请先上料！");
                _currentStationMaterialLoading = (await _stationMaterialLoadingService.GetListByStationIdAsync(_currentStation.Id, _currentStartTask.MaterialCode)).Where(x => x.LastQuantity > 0).OrderBy(x => x.LoadedTime).First();

                await _taskExecutionService.ReportMaterialTaskProductionAsync(_currentStartTask.Id, ReportInputNumber.Value, scrapqty, _currentStation.Id, _currentStation.WorkStationCode, AppSession.CurrentUser.EmployeeId, RemarkInput.Text, scrapreason, _workStartTime);
                if(printflag)
                    await ProcessLabelPrint();
                CuttingConfirmLabelPrint(_currentStationMaterialLoading.BatchCode, _currentStationMaterialLoading.BarCode, ReportInputNumber.Value);
                ReportInputNumber.Value = 0;
                ReportInputNumber.Text = string.Empty;
                await LoadStationMaterialLoadingAsync();
                await QueryOrdersAsync();

                await ReflashLabelInfoAsync(_currentStartTask.Id);
                await QueryExecuteLogsAsync();
                await LoadBomInfoByTaskIdAsync(new List<int>() { _currentPendingTask.MaterialTaskId });

                if (!isFinished)
                    MoveOrderToTop(_currentPendingTask.WorkOrderNo);
                else
                {
                    SubmitButton.Enabled = false;
                    SuspendButton.Enabled = false;
                    _currentStartTask = null;
                }
            }, confirmMsg: "即将提交当前报工，是否继续？", successMsg: "提交完成！");
        }

        private void ScrapReasonSelect_DoubleClick(object sender, EventArgs e)
        {
            ScrapReasonSelect.ReadOnly = false;
        }

        private void ScrapLenInputNumber_DoubleClick(object sender, EventArgs e)
        {
            ScrapLenInputNumber.ReadOnly = false;
        }

        private async void InspectLabelButton_Click(object sender, EventArgs e)
        {
            await RunAsync(InspectLabelButton, async () =>
            {
                var selectValue = PrinterSelect.SelectedValue as string;
                if (!string.IsNullOrEmpty(selectValue) && selectValue.indexOf("-") > 0)
                {
                    var printerName = selectValue[..selectValue.LastIndexOf('-')];
                    var printerTypeStr = selectValue[(selectValue.LastIndexOf('-') + 1)..];
                    var printerType = EnumExtensions.GetEnumByDescription<PrinterType>(printerTypeStr);

                    var cuttingParam = JsonConvert.DeserializeObject<CableCutParamDto>(_currentStartTask.ExtAttributes);

                    var result = LabelPrintHelper.CuttingInspectLabelPrinter(_currentStartTask.MaterialCode, _currentStartTask.MaterialDesc, _currentPendingTask.WorkOrderNo + "QC", (decimal)cuttingParam.CuttingLength, Math.Round((decimal)cuttingParam.BomLength / (decimal)cuttingParam.CablePcs - (decimal)cuttingParam.DownTol, 1), Math.Round((decimal)cuttingParam.BomLength / (decimal)cuttingParam.CablePcs + (decimal)cuttingParam.UpTol * 0.8m, 1), printerType, printerName);
                    if (result)
                        AntdUI.Message.success(this.ParentForm, "打印成功！");
                    else
                        AntdUI.Message.error(this.ParentForm, "打印失败，请重试！");

                }

            }, confirmMsg: "即将打印断线首检标签，是否继续？");
        }

        private async Task<bool> ProcessLabelPrint()
        {

            var selectValue = PrinterSelect.SelectedValue as string;
            if (!string.IsNullOrEmpty(selectValue) && selectValue.indexOf("-") > 0)
            {
                var printerName = selectValue[..selectValue.LastIndexOf('-')];
                var printerTypeStr = selectValue[(selectValue.LastIndexOf('-') + 1)..];
                var printerType = EnumExtensions.GetEnumByDescription<PrinterType>(printerTypeStr);
                var workorder = await _workOrderService.GetByIdAsync(_currentPendingTask.WorkOrderId);
                var process = await _workOrderProcessService.GetByIdAsync(_currentPendingTask.WorkOrderProcessId);
                var flag = await _taskExecutionService.DetermineMaterialFlagToProcessLabelAsync(workorder.Id);
                var location = await GetNextLocationAsync(process.NextWorkCenter);
                return LabelPrintHelper.ProcessLabelPrinter(workorder.MaterialCode, workorder.MaterialDesc, (decimal)workorder.Quantity, workorder.OrderNumber, process.Operation, workorder.LeadingOrderMaterial, process.WorkCenter, process.NextWorkCenter, (int)workorder.LabelCount, (DateTime)workorder.DispatchDate, flag, location, printerType, printerName);

            }
            return false;

        }

        private void CuttingConfirmLabelPrint(string batchCode, string barCode, decimal confirmQuantity)
        {
            var selectValue = PrinterSelect.SelectedValue as string;
            if (!string.IsNullOrEmpty(selectValue) && selectValue.indexOf("-") > 0)
            {
                var printerName = selectValue[..selectValue.LastIndexOf('-')];
                var printerTypeStr = selectValue[(selectValue.LastIndexOf('-') + 1)..];
                var printerType = EnumExtensions.GetEnumByDescription<PrinterType>(printerTypeStr);
                var cuttingParam = JsonConvert.DeserializeObject<CableCutParamDto>(_currentStartTask.ExtAttributes);

                var result = LabelPrintHelper.CuttingLabelPrinter(_currentStartTask.MaterialCode, _currentStartTask.MaterialDesc, (decimal)_currentStartTask.TargetQuantity, (decimal)cuttingParam.CuttingLength, _currentPendingTask.ProfitCenter, _currentPendingTask.WorkOrderNo, batchCode, barCode, confirmQuantity, printerType, printerName);
                if (!result)
                    AntdUI.Message.error(this.ParentForm, "断线标签打印失败！");
            }
        }

        private async Task<string> GetNextLocationAsync(string workCenterCode)
        {
            var wc = await _workCenterService.GetByCodeAsync(workCenterCode);
            if (wc?.LineStockId != null)
            {
                var loc = await _warehouseLocationService.GetByIdAsync((int)wc.LineStockId);
                return loc?.Name ?? string.Empty;
            }
            return string.Empty;
        }

        private async void ProcessCardPrintButton_Click(object sender, EventArgs e)
        {
            await RunAsync(ProcessCardPrintButton, async () =>
            {
                var result = await ProcessLabelPrint();
                if (result)
                    AntdUI.Message.success(this.ParentForm, "打印成功！");
                else
                    AntdUI.Message.error(this.ParentForm, "打印失败，请重试！");

            }, confirmMsg: "即将打印流转卡标签，是否继续？");
        }

        private async void AdjustButton_Click(object sender, EventArgs e)
        {
            await RunAsync(AdjustButton, async () =>
            {
                var loadingLog = new StationMaterialLoadingDto();
                if (_currentStationMaterialLoading == null)
                {
                    var materialcode = _currentStartTask == null ? _currentPendingTask.MaterialCode : _currentStartTask.MaterialCode;
                    loadingLog = (await _stationMaterialLoadingService.GetListByStationIdAsync(_currentStation.Id, materialcode, status: null)).OrderByDescending(x => x.LoadedTime).FirstOrDefault();
                    if (loadingLog == null)
                        throw new Exception($"未查询到物料{materialcode}的上料记录，请重试");
                }
                else
                {
                    loadingLog = await _stationMaterialLoadingService.GetByIdAsync(_currentStationMaterialLoading.Id);
                }

                if (loadingLog.Status == MaterialLoadingStatus.Active)
                    throw new Exception($"当前标签{loadingLog.BarCode}正在使用中，请先下料后再进行调整！");
                // 【修复点】使用 Factory 打开，并使用 InitData 传参
                _formFactory.Show<InventoryAdjustForm>(form =>
                {
                    // 传递当前任务ID给库存调整界面
                    form.InitData(stockId: loadingLog.LineSideInventoryId, workorderId: _currentPendingTask.WorkOrderId);

                    // 监听成功事件
                    form.OnSuccess += () =>
                    {
                        AntdUI.Message.success(this, "库存调整提交成功！");
                    };
                }, isModal: true);
            });
        }

        private async void ExecuteTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is ExecuteLogViewModel data)
            {
                if (e.Btn.Id == "print")
                {
                    await RunAsync(null, async () =>
                    {
                        if (_currentStartTask == null)
                            throw new Exception("未开工订单无法打印过账标签，请先开工本订单！");

                        var exeLog = await _workOrderTaskExecuteLogService.GetByIdAsync(data.Id);
                        if (exeLog == null)
                            throw new Exception("未查询到当前报工记录，请重新查询！");
                        var consumps = await _workOrderTaskExecuteConsumpService.GetListByExecuteIdAsync(exeLog.Id);
                        if (consumps == null || consumps.Count() == 0)
                            throw new Exception("未查询到当前消耗记录，请重新查询！");
                        var currentConsump = consumps.First();
                        CuttingConfirmLabelPrint(currentConsump.BatchCode, currentConsump.BarCode, (decimal)exeLog.CompletedQuantity);
                    }, confirmMsg: "即将打印当前过账标签，是否继续？", successMsg: "打印完成！");

                }
            }

        }

        private async void AllLoadingTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is StationMaterialLoadingViewModel data)
            {
                if (e.Btn.Id == "unload")
                {
                    await RunAsync(null, async () =>
                    {
                        var loadingId = data.Id;
                        if (data.Id <= 0)
                            throw new Exception($"当前标签ID{data.BarCode}无效，请重新扫描标签！");
                        await _taskExecutionService.UnloadMaterialAsync(data.Id, AppSession.CurrentUser.EmployeeId);
                        await LoadStationMaterialLoadingAsync();

                        if (_currentPendingTask != null)
                            LoadBomInfoByTaskIdAsync(new List<int>() { _currentPendingTask.MaterialTaskId });

                    }, confirmMsg: $"即将下料标签{data.BarCode}释放线边库存，是否继续？", successMsg: "下料已完成！");

                }

            }
        }
    }

    public class StationMaterialLoadingViewModel : AntdUI.NotifyProperty
    {
        public StationMaterialLoadingViewModel(StationMaterialLoadingDto dto)
        {
            _id = dto.Id;
            _workStationCode = dto.WorkStationCode;
            _materialCode = dto.MaterialCode;
            _materialDesc = dto.MaterialDesc;
            _batchCode = dto.BatchCode;
            _barCode = dto.BarCode;
            _lastQuantity = dto.LastQuantity;
            _btns = new AntdUI.CellLink[]
            {
                    new AntdUI.CellButton("unload", "下料", AntdUI.TTypeMini.Primary)
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

        string _workStationCode;

        public string WorkStationCode
        {
            get => _workStationCode;
            set
            {
                if (_workStationCode == value)
                    return;
                _workStationCode = value;
                OnPropertyChanged();
            }
        }

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

        string? _batchCode;
        public string? BatchCode
        {
            get => _batchCode;
            set
            {
                if (_batchCode == value)
                    return;
                _batchCode = value;
                OnPropertyChanged();
            }
        }

        string _barCode;
        public string BarCode
        {
            get => _barCode;
            set
            {
                if (_barCode == value)
                    return;
                _barCode = value;
                OnPropertyChanged();
            }
        }

        decimal _lastQuantity;
        public decimal LastQuantity
        {
            get => _lastQuantity;
            set
            {
                if (_lastQuantity == value)
                    return;
                _lastQuantity = value;
                OnPropertyChanged();
            }
        }

        AntdUI.CellLink[] _btns;
        public AntdUI.CellLink[] btns
        {
            get => _btns;
            set
            {
                _btns = value;
                OnPropertyChanged();
            }
        }
    }
    public class ExecuteLogViewModel : AntdUI.NotifyProperty
    {
        public ExecuteLogViewModel(WorkOrderTaskExecuteLogDto dto)
        {
            _id = dto.Id;
            _batchCode = dto.BatchCode;
            _completedQuantity = dto.CompletedQuantity;
            _employerCode = dto.EmployerCode;
            _createdOn = dto.CreatedOn;
            _btns = new AntdUI.CellLink[]
            {
                    new AntdUI.CellButton("print", "打印", AntdUI.TTypeMini.Primary)
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

        string? _batchCode;
        public string? BatchCode
        {
            get => _batchCode;
            set
            {
                if (_batchCode == value)
                    return;
                _batchCode = value;
                OnPropertyChanged();
            }
        }

        decimal? _completedQuantity;
        public decimal? CompletedQuantity
        {
            get => _completedQuantity;
            set
            {
                if (_completedQuantity == value)
                    return;
                _completedQuantity = value;
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

        DateTime? _createdOn;
        public DateTime? CreatedOn
        {
            get => _createdOn;
            set
            {
                if (_createdOn == value)
                    return;
                _createdOn = value;
                OnPropertyChanged();
            }
        }

        AntdUI.CellLink[] _btns;
        public AntdUI.CellLink[] btns
        {
            get => _btns;
            set
            {
                _btns = value;
                OnPropertyChanged();
            }
        }


    }
}
