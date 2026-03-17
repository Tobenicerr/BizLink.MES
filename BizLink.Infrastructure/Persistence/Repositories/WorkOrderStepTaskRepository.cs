using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Domain.Repositories.Common;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderStepTaskRepository : GenericRepository<WorkOrderStepTask>, IWorkOrderStepTaskRepository
    {
        public WorkOrderStepTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<WorkOrderStepTask> GetByOperationIdAsync(int operationTaskId, string taskCategory)
        {
            return await _db.Queryable<WorkOrderStepTask>().Where(w => w.OperationTaskId == operationTaskId && w.TaskCategory == taskCategory).FirstAsync();
        }

        public async Task<List<WorkOrderStepTask>> GetListByOperationIdAsync(int operationTaskId)
        {
            return await _db.Queryable<WorkOrderStepTask>().Where(w => w.OperationTaskId == operationTaskId).ToListAsync();
        }

        public async Task<List<WorkOrderStepTask>> GetListByOperationIdAsync(List<int> operationTaskIds)
        {
            return await _db.Queryable<WorkOrderStepTask>().Where(w => operationTaskIds.Contains((int)w.OperationTaskId)).ToListAsync();

        }

        public async Task<List<WorkOrderStepTask>> GetListByWorkOrderProcessIdAsync(List<int> processIds, string? taskCategory = null)
        {
            return await _db.Queryable<WorkOrderStepTask, WorkOrderOperationTask>((s, o) => s.OperationTaskId == o.Id)
                .Where((s, o) => processIds.Contains((int)o.WorkOrderProcessId))
                .WhereIF(!string.IsNullOrEmpty(taskCategory), (s, o) => s.TaskCategory == taskCategory)
                .Select((s, o) => new WorkOrderStepTask()
                {
                    Id = s.Id,
                    OperationTaskId = o.Id,
                    WorkOrderProcessId = o.WorkOrderProcessId,
                    StepCode = s.StepCode,
                    WorkCenterGroupId = s.WorkCenterGroupId,
                    WorkCenterGroupCode = s.WorkCenterGroupCode,
                    ActualWorkCenterId = s.ActualWorkCenterId,
                    ActualWorkCenterCode = s.ActualWorkCenterCode,
                    ActualWorkStationId = s.ActualWorkStationId,
                    ActualWorkStationCode = s.ActualWorkStationCode,
                    Quantity = s.Quantity,
                    CompletedQuantity = s.CompletedQuantity,
                    TaskCategory = s.TaskCategory,
                    PreStepIds = s.PreStepIds,
                    Status = s.Status,
                    ActualStartTime = s.ActualStartTime,
                    ActualEndTime = s.ActualEndTime,
                    CreatedOn = s.CreatedOn,
                    CreatedBy = s.CreatedBy,
                    UpdatedOn = s.UpdatedOn,
                    UpdateBy = s.UpdateBy
                }).ToListAsync();
        }

        public async Task<List<WorkOrderStepTask>> GetSortListByIdsAsync(List<int> Ids)
        {
            return await _db.Queryable<WorkOrderStepTask,WorkTaskCategory>((s,c) => s.TaskCategory == c.CategoryCode).Where((s, c) => Ids.Contains(s.Id)).OrderBy((s, c) => c.SortNo).ToListAsync();
        }
    }
}
