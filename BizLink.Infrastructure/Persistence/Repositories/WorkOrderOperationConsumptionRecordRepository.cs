using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderOperationConsumptionRecordRepository : GenericRepository<WorkOrderOperationConsumptionRecord>, IWorkOrderOperationConsumptionRecordRepository
    {
        public WorkOrderOperationConsumptionRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<int> DeleteAsync(List<int> ids)
        {
            return await _db.Deleteable<WorkOrderOperationConsumptionRecord>().In(ids).ExecuteCommandAsync();
        }

        public Task<List<WorkOrderOperationConsumptionRecord>> GetListByProcessIdAsync(int processid)
        {
            return _db.Queryable<WorkOrderOperationConsumptionRecord>()
                .Where(record => record.WorkOrderProcessId == processid)
                .ToListAsync();
        }
    }
}
