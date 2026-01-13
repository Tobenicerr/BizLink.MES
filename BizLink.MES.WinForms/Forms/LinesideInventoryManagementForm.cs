using AntdUI;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.Shared.Helpers;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Infrastructure;
using Dm;
using Dm.util;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.Logging;
using Newtonsoft.Json;
using SqlSugar.DistributedSystem.Snowflake;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace BizLink.MES.WinForms.Forms
{
    // 1. 继承 MesBaseForm，获得 RunAsync 能力
    public partial class LinesideInventoryManagementForm : MesBaseForm
    {
        private const string ParamGroup_SapLocation = "CN11SAPStockLocation";
        private const string Key_CableRawLineStock = "CableRawLineStock";
        private const string Key_SAPRawMtrStock = "SAPRawMtrStock";
        private readonly InventoryModuleFacade _facade;
        private readonly IFormFactory _formFactory;

        // 2. 构造函数：只注入 Facade 和 Factory
        public LinesideInventoryManagementForm(
            InventoryModuleFacade facade,
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

            keyboardInput.PlaceholderText = "请输入物料号或批次...";
            locationSelectMultiple.PlaceholderText = "请选择库位...";

            // 使用 RunAsync 处理加载异常
            await RunAsync(async () =>
            {
                if (AppSession.CurrentFactoryId <= 0)
                {
                    throw new Exception("请先选择工厂！");
                }
                await LoadLocationAsync();
            });
        }

        #region Core Functions

        // --- 查询 ---
        private async void queryButton_Click(object sender, EventArgs e)
        {
            await RunAsync(queryButton, async () =>
            {
                await LoadInventoryAsync();
            });
        }

        private async Task LoadInventoryAsync()
        {
            // 1. 获取全量数据
            var stockList = await _facade.RawStock.GetAllAsync(AppSession.CurrentFactoryId);

            // 2. 内存过滤
            if (!string.IsNullOrWhiteSpace(keyboardInput.Text.Trim()))
            {
                var key = keyboardInput.Text.Trim();
                stockList = stockList.Where(x => x.MaterialCode.Contains(key) || x.BatchCode.Contains(key)).ToList();
            }

            var selectedLocations = locationSelectMultiple.SelectedValue.Select(obj => (MenuItem)obj).ToList();
            if (selectedLocations.Any())
            {
                var locIds = selectedLocations.Select(x => Convert.ToInt32(x.Name)).ToList();
                stockList = stockList.Where(x => locIds.Contains(x.LocationId ?? 0)).ToList();
            }

            if (!quantitySwitch.Checked)
            {
                stockList = stockList.Where(x => x.LastQuantity <= 0).ToList();
            }
            else
            {
                stockList = stockList.Where(x => x.LastQuantity > 0).ToList();
            }

            // 3. 聚合显示 (Master View)
            var stockSummary = stockList
                .GroupBy(x => new { x.MaterialId, x.MaterialCode, x.BaseUnit })
                .Select(g => new RawLinesideStockDto
                {
                    MaterialId = g.Key.MaterialId,
                    MaterialCode = g.Key.MaterialCode,
                    MaterialDesc = g.First().MaterialDesc,
                    BaseUnit = g.Key.BaseUnit,
                    Quantity = g.Sum(x => x.LastQuantity)
                })
                .Select(x => new LinesideInventoryView(x))
                .ToList();

            stockTable.DataSource = stockSummary;
            AntdUI.Message.success(this, $"查询成功：共查询出{stockSummary.Count}笔物料库存！");
        }

        private async Task LoadLocationAsync()
        {
            var stocks = await _facade.RawStock.GetAllAsync(AppSession.CurrentFactoryId);
            var locations = stocks
                .GroupBy(x => new { x.LocationId, x.LocationCode, x.LocationDesc })
                .Select(x => x.Key)
                .Select(s => new MenuItem
                {
                    Name = s.LocationId.ToString(),
                    Text = s.LocationDesc
                }).ToArray();

            locationSelectMultiple.Items.AddRange(locations);
        }

        // --- 展开详情 ---
        private async void stockTable_ExpandChanged(object sender, TableExpandEventArgs e)
        {
            if (e.Expand && e.Record is LinesideInventoryView data)
            {
                try
                {
                    var details = await _facade.RawStock.GetListByMaterialCodeAsync(AppSession.CurrentFactoryId, data.MaterialCode);

                    if (quantitySwitch.Checked)
                        details = details.Where(x => x.LastQuantity > 0).ToList();
                    else
                        details = details.Where(x => x.LastQuantity <= 0).ToList();

                    data.Children = details.OrderByDescending(x => x.BatchCode)
                        .Select(x => new LinesideInventoryDetailView(x)).ToArray();
                }
                catch (Exception ex)
                {
                    AntdUI.Message.error(this, "加载详情失败: " + ex.Message);
                }
            }
        }

        // --- 操作按钮核心逻辑 ---
        private async void stockTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            // A. 主表操作：补料
            if (e.Record is LinesideInventoryView masterData)
            {
                if (e.Btn.Id == "replenish")
                {
                    var dto = new RawLinesideStockDto
                    {
                        MaterialId = (int)masterData.MaterialId,
                        MaterialCode = masterData.MaterialCode,
                        Quantity = masterData.Quantity
                    };

                    // 使用 Factory 打开子窗体
                    _formFactory.Show<InventoryReplenishForm>(form =>
                    {
                        form.InitData(dto);
                        form.OnSuccess += () =>
                        {
                            AntdUI.Message.success(this, "补料申请提交成功！");
                            // 补料通常不影响当前线边库数量显示，所以这里不强制刷新，或者根据需求刷新
                        };
                    }, isModal: true);
                }
            }
            // B. 子表操作：明细管理
            else if (e.Record is LinesideInventoryDetailView detail)
            {
                var stock = await _facade.RawStock.GetByIdAsync((int)detail.Id);
                if (stock == null)
                    throw new Exception("未找到库存记录！");

                //var parameterGroup = await _facade.Params.GetGroupWithItemsAsync("CN11SAPStockLocation");
                //var requestUrl = _facade.ApiSettings["MesApi"].Endpoints["LineStockTransferToSAP"];

                //var request = new TransferSapRequest
                //{
                //    FactoryCode = AppSession.CurrentUser.FactoryName,
                //    EmployeeId = AppSession.CurrentUser.EmployeeId,
                //    TransferType = MaterialTransferType.GoodsIssue,
                //    ConsumptionType = ConsumptionType.MaterialTransfer,
                //    Stocks = new List<TransferStock>
                //    {
                //        new TransferStock
                //        {
                //            StockId = stock.Id,
                //            MaterialCode = stock.MaterialCode,
                //            BatchCode = stock.BatchCode,
                //            Quantity = (decimal)stock.LastQuantity,
                //            BaseUnit = stock.BaseUnit
                //        }
                //    },
                //    FromLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == "SAPRawMtrStock")?.Value ?? "1100",
                //    ToLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == "CableRawLineStock")?.Value ?? "2200"
                //};
                //var json = JsonConvert.SerializeObject(request);
                // 1. 同步 SAP
                if (stock.SapStatus != "2")
                {
                    AntdUI.Message.error(this, $"标签{detail.BarCode}未收货确认，请先在PDA进行收货确认！");
                    return;
                    //var requestUrl = _facade.ApiSettings["MesApi"].Endpoints["LineStockTransferToSAP"];
                    //var parameterGroup = await _facade.Params.GetGroupWithItemsAsync("CN11SAPStockLocation");
                    //string cableLineLocation = parameterGroup?.Items.FirstOrDefault(x => x.Key == "CableRawLineStock")?.Value ?? "2200";
                    //var sapTransderLogs = await _facade.SapTransferLog.GetListByStockIdAsync(stock.Id);

                    ////判断当前ID是否有SAP移库，如无则进行SAP移库
                    //if (sapTransderLogs == null || !sapTransderLogs.Where(x => x.MaterialCode == stock.MaterialCode && x.BatchCode == stock.BatchCode && x.ToLocationCode == cableLineLocation).Any())
                    //{
                    //    var request = new TransferSapRequest
                    //    {
                    //        FactoryCode = AppSession.CurrentUser.FactoryName,
                    //        EmployeeId = AppSession.CurrentUser.EmployeeId,
                    //        TransferType = MaterialTransferType.GoodsIssue,
                    //        ConsumptionType = ConsumptionType.MaterialTransfer,
                    //        Stocks = new List<TransferStock>
                    //{
                    //    new TransferStock
                    //    {
                    //        StockId = stock.Id,
                    //        MaterialCode = stock.MaterialCode,
                    //        BatchCode = stock.BatchCode,
                    //        Quantity = (decimal)stock.LastQuantity,
                    //        BaseUnit = stock.BaseUnit
                    //    }
                    //},
                    //        FromLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == "SAPRawMtrStock")?.Value ?? "1100",
                    //        ToLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == "CableRawLineStock")?.Value ?? "2200"
                    //    };

                    //    var result = await _facade.MesApi.PostAsync<object, object>(requestUrl, request);
                    //    if (result != null && result.IsSuccess)
                    //    {
                    //        //await _facade.RawStock.UpdateAsync(new RawLinesideStockUpdateDto { Id = stock.Id, SapStatus = "2" });
                    //    }
                    //}



                }

                // 1. 调整
                if (e.Btn.Id == "adjustment")
                {
                    if (detail.Status == "2") // 2 = 使用中
                    {
                        AntdUI.Message.error(this, $"标签{detail.BarCode}正在使用中，无法操作！");
                        return;
                    }

                    _formFactory.Show<InventoryAdjustForm>(form =>
                    {
                        form.InitData(stockId: (int)detail.Id);
                        form.OnSuccess += async () =>
                        {
                            AntdUI.Message.success(this, "库存调整成功！");
                            await LoadInventoryAsync(); // 刷新列表
                        };
                    }, isModal: true);
                }
                // 2. 转移
                else if (e.Btn.Id == "transfer")
                {
                    _formFactory.Show<InventoryTransferForm>(form =>
                    {
                        form.InitData((int)detail.Id);
                        form.OnSuccess += async () =>
                        {
                            AntdUI.Message.success(this, "库存转移成功！");
                            await LoadInventoryAsync(); // 刷新列表
                        };
                    }, isModal: true);
                }
                // 3. 退库
                else if (e.Btn.Id == "return")
                {
                    // 使用 RunAsync 处理确认和执行
                    await RunAsync(async () =>
                    {
                        await ExecuteReturnAsync(detail);
                    }, confirmMsg: "即将将选中的物料标签进行退库，是否继续？");
                }
                // 4. 打印
                else if (e.Btn.Id == "labelprint")
                {
                    _formFactory.Show<PrinterSelectForm>(form =>
                    {
                        // 监听窗体关闭事件来获取结果
                        form.FormClosed += async (s, args) =>
                        {
                            if (form.DialogResult == DialogResult.OK && !string.IsNullOrEmpty(form.SelectedPrinterName))
                            {
                                // 执行打印
                                await RunAsync(async () =>
                                {
                                    await ExecutePrintAsync(detail, form.SelectedPrinterName);
                                });
                            }
                        };
                    }, isModal: true);
                }
            }
        }

        // --- 业务逻辑：退库 ---
        private async Task ExecuteReturnAsync(LinesideInventoryDetailView detail)
        {
            var stock = await _facade.RawStock.GetByIdAsync((int)detail.Id);
            if (stock == null)
                throw new Exception("未找到库存记录！");

            // 1. 同步 SAP
            //if (stock.SapStatus != "2")
            //{
            //    var parameterGroup = await _facade.Params.GetGroupWithItemsAsync("CN11SAPStockLocation");
            //    var requestUrl = _facade.ApiSettings["MesApi"].Endpoints["LineStockTransferToSAP"];

            //    var request = new TransferSapRequest
            //    {
            //        FactoryCode = AppSession.CurrentUser.FactoryName,
            //        EmployeeId = AppSession.CurrentUser.EmployeeId,
            //        TransferType = MaterialTransferType.GoodsIssue,
            //        ConsumptionType = ConsumptionType.MaterialTransfer,
            //        Stocks = new List<TransferStock>
            //        {
            //            new TransferStock
            //            {
            //                StockId = stock.Id,
            //                MaterialCode = stock.MaterialCode,
            //                BatchCode = stock.BatchCode,
            //                Quantity = (decimal)stock.LastQuantity,
            //                BaseUnit = stock.BaseUnit
            //            }
            //        },
            //        FromLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == "SAPRawMtrStock")?.Value ?? "1100",
            //        ToLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == "CableRawLineStock")?.Value ?? "2200"
            //    };

            //    var result = await _facade.MesApi.PostAsync<object, object>(requestUrl, request);
            //    if (result != null && result.IsSuccess)
            //    {
            //        await _facade.RawStock.UpdateAsync(new RawLinesideStockUpdateDto { Id = stock.Id, SapStatus = "2" });
            //    }
            //}

            var sapLocationParams = await _facade.Params.GetGroupWithItemsAsync(ParamGroup_SapLocation);
            string GetLocation(string key, string defaultVal) => sapLocationParams.Items.FirstOrDefault(x => x.Key == key)?.Value ?? defaultVal;
            string fromLoc = GetLocation(Key_CableRawLineStock, "2200");
            string toLoc = GetLocation(Key_SAPRawMtrStock, "1100");
            // 2. 调用 JY 系统 (WMS)
            var jyUrl = _facade.ApiSettings["JyApi"].Endpoints["WMSTaskCreate"];
            var jyRequest = new
            {
                jpd = new
                {
                    PLANT = AppSession.CurrentUser.FactoryName,
                    BILLCODE = _facade.SerialHelperService.GenerateNext("WmsReturnSerialNo"),
                    BILLTYPE = "生产退料",
                    FSYSTEM = "MES",
                    FWHERE = fromLoc,
                    BSN = 1,
                    STOCKCODE = toLoc,
                    STOCKNAME = string.Empty,
                    STOCKCODE2 = string.Empty,
                    STOCKNAME2 = string.Empty,
                    FROMBILLID = string.Empty,

                    Fuser = AppSession.CurrentUser.EmployeeId,
                    SupplierNo =string.Empty,
                    ExtJson = string.Empty,
                    ASLINE = string.Empty,
                    SENDTIME = string.Empty,
                    FORCENUM = "1",
                    items = new List<object>
                    {
                        new {
                            MATERIALNO = detail.MaterialCode,
                            MATERIALNAME= detail.MaterialDesc,
                            MaterialStd = string.Empty,
                            WERKS = AppSession.CurrentUser.FactoryName,
                            BATNO = detail.BatchCode,
                            QTY = detail.Quantity,
                            DW = detail.BaseUnit,
                            CostCenterNo = string.Empty,
                            CostCenterName = string.Empty,
                            STOCKLOCATIONNAME = string.Empty,
                            StockAreaCode = string.Empty,
                            FROMITEMID = string.Empty,
                            sns = new List<object> { new { BARCODE = detail.BarCode, TypeJP = "SJ", Qty = detail.Quantity } }
                        }
                    }
                }
            };

            var jyJson = JsonConvert.SerializeObject(jyRequest);
            var jyResult = await _facade.JyApi.PostAsync<object>(jyUrl, jyJson);

            if (!jyResult.IsSuccess && !string.IsNullOrWhiteSpace(jyResult.Message))
            {
                var message = jyResult.Message.Contains(" at") ? jyResult.Message.Split(" at")[0] : jyResult.Message;
                AntdUI.Message.error(this, $"WMS退回提交失败: {message}");
            }

            //移库sap

           

            // 3. 更新本地库存
            await _facade.RawStock.UpdateAsync(new RawLinesideStockUpdateDto
            {
                Id = (int)detail.Id,
                LastQuantity = 0,
                UpdateBy = AppSession.CurrentUser.EmployeeId,
                UpdatedAt = DateTime.Now
            });

            // 4. 记录日志
            var stocklog = await _facade.StockLog.CreateAsync(new RawLinesideStockLogCreateDto
            {
                RawLinesideStockId = detail.Id,
                OperationType = StockOperationType.Return,
                InOutStatus = InOutStatus.Out,
                ChangeQuantity = (decimal)detail.Quantity,
                QuantityBefore = (decimal)detail.Quantity,
                QuantityAfter = 0,
                MaterialCode = detail.MaterialCode,
                BarCode = detail.BarCode,
                BatchCode = detail.BatchCode,
                LocationId = (int)detail.LocationId,
                LocationCode = detail.LocationCode,
                BaseUnit = detail.BaseUnit,
                CreateBy = AppSession.CurrentUser.EmployeeId,
                Remark = StockOperationType.Return.GetDescription()
            });




            var requestUrl = _facade.ApiSettings["MesApi"].Endpoints["LineStockTransferToSAP"];
            var request = new TransferSapRequest
            {
                FactoryCode = AppSession.CurrentUser.FactoryName,
                EmployeeId = AppSession.CurrentUser.EmployeeId,
                TransferType = MaterialTransferType.GoodsIssue,
                ConsumptionType = ConsumptionType.MaterialTransfer,
                Stocks = new List<TransferStock> {
                    new TransferStock {
                        StockId = stock.Id,
                        StockLogId = stocklog.Id,
                        MaterialCode = stock.MaterialCode,
                        BatchCode = stock.BatchCode,
                        Quantity = (decimal)stock.LastQuantity,
                        BaseUnit = stock.BaseUnit
                    }
                },
                FromLocation = fromLoc,
                ToLocation = toLoc,
            };
            var json = JsonConvert.SerializeObject(request);
            var result = await _facade.MesApi.PostAsync<object, object>(requestUrl, request);
            if (result != null && result.IsSuccess)
            {
                AntdUI.Modal.open(this, "SAP移库结束", string.Join("\r\n", result.Message.replace("：", "\r\n").split(";").Distinct().Take(10)));
            }
            AntdUI.Message.success(this, "材料退回提交成功！");
            await LoadInventoryAsync(); // 刷新列表
        }

        // --- 业务逻辑：打印 ---
        private async Task ExecutePrintAsync(LinesideInventoryDetailView detail, string printerName)
        {
            if (string.IsNullOrWhiteSpace(printerName))
                return;

            var parts = printerName.Split('-');
            var printType = (parts.Length > 1 && parts[1] == PrinterType.NetworkPrinter.GetDescription())
                ? PrinterType.NetworkPrinter : PrinterType.LocalPrinter;
            var actualPrinterName = parts[0];

            // 重新获取最新库存数据 (以防界面数据滞后)
            var stock = await _facade.RawStock.GetByIdAsync((int)detail.Id);
            if (stock == null)
                throw new Exception("库存记录已不存在！");

            bool printRtn = LabelPrintHelper.ShipmentOfRawMaterialPrinter(
                printType,
                actualPrinterName,
                stock.MaterialCode,
                stock.MaterialDesc,
                stock.BatchCode,
                stock.LocationCode,
                (decimal)stock.LastQuantity,
                stock.BarCode
            );

            if (printRtn)
            {
                AntdUI.Message.success(this, "打印指令发送成功！");
            }
            else
            {
                throw new Exception("打印指令发送失败，请检查打印机设置！");
            }
        }

        // --- 核心功能：新增库存 ---
        private void stockCreateButton_Click(object sender, EventArgs e)
        {
            _formFactory.Show<InventoryAddForm>(form =>
            {
                form.OnSuccess += async () =>
                {
                    AntdUI.Message.success(this, "库存新增成功！");
                    await LoadInventoryAsync();
                };
            }, isModal: true);
        }

        // --- 核心功能：导出 ---
        private async void exportButton_Click(object sender, EventArgs e)
        {
            await RunAsync(exportButton, async () =>
            {
                var data = (await _facade.RawStock.GetAllAsync(AppSession.CurrentFactoryId))
                    .Where(x => x.LastQuantity > 0 && x.BaseUnit.ToUpper() == "M")
                    .Select(x => new
                    {
                        x.MaterialCode,
                        x.MaterialDesc,
                        x.BatchCode,
                        x.BarCode,
                        x.BaseUnit,
                        x.LastQuantity,
                        x.LocationCode,
                        x.LocationDesc,
                        SapStatus = x.SapStatus == "2" ? "已同步" : "未同步"
                    });

                ExcelExportHelper.ExportToExcel(this.ParentForm, data, $"断线线边库存{DateTime.Now:yyyyMMdd}");
            }, confirmMsg: "是否导出线边库中的库存信息？");
        }

        #endregion

        #region UI Helpers
        private void InitializeTable()
        {
            stockTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Left) { KeyTree = "Children" }.SetWidth("auto").SetFixed(),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetWidth("auto"),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        if (value is string s)
                        {
                            return s == "1" ? new CellTag("正常", TTypeMini.Success) :
                                   s == "2" ? new CellTag("使用中", TTypeMini.Error) :
                                   new CellTag("未知", TTypeMini.Default);
                        }
                        return null;
                    }
                }.SetWidth("auto"),
                new AntdUI.Column("BatchCode", "批次", AntdUI.ColumnAlign.Center).SetWidth("auto"),
                new AntdUI.Column("BarCode", "标签", AntdUI.ColumnAlign.Center).SetWidth("auto"),
                new AntdUI.Column("Quantity", "数量", AntdUI.ColumnAlign.Center).SetDisplayFormat("0.###"),
                new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("Location", "库位", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("SapStatus", "SAP同步状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        if (value is string s)
                        {
                            return s == "1" ? new CellTag("未同步", TTypeMini.Error) :
                                   s == "2" ? new CellTag("已同步", TTypeMini.Success) :
                                   new CellTag("未知", TTypeMini.Error);
                        }
                        return null;
                    }
                }.SetFixed().SetWidth("auto"),
                new AntdUI.Column("btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto"),
            };
        }

        private AntdUI.Table.CellStyleInfo stockTable_SetRowStyle(object sender, TableSetRowStyleEventArgs e)
        {
            if (e.Record is LinesideInventoryView)
            {
                return new AntdUI.Table.CellStyleInfo { BackColor = System.Drawing.Color.WhiteSmoke };
            }
            else if (e.Record is LinesideInventoryDetailView detail && !string.IsNullOrWhiteSpace(keyboardInput.Text) && detail.BatchCode.Contains(keyboardInput.Text.Trim()))
            {
                return new AntdUI.Table.CellStyleInfo { BackColor = System.Drawing.Color.Red };
            }
            return null;
        }
        #endregion

        private async void sapSyncButton_Click(object sender, EventArgs e)
        {
            await RunAsync(sapSyncButton, async () =>
            {
                var data = (await _facade.RawStock.GetAllAsync(AppSession.CurrentFactoryId))
                    .Where(x => x.LastQuantity > 0 && x.BaseUnit.ToUpper() == "M" && x.SapStatus == "1").Take(5).Select(stock => new TransferStock() 
                    {
                        StockId = stock.Id,
                        MaterialCode = stock.MaterialCode,
                        BatchCode = stock.BatchCode,
                        Quantity = (decimal)stock.LastQuantity,
                        BaseUnit = stock.BaseUnit
                    }).ToList();

                if (!data.Any())
                    throw new Exception("没有需要同步到SAP的库存记录！");


                var sapLocationParams = await _facade.Params.GetGroupWithItemsAsync(ParamGroup_SapLocation);

                string GetLocation(string key, string defaultVal) => sapLocationParams.Items.FirstOrDefault(x => x.Key == key)?.Value ?? defaultVal;
                string toLoc = GetLocation(Key_CableRawLineStock, "2200");
                string fromLoc = GetLocation(Key_SAPRawMtrStock, "1100");
                var requestUrl = _facade.ApiSettings["MesApi"].Endpoints["LineStockTransferToSAP"];

                var request = new TransferSapRequest
                {
                    FactoryCode = AppSession.CurrentUser.FactoryName,
                    EmployeeId = AppSession.CurrentUser.EmployeeId,
                    TransferType = MaterialTransferType.GoodsIssue,
                    ConsumptionType = ConsumptionType.MaterialTransfer,
                    Stocks = data,
                    FromLocation = fromLoc,
                    ToLocation = toLoc
                };
                var json = JsonConvert.SerializeObject(request);
                var result = await _facade.MesApi.PostAsync<object, object>(requestUrl, request);
                if (result != null && result.IsSuccess)
                {
                    AntdUI.Modal.open(this.ParentForm, "同步结束", string.Join("\r\n", result.Message.replace("：","\r\n").split(";").Distinct().Take(10)));
                }


            }, confirmMsg: "是否批量同步线边库存至SAP？");
        }
    }

    public class LinesideInventoryView : AntdUI.NotifyProperty
    {
        public LinesideInventoryView(RawLinesideStockDto dto)
        {
            _materialId = dto.MaterialId;
            _materialCode = dto.MaterialCode;
            _materialDesc = dto.MaterialDesc;
            _baseUnit = dto.BaseUnit;
            _quantity = dto.Quantity;
            _children = new LinesideInventoryDetailView[1];

            // 初始化按钮
            _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("replenish", "补料", AntdUI.TTypeMini.Primary),
                    };
        }

        int? _materialId;
        public int? MaterialId
        {
            get => _materialId;
            set
            {
                if (_materialId == value)
                    return;
                _materialId = value;
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

        string? _materialDesc;
        public string? MaterialDesc
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

        string? _baseUnit;
        public string? BaseUnit
        {
            get => _baseUnit;
            set
            {
                if (_baseUnit == value)
                    return;
                _baseUnit = value;
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

        LinesideInventoryDetailView[] _children;
        public LinesideInventoryDetailView[] Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }
    }

    public class LinesideInventoryDetailView : AntdUI.NotifyProperty
    {
        public LinesideInventoryDetailView(RawLinesideStockDto dto)
        {
            _id = dto.Id;
            _materialId = dto.MaterialId;
            _materialCode = dto.MaterialCode;
            _materialDesc = dto.MaterialDesc;
            _batchCode = dto.BatchCode;
            _barcode = dto.BarCode;
            _baseUnit = dto.BaseUnit;
            _quantity = dto.LastQuantity;
            _locationId = dto.LocationId;
            _locationCode = dto.LocationCode;
            _locationDesc = dto.LocationDesc;
            _location = $"{dto.LocationCode}/{dto.LocationDesc}";
            _status = dto.Status;
            _sapStatus = dto.SapStatus;

            // 初始化按钮
            _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("adjustment", "调整", AntdUI.TTypeMini.Default){ BorderWidth = 1},
                        new AntdUI.CellButton("transfer", "转移", AntdUI.TTypeMini.Success)
                        {
                            Enabled =  dto.LastQuantity > 0
                        },
                        new AntdUI.CellButton("return", "退回", AntdUI.TTypeMini.Error)
                        {
                            Enabled =  dto.LastQuantity > 0
                        },
                        new AntdUI.CellButton("labelprint", "打印", AntdUI.TTypeMini.Primary)
                        {
                            Enabled =  dto.LastQuantity > 0
                        },
                    };
        }

        int? _id;
        public int? Id
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

        int? _materialId;
        public int? MaterialId
        {
            get => _materialId;
            set
            {
                if (_materialId == value)
                    return;
                _materialId = value;
                OnPropertyChanged();
            }
        }


        int? _locationId;
        public int? LocationId
        {
            get => _locationId;
            set
            {
                if (_locationId == value)
                    return;
                _locationId = value;
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

        string? _locationCode;
        public string? LocationCode
        {
            get => _locationCode;
            set
            {
                if (_locationCode == value)
                    return;
                _locationCode = value;
                OnPropertyChanged();
            }
        }

        string? _locationDesc;
        public string? LocationDesc
        {
            get => _locationDesc;
            set
            {
                if (_locationDesc == value)
                    return;
                _locationDesc = value;
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

        string? _sapStatus;
        public string? SapStatus
        {
            get => _sapStatus;
            set
            {
                if (_sapStatus == value)
                    return;
                _sapStatus = value;
                OnPropertyChanged();
            }
        }

        string? _materialDesc;
        public string? MaterialDesc
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

        string? _baseUnit;
        public string? BaseUnit
        {
            get => _baseUnit;
            set
            {
                if (_baseUnit == value)
                    return;
                _baseUnit = value;
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

        string? _barcode;
        public string? BarCode
        {
            get => _barcode;
            set
            {
                if (_barcode == value)
                    return;
                _barcode = value;
                OnPropertyChanged();
            }
        }

        string? _location;
        public string? Location
        {
            get => _location;
            set
            {
                if (_location == value)
                    return;
                _location = value;
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
