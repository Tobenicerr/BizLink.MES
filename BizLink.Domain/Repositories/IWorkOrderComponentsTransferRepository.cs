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
    public interface IWorkOrderComponentsTransferRepository : IGenericRepository<V_WorkOrderComponentsTransfer>
    {
        Task<(List<V_WorkOrderComponentsTransfer>, int totalCount)> GetPageListAsync(int pageIndex, int pageSize, int factoryId, string? keyword, List<string>? workorders, DateTime? startdate, DateTime? dispatchdate);
    }
}
