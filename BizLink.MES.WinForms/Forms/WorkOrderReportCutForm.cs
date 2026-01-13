using AntdUI;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Helper;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Common.Helper;
using BizLink.MES.WinForms.Common.Views;
using BizLink.MES.WinForms.Infrastructure;
using Dm;
using Dm.util;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = AntdUI.Button;

namespace BizLink.MES.WinForms.Forms
{
    // 3. 继承 MesBaseForm (获得 RunAsync 能力)
    public partial class WorkOrderReportCutForm : MesBaseForm
    {
        #region Fields & Services

        // 4. 只注入 Facade 和 Factory
        private readonly CutModuleFacade _facade;
        private readonly IFormFactory _formFactory;

        private WorkOrderTaskCreateDto _workOrderTaskDto;
        private List<string> _workCenters = new List<string>();
        private List<WorkOrderInProgressCutView> _orderViews = new List<WorkOrderInProgressCutView>();

        private const string WorkCenterGroup_CableCut = "CABLECUT";

        #endregion

        // 5. 构造函数瘦身：从 20 个参数减少到 2 个
        public WorkOrderReportCutForm(
            CutModuleFacade facade,
            IFormFactory formFactory)
        {
            _facade = facade;
            _formFactory = formFactory;

            InitializeComponent();
            InitializeTable();
        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            startDatePicker.Value = DateTime.Now.Date;
            workcenterSelect.PlaceholderText = "请先选择工作中心组...";
            workstationSelect.PlaceholderText = "请先选择工位...";
            orderInput.PlaceholderText = "请输入订单号或断线物料进行搜索...";

            SetButtonState(false);
        }
        //private async void WorkOrderReportForm_Load(object sender, EventArgs e)
        //{
        //    startDatePicker.Value = DateTime.Now.Date;
        //    workcenterSelect.PlaceholderText = "请先选择工作中心组...";
        //    workstationSelect.PlaceholderText = "请先选择工位...";
        //    orderInput.PlaceholderText = "请输入订单号或断线物料进行搜索...";

        //    SetButtonState(false);


        //    // 可选：预加载数据
        //    // await RunAsync(async () => await LoadInitData());
        //}

        #region Event Handlers

        private async void executeButton_Click(object sender, EventArgs e)
        {
            // 6. 使用 RunAsync 自动处理 Loading、禁用、异常捕获
            await RunAsync(executeButton, async () =>
            {
                // --- 校验逻辑 ---
                if (workstationSelect.SelectedValue is not MenuItem stationItem)
                    throw new Exception("未选择工位，请选择后再进入生产！");

                int workstationId = int.Parse(stationItem.Name);
                var workstation = await _facade.WorkStation.GetByIdAsync(workstationId);

                if (workstation == null)
                    throw new Exception("工位信息无效！");



                if (stationConfirmButtom.Text != "解除确认")
                    throw new Exception("请先进行工位确认！");

                if (_workOrderTaskDto == null)
                    throw new Exception("请先选择一笔订单后再进入生产！");

                // --- 业务逻辑 ---
                var taskDto = await GetOrCreateTaskAsync(_workOrderTaskDto);

                if (taskDto.Status == ((int)WorkOrderStatus.Paused).ToString())
                    throw new Exception("断线任务已挂起，无法继续报工，请手动关闭当前断线任务");

                if (taskDto?.CableLength == null || taskDto.CableLength <= 0)
                    throw new Exception("断线长度错误，请先维护断线参数或指定替代物料！");

                // --- 打开详情页 (使用 Factory) ---
                // OpenDrawer 会自动创建 Scope，并在 Drawer 关闭时释放 Scope
                _formFactory.OpenDrawer<WorkOrderReportCutDetailForm>(this.ParentForm, form =>
                {
                    // 调用详情页的 InitData 方法传参
                    form.InitData(taskDto, workstationId);

                    // 绑定刷新事件
                    form.LoadOrderData += LoadOrderData;
                });
            });
        }

