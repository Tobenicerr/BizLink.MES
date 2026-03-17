using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderOperationConsumpRepository : GenericRepository<WorkOrderOperationConsump>, IWorkOrderOperationConsumpRepository
    {
        public WorkOrderOperationConsumpRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<int> AddAsync(List<WorkOrderOperationConsump> workOrderOperationConsumps)
        {
            return await _db.Insertable(workOrderOperationConsumps).ExecuteCommandAsync();
        }

        public async Task<int> DeleteAsync(List<int> ids)
        {
            return await _db.Deleteable<WorkOrderOperationConsump>().In(ids).ExecuteCommandAsync();
        }

        public async Task<List<WorkOrderOperationConsump>> GetListByConfirmIdsAsync(List<int> confirmIds)
        {
            return await _db.Queryable<WorkOrderOperationConsump>().Where(x => confirmIds.Contains(x.OperationConfirmId) && x.Status == "1").ToListAsync();
        }

        public async Task<List<WorkOrderOperationConsump>> GetListByProcessIdAsync(int processid)
        {
            return await _db.Queryable<WorkOrderOperationConsump, WorkOrderOperationConfirm>((s, f) => s.OperationConfirmId == f.Id).Where((s, f) => f.ProcessId == processid && s.Status == "1").ToListAsync();
        }

        public async Task<List<WorkOrderOperationConsump>> GetListByProcessIdAsync(List<int> processids)
        {
            return await _db.Queryable<WorkOrderOperationConsump, WorkOrderOperationConfirm>((s, f) => s.OperationConfirmId == f.Id).Where((s, f) => processids.Contains((int)f.ProcessId) && s.Status == "1")
                .Select((s, f) => new WorkOrderOperationConsump() 
                {
                    Id = s.Id,
                    OperationConfirmId = f.Id,
                    WorkOrderProcessId = (int)f.ProcessId,
                    SapConfirmationNo = s.SapConfirmationNo,
                    WorkOrderNo = s.WorkOrderNo,
                    ConfirmSequence = s.ConfirmSequence,
                    ReservationNo = s.ReservationNo,
                    ReservationItem = s.ReservationItem,
                    MaterialCode = s.MaterialCode,
                    BatchCode = s.BatchCode,
                    FactoryCode = s.FactoryCode,
                    FromLocationCode = s.FromLocationCode,
                    MovementType = s.MovementType,
                    Quantity = s.Quantity,
                    BaseUnit = s.BaseUnit,
                    MovementReason = s.MovementReason,
                    Status = s.Status,
                    CreatedAt = s.CreatedAt,
                    CreatedBy = s.CreatedBy,
                    UpdatedAt = s.UpdatedAt,
                    UpdatedBy = s.UpdatedBy
                }).ToListAsync();

        }
    }
}
