using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderKittingExecuteRepository : IGenericRepository<V_WorkOrderKittingExecute>
    {

        Task<(List<V_WorkOrderKittingExecute>, int)> GetPageListAsync(int pageIndex, int pageSize, int factoryId, DateTime? startdateStart, DateTime? startdateEnd, List<string> workOrders, List<string> groupCodes);
        Task<List<V_WorkOrderKittingExecute>> GetListByGroupCodeAsync(string groupCode);

    }
}
