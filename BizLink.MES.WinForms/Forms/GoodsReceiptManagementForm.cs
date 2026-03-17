using AntdUI;
using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Response;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Helper;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using DocumentFormat.OpenXml.Drawing.Charts;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class GoodsReceiptManagementForm : MesBaseForm
    {

        private readonly AssemblyModuleFacade _facade;
        private readonly IFormFactory _formFactory;

        private readonly IFinishedGoodsReceiptService _finishedGoodsReceiptService;
        private readonly ITaskExecutionService _taskExecutionService;

        private const string SERIAL_CODE_PREFIX = "FinishedGoodsReceiptToSAP";
        private const string DEFAULT_PRINT_LAYOUT = "LS_KSCE_534_E";

        private const string LAYOUT_PARAM_GROUP = "FinishGoodsReceiptLabelLayout";
        private WorkOrderProcessDto _currentProcess = new WorkOrderProcessDto();
        private string? _lastPrintedXml = null;

        // 弹性策略 (Polly)
        private readonly AsyncRetryPolicy _retryPolicy;

        public GoodsReceiptManagementForm(AssemblyModuleFacade facade, IFinishedGoodsReceiptService finishedGoodsReceiptService, IFormFactory formFactory, ITaskExecutionService taskExecutionService)
        {
            InitializeComponent();
            InitializeTable();
            _facade = facade;
            _finishedGoodsReceiptService = finishedGoodsReceiptService;
            _formFactory = formFactory;
            _taskExecutionService = taskExecutionService;

            // 初始化重试策略：指数退避 (2s, 4s, 8s)
            _retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DispatchDatePickerRange.PlaceholderStart = "排产日期开始";
            DispatchDatePickerRange.PlaceholderEnd = "排产日期结束";
            WorkCenterSelect.PlaceholderText = "请选择工作中心";
            PrinterSelect.PlaceholderText = "请选择打印机";
            WorkOrderInput.PlaceholderText = "请输入订单号";

            ToggleInputs(true);
            LoadWorkCenterSelectAsync();
            LoadPrinters();

        }
        private void InitializeTable()
        {
            OrderTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("WorkOrderNo", "订单号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
              new AntdUI.Column("MaterialCode", "订单物料", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),


                new AntdUI.Column("Quantity", "订单数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CompletedQuantity", "完成数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ReceiptedQuantity", "入库数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("DispatchDate", "计划完成日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Operation", "工序序号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenter", "工作中心", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("StorageLocation", "入库地点", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center) {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "未完成" => new AntdUI.CellTag("未完成", AntdUI.TTypeMini.Error),
                            "待入库" => new AntdUI.CellTag("待入库", AntdUI.TTypeMini.Primary),
                            "部分入库" => new AntdUI.CellTag("部分入库", AntdUI.TTypeMini.Success),
                            _ => new AntdUI.CellTag("未完成", AntdUI.TTypeMini.Error)
                        };
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

            };
            ReceiptTable.Columns = new ColumnCollection
            {
                new AntdUI.Column("WorkOrderNo", "订单号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "数量", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("SapStatus", "Sap同步状态", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("SapBatchNo", "Sap批次号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("SapMessageType", "Sap返回结果", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("SapMessage", "Sap返回信息", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

            };

        }

        private void ToggleInputs(bool enable)
        {
            DispatchDatePickerRange.Enabled = enable;
            WorkCenterSelect.Enabled = enable;
            PrinterSelect.Enabled = enable;

            if (!enable)
            {
                SearchButton.Text = "解除确认";
                SearchButton.Type = AntdUI.TTypeMini.Error;
                WorkOrderInput.Enabled = true;
            }
            else
            {
                SubmitButton.Enabled = false;
                ReprintButton.Enabled = false;
                ResetButton.Enabled = false;
                SearchButton.Text = "工位确认";
                SearchButton.Type = AntdUI.TTypeMini.Primary;
                WorkOrderInput.Enabled = false;
            }
        }

        #region 数据加载

        private async Task LoadWorkCenterSelectAsync()
        {
            var workcenters = await _facade.WorkCenter.GetAllAsync(AppSession.CurrentFactoryId);
            WorkCenterSelect.Items.Clear();
            if (workcenters != null && workcenters.Where(x => x.IsGroup == false).Count() > 0)
                WorkCenterSelect.Items.AddRange(workcenters.Where(x => x.IsGroup == false).OrderBy(x => x.WorkCenterCode).Select(x => new AntdUI.MenuItem()
                {
                    Name = x.Id.ToString(),
                    Text = $"{x.WorkCenterCode}-{x.WorkCenterName}"
                }).ToArray());
        }


        private async Task RefreshReceiptLogsAsync(int processId)
        {
            ReceiptTable.DataSource = null;
            var process = await _facade.WorkOrderProcessService.GetByIdAsync(processId);
            var logs = await _finishedGoodsReceiptService.GetListByWorkOrderProcessIdAsync(processId);

            if (logs?.Any() == true)
            {
                decimal totalReceipted = logs.Sum(x => (decimal)x.Quantity);
                decimal remaining = (process.Quantity ?? 0) - totalReceipted;
                ReceiptedNumLabel.Text = $"{Math.Max(0, remaining):0.#} PCS";
                ReceiptTable.DataSource = logs;
            }
            else
            {
                ReceiptedNumLabel.Text = $"{process.Quantity:0.#} PCS";
            }

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

        #endregion


        #region 事件处理器 (轻量化 UI 入口)
        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SearchButton, async () =>
            {
                if (SearchButton.Text.Contains("工位确认"))
                {

                    ValidateSearchInputs();
                    var data = await QueryOrdersAsync();
                    OrderTable.DataSource = data;
                    ToggleInputs(false);
                }
                else
                {
                    OrderTable.DataSource = null;
                    ToggleInputs(true);
                }
            });
        }

        private async void WorkOrderInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                await RunAsync(async () =>
                {
                    var selectedProcess = await FindProcessByOrderNoAsync(WorkOrderInput.Text.Trim());
                    BindProcessToUI(selectedProcess);
                    await RefreshReceiptLogsAsync(selectedProcess.Id);
                });
            }

        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SubmitButton, async () =>
            {

                // 1. 校验
                ValidateSubmission();

                // 2. 创建本地记录
                var receipt = await CreateLocalReceiptAsync();
                UpdateStepStatus(TStepState.Process, 1);

                // 3. 推送 SAP
                receipt = await PushToSapAsync(receipt);
                UpdateStepStatus(TStepState.Process, 2);
                SapBatchInput.Text = receipt.SapBatchNo;

                // 4. 打印标签
                await PrintLabelAsync(receipt);
                UpdateStepStatus(TStepState.Finish, 3); // 完成所有步骤

                // 5. 刷新界面
                await RefreshReceiptLogsAsync(receipt.WorkOrderProcessId);
                AntdUI.Message.success(this, $"订单已入库{ReceiptInputNumber.Value}PCS,并成功打印指令至打印机！");
            },confirmMsg:$"订单{_currentProcess.WorkOrderNo}即将入库{ReceiptInputNumber.Value}PCS并打印标签,是否继续？");
        }
        private async void ReprintButton_Click(object sender, EventArgs e)
        {
            await RunAsync(ReprintButton, async () =>
            {
                if (string.IsNullOrEmpty(_lastPrintedXml))
                    throw new Exception("暂无最近的打印记录可供补打");

                await ExecutePrintRequestAsync(_lastPrintedXml);

            }, successMsg: "补打标签指令已发送");
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ReceiptInputNumber.Value = 0;
            SapBatchInput.Text = string.Empty;
        }

        private void WorkCenterSelect_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e) => WorkCenterSelect.SelectionStart = 0;
        private void PrinterSelect_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e) => PrinterSelect.SelectionStart = 0;
        private void ReceiptInputNumber_ValueChanged(object sender, DecimalEventArgs e) => ReprintButton.Enabled = false;

        #endregion

        #region 核心业务逻辑封装 (Business Logic)
        private void ValidateSearchInputs()
        {
            if (DispatchDatePickerRange.Value == null || DispatchDatePickerRange.Value.Length < 2)
                throw new Exception("请先选择排产日期范围！");
            if (WorkCenterSelect.SelectedValue == null)
                throw new Exception("请先选择工作中心！");
            if (PrinterSelect.SelectedValue == null)
                throw new Exception("请先选择打印机！");
        }

        private void ValidateSubmission()
        {
            if (ReceiptInputNumber.Value <= 0) throw new Exception("入库数量必须大于0");
            if (_currentProcess == null || _currentProcess.Id == 0) throw new Exception("订单信息无效，请重新扫描订单号");
            //if (_currentProcess.CompletedQuantity <= 0)
            //    throw new Exception("当前工序未报工，无法进行入库！");
            if (PrinterSelect.SelectedValue == null) throw new Exception("请选择打印机");
        }

        private async Task<List<GoodsReceiptManagementOrderView>> QueryOrdersAsync()
        {
            var workCenterCode = (((AntdUI.MenuItem)WorkCenterSelect.SelectedValue).Text).Split('-')[0];
            var startDate = DispatchDatePickerRange.Value[0];
            var endDate = DispatchDatePickerRange.Value[1];

            var processes = await _facade.WorkOrderProcessService.GetNeedReceiptWorkOrderProcessAsync(
                AppSession.CurrentFactoryId, startDate, endDate, workCenterCode);

            if (processes == null || !processes.Any())
                throw new Exception("未查询到符合条件的订单信息");

            // 批量获取已入库记录，减少数据库往返
            var processIds = processes.Select(r => r.Id).ToList();
            var allReceipts = await _finishedGoodsReceiptService.GetListByWorkOrderProcessIdAsync(processIds);

            // 在内存中聚合数据
            return processes.GroupJoin(allReceipts,
                proc => proc.Id,
                receipt => receipt.WorkOrderProcessId,
                (proc, receipts) =>
                {
                    decimal receiptedQty = receipts?.Sum(x => x.Quantity) ?? 0;
                    return new GoodsReceiptManagementOrderView
                    {
                        WorkOrderId = (int)proc.WorkOrderId,
                        WorkOrderProcessId = proc.Id,
                        WorkOrderNo = proc.WorkOrderNo,
                        Operation = proc.Operation,
                        WorkCenter = proc.WorkCenter,
                        Quantity = proc.Quantity,
                        CompletedQuantity = proc.CompletedQuantity,
                        ReceiptedQuantity = receiptedQty,
                        DispatchDate = proc.DispatchDate,
                        ProfitCenter = proc.ProfitCenter,
                        MaterialCode = proc.MaterialCode,
                        MaterialDesc = proc.MaterialDesc,
                        StorageLocation = proc.StorageLocation,
                        Status = CalculateStatus(proc.CompletedQuantity ?? 0, proc.Quantity ?? 0, receiptedQty)
                    };
                }).ToList();
        }

        private string CalculateStatus(decimal completed, decimal required, decimal receipted)
        {
            if (receipted >= required) return "已入库";
            if (receipted > 0) return "部分入库";
            if (completed == 0) return "未开始";
            return "待入库";
        }

        private async Task<WorkOrderProcessDto> FindProcessByOrderNoAsync(string orderNo)
        {
            var dataSource = OrderTable.DataSource as List<GoodsReceiptManagementOrderView>;
            if (dataSource == null || !dataSource.Any())
                throw new Exception("请先执行查询");

            var matches = dataSource.Where(x => x.WorkOrderNo == orderNo).ToList();
            if (!matches.Any()) throw new Exception($"列表中未找到订单 {orderNo}");
            if (matches.Count > 1) throw new Exception($"订单 {orderNo} 存在多条记录，请联系管理员");

            var targetView = matches.First();

            // 获取完整 Process 信息
            var process = await _facade.WorkOrderProcessService.GetByIdAsync(targetView.WorkOrderProcessId);

            // 补全物料信息 (从 WorkOrder 获取)
            var workOrder = await _facade.WorkOrderService.GetByIdAsync((int)process.WorkOrderId);
            process.MaterialCode = workOrder.MaterialCode;
            process.MaterialDesc = workOrder.MaterialDesc;
            process.StorageLocation = workOrder.StorageLocation;

            return process;
        }

        private void BindProcessToUI(WorkOrderProcessDto process)
        {
            _currentProcess = process;
            WorkOrderLabel.Text = process.WorkOrderNo;
            MaterialLabel.Text = $"{process.MaterialCode} / {process.MaterialDesc}";
            SubmitButton.Enabled = true;
            ResetButton.Enabled = true;
        }

        private async Task<FinishedGoodsReceiptDto> CreateLocalReceiptAsync()
        {
            var factory = await _facade.FactoryService.GetByIdAsync(AppSession.CurrentFactoryId);
            var serialNo = _facade.Serial.GenerateNext($"{factory.FactoryCode}{SERIAL_CODE_PREFIX}");

            var dto = new FinishedGoodsReceiptCreateDto
            {
                WorkOrderId = (int)_currentProcess.WorkOrderId,
                WorkOrderProcessId = _currentProcess.Id,
                WorkOrderNo = _currentProcess.WorkOrderNo,
                MaterialCode = _currentProcess.MaterialCode,
                MaterialDesc = _currentProcess.MaterialDesc,
                Quantity = ReceiptInputNumber.Value,
                FactoryId = factory.Id,
                FactoryCode = factory.FactoryCode,
                BaseUnit = "ST",
                StorageLocation = _currentProcess.StorageLocation,
                WorkCenterCode = _currentProcess.WorkCenter,
                SapTransferNo = serialNo,
                CreatedBy = AppSession.CurrentUser.EmployeeId
            };

            var result = await _finishedGoodsReceiptService.CreateAsync(dto);
            if (result == null) throw new Exception("本地创建入库记录失败");
            return result;
        }

        private async Task<FinishedGoodsReceiptDto> PushToSapAsync(FinishedGoodsReceiptDto receipt)
        {
            var receiptDto = await _taskExecutionService.FinishedGoodsReceiptToSapAsync(receipt);
            if (receiptDto.SapMessageType?.ToUpper() != "S")
                throw new Exception(receiptDto.SapMessageType);
            return receiptDto;
                
        }

        private async Task PrintLabelAsync(FinishedGoodsReceiptDto receipt)
        {
            try
            {
                // 1. 获取 SAP 标签数据
                var dataUrl = $"{_facade.ApiSettings["MesApi"].Endpoints["GetSapLabelComponent"]}?factoryCode={receipt.FactoryCode}&materialCode={receipt.MaterialCode}";
                var labelDataRes = await _facade.MesApi.GetAsync<SapLabelDataComponentDto>(dataUrl);

                if (!labelDataRes.IsSuccess || labelDataRes.Data == null)
                    throw new Exception($"获取标签数据失败: {labelDataRes.Message}");

                var labelData = labelDataRes.Data;
                labelData.LIEFL = receipt.Quantity.ToString();
                labelData.SAPAUF = receipt.WorkOrderNo;
                labelData.ZUSATZFELD = receipt.SapBatchNo;
                labelData.CHARG = receipt.SapBatchNo;
                labelData.TYPBEZ01 = receipt.MaterialDesc;

                // 2. 确定模板名称
                string layoutName = await ResolveLayoutNameAsync(receipt.FactoryCode, labelData.ETFORM, labelData.SPRSL);

                // 3. 生成 XML (TODO: 将路径配置化)
                string layoutPath = $@"D:\bartender\layout\{layoutName}.btw";
                string printerName = PrinterSelect.SelectedValue.ToString().Split('-')[0];

                var xml = BartenderConverter.GenerateXml(labelData, layoutPath, printerName);
                _lastPrintedXml = xml; // 缓存用于补打

                // 4. 执行打印
                await ExecutePrintRequestAsync(xml);
                ReprintButton.Enabled = true;
            }
            catch (Exception ex)
            {
                // 打印失败仅提示，不抛出异常阻断流程（因为入库已成功）
                AntdUI.Message.warn(this, $"入库成功，但打印失败: {ex.Message}");
            }
        }

        private async Task<string> ResolveLayoutNameAsync(string factoryCode, string etform, string sprsl)
        {
            var paramsGroup = await _facade.Params.GetGroupWithItemsAsync($"{factoryCode}{LAYOUT_PARAM_GROUP}");
            if (paramsGroup?.Items == null) return DEFAULT_PRINT_LAYOUT;

            var candidates = paramsGroup.Items.Where(x => x.Key == etform).ToList();
            if (!candidates.Any()) return DEFAULT_PRINT_LAYOUT;

            // 如果有多个匹配，弹窗让用户选
            if (candidates.Count > 1)
            {
                string? selected = null;
                var menuItems = candidates.Select(x => new AntdUI.MenuItem
                {
                    Name = ProcessLayoutName(x.Value, sprsl),
                    Text = ProcessLayoutName(x.Value, sprsl)
                }).ToList();

                _formFactory.Show<CommonSelectForm>(f => {
                    f.FormClosed += (s, e) => {
                        if (f.DialogResult == DialogResult.OK) selected = f.SelectedValue.Text;
                    };
                }, true, "选择标签模板", menuItems);

                if (string.IsNullOrEmpty(selected)) throw new Exception("未选择打印模板");
                return selected;
            }

            return ProcessLayoutName(candidates.First().Value, sprsl);
        }

        private string ProcessLayoutName(string templateName, string languageCode)
        {
            if (string.IsNullOrWhiteSpace(templateName)) return "";
            if (string.IsNullOrWhiteSpace(languageCode)) return templateName.TrimEnd('_');

            // 智能替换逻辑：Label_EN -> Label_CN
            int lastUnderscore = templateName.LastIndexOf('_');
            return lastUnderscore > 0
                ? templateName.Substring(0, lastUnderscore) + "_" + languageCode
                : templateName + "_" + languageCode;
        }

        private async Task ExecutePrintRequestAsync(string xmlContent)
        {
            var url = _facade.ApiSettings["BartenderApi"].Endpoints["LabelPrint"];

            // 使用 Polly 重试打印请求
            var response = await _retryPolicy.ExecuteAsync(async () =>
                await _facade.BartApiClient.PostXmlAsync<BartenderResponse>(url, xmlContent));

            if (response == null || !response.IsQueuedSuccess)
            {
                throw new Exception($"打印服务返回错误状态: {response?.Status}");
            }
        }

        private void UpdateStepStatus(TStepState status, int currentStep)
        {
            ReceiptSteps.Status = status;
            ReceiptSteps.Current = currentStep;
        }

        #endregion
    }

    public class GoodsReceiptManagementOrderView : AntdUI.NotifyProperty
    {
        private int _workOrderId;
        public int WorkOrderId
        {
            get => _workOrderId;
            set { if (_workOrderId == value) return; _workOrderId = value; OnPropertyChanged(); }
        }

        private int _workOrderProcessId;
        public int WorkOrderProcessId
        {
            get => _workOrderProcessId;
            set { if (_workOrderProcessId == value) return; _workOrderProcessId = value; OnPropertyChanged(); }
        }

        private string? _workOrderNo;
        public string? WorkOrderNo
        {
            get => _workOrderNo;
            set { if (_workOrderNo == value) return; _workOrderNo = value; OnPropertyChanged(); }
        }

        private string? _operation;
        public string? Operation
        {
            get => _operation;
            set { if (_operation == value) return; _operation = value; OnPropertyChanged(); }
        }

        private string? _workCenter;
        public string? WorkCenter
        {
            get => _workCenter;
            set { if (_workCenter == value) return; _workCenter = value; OnPropertyChanged(); }
        }

        private string? _materialCode;
        public string? MaterialCode
        {
            get => _materialCode;
            set { if (_materialCode == value) return; _materialCode = value; OnPropertyChanged(); }
        }

        private string? _materialDesc;
        public string? MaterialDesc
        {
            get => _materialDesc;
            set { if (_materialDesc == value) return; _materialDesc = value; OnPropertyChanged(); }
        }

        private decimal? _quantity;
        public decimal? Quantity
        {
            get => _quantity;
            set { if (_quantity == value) return; _quantity = value; OnPropertyChanged(); }
        }

        private decimal? _completedQuantity;
        public decimal? CompletedQuantity
        {
            get => _completedQuantity;
            set { if (_completedQuantity == value) return; _completedQuantity = value; OnPropertyChanged(); }
        }

        private decimal? _receiptedQuantity;
        public decimal? ReceiptedQuantity
        {
            get => _receiptedQuantity;
            set { if (_receiptedQuantity == value) return; _receiptedQuantity = value; OnPropertyChanged(); }
        }

        private DateTime? _dispatchDate;
        public DateTime? DispatchDate
        {
            get => _dispatchDate;
            set { if (_dispatchDate == value) return; _dispatchDate = value; OnPropertyChanged(); }
        }

        private string? _profitCenter;
        public string? ProfitCenter
        {
            get => _profitCenter;
            set { if (_profitCenter == value) return; _profitCenter = value; OnPropertyChanged(); }
        }

        private string? _storageLocation;
        public string? StorageLocation
        {
            get => _storageLocation;
            set { if (_storageLocation == value) return; _storageLocation = value; OnPropertyChanged(); }
        }

        private string? _status;
        public string? Status
        {
            get => _status;
            set { if (_status == value) return; _status = value; OnPropertyChanged(); }
        }
    }
}
