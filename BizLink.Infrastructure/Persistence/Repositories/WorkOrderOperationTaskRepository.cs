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
    public class WorkOrderOperationTaskRepository : GenericRepository<WorkOrderOperationTask>, IWorkOrderOperationTaskRepository
    {
        public WorkOrderOperationTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
