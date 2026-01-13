using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using DocumentFormat.OpenXml.Spreadsheet;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

namespace BizLink.MES.WinForms.Forms
{
    public partial class OrderScrapDeclarationForm : MesBaseForm
    {
        private readonly SapOrderModuleFacade _facade;
        private readonly ISapOrderScrapDeclarationService _sapOrderScrapDeclarationService;
        private SapOrderDto _currentOrder;
        private SapOrderOperation _sapOrderOperation;
        private const string Group_ScpScrapReason = "SCPScrapReason";
        private const string Group_ScpOtherScrap = "OTHER";
        public OrderScrapDeclarationForm(SapOrderModuleFacade facade, ISapOrderScrapDeclarationService sapOrderScrapDeclarationService)
        {

            InitializeComponent();
            InitializeTable();
            _facade = facade;
            _sapOrderScrapDeclarationService = sapOrderScrapDeclarationService;

        }

        private async void InitializeTable()
        {

            SapOrderTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("OrderNo", "订单/工序", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is SapOrderOperation model)
                            {
                                // 2. 自由组合多个字段
                                // 这里演示将 "订单号" 和 "工序" 拼接，中间换行
                                return model.OrderNo + "\r\n" + model.OperationNo;
        
                                // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                // return new CellText(model.OrderNo, model.Process); 
                            }
                            return value;
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialCode", "物料信息", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is SapOrderOperation model)
                            {
                                // 2. 自由组合多个字段
                                // 这里演示将 "订单号" 和 "工序" 拼接，中间换行
                                return $"订单物料：{model.MaterialCode}\r\n成品物料：{model.LeadingMaterial}\r\n塑料粒子：{model.ComponentMaterial}";

                                // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                // return new CellText(model.OrderNo, model.Process); 
                            }
                            return value;
                    }
                }.SetWidth("200").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("TargetQuantity", "数量(M)", AntdUI.ColumnAlign.Center).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenter", "工作中心/BU", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is SapOrderOperation model)
                            {
                                // 2. 自由组合多个字段
                                // 这里演示将 "订单号" 和 "工序" 拼接，中间换行
                                return model.WorkCenter + "\r\n" + model.ProfitCenter;
        
                                // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                // return new CellText(model.OrderNo, model.Process); 
                            }
                            return value;
                    }
                }.SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("OrderStatus", "状态", AntdUI.ColumnAlign.Center){
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "已填报" => new AntdUI.CellTag("已填报", AntdUI.TTypeMini.Success),
                            _ => new AntdUI.CellTag("未填报", AntdUI.TTypeMini.Error),
                        };
                    }
                }.SetFixed().SetLocalizationTitleID("Table.Column."),
            };


            SapBomTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("BomItem", "BOM行", AntdUI.ColumnAlign.Left)
                {
                    Width = "80",
                    KeyTree = "Children",
                    ReadOnly = true
                }.SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center)
                {
                    ReadOnly = true
                }.SetDisplayFormat("0").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center) { ReadOnly = true }.SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("RequireQuantity", "BOM用量", AntdUI.ColumnAlign.Center) { ReadOnly = true }.SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),

               new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center) { ReadOnly = true }.SetLocalizationTitleID("Table.Column."),

               new AntdUI.Column("ComponentScrap", "报废率(%)", AntdUI.ColumnAlign.Center) { ReadOnly = true }.SetLocalizationTitleID("Table.Column."),
               new AntdUI.Column("ScrapQuantityText", "报废数量", AntdUI.ColumnAlign.Center)
               .SetLocalizationTitleID("Table.Column."),
               //new AntdUI.ColumnSelect("ScrapBaseUnit", "单位", AntdUI.ColumnAlign.Center){
               //    Width = "80",
                   
               //    Items = new List<SelectItem>()
               //    {
               //      new SelectItem("KG"),
               //      new SelectItem("M")
               //    }
               // }.SetLocalizationTitleID("Table.Column."),
                       new AntdUI.Column("ScrapReason", "报废原因", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                       new AntdUI.Column("CreateBy", "创建人", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                       new AntdUI.Column("CreateOn", "创建时间", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                 new AntdUI.Column("Btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
            };


            ScrapInputTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("BomItem", "BOM行", AntdUI.ColumnAlign.Left)
                {
                    Width = "80",
                    KeyTree = "Children",
                    ReadOnly = true
                }.SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center)
                {
                    ReadOnly = true
                }.SetDisplayFormat("0").SetLocalizationTitleID("Table.Column."),



                new AntdUI.Column("RequireQuantity", "BOM用量", AntdUI.ColumnAlign.Center) { ReadOnly = true }.SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),



               new AntdUI.Column("ComponentScrap", "报废率(%)", AntdUI.ColumnAlign.Center) { ReadOnly = true }.SetLocalizationTitleID("Table.Column."),
               new AntdUI.Column("ScrapQuantityText", "报废数量", AntdUI.ColumnAlign.Center)
               .SetLocalizationTitleID("Table.Column."),
               new AntdUI.ColumnSelect("BaseUnit", "单位", AntdUI.ColumnAlign.Center) {
                   Width = "80",
                   Items = new List<SelectItem>()
                   {
                     new SelectItem("KG"),
                     new SelectItem("M")
                   }

               }.SetLocalizationTitleID("Table.Column."),
               //new AntdUI.ColumnSelect("ScrapBaseUnit", "单位", AntdUI.ColumnAlign.Center){
               //    Width = "80",
                   
               //    Items = new List<SelectItem>()
               //    {
               //      new SelectItem("KG"),
               //      new SelectItem("M")
               //    }
               // }.SetLocalizationTitleID("Table.Column."),
               new AntdUI.ColumnSelect("ScrapReason", "报废原因", AntdUI.ColumnAlign.Center)
               .SetWidth("300").SetLocalizationTitleID("Table.Column."),
               new AntdUI.Column("Remark", "备注", AntdUI.ColumnAlign.Center)
               {
                 Width = "150",
               }.SetLocalizationTitleID("Table.Column."),
               new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center) { ReadOnly = true }.SetLocalizationTitleID("Table.Column."),
            };

        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ScrapInputNumber.Text = string.Empty;
            DispatchDatePicker.PlaceholderText = "请选择排产日期...";
            WorkCenterSelectMultiple.PlaceholderText = "请选择工作中心...";
            WorkOrderInput.PlaceholderText = "请输入订单号...";
            BomSelect.PlaceholderText = "请选择BOM行...";
            ScrapReasonSelect.PlaceholderText = "请选择报废原因...";
            ScrapInputNumber.PlaceholderText = "请输入报废数量...";
            RemarkInput.PlaceholderText = "请输入备注信息...";
            await LoadWorkCenterAsync();
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SearchButton, async () =>
            {
                await LoadOrderDataAsync();
            });
        }

        private async Task LoadOrderDataAsync()
        {
            await RunAsync(async () =>
            {
                SapOrderTable.DataSource = null;
                _currentOrder = null;
                var factory = await _facade.FactoryService.GetByIdAsync(AppSession.CurrentFactoryId);
                var requestUrl = _facade.ApiSettings["MesApi"].Endpoints["GetCN10WorkOrders"];
                var request = new SapOrderRequest()
                {
                    FactoryCode = factory.FactoryCode,

                };

                var workorders = WorkOrderInput.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (workorders.Count > 0)
                {
                    request.OrderNos = workorders;
                }
                else
                {
                    if (DispatchDatePicker.Value == null)
                        throw new Exception("请先选择订单排产日期！");
                    var workcenters = WorkCenterSelectMultiple.SelectedValue.Select(x => ((MenuItem)x).Name).ToList();
                    if (workcenters == null)
                        throw new Exception("请先选择工作中心！");
                    request.DispatchDate = DispatchDatePicker.Value?.Date;
                    request.WorkCenterCode = workcenters;
                }
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                var result = await _facade.MesApi.PostAsync<object, SapOrderDto>(requestUrl, request);

                if (result.IsSuccess)
                {
                    _currentOrder = result.Data;
                    var scrapDeclarations = await _sapOrderScrapDeclarationService.GetListByOrderNosAsync(_currentOrder.sapOrderOperations.Select(x => x.OrderNo).Distinct().ToList());
                    var data = result.Data.sapOrderOperations.Where(w => result.Data.sapOrderBoms.Select(x => x.OrderNo).Contains(w.OrderNo) && result.Data.sapOrderBoms.Select(x => x.Operation).Contains(w.OperationNo)).ToList();

                    if (scrapDeclarations != null && scrapDeclarations.Any())
                    {
                        foreach (var op in data)
                        {
                            var opScrapDeclarations = scrapDeclarations.Where(sd => sd.WorkOrderNo == op.OrderNo && sd.OperationNo == op.OperationNo).ToList();
                            if (opScrapDeclarations != null && opScrapDeclarations.Any())
                            {
                                op.OrderStatus = "已填报";
                            }
                        }
                    }
                    SapOrderTable.DataSource = data;
                    AntdUI.Message.success(this, $"查询成功，共查询出{data.Count}笔订单");
                }
                else
                {
                    throw new Exception($"加载订单数据失败：{result.Message}");
                }
            });

        }

        private async Task LoadWorkCenterAsync()
        {
            await RunAsync(async () =>
            {
                var workcenters = await _facade.WorkCenter.GetAllAsync(AppSession.CurrentFactoryId);
                WorkCenterSelectMultiple.Items.Clear();
                WorkCenterSelectMultiple.Items.AddRange(workcenters.Select(wc => new MenuItem
                {
                    Name = wc.WorkCenterCode,
                    Text = $"{wc.WorkCenterCode}-{wc.WorkCenterName}"
                }).ToArray());
            });
        }

        private async Task LoadBomDataAsync(SapOrderOperation selectedOrder)
        {
            ScrapInputNumber.Value = 0;
            ScrapReasonSelect.SelectedValue = null;
            BomSelect.SelectedValue = null;
            _sapOrderOperation = selectedOrder;
            SapBomTable.DataSource = null;
            ScrapInputTable.DataSource = null;
            var boms = new List<SapOrderBom>();
            boms.Add(new SapOrderBom()
            {
                BomItem = "0000",
                MaterialCode = selectedOrder.MaterialCode,
            });
            boms.AddRange(_currentOrder.sapOrderBoms
                .Where(bom => bom.OrderNo == selectedOrder.OrderNo && bom.Operation == selectedOrder.OperationNo && bom.FixIndicator != "X").OrderBy(bom => bom.BomItem));
            var items = await _facade.Params.GetGroupWithItemsAsync(Group_ScpScrapReason);
            /*ScrapReasonSelect.Items.Clear();
            ScrapReasonSelect.Items.AddRange(items.Items.Select(i => new MenuItem()
            {
                Name = i.Key,
                Text = i.Name
            }).ToArray());

            BomSelect.Items.Clear();
            BomSelect.Items.AddRange(boms.Select(b => new MenuItem()
            {
                Name = $"{b.BomItem} {b.MaterialCode}",
                Text = $"{b.BomItem} {b.MaterialCode}"
            }).ToArray());*/

            var scrapInputView = boms.Select(x => new SapOrderBomView()
            {
                BomItem = x.BomItem,
                MaterialCode = x.MaterialCode,
                MaterialDesc = x.MaterialDesc,
                RequireQuantity = x.RequireQuantity,
                BaseUnit = x.BaseUnit,
                ComponentScrap = x.ComponentScrap

            }).ToList();

            ScrapInputTable.DataSource = scrapInputView;

            var columnSelect = (ColumnSelect)ScrapInputTable.Columns["ScrapReason"];
            columnSelect.Items = items.Items.Select(i => new SelectItem($"{i.Key}-{i.Name}")).ToList();
            var view = boms.Select(x => new SapOrderBomView()
            {
                BomItem = x.BomItem,
                MaterialCode = x.MaterialCode,
                MaterialDesc = x.MaterialDesc,
                RequireQuantity = x.RequireQuantity,
                BaseUnit = x.BaseUnit,
                ComponentScrap = x.ComponentScrap

            }).ToList();

            var scrapDeclarations = await _sapOrderScrapDeclarationService.GetListByOperationAsync(_sapOrderOperation.OrderNo, _sapOrderOperation.OperationNo);
            if (scrapDeclarations != null && scrapDeclarations.Any())
            {
                foreach (var viewItem in view)
                {
                    var matchs = scrapDeclarations.Where(s => s.ScrapMaterialCode == viewItem.MaterialCode && s.ScrapBomItem == viewItem.BomItem).ToList();
                    if (matchs != null)
                    {
                        viewItem.ScrapQuantity = matchs.Sum(x => x.ScrapQuantity);
                    }
                }
            }

            foreach (var item in view)
            {
                item.Children = scrapDeclarations
                .Where(s => s.ScrapBomItem == item.BomItem && s.ScrapMaterialCode == item.MaterialCode)
                .Select(s => new SapOrderDeclarationView
                {
                    Id = s.Id,
                    ScrapQuantity = s.ScrapQuantity,
                    ScrapReason = s.ScrapReason,
                    CreateBy = s.CreatedBy,
                    CreateOn = s.CreatedOn
                }).ToArray();
            }

            SapBomTable.DataSource = view;
            SapBomTable.ExpandAll();
        }

        private async void SapOrderTable_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is SapOrderOperation selectedOrder)
            {
                await LoadBomDataAsync(selectedOrder);
            }
        }

        private async void ResetButton_Click(object sender, EventArgs e)
        {
            await RunAsync(async () =>
            {
                //ScrapInputNumber.Value = 0;
                //ScrapReasonSelect.SelectedValue = null;
                //BomSelect.SelectedValue = null;
                LoadBomDataAsync(_sapOrderOperation);

            }, confirmMsg: "即将重置已填写的报废记录，是否继续？");
        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SubmitButton, async () =>
            {
                //    if(BomSelect.SelectedValue == null)
                //        throw new Exception("请先选择BOM行！");
                //    var bomSelect = ((MenuItem)BomSelect.SelectedValue).Text;
                //    if (string.IsNullOrEmpty(bomSelect))
                //        throw new Exception("请先选择BOM行！");
                //    if (ScrapReasonSelect.SelectedValue == null)
                //        throw new Exception("请先选择报废原因！");
                //    var scrapReason = (MenuItem)ScrapReasonSelect.SelectedValue;
                //    if (scrapReason == null)
                //        throw new Exception("请先选择报废原因！");
                //    var scrapQty = ScrapInputNumber.Value;
                //    if (scrapQty <= 0)
                //        throw new Exception("请先填写报废数量，且报废数量需大于0！");

                //var data = SapBomTable.DataSource as List<SapOrderBomView>;
                //var bomitem = data.Where(x => x.BomItem == bomSelect.Split(' ')[0] && x.MaterialCode == bomSelect.Split(' ')[1]).FirstOrDefault();
                //var result = await _sapOrderScrapDeclarationService.CreateAsync(new SapOrderScrapDeclarationCreateDto()
                //{
                //    FactoryCode = _sapOrderOperation.PlantCode,
                //    WorkOrderNo = _sapOrderOperation.OrderNo,
                //    OperationNo = _sapOrderOperation.OperationNo,
                //    MaterialCode = _sapOrderOperation.MaterialCode,
                //    WorkCenterCode = _sapOrderOperation.WorkCenter,
                //    SuperiorOrder = _sapOrderOperation.SuperiorOrder,
                //    LeadingOrder = _sapOrderOperation.LeadingOrder,
                //    LeadingMaterial = _sapOrderOperation.LeadingMaterial,
                //    ScrapBomItem = bomSelect.Split(' ')[0],
                //    ScrapMaterialType = bomSelect.Split(' ')[0] == "0000" ? "FINISHED_GOODS" : "RAW_MATERIAL",
                //    ScrapMaterialCode = bomSelect.Split(' ')[1],
                //    ScrapMaterialDesc = bomitem?.MaterialDesc,
                //    RequireQuantity = bomitem?.RequireQuantity,
                //    BaseUnit = bomitem?.BaseUnit,
                //    ComponentScrap = bomitem?.ComponentScrap,
                //    ScrapQuantity = scrapQty,
                //    ScrapReason = $"{scrapReason.Name}-{scrapReason.Text}",
                //    Remark = RemarkInput.Text ?? string.Empty,
                //    CreatedBy = AppSession.CurrentUser.EmployeeId
                //});
                //ScrapReasonSelect.SelectedValue = null;
                //ScrapInputNumber.Value = 0;
                //BomSelect.SelectedValue = null;
                //RemarkInput.Text = string.Empty;
                //ScrapInputNumber.Text = string.Empty;

                if (ScrapInputTable.DataSource == null)
                    throw new Exception($"未获取到当前订单[{_sapOrderOperation.OrderNo}]的BOM信息，请刷新后重试！");
                var data = ScrapInputTable.DataSource as List<SapOrderBomView>;
                var inputData = (ScrapInputTable.DataSource as List<SapOrderBomView>).Where(v => v.ScrapQuantity > 0 );
                if (inputData == null || inputData.Count() == 0)
                    throw new Exception("未获取到待报废的数量，请先填写报废数量！");

                foreach (var item in inputData)
                {
                    if (item.ScrapQuantity != null && item.ScrapQuantity < 0)
                        throw new Exception($"BOM行[{item.BomItem}]的报废数量不能小于0，请修改后重试！");
                    if(item.BaseUnit == null)
                        throw new Exception($"BOM行[{item.BomItem}]的报废单位不能为空，请选择后重试！");
                    if (item.ScrapReason == null)
                        throw new Exception($"BOM行[{item.BomItem}]的报废原因不能为空，请选择后重试！");
                    if (item.ScrapReason.Contains(Group_ScpOtherScrap) && string.IsNullOrWhiteSpace(item.Remark))
                        throw new Exception($"BOM行[{item.BomItem}]的报废原因为其他时，备注不能为空，请填写后重试！");
                }

                var result = await _sapOrderScrapDeclarationService.CreateBatchAsync(
                inputData.Select(i => new SapOrderScrapDeclarationCreateDto()
                {
                    FactoryCode = _sapOrderOperation.PlantCode,
                    WorkOrderNo = _sapOrderOperation.OrderNo,
                    OperationNo = _sapOrderOperation.OperationNo,
                    MaterialCode = _sapOrderOperation.MaterialCode,
                    WorkCenterCode = _sapOrderOperation.WorkCenter,
                    SuperiorOrder = _sapOrderOperation.SuperiorOrder,
                    LeadingOrder = _sapOrderOperation.LeadingOrder,
                    LeadingMaterial = _sapOrderOperation.LeadingMaterial,
                    ScrapBomItem = i.BomItem,
                    ScrapMaterialType = i.BomItem == "0000" ? "FINISHED_GOODS" : "RAW_MATERIAL",
                    ScrapMaterialCode = i.MaterialCode,
                    ScrapMaterialDesc = i.MaterialDesc,
                    RequireQuantity = i.RequireQuantity,
                    BaseUnit = i.BaseUnit,
                    ComponentScrap = i.ComponentScrap,
                    ScrapQuantity = i.ScrapQuantity,
                    ScrapReason = i.ScrapReason,
                    Remark = RemarkInput.Text ?? string.Empty,
                    CreatedBy = AppSession.CurrentUser.EmployeeId
                }).ToList());
                if (result == null)
                    throw new Exception($"保存报废记录失败！");
                LoadBomDataAsync(_sapOrderOperation);
        
                
                var orderdata = SapOrderTable.DataSource as List<SapOrderOperation>;
                var order = orderdata.Where(x => x.OrderNo == _sapOrderOperation.OrderNo && x.OperationNo == _sapOrderOperation.OperationNo).FirstOrDefault();
                order.OrderStatus = "已填报";
                SapOrderTable.Refresh();


            }, confirmMsg: "即将保存当前填写的报废，是否继续？", successMsg: "保存成功！");
        }

        private async void SapBomTable_ExpandChanged(object sender, TableExpandEventArgs e)
        {
            if (e.Expand && e.Record is SapOrderBomView data)
            {
                var scrapDeclarations = await _sapOrderScrapDeclarationService.GetListByOperationAsync(_sapOrderOperation.OrderNo, _sapOrderOperation.OperationNo);

                data.Children = scrapDeclarations
                    .Where(s => s.ScrapBomItem == data.BomItem && s.ScrapMaterialCode == data.MaterialCode)
                    .Select(s => new SapOrderDeclarationView
                    {
                        Id = s.Id,
                        ScrapQuantity = s.ScrapQuantity,
                        ScrapReason = s.ScrapReason,
                        CreateBy = s.CreatedBy,
                        CreateOn = s.CreatedOn
                    }).ToArray();
            }
        }

        private async void SapBomTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is SapOrderDeclarationView item && e.Btn.Id == "delete")
            {
                await RunAsync(async () =>
                {
                    var result = await _sapOrderScrapDeclarationService.UpdateAsync(new SapOrderScrapDeclarationUpdateDto() 
                    {
                        Id = item.Id,
                        IsActive = false,
                        UpdatedBy = AppSession.CurrentUser.EmployeeId,
                        UpdatedOn = DateTime.Now

                    });
                    if (!result)
                        throw new Exception("删除报废记录失败！");
                    LoadBomDataAsync(_sapOrderOperation);
                }, confirmMsg: "即将删除该报废填报记录，是否继续？", successMsg: "删除报废填报记录成功！");
            }
        }
    }

    public class SapOrderBomView : AntdUI.NotifyProperty
    {

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

        decimal? _requireQuantity;
        public decimal? RequireQuantity
        {
            get => _requireQuantity;
            set
            {
                if (_requireQuantity == value)
                    return;
                _requireQuantity = value;
                OnPropertyChanged();
            }
        }

        string _baseUnit;
        public string BaseUnit
        {
            get => _baseUnit;
            set
            {
                if (_baseUnit == value)
                    return;
                _baseUnit = value;
                OnPropertyChanged();
            }
        }

        decimal? _componentScrap;
        public decimal? ComponentScrap
        {
            get => _componentScrap;
            set
            {
                if (_componentScrap == value)
                    return;
                _componentScrap = value;
                OnPropertyChanged();
            }
        }

        string _bomItem;
        public string BomItem
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

        int? _reservationItem;
        public int? ReservationItem
        {
            get => _reservationItem;
            set
            {
                if (_reservationItem == value)
                    return;
                _reservationItem = value;
                OnPropertyChanged();
            }
        }

        int? _reservationNo;
        public int? ReservationNo
        {
            get => _reservationNo;
            set
            {
                if (_reservationNo == value)
                    return;
                _reservationNo = value;
                OnPropertyChanged();
            }
        }

        string _orderNo;
        public string OrderNo
        {
            get => _orderNo;
            set
            {
                if (_orderNo == value)
                    return;
                _orderNo = value;
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



        decimal? _scrapQuantity;
        public decimal? ScrapQuantity
        {
            get => _scrapQuantity;
            set
            {
                if (_scrapQuantity == value)
                    return;
                _scrapQuantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ScrapQuantityText));
            }
        }

        // --- 核心：专供 Table 绑定的字符串代理属性 ---
        public string ScrapQuantityText
        {
            get => _scrapQuantity?.ToString();
            set
            {
                if (decimal.TryParse(value, out decimal result))
                {
                    _scrapQuantity = result;
                }
                else
                {
                    _scrapQuantity = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(ScrapQuantity));
            }
        }

        string? _scrapBaseUnit;
        public string? ScrapBaseUnit
        {
            get => _scrapBaseUnit;
            set
            {
                if (_scrapBaseUnit == value)
                    return;
                _scrapBaseUnit = value;
                OnPropertyChanged();
            }
        }

        string? _scrapReason;
        public string? ScrapReason
        {
            get => _scrapReason;
            set
            {
                if (_scrapReason == value)
                    return;
                _scrapReason = value;
                OnPropertyChanged();
            }
        }

        string? _remark;
        public string? Remark
        {
            get => _remark;
            set
            {
                if (_remark == value)
                    return;
                _remark = value;
                OnPropertyChanged();
            }
        }

        string? _createBy;
        public string? CreateBy
        {
            get => _createBy;
            set
            {
                if (_createBy == value)
                    return;
                _createBy = value;
                OnPropertyChanged();
            }
        }

        DateTime? _createOn;
        public DateTime? CreateOn
        {
            get => _createOn;
            set
            {
                if (_createOn == value)
                    return;
                _createOn = value;
                OnPropertyChanged();
            }
        }

        SapOrderDeclarationView[] _children = new SapOrderDeclarationView[1];
        public SapOrderDeclarationView[] Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }


    }

    public class SapOrderDeclarationView : AntdUI.NotifyProperty
    {

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

        decimal? _requireQuantity;
        public decimal? RequireQuantity
        {
            get => _requireQuantity;
            set
            {
                if (_requireQuantity == value)
                    return;
                _requireQuantity = value;
                OnPropertyChanged();
            }
        }

        string _baseUnit;
        public string BaseUnit
        {
            get => _baseUnit;
            set
            {
                if (_baseUnit == value)
                    return;
                _baseUnit = value;
                OnPropertyChanged();
            }
        }

        decimal? _componentScrap;
        public decimal? ComponentScrap
        {
            get => _componentScrap;
            set
            {
                if (_componentScrap == value)
                    return;
                _componentScrap = value;
                OnPropertyChanged();
            }
        }

        string _bomItem;
        public string BomItem
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

        int? _reservationItem;
        public int? ReservationItem
        {
            get => _reservationItem;
            set
            {
                if (_reservationItem == value)
                    return;
                _reservationItem = value;
                OnPropertyChanged();
            }
        }

        int? _reservationNo;
        public int? ReservationNo
        {
            get => _reservationNo;
            set
            {
                if (_reservationNo == value)
                    return;
                _reservationNo = value;
                OnPropertyChanged();
            }
        }

        string _orderNo;
        public string OrderNo
        {
            get => _orderNo;
            set
            {
                if (_orderNo == value)
                    return;
                _orderNo = value;
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



        decimal? _scrapQuantity;
        public decimal? ScrapQuantity
        {
            get => _scrapQuantity;
            set
            {
                if (_scrapQuantity == value)
                    return;
                _scrapQuantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ScrapQuantityText));
            }
        }

        // --- 核心：专供 Table 绑定的字符串代理属性 ---
        public string? ScrapQuantityText
        {
            get => _scrapQuantity?.ToString();
            set
            {
                if (decimal.TryParse(value, out decimal result))
                {
                    _scrapQuantity = result;
                }
                else
                {
                    _scrapQuantity = null;
                }
                OnPropertyChanged();
                OnPropertyChanged(nameof(ScrapQuantity));
            }
        }

        string? _scrapBaseUnit;
        public string? ScrapBaseUnit
        {
            get => _scrapBaseUnit;
            set
            {
                if (_scrapBaseUnit == value)
                    return;
                _scrapBaseUnit = value;
                OnPropertyChanged();
            }
        }

        SelectItem? _scrapReasonSelect;
        public SelectItem? ScrapReasonSelect
        {
            get => _scrapReasonSelect;
            set
            {
                if (_scrapReasonSelect == value)
                    return;
                _scrapReasonSelect = value;
                OnPropertyChanged();
            }
        }

        string? _scrapReason;
        public string? ScrapReason
        {
            get => _scrapReason;
            set
            {
                if (_scrapReason == value)
                    return;
                _scrapReason = value;
                OnPropertyChanged();
            }
        }

        string? _remark;
        public string? Remark
        {
            get => _remark;
            set
            {
                if (_remark == value)
                    return;
                _remark = value;
                OnPropertyChanged();
            }
        }

        string? _createBy;
        public string? CreateBy
        {
            get => _createBy;
            set
            {
                if (_createBy == value)
                    return;
                _createBy = value;
                OnPropertyChanged();
            }
        }

        DateTime? _createOn;
        public DateTime? CreateOn
        {
            get => _createOn;
            set
            {
                if (_createOn == value)
                    return;
                _createOn = value;
                OnPropertyChanged();
            }
        }
        AntdUI.CellLink[] _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("delete", "删除", AntdUI.TTypeMini.Primary),
                    };
        public AntdUI.CellLink[] Btns
        {
            get => _btns;
            set
            {
                if (_btns == value)
                    return;
                _btns = value;
                OnPropertyChanged();
            }
        }

    }

}
