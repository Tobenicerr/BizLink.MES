using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderTaskExecuteLogRepository : IGenericRepository<WorkOrderTaskExecuteLog>
    {
        Task<List<WorkOrderTaskExecuteLog>> GetListByStepTaskIdAsync(int stepTaskId);

        Task<List<WorkOrderTaskExecuteLog>> GetListByTaskIdAsync(int taskId, string TaskLevel);
    }
}
