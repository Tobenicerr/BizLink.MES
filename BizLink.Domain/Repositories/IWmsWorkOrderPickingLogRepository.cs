using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWmsWorkOrderPickingLogRepository : IGenericRepository<V_WmsWorkOrderPickingLog>
    {
        Task<List<V_WmsWorkOrderPickingLog>> GetListByWorkOrderAsync(string workOrderNo);

        Task<List<V_WmsWorkOrderPickingLog>> GetListByWorkOrderAsync(List<string> workOrderNos);
    }
}
