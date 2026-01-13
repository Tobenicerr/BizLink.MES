using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using SqlSugar;
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
    public partial class MaterialShortageReportForm : BaseForm
    {
        private readonly ICenterStockOutService _centerStockOutService;
        private readonly IAutoStockOutService _autoStockOutService;
        private readonly IWorkOrderService _workOrderService;
        public MaterialShortageReportForm(ICenterStockOutService centerStockOutService, IAutoStockOutService autoStockOutService, IWorkOrderService workOrderService)
        {
            InitializeComponent();
            InitializeTable();
            _centerStockOutService = centerStockOutService;
            _autoStockOutService = autoStockOutService;
            _workOrderService = workOrderService;
        }

        private void MaterialShortageReportForm_Load(object sender, EventArgs e)
        {
            keywordInput.PlaceholderText = "请输入订单号...";
        }

        private async void queryButton_Click(object sender, EventArgs e)
        {
            queryButton.Loading = true;
            spin.Visible = true;
            try
            {
                var count = await LoadDataAsync();
                if (count > 0)
                {
                    AntdUI.Message.success(this, $"查询成功，共查询到 {count} 条数据！");
                }
                else
                {
                    AntdUI.Message.info(this, "未查询到相关数据！");
                    orderTable.DataSource = null;

                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, $"查询出错：{ex.Message}");
            }
            finally
            {
                queryButton.Loading = false;
                spin.Visible = false;

            }
        }

        private void pagination_ValueChanged(object sender, PagePageEventArgs e)
        {

        }

        private async Task<int> LoadDataAsync()
        {
            // 1. 预先处理 UI 输入，避免后续跨线程访问，并处理分隔符
            var rawKeywords = keywordInput.Text;
            var keywords = new HashSet<string>(
                rawKeywords.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries),
                StringComparer.OrdinalIgnoreCase // 忽略大小写比较
            );
            var hasKeywords = keywords.Count > 0;

            // 2. 并行获取两个仓库的数据 (Task.WhenAll)
            var centerStockTask = _centerStockOutService.GetAllAsync();
            var autoStockTask = _autoStockOutService.GetAllAsync();

            await Task.WhenAll(centerStockTask, autoStockTask);


            var centerStockShort = centerStockTask.Result;
            var autoStockShort = autoStockTask.Result;

            //var centerStockShort = await _centerStockOutService.GetAllAsync();
            //var autoStockShort = await _autoStockOutService.GetAllAsync();

            // 3. 统一数据格式 (CommonStockDto)
            // 注意：如果数据量极大，这里建议使用迭代器 yield return 或 Capacity 预设
            var unionStockList = new List<CommonStockDto>(centerStockShort.Count() + autoStockShort.Count());

            unionStockList.AddRange(centerStockShort.Where(c => c.Status == "-1").Select(c => new CommonStockDto
            {
                WorkOrderNo = c.WorkOrderNo,
                StockType = "中央仓",
                MaterialCode = c.MaterialCode,
                Quantity = c.Quantity,
                PickingQuantity = c.PickingQuantity,
                LastQuantity = c.LastQuantity,
                UseageQuantity = c.UseageQuantity,
                Status = c.Status,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                UpdatedAt = c.UpdatedAt,
                UpdateBy = c.UpdateBy,
            }));

            unionStockList.AddRange(autoStockShort.Select(a => new CommonStockDto
            {
                WorkOrderNo = a.WorkOrderNo,
                StockType = "自动仓",
                MaterialCode = a.MaterialCode,
                Quantity = a.Quantity,
                PickingQuantity = a.PickingQuantity,
                LastQuantity = a.LastQuantity,
                UseageQuantity = a.UseageQuantity,
                Status = a.Status,
                CreatedAt = a.CreatedAt,
                CreatedBy = a.CreatedBy,
                UpdatedAt = a.UpdatedAt,
                UpdateBy = a.UpdateBy,
            }));

            // 4. 【关键优化】如果有关键字，立刻在内存中过滤库存数据
            // 这样可以避免去查询无关 WorkOrder 的详情
            if (hasKeywords)
            {
                // 这里假设 keyword 是工单号或物料号，根据实际需求调整 Where 条件
                unionStockList = unionStockList
                    .Where(x => keywords.Contains(x.WorkOrderNo) || keywords.Contains(x.MaterialCode))
                    .ToList();
            }

            if (unionStockList.Count == 0)
                return 0;

            // 5. 提取去重后的工单号
            var distinctWorkOrderNos = unionStockList
                .Select(x => x.WorkOrderNo)
                .Distinct()
                .ToList();

            // 6. 批量获取工单详情
            // 此时 distinctWorkOrderNos 已经是经过关键字过滤后的子集，数量最少
            var workOrderDtos = await _workOrderService.GetByOrdrNoAsync(distinctWorkOrderNos);

            // 7. 组装 ViewModel (内存 Join)
            var viewModels = workOrderDtos
                       .Where(o => o.FactoryId == AppSession.CurrentFactoryId) // 过滤工厂
                                                                               // 1. 使用 GroupJoin 将工单和库存关联，此时 stocks 是一个列表
                       .GroupJoin(
                           unionStockList,
                           order => order.OrderNumber,
                           stock => stock.WorkOrderNo,
                           (order, stocks) => new { Order = order, Stocks = stocks } // 注意：这里保留整个 stocks 列表，不要用 FirstOrDefault
                       )
                       // 2. 关键步骤：使用 SelectMany 将“一对列表”展开为“多行”
                       .SelectMany(
                           temp => temp.Stocks.DefaultIfEmpty(), // DefaultIfEmpty 确保左连接（即使没有库存也保留工单行）
                           (temp, stock) => new { temp.Order, Stock = stock } // 将工单与单个库存项配对
                       )
                       // 3. 确保 Stock 不为空 (这实际上将其变成了 Inner Join)
                       // 如果您希望保留没有库存的工单，请删除此 Where，并在下面的 Select 中处理 Stock 为 null 的情况
                       .Where(x => x.Stock != null)
                       .Select(x => new MaterialShortageReportViewModel
                       {
                           WorkOrderNo = x.Order.OrderNumber,
                           UsageStatus = (x.Stock.UseageQuantity ?? 0) >= ((x.Stock.LastQuantity ?? 0) < 0 ? 0 : x.Stock.LastQuantity ?? 0) ? "可补料":"不可补料",
                           StartDate = x.Order.ScheduledStartDate,
                           DispatchDate = x.Order.ScheduledFinishDate,
                           MaterialCode = x.Order.MaterialCode,
                           Quantity = x.Order.Quantity,
                           LabelCount = x.Order.LabelCount,
                           ProfitCenter = x.Order.ProfitCenter,
                           LeadingMaterial = x.Order.LeadingOrderMaterial,
                           // 注意：此时 x.Stock 已经不是 null 了 (因为上面的 Where)
                           // 如果移除了上面的 Where，这里需要用 x.Stock?.Status 来防空
                           Status = x.Stock.Status == "-1" ? "缺料" : (x.Stock.Status == "2" ? "强制关闭" : "正常拣配"),

                           // 库存相关数据
                           PickingMaterial = x.Stock.MaterialCode,
                           PlanQuantity = x.Stock.Quantity,
                           PickedQuantity = x.Stock.PickingQuantity ?? 0,
                           LastQuantity = (x.Stock.LastQuantity ?? 0) < 0 ? 0 : x.Stock.LastQuantity ?? 0,
                           StockType = x.Stock.StockType,
                           UsageQuantity = x.Stock.UseageQuantity ?? 0
                       })
                       .OrderByDescending(x => x.StartDate)
                       .ToList();

            orderTable.DataSource = viewModels;
            return viewModels.Count;
        }


        private void InitializeTable()
        {

            orderTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("WorkOrderNo", "订单号", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("UsageStatus", "补料状态", AntdUI.ColumnAlign.Center){
                    Render = (value, record, index) =>
                    {
                        switch (value as string)
                        {
                            case "可补料":
                                return new AntdUI.CellBadge(AntdUI.TState.Success,"")
                                {
                                    DotRatio = 0.8f
                                };

                            case "不可补料":
                                return new AntdUI.CellBadge(AntdUI.TState.Error,"")        
                                {
                                    DotRatio = 0.8f
                                };;

                            default:
                                return null;
                        }
                    }
                }.SetFixed().SetWidth("auto").SetAlign().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("StartDate", "开工日期", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("DispatchDate", "装配日期", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetDisplayFormat("yyyy-MM-dd").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        switch (value as string)
                        {
                            case "缺料":
                                return new AntdUI.CellTag("缺料", AntdUI.TTypeMini.Error);

                            case "正常拣配":
                                return new AntdUI.CellTag("正常拣配", AntdUI.TTypeMini.Success);

                            case "强制关闭":
                                return new AntdUI.CellTag("强制关闭", AntdUI.TTypeMini.Primary);
                            default:
                                return null;
                        }
                    }
                }.SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("StockType", "物料属性", AntdUI.ColumnAlign.Center)     {
                    Render = (value, record, index) =>
                    {
                        switch (value as string)
                        {

                            case "自动仓":
                                return new AntdUI.CellTag("自动仓", AntdUI.TTypeMini.Success);

                            case "中央仓":
                                return new AntdUI.CellTag("中央仓", AntdUI.TTypeMini.Primary);
                            default:
                                return null;
                        }
                    }
                }.SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("PickingMaterial", "缺料物料", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("PlanQuantity", "计划数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("LastQuantity", "缺料数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("PickedQuantity", "已拣数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("LeadingMaterial", "成品料号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("LabelCount", "标签数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDisplayFormat("F0").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetWidth("auto").SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("UsageQuantity", "可用库存", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetDisplayFormat("F0").SetDefaultFilter().SetSortOrder().SetLocalizationTitleID("Table.Column."),

             };

        }

        private class CommonStockDto
        {
            public string? WorkOrderNo
            {
                get; set;
            }

            public string? StockType
            {
                get; set;
            }


            public string? MaterialCode
            {
                get; set;
            }

            public decimal? Quantity
            {
                get; set;
            }


            public decimal? PickingQuantity
            {
                get; set;
            }
            public decimal? LastQuantity
            {
                get; set;
            }

            public decimal? UseageQuantity
            {
                get; set;
            }

            public string? Status
            {
                get; set;
            }


            public DateTime? CreatedAt
            {
                get; set;
            }


            public string? CreatedBy
            {
                get; set;
            }

            public DateTime? UpdatedAt
            {
                get; set;
            }

            public string? UpdateBy
            {
                get; set;
            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            try
            {
                exportButton.Loading = true;
                var data = orderTable.DataSource as List<MaterialShortageReportViewModel>;
                if (data == null || data.Count() <= 0)
                    throw new Exception("未查询到可以导出的数据源！");

                if (AntdUI.Modal.open(this.ParentForm, "提示", "是否导出缺料清单？", TType.Warn) == DialogResult.OK)
                {
                    ExcelExportHelper.ExportToExcel(this.ParentForm, data, $"仓库缺料清单{DateTime.Now.ToString("yyyyMMdd")}");
                }
            }
            catch (Exception ex)
            {

                AntdUI.Message.error(this.ParentForm, $"导出失败：{ex.Message}");
            }

            finally
            {
                exportButton.Loading = false;

            }
        }

        private class MaterialShortageReportViewModel()
        {
            public string? WorkOrderNo
            {
                get; set;
            }

            public string? UsageStatus
            {
                get; set;
            }
            public DateTime? StartDate
            {
                get; set;
            }
            public DateTime? DispatchDate
            {
                get; set;
            }
            public string? MaterialCode
            {
                get; set;
            }
            public decimal? Quantity
            {
                get;
                set;
            }
            public decimal? LabelCount
            {
                get;
                set;
            }
            public string? ProfitCenter
            {
                get;
                set;
            }
            public string? LeadingMaterial
            {
                get;
                set;
            }
            // 注意：此时 x.Stock 已经不是 null 了 (因为上面的 Where)
            // 如果移除了上面的 Where，这里需要用 x.Stock?.Status 来防空
            public string? Status
            {
                get;
                set;
            }

            // 库存相关数据
            public string? PickingMaterial
            {
                get;
                set;
            }
            public decimal? PlanQuantity
            {
                get;
                set;
            }
            public decimal? PickedQuantity
            {
                get;
                set;
            }
            public decimal? LastQuantity
            {
                get;
                set;
            }
            public string? StockType
            {
                get;
                set;
            }
            public decimal? UsageQuantity
            {
                get;
                set;
            }
        }

    }
}
