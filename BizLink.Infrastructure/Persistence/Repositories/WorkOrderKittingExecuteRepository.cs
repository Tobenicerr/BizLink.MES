using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data.OscarClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderKittingExecuteRepository : GenericRepository<V_WorkOrderKittingExecute>, IWorkOrderKittingExecuteRepository
    {
        public WorkOrderKittingExecuteRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<V_WorkOrderKittingExecute>> GetListByGroupCodeAsync(string groupCode)
        {
            return await _db.Queryable<V_WorkOrderKittingExecute>().Where(v => v.GroupCode == groupCode).ToListAsync();
        }

        public async Task<(List<V_WorkOrderKittingExecute>, int)> GetPageListAsync(int pageIndex, int pageSize, int factoryId, DateTime? startdateStart, DateTime? startdateEnd, List<string> workOrders, List<string> groupCodes)
        {
            var query = _db.Queryable<V_WorkOrderKittingExecute>()
                                       .Where(v => v.FactoryId == factoryId)
                                       .WhereIF(workOrders != null && workOrders.Count() > 0, v => workOrders.Contains(v.WorkOrderNo))
                                       .WhereIF(groupCodes != null && groupCodes.Count() > 0, v => groupCodes.Contains(v.GroupCode))


                                       .WhereIF(startdateStart != null, v => v.StartDate >= startdateStart)
                                       .WhereIF(startdateEnd != null, v => v.StartDate <= startdateEnd)
                                   
                                       .OrderBy(v => v.WorkOrderNo).OrderByDescending(v => v.GroupCode);

            var totalCount = await query.CountAsync();
            var list = await query.ToPageListAsync(pageIndex, pageSize);

            return (list, totalCount);
        }
    }
}
