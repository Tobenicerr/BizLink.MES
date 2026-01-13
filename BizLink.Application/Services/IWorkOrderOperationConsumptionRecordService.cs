using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderOperationConsumptionRecordService : IGenericService<WorkOrderOperationConsumptionRecordDto, WorkOrderOperationConsumptionRecordCreateDto, WorkOrderOperationConsumptionRecordUpdateDto>
    {
        Task<List<WorkOrderOperationConsumptionRecordDto>> GetListByProcessIdAsync(int processid);

        Task<int> DeleteAsync(List<int> ids);
    }
}
