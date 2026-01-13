using AntdUI;
using AutoMapper.Internal;
using Azure;
using Azure.Core;
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
using Dm.util;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BizLink.MES.WinForms.Forms
{
    // 1. 继承 MesBaseForm
    public partial class OperationConfirmForm : MesBaseForm
    {
        private readonly OperationConfirmModuleFacade _facade;
        private bool _isProgrammaticPageChange = false;
        private readonly IServiceScopeFactory _scopeFactory; // 【新增】用于创建临时事务 Scope

        // 常量
        private const string PackConfirmWorkCenterParamGroup = "CN11AllowedConfirmWorkCenter";
        private const string PackConfirmWorkCenterKey = "PackWorkCenter";

        // 2. 构造函数：只注入 Facade
        public OperationConfirmForm(OperationConfirmModuleFacade facade,IServiceScopeFactory scopeFactory)
        {
            _facade = facade;
            _scopeFactory = scopeFactory;
            InitializeComponent();
            InitializeTable();
        }

        // 3. 窗体加载
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            statusSelect.PlaceholderText = "请选择同步状态...";
            ordersInput.PlaceholderText = "请输入订单号...";
            operationSelect.PlaceholderText = "请选择工序序号...";

            InitStatusSelect();

            // 异步加载工序下拉框
            await RunAsync((Func<Task>)(async () =>
            {
                var operations = await _facade.WorkOrderProcessService.GetAllOperationAsync();
                this.operationSelect.Items.Clear();
                if (operations != null)
                {
                    this.operationSelect.Items.AddRange((object[])operations.OrderBy(x => x).Select(x => new MenuItem { Name = x, Text = x }).ToArray());
                }
                //  this.operationSelect.SelectedIndex = 0;

            }));
        }

        private void InitStatusSelect()
        {
            statusSelect.Items.Clear();
            statusSelect.Items.AddRange(new[]
            {
                new MenuItem { Name = "", Text = "全部" },
                new MenuItem { Name = "0", Text = "未同步" },
                new MenuItem { Name = "-1", Text = "同步失败" },
                new MenuItem { Name = "1", Text = "已同步" }
            });
            statusSelect.SelectedIndex = 1;
        }

        // --- 查询 ---
        private async void queryButton_Click(object sender, EventArgs e)
        {
            await RunAsync(queryButton, async () =>
            {
                _isProgrammaticPageChange = true;
                try
                {
                    // 这里的赋值可能会触发 ValueChanged 事件
                    // 但因为标志位为 true，事件内部会直接 return，不会执行查询
                    pagination.Current = 1;
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
            confirmTable.DataSource = null;
            var pageSize = pagination.PageSize;
            var pageIndex = pagination.Current;
            var orders = ordersInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var status = (statusSelect.SelectedValue as MenuItem)?.Name ?? "";
            //var operation = (operationSelect.SelectedValue as MenuItem)?.Text ?? "";
            var operation = (operationSelect.SelectedValue).Select(obj => Convert.ToString(obj) ?? string.Empty).ToList();
            // 使用 Facade 调用
            var result = await _facade.OperationConfirm.GetOperationConfirmPageListAsync(pageIndex, pageSize, orders, status, operation);

            if (result != null && result.TotalCount > 0)
            {
                confirmTable.DataSource = result.Items.Select(x => new OperationConfirmFormView(x)).ToList();
                pagination.Total = result.TotalCount;
            }
            else
            {
                confirmTable.DataSource = null;
            }

            return result.TotalCount;

        }

        // --- 展开详情 ---
        private async void confirmTable_ExpandChanged(object sender, TableExpandEventArgs e)
        {
            if (e.Expand && e.Record is OperationConfirmFormView data)
            {
                try
                {
                    // 使用 Facade 调用
                    var confirm = await _facade.OperationConfirm.GetConfirmWitemConsumeptionAsync(data.Id ?? 0);
                    if (confirm != null)
                    {
                        data.Children = confirm.Consumps.Select(x => new WorkOrderOperationConsumpDto
                        {
                            MaterialCode = x.MaterialCode,
                            Quantity = x.Quantity,
                            BatchCode = x.BatchCode,
                            BaseUnit = x.BaseUnit,
                            MovementType = x.MovementType,
                            MovementReason = x.MovementReason,
                            ReservationItem = x.ReservationItem,
                            ReservationNo = x.ReservationNo,
                            CreatedAt = x.CreatedAt
                        }).ToArray();
                    }
                }
                catch (Exception ex)
                {
                    AntdUI.Message.error(this, "加载详情失败：" + ex.Message);
                }
            }
        }

        // --- 操作：重推 (Re-Entry) ---
        private async void confirmTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is OperationConfirmFormView data)
            {
                if (e.Btn.Id == "reentry")
                {
                    // 使用 RunAsync 处理重推逻辑
                    await RunAsync(async () =>
                    {
                        if (data.Id == null || data.Id == 0)
                        {
                            await HandleNewEntryRePushAsync(data);
                        }
                        else
                        {
                            await HandleExistingEntryRePushAsync(data);
                        }

                        data.Status = "1"; // 更新 UI 状态


                    }, successMsg: "重推成功！");
                }
                else if (e.Btn.Id == "split")
                {

                    await RunAsync(async () =>
                    {
                        var inputNum = new AntdUI.InputNumber
                        {
                            PlaceholderText = "请输入拆分数量...",
                            Width = 400,
                            Height = 45,
                            BorderWidth = 1
                        };
                        // 显示结果
                        var result = AntdUI.Modal.open(new AntdUI.Modal.Config(this.ParentForm, "拆分数量", inputNum, AntdUI.TType.Warn));
                        if (result == DialogResult.OK)
                        {
                            decimal userValue = inputNum.Value;
                            if (userValue <= 0)
                                throw new Exception("拆分数量必须为大于0的整数！");
                            var operationConfirm = await _facade.OperationConfirm.GetConfirmWitemConsumeptionAsync((int)data.Id);
                            if (operationConfirm != null)
                            {
                                if (operationConfirm.Consumps != null && operationConfirm.Consumps.Count() > 0)
                                {
                                    throw new Exception("当前工序包含物料消耗，无法拆分");
                                }
                                if (operationConfirm.Status != "-1")
                                {
                                    throw new Exception("当前报工状态不允许拆分！");
                                }
                                using (var transactionScope = _scopeFactory.CreateScope())
                                {
                                    var scopedFacade = transactionScope.ServiceProvider.GetRequiredService<OperationConfirmModuleFacade>();
                                    var rtn = await scopedFacade.OperationConfirm.SplitOperationConfirmAsync((int)data.Id, userValue, scopedFacade.Serial.GenerateNext($"{operationConfirm.FactoryCode}ConfirmReportToSAP"));
                                    if (!rtn)
                                        throw new Exception("拆分失败！");

                                }


                            }
                            else
                                throw new Exception("未查询到报工信息,无法拆分！");


                        }
                        else
                            throw new Exception("拆分已取消！");
                    }, successMsg: "拆分成功！");
                    
                }

            }
        }

        private async Task HandleNewEntryRePushAsync(OperationConfirmFormView data)
        {
            //判断当前工序是否有装配完工报工
            var allStations = await _facade.WorkStation.GetAllAsync(AppSession.CurrentFactoryId);
            if (allStations != null)
            {

                var endStation = allStations.Where(x => x.WorkAreaId == 2 && x.WorkCenterId == 0 && x.IsEndStep).First();
                var taskNo = data.WorkOrderNo + "-" + data.OperationNo;
                var tasks = await _facade.Task.GetByTaskNoAsync(taskNo);
                if (tasks.Any(x => x.WorkStationId == endStation.Id && x.CompletedQty > 0))
                {
                    var task = tasks.Where(x => x.WorkStationId == endStation.Id && x.CompletedQty > 0).First();
                    var sapRequestUrl = _facade.ApiSettings["MesApi"].Endpoints["OperationConfirmToSAP"];
                    // 模拟调用：

                    var taskConfirms = await _facade.Confirm.GetListByTaskIdAsync(task.Id);
                    foreach (var item in taskConfirms)
                    {
                        var requestBody = new
                        {
                            factoryCode = AppSession.CurrentUser.FactoryName,
                            processId = data.ProcessId,
                            employeeId = AppSession.CurrentUser.EmployeeId,
                            confirmId = item.Id,
                        };

                        // 注意：PutAsync 的 URL 处理可能需要检查
                        var result = await _facade.MesApi.PostAsync<object, object>(sapRequestUrl, requestBody);

                        if (result == null || !result.IsSuccess)
                        {
                            data.Status = "-1"; // 更新 UI 状态为失败
                            data.Message = (result?.Message.split("\"message\":\"")[1]).split("\",\"data")[0] ?? "重推失败";
                            data.MessageType = "E";
                            throw new Exception($"重推失败：{result?.Message ?? "API返回空"}");
                        }
                    }

                }
            }
            var allowconfirmworkcenters = (await _facade.Params.GetGroupWithItemsAsync(PackConfirmWorkCenterParamGroup))
                ?.Items.FirstOrDefault(x => x.Key == PackConfirmWorkCenterKey)?.Value;

            var currentProcess = await _facade.WorkOrderProcessService.GetByIdAsync((int)data.ProcessId);
            if (currentProcess == null)
                throw new Exception($"找不到工序 (ID: {data.ProcessId})");

            var allProcesses = await _facade.WorkOrderProcessService.GetListByOrderIdAync((int)currentProcess.WorkOrderId);

            bool isAllowed = allowconfirmworkcenters?.Split(',').Contains(currentProcess.WorkCenter) ?? false;
            if (currentProcess.Id == allProcesses.OrderBy(x => x.Operation).First().Id)
            {
                if (allProcesses.Count <= 1 || !isAllowed)
                {
                    throw new Exception("当前订单暂不在MES中报工，请在SAP中报工！");
                }

                var sapRequestUrl = _facade.ApiSettings["MesApi"].Endpoints["WorkOrderPickVerfCommit"]; // 或者从 ApiSettings 获取


                // 模拟调用：
                var requestBody = new
                {
                    WorkOrderId = currentProcess.WorkOrderId,
                    Status = "verfReentry",
                    UpdateBy = AppSession.CurrentUser.EmployeeId,
                    UpdateOn = DateTime.Now,
                };

                // 注意：PutAsync 的 URL 处理可能需要检查
                var result = await _facade.MesApi.PutAsync<object, object>(sapRequestUrl, requestBody);

                if (result == null || !result.IsSuccess)
                {
                    data.Status = "-1"; // 更新 UI 状态为失败
                    data.Message = (result?.Message.split("\"message\":\"")[1]).split("\",\"data")[0] ?? "重推失败";
                    data.MessageType = "E";
                    throw new Exception($"重推失败：{result?.Message ?? "API返回空"}");
                }
                else
                {
                    //sap报工
                    var operationConfirm = await _facade.OperationConfirm.GetListByProcessIdAsync(currentProcess.Id);

                    foreach (var confirm in operationConfirm)
                    {

                        if (confirm.Status != "1")
                        {
                            sapRequestUrl = _facade.ApiSettings["MesApi"].Endpoints["ReentryConfirmToSAP"];

                            var query = HttpUtility.ParseQueryString(string.Empty);

                            query["confirmid"] = data.Id.ToString();

                            string requestUriWithQuery = sapRequestUrl + "?" + query.ToString();

                            var response = await _facade.MesApi.PostAsync<object, object>(requestUriWithQuery, null);

                            if (response == null || !response.IsSuccess)
                            {
                                data.Status = "-1"; // 更新 UI 状态为失败
                                data.Message = response?.Message ?? "重推失败";
                                data.MessageType = "E";
                                throw new Exception($"重推失败：{response?.Message ?? "API返回空"}");
                            }
                        }
                    }
                }
            }
            else
                throw new Exception("当前工序未报工，无法重推！");
        }

        private async Task HandleExistingEntryRePushAsync(OperationConfirmFormView data)
        {
            if (data.Status == "1")
                throw new Exception("该报工已同步至SAP，无需重推！");

            var sapRequestUrl = _facade.ApiSettings["MesApi"].Endpoints["ReentryConfirmToSAP"];

            var query = HttpUtility.ParseQueryString(string.Empty);

            query["confirmid"] = data.Id.ToString();

            string requestUriWithQuery = sapRequestUrl + "?" + query.ToString();

            var response = await _facade.MesApi.PostAsync<object, object>(requestUriWithQuery, null);

            if (response == null || !response.IsSuccess)
            {
                data.Status = "-1"; // 更新 UI 状态为失败
                data.Message = response?.Message ?? "重推失败";
                data.MessageType = "E";
                throw new Exception($"重推失败：{response?.Message ?? "API返回空"}");
            }
        }

        // --- 导出 ---
        private async void exportButton_Click(object sender, EventArgs e)
        {
            await RunAsync(exportButton, async () =>
            {
                var data = confirmTable.DataSource as List<OperationConfirmFormView>;
                if (data == null || !data.Any())
                {
                    throw new Exception("未查询到待导出的数据源，导出失败");
                }
                ExcelExportHelper.ExportToExcel(this.ParentForm, data, "SAP报工记录");

            }, confirmMsg: "即将导出当前数据集，是否继续？");
        }

        // --- UI 初始化 ---

        private void InitializeTable()
        {
            confirmTable.Columns = new AntdUI.ColumnCollection

            {

                new AntdUI.Column("Id", "Id", AntdUI.ColumnAlign.Center)

                {

                    Visible = false

                },

                new AntdUI.Column("WorkOrderNo", "订单号", AntdUI.ColumnAlign.Center)

                {

                   KeyTree = "Children",

                   Render = (value, record, index) =>

                    {

                        return ((value??"") as string).TrimStart('0');

                    }



                }.SetFixed().SetWidth("auto").SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("OperationNo", "工序序号", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("ConfirmSequence", "报工序号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("WorkCenterCode", "工作中心", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("YieldQuantity", "报工数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)

                {

                    Render = (value, record, index) =>

                    {

                        switch (value as string)

                        {

                            case "-1":

                                return new AntdUI.CellTag("同步失败", AntdUI.TTypeMini.Error);

                            case "0":

                                return new AntdUI.CellTag("未同步", AntdUI.TTypeMini.Primary);



                            case "1":

                                return new AntdUI.CellTag("已同步", AntdUI.TTypeMini.Success);

                            default:

                                return null;

                        }

                    }

                }.SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MessageType", "Sap返回类别", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Message", "Sap返回信息", AntdUI.ColumnAlign.Left).SetWidth("1000").SetColAlign().SetLocalizationTitleID("Table.Column."),



                new AntdUI.Column("ReservationNo", "预留号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("ReservationItem", "预留行", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialCode", "过账物料", AntdUI.ColumnAlign.Center)

                {

                    Render = (value, record, index) =>

                            {

                                return ((value ?? "") as string).TrimStart('0');

                            }

                }.SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Quantity", "过账数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("BatchCode", "过账批次", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MovementType", "过账类型", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MovementReason", "报废原因", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),



                new AntdUI.Column("CompletedFlag", "完工标识", AntdUI.ColumnAlign.Center){

                    Render = (value, record, index) =>

                        {

                            switch (value as string)

                            {

                                case "X":

                                    return new AntdUI.CellTag("已完工", AntdUI.TTypeMini.Success);

                                default:

                                    return null;

                            }

                        }

                }.SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("FactoryCode", "工厂代码", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("PostingDate", "过账日期", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("yyyy-MM-dd").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("SapConfirmationNo", "Sap确认号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("CreatedAt", "提交日期", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),



               new AntdUI.Column("Btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),



             };



        }

        private async void pagination_ValueChanged(object sender, PagePageEventArgs e)
        {
            // 如果是代码内部请求修改页码（例如点击查询按钮重置为1），则不触发查询
            if (_isProgrammaticPageChange)
                return;

            // 只有用户手动点击分页条时，才触发查询
            await LoadDataAsync();
        }
    }

    public class OperationConfirmFormView : AntdUI.NotifyProperty
    {
        public OperationConfirmFormView(WorkOrderOperationConfirmDto dto)
        {
            _id = dto.Id;
            _workOrderNo = dto.WorkOrderNo;
            _processId = dto.ProcessId;
            _operationNo = dto.OperationNo;
            _confirmSequence = dto.ConfirmSequence;
            _workCenterCode = dto.WorkCenterCode;
            _yieldQuantity = dto.YieldQuantity;
            _completedFlag = dto.CompletedFlag;
            _status= dto.Status;
            _messageType = dto.MessageType;
            _message = SapErrorTranslator.ToChinese(dto.Message);
            _factoryCode = dto.FactoryCode;
            _postingDate = dto.PostingDate;
            _sapConfirmationNo = dto.SapConfirmationNo;
            _createdAt = dto.CreatedAt;

            // 初始化按钮
            _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("reentry", "重推", AntdUI.TTypeMini.Primary){ Enabled = _status == "0" || _status == "-1"  },
                        new AntdUI.CellButton("split", "拆分", AntdUI.TTypeMini.Error){ Enabled =  _status == "-1"  } };
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

        string? _workOrderNo;
        public string? WorkOrderNo
        {
            get => _workOrderNo;
            set
            {
                if (_workOrderNo == value)
                    return;
                _workOrderNo = value;
                OnPropertyChanged();
            }
        }

        int? _processId;
        public int? ProcessId
        {
            get => _processId;
            set
            {
                if (_processId == value)
                    return;
                _processId = value;
                OnPropertyChanged();
            }
        }

        string? _operationNo;
        public string? OperationNo
        {
            get => _operationNo;
            set
            {
                if (_operationNo == value)
                    return;
                _operationNo = value;
                OnPropertyChanged();
            }
        }

        string? _confirmSequence;
        public string? ConfirmSequence
        {
            get => _confirmSequence;
            set
            {
                if (_confirmSequence == value)
                    return;
                _confirmSequence = value;
                OnPropertyChanged();
            }
        }

        string? _workCenterCode;
        public string? WorkCenterCode
        {
            get => _workCenterCode;
            set
            {
                if (_workCenterCode == value)
                    return;
                _workCenterCode = value;
                OnPropertyChanged();
            }
        }

        decimal? _yieldQuantity;
        public decimal? YieldQuantity
        {
            get => _yieldQuantity;
            set
            {
                if (_yieldQuantity == value)
                    return;
                _yieldQuantity = value;
                OnPropertyChanged();
            }
        }

        string? _completedFlag;
        public string? CompletedFlag
        {
            get => _completedFlag;
            set
            {
                if (_completedFlag == value)
                    return;
                _completedFlag = value;
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

        string? _messageType;
        public string? MessageType
        {
            get => _messageType;
            set
            {
                if (_messageType == value)
                    return;
                _messageType = value;
                OnPropertyChanged();
            }
        }

        string? _message;
        public string? Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged();
            }
        }

        string? _factoryCode;
        public string? FactoryCode
        {
            get => _factoryCode;
            set
            {
                if (_factoryCode == value)
                    return;
                _factoryCode = value;
                OnPropertyChanged();
            }
        }

        DateTime? _postingDate;
        public DateTime? PostingDate
        {
            get => _postingDate;
            set
            {
                if (_postingDate == value)
                    return;
                _postingDate = value;
                OnPropertyChanged();
            }
        }

        int? _sapConfirmationNo;
        public int? SapConfirmationNo
        {
            get => _sapConfirmationNo;
            set
            {
                if (_sapConfirmationNo == value)
                    return;
                _sapConfirmationNo = value;
                OnPropertyChanged();
            }
        }

        DateTime? _createdAt;
        public DateTime? CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt == value)
                    return;
                _createdAt = value;
                OnPropertyChanged();
            }
        }

        WorkOrderOperationConsumpDto[] _children = new WorkOrderOperationConsumpDto[1];
        public WorkOrderOperationConsumpDto[] Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }

        AntdUI.CellLink[] _btns;
        public AntdUI.CellLink[] Btns
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
