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
    public class WorkOrderTaskRepository : GenericRepository<WorkOrderTask>, IWorkOrderTaskRepository
    {
        public WorkOrderTaskRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<WorkOrderTask>> GetByOrderNoAsync(string orderno)
        {
            return await _db.Queryable<WorkOrderTask>().Where(x => x.OrderNumber == orderno).ToListAsync();
        }

        public async Task<List<WorkOrderTask>> GetByTaskNoAsync(string taskno)
        {
            return await _db.Queryable<WorkOrderTask>().Where(x => x.TaskNumber.Contains(taskno)).ToListAsync();
        }

        public async Task<WorkOrderTask> GetByProcessIdAsync(int id, string cableItem)
        {
            return await _db.Queryable<WorkOrderTask>().Where(x => x.OrderProcessId == id && x.MaterialItem == cableItem).SingleAsync();
        }

        public async Task<List<WorkOrderTask>> GetByProcessIdsAsync(List<int> processid)
        {
            return await _db.Queryable<WorkOrderTask>().Where(x => processid.Contains((int)x.OrderProcessId)).ToListAsync();
        }

        public async Task<bool> BatchAddAsync(List<WorkOrderTask> input)
        {
            return await _db.Insertable(input).ExecuteCommandAsync() == input.Count();
        }

        public async Task<List<WorkOrderTask>> GetListByOrderIdsAsync(List<int> orderid)
        {
            return await _db.Queryable<WorkOrderTask>().Where(x => orderid.Contains((int)x.OrderId)).ToListAsync();
        }

        public async Task<List<WorkOrderTask>> GetListByIdsAsync(List<int> taskids)
        {
            return await _db.Queryable<WorkOrderTask>().Where(x => taskids.Contains(x.Id)).ToListAsync();
        }

        public async Task<int> BatchUpdateAsync(List<WorkOrderTask> input)
        {
            return await _db.Updateable(input).ExecuteCommandAsync();
        }
    }
}
