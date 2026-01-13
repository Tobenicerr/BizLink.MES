using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderTaskConsumRepository : GenericRepository<WorkOrderTaskConsum>, IWorkOrderTaskConsumRepository
    {
        public WorkOrderTaskConsumRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
        public async Task BatchAddAsync(List<WorkOrderTaskConsum> consums)
        {
            await _db.Insertable(consums).ExecuteCommandAsync();
        }

        public async Task<List<WorkOrderTaskConsum>> GetCableListByProcessIdsAsync(List<int> processids)
        {
            return await _db.Queryable<WorkOrderTaskConsum, WorkOrderTaskConfirm, WorkOrderTask, WorkOrderProcess>((s, c, t, p) => s.ConfirmId == c.Id && c.TaskId == t.Id && t.OrderProcessId == p.Id).Where((s, c, t, p) => t.CableLength != null && processids.Contains(p.Id)).Select((s, c, t, p) => s).ToListAsync();
        }

        public async Task<List<WorkOrderTaskConsum>> GetListByConfirmIdAsync(int confirmid)
        {
            return await _db.Queryable<WorkOrderTaskConsum>().Where(x => x.ConfirmId == confirmid).ToListAsync();
        }
    }
}
