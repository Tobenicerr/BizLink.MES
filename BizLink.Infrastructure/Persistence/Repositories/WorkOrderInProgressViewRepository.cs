using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.DbContext;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
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

        public async Task<(List<V_WorkOrderInProgress>, int totalCount)> GetCableTaskPageListAsync(int pageIndex, int pageSize, string? keyword = null, List<string>? workOrderNo = null, DateTime? startTime = null, int? workcenterId = null, int? workStationId = null, string? status = null)
        {
            var query = _db.Queryable<V_WorkOrderInProgress>()
                           .Where(v => SqlFunc.Length(v.CableMaterial) > 0)
                           .WhereIF(workOrderNo != null && workOrderNo.Count() > 0, v => workOrderNo.Contains(v.OrderNumber))
                           .WhereIF(startTime != null, v => v.StartTime >= startTime)
                           .WhereIF(workcenterId != null, v => v.WorkCenterId == workcenterId)
                           .WhereIF(workStationId != null, v => v.WorkStationId == workStationId)
                           .WhereIF(!string.IsNullOrWhiteSpace(keyword), v => v.OrderNumber.Contains(keyword) || v.CableMaterial.Contains(keyword) || v.WorkCenter.Contains(keyword))
                           .WhereIF(!string.IsNullOrWhiteSpace(status), v => SqlFunc.IIF(v.TaskCompletedQty == null,"1",SqlFunc.IIF(v.TaskQuantity == v.TaskCompletedQty,"4","2")) == status)
                           .OrderBy(v => v.StartTime).OrderBy(v=> v.OrderNumber);

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

        public Task<List<V_WorkOrderInProgress>> GetListByProcessIdAsync(List<int> processIds)
        {
            return _db.Queryable<V_WorkOrderInProgress>()
                      .Where(v => processIds.Contains(v.OrderProcessId))
                      .ToListAsync();
        }

        public Task<List<V_WorkOrderInProgress>> GetOngoingCableTaskListByDateAsync(int factoryid, DateTime datetime)
        {
            return _db.Queryable<V_WorkOrderInProgress>()
                      .Where(v => v.FactoryId == factoryid && SqlFunc.IsNull(v.PrevProcessId,0) == 0 && v.StartTime <= datetime.Date && v.StartTime > datetime.AddDays(-7).Date && v.Status != "4")
                      .ToListAsync();
        }

        public async Task<List<V_WorkOrderInProgress>> GetOverdueCableTaskListByDateAsync(int factoryid, string? keyword)
        {
            return await _db.Queryable<V_WorkOrderInProgress>()
                      .Where(v => v.FactoryId == factoryid && SqlFunc.IsNull(v.PrevProcessId, 0) == 0 && v.StartTime < DateTime.Now.Date  && v.Status != "4")
                      .WhereIF(!string.IsNullOrWhiteSpace(keyword), v => v.OrderNumber.Contains(keyword) || v.CableMaterial.Contains(keyword) || v.WorkCenter.Contains(keyword))
                      .ToListAsync();
        }
    }
}
