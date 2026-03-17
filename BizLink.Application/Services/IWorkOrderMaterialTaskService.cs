using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderMaterialTaskService : IGenericService<WorkOrderMaterialTaskDto, WorkOrderMaterialTaskCreateDto, WorkOrderMaterialTaskUpdateDto>
    {
        Task<List<WorkOrderMaterialTaskDto>> GetListByStepTaskIdAsync(int stepId, string? TaskSubType = null);

        Task<List<WorkOrderMaterialTaskDto>> GetListByStepTaskIdAsync(List<int> stepIds, string? TaskSubType = null);

        Task<List<WorkOrderMaterialTaskDto>> GetByIdAsync(List<int> ids);

        Task<bool> UpdateBatchAsync(List<WorkOrderMaterialTaskUpdateDto> updateDtos);

        Task<List<WorkOrderMaterialTaskDto>> GetCuttingViewByProcessIdAsync(List<int> processIds);

        Task<WorkOrderMaterialTaskDto> GetCuttingViewByBomIdAsync(int bomId);

    }
}
