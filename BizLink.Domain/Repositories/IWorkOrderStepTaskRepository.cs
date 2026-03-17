using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderStepTaskRepository : IGenericRepository<WorkOrderStepTask>
    {
        Task<List<WorkOrderStepTask>> GetListByOperationIdAsync(int operationTaskId);

        Task<List<WorkOrderStepTask>> GetListByOperationIdAsync(List<int> operationTaskIds);

        Task<List<WorkOrderStepTask>> GetSortListByIdsAsync(List<int> Ids);


        Task<WorkOrderStepTask> GetByOperationIdAsync(int operationTaskId, string taskCategory);

        Task<List<WorkOrderStepTask>> GetListByWorkOrderProcessIdAsync(List<int> processIds, string? taskCategory = null);

        
    }
}
