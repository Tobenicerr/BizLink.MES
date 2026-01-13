using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IActivityLogRepository : IGenericRepository<ActivityLog>
    {
        /// <summary>
        /// 分页获取日志列表，用于后台查看。
        /// </summary>
        /// <param name="pageIndex">页码 (从1开始)</param>
        /// <param name="pageSize">每页数量</param>
        /// <returns>包含日志列表和总数的元组</returns>
        Task<(IEnumerable<ActivityLog> Logs, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize);
    }
}
