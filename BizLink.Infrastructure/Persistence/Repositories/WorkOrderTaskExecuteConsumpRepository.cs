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
    public class WorkOrderTaskExecuteConsumpRepository : GenericRepository<WorkOrderTaskExecuteConsump>, IWorkOrderTaskExecuteConsumpRepository
    {
        public WorkOrderTaskExecuteConsumpRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<WorkOrderTaskExecuteConsump>> GetListByExelogIdAsync(int exeId)
        {
            return await _db.Queryable<WorkOrderTaskExecuteConsump>().Where(c => c.ExeLogId == exeId && c.Status =="1").ToListAsync();
        }

        public async Task<List<WorkOrderTaskExecuteConsump>> GetListByTaskIdAsync(string taskLevel, int taskId)
        {
            return await _db.Queryable<WorkOrderTaskExecuteConsump, WorkOrderTaskExecuteLog>((c,l) => c.ExeLogId == l.Id)
                .Where((c, l) => l.TaskLevel == taskLevel && l.TaskId == taskId && c.Status == "1").ToListAsync();
        }

        public async Task<List<WorkOrderTaskExecuteConsump>> GetListByTaskIdAsync(string taskLevel, List<int> taskIds)
        {
            return await _db.Queryable<WorkOrderTaskExecuteConsump, WorkOrderTaskExecuteLog>((c, l) => c.ExeLogId == l.Id)
                .Where((c, l) => l.TaskLevel == taskLevel && taskIds.Contains((int)l.TaskId) && c.Status == "1").ToListAsync();
        }
    }
}
