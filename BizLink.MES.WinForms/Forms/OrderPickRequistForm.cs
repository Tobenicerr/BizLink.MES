using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Wordprocessing;
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
using System.Windows.Media.Animation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using BizLink.MES.Application.DTOs.Request;

namespace BizLink.MES.WinForms.Forms
{
    public partial class OrderPickRequistForm : BaseForm
    {

        private readonly IWorkOrderService _workOrderService;
        private readonly IWorkOrderBomItemService _workOrderBomItemService;
        private readonly IWorkOrderProcessService _workOrderProcessService;
        private readonly IProductLinesideStockService _productLinesideStockService;
        private readonly IWorkOrderTaskService _workOrderTaskService;
        private readonly IMaterialViewService _materialViewService;
        private readonly IMesApiClient _mesApiClient;
        private readonly IJyApiClient _jyApiClient;
        private readonly Dictionary<string, ServiceEndpointSettings> _apiSettings;
        private readonly IWorkOrderViewService _workOrderViewService;
        private readonly IAutoStockOutService _autoStockOutService;
        private readonly ICenterStockOutService _centerStockOutService;



        public OrderPickRequistForm(IWorkOrderService workOrderService, IWorkOrderBomItemService workOrderBomItemService, IWorkOrderProcessService workOrderProcessService, IProductLinesideStockService productLinesideStockService, IWorkOrderTaskService workOrderTaskService, IMaterialViewService materialViewService, IMesApiClient mesApiClient, IJyApiClient jyApiClient, IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings, IWorkOrderViewService workOrderViewService, IAutoStockOutService autoStockOutService, ICenterStockOutService centerStockOutService)
        {
            InitializeComponent();
            InitializeTable();
            _workOrderService = workOrderService;
            _workOrderBomItemService = workOrderBomItemService;
            _workOrderProcessService = workOrderProcessService;
            _productLinesideStockService = productLinesideStockService;
            _workOrderTaskService = workOrderTaskService;
            _materialViewService = materialViewService;
            _mesApiClient = mesApiClient;
            _jyApiClient = jyApiClient;
            _apiSettings = apiSettings.Value;
            _workOrderViewService = workOrderViewService;
            _autoStockOutService = autoStockOutService;
            _centerStockOutService = centerStockOutService;
        }

