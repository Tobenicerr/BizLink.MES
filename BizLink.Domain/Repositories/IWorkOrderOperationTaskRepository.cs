using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderOperationTaskRepository :IGenericRepository<WorkOrderOperationTask>
    {
        Task<List<WorkOrderOperationTask>> GetByIdAsync(List<int> ids);

        Task<List<WorkOrderOperationTask>> GetListByProcessIdAsync(List<int> processIds);

        Task<List<WorkOrderOperationTask>> GetListByOrderIdAsync(int orderId);
    }
}
