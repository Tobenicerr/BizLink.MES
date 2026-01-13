using AntdUI;
using Azure;
using Azure.Core;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.Shared.Helpers;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Infrastructure;
using Dm;
using Dm.util;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SqlSugar;
using SqlSugar.SplitTableExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Npgsql.Replication.PgOutput.Messages.RelationMessage;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BizLink.MES.WinForms.Forms
{
    public partial class WorkOrderManagementForm : MesBaseForm
    {
        //private readonly IWorkOrderViewService _workOrderViewService;
        //private readonly IWorkOrderProcessViewService _workOrderProcessViewService;
        //private readonly IWorkOrderBomViewService _workOrderBomViewService;

        //private readonly IWorkOrderService _workOrderService;
        //private readonly IWorkOrderViewService _workOrderViewService;

        //private readonly IWorkOrderProcessService _workOrderProcessService;
        //private readonly IWorkOrderBomItemService _workOrderBomService;
        //private readonly IJyApiClient _jyApiClient;
        //private readonly IMaterialViewService _materialViewService;
        //private readonly IParameterGroupService _parameterGroupService;
        //private readonly IRawLinesideStockService _rawLinesideStockService;
        //private readonly IRawMaterialInventoryViewService _rawMaterialInventoryViewService;
        //private readonly Dictionary<string, ServiceEndpointSettings> _apiSettings;
        //private readonly IWorkCenterService _workCenterService;
        //private readonly IWarehouseLocationService _warehouseLocationService;
        //private readonly IWorkOrderTaskConsumService _workOrderTaskConsumService;

        private const string Group_BagMaterialList = "BagMaterialList";
        private readonly WorkOrderModuleFacade _facade;
        private readonly IFormFactory _formFactory;

        // 2. 构造函数：只注入 Facade 和 Factory
        public WorkOrderManagementForm(
            WorkOrderModuleFacade facade,
            IFormFactory formFactory)
        {
            _facade = facade;
            _formFactory = formFactory;

            InitializeComponent();
            InitializeTable();
        }

        // 3. 初始化加载
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (AppSession.CurrentFactoryId <= 0)
            {
                AntdUI.Message.error(this, "请先选择工厂！");
                // 这种严重错误可以考虑直接关闭窗体或禁用
                this.Enabled = false;
            }

            orderListInput.PlaceholderText = "请输入订单号...";
            dispatchDatePicker.PlaceholderText = "请选择装配日期...";
            startDatePicker.PlaceholderText = "请选择开工日期...";
        }


        #region Data Loading

        // --- 查询主表 ---
        private async void QueryButton_Click(object sender, EventArgs e)
        {
            await RunAsync(QueryButton, async () =>
            {
                await LoadDataAsync();
            });
        }

        private async Task LoadDataAsync()
        {
            // 清空详情表
            processTable.DataSource = null;
            bomTable.DataSource = null;

            var orders = orderListInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var result = await _facade.WorkOrderService.GetListByDispatchDateAsync(
                dispatchDatePicker.Value,
                startDatePicker.Value,
                orders,
                AppSession.CurrentFactoryId);

            if (result != null && result.Any())
            {
                orderTable.DataSource = result.Select(x => new WorkOrderView(x)).ToList();
                AntdUI.Message.success(this, $"查询成功：共有{result.Count}笔订单");
            }
            else
            {
                orderTable.DataSource = null;
                throw new Exception("未查询到记录！");
            }
        }

        // --- 加载详情 (点击行) ---
        private async void orderTable_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.RowIndex >= 0 && e.Record is WorkOrderView data)
            {
                // 使用 RunAsync 自动处理 Loading，不需要手动 toggle Enabled
                // 传入 null 作为 triggerBtn 因为点击的是表格行
                await RunAsync(async () =>
                {
                    await LoadDetailDataAsync(data.Id);
                });
            }
        }

        private async Task LoadDetailDataAsync(int orderId)
        {
            //// 并行加载工艺和BOM
            //var processTask = _facade.WorkOrderProcessService.GetListByOrderIdAync(orderId);
            //var bomTask = _facade.WorkOrderBomItemService.GetListByOrderIdAync(orderId);

            //await Task.WhenAll(processTask, bomTask);

            var processes = await _facade.WorkOrderProcessService.GetListByOrderIdAync(orderId);
            var boms = await _facade.WorkOrderBomItemService.GetListByOrderIdAync(orderId);

            if (processes != null)
            {
                processTable.DataSource = processes.Select(x => new WorkOrderProcessView(x)).ToList();
            }

            if (boms != null)
            {
                bomTable.DataSource = boms
                    .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true)
                    .Select(x => new WorkOrderBomView(x))
                    .ToList();
            }
        }

        #endregion

        #region Operations

        // --- 领料申请 ---
        private async void pickOrderRequistButton_Click(object sender, EventArgs e)
        {
            await RunAsync(pickOrderRequistButton, async () =>
            {
                // 1. 校验
                if (startDatePicker.Value == null)
                    throw new Exception("请先选择开工日期！");
                if (orderTable.DataSource == null)
                    throw new Exception("未查询到可以领料的订单！");

                var data = (orderTable.DataSource as List<WorkOrderView>).Where(x => x.check).ToList();
                if (!data.Any())
                    throw new Exception("未选中订单，请先选择需要领料的订单！");

                // 2. 确认
                if (AntdUI.Modal.open(this.ParentForm, "按单拣配物料领料", "即将向WMS推送按单拣配物料领料需求，是否继续？") != DialogResult.OK)
                    return;

                // 3. 执行 (逻辑已下沉到 Facade)
                requistProgress.Visible = true;
                requistProgress.Value = 0;
                var sb = new StringBuilder();
                int count = 0;

                foreach (var view in data)
                {
                    var orderDto = new WorkOrderDto { Id = view.Id, OrderNumber = view.OrderNumber };
                    // 调用 Facade 封装好的业务逻辑
                    var msg = await _facade.ExecuteMaterialRequisitionAsync(orderDto, AppSession.CurrentUser);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        sb.AppendLine(msg);
                    }

                    requistProgress.Value = (float)++count / data.Count;
                }

                sb.AppendLine("按单领料推送处理完成！");

                // 4. 结果反馈
                AntdUI.Modal.open(new AntdUI.Modal.Config(this.ParentForm, "处理结果", new AntdUI.Input
                {
                    Text = sb.ToString(),
                    Width = 350,
                    Height = 300,
                    BorderWidth = 0,
                    Multiline = true,
                    ReadOnly = true
                }, AntdUI.TType.Info));

            }, successMsg: null); // 成功消息已在内部通过 Modal 展示

            // Finally 块中的逻辑由 RunAsync 处理，这里只需重置 Progress
            requistProgress.Visible = false;
        }

        // --- 流转卡打印 ---
        private async void processcardPrintButton_Click(object sender, EventArgs e)
        {
            await RunAsync(processcardPrintButton, async () =>
            {
                // 1. 校验
                if (orderTable.DataSource == null)
                    throw new Exception("未查询到可以打印的订单！");
                var data = (orderTable.DataSource as List<WorkOrderView>).Where(x => x.check).OrderBy(x => x.OrderNumber).ToList();
                if (!data.Any())
                    throw new Exception("未选中订单！");

                // 2. 选择打印机
                string chosenPrinter = null;
                // 使用 Factory 打开打印机选择窗口
                _formFactory.Show<PrinterSelectForm>(form =>
                {
                    form.FormClosed += (s, args) =>
                    {
                        if (form.DialogResult == DialogResult.OK)
                            chosenPrinter = form.SelectedPrinterName;
                    };
                }, isModal: true);

                if (string.IsNullOrWhiteSpace(chosenPrinter))
                    throw new Exception("打印已取消或未选择打印机！");

                // 3. 执行打印
                var parts = chosenPrinter.Split('-');
                var printType = (parts.Length > 1 && parts[1] == PrinterType.NetworkPrinter.GetDescription())
                    ? PrinterType.NetworkPrinter : PrinterType.LocalPrinter;
                var printerName = parts[0];

                foreach (var item in data)
                {
                    var order = await _facade.WorkOrderService.GetByIdAsync(item.Id);
                    var processes = await _facade.WorkOrderProcessService.GetListByOrderIdAync(order.Id);
                    if (!processes.Any())
                        throw new Exception($"订单{item.OrderNumber}无工序信息！");

                    //// 逻辑：判断是否超领/袋装 (这里可以封装到 Helper 或 Facade)
                    //var boms = (await _facade.WorkOrderBomItemService.GetListByOrderIdAync(order.Id))
                    //           .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true).ToList();

                    string flag = await DetermineMaterialFlagAsync(order.Id);

                    var firstProcess = processes.OrderBy(x => x.Operation).First();
                    string locCode = "";
                    if (!string.IsNullOrWhiteSpace(firstProcess.NextWorkCenter))
                    {
                        var wc = await _facade.WorkCenter.GetByCodeAsync(firstProcess.NextWorkCenter);
                        if (wc?.LineStockId != null)
                        {
                            var loc = await _facade.Location.GetByIdAsync((int)wc.LineStockId);
                            locCode = loc?.Name ?? "";
                        }
                    }

                    var rtn = LabelPrintHelper.ProcessCardPrinter(order, firstProcess, flag, locCode, printType, printerName);
                    if (!rtn)
                        throw new Exception($"订单{item.OrderNumber}打印失败");

                    // 更新打印次数
                    await _facade.WorkOrderProcessService.UpdateAsync(new WorkOrderProcessUpdateDto
                    {
                        Id = firstProcess.Id,
                        WorkOrderId = firstProcess.WorkOrderId,
                        ProcessCardPrintCount = firstProcess.ProcessCardPrintCount + 1
                    });
                }

            }, successMsg: "打印成功！");
        }

        private async Task<string> DetermineMaterialFlagAsync(int orderId)
        {
            var boms = (await _facade.WorkOrderBomItemService.GetListByOrderIdAync(orderId))
                .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true).ToList();

            if (!boms.Any()||boms.Where(x => x.ConsumeType != null && (new List<int> { 0,2}).Contains((int)x.ConsumeType)).Count() == 0)
                return "空";

            if (boms.Any(x => x.ConsumeType == 2))
            {
                if (boms.Any(x => x.MaterialCode.StartsWith('E') && x.ConsumeType == 2))
                    return "超";

                if (!boms.Any(x => x.ConsumeType == 2 && !x.MaterialCode.StartsWith('8')))
                {
                    var bagParams = await _facade.Params.GetGroupWithItemsAsync(Group_BagMaterialList);
                    var bagMaterials = bagParams.Items.FirstOrDefault()?.Value.Split(',').ToList() ?? new List<string>();
                    var bomMaterials = boms.Where(x => x.ConsumeType == 2).Select(x => x.MaterialCode).ToList();

                    if (!bomMaterials.Except(bagMaterials).Any())
                        return "袋";
                }
            }
            return string.Empty;
        }

        // --- 领料申请详情 (子窗体) ---
        private void materialRequistButton_Click(object sender, EventArgs e)
        {
            if (startDatePicker.Value == null)
            {
                AntdUI.Message.error(this, "请先选择开工日期！");
                return;
            }

            // 使用 Factory 打开 Drawer
            // 注意：MaterialRequisitionForm 需要重构以支持 InitData，或者直接注入 Facade
            _formFactory.OpenDrawer<MaterialRequisitionForm>(this.ParentForm, form =>
            {
                // 假设您已经重构了 MaterialRequisitionForm
                form.InitData(startDatePicker.Value);

                // 临时兼容：如果还没重构，可能需要保留旧的构造函数，但 Factory CreateInstance 要求构造函数能被 DI 解析
                // 建议尽快重构 MaterialRequisitionForm
            });
        }

        #endregion

        private void InitializeTable()
        {
            orderTable.Columns = new AntdUI.ColumnCollection
            {
               new AntdUI.Column("Id", "Id", AntdUI.ColumnAlign.Center)
               {
                    Visible = false
                },

                new AntdUI.ColumnCheck("check").SetFixed(),
                new AntdUI.Column("OrderNumber", "订单号", AntdUI.ColumnAlign.Center).SetFixed().SetLocalizationTitleID("Table.Column.").SetDefaultFilter(),
                new AntdUI.Column("MaterialCode", "物料代码", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column.").SetDefaultFilter(),

                new AntdUI.Column("StartDate", "开工日期", AntdUI.ColumnAlign.Center)
                {
                  LocalizationTitle = "Table.Column.StartDate",
                  DisplayFormat = "yyyy-MM-dd"
                }.SetDefaultFilter(typeof(DateTime)),
                new AntdUI.Column("DispatchDate", "排产日期", AntdUI.ColumnAlign.Center)
                {
                  LocalizationTitle = "Table.Column.DispatchDate",
                  DisplayFormat = "yyyy-MM-dd"
                }.SetDefaultFilter(typeof(DateTime)),

                new AntdUI.Column("FactoryCode", "所属工厂", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "数量", AntdUI.ColumnAlign.Center).SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        string statusText = value as string;
                        return CellTagHelper.BuildWorkOrderStatusTag(statusText);
                    }
                }.SetLocalizationTitleID("Table.Column.").SetDefaultFilter(),
                //{
                //    LocalizationTitle ="Table.Column.{id}",
                //    Call = (value, record, i_row, i_col) => {
                //        System.Threading.Thread.Sleep(2000);
                //        return value;
                //    }
                //}
                //,
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("LeadingOrder", "成品订单", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LeadingMaterial", "成品物料", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LabelCount", "标签数量", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CableCount", "电缆根数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetDefaultFilter(),
                new AntdUI.Column("ProcessCardPrintCount", "流转卡打印次数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetDefaultFilter(),
                new AntdUI.Column("PlannerRemark", "计划备注", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto")
            };
            bomTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("WorkOrderNo", "订单号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Operation", "工序序号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("BomItem", "BOM行号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialCode", "物料代码", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("RequiredQty", "需求数量", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column.").SetDisplayFormat("F3"),

                new AntdUI.Column("ConsumeType", "物料属性", AntdUI.ColumnAlign.Center)  {
                    Render = (value, record, index) =>
                    {
                        string text = value as string;
                        return CellTagHelper.BuildConsumeTypeTag(text);
                    }
                }.SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Unit", "单位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column.")
            };
            processTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("WorkOrder", "订单号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Operation", "工序序号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)  {
                    Render = (value, record, index) =>
                    {
                        string statusText = value as string;
                        return CellTagHelper.BuildWorkOrderStatusTag(statusText);
                    }
                }.SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenter", "工作中心", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenterName", "工作中心名称", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("StartTime", "计划开始", AntdUI.ColumnAlign.Center)
                {
                  LocalizationTitle = "Table.Column.StartTime",
                  DisplayFormat = "yyyy-MM-dd"
                },
                new AntdUI.Column("EndTime", "计划结束", AntdUI.ColumnAlign.Center)                {
                  LocalizationTitle = "Table.Column.EndTime",
                  DisplayFormat = "yyyy-MM-dd"
                },
                new AntdUI.Column("ActStartTime", "实际开始", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ActEndTime", "实际结束", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                //new AntdUI.Column("NextWorkCenter", "下一工作中心", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column.")

            };
        }

        private  async void orderTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is WorkOrderView data && e.Btn.Id == "delete")
            {
                await RunAsync(async () =>
                {
                    await _facade.WorkOrderService.UpdateAsync(new WorkOrderUpdateDto()
                    {
                        Id = data.Id,
                        Status = ((int)WorkOrderStatus.Deleted).ToString(),
                        UpdateBy = AppSession.CurrentUser.EmployeeId,
                        UpdateOn = DateTime.Now
                    });
                }, successMsg: $"订单 {data.OrderNumber} 删除成功！", confirmMsg: $"确认删除订单 {data.OrderNumber} 吗？");
            }

        }
    }
    public class WorkOrderProcessView : AntdUI.NotifyProperty
    {
        public WorkOrderProcessView(WorkOrderProcessDto process)
        {
            _workOrderId = (int)process.WorkOrderId;
            _workOrder = process.WorkOrderNo;
            _operation = process.Operation;
            _operationId = process.Id;
            _workCenter = process.WorkCenter;
            _workCenterName = process.WorkCenter;
            _status = process.Status ?? "0";
            _startTime = process.StartTime;
            _endTime = process.EndTime;
            _actStartTime = process.ActStartTime;
            _actEndTime = process.ActEndTime;
        }


        int _workOrderId;
        public int WorkOrderId
        {
            get => _workOrderId;
            set
            {
                if (_workOrderId == value)
                    return;
                _workOrderId = value;
                OnPropertyChanged();
            }
        }

        string? _workOrder;
        public string? WorkOrder
        {
            get => _workOrder;
            set
            {
                if (_workOrder == value)
                    return;
                _workOrder = value;
                OnPropertyChanged();
            }
        }

        string _operation;
        public string Operation
        {
            get => _operation;
            set
            {
                if (_operation == value)
                    return;
                _operation = value;
                OnPropertyChanged();
            }
        }

        int _operationId;
        public int OperationId
        {
            get => _operationId;
            set
            {
                if (_operationId == value)
                    return;
                _operationId = value;
                OnPropertyChanged();
            }
        }

        string? _workCenter;
        public string? WorkCenter
        {
            get => _workCenter;
            set
            {
                if (_workCenter == value)
                    return;
                _workCenter = value;
                OnPropertyChanged();
            }
        } // 工作中心

        string? _workCenterName;
        public string? WorkCenterName
        {
            get => _workCenterName;
            set
            {
                if (_workCenterName == value)
                    return;
                _workCenterName = value;
                OnPropertyChanged();
            }
        } // 工作中心

        string _status;
        public string Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;
                _status = value;
                OnPropertyChanged();
            }
        } // 工序状态

        DateTime? _startTime;
        public DateTime? StartTime
        {
            get => _startTime;
            set
            {
                if (_startTime == value)
                    return;
                _startTime = value;
                OnPropertyChanged();
            }
        } // 计划开始时间

        DateTime? _endTime;
        public DateTime? EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime == value)
                    return;
                _endTime = value;
                OnPropertyChanged();
            }
        } // 计划结束时间

        DateTime? _actStartTime;
        public DateTime? ActStartTime
        {
            get => _actStartTime;
            set
            {
                if (_actStartTime == value)
                    return;
                _actStartTime = value;
                OnPropertyChanged();
            }
        } // 实际开始时间

        DateTime? _actEndTime;
        public DateTime? ActEndTime
        {
            get => _actEndTime;
            set
            {
                if (_actEndTime == value)
                    return;
                _actEndTime = value;
                OnPropertyChanged();
            }
        } // 实际结束时间

        public static CellTag BuildStatusTag(string status)
        {
            switch (status)
            {
                case "0":
                    return new AntdUI.CellTag("未开工", AntdUI.TTypeMini.Error);

                case "1":
                    return new AntdUI.CellTag("已排产", AntdUI.TTypeMini.Default);

                case "2":
                    return new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Info);

                case "3":
                    return new AntdUI.CellTag("已挂起", AntdUI.TTypeMini.Warn);

                case "4":
                    return new AntdUI.CellTag("已完成", AntdUI.TTypeMini.Success);

                default:
                    return new AntdUI.CellTag("未知", AntdUI.TTypeMini.Default);
            }
        }
    }
    public class WorkOrderView : AntdUI.NotifyProperty
    {
        public WorkOrderView(WorkOrderDto entity)
        {
            _check = false;
            _id = entity.Id;
            _factoryCode = entity.FactoryCode;
            _orderNumber = entity.OrderNumber;
            _materialCode = entity.MaterialCode;
            _materialDesc = entity.MaterialDesc;
            _quantity = entity.Quantity;
            _dispatchDate = entity.DispatchDate;
            _startDate = entity.ScheduledStartDate;
            _status = entity.Status ?? "0";
            _plannerRemark = entity.PlannerRemark;
            _profitCenter = entity.ProfitCenter;
            _leadingOrder = entity.LeadingOrder;
            _leadingMaterial = entity.LeadingOrderMaterial;
            _labelCount = entity.LabelCount;
            _cableCount = entity.CableCount;
            _processCardPrintCount = entity.ProcessCardPrintCount;
            _btns = new AntdUI.CellLink[] {
                new AntdUI.CellButton("delete", "删除", AntdUI.TTypeMini.Error)
            };
        }
        bool _check = false;
        public bool check
        {
            get => _check;
            set
            {
                if (_check == value)
                    return;
                _check = value;
                OnPropertyChanged();
            }
        }
        int _id;
        public int Id
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

        int? _cableCount;
        public int? CableCount
        {
            get => _cableCount;
            set
            {
                if (_cableCount == value)
                    return;
                _cableCount = value;
                OnPropertyChanged();
            }
        }

        int? _processCardPrintCount;
        public int? ProcessCardPrintCount
        {
            get => _processCardPrintCount;
            set
            {
                if (_processCardPrintCount == value)
                    return;
                _processCardPrintCount = value;
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
        string? _orderNumber;
        public string? OrderNumber
        {
            get => _orderNumber;
            set
            {
                if (_orderNumber == value)
                    return;
                _orderNumber = value;
                OnPropertyChanged();
            }
        }
        string? _materialCode;
        public string? MaterialCode
        {
            get => _materialCode;
            set
            {
                if (_materialCode == value)
                    return;
                _materialCode = value;
                OnPropertyChanged();
            }
        }
        string? _materialDesc;
        public string? MaterialDesc
        {
            get => _materialDesc;
            set
            {
                if (_materialDesc == value)
                    return;
                _materialDesc = value;
                OnPropertyChanged();
            }
        }
        decimal? _quantity;
        public decimal? Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                    return;
                _quantity = value;
                OnPropertyChanged();
            }
        }
        DateTime? _dispatchDate;
        public DateTime? DispatchDate
        {
            get => _dispatchDate;
            set
            {
                if (_dispatchDate == value)
                    return;
                _dispatchDate = value;
                OnPropertyChanged();
            }
        }

        DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate == value)
                    return;
                _startDate = value;
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
        string? _plannerRemark;
        public string? PlannerRemark
        {
            get => _plannerRemark;
            set
            {
                if (_plannerRemark == value)
                    return;
                _plannerRemark = value;
                OnPropertyChanged();
            }
        }
        string? _profitCenter;
        public string? ProfitCenter
        {
            get => _profitCenter;
            set
            {
                if (_profitCenter == value)
                    return;
                _profitCenter = value;
                OnPropertyChanged();
            }
        }

        string? _leadingOrder;
        public string? LeadingOrder
        {
            get => _leadingOrder;
            set
            {
                if (_leadingOrder == value)
                    return;
                _leadingOrder = value;
                OnPropertyChanged();
            }
        }
        string? _leadingMaterial;
        public string? LeadingMaterial
        {
            get => _leadingMaterial;
            set
            {
                if (_leadingMaterial == value)
                    return;
                _leadingMaterial = value;
                OnPropertyChanged();
            }
        }


        int? _labelCount;
        public int? LabelCount
        {
            get => _labelCount;
            set
            {
                if (_labelCount == value)
                    return;
                _labelCount = value;
                OnPropertyChanged();
            }
        }

        AntdUI.CellLink[] _btns;
        public AntdUI.CellLink[] btns
        {
            get => _btns;
            set
            {
                _btns = value;
                OnPropertyChanged();
            }
        }
    }
    public class WorkOrderBomView : AntdUI.NotifyProperty
    {
        public WorkOrderBomView(WorkOrderBomItemDto dto)
        {
            _id = dto.Id;
            _workOrderNo = dto.WorkOrderNo;
            _operation = dto.Operation;
            _materialCode = dto.MaterialCode;
            _materialDesc = dto.MaterialDesc;
            _requiredQty = dto.RequiredQuantity;
            _bomItem = dto.BomItem;
            _processId = dto.WorkOrderProcessId;
            _unit = dto.Unit;
            _allowMovement = dto.MovementAllowed;
            _consumeType = dto.ConsumeType == null ? string.Empty : ((ConsumeType)dto.ConsumeType).GetDescription();
            _syncWms = (int)dto.SyncWMSStatus;
        }

        int _syncWms;
        public int SyncWms
        {
            get => _syncWms;
            set
            {
                if (_syncWms == value)
                    return;
                _syncWms = value;
                OnPropertyChanged();
            }
        }
        string _consumeType;
        public string ConsumeType
        {
            get => _consumeType;
            set
            {
                if (_consumeType == value)
                    return;
                _consumeType = value;
                OnPropertyChanged();
            }
        }

        bool? _allowMovement;
        public bool? AllowMovement
        {
            get => _allowMovement;
            set
            {
                if (_allowMovement == value)
                    return;
                _allowMovement = value;
                OnPropertyChanged();
            }
        }

        string? _unit;
        public string? Unit
        {
            get => _unit;
            set
            {
                if (_unit == value)
                    return;
                _unit = value;
                OnPropertyChanged();
            }
        }

        int _processId;
        public int ProcessId
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

        string? _bomItem;
        public string? BomItem
        {
            get => _bomItem;
            set
            {
                if (_bomItem == value)
                    return;
                _bomItem = value;
                OnPropertyChanged();
            }
        }

        string? _materialCode;
        public string? MaterialCode
        {
            get => _materialCode;
            set
            {
                if (_materialCode == value)
                    return;
                _materialCode = value;
                OnPropertyChanged();
            }
        }
        decimal? _requiredQty;
        public decimal? RequiredQty
        {
            get => _requiredQty;
            set
            {
                if (_requiredQty == value)
                    return;
                _requiredQty = value;
                OnPropertyChanged();
            }
        }

        string? _materialDesc;
        public string? MaterialDesc
        {
            get => _materialDesc;
            set
            {
                if (_materialDesc == value)
                    return;
                _materialDesc = value;
                OnPropertyChanged();
            }
        }

        int _id;
        public int Id
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

        string? _operation;
        public string? Operation
        {
            get => _operation;
            set
            {
                if (_operation == value)
                    return;
                _operation = value;
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

    }
}
