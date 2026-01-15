using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Common;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderRepository :GenericRepository<WorkOrder>, IWorkOrderRepository
    {
        public WorkOrderRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<int> CountByOrderNosNoLockAsync(List<string> ordernos)
        {
            return await _db.Queryable<WorkOrder>().With(SqlWith.NoLock).Where(x => ordernos.Contains(x.OrderNumber) && StatusQueryRules.ActiveWorkOrderStatus.Contains(x.Status)).CountAsync();
        }

        public async Task<List<int>> AddBatchAsync(List<WorkOrder> createDtos)
        {
            return await _db.Insertable(createDtos).ExecuteReturnPkListAsync<int>();
        }

        public async Task<WorkOrder> GetByOrderNoAsync(string orderno)
        {
            return await _db.Queryable<WorkOrder>().Where(x => x.OrderNumber == orderno && StatusQueryRules.ActiveWorkOrderStatus.Contains(x.Status)).FirstAsync();
        }

        public async Task<List<WorkOrder>> GetByOrderNoAsync(List<string> ordernos)
        {
            return await _db.Queryable<WorkOrder>().Where(x => ordernos.Contains(x.OrderNumber) && StatusQueryRules.ActiveWorkOrderStatus.Contains(x.Status)).ToListAsync();
        }

        public async Task<(List<WorkOrder>, List<WorkOrderProcess>, List<WorkOrderTask>)> GetCableTaskConfirmListAsync(List<string>? orders, DateTime? starttime, int? workcenterid, int? workstationid)
        {
            var orderIds = await _db.Queryable<WorkOrder, WorkOrderProcess, WorkOrderTask, WorkCenter, Factory>((w, p, t, wc, f) => new JoinQueryInfos(
                JoinType.Inner, w.Id == p.WorkOrderId,
                JoinType.Left, p.Id == t.OrderProcessId && w.Id == t.OrderId,
                JoinType.Inner, p.WorkCenter == wc.WorkCenterCode && wc.IsGroup == false,
                JoinType.Inner, w.FactoryId == f.Id && f.IsActive == true
            )).Where((w, p, t, wc, f) => SqlFunc.Subqueryable<WorkOrderBomItem>().Where(oi => oi.WorkOrderId == w.Id && oi.ConsumeType == 0 && oi.MovementAllowed == true && oi.RequiredQuantity > 0).Any() && w.FactoryId == 2 && StatusQueryRules.ActiveWorkOrderStatus.Contains(w.Status))
                .WhereIF(orders != null && orders.Count() > 0, (w, p, t, wc, f) => orders.Contains(w.OrderNumber))
            .WhereIF(starttime != null, (w, p, t, wc, f) => w.ScheduledStartDate == starttime)
            .WhereIF(workcenterid != null, (w, p, t, wc, f) => wc.Id == workcenterid)
            .WhereIF(workstationid != null, (w, p, t, wc, f) => t.WorkStationId == workstationid).Select(w => w.Id).Distinct().ToListAsync();

            var workorders = await _db.Queryable<WorkOrder, Factory>((w, f) => w.FactoryId == f.Id).Where(w => orderIds.Contains(w.Id)).Select((w, f) => new WorkOrder()
            {
                Id = w.Id,
                OrderNumber = w.OrderNumber,
                FactoryId = w.FactoryId,
                FactoryCode = f.FactoryCode,
                ScheduledStartDate = w.ScheduledStartDate,
                ScheduledFinishDate = w.ScheduledFinishDate,
                ProfitCenter = w.ProfitCenter,
                MaterialCode = w.MaterialCode,
                MaterialDesc = w.MaterialDesc,
                LeadingOrder = w.LeadingOrder,
                LeadingOrderMaterial = w.LeadingOrderMaterial,
                DispatchDate = w.DispatchDate,
                Quantity = w.Quantity,
                Status = w.Status,
                PlannerRemark = w.PlannerRemark,
                SuperiorOrder = w.SuperiorOrder,
                LabelCount = w.LabelCount
            }).ToListAsync();
            //var workorders = await _db.Queryable<WorkOrder, WorkOrderProcess, WorkOrderTask,WorkCenter,Factory>((w, p, t,wc,f) => new JoinQueryInfos(
            //    JoinType.Inner, w.Id == p.WorkOrderId,
            //    JoinType.Left, p.Id == t.OrderProcessId && w.Id == t.OrderId,
            //    JoinType.Inner, p.WorkCenter == wc.WorkCenterCode && wc.IsGroup == false,
            //    JoinType.Inner,w.FactoryId == f.Id && f.IsActive == true
            //)).Where((w, p, t, wc, f) => SqlFunc.Subqueryable<WorkOrderBomItem>().Where(oi => oi.WorkOrderId == w.Id && oi.ConsumeType == 0).Any() && w.FactoryId == 2)
            //    .WhereIF(orders != null && orders.Count()> 0 , (w, p, t, wc, f) => orders.Contains(w.OrderNumber))
            //.WhereIF(starttime != null, (w, p, t, wc, f) => w.ScheduledStartDate == starttime)
            //.WhereIF(workcenterid != null, (w, p, t, wc, f) => wc.Id == workcenterid)
            //.WhereIF(workstationid != null, (w, p, t, wc, f) => t.WorkStationId == workstationid).Select((w, p, t, wc, f) => new WorkOrder() 
            //{
            //    Id = w.Id,
            //    OrderNumber = w.OrderNumber,
            //    FactoryId = w.FactoryId,
            //    FactoryCode = f.FactoryCode,
            //    ScheduledStartDate = w.ScheduledStartDate,
            //    ScheduledFinishDate = w.ScheduledFinishDate,
            //    ProfitCenter = w.ProfitCenter,
            //    MaterialCode = w.MaterialCode,
            //    MaterialDesc = w.MaterialDesc,
            //    LeadingOrder = w.LeadingOrder,
            //    LeadingOrderMaterial = w.LeadingOrderMaterial,
            //    DispatchDate = w.DispatchDate,
            //    Quantity = w.Quantity,
            //    Status = w.Status,
            //    PlannerRemark = w.PlannerRemark,
            //    SuperiorOrder = w.SuperiorOrder,
            //    LabelCount = w.LabelCount,

            //}).ToListAsync();
            var processes = await _db.Queryable<WorkOrderProcess>().GroupBy( p=> p.WorkOrderId).Select(p => new { WorkOrderId = p.WorkOrderId,Id = SqlFunc.AggregateMin(p.Id) }).MergeTable().LeftJoin<WorkOrderProcess>((a, b) => a.Id == b.Id)
                .Where((a, b) => workorders.Select(w => w.Id).ToList().Contains((int)a.WorkOrderId)).Select((a, b) => b).ToListAsync();
            var tasks = await _db.Queryable<WorkOrderTask, WorkStation>((t, w) => t.WorkStationId == w.Id).Where((t, w) => workorders.Select(w => w.Id).ToList().Contains((int)t.OrderId)).Select((t, w) => new WorkOrderTask()
            {
                Id = t.Id,
                OrderId = t.OrderId,
                OrderProcessId = t.OrderProcessId,
                TaskNumber = t.TaskNumber,
                Quantity = t.Quantity,
                Status = t.Status,
                CableLength = t.CableLength,
                CableLengthUsl = t.CableLengthUsl,
                CableLengthDsl = t.CableLengthDsl,
                CompletedQty = t.CompletedQty,
                WorkCenter = t.WorkCenter,
                WorkStationId = t.WorkStationId,
                WorkStationCode = w.WorkStationCode,
                MaterialItem = t.MaterialItem,
                MaterialCode = t.MaterialCode,
                MaterialDesc = t.MaterialDesc,
                ProfitCenter = t.ProfitCenter,
                NextWorkCenter = t.NextWorkCenter,
                Operation = t.Operation
            }).ToListAsync();
            return (workorders.Distinct().ToList(), processes.Distinct().ToList(), tasks.Distinct().ToList());
        }

        public async Task<List<WorkOrder>> GetListByBomMaterialAsync(string bommaterialcode)
        {
            return await _db.Queryable<WorkOrder,WorkOrderBomItem>((o,b) => o.Id == b.WorkOrderId)
                .With(SqlWith.NoLock)
                .Where((o,b) => b.MaterialCode == bommaterialcode && StatusQueryRules.ActiveWorkOrderStatus.Contains(o.Status)).OrderByDescending(o => o.ScheduledStartDate).Distinct().ToListAsync();
        }

        public async Task<List<WorkOrder>> GetListByDispatchDateAsync(DateTime? dispatchdate, DateTime? startdate, List<string>? ordernos, int factoryid)
        {
            return await _db.Queryable<WorkOrder,Factory>((w,f)=> w.FactoryId == f.Id)
            .Where(w => w.FactoryId == factoryid && StatusQueryRules.ActiveWorkOrderStatus.Contains(w.Status))
            .WhereIF(dispatchdate != null,w => w.DispatchDate == dispatchdate)
            .WhereIF(startdate != null,w => w.ScheduledStartDate == startdate)
            .WhereIF(ordernos != null && ordernos.Count > 0, w => ordernos.Contains(w.OrderNumber))
            .Select((w, f) => new WorkOrder 
            {
                Id = w.Id,
                OrderNumber = w.OrderNumber,
                FactoryId = w.FactoryId,
                FactoryCode = f.FactoryCode,
                DispatchDate = w.DispatchDate,
                MaterialCode = w.MaterialCode,
                MaterialDesc = w.MaterialDesc,
                Quantity = w.Quantity,
                Status = w.Status,
                PlannerRemark = w.PlannerRemark,
                SuperiorOrder = w.SuperiorOrder,
                LeadingOrder = w.LeadingOrder,
                LeadingOrderMaterial = w.LeadingOrderMaterial,
                ScheduledStartDate = w.ScheduledStartDate,
                ProfitCenter = w.ProfitCenter,
                LabelCount = w.LabelCount,
                CableCount = SqlFunc.Subqueryable<WorkOrderBomItem>().Where(oi => oi.WorkOrderId == w.Id && oi.ConsumeType == 0).Count(),
                ProcessCardPrintCount = SqlFunc.Subqueryable<WorkOrderProcess>().Where(op => op.WorkOrderId == w.Id).Sum(op => op.ProcessCardPrintCount)

            }).ToListAsync();
        }

        public async Task<List<WorkOrder>> GetListByDispatchDateEndAsync(int factoryid, DateTime startdate)
        {
            return await _db.Queryable<WorkOrder>().Where(w => w.FactoryId == factoryid && w.ScheduledStartDate <= startdate && w.ScheduledStartDate >= startdate.AddDays(-30).Date && StatusQueryRules.ActiveWorkOrderStatus.Contains(w.Status)).ToListAsync();
        }

        public async Task<List<WorkOrder>> GetByIdAsync(List<int> ids)
        {
            return await _db.Queryable<WorkOrder>().Where(x => ids.Contains(x.Id) && StatusQueryRules.ActiveWorkOrderStatus.Contains(x.Status)).ToListAsync();
        }

        public Task<bool> UpdateBatchAsync(List<WorkOrder> updateDtos)
        {
           return _db.Updateable(updateDtos).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandHasChangeAsync();
        }
    }
}
