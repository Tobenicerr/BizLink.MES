using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    internal class WorkOrderComponentsTransferRepository : GenericRepository<V_WorkOrderComponentsTransfer>, IWorkOrderComponentsTransferRepository
    {
        public WorkOrderComponentsTransferRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<(List<V_WorkOrderComponentsTransfer>, int totalCount)> GetPageListAsync(int pageIndex, int pageSize, int factoryId, string? keyword, List<string>? workorders, DateTime? startdate, DateTime? dispatchdate)
        {
            var query = _db.Queryable<V_WorkOrderComponentsTransfer>()
                .Where(v => v.FactoryId == factoryId)
                .WhereIF(!string.IsNullOrWhiteSpace(keyword), v => v.WorkOrderNo.Contains(keyword) || v.MaterialCode.Contains(keyword) || v.BatchCode.Contains(keyword) || v.ProcessStatus.Contains(keyword) || v.TransferStatus.Contains(keyword)
                || v.FromLocationCode.Contains(keyword))
                .WhereIF(workorders != null && workorders.Count > 0, v => workorders.Contains(v.WorkOrderNo))
                .WhereIF(startdate != null, v => v.ScheduledStartDate == startdate)
                .WhereIF(dispatchdate != null, v => v.ScheduledFinishDate == dispatchdate)
                .OrderBy(v => v.ScheduledStartDate)
                .OrderBy(v => v.WorkOrderNo)
                .OrderBy(v => v.MaterialCode)
                .OrderBy(v => v.BaseUnit);

            var totalCount = await query.CountAsync();
            var list = await query.ToPageListAsync(pageIndex, pageSize);

            return (list, totalCount);
        }
    }
}
