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

        public async Task<WorkCenterGroup> GetByWorkCenterCodeAsync(string workcenterCode)
        {
            return await _db.Queryable<WorkCenterGroup,WorkCenterGroupMember,WorkCenter>((g,m,c) => g.Id == m.GroupId && m.WorkCenterId == c.Id).Where((g, m, c) => c.WorkCenterCode == workcenterCode).FirstAsync();
        }

        public async Task<List<WorkCenterGroup>> GetByWorkCenterCodeAsync(List<string> workcenterCodes)
        {
            return await _db.Queryable<WorkCenterGroup, WorkCenterGroupMember, WorkCenter>((g, m, c) => g.Id == m.GroupId && m.WorkCenterId == c.Id)
                .Where((g, m, c) => workcenterCodes.Contains(c.WorkCenterCode))
                .Select((g, m, c) => new WorkCenterGroup() 
                {
                    Id = g.Id,
                    FactoryId = g.FactoryId,
                    GroupCode = g.GroupCode,
                    GroupName = g.GroupName,
                    GroupDesc = g.GroupDesc,
                    GroupType = g.GroupType,
                    Status = g.Status,
                    IsDelete = g.IsDelete,
                    CreatedAt = g.CreatedAt,
                    UpdatedAt = g.UpdatedAt,
                    CreatedBy = g.CreatedBy,
                    UpdateBy = g.UpdateBy,
                    WorkCenterCode = c.WorkCenterCode,
                    WorkCenterId = c.Id
                })
                .ToListAsync();

        }

        public async Task<List<WorkCenterGroup>> GetListByGroupTypeAsync(int factoryid, string grouptype)
        {
            return await _db.Queryable<WorkCenterGroup>().Where(x => x.GroupType == grouptype && x.FactoryId == factoryid).ToListAsync();
        }
    }
}
