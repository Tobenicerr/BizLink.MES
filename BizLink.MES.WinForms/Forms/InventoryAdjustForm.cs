using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Facade;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Helpers;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Infrastructure; // 引用基础架构
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    // 1. 继承 MesWindowForm (弹窗基类)
    public partial class InventoryAdjustForm : MesWindowForm
    {
        #region Fields & Constants

        private readonly InventoryModuleFacade _facade;
        private readonly IFormFactory _formFactory;

        // 数据状态
        private int? _workOrderTaskId;
        private int _stockId;
        private RawLinesideStockDto _rawLinesideStockDto;
        private List<WorkOrderTaskMaterialAddDto> _workOrderTaskMaterialAddDto;

        // 事件回调
        public event Action OnSuccess;

        // 常量
        private const string ParamGroup_CableCutScrapReason = "CableCutScrapReason";
        private const string ParamGroup_CostCenterList = "CostCenterList";
        private const string ParamGroup_LocalWorkOrder = "CN11StockAdjustLocalWorkOrder";
        private const string ParamGroup_SapLocation = "CN11SAPStockLocation";
        private const string Key_CableRawLineStock = "CableRawLineStock";
        private const string Key_SAPProMtrStock = "SAPProMtrStock";

        #endregion

        // 2. 构造函数：只注入 Facade 和 Factory
        public InventoryAdjustForm(InventoryModuleFacade facade, IFormFactory formFactory)
        {
            _facade = facade;
            _formFactory = formFactory;
            InitializeComponent();
        }

        // 3. 初始化数据入口 (替代构造函数传参)
        public void InitData(int? stockId = null, int? workOrderTaskId = null)
        {
            if (stockId.HasValue)
                _stockId = stockId.Value;
            if (workOrderTaskId.HasValue)
                _workOrderTaskId = workOrderTaskId.Value;
        }

        // 4. 窗体加载
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e); // 必须调用
            InitializeUIComponents();

            await RunAsync(async () =>
            {
                await LoadDataAsync();
            });
        }

        private void InitializeUIComponents()
        {
            // 批量设置样式
            AntdUI.Label[] labels = { label2, label3, label4, label6, label7, label8, label9, label10 };
            foreach (var lbl in labels)
                lbl.PrefixColor = Color.Red;

            adjustSelect.PlaceholderText = "请选择调整类型...";
            changeInputNumber.PlaceholderText = "请输入调整数量...";
            orderSelect.PlaceholderText = "请选择订单号...";
            adjustReasonSelect.PlaceholderText = "请选择调整原因...";
            costCenterSelect.PlaceholderText = "请选择成本中心...";
        }

        #region Data Loading

        private async Task LoadDataAsync()
        {
            // 1. 初始化调整类型
            var opTypes = new List<StockOperationType> { StockOperationType.AdjustStockGain, StockOperationType.AdjustStockLoss };

            if (_workOrderTaskId == null)
            {
                opTypes.AddRange(new[] {
                    StockOperationType.ShipmentOfRawMaterials,
                    StockOperationType.StockGain,
                    StockOperationType.StockLoss,
                    StockOperationType.ShipmentReturn,
                    StockOperationType.AdjustStockGain,
                    StockOperationType.AdjustStockLoss,
                    StockOperationType.GoodsIssuetoCostCenter,
                    StockOperationType.ReversalofGoodsIssuetoCostCenter,
                    StockOperationType.ProductLineStockTransfer
                });
            }

            adjustSelect.Items.Clear();
            adjustSelect.Items.AddRange(opTypes.Select(t => new MenuItem
            {
                Name = ((int)t).ToString(),
                Text = t.GetDescription()
            }).ToArray());

            // 2. 加载库存或任务信息
            var workorders = new List<WorkOrderDto>();

            // A. 从库存加载
            if (_stockId > 0)
            {
                _rawLinesideStockDto = await _facade.RawStock.GetByIdAsync(_stockId);
                if (_rawLinesideStockDto != null)
                {
                    PopulateStockInfo(_rawLinesideStockDto);
                    var orders = await _facade.WorkOrderService.GetListByBomMaterialAsync(_rawLinesideStockDto.MaterialCode);
                    if (orders != null)
                        workorders.AddRange(orders);
                }
            }

            // B. 从任务加载
            if (_workOrderTaskId > 0)
            {
                var task = await _facade.Task.GetByIdAsync(_workOrderTaskId.Value);
                if (task != null)
                {
                    var wo = await _facade.WorkOrderService.GetByIdAsync(task.OrderId);
                    if (wo != null)
                        workorders.Add(wo);

                    _workOrderTaskMaterialAddDto = await _facade.MaterialAdd.GetByTaskIdAsync(task.Id);
                    if (_workOrderTaskMaterialAddDto != null)
                    {
                        materialSelect.Items.AddRange(_workOrderTaskMaterialAddDto.Select(x => x.MaterialCode).Distinct().ToArray());
                    }
                }
            }

            orderSelect.Items.AddRange(workorders.Select(x => new MenuItem { Name = x.Id.ToString(), Text = x.OrderNumber }).ToArray());

            // 3. 加载调整原因
            var parameterGroup = await _facade.Params.GetGroupWithItemsAsync(ParamGroup_CableCutScrapReason);
            adjustReasonSelect.Items.Clear();
            if (parameterGroup != null)
            {
                foreach (var item in parameterGroup.Items)
                {
                    adjustReasonSelect.Items.Add(new MenuItem { Name = item.Value, Text = $"{item.Value}-{item.Name}" });
                }
            }
        }

        private void PopulateStockInfo(RawLinesideStockDto stock)
        {
            materialSelect.Items.Add(stock.MaterialCode);
            materialSelect.SelectedIndex = 0;
            batchSelect.Items.Add(stock.BatchCode);
            batchSelect.SelectedIndex = 0;
            barcodeSelect.Items.Add(stock.BarCode);
            barcodeSelect.SelectedIndex = 0;
            originalInputNumber.Value = (decimal)stock.LastQuantity;
        }

        #endregion

        #region Submission Logic

        private async void submitButton_Click(object sender, EventArgs e)
        {
            // 使用 RunAsync 自动处理 Loading、禁用、异常
            await RunAsync(submitButton, async () =>
            {
                // 1. 获取配置与校验
                if (adjustSelect.SelectedValue == null)
                    throw new Exception("调整类型未选择！");

                var opType = (StockOperationType)int.Parse(((MenuItem)adjustSelect.SelectedValue).Name);

                if (opType == StockOperationType.ProductLineStockTransfer)
                {
                    if (adjustReasonSelect.SelectedValue == null)
                    {
                        if (AntdUI.Modal.open(this, "提示", "2100库移库调整未选择调整原因，如果需要报废物料，请重新选择报废原因，是否继续？", AntdUI.TType.Warn) != DialogResult.OK)
                            return;
                    }
                    else
                    {
                        if (AntdUI.Modal.open(this, "提示", "2100库移库调整选择调整原因，本次调整将报废至订单，是否继续？", AntdUI.TType.Warn) != DialogResult.OK)
                            return;
                        opType = StockOperationType.StockLoss;
                    }
                }

                var config = GetOperationConfig(opType);


                // 3. 确保库存对象已加载
                await EnsureStockDtoLoadedAsync();

                await ValidateInputsAsync(config);

                // 2. 确认
                if (AntdUI.Modal.open(this, "提示", "即将对库存进行调整，是否继续？", AntdUI.TType.Warn) != DialogResult.OK)
                    return;


                // 4. 执行数据库更新
                var changeQty = changeInputNumber.Value;
                var deltaQty = config.IsAddition ? changeQty : -changeQty;

                // A. 更新库存
                var updateDto = new RawLinesideStockUpdateDto
                {
                    Id = _rawLinesideStockDto.Id,
                    LastQuantity = _rawLinesideStockDto.LastQuantity + deltaQty,
                    UpdateBy = AppSession.CurrentUser.EmployeeId,
                    UpdatedAt = DateTime.Now,
                };

                if (!await _facade.RawStock.UpdateAsync(updateDto))
                    throw new Exception("库存更新失败，请重试！");

                // B. 写入日志
                var logDto = CreateLogDto(opType, config, deltaQty);
                var stockLog = await _facade.StockLog.CreateAsync(logDto);
                if (stockLog == null)
                    throw new Exception("日志创建失败！");

                // 5. SAP 同步
                bool sapSuccess = false;
                if (config.TransferType != MaterialTransferType.GoodsIssue || opType == StockOperationType.ShipmentOfRawMaterials || opType == StockOperationType.ShipmentReturn)
                {
                    sapSuccess = await SyncToSapAsync(opType, config, stockLog, changeQty);
                    // 回写日志状态
                    await _facade.StockLog.UpdateAsync(new RawLinesideStockLogUpdateDto
                    {
                        Id = stockLog.Id,
                        SapStatus = sapSuccess ? "1" : "-1"
                    });
                }

                // 6. 打印 (仅外发且SAP成功)
                if (sapSuccess && opType == StockOperationType.ShipmentOfRawMaterials)
                {
                    await HandlePrintingAsync(changeQty);
                }

                // 7. 成功回调
                OnSuccess?.Invoke();
                this.DialogResult = DialogResult.OK;
                this.Close();

            }, successMsg: "操作成功！");
        }

        #endregion

        #region Business Logic Helpers

        private OperationConfig GetOperationConfig(StockOperationType type)
        {
            switch (type)
            {
                case StockOperationType.StockGain:
                    return new OperationConfig { IsAddition = true, NeedReason = true, NeedOrder = true, ConsumptionType = ConsumptionType.ScrapReversal, TransferType = MaterialTransferType.RawMaterialScrapAgainstOrder };
                case StockOperationType.StockLoss:
                    return new OperationConfig { IsAddition = false, NeedReason = true, NeedOrder = true, ConsumptionType = ConsumptionType.Scrap, TransferType = MaterialTransferType.RawMaterialScrapAgainstOrder };
                case StockOperationType.ShipmentOfRawMaterials:
                    return new OperationConfig { IsAddition = false, NeedOrder = false, ConsumptionType = ConsumptionType.MaterialTransfer, TransferType = MaterialTransferType.GoodsIssue };
                case StockOperationType.ShipmentReturn:
                    return new OperationConfig { IsAddition = true, NeedOrder = false, ConsumptionType = ConsumptionType.MaterialTransfer, TransferType = MaterialTransferType.GoodsIssue };
                case StockOperationType.AdjustStockGain:
                    return new OperationConfig { IsAddition = true, NeedReason = true, NeedOrder = true, ConsumptionType = ConsumptionType.AdjustStockGain, TransferType = MaterialTransferType.RawMaterialScrapAgainstOrder };
                case StockOperationType.AdjustStockLoss:
                    return new OperationConfig { IsAddition = false, NeedReason = true, NeedOrder = true, ConsumptionType = ConsumptionType.AdjustStockLoss, TransferType = MaterialTransferType.RawMaterialScrapAgainstOrder };
                case StockOperationType.GoodsIssuetoCostCenter:
                    return new OperationConfig { IsAddition = false, NeedCostCenter = true, ConsumptionType = ConsumptionType.GoodsIssuetoCostCenter, TransferType = MaterialTransferType.GoodsIssuetoCostCenter };
                case StockOperationType.ReversalofGoodsIssuetoCostCenter:
                    return new OperationConfig { IsAddition = true, NeedCostCenter = true, ConsumptionType = ConsumptionType.ReversalofGoodsIssuetoCostCenter, TransferType = MaterialTransferType.ReversalofGoodsIssuetoCostCenter };
                case StockOperationType.ProductLineStockTransfer:
                    return new OperationConfig { IsAddition = false, NeedOrder = true, NeedReason = null, ConsumptionType = ConsumptionType.Consumption, TransferType = MaterialTransferType.RawMaterialScrapAgainstOrder };
                default:
                    return new OperationConfig { IsAddition = false, TransferType = MaterialTransferType.GoodsIssue };
            }
        }

        private async Task ValidateInputsAsync(OperationConfig config)
        {
            if (changeInputNumber.Value <= 0)
                throw new Exception("请输入有效的调整数量！");
            if (config.NeedReason != null && (bool)config.NeedReason && adjustReasonSelect.SelectedValue == null)
                throw new Exception("请选择调整原因！");
            if (config.NeedOrder && orderSelect.SelectedValue == null)
                throw new Exception("请选择订单号！");
            if (config.NeedCostCenter && costCenterSelect.SelectedValue == null)
                throw new Exception("请选择成本中心！");

            if (!config.IsAddition && changeInputNumber.Value > originalInputNumber.Value)
            {
                throw new Exception($"调整数量({changeInputNumber.Value})不能大于当前库存({originalInputNumber.Value})！");
            }
            await Task.CompletedTask;
        }

        private async Task EnsureStockDtoLoadedAsync()
        {
            if (_stockId == 0)
            {
                if (barcodeSelect.SelectedValue == null)
                    throw new Exception("请选择条码或库存！");
                _rawLinesideStockDto = await _facade.RawStock.GetByBarCodeAsync(AppSession.CurrentFactoryId, barcodeSelect.SelectedValue.ToString());
            }
            else
            {
                _rawLinesideStockDto = await _facade.RawStock.GetByIdAsync(_stockId);
            }

            if (_rawLinesideStockDto == null)
                throw new Exception("未查询到线边库库存信息！");
        }

        private RawLinesideStockLogCreateDto CreateLogDto(StockOperationType opType, OperationConfig config, decimal deltaQty)
        {
            return new RawLinesideStockLogCreateDto
            {
                RawLinesideStockId = _rawLinesideStockDto.Id,
                OperationType = opType,
                InOutStatus = config.IsAddition ? InOutStatus.In : InOutStatus.Out,
                WorkOrderId = orderSelect.SelectedValue != null ? int.Parse(((MenuItem)orderSelect.SelectedValue).Name) : null,
                WorkOrderNo = orderSelect.SelectedValue != null ? ((MenuItem)orderSelect.SelectedValue).Text : null,
                ChangeQuantity = Math.Abs(deltaQty),
                QuantityBefore = (decimal)_rawLinesideStockDto.LastQuantity,
                QuantityAfter = (decimal)_rawLinesideStockDto.LastQuantity + deltaQty,
                MaterialCode = _rawLinesideStockDto.MaterialCode,
                BarCode = _rawLinesideStockDto.BarCode,
                BatchCode = _rawLinesideStockDto.BatchCode,
                BaseUnit = _rawLinesideStockDto.BaseUnit,
                TransferReason = adjustReasonSelect.SelectedValue == null ? null : ((MenuItem)adjustReasonSelect.SelectedValue).Name,
                LocationId = (int)_rawLinesideStockDto.LocationId,
                LocationCode = _rawLinesideStockDto.LocationCode,
                CreateBy = AppSession.CurrentUser.EmployeeId,
                Remark = opType.GetDescription()
            };
        }

        private async Task<bool> SyncToSapAsync(StockOperationType opType, OperationConfig config, RawLinesideStockLogDto log, decimal qty)
        {
            var sapLocationParams = await _facade.Params.GetGroupWithItemsAsync(ParamGroup_SapLocation);

            string GetLocation(string key, string defaultVal) => sapLocationParams.Items.FirstOrDefault(x => x.Key == key)?.Value ?? defaultVal;
            string fromLoc = GetLocation(Key_CableRawLineStock, "2200");
            string toLoc = null;

            if (opType == StockOperationType.ShipmentReturn)
            {
                toLoc = fromLoc;
                fromLoc = GetLocation(Key_SAPProMtrStock, "3100");
            }
            else if (opType == StockOperationType.ShipmentOfRawMaterials)
            {
                toLoc = GetLocation(Key_SAPProMtrStock, "3100");
            }

            var request = new TransferSapRequest
            {
                FactoryCode = AppSession.CurrentUser.FactoryName,
                EmployeeId = AppSession.CurrentUser.EmployeeId,
                TransferType = config.TransferType,
                ConsumptionType = config.ConsumptionType,
                Stocks = new List<TransferStock> {
                    new TransferStock {
                        StockId = _rawLinesideStockDto.Id,
                        StockLogId = log.Id,
                        MaterialCode = _rawLinesideStockDto.MaterialCode,
                        BatchCode = _rawLinesideStockDto.BatchCode,
                        Quantity = qty,
                        BaseUnit = _rawLinesideStockDto.BaseUnit
                    }
                },
                WorkOrderId = orderSelect.SelectedValue != null ? int.Parse(((MenuItem)orderSelect.SelectedValue).Name) : null,
                WorkOrderNo = orderSelect.SelectedValue != null ? ((MenuItem)orderSelect.SelectedValue).Text : null,
                FromLocation = fromLoc,
                ToLocation = toLoc,
                MovementReason = adjustReasonSelect.SelectedValue?.ToString(), // 注意：确认 Facade 里的 Name
                CostCenterCode = costCenterSelect.SelectedValue != null ? ((MenuItem)costCenterSelect.SelectedValue).Name : null
            };

            // 修正 SelectedValue 类型获取
            if (adjustReasonSelect.SelectedValue is MenuItem item)
                request.MovementReason = item.Name;
            var json = JsonConvert.SerializeObject(request);
            var rtn = await _facade.MesApi.PostAsync<object, object>(_facade.ApiSettings["MesApi"].Endpoints["LineStockTransferToSAP"], request);
            return rtn != null && rtn.IsSuccess;
        }

        // 【核心修改】HandlePrintingAsync
        private async Task HandlePrintingAsync(decimal qty)
        {
            string chosenPrinter = null;

            // 1. 使用 Factory 打开模态窗口
            // 技巧：利用 setup 回调绑定 FormClosed 事件来捕获结果
            // isModal: true 会阻塞当前线程（UI线程消息循环），直到窗体关闭
            _formFactory.Show<PrinterSelectForm>(form =>
            {
                form.FormClosed += (s, e) =>
                {
                    // 在窗体关闭时捕获数据
                    if (form.DialogResult == DialogResult.OK)
                    {
                        chosenPrinter = form.SelectedPrinterName;
                    }
                };
            }, isModal: true);

            // 2. 窗体关闭后继续执行
            if (string.IsNullOrWhiteSpace(chosenPrinter))
            {
                // 如果用户取消了打印，根据需求处理，这里抛异常中断或仅仅 return
                throw new Exception("已取消打印！");
            }

            var parts = chosenPrinter.Split('-');
            var printType = (parts.Length > 1 && parts[1] == PrinterType.NetworkPrinter.GetDescription())
                ? PrinterType.NetworkPrinter : PrinterType.LocalPrinter;

            // 3. 重新获取最新库存 (因为数量可能已更新)
            var stock = await _facade.RawStock.GetByIdAsync(_rawLinesideStockDto.Id);

            bool printRtn = LabelPrintHelper.ShipmentOfRawMaterialPrinter(
                printType,
                chosenPrinter.Replace("-" + printType.GetDescription(), string.Empty),
                stock.MaterialCode, stock.MaterialDesc, stock.BatchCode, stock.LocationCode, qty
            );

            if (!printRtn)
                throw new Exception("打印指令发送失败！");
        }

        #endregion

        #region UI Events (Dropdown Logic)

        private async void adjustSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            if (adjustSelect.SelectedValue == null)
                return;

            // Reset UI
            adjustReasonSelect.Enabled = false;
            adjustReasonSelect.SelectedIndex = -1;
            orderSelect.Enabled = false;
            orderSelect.SelectedIndex = -1;
            costCenterSelect.Enabled = false;
            costCenterSelect.SelectedIndex = -1;

            var opType = (StockOperationType)int.Parse(((MenuItem)adjustSelect.SelectedValue).Name);
            var config = GetOperationConfig(opType);

            if (config.NeedReason == null || (bool)config.NeedReason)
                adjustReasonSelect.Enabled = true;
            if (config.NeedCostCenter)
            {
                costCenterSelect.Enabled = true;
                if (costCenterSelect.Items.Count == 0)
                {
                    var group = await _facade.Params.GetGroupWithItemsAsync(ParamGroup_CostCenterList);
                    if (group != null)
                        costCenterSelect.Items.AddRange(group.Items.Select(i => new MenuItem { Name = i.Key, Text = $"{i.Key}-{i.Value}" }).ToArray());
                }
            }

            if (config.NeedOrder || opType == StockOperationType.ProductLineStockTransfer)
            {
                orderSelect.Enabled = true;
                if (opType == StockOperationType.AdjustStockGain || opType == StockOperationType.AdjustStockLoss)
                {
                    var group = await _facade.Params.GetGroupWithItemsAsync(ParamGroup_LocalWorkOrder);
                    if (group?.Items.FirstOrDefault() != null)
                    {
                        orderSelect.Items.Clear();
                        var orders = group.Items.First().Value.Split(',').Select(x => new MenuItem { Name = x, Text = x }).ToArray();
                        orderSelect.Items.AddRange(orders);
                    }
                }
                else if (opType == StockOperationType.ProductLineStockTransfer)
                {
                    orderSelect.Items.Clear();
                    var workorders = await _facade.WorkOrderService.GetListByDispatchDateEndAsync(AppSession.CurrentFactoryId, DateTime.Now.AddDays(3).Date);
                    orderSelect.Items.AddRange(workorders.Select(x => new MenuItem { Name = x.Id.ToString(), Text = x.OrderNumber }).ToArray());
                }
                else if (_rawLinesideStockDto != null)
                {
                    orderSelect.Items.Clear();
                    var workorders = await _facade.WorkOrderService.GetListByBomMaterialAsync(_rawLinesideStockDto.MaterialCode);
                    orderSelect.Items.AddRange(workorders.Select(x => new MenuItem { Name = x.Id.ToString(), Text = x.OrderNumber }).ToArray());
                }
            }
        }

        private void materialSelect_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            if (_workOrderTaskMaterialAddDto != null && materialSelect.SelectedValue != null)
            {
                batchSelect.Items.Clear();
                batchSelect.Items.AddRange(_workOrderTaskMaterialAddDto
                    .Where(x => x.MaterialCode == materialSelect.SelectedValue.ToString())
                    .Select(x => x.BatchCode).Distinct().ToArray());
            }
        }

        private void batchSelect_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            if (_workOrderTaskMaterialAddDto != null && materialSelect.SelectedValue != null && batchSelect.SelectedValue != null)
            {
                barcodeSelect.Items.Clear();
                barcodeSelect.Items.AddRange(_workOrderTaskMaterialAddDto
                    .Where(x => x.MaterialCode == materialSelect.SelectedValue.ToString() && x.BatchCode == batchSelect.SelectedValue.ToString())
                    .Select(x => x.BarCode).Distinct().ToArray());
            }
        }

        private void barcodeSelect_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            if (_workOrderTaskMaterialAddDto != null && materialSelect.SelectedValue != null &&
                batchSelect.SelectedValue != null && barcodeSelect.SelectedValue != null)
            {
                originalInputNumber.Value = 0;
                var batch = _workOrderTaskMaterialAddDto
                    .Where(x => x.MaterialCode == materialSelect.SelectedValue.ToString() &&
                                x.BatchCode == batchSelect.SelectedValue.ToString() &&
                                x.BarCode == barcodeSelect.SelectedValue.ToString())
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                if (batch != null)
                    originalInputNumber.Value = (decimal)batch.LastQuantity;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #endregion

        private class OperationConfig
        {
            public bool IsAddition
            {
                get; set;
            }
            public bool? NeedReason
            {
                get; set;
            } = false;
            public bool NeedOrder
            {
                get; set;
            }
            public bool NeedCostCenter
            {
                get; set;
            }
            public ConsumptionType ConsumptionType
            {
                get; set;
            }
            public MaterialTransferType TransferType
            {
                get; set;
            }
        }
    }
}