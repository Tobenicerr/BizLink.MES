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
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using Dm.util;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    // 1. 继承 MesBaseForm
    public partial class WorkOrderReportAssmDetailForm : MesBaseForm
    {
        #region Fields & Services

        private readonly AssemblyModuleFacade _facade;
        private readonly IServiceScopeFactory _scopeFactory; // 【新增】用于创建临时事务 Scope


        // 数据状态
        private V_WorkOrderInProgress _currentOrderInPress;
        private List<WorkOrderTaskDto> _workOrderTaskDtos;

        private decimal? _currentCompletedQty = 0;
        private decimal? _taskQuantity = 0;
        private List<WorkOrderBomItemDto> _workOrderBoms = new List<WorkOrderBomItemDto>();

        // Events
        public delegate Task DataUpdateHandler();
        public event DataUpdateHandler LoadOrderData;

        #endregion

        // 2. 构造函数：只注入 Facade
        public WorkOrderReportAssmDetailForm(AssemblyModuleFacade facade, IServiceScopeFactory scopeFactory)
        {
            _facade = facade;
            _scopeFactory = scopeFactory;
            InitializeComponent();
            InitializeTable();
        }

        // 3. 初始化数据入口 (替代构造函数传参)
        public void InitData(V_WorkOrderInProgress currentOrderInPress, List<WorkOrderTaskDto> workOrderTaskDtos)
        {
            _currentOrderInPress = currentOrderInPress;
            _workOrderTaskDtos = workOrderTaskDtos;
        }

        // 4. 窗体加载
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (_currentOrderInPress == null || _workOrderTaskDtos == null)
                return;

            await RunAsync(async () =>
            {
                await LoadDataAsync();
            });
        }

        private async Task LoadDataAsync()
        {
            // 1. 获取任务及进度
            var taskNo = $"{_currentOrderInPress.OrderNumber}-{_currentOrderInPress.Operation}";
            var tasks = await _facade.Task.GetByTaskNoAsync(taskNo);

            // 计算进度
            // 注意：这里需要确保 _workOrderTaskDtos 中的 ID 在 tasks 中存在
            var relevantTasks = tasks.Join(_workOrderTaskDtos, x => x.Id, y => y.Id, (x, y) => x).ToList();

            if (relevantTasks.Any())
            {
                _currentCompletedQty = relevantTasks.Min(x => x.CompletedQty);
                _taskQuantity = relevantTasks.Max(x => x.Quantity);
            }

            // 2. 更新 UI 标签
            SetLabelText(orderLabel, _currentOrderInPress.OrderNumber);
            SetLabelText(materialLabel, $"{_currentOrderInPress.MaterialCode}\n\r{_currentOrderInPress.MaterialDesc}");
            quantityLabel.Text = $"{Convert.ToInt32(_currentCompletedQty)} / {Convert.ToInt32(_taskQuantity)}";

            // 3. 加载 BOM
            var boms = await _facade.WorkOrderBomItemService.GetListByOrderIdAync(_currentOrderInPress.OrderId);
            _workOrderBoms = boms.Where(x => x.ConsumeType == (int)ConsumeType.BulkMaterial).ToList(); // 散装料
            bomTable.DataSource = _workOrderBoms;

            // 4. 打开工位 Tab 页
            await OpenStationTabAsync();
        }

        private void SetLabelText(AntdUI.Label label, string value)
        {
            // 简单处理冒号分割
            var parts = label.Text.Split('：');
            label.Text = parts.Length > 0 ? $"{parts[0]}：{value}" : value;
        }

        #region Tab Management

        private async Task OpenStationTabAsync()
        {
            foreach (var item in _workOrderTaskDtos)
            {
                // 获取该任务的报工记录
                var confirms = await _facade.Confirm.GetListByTaskIdAsync(item.Id);
                var dataSource = confirms.OrderByDescending(x => x.ConfirmDate).ToList();

                var station = await _facade.WorkStation.GetByIdAsync((int)item.WorkStationId);
                if (station == null)
                    continue;

                OpenOrFocusTab(station.Id.ToString(), station.WorkStationName, () =>
                {
                    // 创建表格控件
                    var table = new AntdUI.Table
                    {
                        Dock = DockStyle.Fill,
                        Columns = CreateConfirmColumns(),
                        DataSource = dataSource,
                        Font = new Font("Microsoft YaHei UI", 11.0f)
                    };
                    return table;
                });
            }
        }


        // 【修复】显式指定 System.Windows.Forms.Control 解决歧义
        private void OpenOrFocusTab(string key, string title, Func<System.Windows.Forms.Control> controlFactory)
        {
            // 检查是否存在
            var existingPage = confirmTabs.Pages.OfType<AntdUI.TabPage>().FirstOrDefault(p => p.Name == key);

            if (existingPage != null)
            {
                // 刷新内容
                var newContent = controlFactory();
                existingPage.Controls.Clear();
                existingPage.Controls.Add(newContent);
                confirmTabs.SelectedTab = existingPage;
            }
            else
            {
                // 创建新页
                var newPage = new AntdUI.TabPage
                {
                    Text = title,
                    Name = key,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(10)
                };

                var content = controlFactory();
                if (content != null)
                    newPage.Controls.Add(content);

                confirmTabs.Pages.Add(newPage);
                confirmTabs.SelectedTab = newPage;
            }
        }

        #endregion

        #region Submission Logic

        private async void submitButton_Click(object sender, EventArgs e)
        {
            await RunAsync(submitButton, async () =>
            {
                // --- 1. 基础校验 ---
                if (reportInputNumber.Value <= 0)
                    throw new Exception("请填写报工数量！");

                if (_currentCompletedQty + reportInputNumber.Value > _taskQuantity)
                    throw new Exception("报工数量超出订单计划数量，请修改！");

                // 自动完工判定
                if (_currentCompletedQty + reportInputNumber.Value == _taskQuantity)
                {
                    this.Invoke((MethodInvoker)delegate {
                        completeSwitch.Checked = true;
                    });
                }

                // --- 2. 二次确认 (如果完工) ---
                if (completeSwitch.Checked)
                {
                    if (AntdUI.Modal.open(new AntdUI.Modal.Config(this, "提示", "当前任务即将完工，是否确认提交？", AntdUI.TType.Success)) != DialogResult.OK)
                        throw new Exception("报工已取消！");
                }
                // --- 2.1 验证完成数量
                // 加载工位
                var allStations = await _facade.WorkStation.GetAllAsync(AppSession.CurrentFactoryId);
                if (allStations != null)
                {


                    var validStations = allStations.Where(x => x.WorkAreaId == 2 && x.WorkCenterId == 0).ToList();
                    var startStation = validStations.Where(x => x.IsStartStep).First();

                    // 提取工序Id
                    var processId = _workOrderTaskDtos.Select(x => x.OrderProcessId).Distinct().First();
                    if (processId > 0)
                    {

                        var temptasks = await _facade.Task.GetByProcessIdsAsync(new List<int>() { processId });
                        if (temptasks == null || temptasks.Count() == 0)
                            throw new Exception("未查询到当前工序下的任务信息！");
                        //获取当前工序首道任务
                        var startTask = temptasks.Where(x => x.WorkStationId == startStation.Id).FirstOrDefault();
                        if (startTask != null)
                        {
                            //判断当前执行的任务是否包含首道工步的任务
                            decimal startConfirm = (decimal)startTask.CompletedQty;
                            var currentStartTask = _workOrderTaskDtos.Where(x => x.WorkStationId == startStation.Id).FirstOrDefault();
                            if (currentStartTask != null)
                            {
                                startConfirm += reportInputNumber.Value;
                            }
                            //本次报工不允许超过首道工步的数量
                            if (_workOrderTaskDtos.Where(x => (x.CompletedQty + reportInputNumber.Value) > startConfirm).Count() > 0)
                                throw new Exception("当前报工数量已超出首道工步报工数量，无法报工！");


                        }
                    }
                }

                // --- 3. 构建数据对象 ---
                var taskUpdateDtos = new List<WorkOrderTaskUpdateDto>();
                var confirmDtos = new List<WorkOrderTaskConfirmCreateDto>();

                // 为每个关联任务生成报工记录 (装配通常涉及多个工位协同，或者同一操作分发给多工位)
                foreach (var task in _workOrderTaskDtos)
                {
                    // 重新获取最新 Task 状态 (避免并发问题)
                    var taskDto = await _facade.Task.GetByIdAsync(task.Id);

                    taskUpdateDtos.Add(new WorkOrderTaskUpdateDto
                    {
                        Id = taskDto.Id,
                        Status = completeSwitch.Checked ? "4" : "2", // 4=Finished, 2=Processing
                        CompletedQty = taskDto.CompletedQty + reportInputNumber.Value,
                        UpdateBy = AppSession.CurrentUser.EmployeeId,
                        UpdateOn = DateTime.Now,
                    });

                    confirmDtos.Add(new WorkOrderTaskConfirmCreateDto
                    {
                        TaskId = taskDto.Id,
                        WorkStationId = (int)taskDto.WorkStationId,
                        ConfirmNumber = _facade.Serial.GenerateNext("ReportLabelSerial"),
                        ConfirmDate = DateTime.Now,
                        ConfirmQuantity = reportInputNumber.Value,
                        EmployerCode = AppSession.CurrentUser.EmployeeId,
                        Status = completeSwitch.Checked ? "1" : "0",
                        Remark = reportRemarkInput.Text.Trim(),
                    });
                }

                // --- 4. 事务提交 ---
                // =====================================================================
                // 【核心修复】：创建一个临时的 Scope 来执行保存事务
                // =====================================================================
                using (var transactionScope = _scopeFactory.CreateScope())
                {
                    // 1. 从新 Scope 中获取全新的 Facade
                    // 这个 Facade 里的 UnitOfWork 是全新的，干净的，没有之前的事务残留
                    var transactionalFacade = transactionScope.ServiceProvider.GetRequiredService<AssemblyModuleFacade>();

                    // 2. 使用全新的 Facade 执行事务性业务
                    // 为了确保数据一致性，我们可以在这里重新读取 Task 的最新 CompletedQty (可选优化)
                    // 但为了保持您原有逻辑，直接使用构建好的 DTO
                    var confirmIds = await transactionalFacade.Confirm.CreateByAssmAsync(confirmDtos, null, taskUpdateDtos);

                    // --- 5. 后置逻辑 (SAP 报工) ---
                    // SAP 报工通常不涉及 MES 数据库事务，或者有自己的事务处理
                    // 可以在这个 Scope 中执行，也可以在外面
                    await TryAutoConfirmToSapAsync(confirmIds,transactionalFacade);
                }

                // --- 5. 后置逻辑 (完工后的 SAP 报工等) ---
                if (completeSwitch.Checked)
                {
                    // 提示并关闭
                    AntdUI.Modal.open(new AntdUI.Modal.Config(this, "保存成功", "当前任务已完工，请重新选择！", AntdUI.TType.Success));
                    await LoadOrderData.Invoke();
                    this.Close();
                }
                else
                {
                    // 重置 UI 并刷新
                    reportInputNumber.Value = 0;
                    scraptInputNumber.Value = 0;
                    scrapResionSelect.Text = string.Empty;
                    reportRemarkInput.Text = string.Empty;

                    AntdUI.Message.success(this, "保存成功！");
                    await LoadDataAsync(); // 刷新界面数据
                }
            });
        }

        private async Task TryAutoConfirmToSapAsync(List<int> confirmIds, AssemblyModuleFacade? facade = null)
        {
            // 如果没有传新 facade，就用默认的（但在事务场景下最好传新的）
            var f = facade ?? _facade;
            // 内部辅助函数：带重试的 API 调用
            async Task CallSapWithRetryAsync(Func<Task<ApiResponse<object>>> apiAction, string actionName)
            {
                const int MaxRetries = 3;
                const int DelayMilliseconds = 5000; // 5秒

                for (int i = 0; i <= MaxRetries; i++)
                {
                    try
                    {
                        var result = await apiAction();

                        if (result != null && !result.IsSuccess)
                        {
                            // 检查错误信息是否包含锁定关键字
                            string msg = result.Message?.ToLower() ?? "";
                            bool isLocked = Regex.IsMatch(msg, @"reservation\s+\d+\s+is\s+already\s+being\s+processed", RegexOptions.IgnoreCase);

                            if (isLocked && i < MaxRetries)
                            {
                                // 是锁定错误且未达最大次数，等待后重试
                                await Task.Delay(DelayMilliseconds);
                                continue;
                            }

                            // 其他错误或重试耗尽，抛出异常
                            throw new Exception(result.Message);
                        }

                        // 成功则直接返回
                        return;
                    }
                    catch (Exception ex)
                    {
                        // 捕获网络层面的异常或手动抛出的业务异常
                        string msg = ex.Message.ToLower();
                        bool isLocked = Regex.IsMatch(msg, @"reservation\s+\d+\s+is\s+already\s+being\s+processed", RegexOptions.IgnoreCase);

                        if (isLocked && i < MaxRetries)
                        {
                            await Task.Delay(DelayMilliseconds);
                            continue;
                        }

                        // 包装异常信息以便上层捕获显示
                        throw new Exception($"{actionName}失败: {ex.Message}");
                    }
                }
            }

            try
            {
                var workstations = await _facade.WorkStation.GetByIdAsync(_workOrderTaskDtos.Select(x => (int)x.WorkStationId).ToList());

                var request = new WorkOrderReportRequest()
                {
                    FactoryCode = AppSession.CurrentUser.FactoryName,
                    EmployeeId = AppSession.CurrentUser.EmployeeId,

                };

                try
                {
                    //判断当前报工中是否存在材料准备工步，存在的话前一道工序进行补报工
                    if (workstations.Where(x => x.IsStartStep).Any() && _currentOrderInPress.PrevProcessId != null)
                    {
                        var startStation = workstations.Where(x => x.IsStartStep).First();

                        var firstConfirm = await _facade.Confirm.GetByStationIdAsync(_currentOrderInPress.OrderProcessId, startStation.Id, confirmIds);
                        var prevProcessId = _currentOrderInPress.PrevProcessId;
                        //var prevOperationConfirm = (await _facade.OperationConfirm.GetListByProcessIdAsync((int)prevProcessId)).FirstOrDefault();

                        request.ProcessId = (int)prevProcessId;
                        request.ConfirmId = firstConfirm.Id;
                        await CallSapWithRetryAsync(async () =>
                        {
                            var prevOperationConfirm = (await _facade.OperationConfirm.GetListByProcessIdAsync(request.ProcessId)).Where(x => x.TaskConfirmId == request.ConfirmId).FirstOrDefault();

                            var result = new ApiResponse<object>();
                            result.IsSuccess = true;
                            if (prevOperationConfirm == null)
                            {
                                // 未报工 -> 补报工 (使用重试)
                                var url = f.ApiSettings["MesApi"].Endpoints["OperationConfirmToSAP"];
                                result = await f.MesApi.PostAsync<object, object>(url, request);
                                prevOperationConfirm = (await _facade.OperationConfirm.GetListByProcessIdAsync(request.ProcessId)).Where(x => x.TaskConfirmId == request.ConfirmId).FirstOrDefault();
                            }
                            else if (prevOperationConfirm.Status != "1")
                            {
                                // 报工失败 -> 重推 (使用重试)
                                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                                query["confirmid"] = prevOperationConfirm.Id.ToString();
                                string endpoint = f.ApiSettings["MesApi"].Endpoints["ReentryConfirmToSAP"];
                                result = await f.MesApi.PostAsync<object, object>($"{endpoint}?{query}", null);
                            }
                            return result;
                        }, $"{startStation.WorkStationName}报工");
                    }

                }
                catch (Exception)
                {
                }
               
                if (workstations.Where(x => x.IsEndStep).Any())
                {
                    var endStation = workstations.First(x => x.IsEndStep);

                    //获取当前完工工位的报工
                    var endConfirm = await _facade.Confirm.GetByStationIdAsync(_currentOrderInPress.OrderProcessId, endStation.Id, confirmIds);

                    // 2. 报工当前工序

                    request.ProcessId = _currentOrderInPress.OrderProcessId;
                    request.ConfirmId = endConfirm.Id;

                    // 完工报工 (使用重试)
                    await CallSapWithRetryAsync(async () =>
                    {
                        //判断当前是否已有报工记录
                        var currOperationConfirm = (await _facade.OperationConfirm.GetListByProcessIdAsync(request.ProcessId)).Where(x => x.TaskConfirmId == request.ConfirmId).FirstOrDefault();
                        var result = new ApiResponse<object>();
                        result.IsSuccess = true;
                        if (currOperationConfirm == null)
                        {
                            var url = f.ApiSettings["MesApi"].Endpoints["OperationConfirmToSAP"];
                            result = await f.MesApi.PostAsync<object, object>(url, request);
                            currOperationConfirm = (await _facade.OperationConfirm.GetListByProcessIdAsync(request.ProcessId)).Where(x => x.TaskConfirmId == request.ConfirmId).FirstOrDefault();
                        }
                        else if (currOperationConfirm.Status != "1")
                        {
                            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                            query["confirmid"] = currOperationConfirm.Id.ToString();
                            string endpoint = f.ApiSettings["MesApi"].Endpoints["ReentryConfirmToSAP"];

                            result = await f.MesApi.PostAsync<object, object>($"{endpoint}?{query}", null);
                        }
                        return result;
                    }, $"{endStation.WorkStationName}报工");
   

   
                }

                //// 获取结束工位
                //var allStations = await _facade.WorkStation.GetAllAsync(AppSession.CurrentFactoryId);
                //var endStation = allStations.FirstOrDefault(x => x.WorkAreaId == 2 && x.WorkCenterId == 0 && x.IsEndStep);

                //if (endStation == null)
                //    throw new Exception("当前工位组未配置结束工位，请联系管理员进行配置！");


                //var request = new WorkOrderReportRequest() 
                //{
                //    FactoryCode = AppSession.CurrentUser.FactoryName,
                //    ProcessId = _currentOrderInPress.OrderProcessId,
                //    EmployeeId = AppSession.CurrentUser.EmployeeId,
                //    ConfirmId = endConfirm.Id,
                //};


            }
            catch (Exception ex)
            {
                // 完工后的 SAP 报错不应阻断主流程，只需提示
                AntdUI.Message.error(this.ParentForm, "任务已完工，报工至 SAP 时发生异常: " + ex.Message);
            }
        }

        public async Task ConfirmToSapByConfirmIdAsync(int confirmId)
        {
            var group = await _facade.Params.GetGroupWithItemsAsync("TransferSAP");
            var isEnabled = group?.Items.FirstOrDefault(x => x.Key == "IsEnabled")?.Value;

            if (bool.TryParse(isEnabled, out bool enabled) && enabled)
            {
                // 使用 _facade.SapRfc (原 SapRfcService)
                var confirmResult = await _facade.SapRfc.ConfirmOrderCompletionToSAPAsync(confirmId);

                if (confirmResult != null && confirmResult.MessageType?.ToUpper() == "S")
                {
                    await _facade.OperationConfirm.UpdateAsync(new WorkOrderOperationConfirmUpdateDto
                    {
                        Id = confirmId,
                        Message = confirmResult.Message,
                        MessageType = confirmResult.MessageType,
                        Status = "1",
                        UpdatedAt = DateTime.Now,
                    });
                }
            }
        }

        #endregion

        #region UI Helpers

        private async void backPicture_Click(object sender, EventArgs e)
        {
            backPicture.Enabled = false;
            await LoadOrderData.Invoke();
            this.Close();
        }

        private void InitializeTable()
        {
            bomTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetFixed().SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialDesc", "物料描述", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("RequiredQuantity", "需求数量", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column.").SetDisplayFormat("F3"),
                new AntdUI.Column("BomItem", "BOM行", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("BaseUnit", "物料单位", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
            };
        }

        private AntdUI.ColumnCollection CreateConfirmColumns()
        {
            return new AntdUI.ColumnCollection
            {
                new AntdUI.Column("ConfirmNumber", "报工序号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConfirmQuantity", "报工数量", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ConfirmDate", "报工日期", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("EmployerCode", "报工人员", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Remark", "备注", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
            };
        }

        #endregion
    }
}
