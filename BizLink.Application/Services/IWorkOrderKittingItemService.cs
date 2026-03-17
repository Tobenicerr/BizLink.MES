using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderKittingItemService : IGenericService<WorkOrderKittingItemDto, WorkOrderKittingItemCreateDto, WorkOrderKittingItemUpdateDto>
    {
        Task<List<WorkOrderKittingItemDto>> GetListByProcessIdAsync(int processId);

        Task<List<WorkOrderKittingItemDto>> GetListByProcessIdAsync(List<int> processIds);
    }
}
