using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderTaskRepository : IGenericRepository<WorkOrderTask>
    {

        Task<WorkOrderTask> GetByProcessIdAsync(int id, string cableItem);

        Task<List<WorkOrderTask>> GetByProcessIdsAsync(List<int> processid);

        Task<List<WorkOrderTask>> GetByOrderNoAsync(string orderno);

        Task<List<WorkOrderTask>> GetByTaskNoAsync(string taskno);

        Task<bool> BatchAddAsync(List<WorkOrderTask> input);

        Task<List<WorkOrderTask>> GetListByOrderIdsAsync(List<int> orderid);

        Task<List<WorkOrderTask>> GetListByIdsAsync(List<int> taskids);

        Task<int> BatchUpdateAsync(List<WorkOrderTask> input);

    }
}
