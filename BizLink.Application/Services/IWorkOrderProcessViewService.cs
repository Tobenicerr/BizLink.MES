using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderProcessViewService
    {
        /// <summary>
        /// 获取工单工序视图的分页列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="orderId">工单ID</param>
        /// <returns>返回工单工序视图的分页结果</returns>
        Task<PagedResultDto<V_WorkOrderProcess>> GetPagedListAsync(int pageIndex, int pageSize, int orderId);
    }
}
