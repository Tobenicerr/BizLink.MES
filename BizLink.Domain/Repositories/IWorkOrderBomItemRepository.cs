using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderBomItemRepository : IGenericRepository<WorkOrderBomItem>
    {
        Task<bool> CreateBatch(List<WorkOrderBomItem> boms);

        Task<List<WorkOrderBomItem>> GetListByOrderNoAync(string orderno);

        Task<List<WorkOrderBomItem>> GetListByOrderNoAync(List<string> orderno);

        Task<List<WorkOrderBomItem>> GetListByOrderIdAync(int orderid);

        Task<List<WorkOrderBomItem>> GetListByOrderIdsAync(List<int> orderids);

        Task<bool> UpdateWmsStatusAsync(List<WorkOrderBomItem> entities);

        Task DeleteByOrderIdAsync(int orderid);

        Task<List<WorkOrderBomItem>> GetListByProcessIdsAsync(List<int> processids);

        Task<List<WorkOrderBomItem>> GetOngoingCableListByProcessIdsAsync(List<int> processids);

        Task<List<WorkOrderBomItem>> GetOngoingCableListByDateAsync(int factoryid, DateTime startdate);

        Task<List<WorkOrderBomItem>> GetByIdAsync(List<int> id);

        Task<bool> UpdateBatchAsync(List<WorkOrderBomItem> entities);
    }
}