        private async void orderTable_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is WorkOrderInProgressCutView data && e.Btn.Id == "edit")
            {
                // 表格内按钮通常不显示 Loading 转圈，或者使用 Grid 自身的 Loading
                // 这里使用 RunAsync 简化异常处理
                orderSpin.Visible = true;
                await RunAsync(null, async () =>
                {
                    await SyncOrderBomFromSapAsync(data);
                    await UpdateRowParamsAsync(data);
                });
                orderSpin.Visible = false;
            }
        }

        private async void workcenterSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            _workCenters.Clear();
            if (e.Value is MenuItem item)
            {
                // 使用 _facade 调用服务
                var workcenter = await _facade.WorkCenter.GetByIdAsync(int.Parse(item.Name));
                if (workcenter.IsGroup)
                {
                    var childworkcenters = await _facade.WorkCenter.GetBygroupIdAsync(workcenter.Id);
                    _workCenters.AddRange(childworkcenters.Select(x => x.WorkCenterCode));
                }
                else
                {
                    _workCenters.Add(item.Text.Split('-')[0]);
                }
            }
        }
        private async void stationConfirmButtom_Click(object sender, EventArgs e)
        {
            await RunAsync(async () =>
            {
                if (stationConfirmButtom.Text.Contains("工位确认"))
                {
                    if (startDatePicker.Value == null)
                        throw new Exception("请先选择日期！");
                    if (string.IsNullOrWhiteSpace(workcenterSelect.Text))
                        throw new Exception("请先选择工作中心！");
                    if (string.IsNullOrWhiteSpace(workstationSelect.Text))
                        throw new Exception("请先选择工位！");

                    ToggleStationInputs(false);
                    await LoadOrderData();
                }
                else
                {
                    ToggleStationInputs(true);
                }
            });
        }
        private async void orderTable_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is WorkOrderInProgressCutView data)
            {
                // 保持原有的防抖/Loading 逻辑
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                detailSpin.Visible = true;
                detailSpin.BringToFront();

                try
                {
                    // 7. 将原本散乱的逻辑保留，但在内部使用 _facade 调用
                    //if (int.TryParse(data.Status, out int status) && status == (int)WorkOrderStatus.UnKnow)
                    //    //throw new Exception("订单未准备，请先处理断线参数！");

                    //_workOrderTaskDto = null;

                    //var existingTask = await _facade.Task.GetByProcessIdAsync(data.OrderProcessId, data.CableItem);
                    //if (existingTask != null)
                    //{
                    //    if (existingTask.CompletedQty == 0)
                    //    {
                    //        await _facade.Task.DeleteAsync(existingTask.Id);
                    //    }
                    //    else
                    //    {
                    //        data.CableLength = existingTask.CableLength;
                    //        data.CableLengthUsl = existingTask.CableLengthUsl;
                    //        data.CableLengthDsl = existingTask.CableLengthDsl;
                    //    }
                    //}

                    InitializeTaskDtoFromView(data);
                    UpdateDetailLabels(data);
                    SetButtonState(true);
                }
                catch (Exception ex)
                {
                    AntdUI.Message.error(this, ex.Message);
                }
                finally
                {
                    stopwatch.Stop();
                    if (stopwatch.ElapsedMilliseconds < 200)
                        await Task.Delay(200 - (int)stopwatch.ElapsedMilliseconds);
                    detailSpin.Visible = false;
                }
            }
        }


        private void orderInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                if (stationConfirmButtom.Text != "解除确认")
                {
                    AntdUI.Message.error(this, "扫描失败，请先进行工位确认！");
                    return;
                }

                string filter = orderInput.Text.Trim();
                orderTable.DataSource = _orderViews
                    .WhereIF(!string.IsNullOrWhiteSpace(filter), w =>
                        w.OrderNumber.Contains(filter) ||
                        (w.CableMaterial != null && w.CableMaterial.Contains(filter)))
                    .ToList();

                orderInput.SelectAll();
            }
        }

        private void startDatePicker_ValueChanged(object sender, DateTimeNEventArgs e)
        {
        }
        private async void suspendButton_Click(object sender, EventArgs e)
        {
            await RunAsync(suspendButton, async () => 
            {
                // --- 校验逻辑 ---
                if (workstationSelect.SelectedValue is not MenuItem stationItem)
                    throw new Exception("未选择工位，请选择后再进行操作！");

                int workstationId = int.Parse(stationItem.Name);
                var workstation = await _facade.WorkStation.GetByIdAsync(workstationId);

                if (workstation == null)
                    throw new Exception("工位信息无效！");



                if (stationConfirmButtom.Text != "解除确认")
                    throw new Exception("请先进行工位确认！");

                if (_workOrderTaskDto == null)
                    throw new Exception("请先选择一笔订单后再进行操作！");
                if (string.IsNullOrWhiteSpace(RemarkInput.Text))
                    throw new Exception("请填写生产备注后再进行订单挂起！");
                // --- 业务逻辑 ---
                //先创建任务，再进行挂起
                var taskDto = await GetOrCreateTaskAsync(_workOrderTaskDto);
                await _facade.Task.UpdateAsync(new WorkOrderTaskUpdateDto() 
                {
                    Id = taskDto.Id,
                    Status = ((int)WorkOrderStatus.Paused).ToString(),
                    ProductionRemark = RemarkInput.Text.Trim(),
                    UpdateBy = AppSession.CurrentUser.EmployeeId,
                    UpdateOn = DateTime.Now
                });

                var orderView = orderTable.DataSource as List<WorkOrderInProgressCutView>;
                var currentTask = orderView.Where(x => x.TaskId == taskDto.Id).First();
                if (currentTask != null)
                {
                    currentTask.Status = ((int)WorkOrderStatus.Paused).ToString();
                }



            }, successMsg: "工单挂起成功！",confirmMsg:$"即将挂起订单{_workOrderTaskDto.OrderNumber}，是否继续？");
        }

        private void CuttingParamModifyForm_CutParamPassed(CableCutParamDto param, int processid, string cableitem)
        {
            orderSpin.Visible = true;

            var targets = _orderViews.Where(x => x.OrderProcessId == processid && x.CableItem == cableitem).ToList();
            foreach (var item in targets)
            {
                item.CableLength = param.CuttingLength;
                item.CableLengthUsl = param.BomLength / param.CablePcs + (decimal)(param.UpTol * 0.8m);
                item.CableLengthDsl = param.BomLength / param.CablePcs - param.DownTol;
                item.CablePcs = param.CablePcs;
                item.Status = ((int)WorkOrderStatus.New).ToString();
            }

            _workOrderTaskDto = null;
            orderTable.DataSource = _orderViews;
            orderSpin.Visible = false;

            AntdUI.Message.success(this, "断线参数更新成功！");
        }

        #endregion

        #region Core Logic Methods

        private async Task<WorkOrderTaskDto> GetOrCreateTaskAsync(WorkOrderTaskCreateDto inputDto)
        {
            // 使用 _facade.Task
            var existingTask = await _facade.Task.GetByProcessIdAsync(inputDto.OrderProcessId, inputDto.MaterialItem);
            if (existingTask != null )
            {
                if (existingTask.Status == ((int)WorkOrderStatus.New).ToString())
                {
                    await _facade.Task.DeleteAsync(existingTask.Id);
                }
                else
                    return existingTask;
            }

            var cableCutParam = await EnsureCableCutParamsAsync(inputDto);

            inputDto.CableLength = cableCutParam.CuttingLength;
            inputDto.CableLengthUsl = cableCutParam.BomLength / cableCutParam.CablePcs + (decimal)(cableCutParam.UpTol * 0.8m);
            inputDto.CableLengthDsl = cableCutParam.BomLength / cableCutParam.CablePcs - cableCutParam.DownTol;

            // 使用 _facade.WorkOrderService (来自基类 BaseAppFacade)
            inputDto.Quantity = cableCutParam.CablePcs * (await _facade.WorkOrderService.GetByIdAsync(inputDto.OrderId)).Quantity;
            inputDto.Status = ((int)WorkOrderStatus.New).ToString();
            inputDto.CreateBy = AppSession.CurrentUser.EmployeeId;


            var createdTask = await _facade.Task.CreateAsync(inputDto);

            // 使用 _facade.WorkOrderProcessService (来自基类)
            var process = await _facade.WorkOrderProcessService.GetByIdAsync(inputDto.OrderProcessId);
            if (process.ActStartTime == null)
            {
                try
                {
                    await _facade.WorkOrderProcessService.UpdateAsync(new WorkOrderProcessUpdateDto
                    {
                        Id = process.Id,
                        ActStartTime = DateTime.Now,
                        UpdateBy = AppSession.CurrentUser.EmployeeId,
                        UpdateOn = DateTime.Now,
                        Status = ((int)WorkOrderStatus.OnGoing).ToString()
                    });
                }
                catch { /* Log error */ }
            }

            return createdTask;
        }
        private async Task<CableCutParamDto> EnsureCableCutParamsAsync(WorkOrderTaskCreateDto taskDto)
        {
            // 使用 _facade.WorkOrderService
            var order = await _facade.WorkOrderService.GetByIdAsync(taskDto.OrderId);
            var paramsList = await GetOrFetchCableCutParams(new List<string> { order.MaterialCode },taskDto.MaterialItem);

            if (!paramsList.Any())
            {
                // 使用 _facade.WorkOrderBomItemService (来自基类)
                var boms = await _facade.WorkOrderBomItemService.GetListByOrderIdAync(taskDto.OrderId);
                var cableBom = boms.FirstOrDefault(x => x.BomItem == taskDto.MaterialItem && x.MaterialCode == taskDto.MaterialCode);
                if (cableBom != null && !string.IsNullOrEmpty(cableBom.SuperMaterialCode))
                {
                    paramsList = await GetOrFetchCableCutParams(new List<string> { cableBom.SuperMaterialCode }, taskDto.MaterialItem);
                }
            }

            if (!paramsList.Any())
                throw new Exception("未查询当前订单的断线参数，请先联系工艺添加断线参数！");

            var targetParam = paramsList.FirstOrDefault(x => x.PositionItem == taskDto.MaterialItem && x.CableMaterialCode == taskDto.MaterialCode)
                              ?? paramsList.FirstOrDefault(x => x.PositionItem == taskDto.MaterialItem);

            if (targetParam == null)
                throw new Exception("未查询到当前订单的断线参数，请先联系工艺添加断线参数！");

            return targetParam;
        }

        /// <summary>
        /// 核心逻辑：本地查询 -> 远程查询 -> 保存本地 -> 返回结果
        /// 【优化】：优先查本地，且对保存操作进行容错处理
        /// </summary>
        private async Task<List<CableCutParamDto>> GetOrFetchCableCutParams(List<string> materialCodes,string bomItem)
        {
            if (materialCodes == null || !materialCodes.Any())
                return new List<CableCutParamDto>();

            // 1. 优先查询本地数据库 (使用 _facade.CutParam)
            var localParams = await _facade.CutParam.GetListBySimiMaterialCodeAsync(materialCodes);
            if (localParams != null && localParams.Any() && localParams.Where(x => x.PositionItem == bomItem).Count() > 0)
            {

                //先将本地参数记录删除
                await _facade.CutParam.DeleteByIdsAsync(localParams.Where(x => x.PositionItem == bomItem).Select(x=> x.Id).ToList());

            }

            // 2. API 调用获取 (使用 _facade.MesApi)
            var requestUrl = _facade.ApiSettings["MesApi"].Endpoints["GetCableCutParamByMaterial"];
            var result = await _facade.MesApi.PostAsync<object, List<CableCutParamCreateDto>>(requestUrl, new
            {
                semiMaterialCode = materialCodes.Distinct()
            });

            if (result.IsSuccess && result.Data != null && result.Data.Any())
            {
                try
                {
                    // 3. 批量保存到本地
                    await _facade.CutParam.CreateBatchAsync(result.Data.Where(x => x.PositionItem == bomItem).ToList());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"缓存断线参数时出现警告 (可忽略): {ex.Message}");
                }
            }

            // 4. 再次从本地查询返回
            return await _facade.CutParam.GetListBySimiMaterialCodeAsync(materialCodes);
        }
        private async Task SyncOrderBomFromSapAsync(WorkOrderInProgressCutView data)
        {
            // 使用 _facade.MesApi
            var requestUrl = _facade.ApiSettings["MesApi"].Endpoints["GetWorkOrderByOrderNos"];
            var result = await _facade.MesApi.PostAsync<object, SapOrderDto>(requestUrl, new
            {
                factoryCode = AppSession.CurrentUser.FactoryName,
                OrderNos = new List<string> { data.OrderNumber }
            });

            if (!result.IsSuccess || result.Data == null)
                return;

            var localBoms = await _facade.WorkOrderBomItemService.GetListByOrderIdAync(data.OrderId);

            if (localBoms == null || !localBoms.Any())
            {
                var processes = await _facade.WorkOrderProcessService.GetListByOrderIdAync(data.OrderId);
                if (processes.Any())
                {
                    var newBoms = result.Data.sapOrderBoms.Join(processes,
                        sap => new { WorkOrderNo = sap.OrderNo, sap.Operation },
                        proc => new { proc.WorkOrderNo, proc.Operation },
                        (sap, proc) => new WorkOrderBomItemCreateDto
                        {
                            WorkOrderId = (int)proc.WorkOrderId,
                            WorkOrderProcessId = proc.Id,
                            WorkOrderNo = sap.OrderNo,
                            RequiredQuantity = sap.RequireQuantity,
                            BomItem = sap.BomItem,
                            Operation = sap.Operation,
                            MaterialCode = sap.MaterialCode,
                            MaterialDesc = sap.MaterialDesc,
                            Unit = sap.BaseUnit,
                            ReservationItem = sap.ReservationItem,
                            MovementAllowed = sap.AllowedMovement.ToUpper() == "X",
                            ConsumeType = sap.ConsumeType,
                            SuperMaterialCode = sap.SuperMaterialCode,
                            CreateBy = AppSession.CurrentUser.EmployeeId
                        }).ToList();

                    await _facade.WorkOrderBomItemService.CreateBatchAsync(newBoms);
                }
            }
            else
            {
                foreach (var bom in localBoms)
                {
                    var sapBom = result.Data.sapOrderBoms.FirstOrDefault(x => x.ReservationItem == bom.ReservationItem && x.MaterialCode == data.CableMaterial);
                    if (sapBom != null)
                    {
                        await _facade.WorkOrderBomItemService.UpdateAsync(new WorkOrderBomItemUpdateDto
                        {
                            Id = bom.Id,
                            BomItem = sapBom.BomItem,
                            SuperMaterialCode = sapBom.SuperMaterialCode,
                            UpdateBy = AppSession.CurrentUser.EmployeeId,
                            UpdateOn = DateTime.Now
                        });
                    }
                }
            }
        }


        private async Task UpdateRowParamsAsync(WorkOrderInProgressCutView data)
        {
            var materialCodes = new List<string> { data.MaterialCode };

            var boms = await _facade.WorkOrderBomItemService.GetListByOrderIdAync(data.OrderId);
            var cableBom = boms.FirstOrDefault(x => x.BomItem == data.CableItem && x.MaterialCode == data.CableMaterial);
            if (cableBom != null && !string.IsNullOrEmpty(cableBom.SuperMaterialCode))
            {
                materialCodes.Add(cableBom.SuperMaterialCode);
            }

            var paramsList = await GetOrFetchCableCutParams(materialCodes, data.CableItem);

            var targetParam = paramsList.FirstOrDefault(x => x.PositionItem == data.CableItem && x.CableMaterialCode == data.CableMaterial)
                              ?? paramsList.FirstOrDefault(x => x.PositionItem == data.CableItem);

            if (targetParam == null)
                throw new Exception("未查询到可选择的断线参数，请先联系工艺添加断线参数！");

            data.CableLength = Math.Round((decimal)targetParam.CuttingLength, 1);
            data.CableLengthUsl = Math.Round((decimal)targetParam.BomLength / (decimal)targetParam.CablePcs + (decimal)targetParam.UpTol * 0.8m, 1);
            data.CableLengthDsl = Math.Round((decimal)targetParam.BomLength / (decimal)targetParam.CablePcs - (decimal)targetParam.DownTol, 1);
            data.CablePcs = targetParam.CablePcs;
            data.Status = ((int)WorkOrderStatus.New).ToString();

            orderTable.Refresh();
        }


        #endregion

        #region UI Helper Methods

        private async void InitializeTable()
        {
            orderTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("ProfitCenter", "BU", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter(),
                new AntdUI.Column("WorkCenter", "工作中心", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter(),
                new AntdUI.Column("OrderNumber", "订单号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter(),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) => CellTagHelper.BuildWorkOrderStatusTag(value as string)
                }.SetFixed().SetDefaultFilter(),
                new AntdUI.Column("MaterialCode", "订单料号", AntdUI.ColumnAlign.Center).SetColAlign(),
                new AntdUI.Column("CableMaterial", "断线物料", AntdUI.ColumnAlign.Center).SetSortOrder().SetDefaultFilter(),
                new AntdUI.Column("Quantity", "订单数量", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("Progress", "进度", AntdUI.ColumnAlign.Center).SetWidth("Auto"),
                new AntdUI.Column("CableItem", "行号", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetSortOrder(),
                new AntdUI.Column("CablePcs", "断线数量", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("CableLength", "断线长度", AntdUI.ColumnAlign.Center).SetDisplayFormat("F1"),
                new AntdUI.Column("CableLengthUsl", "最大断长", AntdUI.ColumnAlign.Center).SetDisplayFormat("F1"),
                new AntdUI.Column("CableLengthDsl", "最小断长", AntdUI.ColumnAlign.Center).SetDisplayFormat("F1"),
                new AntdUI.Column("StartTime", "开工日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd"),
                new AntdUI.Column("DispatchDate", "排产日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd"),
                new AntdUI.Column("PlannerRemark", "计划备注", AntdUI.ColumnAlign.Center),
                new AntdUI.Column("btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto")
            };

            // 使用 _facade 调用服务获取工作中心
            var workcenters = await _facade.WorkCenter.GetListByGroupCodeAsync(WorkCenterGroup_CableCut);
            if (workcenters != null)
            {
                workcenterSelect.Items.AddRange(workcenters.Select(s => new MenuItem
                {
                    Name = s.Id.ToString(),
                    Text = $"{s.WorkCenterCode}-{s.WorkCenterName}"
                }).ToArray());
            }

            var workstations = await _facade.WorkStation.GetByWorkcenterGroupCodeAsync(WorkCenterGroup_CableCut);
            if (workstations != null)
            {
                workstationSelect.Items.AddRange(workstations.OrderBy(s => s.WorkStationCode.SafeSubstring(1, 6)).Select(s => new MenuItem
                {
                    Name = s.Id.ToString(),
                    Text = $"{s.WorkStationCode}-{s.WorkStationName}"
                }).ToArray());
            }
        }

        private async Task LoadOrderData()
        {
            orderSpin.Visible = true;
            orderTable.Enabled = false;
            try
            {
                // 使用 _facade.View (IWorkOrderInProgressViewService)
                var result = await _facade.View.GetListAsync(_workCenters, startDatePicker.Value);
                if (result != null && result.Count > 0)
                {
                    var processIds = result.Select(x => x.OrderProcessId).Distinct().ToList();
                    var tasks = await _facade.Task.GetByProcessIdsAsync(processIds);
                    var taskDict = tasks.ToDictionary(x => (x.OrderProcessId, x.MaterialItem), x => x);

                    foreach (var item in result)
                    {
                        if (taskDict.TryGetValue((item.OrderProcessId, item.CableItem), out var task))
                        {
                            item.Status = task.Status;
                            item.CompletedQty = task.CompletedQty;
                        }
                    }

                    _orderViews = result
                        .Where(w => w.Status != ((int)WorkOrderStatus.Finished).ToString())
                        .Select(w => new WorkOrderInProgressCutView(w))
                        .ToList();

                    string filter = orderInput.Text.Trim();
                    orderTable.DataSource = _orderViews
                        .WhereIF(!string.IsNullOrWhiteSpace(filter), w =>
                            w.OrderNumber.Contains(filter) ||
                            (w.CableMaterial != null && w.CableMaterial.Contains(filter)))
                        .ToList();

                    AntdUI.Message.success(this, $"成功查询出{_orderViews.Count}条待办订单");
                }
                else
                {
                    orderTable.DataSource = null;
                    throw new Exception("未查询到记录！");
                }
            }
            catch (Exception ex)
            {
                AntdUI.Message.error(this, ex.Message);
            }
            finally
            {
                orderSpin.Visible = false;
                orderTable.Enabled = true;
                SetButtonState(false);
            }
        }


        private void ToggleStationInputs(bool enable)
        {
            startDatePicker.Enabled = enable;
            workcenterSelect.Enabled = enable;
            workstationSelect.Enabled = enable;

            SetButtonState(false);

            stationConfirmButtom.Text = enable ? "工位确认" : "解除确认";
            stationConfirmButtom.Type = enable ? AntdUI.TTypeMini.Primary : AntdUI.TTypeMini.Error;
        }

        private void SetButtonState(bool enabled)
        {
            executeButton.Enabled = enabled;
            saveButton.Enabled = enabled;
            suspendButton.Enabled = enabled;
        }


        private void InitializeTaskDtoFromView(WorkOrderInProgressCutView data)
        {
            _workOrderTaskDto = new WorkOrderTaskCreateDto
            {
                OrderProcessId = data.OrderProcessId,
                OrderId = data.OrderId,
                OrderNumber = data.OrderNumber,
                TaskNumber = $"{data.OrderNumber}-{data.Operation}-{data.CableItem}",
                WorkStationId = workstationSelect.SelectedValue != null ? int.Parse(((MenuItem)workstationSelect.SelectedValue).Name) : 0,
                MaterialCode = data.CableMaterial,
                MaterialDesc = data.CableMaterialDesc,
                MaterialItem = data.CableItem,
                Quantity = data.Quantity * data.CablePcs,
                CompletedQty = data.CompletedQty,
                NextWorkCenter = data.NextWorkCenter,
                Operation = data.Operation,
                ProfitCenter = data.ProfitCenter,
                StartTime = data.StartTime,
                DispatchDate = data.DispatchDate,
                WorkCenter = data.WorkCenter,
                Remark = data.PlannerRemark,
                CableLength = data.CableLength,
                CableLengthDsl = data.CableLengthDsl,
                CableLengthUsl = data.CableLengthUsl,
            };
        }

        private void UpdateDetailLabels(WorkOrderInProgressCutView data)
        {
            SetLabel(orderLabel, data.OrderNumber);
            SetLabel(productCodeLabel, data.MaterialCode);
            SetLabel(productDescLabel, data.MaterialDesc);
            SetLabel(startTimeLabel, Convert.ToDateTime(data.StartTime).ToString("yyyy-MM-dd"));
            SetLabel(dispatchDateLabel, Convert.ToDateTime(data.DispatchDate).ToString("yyyy-MM-dd"));
            SetLabel(planQtyLabel, Convert.ToInt32(data.Quantity * data.CablePcs).ToString());
            SetLabel(completeQtyLabel, Convert.ToInt32(data.CompletedQty).ToString());
            SetLabel(cableCodeLabel, data.CableMaterial);
            SetLabel(cableLengthLabel, $"{Math.Round(data.CableLength ?? 0, 1)} mm");
            SetLabel(cableUslLabel, $"{Math.Round(data.CableLengthUsl ?? 0, 1)} mm");
            SetLabel(cableDslLabel, $"{Math.Round(data.CableLengthDsl ?? 0, 1)} mm");
            SetLabel(planRemarkLabel, data.PlannerRemark ?? string.Empty);
        }


        private void SetLabel(AntdUI.Label lbl, string value)
        {
            string prefix = lbl.Text.Contains("：") ? lbl.Text.Split('：')[0] : lbl.Text;
            lbl.Text = $"{prefix}：{value}";
        }


        #endregion
    }

    public class WorkOrderInProgressCutView : V_WorkOrderInProgressView
    {
        public WorkOrderInProgressCutView(V_WorkOrderInProgress entity) : base(entity)
        {
            _btns = new AntdUI.CellLink[] {
                new AntdUI.CellButton("edit", "更新", AntdUI.TTypeMini.Primary)
            };
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
}
