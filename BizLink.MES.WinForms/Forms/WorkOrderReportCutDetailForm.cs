using AntdUI;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Helper;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Helpers;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using BizLink.MES.WinForms.Properties;
using Dm.util;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace BizLink.MES.WinForms.Forms
{
    // 3. 继承 MesBaseForm
    public partial class WorkOrderReportCutDetailForm : MesBaseForm
    {
        #region Fields & Services

        private readonly CutModuleFacade _facade;
        private readonly IFormFactory _formFactory;

        private readonly IServiceScopeFactory _scopeFactory; // 【新增】用于创建临时事务Scope

        // 数据状态
        private WorkOrderTaskDto _workOrderTaskDto;
        private int _workStationId;
        private WorkStationDto _workstation; // 缓存工位信息
        private List<WorkOrderBomItemDto> _workOrderBoms = new List<WorkOrderBomItemDto>();

        // Events
        public delegate Task DataUpdateHandler();
        public event DataUpdateHandler LoadOrderData;

        // Constants
        private const string Group_CableCutScrapReason = "CableCutScrapReason";
        private const string Group_CableSampleRunScrapLen = "CableSampleRunScrapLen";
        private const string Group_SapLocation = "CN11SAPStockLocation";
        private const string Group_BagMaterialList = "BagMaterialList";
        private const string Key_DefaultLength = "DefaultLength";
        private const string Key_CableRawLineStock = "CableRawLineStock";
        private const string Key_CableProLineStock = "CableProLineStock";
        private const string Key_SAPRawMtrStock = "SAPRawMtrStock";

        #endregion

        // 4. 构造函数极大简化
        public WorkOrderReportCutDetailForm(CutModuleFacade facade, IServiceScopeFactory scopeFactory, IFormFactory formFactory)
        {
            _facade = facade;
            InitializeComponent();
            InitializeTable();
            _scopeFactory = scopeFactory;
            _formFactory = formFactory;
        }
        private async void WorkOrderReportDetailForm_Load(object sender, EventArgs e)
        {
            // 使用 RunAsync 处理加载过程的异常
            //await RunAsync(async () =>
            //{
            //    // 加载工位信息
            //    _workstation = await _facade.WorkStation.GetByIdAsync(_workStationId);

            //    await LoadConfigAsync();
            //    await LoadDataAsync();
            //});
        }

        // 【修复点】改用重写 OnLoad 方法，确保加载逻辑 100% 被执行
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e); // 务必调用基类

            // 如果 InitData 没有被正确调用，防止空引用
            if (_workOrderTaskDto == null)
                return;

            // 使用 RunAsync 处理加载过程的异常 (无需按钮)
            await RunAsync(async () =>
            {
                // 1. 加载工位信息
                _workstation = await _facade.WorkStation.GetByIdAsync(_workStationId);

                // 2. 加载配置和业务数据
                await LoadConfigAsync();
                await LoadDataAsync();
            });
        }


        // 5. 初始化数据入口
        public void InitData(WorkOrderTaskDto task, int workstationId)
        {
            _workOrderTaskDto = task;
            _workStationId = workstationId;
        }

        private async Task LoadConfigAsync()
        {
            // 使用 _facade.Params
            var scrapReasonGroup = await _facade.Params.GetGroupWithItemsAsync(Group_CableCutScrapReason);
            scrapResionSelect.Items.Clear();
            if (scrapReasonGroup != null)
            {
                foreach (var item in scrapReasonGroup.Items)
                {
                    scrapResionSelect.Items.Add(new MenuItem
                    {
                        Name = item.Value,
                        Text = $"{item.Value}-{item.Name}"
                    });
                }
            }

            var scrapLenGroup = await _facade.Params.GetGroupWithItemsAsync(Group_CableSampleRunScrapLen);
            var defaultLengthItem = scrapLenGroup?.Items.FirstOrDefault(x => x.Key == Key_DefaultLength);
            string lenValue = defaultLengthItem?.Value ?? "250";
            fixedScraptLabel.Text = $"+{lenValue}";
        }


        private async Task LoadDataAsync()
        {
            // 1. 刷新任务信息
            _workOrderTaskDto = await _facade.Task.GetByIdAsync(_workOrderTaskDto.Id);

            // 2. 获取 BOM 和记录
            // 注意：这里使用了 Task.WhenAll 并行加载，提高性能
            // 原因：同一个 Scope 下的 DbContext 是非线程安全的，不能使用 Task.WhenAll 并行查询，否则会报 Reader Closed 错误。
            var allBomItems = await _facade.WorkOrderBomItemService.GetListByOrderIdAync(_workOrderTaskDto.OrderId);
            var allRecords = await _facade.MaterialAdd.GetByTaskIdAsync(_workOrderTaskDto.Id);
            var confirmList = await _facade.Confirm.GetListByTaskIdAsync(_workOrderTaskDto.Id);


            UpdateTaskInfoLabels();

            // 3. 处理 BOM 和记录
            _workOrderBoms = allBomItems
                .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true && x.ConsumeType == 0 &&
                            x.BomItem.Equals(_workOrderTaskDto.MaterialItem) && x.MaterialCode == _workOrderTaskDto.MaterialCode)
                .ToList();

            List<WorkOrderTaskMaterialAddDto> displayRecords;
            bool isFinished = _workOrderTaskDto.Status == ((int)WorkOrderStatus.Finished).ToString() ||
                              (_workOrderTaskDto.Quantity == _workOrderTaskDto.CompletedQty && _workOrderTaskDto.Quantity > 0);

            if (isFinished)
            {
                displayRecords = allRecords.Where(x => x.Status == "0")
                    .GroupBy(x => x.BarCode)
                    .Select(g => g.OrderByDescending(item => item.Id).First())
                    .ToList();
            }
            else
            {
                displayRecords = allRecords.Where(x => x.Status == "1").ToList();
            }

            // 4. 绑定 BOM 表格
            var tableData = _workOrderBoms.GroupJoin(
                displayRecords,
                x => new { Item = x.BomItem, Material = x.MaterialCode },
                y => new { Item = y.BomItem, Material = y.MaterialCode },
                (bom, recs) => new { bom, recs }
            ).SelectMany(
                x => x.recs.DefaultIfEmpty(),
                (x, y) => new WorkOrderTaskMaterialAddView(_workOrderTaskDto, x.bom, y)
            ).ToList();

            bomTable.DataSource = tableData;

            // 5. 更新状态图标和焦点
            bool hasUnloadedMaterial = tableData.Any(x => string.IsNullOrWhiteSpace(x.BarCode));
            if (!hasUnloadedMaterial && !isFinished)
            {
                materialFlagPicture.Image = (Image)Resources.ResourceManager.GetObject("success");
            }
            else
            {
                materialFlagPicture.Image = (Image)Resources.ResourceManager.GetObject("error");
                if (!isFinished)
                {
                    await AutoFillLastBarcodeAsync();
                }
            }

            // 6. 绑定报工记录表格
            confirmTable.DataSource = confirmList.OrderByDescending(x => x.ConfirmDate).ToList();
        }

        private async void submitButton_Click(object sender, EventArgs e)
        {
            // 使用 RunAsync 自动处理 Loading、禁用、异常
            await RunAsync(submitButton, async () =>
            {
                // --- 1. 基础校验 ---
                if (reportInputNumber.Value <= 0)
                    throw new Exception("请填写报工数量！");
                if (scraptInputNumber.Value > 0 && scrapResionSelect.SelectedValue == null)
                    throw new Exception("请选择报废原因！");

                var materialList = bomTable.DataSource as List<WorkOrderTaskMaterialAddView>;
                if (materialList.Any(x => string.IsNullOrWhiteSpace(x.BarCode)))
                    throw new Exception("物料箱中存在未上料的物料，请检查！");

                // --- 2. 消耗校验 ---
                var targetMaterial = materialList.FirstOrDefault(x => x.MaterialCode.Equals(_workOrderTaskDto.MaterialCode));
                if (targetMaterial == null)
                    throw new Exception("未查询到上料批次！");

                var materialEntity = (await _facade.MaterialAdd.GetByTaskIdAsync(_workOrderTaskDto.Id))
                    .FirstOrDefault(x => x.Status == "1" && x.BarCode == targetMaterial.BarCode);

                if (materialEntity == null || materialEntity.LastQuantity <= 0)
                    throw new Exception($"{targetMaterial.MaterialCode} {targetMaterial.BarCode}已被全部消耗，请重新上料！");

                var scrapLenGroup = await _facade.Params.GetGroupWithItemsAsync(Group_CableSampleRunScrapLen);
                var defaultLenItem = scrapLenGroup.Items.FirstOrDefault(x => x.Key == Key_DefaultLength);
                decimal defaultScrap = defaultLenItem != null && !string.IsNullOrEmpty(defaultLenItem.Value) ? Convert.ToInt32(defaultLenItem.Value) : 250m;

                decimal totalConsumption = (_workOrderTaskDto.CableLengthUsl ?? 0) / 1000m * reportInputNumber.Value + (scraptInputNumber.Value + defaultScrap) / 1000m;
                if (materialEntity.LastQuantity < totalConsumption)
                {
                    throw new Exception($"本次报工即将消耗 {totalConsumption:F2} M，料箱剩余数量 ({materialEntity.LastQuantity:F2}) 无法满足！");
                }

                // --- 3. 任务状态校验 ---
                var currentTask = await _facade.Task.GetByIdAsync(_workOrderTaskDto.Id);
                if (currentTask.CompletedQty + reportInputNumber.Value > currentTask.Quantity)
                    throw new Exception("报工数量已超出任务数量！");

                // 自动完工判定
                bool isComplete = completeSwitch.Checked;
                if (currentTask.CompletedQty + reportInputNumber.Value == currentTask.Quantity)
                {
                    isComplete = true;
                    this.Invoke((MethodInvoker)delegate {
                        completeSwitch.Checked = true;
                    });
                }

                // --- 4. 构建数据对象 ---
                var isPrintProcessCard = (await _facade.Confirm.GetListByOrderNoAsync(_workOrderTaskDto.OrderNumber)).Any();

                var taskUpdateDto = new WorkOrderTaskUpdateDto
                {
                    Id = currentTask.Id,
                    Status = isComplete ? ((int)WorkOrderStatus.Finished).ToString() : "2",
                    CompletedQty = currentTask.CompletedQty + reportInputNumber.Value,
                    UpdateBy = AppSession.CurrentUser.EmployeeId,
                    UpdateOn = DateTime.Now,
                };

                var confirmDto = new WorkOrderTaskConfirmCreateDto
                {
                    TaskId = _workOrderTaskDto.Id,
                    WorkStationId = _workStationId,
                    ConfirmNumber = _facade.Serial.GenerateNext("ReportLabelSerial"), // 假设 Facade 有 SerialService
                    ConfirmDate = DateTime.Now,
                    ConfirmQuantity = reportInputNumber.Value,
                    EmployerCode = AppSession.CurrentUser.EmployeeId,
                    Status = taskUpdateDto.Status == ((int)WorkOrderStatus.Finished).ToString() ? "1" : "0",
                    Remark = reportRemarkInput.Text.Trim(),
                };

                // 5. 构建消耗明细
                var consums = BuildConsumptionList(targetMaterial, defaultScrap, defaultLenItem?.Name ?? "0013", isPrintProcessCard);

                // --- 6. 【关键】事务提交 (Facacde -> Service) ---
                using (var transactionScope = _scopeFactory.CreateScope())
                {
                    // 1. 从新 Scope 中获取全新的 Facade
                    // 这个 Facade 里的 UnitOfWork 是全新的，干净的
                    var transactionalFacade = transactionScope.ServiceProvider.GetRequiredService<CutModuleFacade>();

                    // 2. 使用全新的 Facade 执行事务性业务
                    await transactionalFacade.Confirm.CreateByCableAsync(confirmDto, consums, taskUpdateDto, AppSession.CurrentFactoryId);

                    // 3. 后置操作 (SAP & 打印)
                    // 这些操作通常也可以用新 Scope，或者沿用旧 Scope，只要不涉及刚才那个已提交的事务即可
                    try
                    {
                        var consumsSum = consums.GroupBy(g => new { g.MaterialCode, g.EntryUnitCode, g.BatchCode })
                            .Select(group => new { group.Key, Qty = group.Sum(x => x.EntryQuantity) }).First();

                        // 注意：这里依然可以使用 transactionalFacade，或者原来的 _facade，建议使用 transactionalFacade 保持一致性
                        await SyncInventoryToSapAsync(consumsSum.Key.MaterialCode, consumsSum.Key.BatchCode, consumsSum.Key.EntryUnitCode, (decimal)consumsSum.Qty);
                        await HandlePrintingAsync(confirmDto, consums, isPrintProcessCard);
                    }
                    catch (Exception ex)
                    {
                        AntdUI.Message.warn(this, "报工成功，但SAP同步或打印失败: " + ex.Message);
                    }
                }

                // --- 8. 重置界面 ---
                if (isComplete)
                {
                    AntdUI.Message.success(this, "当前任务已完工！");
                }
                else
                {
                    reportInputNumber.Value = 0;
                    scraptInputNumber.Value = 0;
                    scrapResionSelect.Text = string.Empty;
                    reportRemarkInput.Text = string.Empty;
                    AntdUI.Message.success(this, "报工提交成功！");
                }

                // 刷新数据
                await LoadDataAsync();

            }, confirmMsg: "确认提交本次报工？");
        }

        #region Scanning Logic

        private async void barcodeInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r')
                return;
            string barcode = barcodeInput.Text.Trim();
            if (string.IsNullOrEmpty(barcode))
                return;

            // 使用 RunAsync 处理扫描逻辑
            await RunAsync(async () =>
            {
                await HandleBarcodeScanAsync(barcode);
            });

            barcodeInput.SelectAll();
        }

        private async Task HandleBarcodeScanAsync(string barcode)
        {
            var currentTask = await _facade.Task.GetByIdAsync(_workOrderTaskDto.Id);
            if (currentTask.Status == ((int)WorkOrderStatus.Finished).ToString())
                throw new Exception("当前任务已完工，无法继续上料！");

            var record = await _facade.MaterialAdd.GetByBarcodeAsync(barcode);

            if (record != null)
            {
                if (record.TaskId == _workOrderTaskDto.Id)
                    throw new Exception("当前标签已上料，请勿重复扫描标签！");

                var conflictTask = await _facade.Task.GetByIdAsync(record.TaskId);
                if (conflictTask == null || conflictTask.Status == ((int)WorkOrderStatus.Finished).ToString())
                {
                    await _facade.MaterialAdd.UpdateAsync(new WorkOrderTaskMaterialAddUpdateDto
                    {
                        Id = record.Id,
                        Status = "0",
                        UpdateOn = DateTime.Now,
                        UpdateBy = AppSession.CurrentUser.EmployeeId
                    });
                }
                else
                {
                    throw new Exception($"当前物料标签正在被订单 {conflictTask.OrderNumber} 使用，无法重复上料！");
                }
            }
            else
            {
                var workcenter = await _facade.WorkCenter.GetByCodeAsync(_workOrderTaskDto.WorkCenter);
                if (workcenter == null || !workcenter.LineStockId.HasValue)
                    throw new Exception($"工作中心{_workOrderTaskDto.WorkCenter}配置错误！");

                var stock = await _facade.RawStock.GetByBarCodeAsync(AppSession.CurrentFactoryId, barcode);
                if (stock == null)
                    throw new Exception("未查询到标签信息！");

                if (stock.LocationId != workcenter.LineStockId)
                    throw new Exception($"标签在 {stock.LocationCode}，请先移库到断线线边！");
            }

            var freshStock = await _facade.RawStock.GetByBarCodeAsync(AppSession.CurrentFactoryId, barcode);

            if(freshStock.SapStatus == null || freshStock.SapStatus != "2")
                throw new Exception("标签物料尚未完成PDA收货，请先在PDA上进行收货！");
            await ProcessMaterialLoadingAsync(freshStock);
        }

        private async Task ProcessMaterialLoadingAsync(RawLinesideStockDto stock)
        {
            var dataList = bomTable.DataSource as List<WorkOrderTaskMaterialAddView>;
            if (dataList == null)
                throw new Exception("BOM信息未加载！");

            var targetItem = dataList.FirstOrDefault(x => x.MaterialCode.Equals(stock.MaterialCode));
            if (targetItem == null)
                throw new Exception("标签对应物料与订单BOM不匹配！");

            // 1. 下料旧批次
            var existingLogs = (await _facade.MaterialAdd.GetByTaskIdAsync(_workOrderTaskDto.Id))
                .Where(x => x.Status == "1" && x.MaterialCode.Equals(stock.MaterialCode)).ToList();

            foreach (var item in existingLogs)
            {
                await _facade.MaterialAdd.UpdateAsync(new WorkOrderTaskMaterialAddUpdateDto
                {
                    Id = item.Id,
                    Status = "0",
                    UpdateOn = DateTime.Now,
                    UpdateBy = AppSession.CurrentUser.EmployeeId
                });
            }

            // 2. 上料新批次
            var newRecord = await _facade.MaterialAdd.CreateAsync(new WorkOrderTaskMaterialAddCreateDto
            {
                TaskId = _workOrderTaskDto.Id,
                MaterialCode = targetItem.MaterialCode,
                MaterialDesc = targetItem.MaterialDesc,
                BomItem = targetItem.BomItem,
                BatchCode = stock.BatchCode,
                BarCode = stock.BarCode,
                Quantity = stock.LastQuantity,
                LastQuantity = stock.LastQuantity,
                WorkStationId = _workStationId,
                CreateBy = AppSession.CurrentUser.EmployeeId
            });

            // 3. SAP 移库
            if (stock.SapStatus != "2")
            {

                //判断当前ID是否有SAP移库，如无则进行SAP移库
                var locationGroup = await _facade.Params.GetGroupWithItemsAsync("CN11SAPStockLocation");
                string cableLineLocation = locationGroup?.Items.FirstOrDefault(x => x.Key == "CableRawLineStock")?.Value ?? "2200";
                var sapTransderLogs = await _facade.SapTransferLog.GetListByStockIdAsync(stock.Id);
                if (sapTransderLogs == null || !sapTransderLogs.Where(x => x.MaterialCode == stock.MaterialCode && x.BatchCode == stock.BatchCode && x.ToLocationCode == cableLineLocation).Any())
                {
                    bool sapSuccess = await TransferRawMaterialToLineSideAsync(stock);
                }

                //if (sapSuccess)
                //{
                //    await _facade.RawStock.UpdateAsync(new RawLinesideStockUpdateDto { Id = stock.Id, SapStatus = "2" });
                //}
            }

            // 4. 刷新界面
            await LoadDataAsync();
        }

        #endregion
        private void InitializeTable()
        {
            bomTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter(),
                new AntdUI.Column("BatchCode", "物料批次", AntdUI.ColumnAlign.Center).SetFixed(),
                new AntdUI.Column("BarCode", "批次标签", AntdUI.ColumnAlign.Center).SetFixed(),
                new AntdUI.Column("LastQuantity", "上料数量", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("Auto"),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetDefaultFilter(),
                new AntdUI.Column("ReqQuantity", "需求数量", AntdUI.ColumnAlign.Center).SetColAlign(),
                new AntdUI.Column("BomItem", "BOM行", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("BaseUnit", "物料单位", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto"),
            };

            confirmTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("ConfirmNumber", "报工序号", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("ConfirmQuantity", "报工数量", AntdUI.ColumnAlign.Center).SetDisplayFormat("F"),
                new AntdUI.Column("ConfirmDate", "报工日期", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("EmployerCode", "报工人员", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("Remark", "备注", AntdUI.ColumnAlign.Center),
            };
        }

        //private async Task LoadDataAsync()
        //{
        //    // 1. 获取数据
        //    // 这里可以直接使用获取到的最新 Task 信息
        //    _workOrderTaskDto = await _workOrderTaskService.GetByIdAsync(_workOrderTaskDto.Id);
        //    var allBomItems = await _workOrderBomService.GetListByOrderIdAync(_workOrderTaskDto.OrderId);
        //    var allRecords = await _workOrderTaskMaterialAddService.GetByTaskIdAsync(_workOrderTaskDto.Id);
        //    var confirmList = await _workOrderTaskConfirmService.GetListByTaskIdAsync(_workOrderTaskDto.Id);

        //    // 2. 更新界面 Label
        //    UpdateTaskInfoLabels();

        //    // 3. 处理 BOM 和记录
        //    _workOrderBoms = allBomItems
        //        .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true && x.ConsumeType == 0 &&
        //                    x.BomItem.Equals(_workOrderTaskDto.MaterialItem) && x.MaterialCode == _workOrderTaskDto.MaterialCode)
        //        .ToList();

        //    // 逻辑：如果已完工或数量已满，显示 Status=0 (历史记录)，否则显示 Status=1 (当前在用)
        //    List<WorkOrderTaskMaterialAddDto> displayRecords;
        //    bool isFinished = _workOrderTaskDto.Status == ((int)WorkOrderStatus.Finished).ToString() ||
        //                      (_workOrderTaskDto.Quantity == _workOrderTaskDto.CompletedQty && _workOrderTaskDto.Quantity > 0);

        //    if (isFinished)
        //    {
        //        // 取最后一次下料的历史记录
        //        displayRecords = allRecords.Where(x => x.Status == "0")
        //            .GroupBy(x => x.BarCode)
        //            .Select(g => g.OrderByDescending(item => item.Id).First())
        //            .ToList();
        //    }
        //    else
        //    {
        //        displayRecords = allRecords.Where(x => x.Status == "1").ToList();
        //    }

        //    // 4. 绑定 BOM 表格
        //    var tableData = _workOrderBoms.GroupJoin(
        //        displayRecords,
        //        x => new { Item = x.BomItem, Material = x.MaterialCode },
        //        y => new { Item = y.BomItem, Material = y.MaterialCode },
        //        (bom, recs) => new { bom, recs }
        //    ).SelectMany(
        //        x => x.recs.DefaultIfEmpty(),
        //        (x, y) => new WorkOrderTaskMaterialAddView(_workOrderTaskDto, x.bom, y)
        //    ).ToList();

        //    bomTable.DataSource = tableData;

        //    // 5. 更新状态图标和焦点
        //    bool hasUnloadedMaterial = tableData.Any(x => string.IsNullOrWhiteSpace(x.BarCode));
        //    if (!hasUnloadedMaterial && !isFinished)
        //    {
        //        materialFlagPicture.Image = (Image)Properties.Resources.ResourceManager.GetObject("success");
        //    }
        //    else
        //    {
        //        materialFlagPicture.Image = (Image)Properties.Resources.ResourceManager.GetObject("error");
        //        if (!isFinished)
        //        {
        //            await AutoFillLastBarcodeAsync();
        //        }
        //    }

        //    // 6. 绑定报工记录表格
        //    confirmTable.DataSource = confirmList.OrderByDescending(x => x.ConfirmDate).ToList();
        //}

        private void UpdateTaskInfoLabels()
        {
            SetLabel(orderLabel, _workOrderTaskDto.OrderNumber);
            SetLabel(materialLabel, $"{_workOrderTaskDto.MaterialCode}\n\r{_workOrderTaskDto.MaterialDesc}");
            SetLabel(quantityLabel, $"{Convert.ToInt32(_workOrderTaskDto.CompletedQty)} / {Convert.ToInt32(_workOrderTaskDto.Quantity)}");
            SetLabel(cableLenLabel, FormatLength(_workOrderTaskDto.CableLength));
            SetLabel(cableMaxLenLabel, FormatLength(_workOrderTaskDto.CableLengthUsl));
            SetLabel(cableMinLenLabel, FormatLength(_workOrderTaskDto.CableLengthDsl));
        }


        private void SetLabel(AntdUI.Label lbl, string value)
        {
            string prefix = lbl.Text.Contains("：") ? lbl.Text.Split('：')[0] : lbl.Text;
            lbl.Text = $"{prefix}：{value}";
        }

        private string FormatLength(decimal? val) => val.HasValue ? $"{val.Value:F1} mm" : "N/A mm";

        private async Task AutoFillLastBarcodeAsync()
        {
            if (_workOrderTaskDto.WorkStationId.HasValue)
            {
                var material = await _facade.MaterialAdd.GetLastByWorkStationAsync(_workOrderTaskDto.WorkStationId.Value);
                if (material != null && material.LastQuantity > 0 && material.MaterialCode == _workOrderTaskDto.MaterialCode)
                {
                    barcodeInput.Text = material.BarCode;
                    barcodeInput.Focus();
                }
            }
        }


        #region Scanning Logic




        private async Task<bool> TransferRawMaterialToLineSideAsync(RawLinesideStockDto stock)
        {
            var parameterGroup = await _facade.Params.GetGroupWithItemsAsync(Group_SapLocation);
            string fromLoc = parameterGroup?.Items.FirstOrDefault(x => x.Key == Key_SAPRawMtrStock)?.Value ?? "1100";
            string toLoc = parameterGroup?.Items.FirstOrDefault(x => x.Key == Key_CableRawLineStock)?.Value ?? "2200";

            var request = new TransferSapRequest
            {
                FactoryCode = AppSession.CurrentUser.FactoryName,
                EmployeeId = AppSession.CurrentUser.EmployeeId,
                TransferType = MaterialTransferType.GoodsIssue,
                ConsumptionType = ConsumptionType.MaterialTransfer,
                Stocks = new List<TransferStock>
                {
                    new TransferStock
                    {
                        StockId = stock.Id,
                        MaterialCode = stock.MaterialCode,
                        BatchCode = stock.BatchCode,
                        Quantity = (decimal)stock.LastQuantity,
                        BaseUnit = stock.BaseUnit
                    }
                },
                FromLocation = fromLoc,
                ToLocation = toLoc
            };

            var result = await _facade.MesApi.PostAsync<object, object>(_facade.ApiSettings["MesApi"].Endpoints["LineStockTransferToSAP"], request);
            return result != null && result.IsSuccess;
        }

        #endregion

        #region Submission Logic


        private List<WorkOrderTaskConsumCreateDto> BuildConsumptionList(WorkOrderTaskMaterialAddView mat, decimal defaultScrap, string defaultReason, bool isPrintProcessCard)
        {
            var list = new List<WorkOrderTaskConsumCreateDto>();

            // 正常消耗
            list.Add(new WorkOrderTaskConsumCreateDto
            {
                MaterialCode = mat.MaterialCode,
                BatchCode = mat.BatchCode,
                BarCode = mat.BarCode,
                EntryUnitCode = mat.BaseUnit,
                EntryQuantity = (_workOrderTaskDto.CableLengthUsl ?? 0) / 1000m * reportInputNumber.Value,
                MovementType = ((int)ConsumptionType.Consumption).ToString(),
            });

            // 样线报废 (第一次报工时)
            if (!isPrintProcessCard)
            {
                list.Add(new WorkOrderTaskConsumCreateDto
                {
                    MaterialCode = mat.MaterialCode,
                    BatchCode = mat.BatchCode,
                    BarCode = mat.BarCode,
                    EntryUnitCode = mat.BaseUnit,
                    MovementType = ((int)ConsumptionType.Scrap).ToString(),
                    EntryQuantity = defaultScrap / 1000m,
                    MovementRemark = defaultReason,
                });
            }

            // 额外报废
            if (scraptInputNumber.Value > 0)
            {
                list.Add(new WorkOrderTaskConsumCreateDto
                {
                    MaterialCode = mat.MaterialCode,
                    BatchCode = mat.BatchCode,
                    BarCode = mat.BarCode,
                    EntryUnitCode = mat.BaseUnit,
                    MovementType = ((int)ConsumptionType.Scrap).ToString(),
                    EntryQuantity = scraptInputNumber.Value / 1000m,
                    MovementRemark = ((MenuItem)scrapResionSelect.SelectedValue).Name
                });
            }
            return list;
        }

        private async Task SyncInventoryToSapAsync(string matCode, string batchCode, string unit, decimal qty)
        {
            var parameterGroup = await _facade.Params.GetGroupWithItemsAsync(Group_SapLocation);
            var request = new TransferSapRequest
            {
                FactoryCode = AppSession.CurrentUser.FactoryName,
                EmployeeId = AppSession.CurrentUser.EmployeeId,
                TransferType = MaterialTransferType.GoodsIssue,
                ConsumptionType = ConsumptionType.MaterialTransfer,
                Stocks = new List<TransferStock>
                {
                    new TransferStock
                    {
                        MaterialCode = matCode,
                        BatchCode = batchCode,
                        Quantity = qty,
                        BaseUnit = unit,
                    }
                },
                WorkOrderId = _workOrderTaskDto.OrderId, // 注意：这里需要 OrderID 还是 OrderNo 取决于 API 定义，原代码用 order.Id
                WorkOrderNo = _workOrderTaskDto.OrderNumber,
                FromLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == Key_CableRawLineStock)?.Value ?? "2200",
                ToLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == Key_CableProLineStock)?.Value ?? "2100"
            };

            _facade.MesApi.PostAsync<object, object>(_facade.ApiSettings["MesApi"].Endpoints["LineStockTransferToSAP"], request);
        }

        private async Task HandlePrintingAsync(WorkOrderTaskConfirmCreateDto confirm, List<WorkOrderTaskConsumCreateDto> consums, bool isPrintProcessCard)
        {
            if (string.IsNullOrWhiteSpace(_workstation.PrintType))
                throw new Exception("当前工位未配置打印类型！");
            if (string.IsNullOrWhiteSpace(_workstation.PrinterName))
                throw new Exception("当前工位打印机未设置！");

            PrinterType printType = (PrinterType)Convert.ToInt32(_workstation.PrintType);
            var order = await _facade.WorkOrderService.GetByIdAsync(_workOrderTaskDto.OrderId);
            // 1. 打印流转卡 (如果是第一次)
            if (!isPrintProcessCard)
            {

                string flag = await DetermineMaterialFlagAsync(order.Id);
                string locCode = await GetNextLocationAsync(_workOrderTaskDto.NextWorkCenter);
                var process = await _facade.WorkOrderProcessService.GetByIdAsync(_workOrderTaskDto.OrderProcessId);

                bool procCardSuccess = LabelPrintHelper.ProcessCardPrinter(order, process, flag, locCode, printType, _workstation.PrinterName);
                if (!procCardSuccess)
                    AntdUI.Message.error(this, "流转卡打印失败, 请重新补打！");
            }

            // 2. 打印过账标签
            string consumeBatch = consums.FirstOrDefault(x => x.MovementType == ((int)ConsumptionType.Consumption).ToString())?.BatchCode;

            bool reportSuccess = LabelPrintHelper.CuttingReportPrinter(_workOrderTaskDto, new WorkOrderTaskConfirmDto
            {
                ConfirmNumber = confirm.ConfirmNumber,
                ConfirmQuantity = confirm.ConfirmQuantity
            }, order.ProfitCenter, consumeBatch, printType, _workstation.PrinterName);

            if (!reportSuccess)
                AntdUI.Message.error(this, "过账标签打印失败, 请重新补打！");
        }

        #endregion

        #region Helper Methods

        private async Task<string> DetermineMaterialFlagAsync(int orderId)
        {
            var boms = (await _facade.WorkOrderBomItemService.GetListByOrderIdAync(orderId))
                .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true).ToList();

            if (!boms.Any() || boms.Where(x => x.ConsumeType != null && (new List<int> { 0, 2 }).Contains((int)x.ConsumeType)).Count() == 0)
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

        private async Task<string> GetNextLocationAsync(string workCenterCode)
        {
            var wc = await _facade.WorkCenter.GetByCodeAsync(workCenterCode);
            if (wc?.LineStockId != null)
            {
                var loc = await _facade.Location.GetByIdAsync((int)wc.LineStockId);
                return loc?.Name ?? string.Empty;
            }
            return string.Empty;
        }

        private async void backPicture_Click(object sender, EventArgs e)
        {
            try
            {
                backPicture.Enabled = false;
                // 【修复】增加 try-catch 包裹回调，防止刷新数据时的异常导致窗体无法关闭或状态残留
                if (LoadOrderData != null)
                {
                    await LoadOrderData.Invoke();
                }
            }
            catch (Exception ex)
            {
                // 即使刷新失败，也只提示警告，不阻止用户退出详情页
                AntdUI.Message.warn(this, $"刷新列表失败: {ex.Message}");
            }
            finally
            {
                // 【关键】确保 Close 始终执行，触发 Scope.Dispose()
                backPicture.Enabled = true;

                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private async void suspendButton_Click(object sender, EventArgs e)
        {
            if (AntdUI.Modal.open(new AntdUI.Modal.Config(this, "提示", "即将将当前订单挂起，是否继续？", AntdUI.TType.Warn)) == DialogResult.OK)
            {
                try
                {
                    await _facade.Task.SuspendTask(_workOrderTaskDto.Id);
                    LoadOrderData?.Invoke();
                    // 【修复】增加 try-catch 包裹回调，防止刷新数据时的异常导致窗体无法关闭或状态残留
                    if (LoadOrderData != null)
                    {
                        await LoadOrderData.Invoke();
                    }
                }
                catch (Exception ex)
                {
                    // 即使刷新失败，也只提示警告，不阻止用户退出详情页
                    AntdUI.Message.warn(this, $"刷新列表失败: {ex.Message}");
                }
                finally
                {
                    // 【关键】确保 Close 始终执行，触发 Scope.Dispose()
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }

            }
        }

        private async void bomTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is WorkOrderTaskMaterialAddView data && e.Btn.Id == "remove")
            {
                if (AntdUI.Modal.open(new AntdUI.Modal.Config(this, "提示", "即将将当前批次下料，是否继续？", AntdUI.TType.Warn)) == DialogResult.OK)
                {
                    data.Quantity = 0;
                    data.BarCode = string.Empty;
                    data.BatchCode = string.Empty;
                    materialFlagPicture.Image = (Image)Properties.Resources.ResourceManager.GetObject("error");

                    await _facade.MaterialAdd.UpdateAsync(new WorkOrderTaskMaterialAddUpdateDto
                    {
                        Id = data.Id,
                        Status = "0",
                        UpdateBy = AppSession.CurrentUser.EmployeeId,
                        UpdateOn = DateTime.Now
                    });

                    e.Btn.Enabled = false;
                }
            }
        }

        private async void AdjustButton_Click(object sender, EventArgs e)
        {
            await RunAsync(AdjustButton, async () =>
            {
                var materialAdds = await _facade.MaterialAdd.GetByTaskIdAsync(_workOrderTaskDto.Id);
                if (materialAdds.Any(x => x.Status == "1"))
                {
                    throw new Exception("当前任务存在未下料的标签，请先下料后再进行盘盈盘亏操作");
                }

                // 【修复点】使用 Factory 打开，并使用 InitData 传参
                _formFactory.Show<InventoryAdjustForm>(form =>
                {
                    // 传递当前任务ID给库存调整界面
                    form.InitData(workOrderTaskId: _workOrderTaskDto.Id);

                    // 监听成功事件
                    form.OnSuccess += () =>
                    {
                        AntdUI.Message.success(this, "库存调整提交成功！");
                    };
                }, isModal: true);
            });
        }

        private async void processSReprintButtom_Click(object sender, EventArgs e)
        {
            if (AntdUI.Modal.open(new AntdUI.Modal.Config(this, "流转卡补打", "是否确定补打流转卡？", AntdUI.TType.Success)) == DialogResult.OK)
            {
                try
                {
                    var order = await _facade.WorkOrderService.GetByIdAsync(_workOrderTaskDto.OrderId);
                    if (order == null)
                        throw new Exception("未查询到工单信息！");

                    if (string.IsNullOrWhiteSpace(_workstation.PrintType) || string.IsNullOrWhiteSpace(_workstation.PrinterName))
                        throw new Exception("工位打印机未配置！");

                    PrinterType printType = (PrinterType)Convert.ToInt32(_workstation.PrintType);

                    string flag = await DetermineMaterialFlagAsync(order.Id);
                    string locCode = await GetNextLocationAsync(_workOrderTaskDto.NextWorkCenter);
                    var process = await _facade.WorkOrderProcessService.GetByIdAsync(_workOrderTaskDto.OrderProcessId);

                    bool rtn = LabelPrintHelper.ProcessCardPrinter(order, process, flag, locCode, printType, _workstation.PrinterName);
                    if (!rtn)
                        throw new Exception("打印失败！");
                }
                catch (Exception ex)
                {
                    AntdUI.Message.error(this, ex.Message);
                }
            }
        }

        #endregion
    }

    public class WorkOrderTaskMaterialAddView : AntdUI.NotifyProperty
    {
        public WorkOrderTaskMaterialAddView(WorkOrderTaskDto task, WorkOrderBomItemDto require, WorkOrderTaskMaterialAddDto stock)
        {
            _taskId = task.Id;
            _materialCode = require.MaterialCode;
            _materialDesc = require.MaterialDesc;
            _baseUnit = require.Unit;
            _bomItem = require.BomItem;
            _reqQuantity = require.RequiredQuantity;

            if (stock != null)
            {
                _Id = stock.Id;
                _lastQuantity = stock.LastQuantity;
                _batchCode = stock.BatchCode;
                _barCode = stock.BarCode;
            }
            else
            {
                _Id = 0;
                _lastQuantity = 0;
                _batchCode = string.Empty;
                _barCode = string.Empty;
            }

            _btns = new AntdUI.CellLink[] {
                new AntdUI.CellButton("remove", "下料", AntdUI.TTypeMini.Primary)
                {
                    Enabled = stock != null
                }
            };
        }

        // Properties (Keeping standard boiler-plate for NotifyProperty)
        int _Id; public int Id
        {
            get => _Id; set
            {
                if (_Id == value)
                    return;
                _Id = value;
                OnPropertyChanged();
            }
        }
        int _taskId; public int TaskId
        {
            get => _taskId; set
            {
                if (_taskId == value)
                    return;
                _taskId = value;
                OnPropertyChanged();
            }
        }
        string? _materialCode; public string? MaterialCode
        {
            get => _materialCode; set
            {
                if (_materialCode == value)
                    return;
                _materialCode = value;
                OnPropertyChanged();
            }
        }
        string? _materialDesc; public string? MaterialDesc
        {
            get => _materialDesc; set
            {
                if (_materialDesc == value)
                    return;
                _materialDesc = value;
                OnPropertyChanged();
            }
        }
        string? _baseUnit; public string? BaseUnit
        {
            get => _baseUnit; set
            {
                if (_baseUnit == value)
                    return;
                _baseUnit = value;
                OnPropertyChanged();
            }
        }
        string? _bomItem; public string? BomItem
        {
            get => _bomItem; set
            {
                if (_bomItem == value)
                    return;
                _bomItem = value;
                OnPropertyChanged();
            }
        }
        decimal? _reqQuantity; public decimal? ReqQuantity
        {
            get => _reqQuantity; set
            {
                if (_reqQuantity == value)
                    return;
                _reqQuantity = value;
                OnPropertyChanged();
            }
        }
        decimal? _quantity; public decimal? Quantity
        {
            get => _quantity; set
            {
                if (_quantity == value)
                    return;
                _quantity = value;
                OnPropertyChanged();
            }
        }
        string? _batchCode; public string? BatchCode
        {
            get => _batchCode; set
            {
                if (_batchCode == value)
                    return;
                _batchCode = value;
                OnPropertyChanged();
            }
        }
        string? _barCode; public string? BarCode
        {
            get => _barCode; set
            {
                if (_barCode == value)
                    return;
                _barCode = value;
                OnPropertyChanged();
            }
        }
        decimal? _lastQuantity; public decimal? LastQuantity
        {
            get => _lastQuantity; set
            {
                if (_lastQuantity == value)
                    return;
                _lastQuantity = value;
                OnPropertyChanged();
            }
        }
        string? _status; public string? Status
        {
            get => _status; set
            {
                if (_status == value)
                    return;
                _status = value;
                OnPropertyChanged();
            }
        }
        AntdUI.CellLink[] _btns; public AntdUI.CellLink[] btns
        {
            get => _btns; set
            {
                _btns = value;
                OnPropertyChanged();
            }
        }
    }
}
