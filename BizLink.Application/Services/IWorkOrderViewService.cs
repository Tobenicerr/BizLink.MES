using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderViewService : IGenericService<WorkOrderViewDto, WorkOrderViewCreateDto, WorkOrderViewUpdateDto>
    {
        Task<PagedResultDto<WorkOrderViewDto>> GetPagedListAsync(int pageIndex, int pageSize, string keyword, DateTime? dispatchDate, DateTime? startDate,string factorycode);

        Task UpdateJyWmsRequrementTaskAsync();

        Task<int> GetPickMtrStockByWorkOrderAsync(string workorderno);
    }
}
