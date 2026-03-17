using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services.ApiServices
{
    public interface IWorkOrderTaskApiService
    {
        Task<WorkOrderTaskDto> CreateInspectionTaskByWorkOrderAsync(string userCode, string workOrderNo);

        Task<string> ConfirmInspectionTaskAsync(string userCode, int taskId, decimal completedQty, bool InspectResult);
    }
}
