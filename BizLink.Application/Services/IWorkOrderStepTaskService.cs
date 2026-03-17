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
    public interface IWorkOrderStepTaskService:IGenericService<WorkOrderStepTaskDto, WorkOrderStepTaskCreateDto, WorkOrderStepTaskUpdateDto>
    {
        Task<List<WorkOrderStepTaskDto>> GetListByOperationIdAsync(int operationTaskId);

        Task<List<WorkOrderStepTaskDto>> GetListByOperationIdAsync(List<int> operationTaskIds);

        Task<bool> UpdateBatchAsync(List<WorkOrderStepTaskUpdateDto> updateDtos);

        Task<List<WorkOrderStepTaskDto>> GetListByWorkOrderProcessIdAsync(List<int> processIds,string? taskCategory = null);
    }
}
