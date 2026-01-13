using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderOperationConsumpRepository : IGenericRepository<WorkOrderOperationConsump>
    {
        Task<int> AddAsync(List<WorkOrderOperationConsump> workOrderOperationConsumps);

        Task<int> DeleteAsync(List<int> ids);

        Task<List<WorkOrderOperationConsump>> GetListByProcessIdAsync(int processid);
    }
}
