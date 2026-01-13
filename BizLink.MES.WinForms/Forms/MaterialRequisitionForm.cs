using AntdUI;
using Azure;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Infrastructure;
using Dm.util;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices.Protocols;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using static BizLink.MES.Application.Facade.WorkOrderModuleFacade;

namespace BizLink.MES.WinForms.Forms
{
    public partial class MaterialRequisitionForm : MesBaseForm
    {
        //private readonly DateTime? _dispatchdate;
        ////private readonly List<WorkOrderDto> _workOrders;
        //private readonly IJyApiClient _jyApiClient;
        //private readonly IWorkOrderService _workOrderService;
        //private readonly IWorkOrderBomItemService _workOrderBomService;
        //private readonly IWorkOrderProcessService _workOrderProcessService;
        //private readonly IMaterialViewService _materialViewService;
        //private readonly IRawLinesideStockService _rawLinesideStockService;
        //private readonly IRawMaterialInventoryViewService _rawMaterialInventoryViewService;
        //private readonly IWorkOrderViewService _workOrderViewService;
        //private readonly IWorkOrderTaskConsumService _workOrderTaskConsumService;


        //private readonly Dictionary<string, ServiceEndpointSettings> _apiSettings;


        //private List<WorkOrderBomItemDto> _bomItems = new List<WorkOrderBomItemDto>();
        //private List<WorkOrderProcessDto> _processes = new List<WorkOrderProcessDto>();




        //private List<MaterialRequisitionView> _materialRequisitionViews = new List<MaterialRequisitionView>();



        private readonly WorkOrderModuleFacade _facade;

        private DateTime? _dispatchDate;
        private List<WorkOrderBomItemDto> _bomItems = new List<WorkOrderBomItemDto>();
        private List<WorkOrderProcessDto> _processes = new List<WorkOrderProcessDto>();
        private List<MaterialRequisitionView> _materialRequisitionViews = new List<MaterialRequisitionView>();

        // 2. 构造函数：只注入 Facade
        public MaterialRequisitionForm(WorkOrderModuleFacade facade)
        {
            _facade = facade;
            InitializeComponent();
            InitializeTable();
        }

        // 3. 初始化数据入口
        public void InitData(DateTime? dispatchDate)
        {
            _dispatchDate = dispatchDate;
        }

        // 4. 窗体加载
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_dispatchDate == null)
                return;

