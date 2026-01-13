using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
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
    public partial class WmsMaterialBatchReportForm : MesBaseForm
    {
        private bool _isProgrammaticPageChange = false;
        private IFactoryService _factoryService;
        private readonly IWmsMaterialStockService _wmsMaterialStockService;
        private readonly ISapRfcService _sapRfcService;

        public WmsMaterialBatchReportForm(IFactoryService factoryService, IWmsMaterialStockService wmsMaterialStockService, ISapRfcService sapRfcService)
        {
            InitializeComponent();
            InitializeTable();
            _factoryService = factoryService;
            _wmsMaterialStockService = wmsMaterialStockService;
            _sapRfcService = sapRfcService;
        }


        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            KeywordInput.PlaceholderText = "请输入关键字...";
            MaterialInput.PlaceholderText = "请输入物料编码...";
            BatchInput.PlaceholderText = "请输入批次号...";
            ConsumeTypeSelect.PlaceholderText = "请选择物料类型...";


            ConsumeTypeSelect.Items.Clear();
            ConsumeTypeSelect.Items.Add(new AntdUI.MenuItem() { Name = string.Empty, Text = "全部" });
            ConsumeTypeSelect.Items.AddRange(Enum.GetValues(typeof(ConsumeType)).Cast<ConsumeType>().Select(ct => new AntdUI.MenuItem()
            {
                Name = ((int)ct).ToString(),
                Text = ct.GetDescription()
            }).ToArray());
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

                new AntdUI.Column("ConsumeType", "物料属性", AntdUI.ColumnAlign.Center) {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "电缆" => new AntdUI.CellTag("电缆", AntdUI.TTypeMini.Success),
                            "按单领料" => new AntdUI.CellTag("按单领料", AntdUI.TTypeMini.Primary),
                            "一次性领料" => new AntdUI.CellTag("一次性领料", AntdUI.TTypeMini.Default),
                            "未知" => new AntdUI.CellTag("未知", AntdUI.TTypeMini.Error),
                            _ => null

                        };
                    }
                }.SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BatchCode", "批次", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("UsageQuantity", "可用数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("DownQuantity", "待出库数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LockQuantity", "冻结数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("SapQuantity", "Sap库存", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("LocationCode", "库位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
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
                var materialcodes = MaterialInput.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var batchcodes = BatchInput.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var consumetype = ((MenuItem)ConsumeTypeSelect.SelectedValue)?.Name;
                var factory = await _factoryService.GetByIdAsync(AppSession.CurrentFactoryId);
                var result = await _wmsMaterialStockService.GetBatchPageListAsync(pageIndex, pageSize, factory.FactoryCode, keyword, materialcodes, batchcodes, consumetype);
                if (result != null && result.TotalCount > 0)
                {
                    var materialCodes = result.Items.GroupBy(i => new { i.MaterialCode, i.BatchCode }).Select(i => new SapRawMaterialStockDto()
                    {
                        MaterialCode = i.Key.MaterialCode,
                        BatchCode = i.Key.BatchCode,
                        FactoryCode = factory?.FactoryCode,
                        LocationCode = "1100"
                    }).ToList();
                    var sapStocks = await _sapRfcService.GetRawMaterialStockFromSapAsync(materialCodes);

                    var data = result.Items.GroupJoin(
                        sapStocks,
                        agg => new { agg.MaterialCode, BatchCode = agg.BatchCode ?? string.Empty },
                        info => new { MaterialCode = info.MaterialCode.TrimStart('0'), BatchCode = info.BatchCode ?? string.Empty },

                        (agg, infoGroup) => new { AggregatedItem = agg, SapStock = infoGroup }
                    )
                    .SelectMany(
                        temp => temp.SapStock.DefaultIfEmpty(),
                        (temp, info) => new WmsMaterialBatchView()
                        {
                            MaterialCode = temp.AggregatedItem.MaterialCode,
                            MaterialDesc = temp.AggregatedItem.MaterialDesc,
                            BaseUnit = temp.AggregatedItem.BaseUnit,
                            ConsumeType = Enum.IsDefined(typeof(ConsumeType), temp.AggregatedItem.ConsumeType ?? -1) ? ((ConsumeType)
                            temp.AggregatedItem.ConsumeType).GetDescription() : "未知",
                            Status = temp.AggregatedItem.UsageQuantity+ temp.AggregatedItem.DownQuantity == info?.Quantity ? "正常" : "异常",
                            BatchCode = temp.AggregatedItem.BatchCode,
                            UsageQuantity = temp.AggregatedItem.UsageQuantity,
                            DownQuantity = temp.AggregatedItem.DownQuantity,
                            LockQuantity = temp.AggregatedItem.LockQuantity,
                            SapQuantity = info?.Quantity,
                            LocationCode = info?.LocationCode,
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
                var data = TableControl.DataSource as List<WmsMaterialBatchView>;
                if (data != null && data.Count() > 0)
                {
                    ExcelExportHelper.ExportToExcel(this.ParentForm, data, $"Wms批次库存{DateTime.Now:yyyyMMdd}");
                }
                else
                    throw new Exception("当前无数据可导出，请先执行查询！");


            }, successMsg: "导出完成！", confirmMsg: "是否导出原材料库中的批次库存信息？");
        }

        private async void ExportMatButton_Click(object sender, EventArgs e)
        {
            await RunAsync(ExportMatButton, async () =>
            {
                var data = TableControl.DataSource as List<WmsMaterialBatchView>;
                if (data != null && data.Count() > 0)
                {
                    var exportData = data.GroupBy(d => d.MaterialCode).Select(g => new WmsMaterialBatchView()
                    {
                        MaterialCode = g.Key,
                        MaterialDesc = g.First().MaterialDesc,
                        BaseUnit = g.First().BaseUnit,
                        ConsumeType = g.First().ConsumeType,
                        UsageQuantity = g.Sum(i => i.UsageQuantity) ?? 0,
                        SapQuantity = g.Sum(i => i.SapQuantity) ?? 0,
                        Status = (g.Sum(i => i.UsageQuantity) ?? 0) == (g.Sum(i => i.SapQuantity) ?? 0) ? "正常" : "异常"
                    }).ToList();

                    ExcelExportHelper.ExportToExcel(this.ParentForm, exportData, $"Wms物料库存{DateTime.Now:yyyyMMdd}");
                }
                else
                    throw new Exception("当前无数据可导出，请先执行查询！");


            }, successMsg: "导出完成！", confirmMsg: "是否导出原材料库中的物料库存信息？");
        }
    }

    public class WmsMaterialBatchView
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

        public decimal? UsageQuantity
        {
            get;
            set;
        }

        public decimal? DownQuantity
        {
            get;
            set;
        }

        public decimal? LockQuantity
        {
            get;
            set;
        }

        public decimal? SapQuantity
        {
            get;
            set;
        }

        public string? LocationCode
        {
            get;
            set;
        }

        public string? ConsumeType
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
