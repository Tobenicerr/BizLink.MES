using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Infrastructure;
using Microsoft.Extensions.Options;
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
    public partial class ReplenishmentPlanReportForm : MesBaseForm
    {
        private readonly ReplenishmentModuleFacade _facade;
        //private readonly IMesApiClient _mesApiClient;
        //private readonly IMaterialViewService _materialViewService;
        //private readonly IFactoryService _factoryService;
        //private readonly IAutoMaterialStockService _autoMaterialStockService;
        //private readonly Dictionary<string, ServiceEndpointSettings> _apiSettings;
        public ReplenishmentPlanReportForm(ReplenishmentModuleFacade facade)
        {
            _facade = facade;
            InitializeComponent();
            InitializeTable();
        }

        private void InitializeTable()
        {

            TableControl.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("PlanQuantity", "需求数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),


                new AntdUI.Column("UsageQuantity", "自动仓库存", AntdUI.ColumnAlign.Right).SetDefaultFilter().SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("DiffQuantity", "差异数量", AntdUI.ColumnAlign.Right).SetDefaultFilter().SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ReplenisQuantity", "待补料数量", AntdUI.ColumnAlign.Right).SetDefaultFilter().SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConsumeType", "物料属性", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),


             };

        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            StartDatePicker.PlaceholderText = "请选择计划排产日期...";

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
            var factory = await _facade.FactoryService.GetByIdAsync(AppSession.CurrentFactoryId);
            var date = StartDatePicker.Value;
            if (date == null)
                throw new Exception("请选择计划排产日期！");

            // 2. 调用 Facade 获取计算好的数据
            // 复杂的 API 循环调用和 LINQ 计算全部封装在 Facade 中，UI 不再关心
            var data = await _facade.GenerateReplenishmentReportAsync(factory.FactoryCode,date.Value.AddDays(3));

            if (data != null && data.Any())
            {
                TableControl.DataSource = data;
                return data.Count();
            }
            else
            {
                TableControl.DataSource = null;
                throw new Exception("未查询到符合条件的补料数据！");
            }
            //try
            //{
            //    SpinControl.Visible = true;
            //    TableControl.DataSource = null;
            //    var date = StartDatePicker.Value;
            //    if (date == null)
            //        throw new Exception("请选择计划排产日期！");
            //    var sapBoms = new List<SapOrderBom>();
            //    var tempdate = (date.Value.Date).AddDays(1);
            //    while (tempdate < ((DateTime)date).AddDays(3).Date)
            //    {
            //        var requestUrl = $"{_apiSettings["MesApi"].Endpoints["GetWorkOrdersByDispatchDate"]}?factoryCode={AppSession.CurrentUser.FactoryName}&dispatchDate={tempdate}";
            //        var result = await _mesApiClient.GetAsync<SapOrderDto>(requestUrl);
            //        if (result.IsSuccess && result.Data.sapOrderBoms.Any())
            //        {
            //            sapBoms.AddRange(result.Data.sapOrderBoms);
            //        }
            //        tempdate = tempdate.AddDays(1);
            //    }

            //    if (sapBoms.Any())
            //    {
            //        var factory = await _factoryService.GetByIdAsync(AppSession.CurrentFactoryId);
            //        var materials = await _materialViewService.GetListByCodesAsync(factory.FactoryCode, sapBoms.Select(x => x.MaterialCode).Distinct().ToList());
            //        var antostocks = await _autoMaterialStockService.GetListByMaterialCodeAsync(sapBoms.Select(x => x.MaterialCode).Distinct().ToList());

            //        var joinBoms = sapBoms.Join(materials, b => b.MaterialCode, m => m.MaterialCode, (b, m) => new ReplenishmentPlanView()
            //        {
            //            MaterialCode = b.MaterialCode,
            //            MaterialDesc = b.MaterialDesc,
            //            PlanQuantity = (decimal)b.RequireQuantity,
            //            BaseUnit = b.BaseUnit,
            //            ConsumeType = m.LabelName,

            //        });
            //        var groupStock = antostocks.GroupBy(x => x.MaterialCode).Select(g => new
            //        {
            //            MaterialCode = g.Key,
            //            UsageQuantity = g.Sum(x => x.Quantity)
            //        }).ToList();
            //        var groupMat = joinBoms.Where(b => b.ConsumeType == "自动仓物料").GroupBy(x => x.MaterialCode).Select(g => new ReplenishmentPlanView()
            //        {
            //            MaterialCode = g.Key,
            //            MaterialDesc = g.First().MaterialDesc,
            //            PlanQuantity = g.Sum(x => x.PlanQuantity),
            //            BaseUnit = g.First().BaseUnit,
            //            ConsumeType = g.First().ConsumeType,
            //        }).ToList();
            //        var finalList = groupMat.Join(groupStock, m => m.MaterialCode, s => s.MaterialCode, (m, s) =>
            //        {
            //            m.UsageQuantity = s.UsageQuantity;
            //            m.DiffQuantity = m.PlanQuantity - m.UsageQuantity;
            //            m.ReplenisQuantity = m.DiffQuantity > 0 ? m.DiffQuantity : 0;
            //            return m;
            //        }).Where(x => x.ReplenisQuantity > 0).ToList();
            //        if (finalList.Any())
            //        {
            //            TableControl.DataSource = finalList;
            //            return finalList.Count();
            //        }
            //        else
            //        {
            //            return 0;
            //        }

            //    }

            //    return 0;

            //}
            //catch (Exception)
            //{

            //    throw;
            //}
            //finally
            //{
            //    SpinControl.Visible = false;
            //}
        }

        //private class ReplenishmentPlanView
        //{
        //    public string MaterialCode
        //    {
        //        get; set;
        //    }
        //    public string MaterialDesc
        //    {
        //        get; set;
        //    }
        //    public decimal PlanQuantity
        //    {
        //        get; set;
        //    }
        //    public string BaseUnit
        //    {
        //        get; set;
        //    }
        //    public string ConsumeType
        //    {
        //        get; set;
        //    }
        //    public decimal UsageQuantity
        //    {
        //        get; set;
        //    }
        //    public decimal DiffQuantity
        //    {
        //        get; set;
        //    }
        //    public decimal ReplenisQuantity
        //    {
        //        get; set;
        //    }
        //}

        private async void ExportButton_Click(object sender, EventArgs e)
        {
            await RunAsync(ExportButton, async () =>
            {
                var data = TableControl.DataSource as List<ReplenishmentModuleFacade.ReplenishmentPlanView>;
                if (data == null || !data.Any())
                {
                    throw new Exception("未查询到待导出的数据源，导出失败");
                }
                ExcelExportHelper.ExportToExcel(this.ParentForm, data, "三日补料清单");

            }, confirmMsg: "即将导出当前数据集，是否继续？");
        }
    }
}
