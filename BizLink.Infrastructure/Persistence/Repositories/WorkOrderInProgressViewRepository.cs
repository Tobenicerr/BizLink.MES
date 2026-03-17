using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.DbContext;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using Dm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderInProgressViewRepository : GenericRepository<V_WorkOrderInProgress>, IWorkOrderInProgressViewRepository
    {
        //private readonly IUnitOfWork _unitOfWork;

        //private readonly ISqlSugarClient _dbs;
        public WorkOrderInProgressViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            //_unitOfWork = unitOfWork;
            ////_dbs = _unitOfWork.GetDbClient("Default");
        }
        public async Task<List<V_WorkOrderInProgress>> GetByOrderNoAsync(string orderno)
        {
            return await _db.Queryable<V_WorkOrderInProgress>()
                            .Where(x => x.OrderNumber == orderno)
                            .ToListAsync();
        }

        public async Task<(List<V_WorkOrderInProgress>, int totalCount)> GetCableTaskPageListAsync(int pageIndex, int pageSize, int factoryId, string? keyword = null, List<string>? workOrderNo = null, DateTime? startTime = null, int? workcenterId = null, int? workStationId = null, string? status = null)
        {
            var query = _db.Queryable<WorkOrder, WorkOrderProcess, WorkOrderBomItem, WorkOrderMaterialTask, Factory>((o, p, b, t, f) =>
            new JoinQueryInfos(
                JoinType.Inner, o.Id == p.WorkOrderId,
                JoinType.Inner, p.Id == b.WorkOrderProcessId,
                JoinType.Left, b.Id == t.RefSourceId,
                JoinType.Inner, o.FactoryId == f.Id
                ))
               .Where((o, p, b, t, f) => t.RefReasonCode == TaskReasonCodes.BomRequirement && t.TaskSubType == TaskCategories.CableCut && b.RequiredQuantity > 0 && b.MovementAllowed == true && b.ConsumeType == (int)ConsumeType.CableMaterial)
               .Where((o, p, b, t, f) => o.FactoryId == factoryId)
               .WhereIF(workOrderNo != null && workOrderNo.Count() > 0, (o, p, b, t, f) => workOrderNo.Contains(o.OrderNumber))
               .WhereIF(startTime != null, (o, p, b, t, f) => (DateTime)p.StartTime == startTime)
               .WhereIF(workcenterId != null, (o, p, b, t, f) => t.ActualWorkCenterId == workcenterId)
               .WhereIF(workStationId != null, (o, p, b, t, f) => t.ActualWorkStationId == workStationId)
               .WhereIF(!string.IsNullOrWhiteSpace(keyword), (o, p, b, t, f) => o.OrderNumber.Contains(keyword) || t.MaterialCode.Contains(keyword) || p.WorkCenter.Contains(keyword))
               .WhereIF(!string.IsNullOrWhiteSpace(status), (o, p, b, t, f) => t.Status == status)
               .OrderBy((o, p, b, t, f) => p.StartTime).OrderBy((o, p, b, t, f) => o.OrderNumber)
               .Select((o, p, b, t, f) => new V_WorkOrderInProgress()
               {
                   FactoryId = o.Id,
                   FactoryCode = f.FactoryCode,
                   OrderId = o.Id,
                   OrderProcessId = p.Id,
                   ProfitCenter = o.ProfitCenter,
                   OrderNumber = o.OrderNumber,
                   Operation = p.Operation,
                   MaterialCode = o.MaterialCode,
                   MaterialDesc = o.MaterialDesc,
                   Quantity = o.Quantity,
                   ControlKey = p.ControlKey,
                   RequiredQuantity = b.RequiredQuantity,
                   TaskId = t.Id,
                   TaskQuantity = t.TargetQuantity,
                   TaskCompletedQty = t.CompletedQuantity,
                   CableItem = b.BomItem,
                   CableMaterial = t.MaterialCode,
                   CableMaterialDesc = t.MaterialDesc,
                   LeadingOrderMaterial = o.LeadingOrderMaterial,
                   CompletedQty = p.CompletedQuantity,
                   DispatchDate = o.DispatchDate,
                   StartTime = p.StartTime,
                   ActEndTime = p.ActEndTime,
                   ActStartTime = p.ActStartTime,
                   Status = t.TargetQuantity == 0 ? null : t.Status,
                   PlannerRemark = o.PlannerRemark,
                   WorkCenter = p.WorkCenter,
                   NextWorkCenter = p.NextWorkCenter
               });

            //var query = _db.Queryable<V_WorkOrderInProgress>()
            //               .Where(v => SqlFunc.Length(v.CableMaterial) > 0)
            //               .WhereIF(workOrderNo != null && workOrderNo.Count() > 0, v => workOrderNo.Contains(v.OrderNumber))
            //               .WhereIF(startTime != null, v => (DateTime)v.StartTime == startTime)
            //               .WhereIF(workcenterId != null, v => v.WorkCenterId == workcenterId)
            //               .WhereIF(workStationId != null, v => v.WorkStationId == workStationId)
            //               .WhereIF(!string.IsNullOrWhiteSpace(keyword), v => v.OrderNumber.Contains(keyword) || v.CableMaterial.Contains(keyword) || v.WorkCenter.Contains(keyword))
            //               .WhereIF(!string.IsNullOrWhiteSpace(status), v => SqlFunc.IIF(v.TaskCompletedQty == null,"1",SqlFunc.IIF(v.TaskQuantity == v.TaskCompletedQty,"4","2")) == status)
            //               .OrderBy(v => v.StartTime).OrderBy(v=> v.OrderNumber);

            var totalCount = await query.CountAsync();
            var list = await query.ToPageListAsync(pageIndex, pageSize);

            return (list, totalCount);

        }

        public async Task<List<V_WorkOrderInProgress>> GetListByWorkCenterGroupAsync(int factoryid, int workcentergroupid, DateTime datetimeStart, DateTime datetimeEnd)
        {
            return await _db.Queryable<V_WorkOrderInProgress,WorkCenterGroupMember>((v,g) => v.WorkCenterId == g.WorkCenterId).Where((v, g) => v.FactoryId == factoryid && v.DispatchDate >= datetimeStart && v.DispatchDate <= datetimeEnd && g.GroupId == workcentergroupid).ToListAsync();
        }

        public async Task<List<V_WorkOrderInProgress>> GetListAsync(List<string> workCenters, DateTime? startTime, DateTime? endTime = null)
        {
            var result = await _db.Queryable<V_WorkOrderInProgress>()
                            .WhereIF(endTime == null, x => x.StartTime == startTime)
                            .WhereIF(endTime != null, x => x.StartTime >= startTime && x.StartTime <= endTime)
                            .WhereIF(workCenters != null && workCenters.Count()>0, x => workCenters.Contains(x.WorkCenter))
                            .OrderBy(v => v.CableMaterial).ToListAsync();

            return result;

        }

        public async Task<List<V_WorkOrderInProgress>> GetListByProcessIdAsync(List<int> processIds)
        {
            return await _db.Queryable<V_WorkOrderInProgress>()
                      .Where(v => processIds.Contains(v.OrderProcessId))
                      .ToListAsync();
        }

        public async Task<List<V_WorkOrderInProgress>> GetOngoingCableTaskListByDateAsync(int factoryid, DateTime datetime)
        {

            return await _db.Queryable<WorkOrder, WorkOrderProcess, WorkOrderBomItem, WorkOrderMaterialTask, Factory>((o, p, b, t, f) => o.Id == p.WorkOrderId && p.Id == b.WorkOrderProcessId && b.Id == t.RefSourceId && o.FactoryId == f.Id)
                .Where((o, p, b, t, f) => t.RefReasonCode == TaskReasonCodes.BomRequirement && t.TaskSubType == TaskCategories.CableCut)
                .Where((o, p, b, t, f) => o.FactoryId == factoryid && p.StartTime <= datetime.Date && p.StartTime > datetime.AddDays(-7).Date && SqlFunc.ToInt32(t.Status) < int.Parse(WorkTaskStatus.Completed))
                .Select((o, p, b, t, f) => new V_WorkOrderInProgress()
                {
                    FactoryId = o.Id,
                    FactoryCode = f.FactoryCode,
                    OrderId = o.Id,
                    OrderProcessId = p.Id,
                    ProfitCenter = o.ProfitCenter,
                    OrderNumber = o.OrderNumber,
                    Operation = p.Operation,
                    MaterialCode = o.MaterialCode,
                    MaterialDesc = o.MaterialDesc,
                    Quantity = o.Quantity,
                    ControlKey = p.ControlKey,
                    RequiredQuantity = b.RequiredQuantity,
                    TaskId = t.Id,
                    TaskQuantity = t.TargetQuantity,
                    TaskCompletedQty = t.CompletedQuantity,
                    CableItem = b.BomItem,
                    CableMaterial = t.MaterialCode,
                    CableMaterialDesc = t.MaterialDesc,
                    LeadingOrderMaterial = o.LeadingOrderMaterial,
                    CompletedQty = p.CompletedQuantity,
                    DispatchDate = o.DispatchDate,
                    StartTime = p.StartTime,
                    ActEndTime = p.ActEndTime,
                    ActStartTime = p.ActStartTime,
                    Status = p.Status,
                    PlannerRemark = o.PlannerRemark,
                    WorkCenter = p.WorkCenter,
                    NextWorkCenter = p.NextWorkCenter
                    
                }).ToListAsync();
            //return _db.Queryable<V_WorkOrderInProgress>()
            //          .Where(v => v.FactoryId == factoryid && SqlFunc.IsNull(v.PrevProcessId,0) == 0 && v.StartTime <= datetime.Date && v.StartTime > datetime.AddDays(-7).Date && v.Status != "4")
            //          .ToListAsync();
        }

        public async Task<List<V_WorkOrderInProgress>> GetOverdueCableTaskListByDateAsync(int factoryid, string? keyword)
        {
            return await _db.Queryable<WorkOrder, WorkOrderProcess, WorkOrderBomItem, WorkOrderMaterialTask, Factory>((o, p, b, t, f) => 
            new JoinQueryInfos(
                JoinType.Inner, o.Id == p.WorkOrderId,
                JoinType.Inner, p.Id == b.WorkOrderProcessId,
                JoinType.Left, b.Id == t.RefSourceId,
                JoinType.Inner, o.FactoryId == f.Id
                ))
               .Where((o, p, b, t, f) => t.RefReasonCode == TaskReasonCodes.BomRequirement && t.TaskSubType == TaskCategories.CableCut && b.RequiredQuantity > 0 && b.MovementAllowed == true && b.ConsumeType == (int)ConsumeType.CableMaterial)
               .Where((o, p, b, t, f) => o.FactoryId == factoryid && SqlFunc.ToInt32(SqlFunc.IsNull<string>(t.Status,"0")) < int.Parse(WorkTaskStatus.Completed) && p.StartTime < DateTime.Now.Date && p.StartTime > DateTime.Parse("2026-02-01"))
               .WhereIF(!string.IsNullOrWhiteSpace(keyword), (o, p, b, t, f) => o.OrderNumber.Contains(keyword) || t.MaterialCode.Contains(keyword) || p.WorkCenter.Contains(keyword))
               .Select((o, p, b, t, f) => new V_WorkOrderInProgress()
               {
                   FactoryId = o.Id,
                   FactoryCode = f.FactoryCode,
                   OrderId = o.Id,
                   OrderProcessId = p.Id,
                   ProfitCenter = o.ProfitCenter,
                   OrderNumber = o.OrderNumber,
                   Operation = p.Operation,
                   MaterialCode = o.MaterialCode,
                   MaterialDesc = o.MaterialDesc,
                   Quantity = o.Quantity,
                   ControlKey = p.ControlKey,
                   RequiredQuantity = b.RequiredQuantity,
                   TaskId = t.Id,
                   TaskQuantity = t.TargetQuantity,
                   TaskCompletedQty = t.CompletedQuantity,
                   CableItem = b.BomItem,
                   CableMaterial = t.MaterialCode,
                   CableMaterialDesc = t.MaterialDesc,
                   LeadingOrderMaterial = o.LeadingOrderMaterial,
                   CompletedQty = p.CompletedQuantity,
                   DispatchDate = o.DispatchDate,
                   StartTime = p.StartTime,
                   ActEndTime = p.ActEndTime,
                   ActStartTime = p.ActStartTime,
                   Status = t.TargetQuantity == 0 ? null:t.Status,
                   PlannerRemark = o.PlannerRemark,
                   WorkCenter = p.WorkCenter,
                   NextWorkCenter = p.NextWorkCenter
               }).ToListAsync();

            //return await _db.Queryable<V_WorkOrderInProgress>()
            //          .Where(v => v.FactoryId == factoryid && SqlFunc.IsNull(v.PrevProcessId, 0) == 0 && v.StartTime < DateTime.Now.Date  && v.Status != "4")
            //          .WhereIF(!string.IsNullOrWhiteSpace(keyword), v => v.OrderNumber.Contains(keyword) || v.CableMaterial.Contains(keyword) || v.WorkCenter.Contains(keyword))
            //          .ToListAsync();
        }
    }
}
