using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class ActivityLogRepository : GenericRepository<ActivityLog>, IActivityLogRepository
    {
        public ActivityLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<(IEnumerable<ActivityLog> Logs, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize)
        {
            var query = _db.Queryable<ActivityLog>().OrderBy(it => it.Timestamp, OrderByType.Desc);

            // 第一步：异步获取总记录数
            int totalCount = await query.CountAsync();

            // 第二步：异步获取当前页的数据
            // 使用 Skip 和 Take 是最标准的分页方式
            var logs = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return (logs, totalCount);
        }
    }
}
