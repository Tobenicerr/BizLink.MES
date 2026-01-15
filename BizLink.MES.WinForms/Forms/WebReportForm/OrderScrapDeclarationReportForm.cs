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
    public partial class OrderScrapDeclarationReportForm : MesBaseForm
    {
        private bool _isProgrammaticPageChange = false;

        private readonly ISapOrderScrapDeclarationService _sapOrderScrapDeclarationService;
        private readonly IFactoryService _factoryService;
        private readonly IMaterialViewService _materialViewService;
        public OrderScrapDeclarationReportForm(ISapOrderScrapDeclarationService sapOrderScrapDeclarationService, IFactoryService factoryService, IMaterialViewService materialViewService)
        {
            InitializeComponent();
            InitializeTable();
            _sapOrderScrapDeclarationService = sapOrderScrapDeclarationService;
            _factoryService = factoryService;
            _materialViewService = materialViewService;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            KeywordInput.PlaceholderText = "请输入关键字...";
            CreateDatePicker.PlaceholderText = "请选择创建日期...";
        }

        private void InitializeTable()
        {
            TableControl.Columns = new AntdUI.ColumnCollection
            {


                new AntdUI.Column("WorkOrderNo", "订单号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("OperationNo", "工序序号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenterCode", "工作中心", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "订单物料", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                //new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                //new AntdUI.Column("SuperiorOrder", "上级订单", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                //new AntdUI.Column("LeadingOrder", "顶级订单", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                //new AntdUI.Column("LeadingMaterial", "顶级物料", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ScrapMaterialType", "状态", AntdUI.ColumnAlign.Center) {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "RAW_MATERIAL" => new AntdUI.CellTag("原材料报废", AntdUI.TTypeMini.Primary),
                            "FINISHED_GOODS" => new AntdUI.CellTag("半成品报废", AntdUI.TTypeMini.Success),
                            _ => null
                        };
                    }
                }.SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ScrapMaterialCode", "报废物料", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("ScrapMaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                  new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                  new AntdUI.Column("ScrapQuantity", "报废数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),

                  new AntdUI.Column("ScrapReason", "报废原因", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                      new AntdUI.Column("Remark", "备注", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CreatedBy", "操作人员", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CreatedOn", "创建时间", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                   new AntdUI.Column("FactoryCode", "工厂", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),


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
            TableControl.DataSource = null;
            var pageSize = PaginationControl.PageSize;
            var pageIndex = PaginationControl.Current;
            var keyword = KeywordInput.Text.Trim();
            var createDate = CreateDatePicker.Value;
            var factory = await _factoryService.GetByIdAsync(AppSession.CurrentFactoryId);
            var result = await _sapOrderScrapDeclarationService.GetPageListAsync(pageIndex, pageSize, factory.FactoryCode, keyword, createDate);
            if (result != null)
            {
                //var materialCodes = result.Items.Select(i => i.ScrapMaterialCode).Distinct().ToList();
                //var materials = await _materialViewService.GetListByCodesAsync(factory.FactoryCode,materialCodes);
                TableControl.DataSource = result.Items;
            }
            else
                TableControl.DataSource = null;
            return result.TotalCount;
        }

        private async void PaginationControl_ValueChanged(object sender, AntdUI.PagePageEventArgs e)
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
                var data = TableControl.DataSource as List<SapOrderScrapDeclarationDto>;
                if (data != null && data.Count() > 0)
                {
                    var result = data.Select(d => new
                    {
                        订单号 = d.WorkOrderNo,
                        工序序号 = d.OperationNo,
                        工作中心 = d.WorkCenterCode,
                        订单物料 = d.MaterialCode,
                        //d.MaterialDesc,
                        //d.SuperiorOrder,
                        //d.LeadingOrder,
                        //d.LeadingMaterial,
                        报废类型 = d.ScrapMaterialType == "RAW_MATERIAL" ? "原材料报废" : d.ScrapMaterialType == "FINISHED_GOODS" ? "半成品报废" : d.ScrapMaterialType,
                        报废物料 = d.ScrapMaterialCode,
                        物料描述 = d.ScrapMaterialDesc,
                        单位 = d.BaseUnit,
                        报废数量 = d.ScrapQuantity,
                        报废原因 = d.ScrapReason,
                        备注 = d.Remark,
                        创建人员 = d.CreatedBy,
                        创建时间 = d.CreatedOn,
                        工厂代码 = d.FactoryCode
                    }).ToList();
                    ExcelExportHelper.ExportToExcel(this.ParentForm, result, $"报废填报清单{DateTime.Now:yyyyMMdd}");
                }
                else
                    throw new Exception("当前无数据可导出，请先执行查询！");


            }, successMsg: "导出完成！", confirmMsg: "是否导出报废填报清单信息？");
        }
    }
}
