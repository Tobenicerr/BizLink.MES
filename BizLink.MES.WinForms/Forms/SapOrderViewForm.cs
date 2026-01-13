using AntdUI;
using Azure;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Infrastructure;
using Dm.util;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AntdUI.Table;

namespace BizLink.MES.WinForms.Forms
{
    public partial class SapOrderViewForm : MesBaseForm
    {
        private readonly SapOrderModuleFacade _facade;
        private readonly IFormFactory _formFactory;

        // 冷却计时器 (保留原业务逻辑)
        private System.Windows.Forms.Timer _cooldownTimer;

        // 数据状态
        private SapOrderDto _sapOrder = new SapOrderDto();
        private List<MaterialViewDto> _bomMaterials = new List<MaterialViewDto>();
        public SapOrderViewForm(SapOrderModuleFacade facade, IFormFactory formFactory)
        {
            _facade = facade;
            _formFactory = formFactory;
            InitializeComponent();
            InitializeTable();
        }
        private void InitializeTable()
        {

            sapOrderTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("OrderNo", "订单号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("OperationNo", "工序序号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenter", "工作中心", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LastOperationFinish", "装配日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MesStatus", "状态", AntdUI.ColumnAlign.Center){
                    Render = (value, record, index) =>
                    {
                        return new AntdUI.CellTag(value as string, AntdUI.TTypeMini.Error);
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "订单料号", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),
                //new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("TargetQuantity", "订单数量", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("StartDate", "开工日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("FinishDate", "完工日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("PlannerRemark", "计划备注", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("NextWorkCenter", "下道工作中心", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("SuperiorOrder", "父级订单", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LeadingOrder", "成品订单", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LeadingMaterial", "成品物料", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),


                //{
                //    LocalizationTitle ="Table.Column.{id}",
                //    Call = (value, record, i_row, i_col) => {
                //        System.Threading.Thread.Sleep(2000);
                //        return value;
                //    }
                //}
                
             };

        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            dispatchdatePicker.PlaceholderText = "请先选择装配排产日期...";
            orderInput.PlaceholderText = "请输入订单号...";

            await RunAsync(async () =>
            {
                //if (AppSession.CurrentFactoryId <= 0)
                //{
                //    throw new Exception("请先选择工厂！");
                //}
                await Task.CompletedTask;
            });
        }

        #region Query Logic

        private void dispatchdatePicker_ValueChanged(object sender, DateTimeNEventArgs e)
        {
            queryButton.Enabled = true;
        }

        private async void queryButton_Click(object sender, EventArgs e)
        {
            await RunAsync(queryButton, async () =>
            {
                sapOrderTable.DataSource = null;
                if (orderInput.Text.Length >= 32767)
                    throw new Exception("输入订单内容过长，请分批进行查询！");

                // 2. 准备参数
                var dispatchDate = dispatchdatePicker.Value ?? DateTime.Now;
                List<string> orderNos = null;

                if (!string.IsNullOrEmpty(orderInput.Text))
                {
                    orderNos = orderInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                        .Distinct().ToList();
                }

                // 3. 调用 Facade 获取处理后的数据
                _sapOrder = await _facade.GetProcessedSapOrdersAsync(AppSession.CurrentUser.FactoryName, dispatchDate, orderNos);

                // 4. 绑定 UI
                sapOrderTable.DataSource = _sapOrder.sapOrderOperations;

                // 计算统计信息
                var orderCount = _sapOrder.sapOrderOperations?.GroupBy(x => x.OrderNo).Count() ?? 0;
                totalLabel.Text = $"  订单总数：{orderCount} 笔";

                AntdUI.Message.success(this, "查询成功！");

                // 5. 启动冷却计时器 (保持原逻辑)
                StartCooldown();
            });
        }

        private void StartCooldown()
        {
            queryButton.Enabled = false;
            orderSyncButton.Enabled = false;

            _cooldownTimer?.Dispose();
            _cooldownTimer = new System.Windows.Forms.Timer { Interval = 10000 }; // 10秒
            _cooldownTimer.Tick += (s, args) =>
            {
                _cooldownTimer.Stop();
                queryButton.Enabled = true;
                orderSyncButton.Enabled = true;
                this.Enabled = true;
            };
            _cooldownTimer.Start();
        }

        #endregion

        #region Sync Logic

