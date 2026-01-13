using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderProcessRepository : IGenericRepository<WorkOrderProcess>
    {
        Task<bool> CreateBatch(List<WorkOrderProcess> processes);

        

        Task<List<WorkOrderProcess>> GetListByOrderIdAync(int orderid);

        Task<WorkOrderProcess> GetByOrderNo(string orderno, string operation);

        Task<List<WorkOrderProcess>> GetListByOrderNos(List<string> ordernos);

        Task<List<WorkOrderProcess>> GetListByOrderIdsAync(List<int> orderids);

        Task DeleteByOrderIdAsync(int orderid);

        Task<List<string>> GetAllOperationAsync();

        Task<List<WorkOrderProcess>> GetListByDispatchDateAsync(int factoryid, DateTime startdate, string? operation = null, string? status = null, string? nostatus = null);

        Task<List<WorkOrderProcess>> GetByIdAsync(List<int> id);

        Task<bool> UpdateBatchAsync(List<WorkOrderProcess> entities);

    }
}
