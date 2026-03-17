using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderTaskExecuteConsumpRepository : IGenericRepository<WorkOrderTaskExecuteConsump>
    {
        Task<List<WorkOrderTaskExecuteConsump>> GetListByTaskIdAsync(string taskLevel, int taskId);

        Task<List<WorkOrderTaskExecuteConsump>> GetListByTaskIdAsync(string taskLevel, List<int> taskIds);

        Task<List<WorkOrderTaskExecuteConsump>> GetListByExelogIdAsync(int exeId);
    }
}
