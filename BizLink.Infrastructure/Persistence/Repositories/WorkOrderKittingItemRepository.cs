using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Domain.Repositories.Common;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderKittingItemRepository : GenericRepository<WorkOrderKittingItem>, IWorkOrderKittingItemRepository
    {
        public WorkOrderKittingItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<WorkOrderKittingItem>> GetListByProcessIdAsync(int processId)
        {
            return await _db.Queryable<WorkOrderKittingItem>().Where(x => x.WorkOrderProcessId == processId).ToListAsync();
        }

        public async Task<List<WorkOrderKittingItem>> GetListByProcessIdAsync(List<int> processId)
        {
            return await _db.Queryable<WorkOrderKittingItem>().Where(x => processId.Contains(x.WorkOrderProcessId)).ToListAsync();
        }
    }
}
