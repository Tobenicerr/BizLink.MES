using Azure;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Common;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderOperationConfirmRepository : GenericRepository<WorkOrderOperationConfirm>, IWorkOrderOperationConfirmRepository
    {
        public WorkOrderOperationConfirmRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<WorkOrderOperationConfirm> GetConfirmWitemConsumeptionAsync(int confirmid)
        {
            // 使用导航查询 IncludeMany 一次性加载分组和其下的所有明细项
            return await _db.Queryable<WorkOrderOperationConfirm>()
                            .Includes(g => g.Consumps.Where(x => x.Status == "1").OrderBy(x => x.Id).ToList())
                            .Where(g => g.Id == confirmid)
                            .FirstAsync();
        }

        public async Task<List<WorkOrderOperationConfirm>> GetListByProcessIdAsync(int processid)
        {
            return await _db.Queryable<WorkOrderOperationConfirm>().Where(x => x.ProcessId == processid).ToListAsync();
        }

        public async Task<List<WorkOrderOperationConfirm>> GetListByProcessIdAsync(List<int> processids)
        {
            return await _db.Queryable<WorkOrderOperationConfirm>().Where(x => processids.Contains((int)x.ProcessId)).ToListAsync();
        }

        public async Task<(List<WorkOrderOperationConfirm>,int TotalCount)> GetOperationConfirmPageListAsync(int pageIndex, int pageSize, List<string>? orders, string? status, List<string>? operation)
        {
            var query = _db.Queryable<WorkOrderProcess>()
                .InnerJoin<WorkOrder>((p,w) => p.WorkOrderId == w.Id)
                .LeftJoin<WorkOrderOperationConfirm>((p, w, c) => p.Id == c.ProcessId)
                .Where((p, w, c) => StatusQueryRules.ActiveWorkOrderStatus.Contains(w.Status))
                .WhereIF(orders != null && orders.Count > 0, (p, w, c) => orders.Contains(p.WorkOrderNo) || orders.Contains(c.ConfirmSequence))
                .WhereIF(operation != null && operation.Count() > 0, (p, w, c) => operation.Contains(p.Operation))
                .WhereIF(!string.IsNullOrEmpty(status), (p, w, c) => SqlFunc.IsNull(c.Status, "0") == status).OrderBy((p, w, c) => p.Operation, SqlSugar.OrderByType.Asc)
                .OrderBy((p, w, c) => c.CreatedAt, SqlSugar.OrderByType.Asc)
                .Select((p, w, c) => new WorkOrderOperationConfirm()
                {
                    Id = c.Id,
                    ProcessId = p.Id,
                    SapConfirmationNo = c.SapConfirmationNo,
                    ConfirmSequence = c.ConfirmSequence,
                    CompletedFlag = c.CompletedFlag,
                    WorkOrderNo = p.WorkOrderNo,
                    WorkCenterCode = c.WorkCenterCode ?? p.WorkCenter,
                    WorkOrderId = p.WorkOrderId,
                    OperationNo = p.Operation,
                    PostingDate = c.PostingDate,
                    Remark = c.Remark,
                    EmployeeId = c.EmployeeId,
                    FactoryCode = c.FactoryCode,
                    BaseUnit = c.BaseUnit,
                    YieldQuantity = c.YieldQuantity,
                    ScrapQuantity = c.ScrapQuantity,
                    ActStartDate = c.ActStartDate,
                    ActFinishDate = c.ActFinishDate,
                    ActStartTime = c.ActStartTime,
                    ActFinishTime = c.ActFinishTime,
                    Message = c.Message,
                    MessageType = c.MessageType,
                    CreatedAt = c.CreatedAt,
                    CreatedBy = c.CreatedBy,
                    UpdatedAt = c.UpdatedAt,
                    UpdatedBy = c.UpdatedBy,
                    Status = c.Status,

                });


            var totalCount = await query.CountAsync();
            var list = await query.ToPageListAsync(pageIndex, pageSize);

            return (list, totalCount);

        }
    }
}
