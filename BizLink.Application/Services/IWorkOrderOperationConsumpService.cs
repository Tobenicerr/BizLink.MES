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
    public interface IWorkOrderOperationConsumpService: IGenericService<WorkOrderOperationConsumpDto, WorkOrderOperationConsumpCreateDto, WorkOrderOperationConsumpUpdateDto>
    {
        Task<int> CreateAsync(List<WorkOrderOperationConsumpCreateDto> createDtos);

        Task<int> DeleteAsync(List<int> ids);

        Task<bool> UpdateAsync(List<WorkOrderOperationConsumpUpdateDto> updateDtos);

        Task<List<WorkOrderOperationConsumpDto>> GetListByProcessIdAsync(int processid);
    }
}