        private void InitializeTable()
        {

            orderTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnCheck("check").SetFixed(),
                new AntdUI.Column("OrderNumber", "订单号", AntdUI.ColumnAlign.Left)
                {
                   KeyTree = "Children"

                }.SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("RequireQuantity", "计划数量", AntdUI.ColumnAlign.Center).SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CompleteQuantity", "完成数量", AntdUI.ColumnAlign.Center).SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConsumeTypeDesc", "物料属性", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LeadingOrder", "成品订单", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LeadingMaterial", "成品物料", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                  new AntdUI.Column("FactoryCode", "工厂", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("StartDate", "开工日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("DispatchDate", "装配日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                    new AntdUI.Column("SyncWms", "领料状态", AntdUI.ColumnAlign.Center){
                    Render = (value, record, index) =>
                    {
                        switch (value as string)
                        {
                            case "未领料":
                                return new AntdUI.CellTag("未领料", AntdUI.TTypeMini.Error);

                            case "已领料":
                                return new AntdUI.CellTag("已领料", AntdUI.TTypeMini.Success);

                            case "部分领料":
                                return new AntdUI.CellTag("部分领料", AntdUI.TTypeMini.Error);
                            default:
                                return new AntdUI.CellTag("无需领料", AntdUI.TTypeMini.Default);
                        }

                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                 new AntdUI.Column("IsPicked", "完成状态", AntdUI.ColumnAlign.Center){
                    Render = (value, record, index) =>
                    {
                        switch (value as string)
                        {
                            case "未开始":
                                return new AntdUI.CellTag("未开始", AntdUI.TTypeMini.Error);
                            case "执行中":
                                return new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Default);
                            case "缺料中":
                                return new AntdUI.CellTag("缺料中", AntdUI.TTypeMini.Error);
                            case "已挂起":
                                return new AntdUI.CellTag("已挂起", AntdUI.TTypeMini.Primary);
                            case "待合箱":
                                return new AntdUI.CellTag("待合箱", AntdUI.TTypeMini.Primary);
                            case "已完成":
                                return new AntdUI.CellTag("已完成", AntdUI.TTypeMini.Success);
                            case "已关闭":
                                return new AntdUI.CellTag("已关闭", AntdUI.TTypeMini.Success);
                            default:
                                return new AntdUI.CellTag("无需合箱", AntdUI.TTypeMini.Default);
                        }
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                //{
                //    LocalizationTitle ="Table.Column.{id}",
                //    Call = (value, record, i_row, i_col) => {
                //        System.Threading.Thread.Sleep(2000);
                //        return value;
                //    }
                //}
                
             };

        }

        //private async Task LoadData()
        //{


        //    // 假设 Spin 控件叫 SpinControl，Table 控件叫 userTable
        //    SpinControl.Visible = true; // 1. 显示加载动画
        //    orderTable.Enabled = false; //    禁用表格，防止用户在加载时操作
        //    queryButton.Loading = true;
        //    try
        //    {
        //        if (AppSession.CurrentFactoryId <= 0)
        //            throw new Exception("请先选择工厂！");
        //        var orders = keyboardInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //        // 调用应用服务层的方法
        //        var result = await _workOrderService.GetListByDispatchDateAsync(null, startDatePicker.Value, orders, AppSession.CurrentFactoryId);

        //        // 将返回的DTO数据绑定到界面
        //        if (result != null)
        //        {
        //            var orderpickviews = result.Select(x => new OrderPickRequistView(x)).ToList();
        //            if (!orderpickviews.Any())
        //            {
        //                throw new Exception("未查询到订单记录！");
        //            }

                   
        //            foreach (var item in orderpickviews)
        //            {
        //                var opstatus = await GetOPCompletedStatus(item.Id);
        //                if (opstatus == "已完成")
        //                    item.IsPicked = "已完成";
        //                else
        //                {
        //                    var boms = await _workOrderBomItemService.GetListByOrderIdAync(item.Id);
        //                    if (!boms.Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true && (x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial)).Any())
        //                    {
        //                        item.IsPicked = "无需合箱";
        //                        continue;
        //                    }
        //                    else
        //                    {
        //                        if (!boms.Where(x => x.SyncWMSStatus > 0).Any())
        //                        {
        //                            item.IsPicked = "未开始";
        //                        }
        //                        else
        //                        {
        //                            var detail = await LoadDetailData(item.Id);
        //                            if (detail.Where(x => x.IsPicked == "执行中").Any())
        //                            {
        //                                item.IsPicked = "执行中";
        //                            }
        //                            else if (detail.Where(x => x.IsPicked == "已完成").Count() == detail.Count())
        //                            {

        //                                item.IsPicked = "待合箱";

        //                                item.CompleteQuantity = item.RequireQuantity;
        //                            }
        //                        }
        //                    }
        //                }
        //                //    var detail = await LoadDetailData(item.Id);
        //                //if (detail == null || detail.Count == 0)
        //                //{

        //                //}
        //                //else if (!detail.Where(x => x.IsPicked == "已完成").Any())
        //                //{
        //                //    item.IsPicked = "未开始";
        //                //}
        //                //else if (detail.Where(x => x.IsPicked == "已完成").Any() && detail.Where(x => x.IsPicked == "执行中").Any())
        //                //{
        //                //    item.IsPicked = "执行中";
        //                //}
        //                //else if (detail.Where(x => x.IsPicked == "已完成").Count() == detail.Count())
        //                //{
        //                //    else
        //                //        item.IsPicked = "待合箱";

        //                //    item.CompleteQuantity = item.RequireQuantity;
        //                //}


        //                item.SyncWms = await GetRequistStatus(item.Id);
        //            }

        //            orderTable.DataSource = orderpickviews;
        //            totalLabel.Text = $"  订单总数：{result.Count()} 笔";
        //            AntdUI.Message.success(this, $"查询成功：共有{result.Count}笔订单");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // 最好有一个统一的错误提示方式
        //        AntdUI.Message.error(this, "加载数据失败：" + ex.Message);
        //    }
        //    finally
        //    {
        //        SpinControl.Visible = false; // 2. 无论成功或失败，都隐藏加载动画
        //        orderTable.Enabled = true; //     重新启用表格
        //        queryButton.Loading = false;

        //    }
        //}

        // 【优化核心】：将 LoadData 改为批量处理
        private async Task LoadData()
        {
            SpinControl.Visible = true;
            orderTable.Enabled = false;
            queryButton.Loading = true;
            try
            {

                var orders = keyboardInput.Text.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                // 1. 获取订单主数据
                var result = await _workOrderService.GetListByDispatchDateAsync(finishDatePicker.Value, startDatePicker.Value, orders, AppSession.CurrentFactoryId);

                if (result != null && result.Any())
                {
                    // 准备 ID 列表用于批量查询
                    var orderIds = result.Select(x => x.Id).ToList();
                    var orderNos = result.Select(x => x.OrderNumber).ToList();

                    // 2. 【批量并发查询】一次性获取所有关联数据 (N+1 杀手)
                    // 注意：您需要在 Service 层实现这些 GetListByOrderIdsAsync 方法。
                    // 如果没有，请务必添加，否则性能无法根本解决。
                    //var processesTask = _workOrderProcessService.GetListByOrderIdsAync(orderIds); // 需实现
                    //var bomsTask = _workOrderBomItemService.GetListByOrderIdsAync(orderIds);     // 需实现
                    //var stocksTask = _productLinesideStockService.GetListByOrderNoAsync(orderNos); // 需实现 (或 GetListByOrderNumbers)
                    //var tasksTask = _workOrderTaskService.GetListByOrderIdsAsync(orderIds);       // 需实现

                    //await Task.WhenAll(processesTask, bomsTask, stocksTask, tasksTask);



                    //var allProcesses = processesTask.Result;
                    //var allBoms = bomsTask.Result;
                    //var allStocks = stocksTask.Result;
                    //var allTasks = tasksTask.Result;


                    var allProcesses = await _workOrderProcessService.GetListByOrderIdsAync(orderIds);
                    var allBoms = await _workOrderBomItemService.GetListByOrderIdsAync(orderIds);
                    var allStocks = await _productLinesideStockService.GetListByOrderNoAsync(orderNos);
                    var allTasks = await _workOrderTaskService.GetListByOrderIdsAsync(orderIds);


                    var allCenterStocks = await _centerStockOutService.GetListByWorkOrderAsync(orderNos);

                    var allAutoStocks = await _autoStockOutService.GetAllAsync();

                    // 3. 批量获取物料信息
                    var materialCodes = allBoms.Select(b => b.MaterialCode).Distinct().ToList();
                    var allMaterials = await _materialViewService.GetListByCodesAsync(AppSession.CurrentUser.FactoryName, materialCodes); // 假设已存在
                    var materialDict = allMaterials.ToDictionary(m => m.MaterialCode, m => m.LabelName);

                    // 4. 构建内存查找表 (Lookups)
                    var processLookup = allProcesses.ToLookup(p => p.WorkOrderId);
                    var bomLookup = allBoms.ToLookup(b => b.WorkOrderId);
                    var stockLookup = allStocks.ToLookup(s => s.WorkOrderNo); // 假设通过 WorkOrderNo 关联
                    var autoStockLookup = allAutoStocks.ToLookup(a => a.WorkOrderNo);

                    var centerStockLookup = allCenterStocks.ToLookup(a => a.WorkOrderNo);

                    // 假设 Task 也有 WorkOrderId 或关联到 ProcessId
                    // 这里假设通过 OrderProcessId 关联，我们需要先建立 ProcessId -> Task 的映射
                    var taskLookup = allTasks.ToLookup(t => t.OrderProcessId);

                    var orderpickviews = new List<OrderPickRequistView>();

                    // 5. 在内存中进行计算，不再有 await
                    foreach (var orderDto in result)
                    {
                        var view = new OrderPickRequistView(orderDto);
                        var orderBoms = bomLookup[orderDto.Id].ToList();
                        var orderStocks = stockLookup[orderDto.OrderNumber].Select(x => new ProductLinesideStockDto
                        {
                            MaterialCode = x.MaterialCode,
                            Quantity = x.Quantity // 复制一份以防修改影响源数据
                        }).ToList();

                        var orderAutoStock = autoStockLookup[orderDto.OrderNumber].ToList();
                        var orderCenterStock = centerStockLookup[orderDto.OrderNumber].ToList();

                        // --- 计算 OP 状态 ---
                        var firstProcess = processLookup[orderDto.Id].OrderBy(p => p.Operation).FirstOrDefault();
                        bool isOpFinished = firstProcess != null && firstProcess.Status == ((int)WorkOrderStatus.Finished).ToString();

                        if (isOpFinished)
                        {
                            view.IsPicked = "已完成";
                            view.CompleteQuantity = firstProcess.CompletedQuantity;
                        }
                        // --- 计算明细和拣配状态 ---
                        // 筛选出相关 BOM
                        var validBoms = orderBoms.Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true &&
                            (x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial)).ToList();

                        if (!validBoms.Any())
                        {
                            view.IsPicked = "无需合箱";
                            orderpickviews.Add(view);
                        }
                        else
                        {
                            // 计算明细状态 (纯内存计算)
                            var details = CalculateDetailsInMemory(validBoms, orderAutoStock, orderCenterStock, orderStocks, taskLookup, materialDict);
                            view.Children = details.ToArray();

                            // 计算 SyncWms 状态
                            view.SyncWms = CalculateRequistStatus(orderBoms);

                            // 汇总状态
                            if (!validBoms.Any(x => x.SyncWMSStatus > 0) && view.SyncWms != "已领料" && !details.Any(x => x.IsPicked == "已完成"))
                            {
                                view.IsPicked = "未开始";
                            }
                            else
                            {
                                if (details.Any(x => x.IsPicked == "缺料中"))
                                    view.IsPicked = "缺料中";
                                else if (details.All(x => x.IsPicked == "已完成" || x.IsPicked == "已关闭" || x.IsPicked == "已挂起"))
                                {
                                    view.SyncWms = "已领料";
                                    if (!isOpFinished)
                                        view.IsPicked = "待合箱"; // 逻辑根据您的原代码调整
                                }

                                else
                                    view.IsPicked = "执行中"; // 默认回退
                            }
                            orderpickviews.Add(view);
                        }

                    }

                    orderTable.DataSource = orderpickviews;
                    totalLabel.Text = $"  订单总数：{result.Count()} 笔";
                    AntdUI.Message.success(this, $"查询成功：共有{result.Count}笔订单");
                }
                else
                {
                    throw new Exception("未查询到订单记录！");
                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, "加载数据失败：" + ex.Message);
            }
            finally
            {
                SpinControl.Visible = false;
                orderTable.Enabled = true;
                queryButton.Loading = false;
            }
        }

        // 【新】纯内存计算明细状态，不进行数据库访问
        private List<OrderPickDetailView> CalculateDetailsInMemory(
            List<WorkOrderBomItemDto> boms,
            List<AutoStockOutDto>? autoStocks,
            List<CenterStockOutDto>? centerStocks,
            List<ProductLinesideStockDto> stocks,
            ILookup<int, WorkOrderTaskDto> taskLookup, // 假设 key 是 ProcessId
            Dictionary<string, string> materialLabels)
        {
            var details = boms.Select(x => new OrderPickDetailView(x)).ToList();
            foreach (var item in details)
            {
                //断线强制关闭
                bool isClosed = false;
                if (materialLabels.TryGetValue(item.MaterialCode, out var labelName))
                {
                    item.ConsumeTypeDesc = labelName;
                }

                if (item.ConsumeType == (int)ConsumeType.CableMaterial)
                {
                    // 从内存查找 Task
                    // 注意：原代码是 GetByProcessIdAsync(item.WorkOrderProcessId, item.BomItem)
                    // 这里假设 Task 列表中能找到匹配的
                    var task = taskLookup[item.WorkOrderProcessId].FirstOrDefault(t => t.MaterialItem == item.BomItem); // 假设 MaterialItem 对应 BomItem
                    if (task != null)
                    {
                        item.RequireQuantity = (decimal)task.Quantity;
                        item.CompleteQuantity = (decimal)task.CompletedQty;
                        if (task.Status == ((int)WorkOrderStatus.Paused).ToString())
                            isClosed = true;
                    }
                    item.SyncWms = "已领料";

                }
                else if (item.ConsumeType == (int)ConsumeType.OrderBasedMaterial)
                {
                    // 内存中扣减库存逻辑
                    var relevantStocks = stocks.Where(x => x.MaterialCode == item.MaterialCode).ToList();
                    foreach (var stock in relevantStocks)
                    {
                        if (item.RequireQuantity > item.CompleteQuantity)
                        {
                            decimal needed = (decimal)item.RequireQuantity - (decimal)item.CompleteQuantity;
                            if (stock.Quantity >= needed)
                            {
                                item.CompleteQuantity += needed;
                                stock.Quantity -= needed; // 修改的是内存中的临时对象
                            }
                            else
                            {
                                item.CompleteQuantity += (decimal)stock.Quantity;
                                stock.Quantity = 0;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (item.CompleteQuantity >= item.RequireQuantity)
                    item.IsPicked = "已完成";
                else
                {
                    if (isClosed)
                        item.IsPicked = "已挂起";
                    else
                    {
                        item.IsPicked = "执行中";
                        if (autoStocks.Where(x => x.MaterialCode == item.MaterialCode && x.Status == "-1").Count() > 0 || centerStocks.Where(x => x.MaterialCode == item.MaterialCode && x.Status == "-1").Count() > 0)
                            item.IsPicked = "缺料中";
                        if (centerStocks.Where(x => x.MaterialCode == item.MaterialCode && x.Status == "2").Count() > 0)
                            item.IsPicked = "已关闭";
                    }
                }
            }
            return details.OrderBy(x => x.ConsumeTypeDesc).ToList();
        }

        // 【新】纯内存计算 RequistStatus
        private string CalculateRequistStatus(List<WorkOrderBomItemDto> boms)
        {
            var targets = boms.Where(x => x.ConsumeType == (int)ConsumeType.OrderBasedMaterial && x.RequiredQuantity > 0 && x.MovementAllowed == true).ToList();
            if (!targets.Any(x => x.SyncWMSStatus == 0))
            {
                // 没有 SyncWMSStatus == 0 的，说明要么没有 targets，要么都 > 0
                if (targets.Any())
                    return RequistStatus.Yes.GetDescription(); // 全部 > 0
                return RequistStatus.Yes.GetDescription(); // 或者 No? 原逻辑有点歧义，这里照搬 "Count() == 0 -> Yes"
            }

            bool hasSynced = targets.Any(x => x.SyncWMSStatus > 0);
            bool hasUnsynced = targets.Any(x => x.SyncWMSStatus == 0);

            if (hasSynced && hasUnsynced)
                return RequistStatus.Partial.GetDescription();
            return RequistStatus.No.GetDescription();
        }

        private async Task<List<OrderPickDetailView>> LoadDetailData(int orderid)
        {
            try
            {
                var workorder = await _workOrderService.GetByIdAsync(orderid);
                if (workorder == null)
                {
                    throw new Exception("未找到对应的订单信息");
                }
                var boms = (await _workOrderBomItemService.GetListByOrderIdAync(orderid)).Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true).Where(x => x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial).Select(x => new OrderPickDetailView(x)).ToList();
                var products = await _productLinesideStockService.GetListByOrderNoAsync(workorder.OrderNumber);

                foreach (var item in boms)
                {
                    var material = await _materialViewService.GetByCodeAsync(AppSession.CurrentUser.FactoryName, item.MaterialCode);
                    item.ConsumeTypeDesc = material.LabelName;

                    if (item.ConsumeType == (int)ConsumeType.CableMaterial)
                    {
                        var task = await _workOrderTaskService.GetByProcessIdAsync(item.WorkOrderProcessId, item.BomItem);
                        if (task != null)
                        {
                            item.RequireQuantity = (decimal)task.Quantity;
                            item.CompleteQuantity = (decimal)task.CompletedQty;
                        }

                        item.SyncWms = "已领料";
                    }
                    else if (item.ConsumeType == (int)ConsumeType.OrderBasedMaterial)
                    {
                        foreach (var stock in products.Where(x => x.MaterialCode == item.MaterialCode))
                        {
                            if (item.RequireQuantity > item.CompleteQuantity)
                            {
                                if (item.RequireQuantity > stock.Quantity)
                                {
                                    item.CompleteQuantity += (decimal)stock.Quantity;
                                    stock.Quantity = 0;
                                    continue;
                                }
                                else
                                {
                                    item.CompleteQuantity += item.RequireQuantity;
                                    stock.Quantity -= item.RequireQuantity;
                                    break;
                                }
                            }
                            else
                                break;
                        }

                    }

                    if (item.CompleteQuantity >= item.RequireQuantity)
                    {
                        item.IsPicked = "已完成";
                    }
                    else
                    {
                        item.IsPicked = "执行中";
                    }
                }
                return boms.OrderBy(x => x.ConsumeTypeDesc).ToList();
            }
            catch (Exception)
            {

                throw;
            }

        }

        public async Task<string> GetRequistStatus(int orderid)
        {
            var boms = await _workOrderBomItemService.GetListByOrderIdAync(orderid);
            if (boms.Where(x => x.ConsumeType == (int)ConsumeType.OrderBasedMaterial && x.SyncWMSStatus == 0 && x.RequiredQuantity > 0 && x.MovementAllowed == true).Count() == 0)
            {
                return RequistStatus.Yes.GetDescription();
            }
            else
            {
                if (boms.Where(x => x.ConsumeType == (int)ConsumeType.OrderBasedMaterial && x.SyncWMSStatus > 0 && x.RequiredQuantity > 0 && x.MovementAllowed == true).Count() > 0 && boms.Where(x => x.ConsumeType == (int)ConsumeType.OrderBasedMaterial && x.SyncWMSStatus == 0 && x.RequiredQuantity > 0 && x.MovementAllowed == true).Count() > 0)
                {
                    return RequistStatus.Partial.GetDescription();
                }
                else
                {
                    return RequistStatus.No.GetDescription();
                }
            }
        }

        public async Task<string> GetOPCompletedStatus(int orderid)
        {
            var process = (await _workOrderProcessService.GetListByOrderIdAync(orderid)).OrderBy(x => x.Operation).First();
            if (process != null && process.Status == ((int)WorkOrderStatus.Finished).ToString())
                return "已完成";
            else
                return "未完成";

        }


        private void OrderPickRequistForm_Load(object sender, EventArgs e)
        {
            keyboardInput.PlaceholderText = "请输入订单号...";
            startDatePicker.PlaceholderText = "请选择开工日期...";
            finishDatePicker.PlaceholderText = "请选择装配日期...";
        }

        private void queryButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(keyboardInput.Text) && startDatePicker.Value == null)
                    throw new Exception("请输入订单号或选择装配日期！");
                LoadData();
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, $"查询失败：{ex.Message}");
            }

        }

        private async void orderTable_ExpandChanged(object sender, TableExpandEventArgs e)
        {
            //if (e.Expand && e.Record is OrderPickRequistView data)
            //{
            //    var detail = await LoadDetailData(data.Id);


            //    data.Children = detail.ToArray();
            //}
        }

        private async void requistButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (orderTable.DataSource == null)
                    throw new Exception("未查询到可以领料的订单！");
                var data = (orderTable.DataSource as List<OrderPickRequistView>).Where(x => x.check).ToList();
                if (!data.Any())
                    throw new Exception("未选中订单！");

                if (AntdUI.Modal.open(this.ParentForm, "按单拣配物料领料", "即将向WMS推送按单拣配物料领料需求，是否继续？") != DialogResult.OK)
                    return;

                requistProgress.Visible = true;
                requistButton.Enabled = false;
                requistProgress.Value = 0;

                // 1. 批量获取 BOM
                var orderIds = data.Select(x => x.Id).ToList();
                var allBoms = await _workOrderBomItemService.GetListByOrderIdsAync(orderIds); // 批量获取
                var bomLookup = allBoms.ToLookup(b => b.WorkOrderId);

                // 2. 批量获取物料
                var materialCodes = allBoms.Select(x => x.MaterialCode).Distinct().ToList();
                var materials = await _materialViewService.GetListByCodesAsync(AppSession.CurrentUser.FactoryName, materialCodes);
                var materialDict = materials.ToDictionary(x => x.MaterialCode);

                int i = 0;
                var requestUrl = _apiSettings["JyApi"].Endpoints["TransvouchCreate"];
                StringBuilder message = new StringBuilder();

                foreach (var orderView in data)
                {
                    var bom = bomLookup[orderView.Id].Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true).ToList();
                    // ... 您的原有逻辑，但在内存中操作 ...
                    var bomtemp = bom.Where(x => x.ConsumeType == (int)ConsumeType.OrderBasedMaterial && x.SyncWMSStatus == 0).ToList();

                    if (bomtemp.Any())
                    {
                        // 填充 MaterialId
                        foreach (var item in bomtemp)
                        {
                            if (materialDict.TryGetValue(item.MaterialCode, out var matDto))
                                item.MaterialId = matDto.Id;
                        }

                        // 构建请求
                        int syncwms = bom.Where(x => x.ConsumeType == (int)ConsumeType.OrderBasedMaterial).Max(x => (int)x.SyncWMSStatus) + 1;
                        var request = new MaterialRequisitionRequest
                        {
                            VoucherNo = $"{orderView.OrderNumber}-{syncwms}",
                            VoucherTypeCategoryCode = "VTC022",
                            VoucherTypeName = "999",
                            OWarehouseCode = "1100",
                            IWarehouseCode = "2100",
                            OStorageCode = "11004",
                            IStorageCode = "21001",
                            VirtualOPlantCode = "CN11",
                            VirtualIPlantCode = "CN11",
                            EntryDtos = bomtemp.GroupBy(x => x.MaterialId)
                                .Select(g => new MaterialEntry { MaterialId = (int)g.Key, Qty = (decimal)g.Sum(x => x.RequiredQuantity) }).ToList(),
                            PlantCode = "CN11",
                            OperateUser = AppSession.CurrentUser.EmployeeId,
                            OperateUserName = AppSession.CurrentUser.UserName,
                            Remark = "按单"
                        };

                        // 调用 API (无法批量，只能循环)
                        var result = await _jyApiClient.PostAsync<MaterialRequisitionRequest, object>(requestUrl, request);

                        if (!result.IsSuccess)
                            message.AppendLine($"{orderView.OrderNumber}推送失败：{result.Message}");
                        else
                        {
                            // 批量更新当前订单的 BOM 状态
                            var update = bomtemp.Select(x => new WorkOrderBomItemUpdateDto { Id = x.Id, SyncWMSStatus = x.SyncWMSStatus + 1 }).ToList();
                            await _workOrderBomItemService.UpdateWmsStatusAsync(update);
                        }
                    }
                    requistProgress.Value = (float)++i / data.Count;
                }

                if (message.Length > 0)
                    // 4. 结果反馈
                    AntdUI.Modal.open(new AntdUI.Modal.Config(this.ParentForm, "处理结果", new AntdUI.Input
                    {
                        Text = message.ToString(),
                        Width = 350,
                        Height = 300,
                        BorderWidth = 0,
                        Multiline = true,
                        ReadOnly = true
                    }, AntdUI.TType.Info));
                else
                    AntdUI.Message.success(this.ParentForm, "领料操作完成！");
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this.ParentForm, $"领料失败：{ex.Message}");
            }
            finally
            {
                requistProgress.Visible = false;
                requistButton.Enabled = true;
            }
        }

        private async void packButton_Click(object sender, EventArgs e)
        {
            StringBuilder message = new StringBuilder();
            try
            {
                if (orderTable.DataSource == null)
                {
                    throw new Exception("未查询到可以合箱的订单！");
                }

                var data = (orderTable.DataSource as List<OrderPickRequistView>).Where(x => x.check);
                if (data == null || data.Count() == 0)
                {
                    throw new Exception("未选中订单，请先选择需要合箱的订单！");
                }

                if (AntdUI.Modal.open(this.ParentForm, "订单合箱", "即将对选中的订单进行合箱，是否继续？") == DialogResult.OK)
                {
                    requistProgress.Visible = true;
                    packButton.Enabled = false;

                    //再从wms同步一次库存

                    var workorders = string.Join(",", data.Select(x => x.OrderNumber)).TrimEnd(',');
                    var rtn = await _workOrderViewService.GetPickMtrStockByWorkOrderAsync(workorders);

                    var requestUrl = _apiSettings["MesApi"].Endpoints["WorkOrderPickVerfCommit"];
                    requistProgress.Value = 0;
                    int i = 0;
                    foreach (var item in data)
                    {
                        var process = (await _workOrderProcessService.GetListByOrderIdAync(item.Id)).OrderBy(x => x.Operation).First();
                        if (process.Status == ((int)WorkOrderStatus.Finished).ToString())
                        {
                            requistProgress.Value = (float)++i / data.Count();
                            continue;
                        }
                        else
                        {
                            var request = new
                            {
                                WorkOrderId = item.Id,
                                Status = "verfSuccess",
                                updateBy = AppSession.CurrentUser.EmployeeId,
                                updateOn = DateTime.Now,
                            };
                            var json = JsonConvert.SerializeObject(request);
                            var result = await _mesApiClient.PutAsync<object, object>(requestUrl, request);
                            if (!result.IsSuccess)
                                message.AppendLine($"{item.OrderNumber}合箱失败：" + result.Message);
                            else
                            {
                                item.IsPicked = "已完成";
                                item.CompleteQuantity = item.RequireQuantity;
                            }
                            requistProgress.Value = (float)++i / data.Count();
                        }
                    }

                    if (message.Length > 0)
                    {
                        AntdUI.Modal.open(new AntdUI.Modal.Config(this.ParentForm, "提示", new AntdUI.Input()
                        {
                            Text = message.ToString(),
                            Width = 350,
                            Height = 300,
                            BorderWidth = 0,
                            Dock = DockStyle.Bottom,
                            Multiline = true,
                        }, AntdUI.TType.Warn)
                        {
                            CancelText = null
                        });

                    }
                    else
                    {
                        AntdUI.Message.success(this.ParentForm, "合箱操作完成！");
                    }
                }



            }
            catch (Exception ex)
            {

                AntdUI.Message.error(this.ParentForm, $"合箱失败：{ex.Message}");
            }
            finally
            {
                requistProgress.Visible = false;
                packButton.Enabled = true;
                requistProgress.Value = 0;

            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (AntdUI.Modal.open(this.ParentForm, "提示", "是否导出当前列表中的数据源？", TType.Warn) == DialogResult.OK)
            {
                try
                {
                    if (orderTable.DataSource == null)
                    {
                        throw new Exception("未查询到可以导出的订单！");
                    }
                    var data = orderTable.DataSource as List<OrderPickRequistView>;
                    if (data == null || data.Count() == 0)
                    {
                        throw new Exception("当前列表中无数据可供导出！");
                    }
                    ExcelExportHelper.ExportToExcel(this.ParentForm, data.Select(x => new { x.OrderNumber,x.MaterialCode,x.MaterialDesc,x.RequireQuantity,x.CompleteQuantity,x.StartDate,x.DispatchDate,x.ProfitCenter,x.LeadingOrder,x.LeadingMaterial,x.SyncWms,x.IsPicked}), "订单领料&合箱状态");
                }
                catch (Exception ex)
                {
                    AntdUI.Message.error(this.ParentForm, "导出失败：" + ex.Message);
                }
            }
        }
    }

    public class OrderPickRequistView : AntdUI.NotifyProperty
    {

        public OrderPickRequistView(WorkOrderDto dto)
        {
            _check = false;
            _id = dto.Id;
            _factoryCode = dto.FactoryCode;
            _orderNumber = dto.OrderNumber;
            _materialCode = dto.MaterialCode;
            _materialDesc = dto.MaterialDesc;
            _requireQuantity = dto.Quantity;
            _completeQuantity = 0;
            _dispatchDate = dto.DispatchDate;
            _startDate = dto.ScheduledStartDate;
            _profitCenter = dto.ProfitCenter;
            _leadingOrder = dto.LeadingOrder;
            _leadingMaterial = dto.LeadingOrderMaterial;
            _children = new OrderPickDetailView[1];

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

        decimal? _completeQuantity;
        public decimal? CompleteQuantity
        {
            get => _completeQuantity;
            set
            {
                if (_completeQuantity == value)
                    return;
                _completeQuantity = value;
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

        string? _syncWms;
        public string? SyncWms
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

        string? _isPicked;
        public string? IsPicked
        {
            get => _isPicked;
            set
            {
                if (_isPicked == value)
                    return;
                _isPicked = value;
                OnPropertyChanged();
            }
        }

        OrderPickDetailView[] _children;
        public OrderPickDetailView[] Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }


    }

    public class OrderPickDetailView : AntdUI.NotifyProperty
    {
        public OrderPickDetailView(WorkOrderBomItemDto dto)
        {
            _workOrderProcessId= dto.WorkOrderProcessId;
            _bomItem = dto.BomItem;
            _materialCode = dto.MaterialCode;
            _materialDesc = dto.MaterialDesc;
            _requireQuantity = dto.RequiredQuantity;
            _consumeType = dto.ConsumeType;
            _completeQuantity = 0;
            _syncWms = dto.SyncWMSStatus > 0 ? "已领料" : "未领料";
            _isPicked = string.Empty;
            _consumeTypeDesc = string.Empty;
        }
        int _workOrderProcessId;
        public int WorkOrderProcessId
        {
            get => _workOrderProcessId;
            set
            {
                if (_workOrderProcessId == value)
                    return;
                _workOrderProcessId = value;
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

        decimal? _completeQuantity;
        public decimal? CompleteQuantity
        {
            get => _completeQuantity;
            set
            {
                if (_completeQuantity == value)
                    return;
                _completeQuantity = value;
                OnPropertyChanged();
            }
        }

        string? _syncWms;
        public string? SyncWms
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

        string? _isPicked;
        public string? IsPicked
        {
            get => _isPicked;
            set
            {
                if (_isPicked == value)
                    return;
                _isPicked = value;
                OnPropertyChanged();
            }
        }

        int? _consumeType;
        public int? ConsumeType
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

        string? _consumeTypeDesc;
        public string? ConsumeTypeDesc
        {
            get => _consumeTypeDesc;
            set
            {
                if (_consumeTypeDesc == value)
                    return;
                _consumeTypeDesc = value;
                OnPropertyChanged();
            }
        }
    }
}
