using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderProcessViewRepository : IGenericRepository<V_WorkOrderProcess>
    {
        /// <summary>
        /// 获取工单工序视图的分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="keyword">搜索关���字</param>
        /// <returns>返回工单工序视图的分页结果</returns>
        Task<(IEnumerable<V_WorkOrderProcess> Processes, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, int orderId);

        Task<V_WorkOrderProcess> GetByOperationIdAsync(int id);

        Task UpdateJyTaskAsync(string orderno, string bomitem, string batchcode, string barcode);
    }
}
