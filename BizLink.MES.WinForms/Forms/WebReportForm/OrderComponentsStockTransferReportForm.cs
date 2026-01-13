using AntdUI;
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
    public partial class OrderComponentsStockTransferReportForm : MesBaseForm
    {
        private bool _isProgrammaticPageChange = false;
        private readonly IWorkOrderComponentsTransferService _workOrderComponentsTransferService;

        public OrderComponentsStockTransferReportForm(IWorkOrderComponentsTransferService workOrderComponentsTransferService)
        {
            InitializeComponent();
            InitializeTable();
            _workOrderComponentsTransferService = workOrderComponentsTransferService;
        }

        private void InitializeTable()
        {

            TableControl.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("FactoryId", "FactoryId", AntdUI.ColumnAlign.Center)
                {
                    Visible = false
                },

                new AntdUI.Column("WorkOrderId", "WorkOrderId", AntdUI.ColumnAlign.Center)
                {
                    Visible = false
                },

                new AntdUI.Column("WorkOrderProcessId", "WorkOrderProcessId", AntdUI.ColumnAlign.Center)
                {
                    Visible = false
                },
                new AntdUI.Column("WorkOrderNo", "订单号", AntdUI.ColumnAlign.Center).SetFixed().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ScheduledStartDate", "开工日期", AntdUI.ColumnAlign.Center).SetFixed().SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("ScheduledFinishDate", "装配日期", AntdUI.ColumnAlign.Center).SetFixed().SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ProcessStatus", "工序状态", AntdUI.ColumnAlign.Center)   
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "已合箱" => new AntdUI.CellTag("已合箱", AntdUI.TTypeMini.Success),
                            "执行中" => new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Error),
                            _ => null
                        };
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
               

                new AntdUI.Column("Status", "消耗状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "正常" => new AntdUI.CellTag("正常", AntdUI.TTypeMini.Success),
                            "异常" => new AntdUI.CellTag("异常", AntdUI.TTypeMini.Error),
                            _ => null
                        };
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                             

                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("RequiredQuantity", "BOM数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###") .SetLocalizationTitleID("Table.Column."),


                new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("BatchCode", "物料批次", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("TransferStatus", "过账状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "已同步" => new AntdUI.CellTag("已同步", AntdUI.TTypeMini.Success),
                            "同步失败" => new AntdUI.CellTag("同步失败", AntdUI.TTypeMini.Error),
                            "未同步" => new AntdUI.CellTag("未同步", AntdUI.TTypeMini.Primary),

                            _ => null
                        };
                    }
                }.SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("TransferredQuantity", "过账数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###")
                .SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("TransferMessageType", "移库返回类型", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("TransferMessage", "移库返回信息", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        var str = value as string;
                        return ErrorMessageTransfer(str);
                    }
                }.SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("PostingDate", "过账日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ReceivingBatchCode", "接收物料批次", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConfirmStatus", "报工状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "已报工" => new AntdUI.CellTag("已报工", AntdUI.TTypeMini.Success),
                            "报工失败" => new AntdUI.CellTag("报工失败", AntdUI.TTypeMini.Error),
                            "未报工" => new AntdUI.CellTag("未报工", AntdUI.TTypeMini.Primary),

                            _ => null
                        };
                    }
                }.SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                 new AntdUI.Column("ConfirmQuantity", "消耗数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###")
                .SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConfirmMessageType", "报工返回类型", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("ConfirmMessage", "报工返回信息", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        var str = value as string;
                        return ErrorMessageTransfer(str);
                    }
                }.SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("FromLocationCode", "源库位", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ToLocationCode", "目标库位", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),



                new AntdUI.Column("CreatedAt", "创建日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd HH:mi:ss").SetColAlign().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CreatedBy", "创建人员", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),


             };

        }

        private string ErrorMessageTransfer(string enMessage)
        {
            return SapErrorTranslator.ToChinese(enMessage);
        }
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            KeywordInput.PlaceholderText = "请输入关键字...";
            OrderInput.PlaceholderText = "请输入订单号...";
            StartDatePicker.PlaceholderText = "请选择开工日期...";
            DispatcDatePicker.PlaceholderText = "请选择装配日期...";
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
                var workorders = OrderInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                var keyword = KeywordInput.Text.Trim();
                var startDate = StartDatePicker.Value;
                var dispatchDate = DispatcDatePicker.Value;


                var result = await _workOrderComponentsTransferService.GetPageListAsync( pageIndex, pageSize, AppSession.CurrentFactoryId, keyword, workorders, startDate, dispatchDate);
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
    }
}
