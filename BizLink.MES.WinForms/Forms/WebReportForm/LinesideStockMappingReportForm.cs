using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms.WebReportForm
{
    public partial class LinesideStockMappingReportForm : MesBaseForm
    {
        private bool _isProgrammaticPageChange = false;
        private readonly IRawLinesideStockService _rawLinesideStockService;
        private readonly ISapRfcService _sapRfcService;
        private IFactoryService _factoryService;
        public LinesideStockMappingReportForm(IRawLinesideStockService rawLinesideStockService, ISapRfcService sapRfcService, IFactoryService factoryService)
        {
            InitializeComponent();
            InitializeTable();
            _rawLinesideStockService = rawLinesideStockService;
            _sapRfcService = sapRfcService;
            _factoryService = factoryService;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            KeywordInput.PlaceholderText = "请输入关键字...";
            MaterialCodesInput.PlaceholderText = "请输入物料编码...";
            BatchInput.PlaceholderText = "请输入批次号...";
        }
        private void InitializeTable()
        {
            TableControl.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center) {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "正常" => new AntdUI.CellTag("正常", AntdUI.TTypeMini.Success),
                            "异常" => new AntdUI.CellTag("异常", AntdUI.TTypeMini.Error),
                            _ => null
                        };
                    }
                }.SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BatchCode", "批次", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "发料数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LastQuantity", "剩余数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MesLocation", "Mes库位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("SapQuantity", "Sap库存", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("SapLocation", "Sap库位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Remark", "备注", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),


            };
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SearchButton, async () =>
            {
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
                var count = await LoadDataAsync();
                AntdUI.Message.success(this, $"查询成功：共查询出{count}笔记录");
            });
        }

        private async Task<int> LoadDataAsync()
        {
            try
            {
                SpinControl.Visible = true;
                TableControl.DataSource = null;
                var pageSize = PaginationControl.PageSize;
                var pageIndex = PaginationControl.Current;
                var keyword = KeywordInput.Text.Trim();
                var quantitySwitch = QuantitySwitch.Checked;
                var materialcodes = MaterialCodesInput.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var batchcodes = BatchInput.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();


                var result = await _rawLinesideStockService.GetBatchPageListAsync(pageIndex, pageSize, AppSession.CurrentFactoryId, keyword, quantitySwitch, materialcodes, batchcodes);
                if (result != null && result.TotalCount > 0)
                {
                    var factory = await _factoryService.GetByIdAsync(AppSession.CurrentFactoryId);
                    var materialCodes = result.Items.GroupBy(i => new { i.MaterialCode, i.BatchCode }).Select(i => new SapRawMaterialStockDto()
                    {
                        MaterialCode = i.Key.MaterialCode,
                        BatchCode = i.Key.BatchCode,
                        FactoryCode = factory?.FactoryCode,
                        LocationCode = "2200"
                    }).ToList();
                    var sapStocks = await _sapRfcService.GetRawMaterialStockFromSapAsync(materialCodes);

                    var data = result.Items.GroupJoin(
                        sapStocks,
                        agg => new { agg.MaterialCode, agg.BatchCode },
                        info => new { MaterialCode = info.MaterialCode.TrimStart('0'), info.BatchCode },

                        (agg, infoGroup) => new { AggregatedItem = agg, SapStock = infoGroup }
                    )
                    .SelectMany(
                        temp => temp.SapStock.DefaultIfEmpty(),
                        (temp, info) => new LinesideStockMappingView()
                        {
                            MaterialCode = temp.AggregatedItem.MaterialCode,
                            MaterialDesc = temp.AggregatedItem.MaterialDesc,
                            BaseUnit = temp.AggregatedItem.BaseUnit,
                            Status = temp.AggregatedItem.LastQuantity == info?.Quantity ? "正常" : "异常",
                            BatchCode = temp.AggregatedItem.BatchCode,
                            Quantity = temp.AggregatedItem.Quantity,
                            LastQuantity = temp.AggregatedItem.LastQuantity,
                            MesLocation = temp.AggregatedItem.LocationDesc,
                            SapQuantity = info?.Quantity,
                            SapLocation = info?.LocationCode,
                            Remark = info?.Remark
                        }
                    ).ToList();
                    TableControl.DataSource = data;
                    PaginationControl.Total = result.TotalCount;
                }
                else
                {
                    TableControl.DataSource = null;
                }

                return result.TotalCount;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                SpinControl.Visible = false;
            }
        }

        private async void PaginationControl_ValueChanged(object sender, PagePageEventArgs e)
        {
            if (_isProgrammaticPageChange)
            {
                // 如果是程序内部触发的页码变化，直接返回，不执行查询
                return;
            }
            await LoadDataAsync();
        }

        private async void ExportButton_Click(object sender, EventArgs e)
        {
            await RunAsync(ExportButton, async () =>
            {
                var data = TableControl.DataSource as List<LinesideStockMappingView>;
                if (data != null && data.Count() > 0)
                {
                    ExcelExportHelper.ExportToExcel(this.ParentForm, data, $"线边批次库存{DateTime.Now:yyyyMMdd}");
                }
                else
                    throw new Exception("当前无数据可导出，请先执行查询！");


            }, successMsg:"导出完成！",confirmMsg: "是否导出线边库中的批次库存信息？");
        }
    }

    public class LinesideStockMappingView
    {
        public string? MaterialCode
        {
            get;
            set;
        }

        public string? MaterialDesc
        {
            get;
            set;
        }
        public string? BaseUnit
        {
            get;
            set;
        }

        public string? BatchCode
        {
            get;
            set;
        }

        public string? Status
        {
            get;
            set;
        }

        public decimal? Quantity
        {
            get;
            set;
        }

        public decimal? LastQuantity
        {
            get;
            set;
        }

        public decimal? SapQuantity
        {
            get;
            set;
        }

        public string? SapLocation
        {
            get;
            set;
        }

        public string? MesLocation
        {
            get;
            set;
        }

        public string? Remark
        {
            get;
            set;
        }
    }
}
