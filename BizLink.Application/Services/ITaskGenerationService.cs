using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ITaskGenerationService
    {
        // 分析受影响的订单，重新计算并生成/更新任务
        Task GenerateOrUpdateTasksAsync(List<string> orderNos, string currentUser);

        Task<WorkOrderMaterialTaskDto> UpdateCuttingMaterialTaskAsync(int materialTaskId, string employeeId);
    }
}
