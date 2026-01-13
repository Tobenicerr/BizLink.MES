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
    public class WorkOrderOperationConsumpRepository : GenericRepository<WorkOrderOperationConsump>, IWorkOrderOperationConsumpRepository
    {
        public WorkOrderOperationConsumpRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<int> AddAsync(List<WorkOrderOperationConsump> workOrderOperationConsumps)
        {
            return await _db.Insertable(workOrderOperationConsumps).ExecuteCommandAsync();
        }

        public async Task<int> DeleteAsync(List<int> ids)
        {
            return await _db.Deleteable<WorkOrderOperationConsump>().In(ids).ExecuteCommandAsync();
        }

        public async Task<List<WorkOrderOperationConsump>> GetListByProcessIdAsync(int processid)
        {
            return await _db.Queryable<WorkOrderOperationConsump, WorkOrderOperationConfirm>((s, f) => s.OperationConfirmId == f.Id).Where((s, f) => f.ProcessId == processid && s.Status == "1").ToListAsync();
        }
    }
}
