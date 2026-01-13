using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderOperationConsumptionRecordRepository : IGenericRepository<WorkOrderOperationConsumptionRecord>
    {

        Task<List<WorkOrderOperationConsumptionRecord>> GetListByProcessIdAsync(int processid);

        Task<int> DeleteAsync(List<int> ids);
    }
}
