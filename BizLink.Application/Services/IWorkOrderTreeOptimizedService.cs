using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderTreeOptimizedService : IGenericService<WorkOrderTreeOptimizedDto, WorkOrderTreeOptimizedCreateDto, WorkOrderTreeOptimizedUpdateDto>
    {
        Task<List<WorkOrderTreeOptimizedDto>> GetReceipetWorkOrderAsync(int factoryId,DateTime? startDate,DateTime? endDate,string? workCenter); 
    }
}
