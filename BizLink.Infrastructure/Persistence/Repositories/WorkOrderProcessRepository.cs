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
    public class WorkOrderProcessRepository : GenericRepository<WorkOrderProcess>, IWorkOrderProcessRepository
    {
        public WorkOrderProcessRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<bool> CreateBatch(List<WorkOrderProcess> processes)
        {
            var result = await _db.Insertable(processes).ExecuteCommandAsync();
            return result == processes.Count && result > 0;
        }

        public async Task DeleteByOrderIdAsync(int orderid)
        {
            await _db.Deleteable<WorkOrderProcess>().Where(x => x.WorkOrderId == orderid).ExecuteCommandAsync();
        }

        public async Task<List<string>> GetAllOperationAsync()
        {
            return await _db.Queryable<WorkOrderProcess>().Where(x => !string.IsNullOrWhiteSpace(x.Operation)).Distinct().Select(it => it.Operation).ToListAsync();
        }

        public async Task<List<WorkOrderProcess>> GetByIdAsync(List<int> id)
        {
            return await _db.Queryable<WorkOrderProcess>().Where(x => id.Contains(x.Id)).ToListAsync();
        }

        public async Task<WorkOrderProcess> GetByOrderNo(string orderno, string operation)
        {
            return await _db.Queryable<WorkOrder, WorkOrderProcess>((o, p) => o.Id == p.WorkOrderId).Where((o, p) => o.OrderNumber == orderno && p.Operation == operation).Select((o, p) => p).FirstAsync();
        }

        public async Task<List<WorkOrderProcess>> GetListByDispatchDateAsync(int factoryid, DateTime startdate, string? operation = null, string? status = null, string? nostatus = null)
        {
            return await _db.Queryable<WorkOrder, WorkOrderProcess>((o, p) => o.Id == p.WorkOrderId).Where((o, p) => o.FactoryId == factoryid && o.ScheduledStartDate <= startdate && o.ScheduledStartDate >= DateTime.Now.AddDays(-7).Date)
                .WhereIF(!string.IsNullOrEmpty(operation), (o, p) => p.Operation == operation)
                .WhereIF(!string.IsNullOrEmpty(status), (o, p) => p.Status == status)
                .WhereIF(!string.IsNullOrEmpty(nostatus), (o, p) => p.Status != nostatus)
                .Select((o, p) => p).ToListAsync();
        }

        public async Task<List<WorkOrderProcess>> GetListByOrderIdAync(int orderid)
        {
            return await _db.Queryable<WorkOrderProcess>().Where(x => x.WorkOrderId.Equals(orderid)).ToListAsync();
        }

        public async Task<List<WorkOrderProcess>> GetListByOrderIdsAync(List<int> orderids)
        {
            return await _db.Queryable<WorkOrderProcess>().Where(x => orderids.Contains((int)x.WorkOrderId)).ToListAsync();
        }

        public async Task<List<WorkOrderProcess>> GetListByOrderNos(List<string> ordernos)
        {
            return await _db.Queryable<WorkOrderProcess>().Where(x => ordernos.Contains(x.WorkOrderNo)).ToListAsync();
        }

        public async Task<bool> UpdateBatchAsync(List<WorkOrderProcess> entities)
        {
            return await _db.Updateable(entities).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandHasChangeAsync();
        }
    }
}
