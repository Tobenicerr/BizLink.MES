using BizLink.MES.Domain.Common;
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
    public class WorkOrderTreeOptimizedRepository : GenericRepository<V_WorkOrderTreeOptimized>, IWorkOrderTreeOptimizedRepository
    {
        public WorkOrderTreeOptimizedRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<V_WorkOrderTreeOptimized>> GetReceipetWorkOrderAsync(int factoryId, DateTime? startDate, DateTime? endDate, string? workCenter)
        {
            return await _db.Queryable<V_WorkOrderTreeOptimized>().Where(v => v.FactoryId == factoryId && v.ControlKey == "CN06")
                .WhereIF(startDate != null,v => v.DispatchDate >= startDate)
                .WhereIF(endDate != null , v => v.DispatchDate <= endDate)
                .WhereIF(!string.IsNullOrWhiteSpace(workCenter), v => v.WorkCenter == workCenter).ToListAsync();
        }
    }
}
