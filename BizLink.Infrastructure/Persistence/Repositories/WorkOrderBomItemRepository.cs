using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderBomItemRepository : GenericRepository<WorkOrderBomItem>, IWorkOrderBomItemRepository
    {
        public WorkOrderBomItemRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<bool> CreateBatch(List<WorkOrderBomItem> boms)
        {
            var result = await _db.Insertable(boms).ExecuteCommandAsync();
            return result == boms.Count && result > 0;
        }

        public async Task<List<WorkOrderBomItem>> GetListByOrderIdAync(int orderid)
        {
            return await _db.Queryable<WorkOrderBomItem, WorkOrder>((b, o) => b.WorkOrderId == o.Id).Where((b, o) => o.Id == orderid ).Select((b, o) => new WorkOrderBomItem() 
            {
                Id = b.Id,
                WorkOrderNo = o.OrderNumber,
                WorkOrderId = b.WorkOrderId,
                WorkOrderProcessId = b.WorkOrderProcessId,
                MaterialCode = b.MaterialCode,
                MaterialDesc = b.MaterialDesc,
                Unit = b.Unit,
                RequiredQuantity = b.RequiredQuantity,
                BomItem = b.BomItem,
                ReservationItem = b.ReservationItem,
                SuperMaterialCode = b.SuperMaterialCode,
                Operation = b.Operation,
                MovementAllowed = b.MovementAllowed,
                ConsumeType = b.ConsumeType,
                SyncWMSStatus = b.SyncWMSStatus
            }).ToListAsync();
        }

        public async Task<List<WorkOrderBomItem>> GetListByOrderNoAync(string orderno)
        {
           return await _db.Queryable<WorkOrderBomItem, WorkOrder>((b,o) => b.WorkOrderId == o.Id).Where((b, o) => o.OrderNumber == orderno).ToListAsync();
        }

        public async Task<List<WorkOrderBomItem>> GetListByOrderIdsAync(List<int> orderids)
        {
            return await _db.Queryable<WorkOrderBomItem>().Where(x => orderids.Contains(x.WorkOrderId)).ToListAsync();
        }

        public async Task<bool> UpdateWmsStatusAsync(List<WorkOrderBomItem> entities)
        {
            var result = await _db.Updateable(entities).UpdateColumns(b => new { b.SyncWMSStatus,b.UpdateBy,b.UpdateOn}).ExecuteCommandAsync();
            return result == entities.Count && result > 0;
        }

        public async Task<List<WorkOrderBomItem>> GetListByOrderNoAync(List<string> orderno)
        {
            return await _db.Queryable<WorkOrderBomItem, WorkOrder>((b,o) => b.WorkOrderId == o.Id).Where((b,o) => orderno.Contains(o.OrderNumber) && b.RequiredQuantity > 0 && b.MovementAllowed == true).Select((b,o) => new WorkOrderBomItem() 
            {
                Id = b.Id,
                WorkOrderId = b.WorkOrderId,
                WorkOrderNo = o.OrderNumber,
                WorkOrderProcessId = b.WorkOrderProcessId,
                MaterialCode = b.MaterialCode,
                MaterialDesc = b.MaterialDesc,
                Unit = b.Unit,
                RequiredQuantity = b.RequiredQuantity,
                BomItem = b.BomItem,
                ReservationItem = b.ReservationItem,
                SuperMaterialCode = b.SuperMaterialCode,
                Operation = b.Operation,
                MovementAllowed = b.MovementAllowed,
                ConsumeType = b.ConsumeType,
                SyncWMSStatus = b.SyncWMSStatus,
                ComponentScrap = b.ComponentScrap,
                QuantityIsFixed = b.QuantityIsFixed,
                CreateBy = b.CreateBy,
                CreateOn = b.CreateOn,
                UpdateBy = b.UpdateBy,
                UpdateOn = b.UpdateOn
            }).ToListAsync();
        }

        public async Task DeleteByOrderIdAsync(int orderid)
        {
            await _db.Deleteable<WorkOrderBomItem>().Where(x => x.WorkOrderId == orderid).ExecuteCommandAsync();
        }

        public async Task<List<WorkOrderBomItem>> GetListByProcessIdsAsync(List<int> processids)
        {
           return await _db.Queryable<WorkOrderBomItem>().Where(x => processids.Contains(x.WorkOrderProcessId)).ToListAsync();
        }

        public async Task<List<WorkOrderBomItem>> GetOngoingCableListByProcessIdsAsync(List<int> processids)
        {

            return await _db.Queryable<WorkOrderBomItem, WorkOrderTask>((b, t) => new JoinQueryInfos(
                JoinType.Left, b.WorkOrderProcessId == t.OrderProcessId && b.BomItem == t.MaterialItem && b.MaterialCode == t.MaterialCode))
                .Where((b, t) => b.ConsumeType == 0 && b.MovementAllowed == true && b.RequiredQuantity > 0 && SqlFunc.IsNull(t.Status, "") != "4" && processids.Contains(b.WorkOrderProcessId)).Select((b, t) => b).ToListAsync();
        }

        public async Task<List<WorkOrderBomItem>> GetOngoingCableListByDateAsync(int factoryid, DateTime startdate)
        {
            return await _db.Queryable<WorkOrder, WorkOrderProcess, WorkOrderBomItem, WorkOrderTask>((o, p, b, t) => new JoinQueryInfos(
                JoinType.Inner, o.Id == p.WorkOrderId,
                JoinType.Inner, p.Id == b.WorkOrderProcessId && o.Id == b.WorkOrderId,
                JoinType.Left, b.WorkOrderProcessId == t.OrderProcessId && b.BomItem == t.MaterialItem && b.MaterialCode == t.MaterialCode))
                .Where((o, p, b, t) => b.ConsumeType == 0 && b.MovementAllowed == true && b.RequiredQuantity > 0 && SqlFunc.IsNull(t.Status, "") != "4" && o.FactoryId == factoryid && o.ScheduledStartDate <= startdate && o.ScheduledStartDate >= startdate.AddDays(-7).Date
                ).Select((o, p, b, t) => b).ToListAsync();
        }

        public async Task<List<WorkOrderBomItem>> GetByIdAsync(List<int> id)
        {
            return await _db.Queryable<WorkOrderBomItem>().Where(x => id.Contains(x.Id)).ToListAsync();
        }

        public async Task<bool> UpdateBatchAsync(List<WorkOrderBomItem> entities)
        {
            return await _db.Updateable(entities).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandHasChangeAsync();
        }
    }
}
