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
    public interface IWorkOrderBomViewRepository: IGenericRepository<V_WorkOrderBom>
    {
        Task<(List<V_WorkOrderBom> boms, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, int orderId);

        Task<List<V_WorkOrderBom>> GetListByOrderIdAsync(int orderid);
    }

}
