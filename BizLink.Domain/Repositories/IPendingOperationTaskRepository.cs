using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IPendingOperationTaskRepository : IGenericRepository<V_PendingOperationTask>
    {
        Task<List<V_PendingOperationTask>> GetListByWorkCentersAsync(int factoryId, List<string> workcenters, DateTime startDate, DateTime endDate);
    }
}
