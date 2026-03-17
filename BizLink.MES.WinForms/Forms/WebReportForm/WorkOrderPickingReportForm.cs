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
    public partial class WorkOrderPickingReportForm : MesBaseForm
    {

        private bool _isProgrammaticPageChange = false;
        private readonly IWorkOrderKittingExecuteService _workOrderKittingExecuteService;
        private readonly IFormFactory _formFactory;


        public WorkOrderPickingReportForm(IWorkOrderKittingExecuteService workOrderKittingExecuteService,IFormFactory formFactory)
        {
            InitializeComponent();
            InitializeTable();
            _workOrderKittingExecuteService = workOrderKittingExecuteService;
            _formFactory = formFactory;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            StartDatePickerRange.PlaceholderStart = "排产日期开始";
            StartDatePickerRange.PlaceholderEnd = "排产日期结束";
            WorkOrderInput.PlaceholderText = "请输入订单号";
            GroupCodeInput.PlaceholderText = "请输入成组组号";
         

        }
        private void InitializeTable()
        {
            TableControl.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("WorkOrderNo", "订单号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "订单物料", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Quantity", "订单数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("GroupCode", "成组组号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("StartDate", "仓库排产时间", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("yyyy-MM-dd").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center) {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "未成组" => new AntdUI.CellTag("未成组", AntdUI.TTypeMini.Error),
                            "已成组" => new AntdUI.CellTag("已成组", AntdUI.TTypeMini.Primary),
                            "已合箱" => new AntdUI.CellTag("已合箱", AntdUI.TTypeMini.Success),
                            _ => new AntdUI.CellTag("未成组", AntdUI.TTypeMini.Error)
                        };
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("GroupTime", "成组时间", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("KittingTime", "合箱时间", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                
                new AntdUI.Column("OperateUser", "成组人员", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ShotRemark", "缺料备注", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LabelCount", "标签种类数", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("DispatchDate", "计划完成日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),

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
            DateTime? startdateStart = null;
            DateTime? startdateEnd = null;
            var workOrders =  WorkOrderInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var groupCodes = GroupCodeInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (StartDatePickerRange.Value != null && StartDatePickerRange.Value.Count() == 2) 
            {
                startdateStart = StartDatePickerRange.Value[0];
                startdateEnd = StartDatePickerRange.Value[1];
            }
            var result = await _workOrderKittingExecuteService.GetPageListAsync(pageIndex, pageSize, AppSession.CurrentFactoryId, startdateStart, startdateEnd, workOrders, groupCodes);
            if (result != null)
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
                var data = TableControl.DataSource as List<WorkOrderKittingExecuteDto>;
                if (data == null || !data.Any())
                {
                    throw new Exception("未查询到待导出的数据源，导出失败");
                }
                var groupCodes = data.Where(x => !string.IsNullOrEmpty(x.GroupCode)).Select(x => x.GroupCode).Distinct();
                if(groupCodes == null || groupCodes.Count() == 0)
                    throw new Exception("未查询到待导出的成组组号，导出失败");
                string? selected = null;
                if (groupCodes.Count() > 1) 
                {

                    var menuItems = groupCodes.OrderByDescending(x => x).Select(x => new AntdUI.MenuItem
                    {
                        Name = x,
                        Text = x
                    }).ToList();

                    _formFactory.Show<CommonSelectForm>(f => {
                        f.FormClosed += (s, e) => {
                            if (f.DialogResult == DialogResult.OK) selected = f.SelectedValue.Text;
                        };
                    }, true, "选择待导出成组组号", menuItems);

                    if (string.IsNullOrEmpty(selected)) throw new Exception("未选择待导出成组组号");
                }
                else
                    selected = groupCodes.First();

                var result = await _workOrderKittingExecuteService.GetListByGroupCodeAsync(selected);

                ExcelExportHelper.ExportSplitColumnsToA4(this.ParentForm, result.Select(x => x.WorkOrderNo).OrderBy(x => x).Distinct().ToList(), "订单号", $"{selected}成组清单");

            }, confirmMsg: "即将导出当前数据集，是否继续？");
        }

    }
}
