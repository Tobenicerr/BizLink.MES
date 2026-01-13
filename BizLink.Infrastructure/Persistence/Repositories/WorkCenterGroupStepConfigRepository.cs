using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Domain.Repositories.Common;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkCenterGroupStepConfigRepository : GenericRepository<WorkCenterGroupStepConfig>, IWorkCenterGroupStepConfigRepository
    {
        public WorkCenterGroupStepConfigRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
