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
    public class WorkOrderProcessViewService : IWorkOrderProcessViewService
    {

        private readonly IWorkOrderProcessViewRepository _workOrderProcessViewRepository;

        public WorkOrderProcessViewService(IWorkOrderProcessViewRepository workOrderProcessViewRepository)
        {
            _workOrderProcessViewRepository = workOrderProcessViewRepository;
        }
        public async Task<PagedResultDto<V_WorkOrderProcess>> GetPagedListAsync(int pageIndex, int pageSize, int orderId)
        {
            var (processes, totalCount) = await _workOrderProcessViewRepository.GetPagedListAsync(pageIndex, pageSize, orderId);

            return new PagedResultDto<V_WorkOrderProcess> { Items = processes, TotalCount = totalCount };
        }
    }
}
