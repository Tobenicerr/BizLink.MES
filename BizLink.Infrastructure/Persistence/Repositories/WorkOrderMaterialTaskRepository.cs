using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Domain.Repositories.Common;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderMaterialTaskRepository : GenericRepository<WorkOrderMaterialTask>, IWorkOrderMaterialTaskRepository
    {
        public WorkOrderMaterialTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<WorkOrderMaterialTask> GetCuttingViewByBomIdAsync(int bomId)
        {
            var query1 = _db.Queryable<WorkOrderBomItem>()
                         .InnerJoin<WorkOrderTask>((t1, t2) =>
                             t1.WorkOrderId == t2.OrderId &&
                             t1.WorkOrderProcessId == t2.OrderProcessId &&
                             t1.BomItem == t2.MaterialItem &&
                             t1.MaterialCode == t2.MaterialCode)
                         .Where((t1, t2) => t1.Id == bomId)
                         .Select((t1, t2) => new WorkOrderMaterialTask
                         {
                             RefSourceId = t1.Id,
                             MaterialCode = t1.MaterialCode,
                             MaterialDesc = t1.MaterialDesc,
                             TargetQuantity = t2.Quantity,            // 对应 t2.Quantity
                             CompletedQuantity = t2.CompletedQty     // 对应 t2.CompletedQty
                         });

            // 2. 构造第二个子查询 (INNER JOIN Mes_WorkOrderMaterialTask)
            var query2 = _db.Queryable<WorkOrderBomItem>()
                .InnerJoin<WorkOrderMaterialTask>((t1, t2) =>
                    t1.Id == t2.RefSourceId &&
                    t2.TaskSubType == TaskCategories.CableCut &&
                    t2.RefReasonCode == TaskReasonCodes.BomRequirement)
                         .Where((t1, t2) => t1.Id == bomId)
                .Select((t1, t2) => new WorkOrderMaterialTask
                {
                    RefSourceId = t1.Id,
                    MaterialCode = t1.MaterialCode,
                    MaterialDesc = t1.MaterialDesc,
                    TargetQuantity = t2.TargetQuantity,      // 对应 t2.TargetQuantity
                    CompletedQuantity = t2.CompletedQuantity // 对应 t2.CompletedQuantity
                });

            // 3. 将两个子查询 UNION，然后进行 GROUP BY 和 MAX 聚合
            // 注意：您的原生 SQL 用的是 UNION (去重)。如果在业务中不需要严格去重，建议改成 UnionAll() 性能更好
            return await _db.Union(query1, query2)
                .GroupBy(w1 => new
                {
                    w1.RefSourceId,
                    w1.MaterialCode,
                    w1.MaterialDesc
                })
                .Select(w1 => new WorkOrderMaterialTask
                {
                    // 分组字段直接赋值
                    RefSourceId = w1.RefSourceId,
                    MaterialCode = w1.MaterialCode,
                    MaterialDesc = w1.MaterialDesc,

                    // 聚合字段使用 SqlFunc.AggregateMax
                    TargetQuantity = SqlFunc.AggregateMax(w1.TargetQuantity),
                    CompletedQuantity = SqlFunc.AggregateMax(w1.CompletedQuantity)
                }).FirstAsync();
        }

        public async Task<List<WorkOrderMaterialTask>> GetCuttingViewByProcessIdAsync(int processId)
        {
            var query1 = _db.Queryable<WorkOrderBomItem>()
             .InnerJoin<WorkOrderTask>((t1, t2) =>
                 t1.WorkOrderId == t2.OrderId &&
                 t1.WorkOrderProcessId == t2.OrderProcessId &&
                 t1.BomItem == t2.MaterialItem &&
                 t1.MaterialCode == t2.MaterialCode)
             .Where((t1, t2) => t1.WorkOrderProcessId == processId)
             .Select((t1, t2) => new WorkOrderMaterialTask
             {
                 RefSourceId = t1.Id,
                 MaterialCode = t1.MaterialCode,
                 MaterialDesc = t1.MaterialDesc,
                 TargetQuantity = t2.Quantity,            // 对应 t2.Quantity
                 CompletedQuantity = t2.CompletedQty     // 对应 t2.CompletedQty
             });

            // 2. 构造第二个子查询 (INNER JOIN Mes_WorkOrderMaterialTask)
            var query2 = _db.Queryable<WorkOrderBomItem>()
                .InnerJoin<WorkOrderMaterialTask>((t1, t2) =>
                    t1.Id == t2.RefSourceId &&
                    t2.TaskSubType == TaskCategories.CableCut &&
                    t2.RefReasonCode == TaskReasonCodes.BomRequirement)
                .Where((t1, t2) => t1.WorkOrderProcessId == processId)
                .Select((t1, t2) => new WorkOrderMaterialTask
                {
                    RefSourceId = t1.Id,
                    MaterialCode = t1.MaterialCode,
                    MaterialDesc = t1.MaterialDesc,
                    TargetQuantity = t2.TargetQuantity,      // 对应 t2.TargetQuantity
                    CompletedQuantity = t2.CompletedQuantity // 对应 t2.CompletedQuantity
                });

            // 3. 将两个子查询 UNION，然后进行 GROUP BY 和 MAX 聚合
            // 注意：您的原生 SQL 用的是 UNION (去重)。如果在业务中不需要严格去重，建议改成 UnionAll() 性能更好
            return await _db.Union(query1, query2)
                .GroupBy(w1 => new
                {
                    w1.RefSourceId,
                    w1.MaterialCode,
                    w1.MaterialDesc
                })
                .Select(w1 => new WorkOrderMaterialTask
                {
                    // 分组字段直接赋值
                    RefSourceId = w1.RefSourceId,
                    MaterialCode = w1.MaterialCode,
                    MaterialDesc = w1.MaterialDesc,

                    // 聚合字段使用 SqlFunc.AggregateMax
                    TargetQuantity = SqlFunc.AggregateMax(w1.TargetQuantity),
                    CompletedQuantity = SqlFunc.AggregateMax(w1.CompletedQuantity)
                })
                .ToListAsync();
        }

        public async Task<List<WorkOrderMaterialTask>> GetCuttingViewByProcessIdAsync(List<int> processIds)
        {
            //var query1 = _db.Queryable<WorkOrderBomItem>()
            // .InnerJoin<WorkOrderTask>((t1, t2) =>
            //     t1.WorkOrderId == t2.OrderId &&
            //     t1.WorkOrderProcessId == t2.OrderProcessId &&
            //     t1.BomItem == t2.MaterialItem &&
            //     t1.MaterialCode == t2.MaterialCode)
            // .Where((t1, t2) => processIds.Contains(t1.WorkOrderProcessId))
            // .Select((t1, t2) => new WorkOrderMaterialTask
            // {
            //     RefSourceId = t1.Id,
            //     MaterialCode = t1.MaterialCode,
            //     MaterialDesc = t1.MaterialDesc,
            //     TargetQuantity = t2.Quantity,            // 对应 t2.Quantity
            //     CompletedQuantity = t2.CompletedQty     // 对应 t2.CompletedQty
            // });

            //// 2. 构造第二个子查询 (INNER JOIN Mes_WorkOrderMaterialTask)
            //var query2 = _db.Queryable<WorkOrderBomItem>()
            //    .InnerJoin<WorkOrderMaterialTask>((t1, t2) =>
            //        t1.Id == t2.RefSourceId &&
            //        t2.TaskSubType == TaskCategories.CableCut &&
            //        t2.RefReasonCode == TaskReasonCodes.BomRequirement)
            //    .Where((t1, t2) => processIds.Contains(t1.WorkOrderProcessId))
            //    .Select((t1, t2) => new WorkOrderMaterialTask
            //    {
            //        RefSourceId = t1.Id,
            //        MaterialCode = t1.MaterialCode,
            //        MaterialDesc = t1.MaterialDesc,
            //        TargetQuantity = t2.TargetQuantity,      // 对应 t2.TargetQuantity
            //        CompletedQuantity = t2.CompletedQuantity // 对应 t2.CompletedQuantity
            //    });

            //// 3. 将两个子查询 UNION，然后进行 GROUP BY 和 MAX 聚合
            //// 注意：您的原生 SQL 用的是 UNION (去重)。如果在业务中不需要严格去重，建议改成 UnionAll() 性能更好
            //return await _db.Union(query1, query2)
            //    .GroupBy(w1 => new
            //    {
            //        w1.RefSourceId,
            //        w1.MaterialCode,
            //        w1.MaterialDesc
            //    })
            //    .Select(w1 => new WorkOrderMaterialTask
            //    {
            //        // 分组字段直接赋值
            //        RefSourceId = w1.RefSourceId,
            //        MaterialCode = w1.MaterialCode,
            //        MaterialDesc = w1.MaterialDesc,

            //        // 聚合字段使用 SqlFunc.AggregateMax
            //        TargetQuantity = SqlFunc.AggregateMax(w1.TargetQuantity),
            //        CompletedQuantity = SqlFunc.AggregateMax(w1.CompletedQuantity)
            //    })
            //    .ToListAsync();

            return await _db.Queryable<WorkOrderMaterialTask, WorkOrderBomItem>((t1, t2) =>
                    t2.Id == t1.RefSourceId &&
                    t1.TaskSubType == TaskCategories.CableCut &&
                    t1.RefReasonCode == TaskReasonCodes.BomRequirement)
                .Where((t1, t2) => processIds.Contains(t2.WorkOrderProcessId))
                .ToListAsync();

        }


        public async Task<List<WorkOrderMaterialTask>> GetListByBomIdAsync(List<int> bomIds, string? TaskSubType = null, string? refResonCode = null)
        {
            return await _db.Queryable<WorkOrderMaterialTask>().Where(w => bomIds.Contains((int)w.RefSourceId))
                .WhereIF(!string.IsNullOrEmpty(TaskSubType), w => w.TaskSubType == TaskSubType)
                .WhereIF(!string.IsNullOrEmpty(refResonCode), w => w.RefReasonCode == refResonCode)
                .ToListAsync();

        }

        public async Task<List<WorkOrderMaterialTask>> GetListByProcessIdAsync(int processId, string? TaskCategory = null, string? TaskSubType = null)
        {
            return await _db.Queryable<WorkOrderMaterialTask, WorkOrderStepTask, WorkOrderOperationTask>((m,s,o) => m.StepTaskId == s.Id && s.OperationTaskId == o.Id)
                .Where((m, s, o) => o.WorkOrderProcessId == processId)
                .WhereIF(!string.IsNullOrEmpty(TaskCategory), (m, s, o) => s.TaskCategory == TaskCategory)
                .WhereIF(!string.IsNullOrEmpty(TaskSubType), (m, s, o) => m.TaskSubType == TaskSubType)
                .ToListAsync();
        }

        public async Task<List<WorkOrderMaterialTask>> GetListByStepTaskIdAsync(int stepId, string? TaskSubType = null)
        {
            return await _db.Queryable<WorkOrderMaterialTask>().Where(t => t.StepTaskId == stepId)
               .WhereIF(!string.IsNullOrEmpty(TaskSubType), t => t.TaskSubType == TaskSubType).ToListAsync();
        }

        public async Task<List<WorkOrderMaterialTask>> GetListByStepTaskIdAsync(List<int> stepIds, string? TaskSubType = null)
        {
            return await _db.Queryable<WorkOrderMaterialTask>().Where(t => stepIds.Contains((int)t.StepTaskId))
               .WhereIF(!string.IsNullOrEmpty(TaskSubType), t => t.TaskSubType == TaskSubType).ToListAsync();
        }
    }
}