        private async void orderSyncButton_Click(object sender, EventArgs e)
        {
            await RunAsync(orderSyncButton, async () =>
            {
                orderSpin.Visible = true;
                // 1. 校验数据
                if (_sapOrder.sapOrderOperations == null || !_sapOrder.sapOrderOperations.Any())
                {
                    throw new Exception($"未查询到 {dispatchdatePicker.Value} 有效订单");
                }

                // 2. 弹出日期选择框 (使用 Factory)
                DateTime? startDate = null;
                DateTime? endDate = null;

                // 假设 DateRangeSelectForm 尚未重构为依赖注入，如果有参数构造函数，建议重构
                // 这里临时使用 Factory 打开，并通过闭包获取结果
                bool confirmed = false;

                // 这里的 DateRangeSelectForm 需要支持无参构造函数或被 DI 容器接管
                // 如果它很简单，直接 new 也可以，但为了架构一致性：
                // *临时兼容*：如果 DateRangeSelectForm 只有带参构造函数且未注册，请使用 new
                // 下面假设它需要手动创建或通过 factory 传递参数 (如果 factory 支持)

                using (var form = new DateRangeSelectForm("订单开工日期", "订单装配日期"))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        startDate = form.StartSelectedDate;
                        endDate = form.EndSelectedDate;
                        confirmed = true;
                    }
                }

                if (!confirmed || startDate == null || endDate == null)
                    return;

                // 3. 业务校验 (数量限制)
                var minOpDict = _sapOrder.sapOrderOperations
                    .GroupBy(x => x.OrderNo)
                    .ToDictionary(g => g.Key, g => g.Min(x => x.OperationNo));

                var existingOrders = await _facade.WorkOrderService.GetListByDispatchDateAsync(null, startDate, null, AppSession.CurrentFactoryId);
                if (existingOrders != null && existingOrders.Count > 100 && minOpDict.Count > 10)
                {
                    if (AntdUI.Modal.open(this, "提示", $"{startDate:yyyy-MM-dd}已存在超过100笔订单，是否继续同步？", TType.Warn) != DialogResult.OK)
                        return;
                }

                if (AntdUI.Modal.open(this.ParentForm, "提示", "即将向MES推送订单，是否继续？", TType.Warn) != DialogResult.OK)
                    return;

                // 4. 执行同步 (调用 Facade)
                syncProgress.Visible = true;
                var progress = new Progress<float>(val => syncProgress.Value = val);

                await _facade.SyncSapDataToMesAsync(
                    _sapOrder,
                    AppSession.CurrentFactoryId,
                    startDate.Value,
                    endDate.Value,
                    AppSession.CurrentUser.EmployeeId,
                    progress);

                AntdUI.Message.success(this, "推送成功！");

                // 5. 刷新界面 (重新查询或清除已同步项)
                // 这里简单起见，直接清空或重新触发查询
                // 根据原代码逻辑，似乎是重新调用了刷新 Table
                // 我们手动清空已同步的项

                // 重新刷新列表显示
                sapOrderTable.Refresh();

            }, successMsg: null); // 成功消息已内部处理

            // Cleanup
            syncProgress.Visible = false;
            syncProgress.Value = 0;

            orderSpin.Visible = false;
        }

        #endregion

        #region Other Operations

        private async void cableCutParamButton_Click(object sender, EventArgs e)
        {
            await RunAsync(cableCutParamButton, async () =>
            {
                if (string.IsNullOrWhiteSpace(orderInput.Text))
                    return;

                var semimaterial = orderInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Distinct().ToList();

                // 调用 Facade 暴露的 API 接口或 Service
                var requestUrl = _facade.ApiSettings["MesApi"].Endpoints["GetCableCutParamByMaterial"];
                var result = await _facade.MesApi.PostAsync<object, List<CableCutParamCreateDto>>(requestUrl, new
                {
                    semiMaterialCode = semimaterial.Distinct()
                });

                if (result.IsSuccess && result.Data.Any())
                {
                    await _facade.CableCutParam.CreateBatchAsync(result.Data);
                    AntdUI.Message.success(this, "断线参数同步成功！");
                }
                else
                {
                    throw new Exception("未获取到断线参数信息！");
                }
            });
        }

        private void orderInput_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(orderInput.Text))
            {
                queryButton.Enabled = true;
            }
        }

        #endregion


    }
}
