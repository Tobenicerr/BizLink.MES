using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkCenterGroupRepository : GenericRepository<WorkCenterGroup>, IWorkCenterGroupRepository
    {
        public WorkCenterGroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<WorkCenterGroup>> GetListByGroupTypeAsync(int factoryid, string grouptype)
        {
            return await _db.Queryable<WorkCenterGroup>().Where(x => x.GroupType == grouptype && x.FactoryId == factoryid).ToListAsync();
        }
    }
}
