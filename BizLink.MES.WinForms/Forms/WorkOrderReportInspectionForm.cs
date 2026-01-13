using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Facade;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using Dm.util;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class WorkOrderReportInspectionForm : MesBaseForm
    {
        private readonly AssemblyModuleFacade _facade;
        private readonly IFormFactory _formFactory;
        private readonly IServiceScopeFactory _scopeFactory; // 【新增】用于创建临时事务 Scope


        //private const string WorkCenterGroup_FQC = "FQC";
        private WorkOrderTaskDto _workOrderTask;

        public WorkOrderReportInspectionForm(AssemblyModuleFacade facade, IFormFactory formFactory, IServiceScopeFactory scopeFactory)
        {
            InitializeComponent();
            InitializeTable();
            _facade = facade;
            _formFactory = formFactory;
            _scopeFactory = scopeFactory;
        }


        private void InitializeTable()
        {
            OrderTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("OrderNumber", "订单/日期", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is V_WorkOrderInProgress model)
                            {
                                // 2. 自由组合多个字段
                                // 这里演示将 "订单号" 和 "工序" 拼接，中间换行
                                return model.OrderNumber + "\r\n" + model.DispatchDate.Value.ToString("yyyy-MM-dd");
        
                                // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                // return new CellText(model.OrderNo, model.Process); 
                            }
                            return value;
                    }
                }.SetFixed().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "数量", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is V_WorkOrderInProgress model)
                            {
                                // 2. 自由组合多个字段
                                // 这里演示将 "订单号" 和 "工序" 拼接，中间换行
                                return model.CompletedQty.ToString() + "\r\n" + model.Quantity.ToString();
        
                                // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                // return new CellText(model.OrderNo, model.Process); 
                            }
                            return value;
                    }
                }.SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "物料信息", AntdUI.ColumnAlign.Left)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is V_WorkOrderInProgress model)
                            {
                                // 2. 自由组合多个字段
                                // 这里演示将 "订单号" 和 "工序" 拼接，中间换行
                                return $"订单物料：{model.MaterialCode}\r\n成品物料：{model.LeadingOrderMaterial}\r\n{model.MaterialDesc}";
        
                                // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                // return new CellText(model.OrderNo, model.Process); 
                            }
                            return value;
                    }
                }.SetWidth("200").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "0" => new AntdUI.CellTag("未准备", AntdUI.TTypeMini.Error),
                            "1" => new AntdUI.CellTag("已排产", AntdUI.TTypeMini.Primary),
                            "2" => new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Default),
                            "3" => new AntdUI.CellTag("已挂起", AntdUI.TTypeMini.Success),
                            _ => null
                        };
                    }
                }.SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenter", "工作中心/BU", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        if (record is V_WorkOrderInProgress model)
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


            };


            ConfTable.Columns = new AntdUI.ColumnCollection 
            {
                new AntdUI.Column("ConfirmNumber", "报工序号", AntdUI.ColumnAlign.Center){
                    Render = (value, record, index) =>
                    {
                        if (record is WorkOrderTaskConfirmDto model)
                            {
                                // 2. 自由组合多个字段
                                // 这里演示将 "订单号" 和 "工序" 拼接，中间换行
                                return model.ConfirmNumber + "\r\n" + model.ConfirmDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
        
                                // 进阶：如果 AntdUI 版本支持，可以使用 CellText 显示主副标题样式
                                // return new CellText(model.OrderNo, model.Process); 
                            }
                            return value;
                    }
                }.SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("ConfirmQuantity", "报工数量", AntdUI.ColumnAlign.Center).SetDisplayFormat("0").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("EmployerCode", "报工人员", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Status", "完工标识", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        return value as string switch
                        {
                            "1" => new AntdUI.CellTag("完工", AntdUI.TTypeMini.Success),
                            _ => null
                        };
                    }
                }.SetLocalizationTitleID("Table.Column."),
            };
        }


        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            DispathchDatePickerRange.PlaceholderStart = "请选择开始日期...";
            DispathchDatePickerRange.PlaceholderEnd = "请选择结束日期...";

            OrderScanInput.PlaceholderText = "请扫描或输入订单号...";
            WorkcenterGroupSelect.PlaceholderText = "请选择工作中心组...";
            WorkcenterSelect.PlaceholderText = "请选择工作中心...";

            ToggleInputs(true);
            await RunAsync(async () =>
            {
                await LoadWorkCenterGroupAsync();
                //LoadCollapseAsync(null);
            });
        }

        public async System.Threading.Tasks.Task LoadOrderDataAsync(int groupId, DateTime dateStart, DateTime dateEnd)
        {

            await RunAsync(async () =>
            {
                OrderTable.DataSource = null;
                var result = await _facade.View.GetListByWorkCenterGroupAsync(AppSession.CurrentFactoryId, groupId, dateStart, dateEnd);
                if (result != null && result.Count() > 0)
                {
                    var fqclist = result.Where(x => ((x.PrevProcessId == null && x.ControlKey == "CN06") || x.PrevProcessId != null) && x.Status != ((int)WorkOrderStatus.Finished).ToString()).OrderBy(x => x.OrderNumber).OrderByDescending(x => x.Status ?? "0").ToList();

                    if (fqclist != null && fqclist.Count() > 0)
                    {

                        OrderTable.DataSource = fqclist;
                    }
                    else
                        throw new Exception("未查询到待办订单信息！");
                }
                else
                    throw new Exception("未查询到待办订单信息！");
            });

        }

        public async System.Threading.Tasks.Task LoadConfirmDataAsync(int taskId)
        {
            await RunAsync(async () => 
            {
                ConfTable.DataSource = null;
                var task = await _facade.Task.GetByIdAsync(taskId);
                if (task !=null && task.Status == ((int)WorkOrderStatus.Finished).ToString())
                {
                    SubmitButton.Enabled = false;

                }
                else
                    SubmitButton.Enabled = true;

                var result = await _facade.Confirm.GetListByTaskIdAsync(taskId);
                if (result != null && result.Count() > 0)
                {
                    ConfTable.DataSource = result;
                }
            });
        }


        private async System.Threading.Tasks.Task LoadWorkCenterGroupAsync()
        {
            var workcentergroup = await _facade.WorkCenterGroup.GetListByGroupTypeAsync(AppSession.CurrentFactoryId, ((int)WorkCenterGroupType.InspectionGroup).ToString());
            if (workcentergroup != null)
            {
                WorkcenterGroupSelect.Items.Clear();
                WorkcenterGroupSelect.Items.AddRange(workcentergroup.Select(g => new MenuItem()
                {
                    Name = g.Id.ToString(),
                    Text = g.GroupCode + "-" + g.GroupName
                }).ToArray());

            }
        }

        private async System.Threading.Tasks.Task LoadWorkCenterAsync(int groupid)
        {
            var workcenters = await _facade.WorkCenter.GetBygroupIdAsync(groupid);
            if (workcenters != null)
            {
                WorkcenterSelect.Items.Clear();
                WorkcenterSelect.Items.AddRange(workcenters.Select(w => new MenuItem()
                {
                    Name = w.Id.ToString(),
                    Text = w.WorkCenterCode + "-" + w.WorkCenterName
                }).ToArray());
            }
        }

        private async void WorkcenterGroupSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            if (e.Value is MenuItem item)
            {
                if (int.TryParse(item.Name, out int groupid))
                {
                    await RunAsync(async () =>
                    {
                        await LoadWorkCenterAsync(groupid);
                    });
                }
            }
        }

        private void WorkcenterSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            WorkcenterSelect.Select(0, 0);
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {

            await RunAsync(SearchButton, async () =>
            {

                if (SearchButton.Text.Contains("工位确认"))
                {
                    if (DispathchDatePickerRange.Value == null)
                        throw new Exception("请先选择订单排产日期！");
                    var dateStart = DispathchDatePickerRange.Value.FirstOrDefault();
                    var dateEnd = DispathchDatePickerRange.Value.LastOrDefault();
                    if (dateStart == null || dateEnd == null)
                        throw new Exception("请先选择订单排产日期！");

                    var workcenterGroup = (MenuItem)WorkcenterGroupSelect.SelectedValue;
                    int GroupId = 0;
                    if (workcenterGroup == null || !int.TryParse(workcenterGroup.Name, out GroupId))
                        throw new Exception("请先选择工作中心组！");

                    int workcenterId = 0;
                    var workcenter = (MenuItem)WorkcenterSelect.SelectedValue;
                    if (workcenter == null || !int.TryParse(workcenter.Name, out workcenterId))
                        throw new Exception("请先选择工作中心！");

                    await LoadOrderDataAsync(GroupId, dateStart, dateEnd);

                    ToggleInputs(false);
                }
                else
                    ToggleInputs(true);


            });
        }


        private void SetLabel(AntdUI.Label lbl, string? value)
        {
            string prefix = lbl.Text.Contains("：") ? lbl.Text.Split('：')[0] : lbl.Text;
            lbl.Text = $"{prefix}：{value ?? "-"}";
        }
        private async void OrderScanInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\r')
                return;
            var orderNo = OrderScanInput.Text.Trim();
            if (string.IsNullOrEmpty(orderNo))
                return;
            await RunAsync(async () =>
            {
                //var processId = 0;
                var item = MoveOrderToTop(orderNo);
                if (item == null)
                    throw new Exception($"未找到订单号 {orderNo} 的待办订单信息！");

                var workorderProcess = await _facade.WorkOrderProcessService.GetByIdAsync(item.OrderProcessId);
                var workorder = await _facade.WorkOrderService.GetByIdAsync(item.OrderId);
                SetLabel(Orderlabel, workorder.OrderNumber);
                SetLabel(Matlabel, workorder.MaterialCode);
                SetLabel(Desclabel, workorder.MaterialDesc);
                SetLabel(LeadMatlabel, workorder.LeadingOrderMaterial ?? "-");
                SetLabel(confLabel, $"{workorderProcess.CompletedQuantity ?? 0}/{workorderProcess.Quantity ?? 1}");
                progressControl.Value = (float)((workorderProcess.CompletedQuantity ?? 0) / (workorderProcess.Quantity ?? 1));
                if (progressControl.Value == 0)
                {
                    progressControl.State = TType.Error;
                }

                var allStations = await _facade.WorkStation.GetAllAsync(AppSession.CurrentFactoryId);
                if (allStations != null)
                {

                    var endStation = allStations.Where(x => x.WorkAreaId == 2 && x.WorkCenterId == 0 && x.IsEndStep).First();
                    // 2. 准备创建 DTO 列表
                    var createDtos = new WorkOrderTaskCreateDto()
                    {
                        OrderProcessId = workorderProcess.Id,
                        OrderId = workorder.Id,
                        OrderNumber = workorder.OrderNumber,
                        TaskNumber = $"{workorderProcess.WorkOrderNo}-{workorderProcess.Operation}",
                        WorkStationId = endStation.Id,
                        MaterialCode = workorder.MaterialCode,
                        MaterialDesc = workorder.MaterialDesc,
                        Status = ((int)WorkOrderStatus.New).ToString(),
                        Quantity = workorder.Quantity,
                        CompletedQty = 0,
                        NextWorkCenter = workorderProcess.NextWorkCenter,
                        Operation = workorderProcess.Operation,
                        ProfitCenter = workorder.ProfitCenter,
                        StartTime = workorderProcess.StartTime,
                        DispatchDate = workorderProcess.EndTime,
                        WorkCenter = ((MenuItem)WorkcenterSelect.SelectedValue).Text.split("-")[0] ?? workorderProcess.WorkCenter,
                        Remark = workorder.PlannerRemark,
                        CreateBy = AppSession.CurrentUser.EmployeeId,
                    };

                    // 3. 检查任务是否存在
                    var tasks = await _facade.Task.GetByTaskNoAsync(createDtos.TaskNumber);
                    if (tasks != null && tasks.Any())
                    {
                        var task = tasks.First();
                        SetLabel(confLabel, $"{(int)task.CompletedQty}/{(int)task.Quantity}");
                        progressControl.Value = (float)((task.CompletedQty ?? 0) / (task.Quantity ?? 1));
                        if (progressControl.Value > 0)
                            progressControl.State = TType.Success;
                        _workOrderTask = task;

                    }
                    else
                    {
                        _workOrderTask = await _facade.Task.CreateAsync(createDtos);
                    }

                    LoadConfirmDataAsync(_workOrderTask.Id);

                }


            });

        }

        /// <summary>
        /// 功能：扫码置顶
        /// Table 的置顶非常简单，只需要操作数据源 List，然后重新赋值 DataSource 即可
        /// </summary>
        public V_WorkOrderInProgress? MoveOrderToTop(string orderNo)
        {
            var orderDatas = OrderTable.DataSource as List<V_WorkOrderInProgress>;
            var target = orderDatas.FirstOrDefault(x => x.OrderNumber == orderNo);
            if (target != null)
            {
                // 1. 修改数据源顺序
                orderDatas.Remove(target);
                orderDatas.Insert(0, target);

                // 2. 刷新表格 (AntdUI Table 重新赋值 DataSource 会触发重绘，非常快)
                OrderTable.DataSource = null; // 有时候需要重置一下触发刷新，视版本而定
                OrderTable.DataSource = orderDatas;

                // 3. 选中第一行并滚动
                OrderTable.SelectedIndex = 1;
                OrderTable.ScrollLine(1, true);
            }
            return target;
        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SubmitButton,async () => 
            {
                using (var transactionScope = _scopeFactory.CreateScope())
                {
                    var task = await _facade.Task.GetByIdAsync(_workOrderTask.Id);

                    if (task.Status == ((int)WorkOrderStatus.Finished).ToString())
                    {
                        throw new Exception("该订单工序已完成，无法报工！");
                    }
                    if (ConfInputNumber.Value + task.CompletedQty > task.Quantity)
                    {
                        throw new Exception("报工数量超出订单数量，无法报工！");
                    }

                    var scopedFacade = transactionScope.ServiceProvider.GetRequiredService<AssemblyModuleFacade>();
                    var taskUpdateDto = new WorkOrderTaskUpdateDto
                    {
                        Id = task.Id,
                        Status = ConfInputNumber.Value + task.CompletedQty >= task.Quantity ? "4" : "2", // 4=Finished, 2=Processing
                        CompletedQty = ConfInputNumber.Value + task.CompletedQty,
                        UpdateBy = AppSession.CurrentUser.EmployeeId,
                        UpdateOn = DateTime.Now,
                    };
                    var taskConfirmDto = new WorkOrderTaskConfirmCreateDto
                    {
                        TaskId = task.Id,
                        WorkStationId = (int)task.WorkStationId,
                        ConfirmNumber = scopedFacade.Serial.GenerateNext("ReportLabelSerial"),
                        ConfirmDate = DateTime.Now,
                        ConfirmQuantity = ConfInputNumber.Value,
                        EmployerCode = AppSession.CurrentUser.EmployeeId,
                        Status = taskUpdateDto.Status == ((int)WorkOrderStatus.Finished).ToString() ? "1" : "0",
                        //Remark = reportRemarkInput.Text.Trim(),
                    };

                    var confirmIds = await scopedFacade.Confirm.CreateByAssmAsync(new List<WorkOrderTaskConfirmCreateDto>() { taskConfirmDto }, null, new List<WorkOrderTaskUpdateDto>() { taskUpdateDto });

                    task = await scopedFacade.Task.GetByIdAsync(task.Id);

                    SetLabel(confLabel, $"{(int)task.CompletedQty}/{(int)task.Quantity}");
                    progressControl.Value = (float)((task.CompletedQty ?? 0) / (task.Quantity ?? 1));
                    if (progressControl.Value > 0)
                        progressControl.State = TType.Success;
                    _workOrderTask = task;

                    ConfInputNumber.Value = 0;


                    await TryAutoConfirmToSapAsync(confirmIds.First(), scopedFacade);

                }

                LoadConfirmDataAsync(_workOrderTask.Id);


            }, confirmMsg:$"即将对订单{_workOrderTask.OrderNumber}进行报工，是否继续？",successMsg:"报工成功！");
        }

        private void ToggleInputs(bool enable)
        {
            DispathchDatePickerRange.Enabled = enable;
            WorkcenterGroupSelect.Enabled = enable;
            WorkcenterSelect.Enabled = enable;

            if (!enable)
            {
                SearchButton.Text = "解除确认";
                SearchButton.Type = AntdUI.TTypeMini.Error;
                OrderScanInput.Enabled = true;
            }
            else
            {
                SubmitButton.Enabled = false;
                SearchButton.Text = "工位确认";
                SearchButton.Type = AntdUI.TTypeMini.Primary;
                OrderScanInput.Enabled = false;
            }
        }

        private async System.Threading.Tasks.Task TryAutoConfirmToSapAsync(int confirmId, AssemblyModuleFacade? facade = null)
        {
            // 如果没有传新 facade，就用默认的（但在事务场景下最好传新的）
            var f = facade ?? _facade;
            // 内部辅助函数：带重试的 API 调用
            async System.Threading.Tasks.Task CallSapWithRetryAsync(Func<Task<ApiResponse<object>>> apiAction, string actionName)
            {
                const int MaxRetries = 3;
                const int DelayMilliseconds = 2000; // 2秒

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
                                await System.Threading.Tasks.Task.Delay(DelayMilliseconds);
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
                            await System.Threading.Tasks.Task.Delay(DelayMilliseconds);
                            continue;
                        }

                        // 包装异常信息以便上层捕获显示
                        throw new Exception($"{actionName}失败: {ex.Message}");
                    }
                }
            }

            try
            {

                var request = new WorkOrderReportRequest()
                {
                    FactoryCode = AppSession.CurrentUser.FactoryName,
                    EmployeeId = AppSession.CurrentUser.EmployeeId,

                };

                request.ProcessId = _workOrderTask.OrderProcessId;
                request.ConfirmId = confirmId;

                //判断当前是否已有报工记录
                var currOperationConfirm = (await _facade.OperationConfirm.GetListByProcessIdAsync(request.ProcessId)).Where(x => x.TaskConfirmId == request.ConfirmId).FirstOrDefault();
                // 完工报工 (使用重试)
                await CallSapWithRetryAsync(async () =>
                {
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
                }, $"{_workOrderTask.TaskNumber}报工");



            }
            catch (Exception ex)
            {
                // 完工后的 SAP 报错不应阻断主流程，只需提示
                AntdUI.Message.error(this.ParentForm, "任务已完工，报工至 SAP 时发生异常: " + ex.Message);
            }
        }
    }
}
