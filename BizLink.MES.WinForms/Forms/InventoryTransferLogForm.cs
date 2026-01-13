using AntdUI;
using Azure.Core;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.Shared.Helpers;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.WinForms.Common;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class InventoryTransferLogForm : Form
    {
        #region Fields & Constants

        private readonly IRawLinesideStockLogService _rawLinesideStockLogService;
        private readonly IRawLinesideStockService _rawLinesideStockService;
        private readonly IMaterialTransferLogService _materialTransferLogService;
        private readonly IMesApiClient _mesApiClient;
        private readonly IParameterGroupService _parameterGroupService;
        private readonly Dictionary<string, ServiceEndpointSettings> _apiSettings;

        private const string GROUP_SAP_LOCATION = "CN11SAPStockLocation";
        private const string KEY_CABLE_RAW_STOCK = "CableRawLineStock";
        private const string KEY_SAP_PRO_MTR_STOCK = "SAPProMtrStock";
        private const string DEFAULT_FROM_LOC = "2200";
        private const string DEFAULT_TO_LOC = "3100";

        #endregion

        public InventoryTransferLogForm(
            IRawLinesideStockLogService rawLinesideStockLogService,
            IRawLinesideStockService rawLinesideStockService,
            IMesApiClient mesApiClient,
            IParameterGroupService parameterGroupService,
            IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings,
            IMaterialTransferLogService materialTransferLogService)
        {
            InitializeComponent();
            _rawLinesideStockLogService = rawLinesideStockLogService;
            _rawLinesideStockService = rawLinesideStockService;
            _mesApiClient = mesApiClient;
            _parameterGroupService = parameterGroupService;
            _apiSettings = apiSettings.Value;
            _materialTransferLogService = materialTransferLogService;

            InitializeTable();
        }

        private void InventoryTransferLogForm_Load(object sender, EventArgs e)
        {
            keywordInput.PlaceholderText = "请输入物料号或批次号...";
            if (AppSession.CurrentFactoryId <= 0)
            {
                AntdUI.Message.error(this, "请先选择工厂！");
            }
        }

        #region Initialization

        private void InitializeTable()
        {
            transferTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("Id", "Id", ColumnAlign.Center) { Visible = false },
                new AntdUI.Column("TransferId", "TransferId", ColumnAlign.Center) { Visible = false },
                new AntdUI.Column("OperationType", "操作类别", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetFixed().SetDefaultFilter().SetWidth("auto"),
                new AntdUI.Column("SapStatus", "同步Sap状态", ColumnAlign.Center)
                {
                    Render = (value, record, index) => value?.ToString() switch
                    {
                        "-1" => new AntdUI.CellTag("同步失败", TTypeMini.Error),
                        "0" => new AntdUI.CellTag("未同步", TTypeMini.Primary),
                        "1" => new AntdUI.CellTag("已同步", TTypeMini.Success),
                        _ => null
                    }
                }.SetLocalizationTitleID("Table.Column.").SetFixed().SetWidth("auto"),
                new AntdUI.Column("MaterialCode", "物料号", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetWidth("auto"),
                new AntdUI.Column("ChangeQuantity", "调整数量", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetWidth("auto"),
                new AntdUI.Column("QuantityBefore", "调整前数量", ColumnAlign.Center).SetDefaultFilter().SetWidth("auto"),
                new AntdUI.Column("QuantityAfter", "调整后数量", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetWidth("auto"),
                new AntdUI.Column("BatchCode", "物料批次", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetWidth("auto"),
                new AntdUI.Column("BarCode", "物料标签", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetWidth("auto"),
                new AntdUI.Column("LocationCode", "库位代码", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetWidth("auto"),
                new AntdUI.Column("CreateBy", "创建人", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetWidth("auto"),
                new AntdUI.Column("CreateOn", "创建时间", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetWidth("auto"),
                new AntdUI.Column("SapMessage", "SAP返回信息", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetDefaultFilter().SetWidth("auto"),
                new AntdUI.Column("SapMessageType", "SAP返回标志", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetFixed().SetDefaultFilter().SetWidth("auto"),
                new AntdUI.Column("Btns", "操作", ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetFixed().SetWidth("auto")
            };
        }

        #endregion

        #region Data Loading

        private async void queryButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(keywordInput.Text))
            {
                AntdUI.Message.warn(this, "请输入物料号或批次号！");
                return;
            }

            queryButton.Loading = true;
            try
            {
                var count = await LoadDataAsync();
                if (count > 0)
                {
                    AntdUI.Message.success(this, $"查询成功，共查询到 {count} 条数据！");
                    totalLabel.Text = $"  转移记录数：{count} 笔";
                }
                else
                {
                    AntdUI.Message.info(this, "未查询到相关数据！");
                    transferTable.DataSource = null;
                    totalLabel.Text = "  转移记录数：0 笔";
                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, $"查询出错：{ex.Message}");
            }
            finally
            {
                queryButton.Loading = false;
            }
        }

        private async Task<int> LoadDataAsync()
        {
            var keywords = keywordInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (!keywords.Any())
                return 0;

            var stockLogs = await _rawLinesideStockLogService.GetListByKeywordAsync(keywords);
            if (stockLogs == null || !stockLogs.Any())
                return 0;

            var sapTransfers = await _materialTransferLogService.GetListByStockLogIdAsync(stockLogs.Select(x => x.Id).ToList());

            var viewModels = stockLogs.GroupJoin(
                sapTransfers,
                log => log.Id,
                trans => trans.StockLogId,
                (log, transGroup) => new { Log = log, Trans = transGroup.FirstOrDefault() }
            ).Select(x => MapToViewModel(x.Log, x.Trans)).ToList();

            transferTable.DataSource = viewModels;
            return viewModels.Count;
        }

        private InventoryTransferLogViewModel MapToViewModel(RawLinesideStockLogDto log, MaterialTransferLogDto trans)
        {
            string sapStatus;
            if (IsInternalTransfer(log.OperationType))
            {
                sapStatus = "1"; // 内部流转默认视为不需要同步或已同步
            }
            else
            {
                if (trans?.MessageType == null)
                    sapStatus = "0";
                else if (string.Equals(trans.MessageType, "S", StringComparison.OrdinalIgnoreCase))
                    sapStatus = "1";
                else
                    sapStatus = "-1";
            }

            bool canReEntry = IsReEntryAllowed(log.OperationType) && sapStatus != "1";

            return new InventoryTransferLogViewModel
            {
                Id = log.Id,
                TransferId = trans?.Id,
                OperationType = log.OperationType.GetDescription(),
                OperationTypeEnum = log.OperationType,
                SapStatus = sapStatus,
                MaterialCode = log.MaterialCode,
                ChangeQuantity = log.ChangeQuantity,
                QuantityBefore = log.QuantityBefore,
                QuantityAfter = log.QuantityAfter,
                BatchCode = log.BatchCode,
                BarCode = log.BarCode,
                LocationCode = log.LocationCode,
                CreateBy = log.CreateBy,
                CreateOn = log.CreateOn.ToString("yyyy-MM-dd HH:mm:ss"),
                SapMessageType = trans?.MessageType,
                SapMessage = trans?.Message,
                Btns = new[] {
                    new AntdUI.CellButton("reentry", "重推", TTypeMini.Primary) { Enabled = canReEntry }
                }
            };
        }

        // 提取判断逻辑，使 MapToViewModel 更整洁
        private bool IsInternalTransfer(StockOperationType type) =>
            type == StockOperationType.TransferIn ||
            type == StockOperationType.TransferOut ||
            type == StockOperationType.Replenishment;

        private bool IsReEntryAllowed(StockOperationType type) =>
            type == StockOperationType.StockGain ||
            type == StockOperationType.StockLoss ||
            type == StockOperationType.ShipmentOfRawMaterials ||
            type == StockOperationType.ShipmentReturn ||
            type == StockOperationType.AdjustStockLoss ||
            type == StockOperationType.AdjustStockGain ||
            type == StockOperationType.ProductLineStockTransfer ||
            type == StockOperationType.GoodsIssuetoCostCenter ||
            type == StockOperationType.ReversalofGoodsIssuetoCostCenter;

        #endregion

        #region Operations (Re-entry & Printing)

        private async void transferTable_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is not InventoryTransferLogViewModel record || e.Btn.Id != "reentry")
                return;

            try
            {
                var stockLog = await _rawLinesideStockLogService.GetByIdAsync(record.Id);
                if (stockLog == null)
                {
                    AntdUI.Message.error(this, "未查询到移库记录，无法重推！");
                    return;
                }

                if (AntdUI.Modal.open(this.ParentForm, "提示", "即将向 SAP 重新推送，是否继续？", TType.Warn) != DialogResult.OK)
                {
                    return;
                }

                TransferSapRequest request;

                // 路径 A: 基于已有 TransferLog 构建请求 (历史数据优先)
                if (record.TransferId != null)
                {
                    var tranferDto = await _materialTransferLogService.GetByIdAsync((int)record.TransferId);
                    request = new TransferSapRequest
                    {
                        FactoryCode = AppSession.CurrentUser.FactoryName,
                        EmployeeId = AppSession.CurrentUser.EmployeeId,
                        // 使用 EnumExtensions 将 Description 转换回枚举
                        TransferType = EnumExtensions.GetEnumByDescription<MaterialTransferType>(tranferDto.TransferType),
                        ConsumptionType = EnumExtensions.GetEnumByDescription<ConsumptionType>(tranferDto.MovementType),
                        WorkOrderId = stockLog.WorkOrderId,
                        WorkOrderNo = stockLog.WorkOrderNo,
                        FromLocation = tranferDto.FromLocationCode,
                        ToLocation = tranferDto.ToLocationCode,
                        MovementReason = stockLog.TransferReason,
                        Stocks = BuildStocksList(stockLog)
                    };
                }
                // 路径 B: 基于业务规则重新计算请求参数
                else
                {
                    var (transferType, consumptionType, isShipment) = GetTransferConfig(stockLog.OperationType);
                    var parameterGroup = await _parameterGroupService.GetGroupWithItemsAsync(GROUP_SAP_LOCATION);
                    var fromLocation = GetParameterValue(parameterGroup, KEY_CABLE_RAW_STOCK, DEFAULT_FROM_LOC);
                    var toLocation = isShipment ? GetParameterValue(parameterGroup, KEY_SAP_PRO_MTR_STOCK, DEFAULT_TO_LOC) : "";

                    request = new TransferSapRequest
                    {
                        FactoryCode = AppSession.CurrentUser.FactoryName,
                        EmployeeId = AppSession.CurrentUser.EmployeeId,
                        TransferType = transferType,
                        ConsumptionType = consumptionType,
                        WorkOrderId = stockLog.WorkOrderId,
                        WorkOrderNo = stockLog.WorkOrderNo,
                        FromLocation = fromLocation,
                        ToLocation = toLocation,
                        MovementReason = stockLog.TransferReason,
                        Stocks = BuildStocksList(stockLog)
                    };
                }

                // 统一调用执行逻辑
                await ExecuteSapTransferAsync(request, stockLog);
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this.ParentForm, $"操作失败：{ex.Message}");
            }
        }

        // 辅助构建 Stocks 列表
        private List<TransferStock> BuildStocksList(RawLinesideStockLogDto stockLog)
        {
            return new List<TransferStock>
            {
                new TransferStock
                {
                    StockId = stockLog.RawLinesideStockId,
                    StockLogId = stockLog.Id,
                    MaterialCode = stockLog.MaterialCode,
                    BatchCode = stockLog.BatchCode,
                    Quantity = stockLog.ChangeQuantity,
                    BaseUnit = stockLog.BaseUnit
                }
            };
        }

        /// <summary>
        /// 核心执行方法：负责调用API、更新数据库状态、触发打印
        /// </summary>
        private async Task ExecuteSapTransferAsync(TransferSapRequest request, RawLinesideStockLogDto stockLog)
        {
            var requestUrl = _apiSettings["MesApi"].Endpoints["LineStockTransferToSAP"];
            var result = await _mesApiClient.PostAsync<object, object>(requestUrl, request);

            if (result.IsSuccess)
            {
                // 1. 更新成功状态
                await _rawLinesideStockLogService.UpdateAsync(new RawLinesideStockLogUpdateDto { Id = stockLog.Id, SapStatus = "1" });
                AntdUI.Message.success(this.ParentForm, "重推成功！");

                // 2. 刷新表格
                RefreshRowStatus(stockLog.Id);

                // 3. 外发类型触发打印
                if (stockLog.OperationType == StockOperationType.ShipmentOfRawMaterials)
                {
                    await HandlePrintingAsync(stockLog);
                }
            }
            else
            {
                // 失败处理
                await _rawLinesideStockLogService.UpdateAsync(new RawLinesideStockLogUpdateDto { Id = stockLog.Id, SapStatus = "-1" });
                throw new Exception($"SAP接口返回错误: {result.Message}");
            }
        }

        private async Task HandlePrintingAsync(RawLinesideStockLogDto stockLog)
        {
            string printerName;
            using (var form = new PrinterSelectForm())
            {
                if (form.ShowDialog() != DialogResult.OK)
                    return;
                printerName = form.SelectedPrinterName;
            }

            if (string.IsNullOrWhiteSpace(printerName))
            {
                AntdUI.Message.warn(this, "未选择打印机，跳过打印。");
                return;
            }

            var originalStock = await _rawLinesideStockService.GetByIdAsync((int)stockLog.RawLinesideStockId);

            var isNetwork = printerName.Contains(PrinterType.NetworkPrinter.GetDescription());
            var printerType = isNetwork ? PrinterType.NetworkPrinter : PrinterType.LocalPrinter;
            var cleanPrinterName = printerName.Replace("-" + printerType.GetDescription(), string.Empty);

            bool printSuccess = await Task.Run(() => LabelPrintHelper.ShipmentOfRawMaterialPrinter(
                printerType,
                cleanPrinterName,
                stockLog.MaterialCode,
                originalStock?.MaterialDesc ?? "",
                stockLog.BatchCode,
                stockLog.LocationCode,
                stockLog.ChangeQuantity
            ));

            if (!printSuccess)
            {
                AntdUI.Message.error(this, "打印指令发送失败，请检查打印机状态。");
            }
        }

        private void RefreshRowStatus(int logId)
        {
            var list = transferTable.DataSource as List<InventoryTransferLogViewModel>;
            var item = list?.FirstOrDefault(x => x.Id == logId);
            if (item != null)
            {
                item.SapStatus = "1";
                item.Btns[0].Enabled = false;
                transferTable.Refresh();
            }
        }

        #endregion

        #region Helpers

        private (MaterialTransferType, ConsumptionType, bool) GetTransferConfig(StockOperationType opType)
        {
            return opType switch
            {
                StockOperationType.StockLoss => (MaterialTransferType.RawMaterialScrapAgainstOrder, ConsumptionType.Scrap, false),
                StockOperationType.StockGain => (MaterialTransferType.RawMaterialScrapAgainstOrder, ConsumptionType.ScrapReversal, false),
                StockOperationType.ShipmentReturn => (MaterialTransferType.GoodsIssue, ConsumptionType.MaterialTransfer, true),
                StockOperationType.ShipmentOfRawMaterials => (MaterialTransferType.GoodsIssue, ConsumptionType.MaterialTransfer, true),
                StockOperationType.AdjustStockGain => (MaterialTransferType.RawMaterialScrapAgainstOrder, ConsumptionType.AdjustStockGain, false),
                StockOperationType.AdjustStockLoss => (MaterialTransferType.RawMaterialScrapAgainstOrder, ConsumptionType.AdjustStockLoss, false),
                StockOperationType.ProductLineStockTransfer => (MaterialTransferType.RawMaterialScrapAgainstOrder, ConsumptionType.Consumption, false),
                StockOperationType.GoodsIssuetoCostCenter => (MaterialTransferType.GoodsIssuetoCostCenter, ConsumptionType.GoodsIssuetoCostCenter, false),
                StockOperationType.ReversalofGoodsIssuetoCostCenter => (MaterialTransferType.ReversalofGoodsIssuetoCostCenter, ConsumptionType.ReversalofGoodsIssuetoCostCenter, false),
                _ => (MaterialTransferType.GoodsIssue, ConsumptionType.MaterialTransfer, false)
            };
        }

        private string GetParameterValue(ParameterGroupDto group, string key, string defaultValue)
        {
            return group?.Items?.FirstOrDefault(x => x.Key == key)?.Value ?? defaultValue;
        }

        #endregion
    }

    public class InventoryTransferLogViewModel
    {
        public int Id
        {
            get; set;
        }
        public int? TransferId
        {
            get; set;
        }
        public string OperationType
        {
            get; set;
        }
        public StockOperationType OperationTypeEnum
        {
            get; set;
        } // 保留枚举用于逻辑判断
        public string SapStatus
        {
            get; set;
        } // "1", "0", "-1"
        public string MaterialCode
        {
            get; set;
        }
        public decimal ChangeQuantity
        {
            get; set;
        }
        public decimal QuantityBefore
        {
            get; set;
        }
        public decimal QuantityAfter
        {
            get; set;
        }
        public string BatchCode
        {
            get; set;
        }
        public string BarCode
        {
            get; set;
        }
        public string LocationCode
        {
            get; set;
        }
        public string CreateBy
        {
            get; set;
        }
        public string CreateOn
        {
            get; set;
        }
        public string SapMessageType
        {
            get; set;
        }
        public string SapMessage
        {
            get; set;
        }
        public AntdUI.CellLink[] Btns
        {
            get; set;
        }
    }

}
