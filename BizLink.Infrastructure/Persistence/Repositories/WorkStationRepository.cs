using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkStationRepository : GenericRepository<WorkStation>, IWorkStationRepository
    {
        public WorkStationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }



        public async Task<List<WorkStation>> GetAllAsync(int factoryid)
        {
            return await _db.Queryable<WorkStation>().Where(x => x.FactoryId.Equals(factoryid)).ToListAsync();
        }

        public async Task<WorkStation> GetByCodeAsync(string code)
        {
            return await _db.Queryable<WorkStation>().Where(x => x.WorkStationCode.Equals(code)).SingleAsync();
        }

        public async Task<List<WorkStation>> GetByIdAsync(List<int> id)
        {
            return await _db.Queryable<WorkStation>().Where(x => id.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<WorkStation>> GetByWorkcenterGroupCodeAsync(string groupcode)
        {
            return await _db.Queryable<WorkStation, WorkCenterGroupMember, WorkCenterGroup>((s, c, g) => s.WorkCenterId == c.WorkCenterId && c.GroupId == g.Id).Where((s, c, g) => !s.IsDelete  && !g.IsDelete  && g.GroupCode == groupcode).ToListAsync();
        }

        public async Task<List<WorkStation>> GetByWorkcenterIdAsync(int workcenterid)
        {
            return await _db.Queryable<WorkStation>().Where(x => x.WorkCenterId.Equals(workcenterid)).ToListAsync();
        }
    }
}
