using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderViewRepository : IGenericRepository<V_WorkOrder>
    {

        Task<V_WorkOrder> GetByOrderIdAsync(int orderid);
        Task<(IEnumerable<V_WorkOrder> Orders, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, string keyword, DateTime? dispatchDate, DateTime? startDate,string factorycode);

        Task UpdateJyWmsRequrementTaskAsync();

        Task<int> GetPickMtrStockByWorkOrderAsync(string workorderno);

    }
}
