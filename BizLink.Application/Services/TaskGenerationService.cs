using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Enums;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class TaskGenerationService : ITaskGenerationService
    {
        private readonly IWorkOrderOperationTaskService _operationTaskService;
        private readonly IWorkOrderStepTaskService _stepTaskService;
        private readonly IWorkOrderMaterialTaskService _materialTaskService;
        private readonly IWorkTaskCategoryService _workTaskCategoryService;
        private readonly IWorkCenterGroupService _workCenterGroupService;
        private readonly ICableCutParamService _cableCutParamService;

        // 需要注入基础数据服务来获取 Order, Process, Bom
        private readonly IWorkOrderService _orderService;
        private readonly IWorkOrderProcessService _processService;
        private readonly IWorkOrderBomItemService _bomService;
        public TaskGenerationService(IWorkOrderOperationTaskService operationTaskService, IWorkOrderStepTaskService stepTaskService, IWorkOrderMaterialTaskService materialTaskService, IWorkOrderService orderService, IWorkOrderProcessService processService, IWorkOrderBomItemService bomService, IWorkCenterGroupService workCenterGroupService, ICableCutParamService cableCutParamService, IWorkTaskCategoryService workTaskCategoryService)
        {
            _operationTaskService = operationTaskService;
            _stepTaskService = stepTaskService;
            _materialTaskService = materialTaskService;
            _orderService = orderService;
            _processService = processService;
            _bomService = bomService;
            _workCenterGroupService = workCenterGroupService;
            _workTaskCategoryService = workTaskCategoryService;
            _cableCutParamService = cableCutParamService;
        }

        public async Task GenerateOrUpdateTasksAsync(List<string> orderNos, string currentUser)
        {
            var WorkOrders = await _orderService.GetByOrdrNoAsync(orderNos);
            var Processes = await _processService.GetListByOrderIdsAync(WorkOrders.Select(w => w.Id).ToList());
            var boms = await _bomService.GetListByProcessIdsAsync(Processes.Select(w => w.Id).ToList());
            var existingOpTasks = await _operationTaskService.GetListByProcessIdAsync(Processes.Select(w => w.Id).ToList());
            var workcenterGroups = await _workCenterGroupService.GetByWorkCenterCodeAsync(Processes.Select(P => P.WorkCenter).ToList());

            var catCutting = await _workTaskCategoryService.GetByCategoryCodeAsync(TaskCategories.Cutting);
            var catPicking = await _workTaskCategoryService.GetByCategoryCodeAsync(TaskCategories.Picking);
            var catKitting = await _workTaskCategoryService.GetByCategoryCodeAsync(TaskCategories.Kitting);
            //List<int> opTaskIds = new List<int>();
            var opTasksToProcessCreate = new List<WorkOrderOperationTaskCreateDto>();
            var opTasksToProcessUpdate = new List<WorkOrderOperationTaskUpdateDto>();
            foreach (var item in Processes)
            {
                var workcenterGroup = workcenterGroups.Where(g => g.WorkCenterCode == item.WorkCenter).FirstOrDefault();
                var opTasksToProcess = existingOpTasks.Where(t => t.WorkOrderProcessId == item.Id).ToList();
                if (!opTasksToProcess.Any())
                {
                    opTasksToProcessCreate.Add(new WorkOrderOperationTaskCreateDto()
                    {
                        WorkOrderId = item.WorkOrderId,
                        WorkOrderNo = item.WorkOrderNo,
                        WorkOrderProcessId = item.Id,
                        Operation = item.Operation,
                        Quantity = item.Quantity,
                        WorkCenterCode = item.WorkCenter,
                        DispatchDate = item.StartTime,
                        Status = WorkTaskStatus.New,
                        CreatedBy = currentUser
                    });
                    //var newOpTask = await _operationTaskService.CreateAsync(new WorkOrderOperationTaskCreateDto()
                    //{
                    //    WorkOrderId = item.WorkOrderId,
                    //    WorkOrderNo = item.WorkOrderNo,
                    //    WorkOrderProcessId = item.Id,
                    //    Operation = item.Operation,
                    //    Quantity = item.Quantity,
                    //    WorkCenterCode = item.WorkCenter,
                    //    DispatchDate = item.StartTime,
                    //    Status = WorkTaskStatus.New,
                    //    CreatedBy = currentUser
                    //});
                    //if (Processes.Where(p => boms.Select(b => b.WorkOrderProcessId).Contains(p.Id)).Any(p => p.Id == item.Id))
                    //    opTasksToProcess.Add(newOpTask);
                }
                else
                {
                    // [更新逻辑]: 
                    // 如果存在多个任务（如拆分），通常不同步数量，以免覆盖拆分后的逻辑
                    // 仅当只有一个任务且状态为 New 时，才自动同步工单总数量
                    if (opTasksToProcess.Count == 1)
                    {
                        var mainTask = opTasksToProcess.First();
                        if (mainTask.Quantity < item.Quantity)
                        {
                            opTasksToProcessUpdate.Add(new WorkOrderOperationTaskUpdateDto()
                            {
                                Id = mainTask.Id,
                                Quantity = item.Quantity,
                                Status = (mainTask.Status == WorkTaskStatus.Completed || mainTask.Status == WorkTaskStatus.Closed) ? WorkTaskStatus.InProgress : mainTask.Status,
                                UpdateBy = currentUser,
                                UpdatedOn = DateTime.Now
                            });
                            //await _operationTaskService.UpdateAsync(new WorkOrderOperationTaskUpdateDto() 
                            //{
                            //    Id = mainTask.Id,
                            //    Quantity = item.Quantity,
                            //    Status = (mainTask.Status == WorkTaskStatus.Completed || mainTask.Status == WorkTaskStatus.Closed) ? WorkTaskStatus.InProgress : mainTask.Status,
                            //    UpdateBy = currentUser,
                            //    UpdatedOn = DateTime.Now
                            //});
                        }
                    }
                }
            }

            if (opTasksToProcessCreate.Any())
            {
                var newOpTaskIds = await _operationTaskService.CreateBatchAsync(opTasksToProcessCreate);
                        
            }

            if (opTasksToProcessUpdate.Any())
            {
                await _operationTaskService.UpdateBatchAsync(opTasksToProcessUpdate);
            }

            var stepsToCreate = new List<WorkOrderStepTaskCreateDto>();
            var stepsToUpdate = new List<WorkOrderStepTaskUpdateDto>();

            // 用于 Phase 4 的上下文传递
            var stepContexts = new List<StepGenerationContext>();

            var allOpTasks = await _operationTaskService.GetListByProcessIdAsync(boms.Select(x=> x.WorkOrderProcessId).ToList());


            var allExistingSteps = await _stepTaskService.GetListByOperationIdAsync(allOpTasks.Select(x => x.Id).ToList());

            foreach (var opTask in allOpTasks)
            {
                // 场景 A: 电缆装配 (切线/拣配) -> 生成加工任务
                //await ProcessStepAndMaterialTasksAsync(opTask, boms.Where(b => b.WorkOrderProcessId == item.Id).ToList(), workcenterGroup, catCutting, catPicking, catKitting, currentUser);
                var process = Processes.FirstOrDefault(p => p.Id == opTask.WorkOrderProcessId);
                if (process == null) continue;

                var wcGroup = workcenterGroups.FirstOrDefault(g => g.WorkCenterCode == process.WorkCenter);

                var processBoms = boms.Where(b => b.WorkOrderProcessId == process.Id).ToList();
                var cableBoms = processBoms.Where(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed && x.ConsumeType == (int)ConsumeType.CableMaterial).ToList();
                var pickBoms = processBoms.Where(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed && x.ConsumeType == (int)ConsumeType.OrderBasedMaterial).ToList();

                var ctx = new StepGenerationContext
                {
                    OpTaskId = opTask.Id,
                    OpQuantity = opTask.Quantity ?? 0,
                    WcGroup = wcGroup,
                    CableBoms = cableBoms,
                    PickBoms = pickBoms,
                    ExistingSteps = allExistingSteps.Where(s => s.OperationTaskId == opTask.Id).ToList()
                };
                stepContexts.Add(ctx);

                // 准备 Cutting Step
                if (cableBoms.Any())
                {
                    PrepareStepTask(ctx, catCutting, currentUser, stepsToCreate, stepsToUpdate);
                }

                // 准备 Picking Step
                if (pickBoms.Any())
                {
                    PrepareStepTask(ctx, catPicking, currentUser, stepsToCreate, stepsToUpdate);
                }

                // 准备 Kitting Step
                if (cableBoms.Any() || pickBoms.Any())
                {
                    PrepareStepTask(ctx, catKitting, currentUser, stepsToCreate, stepsToUpdate);
                }
                else 
                {
                    //查找是否有kitting任务，无需合箱关闭合箱任务
                    var kittingtask = await _stepTaskService.GetListByOperationIdAsync(opTask.Id);
                    if (kittingtask.Any(x => x.TaskCategory == catKitting.CategoryCode)) 
                    {
                        stepsToUpdate.AddRange(kittingtask.Where(x => x.TaskCategory == catKitting.CategoryCode).Select(x => new WorkOrderStepTaskUpdateDto()
                        {
                            Id = x.Id,
                            Status = WorkTaskStatus.Closed,
                            UpdateBy = currentUser,
                            UpdatedOn = DateTime.Now
                        }));
                    }
                }

            }

            // 执行工步任务数据库操作
            if (stepsToCreate.Any()) await _stepTaskService.CreateBatchAsync(stepsToCreate);
            if (stepsToUpdate.Any()) await _stepTaskService.UpdateBatchAsync(stepsToUpdate);


            // ★ 重新加载所有工步任务（获取 ID 用于 Kitting 关联和 MaterialTask 挂载）
            var finalSteps = await _stepTaskService.GetListByOperationIdAsync(allOpTasks.Select(x => x.Id).ToList());
            // ==============================================================================
            // Phase 3.5: 更新 Kitting 的 PreStepIds (依赖 ID)
            // ==============================================================================
            var kittingUpdates = new List<WorkOrderStepTaskUpdateDto>();
            foreach (var ctx in stepContexts)
            {
                // 仅当需要 Kitting 时处理
                if (ctx.CableBoms.Any() || ctx.PickBoms.Any())
                {
                    var steps = finalSteps.Where(s => s.OperationTaskId == ctx.OpTaskId).ToList();
                    var kitStep = steps.FirstOrDefault(s => s.TaskCategory == catKitting.CategoryCode);
                    var cutStep = steps.FirstOrDefault(s => s.TaskCategory == catCutting.CategoryCode);
                    var pickStep = steps.FirstOrDefault(s => s.TaskCategory == catPicking.CategoryCode);

                    if (kitStep != null)
                    {
                        var preIds = new List<int>();
                        if (cutStep != null) preIds.Add(cutStep.Id);
                        if (pickStep != null) preIds.Add(pickStep.Id);

                        var newPreStr = string.Join(",", preIds);
                        if (kitStep.PreStepIds != newPreStr)
                        {
                            kittingUpdates.Add(new WorkOrderStepTaskUpdateDto { Id = kitStep.Id, PreStepIds = newPreStr });
                        }
                    }
                }
            }
            if (kittingUpdates.Any()) await _stepTaskService.UpdateBatchAsync(kittingUpdates);

            // ==============================================================================
            // Phase 4: 物料任务 (Material Task) - 批量处理
            // ==============================================================================
            // 收集所有需要生成 Cutting Material Task 的上下文
            var cuttingContexts = stepContexts.Where(c => c.CableBoms.Any()).ToList();

            if (cuttingContexts.Any())
            {
                // 1. 批量获取断线参数
                var allCableBoms = cuttingContexts.SelectMany(c => c.CableBoms).ToList();
                var superMatCodes = allCableBoms.Select(x => x.SuperMaterialCode).Distinct().ToList();
                var cutParams = await _cableCutParamService.GetListBySimiMaterialCodeFromSapAsync(superMatCodes);
                var cutParamMap = cutParams.ToDictionary(x => (x.SemiMaterialCode, x.PositionItem, x.CableMaterialCode.TrimStart('0')), x => x);
                var fallbackMap = cutParams.GroupBy(x => (x.SemiMaterialCode, x.PositionItem)).ToDictionary(g => g.Key, g => g.First());
                // 2. 批量获取现有的 Material Tasks
                // 找到所有 Cutting 类型的 StepId
                var cuttingStepIds = finalSteps
                    .Where(s => s.TaskCategory == catCutting.CategoryCode)
                    .Select(s => s.Id)
                    .ToList();

                var allMaterialTasks = await _materialTaskService.GetListByStepTaskIdAsync(cuttingStepIds);

                var matTasksToCreate = new List<WorkOrderMaterialTaskCreateDto>();
                var matTasksToUpdate = new List<WorkOrderMaterialTaskUpdateDto>();

                foreach (var ctx in cuttingContexts)
                {
                    // 找到当前工序下的 Cutting Step
                    var cutStep = finalSteps.FirstOrDefault(s => s.OperationTaskId == ctx.OpTaskId && s.TaskCategory == catCutting.CategoryCode);
                    if (cutStep == null) continue;

                    var existingTasks = allMaterialTasks.Where(t => t.StepTaskId == cutStep.Id).ToList();

                    foreach (var bom in ctx.CableBoms)
                    {
                        decimal targetValue = 0;
                        string extAttr = null;
                        decimal targetQuantity = 0;

                        if (cutParamMap.TryGetValue((bom.SuperMaterialCode, bom.BomItem, bom.MaterialCode), out var param))
                        {
                            targetValue = (decimal)param.BomLength / (decimal)param.CablePcs + (decimal)(param.UpTol * 0.8m);
                            targetQuantity = (decimal)param.CablePcs * (decimal)cutStep.Quantity;
                            extAttr = JsonConvert.SerializeObject(new
                            {
                                param.SemiMaterialCode,
                                param.PositionItem,
                                param.CableMaterialCode,
                                param.CablePcs,
                                param.BomLength,
                                param.UpTol,
                                param.DownTol,
                                param.CuttingLength,
                                param.CuttingTime,
                                param.AlphaFactor,
                                param.BetaFactor,
                                param.ReelCode,
                                param.Remark
                            });
                        }
                        else if (fallbackMap.TryGetValue((bom.SuperMaterialCode, bom.BomItem), out var backparam)) 
                        {
                            targetValue = (decimal)backparam.BomLength / (decimal)backparam.CablePcs + (decimal)(backparam.UpTol * 0.8m);
                            targetQuantity = (decimal)backparam.CablePcs * (decimal)cutStep.Quantity;
                            extAttr = JsonConvert.SerializeObject(new
                            {
                                backparam.SemiMaterialCode,
                                backparam.PositionItem,
                                backparam.CableMaterialCode,
                                backparam.CablePcs,
                                backparam.BomLength,
                                backparam.UpTol,
                                backparam.DownTol,
                                backparam.CuttingLength,
                                backparam.CuttingTime,
                                backparam.AlphaFactor,
                                backparam.BetaFactor,
                                backparam.ReelCode,
                                backparam.Remark
                            });
                        }

                        var task = existingTasks.FirstOrDefault(x => x.RefSourceId == bom.Id && x.RefReasonCode == TaskReasonCodes.BomRequirement);

                        if (task == null)
                        {
                            matTasksToCreate.Add(new WorkOrderMaterialTaskCreateDto
                            {
                                StepTaskId = cutStep.Id,
                                SourceBomId = bom.Id,
                                MaterialCode = bom.MaterialCode,
                                MaterialDesc = bom.MaterialDesc,
                                TargetQuantity = targetQuantity,
                                TargetValue = targetValue,
                                TargetUnit = "mm",
                                ExtAttributes = extAttr,
                                TaskSubType = TaskCategories.CableCut,
                                Priority = 0,
                                RefSourceId = bom.Id,
                                RefReasonCode = TaskReasonCodes.BomRequirement,
                                Status = WorkTaskStatus.New,
                                CreatedBy = currentUser,
                            });
                        }
                        else
                        {
                            if (task.Status != WorkTaskStatus.Completed )
                            {
                                if (task.TargetQuantity != targetQuantity || task.TargetValue != targetValue)
                                {
                                    matTasksToUpdate.Add(new WorkOrderMaterialTaskUpdateDto
                                    {
                                        Id = task.Id,
                                        Status = WorkTaskStatus.New,
                                        TargetQuantity = targetQuantity,
                                        TargetValue = targetValue,
                                        ExtAttributes = extAttr,
                                        UpdateBy = currentUser,
                                        UpdatedOn = DateTime.Now
                                    });
                                }
                            }
                        }
                    }

                    // 处理删除 (Cancel)
                    var activeBomIds = ctx.CableBoms.Select(x => x.Id).ToHashSet();
                    var tasksToCancel = existingTasks
                        .Where(x => x.RefReasonCode == TaskReasonCodes.BomRequirement && !activeBomIds.Contains(x.RefSourceId ?? 0))
                        .ToList();

                    foreach (var task in tasksToCancel)
                    {
                        if (task.Status != WorkTaskStatus.Cancelled)
                        {
                            matTasksToUpdate.Add(new WorkOrderMaterialTaskUpdateDto
                            {
                                Id = task.Id,
                                Status = WorkTaskStatus.Cancelled,
                                UpdateBy = currentUser,
                                UpdatedOn = DateTime.Now
                            });
                        }
                    }
                }

                // 执行物料任务数据库操作
                if (matTasksToCreate.Any()) await _materialTaskService.CreateBatchAsync(matTasksToCreate);
                if (matTasksToUpdate.Any()) await _materialTaskService.UpdateBatchAsync(matTasksToUpdate);
            }

        }

        public async Task<WorkOrderMaterialTaskDto> UpdateCuttingMaterialTaskAsync(int materialTaskId, string employeeId) 
        {
            var task = await _materialTaskService.GetByIdAsync(materialTaskId);
            if (task == null)
                throw new Exception("未查询到断线物料级任务信息！");

            var steptask = await _stepTaskService.GetByIdAsync((int)task.StepTaskId);
            if (steptask == null)
                throw new Exception("未查询到断线工步级任务信息！");

            var bom = await _bomService.GetByIdAsync((int)task.RefSourceId);
            if (bom == null)
                throw new Exception("未查询到BOM信息！");

            var cutParams = await _cableCutParamService.GetListBySimiMaterialCodeFromSapAsync(new List<string> { bom.SuperMaterialCode });
            if (cutParams != null && cutParams.Where(x => x.PositionItem == bom.BomItem && x.CableMaterialCode == bom.MaterialCode).Count() > 0) 
            {
                var cutParam = cutParams.Where(x => x.PositionItem == bom.BomItem && x.CableMaterialCode == bom.MaterialCode).FirstOrDefault();
                if (cutParam != null) 
                {
                    await _materialTaskService.UpdateAsync(new WorkOrderMaterialTaskUpdateDto()
                    {
                        Id = task.Id,
                        TargetValue = (decimal)cutParam.BomLength / (decimal)cutParam.CablePcs + (decimal)(cutParam.UpTol * 0.8m),
                        TargetQuantity = (decimal)cutParam.CablePcs * (decimal)steptask.Quantity,
                        ExtAttributes = JsonConvert.SerializeObject(new
                        {
                            cutParam.SemiMaterialCode,
                            cutParam.PositionItem,
                            cutParam.CableMaterialCode,
                            cutParam.CablePcs,
                            cutParam.BomLength,
                            cutParam.UpTol,
                            cutParam.DownTol,
                            cutParam.CuttingLength,
                            cutParam.CuttingTime,
                            cutParam.AlphaFactor,
                            cutParam.BetaFactor,
                            cutParam.ReelCode,
                            cutParam.Remark
                        }),
                        UpdateBy = employeeId,
                        UpdatedOn = DateTime.Now
                    });
                }
            }
            return await _materialTaskService.GetByIdAsync(materialTaskId);

        }

        private async Task ProcessStepAndMaterialTasksAsync(WorkOrderOperationTaskDto opTask, List<WorkOrderBomItemDto> boms,WorkCenterGroupDto? wcgroup, WorkTaskCategoryDto catCutting,WorkTaskCategoryDto catPicking,WorkTaskCategoryDto catKitting, string currentUser)
        {
            try
            {
                // 1. 分析物料
                var cableBoms = boms.Where(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed && x.ConsumeType == (int)ConsumeType.CableMaterial).ToList();
                var pickBoms = boms.Where(x => x.RequiredQuantity > 0 && (bool)x.MovementAllowed && x.ConsumeType == (int)ConsumeType.OrderBasedMaterial).ToList();

                // 2. 处理 Cutting 任务 (断线)
                var prestepIds = new List<int>();
                if (cableBoms.Any())
                {

                    var cutStep = await EnsureStepTaskAsync(opTask.Id, catCutting, wcgroup, opTask.Quantity ?? 0, currentUser);
                    prestepIds.Add(cutStep.Id);
                    // 处理具体的 MaterialTask
                    await SyncCuttingTasksAsync(cutStep, cableBoms, TaskCategories.CableCut, currentUser);
                }
                else
                {
                    // 如果 BOM 变更导致没有电缆了，需要取消对应的 StepTask 吗？视业务而定。
                    // 建议：如果任务未开始，可以 Cancel。
                }

                // 3. 处理 Picking 任务 (拣配)
                if (pickBoms.Any())
                {
                    var pickStep = await EnsureStepTaskAsync(opTask.Id, catPicking, wcgroup, opTask.Quantity ?? 0, currentUser);
                    prestepIds.Add(pickStep.Id);
                    // 拣配通常不需要 MaterialTask 细项，或者如果需要，逻辑同上
                }

                //// 4. 处理 Kitting 任务 (齐套)
                if (cableBoms.Any() || pickBoms.Any())
                {
                    var kitStep = await EnsureStepTaskAsync(opTask.Id, catKitting, wcgroup, opTask.Quantity ?? 0, currentUser);
                    await _stepTaskService.UpdateAsync(new WorkOrderStepTaskUpdateDto()
                    {
                        Id = kitStep.Id,
                        PreStepIds = string.Join(",", prestepIds)
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private async Task<WorkOrderStepTaskDto> EnsureStepTaskAsync(
            int opTaskId,
            WorkTaskCategoryDto category,
            WorkCenterGroupDto? wcgroup,
            decimal Quantity,
            string currentUser)
        {

            try
            {
                // 获取该 OperationTask 下已存在的 StepTasks
                var existingSteps = await _stepTaskService.GetListByOperationIdAsync(opTaskId);
                var stepCode = 0;
                if (existingSteps != null && existingSteps.Count() > 0)
                    stepCode = existingSteps.Max(s => int.Parse(s.StepCode));
                var step = existingSteps.FirstOrDefault(x => x.TaskCategory == category.CategoryCode);
                if (step == null)
                {
                    step = await _stepTaskService.CreateAsync(new WorkOrderStepTaskCreateDto
                    {
                        OperationTaskId = opTaskId,
                        WorkCenterGroupId = wcgroup?.Id,
                        WorkCenterGroupCode = wcgroup?.GroupCode,
                        Quantity = Quantity,
                        CompletedQuantity = 0,
                        StepCode = stepCode == 0 ? "10" : (stepCode + 10).ToString(),
                        Status = WorkTaskStatus.New,
                        TaskCategory = category.CategoryCode,
                        CreatedBy = currentUser,
                    });
                }
                else
                {
                    if (step.Quantity != Quantity)
                    {
                        if (await _stepTaskService.UpdateAsync(new WorkOrderStepTaskUpdateDto
                        {
                            Id = step.Id,
                            Quantity = Quantity,
                            UpdateBy = currentUser,
                            UpdatedOn = DateTime.Now,
                        }))
                        {
                            step = await _stepTaskService.GetByIdAsync(step.Id);
                        }
                    }
                }
                return step;
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        /// <summary>
        /// 核心逻辑：同步 CuttingTask (含检验项生成)
        /// </summary>
        private async Task SyncCuttingTasksAsync(WorkOrderStepTaskDto stepTask, List<WorkOrderBomItemDto> currentBoms, string subType, string currentUser)
        {
            try
            {// 1. 批量获取所有涉及物料的断线/检验参数
                var materialCodes = currentBoms.Select(x => x.SuperMaterialCode).Distinct().ToList();
                var cutParams = await _cableCutParamService.GetListBySimiMaterialCodeAsync(materialCodes);
                var cutParamMap = cutParams.ToDictionary(x => (x.SemiMaterialCode, x.PositionItem, x.CableMaterialCode), x => x);

                // 获取现有的物料任务
                var existingTasks = await _materialTaskService.GetListByStepTaskIdAsync(stepTask.Id);

                // A. 处理 新增 和 更新
                foreach (var bom in currentBoms)
                {
                    // [场景：新增 BOM] -> 创建任务
                    decimal targetValue = 0;
                    string extAttr = null;
                    decimal targetQuantity = 0;
                    // 读取工艺参数
                    if (cutParamMap.TryGetValue((bom.SuperMaterialCode, bom.BomItem, bom.MaterialCode), out var param))
                    {
                        targetValue = (decimal)param.BomLength / (decimal)param.CablePcs + (decimal)(param.UpTol * 0.8m);
                        targetQuantity = (decimal)param.CablePcs * (decimal)stepTask.Quantity;
                        extAttr = JsonConvert.SerializeObject(new
                        {
                            param.SemiMaterialCode,
                            param.PositionItem,
                            param.CableMaterialCode,
                            param.CablePcs,
                            param.BomLength,
                            param.UpTol,
                            param.DownTol,
                            param.CuttingLength,
                            param.CuttingTime,
                            param.AlphaFactor,
                            param.BetaFactor,
                            param.ReelCode,
                            param.Remark

                        });
                    }

                    // 使用 RefSourceId + RefReasonCode 组合键查找
                    var task = existingTasks.FirstOrDefault(x =>
                        x.RefSourceId == bom.Id &&
                        x.RefReasonCode == TaskReasonCodes.BomRequirement);

                    if (task == null)
                    {
                        task = await _materialTaskService.CreateAsync(new WorkOrderMaterialTaskCreateDto
                        {
                            StepTaskId = stepTask.Id,
                            SourceBomId = bom.Id,
                            MaterialCode = bom.MaterialCode,
                            MaterialDesc = bom.MaterialDesc,

                            // 任务量化指标
                            TargetQuantity = targetQuantity, // 根数
                            TargetValue = targetValue,             // 长度 (mm)
                            TargetUnit = "mm",

                            // 扩展属性与类型
                            ExtAttributes = extAttr,
                            TaskSubType = subType, // "CABLE_CUT"
                            Priority = 0,

                            // 核心追溯字段
                            RefSourceId = bom.Id,
                            RefReasonCode = TaskReasonCodes.BomRequirement,

                            Status = WorkTaskStatus.New,
                            CreatedBy = currentUser,
                        });

                        // ========================================================
                        // ★ 自动生成 "首检 (FAI)" 检验项
                        // ========================================================
                        //if (subType == TaskCategories.CableCut && param != null)
                        //{
                        //    var inspections = new List<Mes_TaskInspectionItem>();

                        //    // 1. 长度检验
                        //    inspections.Add(new Mes_TaskInspectionItem
                        //    {
                        //        MaterialTaskId = task.Id,
                        //        InspectionType = "FAI",
                        //        ParamName = "总长度",
                        //        TargetValue = param.CuttingLength,
                        //        UpperLimit = param.CuttingLength + (param.TolerancePlus ?? 1m),
                        //        LowerLimit = param.CuttingLength - (param.ToleranceMinus ?? 1m),
                        //        Unit = "mm",
                        //        CreatedOn = DateTime.Now
                        //    });

                        //    // 2. 剥皮检验 (如果有)
                        //    if (param.StripLengthFront > 0)
                        //    {
                        //        inspections.Add(new Mes_TaskInspectionItem
                        //        {
                        //            MaterialTaskId = task.Id,
                        //            InspectionType = "FAI",
                        //            ParamName = "前端剥皮",
                        //            TargetValue = param.StripLengthFront,
                        //            UpperLimit = param.StripLengthFront + 0.5m,
                        //            LowerLimit = param.StripLengthFront - 0.5m,
                        //            Unit = "mm",
                        //            CreatedOn = DateTime.Now
                        //        });
                        //    }

                        //    if (inspections.Any())
                        //    {
                        //        await _inspectionRepo.InsertRangeAsync(inspections);
                        //    }
                        //}
                    }
                    else
                    {
                        // [场景：修改 BOM 数量] -> 更新任务
                        // 如果任务已取消，重新激活
                        if (task.Status == WorkTaskStatus.Cancelled || task.Status == WorkTaskStatus.New)
                        {
                            await _materialTaskService.UpdateAsync(new WorkOrderMaterialTaskUpdateDto()
                            {
                                Id = task.Id,
                                Status = WorkTaskStatus.New,
                                TargetQuantity = targetQuantity,
                                TargetValue = targetValue,
                                ExtAttributes = extAttr,
                                UpdateBy = currentUser,
                                UpdatedOn = DateTime.Now
                            });
                        }
                        //// 如果任务还是 New，同步数量变更
                        //else if (task.Status == WorkTaskStatus.New && task.TargetQuantity != bom.RequiredQuantity)
                        //{
                        //    task.TargetQuantity = bom.RequiredQuantity;
                        //    task.UpdateBy = currentUser;
                        //    task.UpdatedOn = DateTime.Now;
                        //    await _matTaskRepo.UpdateAsync(task);
                        //}
                    }
                }

                // B. 处理 删除 (BOM 被软删除/硬删除)
                var activeBomIds = currentBoms.Select(x => x.Id).ToHashSet();

                // 找出属于 BOM 需求但不在当前 active 列表里的任务
                var tasksToCancel = existingTasks
                    .Where(x => x.RefReasonCode == TaskReasonCodes.BomRequirement && !activeBomIds.Contains(x.RefSourceId ?? 0))
                    .ToList();


                List<WorkOrderMaterialTaskUpdateDto> updateDtos = new List<WorkOrderMaterialTaskUpdateDto>();
                foreach (var task in tasksToCancel)
                {
                    if (task.Status != WorkTaskStatus.Cancelled)
                    {

                        updateDtos.Add(new WorkOrderMaterialTaskUpdateDto()
                        {
                            Id = task.Id,
                            Status = WorkTaskStatus.Cancelled,
                            UpdateBy = currentUser,
                            UpdatedOn = DateTime.Now

                        });
                    }
                }


                if (updateDtos.Count() > 0)
                    await _materialTaskService.UpdateBatchAsync(updateDtos);

            }
            catch (Exception)
            {

                throw;
            }
            
        }

        // 纯内存逻辑：准备 Step DTOs
        private void PrepareStepTask(
            StepGenerationContext ctx,
            WorkTaskCategoryDto category,
            string currentUser,
            List<WorkOrderStepTaskCreateDto> creates,
            List<WorkOrderStepTaskUpdateDto> updates)
        {
            var step = ctx.ExistingSteps.FirstOrDefault(x => x.TaskCategory == category.CategoryCode);

            // 计算最大 StepCode 逻辑
            // 注意：这在并发或多线程下可能需要更复杂的处理，这里简化为取当前列表最大
            // 在批量逻辑中，因为还没有写入DB，无法准确知道上一条的 StepCode。
            // 简单策略：如果不重要，可以使用固定规则；如果重要，需要预先排序。
            // 这里为了简化，假设 stepCode 只是显示顺序，不影响主键

            if (step == null)
            {
                var existingMaxCode = 0;
                if (ctx.ExistingSteps.Any())
                {
                    var codes = ctx.ExistingSteps.Select(s => int.TryParse(s.StepCode, out int c) ? c : 0);
                    if (codes.Any()) existingMaxCode = codes.Max();
                }

                // 检查当前 context 已经加入到 creates 列表中的 step，避免 code 冲突（如果一个工序创建多个 step）
                // 这是一个微小的 edge case，简单处理为 +10

                creates.Add(new WorkOrderStepTaskCreateDto
                {
                    OperationTaskId = ctx.OpTaskId,
                    WorkCenterGroupId = ctx.WcGroup.Id,
                    WorkCenterGroupCode = ctx.WcGroup.GroupCode,
                    Quantity = ctx.OpQuantity,
                    CompletedQuantity = 0,
                    StepCode = (existingMaxCode + 10).ToString(), // 简化的逻辑
                    Status = WorkTaskStatus.New,
                    TaskCategory = category.CategoryCode,
                    CreatedBy = currentUser,
                });
            }
            else
            {
                if (step.Quantity != ctx.OpQuantity)
                {
                    updates.Add(new WorkOrderStepTaskUpdateDto
                    {
                        Id = step.Id,
                        Quantity = ctx.OpQuantity,
                        UpdateBy = currentUser,
                        UpdatedOn = DateTime.Now,
                    });
                }
            }
        }

        // 辅助上下文类
        private class StepGenerationContext
        {
            public int OpTaskId { get; set; }
            public decimal OpQuantity { get; set; }
            public WorkCenterGroupDto WcGroup { get; set; }
            public List<WorkOrderStepTaskDto> ExistingSteps { get; set; }
            public List<WorkOrderBomItemDto> CableBoms { get; set; }
            public List<WorkOrderBomItemDto> PickBoms { get; set; }
        }
    }

}
