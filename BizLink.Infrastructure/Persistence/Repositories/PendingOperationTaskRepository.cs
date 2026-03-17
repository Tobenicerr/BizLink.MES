using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class PendingOperationTaskRepository : GenericRepository<V_PendingOperationTask>, IPendingOperationTaskRepository
    {
        public PendingOperationTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<V_PendingOperationTask>> GetListByWorkCentersAsync(int factoryId, List<string> workcenters, DateTime startDate, DateTime endDate)
        {
            return await _db.Queryable<V_PendingOperationTask>().Where(x => x.FactoryId == factoryId && workcenters.Contains(x.WorkCenter) && x.DispatchDate >= startDate && x.DispatchDate <= endDate).ToListAsync();
        }
    }
}
