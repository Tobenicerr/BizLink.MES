using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderBomViewService : IWorkOrderBomViewService
    {
        private readonly IWorkOrderBomViewRepository _workOrderBomViewRepository;
        public WorkOrderBomViewService(IWorkOrderBomViewRepository workOrderBomViewRepository)
        {
            _workOrderBomViewRepository = workOrderBomViewRepository;
        }

        public async Task<PagedResultDto<V_WorkOrderBom>> GetPagedListAsync(int pageIndex, int pageSize, int orderId)
        {
            var (boms, totalCount) = await _workOrderBomViewRepository.GetPagedListAsync(pageIndex, pageSize, orderId);

            return new PagedResultDto<V_WorkOrderBom> { Items = boms, TotalCount = totalCount };
        }

        public async Task<List<V_WorkOrderBom>> GetListByOrderIdAsync(int orderid)
        {
            return await _workOrderBomViewRepository.GetListByOrderIdAsync(orderid);
        }
    }
}
