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
using static Dm.net.buffer.ByteArrayBuffer;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkCenterRepository : GenericRepository<WorkCenter>, IWorkCenterRepository
    {

        public WorkCenterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<List<WorkCenter>> GetAllAsync(int factoryid)
        {
            return await _db.Queryable<WorkCenter>().Where(x => x.FactoryId.Equals(factoryid)).ToListAsync();
        }

        public async Task<WorkCenter> GetByCodeAsync(string code)
        {
            return await _db.Queryable<WorkCenter>().Where(x => x.WorkCenterCode.Equals(code)).FirstAsync();
        }

        public async Task<List<WorkCenter>> GetBygroupIdAsync(int groupid)
        {
            return await _db.Queryable<WorkCenter, WorkCenterGroupMember, WorkCenterGroup> ((w,m,g) => (w.Id == m.WorkCenterId && m.GroupId == g.Id)).Where((w, m, g) => g.Id.Equals(groupid)).ToListAsync();
        }

        public async Task<List<WorkCenter>> GetListByGroupCodeAsync(string groupcode)
        {
            return await _db.Queryable<WorkCenter, WorkCenterGroupMember, WorkCenterGroup>((w, m, g) => (w.Id == m.WorkCenterId && m.GroupId == g.Id)).Where((w, m, g) => !w.IsDelete && g.GroupCode.Equals(groupcode)).ToListAsync();
        }

        public async Task<List<WorkCenter>> GetListByGroupCodeAsync(List<string> groupcode)
        {
            return await _db.Queryable<WorkCenter, WorkCenterGroupMember, WorkCenterGroup>((w, m, g) => (w.Id == m.WorkCenterId && m.GroupId == g.Id)).Where((w, m, g) => !w.IsDelete && groupcode.Contains(g.GroupCode)).ToListAsync();
        }
    }
}
