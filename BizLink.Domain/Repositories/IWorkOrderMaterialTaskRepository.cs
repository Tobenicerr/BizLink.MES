using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderMaterialTaskRepository: IGenericRepository<WorkOrderMaterialTask>
    {

        Task<List<WorkOrderMaterialTask>> GetListByStepTaskIdAsync(int stepId, string? TaskSubType = null);

        Task<List<WorkOrderMaterialTask>> GetListByStepTaskIdAsync(List<int> stepIds, string? TaskSubType = null);

        Task<List<WorkOrderMaterialTask>> GetListByProcessIdAsync(int processId, string? TaskCategory = null, string? TaskSubType = null);

        Task<List<WorkOrderMaterialTask>> GetListByBomIdAsync(List<int> bomIds,  string? TaskSubType = null, string? refResonCode = null);


        Task<List<WorkOrderMaterialTask>> GetCuttingViewByProcessIdAsync(int processId);

        Task<List<WorkOrderMaterialTask>> GetCuttingViewByProcessIdAsync(List<int> processIds);

        Task<WorkOrderMaterialTask> GetCuttingViewByBomIdAsync(int bomId);
    }
}