            await RunAsync(async () =>
            {
                SpinControl.Visible = true;
                // 加载基础数据
                _bomItems = (await _facade.WorkOrderInProgressView.GetOngoingCableTaskListByDateAsync(AppSession.CurrentFactoryId, _dispatchDate.Value)).Select(x => new WorkOrderBomItemDto() 
                {
                    WorkOrderId = x.OrderId,
                    WorkOrderProcessId = x.OrderProcessId,
                    Operation = x.Operation,
                    MaterialCode = x.CableMaterial.TrimStart('0'),
                    RequiredQuantity = x.RequiredQuantity,
                    ConsumeType = 0
                }).ToList();
                _processes = await _facade.WorkOrderProcessService.GetListByOrderIdsAync(_bomItems.Select(x => x.WorkOrderId).ToList());

                // 补全物料ID
                var materials = await _facade.MaterialView.GetListByCodesAsync(AppSession.CurrentUser.FactoryName, _bomItems.Select(x => x.MaterialCode).ToList());
                var materialDic = materials.ToDictionary(x => x.MaterialCode);

                foreach (var item in _bomItems)
                {
                    if (materialDic.TryGetValue(item.MaterialCode, out var materialdto))
                    {
                        item.MaterialId = materialdto.Id;
                        item.MaterialDesc = materialdto.MaterialName;
                        item.Unit = materialdto.BaseUnit;
                    }
                }

                await LoadDataAsync();
            });
        }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {


            try
            {
                dispatchLabel.Text = _dispatchDate.Value.ToString("MM-dd") + " 领料计划"; // 假设原本 dispatchLabel.Text 包含后缀文本

                var boms = _bomItems.Where(b => b.ConsumeType == 0).ToList();

                var workcentergroup = _processes.Join(boms,
                    o => new { WorkOrderId = (int)o.WorkOrderId, o.Operation },
                    b => new { b.WorkOrderId, b.Operation },
                    (process, bom) => new
                    {
                        Process = process,
                        Bom = bom
                    });

                //var taskConsumes = await _facade.WorkOrderInProgressView.GetListByProcessIdAsync(_processes.Select(x => (int)x.Id).ToList());

                //// 汇总已消耗量
                //var consumedLookup = taskConsumes.Where(x => x.Status != ((int)WorkOrderStatus.Finished).ToString() && !string.IsNullOrWhiteSpace(x.CableMaterial) && x.StartTime < _dispatchDate)
                //    .GroupBy(c => c.CableMaterial.TrimStart('0'))
                //    .ToDictionary(g => g.Key, g => g.Sum(c => c.CableLengthUsl ?? 0 * c.Quantity ?? 0));

                // 计算需求
                var requisition = workcentergroup
                    .GroupBy(m => new
                    {
                        m.Bom.Operation,
                        m.Bom.MaterialId,
                        MaterialCode = m.Bom.MaterialCode.TrimStart('0'),
                        m.Bom.MaterialDesc,
                        m.Bom.Unit,
                        m.Bom.ConsumeType,
                    }).Select(m => new
                    {
                        Operation = m.Key.Operation,
                        MaterialId = m.Key.MaterialId,
                        MaterialCode = m.Key.MaterialCode,
                        MaterialDesc = m.Key.MaterialDesc,
                        BaseUnit = m.Key.Unit,
                        ConsumeType = m.Key.ConsumeType,
                        SyncWMSStatus = m.Max(s => s.Bom.SyncWMSStatus),
                        ReqQuantity = (decimal)m.Sum(s => s.Bom.RequiredQuantity)
                    }).ToList();

                // 计算最大工作中心
                var maxworkcenter = workcentergroup.GroupBy(s => new { s.Bom.MaterialCode, s.Process.WorkCenter })
                    .Select(g => new { g.Key.MaterialCode, g.Key.WorkCenter, ReqQty = g.Sum(s => s.Bom.RequiredQuantity) })
                    .GroupBy(x => x.MaterialCode)
                    .Select(g => g.OrderByDescending(s => s.ReqQty).First())
                    .ToList();

                var maxworkcenterTrimmed = maxworkcenter
                    .Select(m => new { TrimmedMaterialCode = m.MaterialCode.TrimStart('0'), m.WorkCenter })
                    .ToList();

                // 合并结果
                _materialRequisitionViews = requisition.GroupJoin(
                    maxworkcenterTrimmed,
                    x => x.MaterialCode,
                    y => y.TrimmedMaterialCode,
                    (x, y) => new { requisitions = x, group = y }
                )
                .SelectMany(
                    t => t.group.DefaultIfEmpty(),
                    (t, y) => new MaterialRequisitionView
                    {
                        Operation = t.requisitions.Operation,
                        MaterialId = t.requisitions.MaterialId,
                        MaterialCode = t.requisitions.MaterialCode,
                        MaterialDesc = t.requisitions.MaterialDesc,
                        ReqQuantity = t.requisitions.ReqQuantity,
                        BaseUnit = t.requisitions.BaseUnit,
                        ConsumeType = "电缆", // 注意类型转换
                        SyncWmsStatus = t.requisitions.SyncWMSStatus == 0 ? "未推送" : "已推送",
                        ConsumeMaxWorkCenter = y?.WorkCenter
                    }).ToList();

                materialGroupTable.DataSource = _materialRequisitionViews;

                //// 初始化筛选下拉框
                //consumetypeSelect.Items.Clear();
                //consumetypeSelect.Items.AddRange(_materialRequisitionViews.GroupBy(s => s.ConsumeType).Select(x => new MenuItem
                //{
                //    Text = x.Key
                //}).ToArray());

                //// 默认选中 "电缆" (如果存在)
                //if (_materialRequisitionViews.Any(x => x.ConsumeType == "电缆"))
                //{
                //    consumetypeSelect.SelectedValue = "电缆";
                //}
                //// 触发一次筛选
                //FilterData();
            }
            catch (Exception)
            {

                throw;
            }
            finally 
            {
                if (SpinControl.Visible == true)
                    SpinControl.Visible = false;
            }

      
        }

        private void FilterData()
        {
            var filter = consumetypeSelect.Text;
            if (string.IsNullOrEmpty(filter))
            {
                materialGroupTable.DataSource = _materialRequisitionViews;
            }
            else
            {
                materialGroupTable.DataSource = _materialRequisitionViews.Where(x => (x.ConsumeType ?? string.Empty) == filter).ToList();
            }
        }

        private async void pushButton_Click(object sender, EventArgs e)
        {
            await RunAsync(pushButton, async () =>
            {
                pushProgress.Visible = true;
                pushProgress.Value = 0;
                var message = new StringBuilder();

                // 1. 校验
                if (!_materialRequisitionViews.Any(x => x.check))
                    throw new Exception("请至少选择一条需要推送的物料需求记录!");

                var data = _materialRequisitionViews.Where(x => x.ConsumeType == ConsumeType.CableMaterial.GetDescription() && x.check).ToList();

                if (!data.Any())
                    throw new Exception("没有可推送的电缆物料需求，请确认!");

                // 2. 确认
                if (AntdUI.Modal.open(this, "提示", "即将向WMS推送领料需求，是否继续？", TType.Warn) != DialogResult.OK)
                    return;

                // 3. 执行推送逻辑
                // A. 扣减线边库库存
                foreach (var item in data)
                {
                    var stocks = await _facade.RawStock.GetListByMaterialCodeAsync(AppSession.CurrentFactoryId, item.MaterialCode);
                    var totalStock = stocks.Where(x => x.LastQuantity > 0).Sum(l => l.LastQuantity);
                    if (totalStock > 0)
                    {
                        item.ReqQuantity = item.ReqQuantity - totalStock;
                    }
                }

                // B. 更新WMS任务状态
                await _facade.WorkOrderView.UpdateJyWmsRequrementTaskAsync();

                // C. 推送 WMS
                var requestUrl = _facade.ApiSettings["JyApi"].Endpoints["TransvouchCreate"];

                var entryDtos = data.Where(x => x.ReqQuantity > 0)
                    .GroupBy(x => x.MaterialId)
                    .Select(g => new MaterialEntry
                    {
                        MaterialId = (int)g.Key,
                        Qty = (decimal)g.Sum(x => x.ReqQuantity)
                    }).ToList();

                var request = new MaterialRequisitionRequest
                {
                    VoucherTypeCategoryCode = "VTC022",
                    VoucherTypeName = "999",
                    OWarehouseCode = "1100",
                    IWarehouseCode = "2100",
                    OStorageCode = "11004",
                    IStorageCode = "21001",
                    VirtualOPlantCode = AppSession.CurrentUser.FactoryName,
                    VirtualIPlantCode = AppSession.CurrentUser.FactoryName,
                    EntryDtos = entryDtos,
                    PlantCode = AppSession.CurrentUser.FactoryName,
                    IsActive = false,
                    OperateUser = AppSession.CurrentUser.EmployeeId,
                    OperateUserName = AppSession.CurrentUser.UserName,
                    Remark = "断线"
                };

                var result = await _facade.JyApi.PostAsync<MaterialRequisitionRequest, object>(requestUrl, request);

                if (!result.IsSuccess)
                {
                    message.AppendLine("电缆领料推送失败：" + result.Message);
                }
                else
                {
                    // D. 更新本地 BOM 状态
                    var materialsToUpdate = data.Select(d => d.MaterialCode).ToHashSet();
                    var updateDtos = _bomItems
                        .Where(x => materialsToUpdate.Contains(x.MaterialCode))
                        .Select(x => new WorkOrderBomItemUpdateDto
                        {
                            Id = x.Id,
                            SyncWMSStatus = x.SyncWMSStatus + 1,
                            UpdateOn = DateTime.Now,
                            UpdateBy = AppSession.CurrentUser.EmployeeId
                        }).ToList();

                    await _facade.WorkOrderBomItemService.UpdateWmsStatusAsync(updateDtos);
                    message.AppendLine("电缆领料推送成功!");
                }

                // 显示结果
                AntdUI.Modal.open(new AntdUI.Modal.Config(this, "提示", new AntdUI.Input
                {
                    Text = message.ToString(),
                    Width = 350,
                    Height = 300,
                    BorderWidth = 0,
                    Multiline = true,
                    ReadOnly = true
                }, AntdUI.TType.Info));

            }, successMsg: null); // 结果已通过 Modal 展示

            // Finally 逻辑
            pushProgress.Visible = false;
            pushProgress.Value = 0;
        }

        private void consumetypeSelect_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            FilterData();
        }

        private void backPicture_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InitializeTable()
        {

            materialGroupTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnCheck("check").SetFixed(),
                new AntdUI.Column("Operation", "工序", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConsumeMaxWorkCenter", "工作中心", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ReqQuantity", "需求数量", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("SyncWmsStatus", "是否推送WMS", AntdUI.ColumnAlign.Center){
                    Render = (value, record, index) =>
                    {
                        string text = value as string;
                        return text == "已推送" ? new AntdUI.CellTag("已推送", AntdUI.TTypeMini.Success) : new AntdUI.CellTag("未推送", AntdUI.TTypeMini.Error);
                    }
                }.SetLocalizationTitleID("Table.Column.").SetDefaultFilter(),
                new AntdUI.Column("BaseUnit", "物料单位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConsumeType", "物料属性", AntdUI.ColumnAlign.Center){
                    Render = (value, record, index) =>
                    {
                        string text = value as string;
                        return CellTagHelper.BuildConsumeTypeTag(text);
                    }
                }.SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                //{
                //    LocalizationTitle ="Table.Column.{id}",
                //    Call = (value, record, i_row, i_col) => {
                //        System.Threading.Thread.Sleep(2000);
                //        return value;
                //    }
                //}
                
             };
        }

        //public MaterialRequisitionForm(DateTime? dispatchdate,IJyApiClient jyApiClient, IWorkOrderService workOrderService , IWorkOrderBomItemService workOrderBomService, IWorkOrderProcessService workOrderProcessService, IMaterialViewService materialViewService, IRawLinesideStockService rawLinesideStockService, IRawMaterialInventoryViewService rawMaterialInventoryViewService, Dictionary<string, ServiceEndpointSettings> apiSettings, IWorkOrderViewService workOrderViewService, IWorkOrderTaskConsumService workOrderTaskConsumService)
        //{
        //    InitializeComponent();
        //    InitializeTable();

        //    _dispatchdate = dispatchdate;
        //    _jyApiClient = jyApiClient;
        //    _workOrderService = workOrderService;
        //    _workOrderBomService = workOrderBomService;

        //    _workOrderProcessService = workOrderProcessService;
        //    _materialViewService = materialViewService;
        //    _rawLinesideStockService = rawLinesideStockService;
        //    _rawMaterialInventoryViewService = rawMaterialInventoryViewService;
        //    _apiSettings = apiSettings;
        //    _workOrderViewService = workOrderViewService;
        //    _workOrderTaskConsumService = workOrderTaskConsumService;
        //}

        //private void backPicture_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}



        //private async void MaterialRequisitionForm_Load(object sender, EventArgs e)
        //{
        //    SpinControl.Visible = true;


        //    _bomItems = await _workOrderBomService.GetOngoingCableListByDateAsync(AppSession.CurrentFactoryId, (DateTime)_dispatchdate);
        //    _processes = await _workOrderProcessService.GetListByOrderIdsAync(_bomItems.Select(x => x.WorkOrderId).ToList());

        //    //_bomItems = await _workOrderBomService.GetOngoingCableListByProcessIdsAsync(_processes.Select(x => (int)x.WorkOrderId).ToList());

        //    var materials = await _materialViewService.GetListByCodesAsync(AppSession.CurrentUser.FactoryName, _bomItems.Select(x => x.MaterialCode).ToList());

        //    var materialDic = materials.ToDictionary(x => x.MaterialCode);

        //    foreach (var item in _bomItems)
        //    {
        //        if (materialDic.TryGetValue(item.MaterialCode, out MaterialViewDto materialdto))
        //        {
        //            // 如果找到了，就用源对象的值来更新目标对象的属性
        //            item.MaterialId = materialdto.Id;
        //        }
        //    }

        //    await LoadData();

        //    SpinControl.Visible = false;
        //}

        //private async System.Threading.Tasks.Task LoadData()
        //{
        //    try
        //    {

        //        dispatchLabel.Text = ((DateTime)_dispatchdate).ToString("MM-dd") + dispatchLabel.Text;
        //        var boms = _bomItems.Where(b => b.ConsumeType == 0).ToList();
        //        var workcentergroup = _processes.Join(boms,
        //        o => new { WorkOrderId = (int)o.WorkOrderId, o.Operation },
        //        b => new { b.WorkOrderId, b.Operation },
        //        (process, bom) => new
        //        {
        //            Process = process, // 包含 WorkCenter
        //            Bom = bom         // 包含物料信息
        //        });

        //        var taskConsumes = await _workOrderTaskConsumService.GetCableListByProcessIdsAsync(_processes.Select(x => (int)x.WorkOrderId).ToList());

        //        // (新) 将已消耗列表转换为字典，以便 O(1) 快速查找
        //        // 假设 taskConsumes 列表项 包含 MaterialCode 和 ConsumedQuantity
        //        var consumedLookup = taskConsumes
        //            .GroupBy(c => c.MaterialCode.TrimStart('0')) // 确保 Key 与 requisition 匹配
        //            .ToDictionary(
        //                g => g.Key,
        //                g => g.Sum(c => (decimal)c.EntryQuantity) // 按物料汇总已消耗量
        //            );

        //        // 4. 按物料分组，聚合需求量 (生成领料单的基础数据)
        //        // ！！！ 修复：数据源现在是 workcentergroup，而不是 boms ！！！
        //        var requisition = workcentergroup
        //            .GroupBy(m => new
        //            {
        //                m.Bom.Operation,
        //                m.Bom.MaterialId,
        //                MaterialCode = m.Bom.MaterialCode.TrimStart('0'), // 清理 Key
        //                m.Bom.MaterialDesc,
        //                //m.Bom.RequiredQuantity,
        //                m.Bom.Unit,
        //                m.Bom.ConsumeType,
        //                //m.Bom.SyncWMSStatus
        //            }).Select(m =>
        //            {
        //                // (新) 计算逻辑
        //                decimal totalRequired = (decimal)m.Sum(s => s.Bom.RequiredQuantity);
        //                // (新) 查找已消耗量，如果找不到则默认为 0
        //                consumedLookup.TryGetValue(m.Key.MaterialCode, out decimal totalConsumed);
        //                return new
        //                {
        //                    Operation = m.Key.Operation,
        //                    MaterialId = m.Key.MaterialId,
        //                    MaterialCode = m.Key.MaterialCode.TrimStart('0'),
        //                    MaterialDesc = m.Key.MaterialDesc,
        //                    BaseUnit = m.Key.Unit,
        //                    ConsumeType = m.Key.ConsumeType,
        //                    SyncWMSStatus = m.Max(s => s.Bom.SyncWMSStatus),
        //                    ReqQuantity = totalRequired - totalConsumed < 0 ? 0 : totalRequired - totalConsumed// (新) 净需求量
        //                };
        //            }).Where(x => x.ReqQuantity > 0).ToList();






        //        //new { Operation = m.Key.Operation, MaterialId = m.Key.MaterialId, MaterialCode = m.Key.MaterialCode.TrimStart('0'), MaterialDesc = m.Key.MaterialDesc, BaseUnit = m.Key.Unit, ConsumeType = m.Key.ConsumeType,
        //        //        SyncWMSStatus = m.Max(s => s.Bom.SyncWMSStatus),
        //        //        ReqQuantity = m.Sum(s => s.Bom.RequiredQuantity) // <--- 正确汇总
        //        //    }).ToList();

        //        //var requisition = boms.GroupBy(m => new { m.Operation, m.MaterialId, m.MaterialCode, m.MaterialDesc, m.Unit, m.ConsumeType, m.SyncWMSStatus }).Select(m => new { Operation = m.Key.Operation, MaterialId = m.Key.MaterialId, MaterialCode = m.Key.MaterialCode.TrimStart('0'), MaterialDesc = m.Key.MaterialDesc, BaseUnit = m.Key.Unit, ConsumeType = m.Key.ConsumeType, m.Key.SyncWMSStatus, ReqQuantity = m.Sum(s => s.RequiredQuantity) }).ToList();

        //        //var workcentergroup = _processes.Join(boms, o => o.Id, b => b.WorkOrderProcessId, (x, y) => new { x.WorkOrderId, x.WorkOrderNo, x.Operation, x.WorkCenter, y.MaterialCode, y.RequiredQuantity });
        //        // 5. (已修复) 计算最大工作中心 (数据源也是 workcentergroup)
        //        var maxworkcenter = workcentergroup.GroupBy(s => new 
        //        { s.Bom.MaterialCode, s.Process.WorkCenter })
        //            .Select(g => new { g.Key.MaterialCode, g.Key.WorkCenter, ReqQty = g.Sum(s => s.Bom.RequiredQuantity) })
        //            .GroupBy(x => x.MaterialCode)
        //            .Select(g => g.OrderByDescending(s => s.ReqQty).First())
        //            .ToList();

        //        // (新) 预处理 maxworkcenter 的 Key 以进行高效连接
        //        var maxworkcenterTrimmed = maxworkcenter
        //            .Select(m => new
        //            {
        //                TrimmedMaterialCode = m.MaterialCode.TrimStart('0'),
        //                m.WorkCenter
        //            })
        //            .ToList();

        //        // 6. (已修复) 将 "最大需求量工作中心" (maxworkcenter) 左连接 (GroupJoin) 回 "领料单" (requisition)
        //        _materialRequisitionViews = requisition.GroupJoin(
        //            maxworkcenterTrimmed, // 使用清理过的 Key 列表
        //            x => x.MaterialCode,      // requisition 的 Key 已清理
        //            y => y.TrimmedMaterialCode, // maxworkcenter 的 Key 也已清理
        //            (x, y) => new { requisitions = x, group = y }
        //        )
        //        .SelectMany(
        //            t => t.group.DefaultIfEmpty(),
        //            (t, y) => new // 临时的匿名对象
        //            {
        //                // --- 修复: 移除了 Operation ---
        //                t.requisitions.Operation,
        //                t.requisitions.MaterialId,
        //                t.requisitions.MaterialCode,
        //                t.requisitions.MaterialDesc,
        //                t.requisitions.ReqQuantity, // 这是新的净需求量
        //                t.requisitions.BaseUnit,
        //                t.requisitions.ConsumeType,
        //                t.requisitions.SyncWMSStatus,
        //                ConsumeMaxWorkCenter = y?.WorkCenter
        //            })
        //        .Select(x => new MaterialRequisitionView() // 转换为最终视图
        //        {
        //            // --- 修复: 移除了 Operation ---
        //            Operation = x.Operation, 
        //            MaterialId = x.MaterialId,
        //            MaterialCode = x.MaterialCode,
        //            MaterialDesc = x.MaterialDesc,
        //            ReqQuantity = x.ReqQuantity,
        //            BaseUnit = x.BaseUnit,
        //            ConsumeType = x.ConsumeType == null ? "" : ((ConsumeType)x.ConsumeType).GetDescription(),
        //            ConsumeMaxWorkCenter = x.ConsumeMaxWorkCenter,
        //            SyncWmsStatus = (int)x.SyncWMSStatus == 0 ? "未推送" : "已推送"
        //        }).ToList();

        //        _materialRequisitionViews = requisition.GroupJoin(maxworkcenter, x => x.MaterialCode, y => y.MaterialCode.TrimStart('0'), (x, y) => new { requisitions = x, group = y }).SelectMany(t => t.group.DefaultIfEmpty(), (t, y) => new { t.requisitions.Operation, t.requisitions.MaterialId, t.requisitions.MaterialCode, t.requisitions.MaterialDesc, t.requisitions.ReqQuantity, t.requisitions.BaseUnit, ConsumeType = t.requisitions.ConsumeType, t.requisitions.SyncWMSStatus, ConsumeMaxWorkCenter = y?.WorkCenter }).Select(x => new MaterialRequisitionView() { Operation = x.Operation, MaterialId = x.MaterialId, MaterialCode = x.MaterialCode, MaterialDesc = x.MaterialDesc, ReqQuantity = x.ReqQuantity, BaseUnit = x.BaseUnit, ConsumeType = x.ConsumeType == null ? "" : ((ConsumeType)x.ConsumeType).GetDescription(), ConsumeMaxWorkCenter = x.ConsumeMaxWorkCenter, SyncWmsStatus = (int)x.SyncWMSStatus == 0 ? "未推送" : "已推送" }).ToList();
        //        ;
        //        materialGroupTable.DataSource = _materialRequisitionViews;

        //        consumetypeSelect.Items.Clear();
        //        consumetypeSelect.Items.AddRange(_materialRequisitionViews.GroupBy(s => s.ConsumeType).Select(g => new { g.Key }).Select(x => new MenuItem
        //        {
        //            Text = x.Key
        //        }).ToArray());
        //        consumetypeSelect.SelectedValue = "电缆";
        //    }
        //    catch (Exception ex)
        //    {

        //        AntdUI.Message.error(this, ex.Message);
        //    }

        //}

        private void queryButton_Click(object sender, EventArgs e)
        {
            materialGroupTable.DataSource = null;
            var filter = consumetypeSelect.Text;
            materialGroupTable.DataSource = _materialRequisitionViews.Where(x => (x.ConsumeType ?? string.Empty) == filter).ToList();
        }

        // 【核心修改】导出逻辑
        private async void exportButton_Click(object sender, EventArgs e)
        {
            await RunAsync(exportButton, async () =>
            {
                // 1. 获取当前数据
                var data = materialGroupTable.DataSource as List<MaterialRequisitionView>;
                if (data == null || !data.Any())
                {
                    throw new Exception("未查询到待导出的数据源，导出失败");
                }

                // 2. 转换为 Facade 接受的 Demand DTO
                var demands = data.Select(x => new WorkOrderModuleFacade.MaterialRequisitionDemandDto
                {
                    MaterialCode = x.MaterialCode,
                    MaterialDesc = x.MaterialDesc,
                    Operation = x.Operation,
                    ReqQuantity = x.ReqQuantity,
                    BaseUnit = x.BaseUnit,
                    ConsumeType = x.ConsumeType,
                    ConsumeMaxWorkCenter = x.ConsumeMaxWorkCenter
                }).ToList();

                var factory = await _facade.FactoryService.GetByIdAsync(AppSession.CurrentFactoryId);
                // 3. 调用 Facade 执行复杂的库存分配计算
                var exportData = await _facade.GenerateMaterialRequisitionExportDataAsync(demands, factory);

                // 4. 导出结果
                var fileName = string.IsNullOrWhiteSpace(consumetypeSelect.Text) ? "无属性物料" : consumetypeSelect.Text;
                ExcelExportHelper.ExportToExcel(this.ParentForm, exportData.OrderBy(x => x.MaterialCode).Select(x =>  new 
                {
                    x.Operation,
                    x.MaterialCode,
                    x.MaterialDesc,
                    x.ReqQuantity,
                    x.BaseUnit,
                    x.ConsumeType,
                    x.ConsumeMaxWorkCenter,
                    x.LinesideReqQuantity,
                    x.LinesideStockQuantity,
                    x.LinesideStockLocation,
                    x.LinesideStockBarcode,
                    x.rawReqQuantity,
                    x.rawStockQuantity,
                    x.rawStockLocation,
                    x.rawStockBarcode
                }).ToList(), fileName);

            }, confirmMsg: "即将导出当前数据集，是否继续？");
        }

        ///// <summary>
        ///// (新) 执行所有数据获取和业务逻辑的核心方法
        ///// </summary>
        //private async Task<List<MaterialRequisitionView>> GenerateExportDataAsync(List<MaterialRequisitionView> demands)
        //{
        //    // 1. 筛选出需要处理的需求和物料
        //    var cableDemands = demands.Where(x => x.ConsumeType == ConsumeType.CableMaterial.GetDescription()).ToList();
        //    var materialCodes = cableDemands.Select(d => d.MaterialCode).Distinct().ToList();

        //    if (!materialCodes.Any())
        //    {
        //        return new List<MaterialRequisitionView>();
        //    }

        //    // --- 性能优化 (N+1 修复): 批量获取所有数据 ---
        //    // (您必须在您的服务中实现 GetListByMaterialCodesAsync 方法)
        //    var lineStockTask = await  _rawLinesideStockService.GetListByMaterialCodeAsync(AppSession.CurrentFactoryId, materialCodes);
        //    var rawStockTask =await _rawMaterialInventoryViewService.GetListByMaterialCodeAsync(AppSession.CurrentUser.FactoryName, materialCodes);

        //    // 并行执行两个数据库查询
        //   // await System.Threading.Tasks.Task.WhenAll(lineStockTask, rawStockTask);

        //    // --- 性能优化: 将结果分组以便快速查找 (O(1)) ---
        //    var lineStockLookup = lineStockTask
        //        .Where(x => x.LastQuantity > 0)
        //        .OrderBy(x => x.BarCode) // 预排序
        //        .ToLookup(x => x.MaterialCode);

        //    var rawStockLookup = rawStockTask
        //        .OrderBy(x => x.BatchCode) // 预排序
        //        .ToLookup(x => x.MaterialCode);


        //    var finalExportList = new List<MaterialRequisitionView>();

        //    // 3. 在内存中处理数据 (非常快)
        //    foreach (var item in cableDemands)
        //    {
        //        decimal neededQuantity = (decimal)item.ReqQuantity;

        //        var lineAllocations = new List<AllocationResult>();
        //        var rawAllocations = new List<AllocationResult>();

        //        // 4. 从断线库分配
        //        var availableLineStock = lineStockLookup[item.MaterialCode];
        //        if (availableLineStock.Any())
        //        {
        //            // AllocateLineStock 会修改 neededQuantity 和 lineAllocations
        //            neededQuantity = AllocateLineStock(availableLineStock, neededQuantity, lineAllocations);
        //        }

        //        // 5. 如果仍需要，从原材料库分配
        //        if (neededQuantity > 0)
        //        {
        //            var availableRawStock = rawStockLookup[item.MaterialCode];
        //            if (availableRawStock.Any())
        //            {
        //                // AllocateRawStock 会修改 rawAllocations
        //                AllocateRawStock(availableRawStock, neededQuantity, rawAllocations);
        //            }
        //        }

        //        // 6. 构建导出结果 (替换复杂的 GroupJoin)
        //        if (!lineAllocations.Any() && !rawAllocations.Any())
        //        {
        //            // 物料有需求，但未找到任何库存
        //            finalExportList.Add(CreateExportRow(item, null, null));
        //        }
        //        else
        //        {
        //            // 添加所有断线库的分配行
        //            foreach (var la in lineAllocations)
        //            {
        //                finalExportList.Add(CreateExportRow(item, la, null));
        //            }
        //            // 添加所有原材料库的分配行
        //            foreach (var ra in rawAllocations)
        //            {
        //                finalExportList.Add(CreateExportRow(item, null, ra));
        //            }
        //        }
        //    }

        //    return finalExportList;
        //}

        ///// <summary>
        ///// (新) 辅助方法：处理断线库的库存分配逻辑
        ///// </summary>
        //private decimal AllocateLineStock(IEnumerable<RawLinesideStockDto> availableStock, decimal neededQuantity, List<AllocationResult> allocations)
        //{
        //    decimal? originalStock = null;
        //    foreach (var stockInfo in availableStock)
        //    {
        //        if (originalStock == null)
        //            originalStock = stockInfo.LastQuantity; // 复制原始代码的逻辑: 捕获第一个库存量

        //        if (neededQuantity <= 0)
        //            break;

        //        var quantityToTake = Math.Min(neededQuantity, (decimal)stockInfo.LastQuantity);
        //        allocations.Add(new AllocationResult
        //        {
        //            OriginalQuantity = originalStock ?? 0, // 复制原始代码的逻辑
        //            QuantityTaken = quantityToTake,
        //            Location = stockInfo.LocationCode,
        //            Barcode = stockInfo.BarCode
        //        });

        //        neededQuantity -= quantityToTake;
        //        stockInfo.LastQuantity -= quantityToTake; // 在内存中消耗库存 (与原始代码一致)
        //    }
        //    return neededQuantity; // 返回剩余的需求量
        //}

        ///// <summary>
        ///// (新) 辅助方法：处理原材料库的库存分配逻辑
        ///// </summary>
        //private void AllocateRawStock(IEnumerable<RawMaterialInventoryViewDto> availableStock, decimal neededQuantity, List<AllocationResult> allocations)
        //{
        //    decimal? originalStock = null;
        //    foreach (var stockInfo in availableStock)
        //    {
        //        if (originalStock == null)
        //            originalStock = stockInfo.Quantity;

        //        if (neededQuantity <= 0)
        //            break;

        //        if (stockInfo.Quantity > 0)
        //        {
        //            var quantityToTake = Math.Min(neededQuantity, (decimal)stockInfo.Quantity);
        //            allocations.Add(new AllocationResult
        //            {
        //                OriginalQuantity = originalStock ?? 0,
        //                QuantityTaken = quantityToTake,
        //                Location = stockInfo.RawLocationName,
        //                Barcode = stockInfo.BarCode
        //            });

        //            neededQuantity -= quantityToTake;
        //            stockInfo.Quantity -= quantityToTake; // 在内存中消耗库存 (与原始代码一致)
        //        }
        //    }
        //}

        ///// <summary>
        ///// (新) 辅助方法：用于创建最终导出行的 DTO
        ///// </summary>
        //private MaterialRequisitionView CreateExportRow(MaterialRequisitionView demand, AllocationResult lineAlloc, AllocationResult rawAlloc)
        //{
        //    return new MaterialRequisitionView
        //    {
        //        MaterialCode = demand.MaterialCode,
        //        MaterialDesc = demand.MaterialDesc,
        //        Operation = demand.Operation,
        //        ReqQuantity = demand.ReqQuantity,
        //        BaseUnit = demand.BaseUnit,
        //        ConsumeType = demand.ConsumeType,
        //        ConsumeMaxWorkCenter = demand.ConsumeMaxWorkCenter,

        //        // 断线库信息
        //        LinesideReqQuantity = lineAlloc?.QuantityTaken ?? (rawAlloc == null ? demand.ReqQuantity : 0), // 如果是断线库行，显示分配量；如果是原材料库行，显示0；如果两者都不是，显示总需求
        //        LinesideStockQuantity = lineAlloc?.OriginalQuantity ?? 0,
        //        LinesideStockLocation = lineAlloc?.Location ?? string.Empty,
        //        LinesideStockBarcode = lineAlloc?.Barcode ?? string.Empty,

        //        // 原材料库信息
        //        rawReqQuantity = rawAlloc?.QuantityTaken ?? 0,
        //        rawStockQuantity = rawAlloc?.OriginalQuantity,
        //        rawStockLocation = rawAlloc?.Location ?? string.Empty,
        //        rawStockBarcode = rawAlloc?.Barcode ?? string.Empty,
        //    };
        //}

        //private async void pushButton_Click(object sender, EventArgs e)
        //{
        //    pushButton.Loading = true;
        //    try
        //    {
        //        pushProgress.Visible = true;
        //        StringBuilder message = new StringBuilder();
        //        var requestUrl = _apiSettings["JyApi"].Endpoints["TransvouchCreate"];
        //        var request = new MaterialRequisitionRequest();
        //        var Json = string.Empty;
        //        var result = new ApiResponse<object>();
        //        //推送物料需求
        //        if (_materialRequisitionViews.Where(x => x.check).Count() == 0)
        //            throw new Exception("请至少选择一条需要推送的物料需求记录!");

        //        var data = _materialRequisitionViews.Where(x => x.ConsumeType == ConsumeType.CableMaterial.GetDescription() && x.check /*&& x.SyncWmsStatus == "未推送"*/);

        //        if (data.Count() == 0)
        //            throw new Exception("没有可推送的电缆物料需求，请确认!");
        //        if (data != null && data.Count() > 0)
        //        {
        //            if (AntdUI.Modal.open(this, "提示", "即将向WMS推送领料需求，是否继续？", TType.Warn) == DialogResult.OK)
        //            {
        //                //推送需求时扣减1104库存
        //                foreach (var item in data)
        //                {
        //                    var linestocks = (await _rawLinesideStockService.GetListByMaterialCodeAsync(AppSession.CurrentFactoryId, item.MaterialCode)).Where(x => x.LastQuantity > 0).GroupBy(s => s.MaterialCode).Select(g => new
        //                    {
        //                        MaterialCode = g.Key,
        //                        Quantity = g.Sum(l => l.LastQuantity)
        //                    }).FirstOrDefault();
        //                    if (linestocks != null && linestocks.Quantity > 0)
        //                    {
        //                        item.ReqQuantity = item.ReqQuantity - linestocks.Quantity;
        //                    }
        //                }
        //                //更新WMS断线领料任务单为关闭状态
        //                await _workOrderViewService.UpdateJyWmsRequrementTaskAsync();
        //                //断线物料推送
        //                request = new MaterialRequisitionRequest
        //                {
        //                    VoucherTypeCategoryCode = "VTC022",
        //                    VoucherTypeName = "999",
        //                    OWarehouseCode = "1100",
        //                    IWarehouseCode = "2100",
        //                    OStorageCode = "11004",
        //                    IStorageCode = "21001",
        //                    VirtualOPlantCode = AppSession.CurrentUser.FactoryName,
        //                    VirtualIPlantCode = AppSession.CurrentUser.FactoryName,
        //                    EntryDtos = data.Where(x => x.ConsumeType == ConsumeType.CableMaterial.GetDescription() && x.ReqQuantity > 0).GroupBy(x => x.MaterialId).Select(g => new { MaterialId = g.Key, RequiredQuantity = g.Sum(x => x.ReqQuantity) }).Select(x => new MaterialEntry { MaterialId = (int)x.MaterialId, Qty = (decimal)x.RequiredQuantity }).ToList(),
        //                    PlantCode = AppSession.CurrentUser.FactoryName,
        //                    IsActive = false,
        //                    IsActiveDisplay = "",
        //                    OperateUser = AppSession.CurrentUser.EmployeeId,
        //                    OperateUserName = AppSession.CurrentUser.UserName,
        //                    Remark = "断线"
        //                };
        //                Json = JsonConvert.SerializeObject(request, Formatting.Indented);
        //                result = await _jyApiClient.PostAsync<MaterialRequisitionRequest, object>(requestUrl, request);
        //                //result.IsSuccess = true;
        //                if (!result.IsSuccess)
        //                    message.AppendLine("电缆领料推送失败：" + result.Message);
        //                else
        //                {
        //                    var update = _bomItems.Where(x => data.Select(d => d.MaterialCode).Contains(x.MaterialCode)).Select(x => new WorkOrderBomItemUpdateDto()
        //                    {
        //                        Id = x.Id,
        //                        SyncWMSStatus = x.SyncWMSStatus + 1,
        //                        UpdateOn = DateTime.Now,
        //                        UpdateBy = AppSession.CurrentUser.EmployeeId
        //                    }).ToList();
        //                    var flag = await _workOrderBomService.UpdateWmsStatusAsync(update);
        //                    message.AppendLine("电缆领料推送成功!");

        //                }

        //                #region 取消按单推送
        //                //按单物料推送
        //                //int i = 1;
        //                //int step = _bomItems.Where(x => x.ConsumeType == 2 && x.SyncWMSStatus == 0).GroupBy(x => x.WorkOrderId).Select(group => group.First()).Select(x => x.WorkOrderId).Count() + 1;

        //                //pushProgress.Value = (float)i / step;
        //                //foreach (var item in _bomItems.Where(x => x.ConsumeType == 2 && x.SyncWMSStatus == 0 && x.RequiredQuantity > 0).GroupBy(x => x.WorkOrderId).Select(group => group.First()).Select(x => x.WorkOrderId))
        //                //{

        //                //    request = null;
        //                //    Json = string.Empty;
        //                //    result = null;
        //                //    var workorder = await _workOrderService.GetByIdAsync(item);
        //                //    var bomtemp = _bomItems.Where(x => x.WorkOrderId == item && x.ConsumeType == 2 && x.RequiredQuantity > 0);
        //                //    var syncwms = bomtemp.Max(x => x.SyncWMSStatus) + 1;
        //                //    request = new MaterialRequisitionRequest
        //                //    {
        //                //        VoucherNo = $"{workorder.OrderNumber}-{syncwms}",
        //                //        VoucherTypeCategoryCode = "VTC022",
        //                //        VoucherTypeName = "999",
        //                //        OWarehouseCode = "1100",
        //                //        IWarehouseCode = "2100",
        //                //        OStorageCode = "11004",
        //                //        IStorageCode = "21001",
        //                //        VirtualOPlantCode = "CN11",
        //                //        VirtualIPlantCode = "CN11",
        //                //        EntryDtos = bomtemp.Where(x => x.SyncWMSStatus == 0).GroupBy(x => x.MaterialId).Select(g => new { MaterialId = g.Key, RequiredQuantity = g.Sum(x => x.RequiredQuantity) }).Select(x => new MaterialEntry { MaterialId = (int)x.MaterialId, Qty = (decimal)x.RequiredQuantity }).ToList(),

        //                //        PlantCode = "CN11",
        //                //        IsActive = false,
        //                //        IsActiveDisplay = null,
        //                //        OperateUser = AppSession.CurrentUser.EmployeeId,
        //                //        OperateUserName = AppSession.CurrentUser.UserName,
        //                //        Remark = "按单"
        //                //    };
        //                //    Json = JsonConvert.SerializeObject(request, Formatting.Indented);
        //                //    result = await _jyApiClient.PostAsync<MaterialRequisitionRequest, object>(requestUrl, request);
        //                //    if (!result.IsSuccess)
        //                //        message.AppendLine($"{workorder.OrderNumber}按单领料推送失败：" + result.Message);
        //                //    else
        //                //    {
        //                //        var update = bomtemp.Select(x => new WorkOrderBomItemUpdateDto()
        //                //        {
        //                //            Id = x.Id,
        //                //            SyncWMSStatus = x.SyncWMSStatus + 1
        //                //        }).ToList();
        //                //        await _workOrderBomService.UpdateWmsStatusAsync(update);

        //                //    }
        //                //    pushProgress.Value = (float)++i / step;

        //                //}

        //                //AntdUI.Modal.open(new AntdUI.Modal.Config(this, "提示", new AntdUI.Input()
        //                //{
        //                //    Text = message.ToString(),
        //                //    Width = 350,
        //                //    Height = 400,
        //                //    BorderWidth = 0,
        //                //    Dock = DockStyle.Bottom,
        //                //    Multiline = true,
        //                //}, AntdUI.TType.Warn)
        //                //{
        //                //    CancelText = null
        //                //});
        //                #endregion

        //                AntdUI.Message.info(this.ParentForm, message.ToString());
        //            }


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        AntdUI.Modal.open(new AntdUI.Modal.Config(this, "错误", ex.Message, AntdUI.TType.Error)
        //        {
        //            CancelText = null
        //        });
        //    }
        //    finally
        //    {
        //        pushButton.Loading = false;
        //        pushProgress.Visible = false;
        //        pushProgress.Value = 0;
        //    }

        //}

        //private void consumetypeSelect_SelectedIndexChanged(object sender, IntEventArgs e)
        //{
        //    materialGroupTable.DataSource = null;
        //    var filter = consumetypeSelect.Text;
        //    materialGroupTable.DataSource = _materialRequisitionViews.Where(x => (x.ConsumeType ?? string.Empty) == filter).ToList();
        //}

    }

    public class MaterialRequisitionView : AntdUI.NotifyProperty
    {
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

        public string? Operation
        {
            get;
            set;
        }

        public int? MaterialId
        {
            get;
            set;
        }
        public string? MaterialCode
        {
            get;
            set;
        }

        public string? MaterialDesc
        {
            get;
            set;
        }
        public decimal? ReqQuantity
        {
            get;
            set;
        }


        public string? BaseUnit
        {
            get;
            set;
        }

        public string? ConsumeType
        {
            get;
            set;
        }

        public string SyncWmsStatus
        {
            get;
            set;
        }

        public string? ConsumeMaxWorkCenter
        {
            get;
            set;
        }

        public decimal? LinesideReqQuantity
        {
            get;
            set;
        }

        public decimal? LinesideStockQuantity
        {
            get;
            set;
        }

        public string? LinesideStockLocation
        {
            get;
            set;
        }

        public string? LinesideStockBarcode
        {
            get;
            set;
        }


        public decimal? rawReqQuantity
        {
            get;
            set;
        }
        public decimal? rawStockQuantity
        {
            get;
            set;
        }



        public string? rawStockLocation
        {
            get;
            set;
        }

        public string? rawStockBarcode
        {
            get;
            set;
        }
    }

    public class RawLinesideStockInfo
    {
        public RawLinesideStockDto BarCode
        {
            get; set;
        }
        public decimal RemainingQuantity
        {
            get; set;
        }
    }

    // (新) 内部 DTO，用于在分配期间传递结果
    public class AllocationResult
    {
        public decimal OriginalQuantity
        {
            get; set;
        }
        public decimal QuantityTaken
        {
            get; set;
        }
        public string Location
        {
            get; set;
        }
        public string Barcode
        {
            get; set;
        }
    }
}
