using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
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
    public class WorkOrderTaskExecuteLogRepository : GenericRepository<WorkOrderTaskExecuteLog>, IWorkOrderTaskExecuteLogRepository
    {
        public WorkOrderTaskExecuteLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<WorkOrderTaskExecuteLog>> GetListByStepTaskIdAsync(int stepTaskId)
        {
            return await _db.Queryable<WorkOrderTaskExecuteLog,WorkOrderMaterialTask>((e,t) => e.TaskId == t.Id && e.TaskLevel == TaskLevel.Material)
                .Where((e, t) => t.StepTaskId == stepTaskId).ToListAsync();
        }

        public async Task<List<WorkOrderTaskExecuteLog>> GetListByTaskIdAsync(int taskId, string taskLevel)
        {
            return await _db.Queryable<WorkOrderTaskExecuteLog>().Where(e => e.TaskId == taskId && e.TaskLevel == taskLevel).ToListAsync();
        }
    }
}
