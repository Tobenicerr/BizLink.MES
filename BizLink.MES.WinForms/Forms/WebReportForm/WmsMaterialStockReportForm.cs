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
    public partial class WmsMaterialStockReportForm : MesBaseForm
    {
        private bool _isProgrammaticPageChange = false;
        private readonly IWmsMaterialStockService _wmsMaterialStockService;
        private readonly IFactoryService _factoryService;
        public WmsMaterialStockReportForm(IWmsMaterialStockService wmsMaterialStockService, IFactoryService factoryService)
        {
            InitializeComponent();
            InitializeTable();
            _wmsMaterialStockService = wmsMaterialStockService;
            _factoryService = factoryService;
        }

        private void WmsMaterialStockReportForm_Load(object sender, EventArgs e)
        {

        }

        private void InitializeTable()
        {

            TableControl.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("Id", "Id", AntdUI.ColumnAlign.Center)
                {
                    Visible = false
                },

                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BatchCode", "物料批次", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BarCode", "物料标签", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "源数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###")
                .SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("UsageQuantity", "可用数量", AntdUI.ColumnAlign.Right).SetDefaultFilter().SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("StockCode", "仓库代码", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ShelveCode", "存储区代码", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ShelveName", "存储区描述", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                //new AntdUI.Column("StockName", "仓库名称", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LocationName", "库存地点", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("FactoryCode", "工厂", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LockQuantity", "冻结数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetColAlign().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("DownQuantity", "待出库数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetColAlign().SetLocalizationTitleID("Table.Column."),


             };

        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            keywordInput.PlaceholderText = "请输入关键字...";
            MaterialInput.PlaceholderText = "请输入物料号...";
            BatchInput.PlaceholderText = "请输入批次号...";
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
                var materials = MaterialInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var batchs = BatchInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var keyword = keywordInput.Text.Trim();

                var factory = await _factoryService.GetByIdAsync(AppSession.CurrentFactoryId);
                var result = await _wmsMaterialStockService.GetPageListAsync(factory.FactoryCode, pageIndex, pageSize, keyword, materials, batchs);
                if (result != null && result.TotalCount > 0)
                {
                    TableControl.DataSource = result.Items;
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
                var data = TableControl.DataSource as List<WmsMaterialStockDto>;
                if (data == null || !data.Any())
                {
                    throw new Exception("未查询到待导出的数据源，导出失败");
                }
                ExcelExportHelper.ExportToExcel(this.ParentForm, data, "Wms库存记录");

            }, confirmMsg: "即将导出当前数据集，是否继续？");
        }
    }
}
