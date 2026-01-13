using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
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
    public partial class CableCutOverdueTaskReportForm : MesBaseForm
    {
        private readonly IWorkOrderInProgressViewService _workOrderInProgressViewService;
        private readonly IWorkOrderTaskService _workOrderTaskService;
        public CableCutOverdueTaskReportForm(IWorkOrderInProgressViewService workOrderInProgressViewService, IWorkOrderTaskService workOrderTaskService)
        {
            InitializeComponent();
            InitializeTable();
            _workOrderInProgressViewService = workOrderInProgressViewService;
            _workOrderTaskService = workOrderTaskService;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            KeywordInput.PlaceholderText = "请输入关键字...";
        }
        private void InitializeTable()
        {
            TableControl.Columns = new AntdUI.ColumnCollection
            {

                 
                new AntdUI.Column("OrderNumber", "订单号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "订单物料", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "订单数量(PCS)", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("RequiredQuantity", "需求数量(M)", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenter", "工作中心", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center) {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "0" => new AntdUI.CellTag("未准备", AntdUI.TTypeMini.Error),
                            "1" => new AntdUI.CellTag("已排产", AntdUI.TTypeMini.Primary),
                            "2" => new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Default),
                            "3" => new AntdUI.CellTag("已挂起", AntdUI.TTypeMini.Success),
                            _ => null
                        };
                    }
                }.SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("StartTime", "开工日期", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("DispatchDate", "装配日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CableMaterial", "断线物料", AntdUI.ColumnAlign.Right).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("TaskQuantity", "断线数量(PCS)", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CompletedQty", "完成数量(PCS)", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Operator", "操作人员", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("OperationTime", "操作时间", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Remark", "生产备注", AntdUI.ColumnAlign.Left).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),


            };
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SearchButton, async () =>
            {
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
                var keyword = KeywordInput.Text.Trim();
                var result = await _workOrderInProgressViewService.GetOverdueCableTaskListByDateAsync(AppSession.CurrentFactoryId, keyword);
                if (result != null )
                {
                   var taskids = result.Where(r => r.TaskId != null).Select(r => (int)r.TaskId).ToList();
                    var cuttasks = await _workOrderTaskService.GetByIdAsync(taskids);
                    TableControl.DataSource = result.GroupJoin(cuttasks,agg => agg.TaskId,task => task.Id,(agg,task) => new { agg, task })
                        .SelectMany(t => t.task.DefaultIfEmpty(),(temp, task) => new 
                        {
                            temp.agg.OrderNumber,
                            temp.agg.MaterialCode,
                            temp.agg.MaterialDesc,
                            temp.agg.Quantity,
                            temp.agg.RequiredQuantity,
                            temp.agg.WorkCenter,
                            temp.agg.Status,
                            temp.agg.StartTime,
                            temp.agg.DispatchDate,
                            temp.agg.CableMaterial,
                            temp.agg.CompletedQty,
                            temp.agg.ProfitCenter,
                            TaskQuantity = task?.Quantity??0,
                            Operator = task?.UpdateBy??task?.CreateBy,
                            OperationTime = task?.UpdateOn??task?.CreateOn,
                            Remark = task?.ProductionRemark

                        }).OrderBy(x => x.OrderNumber).OrderByDescending(x => x.StartTime).ToList();
                    return result.Count();
                }
                else
                {
                    TableControl.DataSource = null;
                    return 0;
                }

                

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

    }
}
