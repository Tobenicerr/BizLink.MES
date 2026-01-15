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
    }
}
