using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public  interface IWorkOrderBomViewService
    {

        Task<PagedResultDto<V_WorkOrderBom>> GetPagedListAsync(int pageIndex, int pageSize, int orderId);

        Task<List<V_WorkOrderBom>> GetListByOrderIdAsync(int orderid);
    }
}
