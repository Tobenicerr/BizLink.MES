using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Infrastructure;
using DocumentFormat.OpenXml.Drawing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms.WebReportForm
{
    public partial class AssemblyOrderProgressReportForm : MesBaseForm
    {
        private bool _isProgrammaticPageChange = false;
        private readonly IFactoryService _factoryService;
        private readonly IAssemblyOrderProgressService _assemblyOrderProgressService;
        private readonly IWorkCenterGroupService _workCenterGroupService;
        private readonly IWorkCenterService _workCenterService;
        private readonly IWorkOrderOperationConfirmService _workOrderOperationConfirmService;
        private readonly IUserService _userService;

        public AssemblyOrderProgressReportForm(IFactoryService factoryService, IAssemblyOrderProgressService assemblyOrderProgressService, IWorkCenterGroupService workCenterGroupService, IWorkCenterService workCenterService, IWorkOrderOperationConfirmService workOrderOperationConfirmService, IUserService userService)
        {
            InitializeComponent();
            InitializeTable();
            _factoryService = factoryService;
            _assemblyOrderProgressService = assemblyOrderProgressService;
            _workCenterGroupService = workCenterGroupService;
            _workCenterService = workCenterService;
            _workOrderOperationConfirmService = workOrderOperationConfirmService;
            _userService = userService;
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DispatchDatePicker.PlaceholderText = "请选择排产日期";
            DispatchDatePickerRange.PlaceholderStart = "排产日期开始";
            DispatchDatePickerRange.PlaceholderEnd = "排产日期结束";
            ConfirmDatePickerRange.PlaceholderStart = "报工日期开始";
            ConfirmDatePickerRange.PlaceholderEnd = "报工日期结束";
            WorkCenterGroupSelect.PlaceholderText = "请选择工作中心组";
            WorkCenterSelect.PlaceholderText = "请选择工作中心";
            WorkOrderInput.PlaceholderText = "请输入订单号";
            WorkCenterGroupSelect.Items.Clear();
            var workcentergroups = await _workCenterGroupService.GetAllAsync();
            if (workcentergroups != null && workcentergroups.Count() > 0)
            {
                WorkCenterGroupSelect.Items.AddRange(workcentergroups.Select(g => new AntdUI.MenuItem()
                {
                    Name = g.Id.ToString(),
                    Text = g.GroupName
                }).ToArray());
            }

        }
        private void InitializeTable()
        {
            TableControl.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("OrderNumber", "订单号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "订单物料", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Operation", "工序序号", AntdUI.ColumnAlign.Center).SetFixed().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenter", "工作中心", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                     
                new AntdUI.Column("Quantity", "订单数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CompletedQuantity", "完成数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MachineTime", "标准工时", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkingTime", "运行工时", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),


                new AntdUI.Column("DispatchDate", "计划完成日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConfirmedQuantity", "已确认数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("UnConfirmedQuantity", "未确认数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),


                new AntdUI.Column("ConfirmDate", "报工时间", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("FactoryCode", "工厂代码", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "Mes状态", AntdUI.ColumnAlign.Center) {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "未开始" => new AntdUI.CellTag("未开始", AntdUI.TTypeMini.Error),
                            "执行中" => new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Primary),
                            "已完成" => new AntdUI.CellTag("已完成", AntdUI.TTypeMini.Success),
                            "超量完成" => new AntdUI.CellTag("超量完成", AntdUI.TTypeMini.Error),
                            _ => new AntdUI.CellTag("未开始", AntdUI.TTypeMini.Error)
                        };
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("SapStatus", "Sap状态", AntdUI.ColumnAlign.Center) {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "未确认" => new AntdUI.CellTag("未确认", AntdUI.TTypeMini.Error),
                            "已确认" => new AntdUI.CellTag("已确认", AntdUI.TTypeMini.Success),
                            "超量确认" => new AntdUI.CellTag("超量确认", AntdUI.TTypeMini.Error),

                            _ => new AntdUI.CellTag("未确认", AntdUI.TTypeMini.Error)
                        };
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

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
            DateTime? dispatchdateStart = null;
            DateTime? dispatchdateEnd = null;
            if (DispatchDatePicker.Value != null)
            {
                dispatchdateStart = DispatchDatePicker.Value.Value.Date;
                dispatchdateEnd = DispatchDatePicker.Value.Value.Date.AddDays(1).AddSeconds(-1);
            }
            //DateTime? dispatchdateStart = DispatchDatePickerRange.Value != null && DispatchDatePickerRange.Value[0] != null ? DispatchDatePickerRange.Value[0] : null;
            //DateTime? dispatchdateEnd = DispatchDatePickerRange.Value != null && DispatchDatePickerRange.Value[1] != null ? DispatchDatePickerRange.Value[1] : null;
            DateTime? confirmDateStart = ConfirmDatePickerRange.Value != null && ConfirmDatePickerRange.Value[0] != null ? ConfirmDatePickerRange.Value[0] : null;
            DateTime? confirmDateEnd = ConfirmDatePickerRange.Value != null && ConfirmDatePickerRange.Value[1] != null ? ConfirmDatePickerRange.Value[1] : null;
            List<string> workcenters = new List<string>();

            if (WorkCenterSelect.SelectedValue != null)
            {
                workcenters.Add(((AntdUI.MenuItem)WorkCenterSelect.SelectedValue).Name);
            }
            else
            {
                if (WorkCenterGroupSelect.SelectedValue != null && int.TryParse(((AntdUI.MenuItem)WorkCenterGroupSelect.SelectedValue).Name, out int selectedGroupId))
                {
                    var workcentersInGroup = await _workCenterService.GetBygroupIdAsync(selectedGroupId);
                    if (workcentersInGroup != null && workcentersInGroup.Count() > 0)
                    {
                        workcenters.AddRange(workcentersInGroup.Select(w => w.WorkCenterCode));
                    }
                }
            }
            List<string> workorders = WorkOrderInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var factory = await _factoryService.GetByIdAsync(AppSession.CurrentFactoryId);
            var result = await _assemblyOrderProgressService.GetPageListAsync(pageIndex, pageSize, factory.FactoryCode, workorders, workcenters, dispatchdateStart, dispatchdateEnd, confirmDateStart, confirmDateEnd);
            if (result != null)
            {
                TableControl.DataSource = result.Items.Select(x => new AssemblyOrderProgressReportView(x)).ToList();
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

        private async void WorkCenterGroupSelect_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (WorkCenterGroupSelect.SelectedValue != null && int.TryParse(((AntdUI.MenuItem)WorkCenterGroupSelect.SelectedValue).Name, out int selectedGroupId))
            {
                var workcenters = await _workCenterService.GetBygroupIdAsync(selectedGroupId);
                if (workcenters != null && workcenters.Count() > 0)
                {
                    WorkCenterSelect.Items.Clear();
                    WorkCenterSelect.Items.AddRange(workcenters.Select(w => new AntdUI.MenuItem()
                    {
                        Name = w.WorkCenterCode,
                        Text = $"{w.WorkCenterCode}-{w.WorkCenterDesc}"
                    }).ToArray());
                }

            }
        }

        private async void ExportButton_Click(object sender, EventArgs e)
        {
            await RunAsync(ExportButton, async () =>
            {
                var data = TableControl.DataSource as List<AssemblyOrderProgressReportView>;
                if (data != null && data.Count() > 0)
                {

                    var opConfirms = await _workOrderOperationConfirmService.GetListByProcessIdAsync(data.Select(x => (int)x.Id).ToList());
                    var users = await _userService.GetByEmployeeIdAsync(opConfirms.Where(c => c.EmployeeId != null).Select(c => c.EmployeeId).Distinct().ToList());
                    var result = data.GroupJoin(opConfirms, p => p.Id, c => c.ProcessId, (p, c) => new { p, c })
                    .SelectMany(temp => temp.c.DefaultIfEmpty(),(temp,confirm) => new { temp.p,confirm})
                    .GroupJoin(users, x => x.confirm.EmployeeId, u => u.EmployeeId, (x, u) => new { x.p, x.confirm, user = u })
                    .SelectMany(x => x.user.DefaultIfEmpty(), (x, user) => new
                    {
                        
                        订单号 = x.p.OrderNumber,
                        物料号 = x.p.MaterialCode,
                        物料描述 = x.p.MaterialDesc,
                        工序序号 = x.p.Operation,
                        工作中心 = x.p.WorkCenter,
                        计划完成日期 = x.p.DispatchDate,

                        MES状态 = x.p.Status,
                        SAP状态 = x.p.SapStatus,

                        计划数量 = x.p.Quantity,
                        完成数量 = x.p.CompletedQuantity,
                        标准工时 = x.p.MachineTime,
                        运行工时 = x.p.WorkingTime,

                        SAP扫描总数量 = x.p.ConfirmedQuantity,
                        SAP未扫描总数量 = x.p.UnConfirmedQuantity,
                        报工人员 = $"{x.confirm?.EmployeeId}-{user.UserName}",
                        报工数量 = x.confirm?.YieldQuantity,
                        员工工时 = Math.Round((x.p.MachineTime * x.confirm?.YieldQuantity / x.p.Quantity)??0,3),
                        报工日期 = x.confirm?.CreatedAt,
                        工厂 = x.p.FactoryCode,
                        BU = x.p.ProfitCenter,

                    }).ToList();

                    ExcelExportHelper.ExportToExcel(this.ParentForm, result, $"装配报工记录{DateTime.Now:yyyyMMdd}");
                }
                else
                    throw new Exception("当前无数据可导出，请先执行查询！");


            }, successMsg: "导出完成！", confirmMsg: "是否导出装配报工记录信息？");
        }
    }

    public class AssemblyOrderProgressReportView : AntdUI.NotifyProperty
    {
        public AssemblyOrderProgressReportView(AssemblyOrderProgressDto dto) 
        {
            _id = dto.Id;
            _factoryCode = dto.FactoryCode;
            _profitCenter = dto.ProfitCenter;
            _orderNumber = dto.OrderNumber;
            _materialCode = dto.MaterialCode;
            _materialDesc = dto.MaterialDesc;
            _operation = dto.Operation;
            _dispatchDate = dto.DispatchDate;
            _superiorOrder = dto.SuperiorOrder;
            _leadingOrder = dto.LeadingOrder;
            _endTime = dto.EndTime;
            _status = dto.CompletedQuantity == 0 ? "未开始" : (dto.Quantity == dto.CompletedQuantity ? "已完成" : (dto.Quantity < dto.CompletedQuantity ? "超量完成" : "执行中"));
            _sapStatus = dto.Quantity ==  dto.ConfirmedQuantity ? "已确认" : (dto.Quantity < dto.ConfirmedQuantity ? "超量确认" : "未确认");
            _quantity = dto.Quantity;
            _completedQuantity = dto.CompletedQuantity;
            _machineTime = dto.MachineTime;
            _workingTime = dto.WorkingTime;
            _workCenter = dto.WorkCenter;
            _confirmedQuantity = dto.ConfirmedQuantity;
            _unConfirmedQuantity = dto.UnConfirmedQuantity;
            _confirmDate = dto.ConfirmDate;

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

        string? _superiorOrder;
        public string? SuperiorOrder
        {
            get => _superiorOrder;
            set
            {
                if (_superiorOrder == value)
                    return;
                _superiorOrder = value;
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

        string? _sapStatus;
        public string? SapStatus
        {
            get => _sapStatus;
            set
            {
                if (_sapStatus == value)
                    return;
                _sapStatus = value;
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

        decimal? _completedQuantity;
        public decimal? CompletedQuantity
        {
            get => _completedQuantity;
            set
            {
                if (_completedQuantity == value)
                    return;
                _completedQuantity = value;
                OnPropertyChanged();
            }
        }

        decimal? _machineTime;
        public decimal? MachineTime
        {
            get => _machineTime;
            set
            {
                if (_machineTime == value)
                    return;
                _machineTime = value;
                OnPropertyChanged();
            }
        }

        decimal? _workingTime;
        public decimal? WorkingTime
        {
            get => _workingTime;
            set
            {
                if (_workingTime == value)
                    return;
                _workingTime = value;
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
        }

        decimal? _confirmedQuantity;
        public decimal? ConfirmedQuantity
        {
            get => _confirmedQuantity;
            set
            {
                if (_confirmedQuantity == value)
                    return;
                _confirmedQuantity = value;
                OnPropertyChanged();
            }
        }

        decimal? _unConfirmedQuantity;
        public decimal? UnConfirmedQuantity
        {
            get => _unConfirmedQuantity;
            set
            {
                if (_unConfirmedQuantity == value)
                    return;
                _unConfirmedQuantity = value;
                OnPropertyChanged();
            }
        }

        DateTime? _confirmDate;
        public DateTime? ConfirmDate
        {
            get => _confirmDate;
            set
            {
                if (_confirmDate == value)
                    return;
                _confirmDate = value;
                OnPropertyChanged();
            }
        }
    }
}
