using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Domain.Repositories.Common;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class PendingMaterialTaskCuttingRepository : GenericRepository<V_PendingMaterialTaskCutting>, IPendingMaterialTaskCuttingRepository
    {
        public PendingMaterialTaskCuttingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<V_PendingMaterialTaskCutting>> GetListByExecutionAsync(DateTime startdate, string workcenter)
        {
            return await _db.Queryable<V_PendingMaterialTaskCutting>().Where(x => x.ScheduledStartDate == startdate && x.WorkCenter == workcenter).ToListAsync();
        }
    }
}
