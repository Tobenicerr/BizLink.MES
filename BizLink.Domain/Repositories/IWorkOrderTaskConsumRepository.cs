using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderTaskConsumRepository : IGenericRepository<WorkOrderTaskConsum>
    {
        Task BatchAddAsync(List<WorkOrderTaskConsum> consums);
        Task<List<WorkOrderTaskConsum>> GetListByConfirmIdAsync(int confirmid);

        Task<List<WorkOrderTaskConsum>> GetCableListByProcessIdsAsync(List<int> processids);
    }
}
