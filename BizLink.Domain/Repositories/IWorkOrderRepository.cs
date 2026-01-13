using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderRepository : IGenericRepository<WorkOrder>
    {
        Task<List<WorkOrder>> GetListByDispatchDateAsync(DateTime? dispatchdate, DateTime? startdate, List<string>? ordernos, int factoryid);

        Task<int> CountByOrderNosNoLockAsync(List<string> ordernos);

        Task<List<WorkOrder>> GetListByBomMaterialAsync(string bommaterialcode);

        Task<List<WorkOrder>> GetByIdAsync(List<int> ids);

        Task<WorkOrder> GetByOrderNoAsync(string orderno);

        Task<List<WorkOrder>> GetByOrderNoAsync(List<string> ordernos);

        Task<(List<WorkOrder>, List<WorkOrderProcess>, List<WorkOrderTask>)> GetCableTaskConfirmListAsync(List<string>? orders, DateTime? starttime, int? workcenterid, int? workstationid);

        Task<List<WorkOrder>> GetListByDispatchDateEndAsync(int factoryid, DateTime startdate);

        Task<List<int>> AddBatchAsync(List<WorkOrder> createDtos);

        Task<bool> UpdateBatchAsync(List<WorkOrder> updateDtos);
    }
}
