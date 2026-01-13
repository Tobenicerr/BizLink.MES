using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderTaskConfirmRepository:GenericRepository<WorkOrderTaskConfirm>,  IWorkOrderTaskConfirmRepository
    {


        public WorkOrderTaskConfirmRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            //_db = db;
            //_taskRep = new WorkOrderTaskRepository(unitOfWork);
            //_consumRep = new WorkOrderTaskConsumRepository(_db);
            //_materialAddRep = new WorkOrderTaskMaterialAddRepository(_db);
            //_productLinesideStockRep = new ProductLinesideStockRepository(_db);
        }

        public async Task<List<int>> BatchAddAsync(List<WorkOrderTaskConfirm> input)
        {
            return await _db.Insertable(input).ExecuteReturnPkListAsync<int>();
        }

        public async Task<WorkOrderTaskConfirm> GetByStationIdAsync(int prossid, int stationid, List<int> confirmids)
        {
            return await _db.Queryable<WorkOrderTaskConfirm, WorkOrderTask>((c, t) => c.TaskId == t.Id).Where((c, t) => t.OrderProcessId == prossid && t.WorkStationId == stationid && confirmids.Contains(c.Id)).FirstAsync();
        }

        public async Task<List<WorkOrderTaskConfirm>> GetByEndStationAsync(List<int> prossid, int stationid)
        {
            return await _db.Queryable<WorkOrderTaskConfirm, WorkOrderTask>((c, t) => c.TaskId == t.Id).Where((c, t) => prossid.Contains((int)t.OrderProcessId) && t.WorkStationId == stationid).ToListAsync();
        }

        public Task<List<WorkOrderTaskConfirm>> GetListByOrderNoAsync(string orderno)
        {
            return _db.Queryable<WorkOrderTaskConfirm, WorkOrderTask>((c,t) => c.TaskId == t.Id)
                .Where((c, t) => t.OrderNumber == orderno).Select(c => c).ToListAsync();
        }


        //public async void AddByTranAsync(WorkOrderTaskConfirm confirm, List<WorkOrderTaskConsum> consums, WorkOrderTask task, ProductLinesideStock productLinesideStock)
        //{
        //     _db.Ado.UseTran( async() => 
        //    {
        //       var confirmId =  _db.Insertable(confirm).ExecuteReturnIdentity();

        //        await _taskRep.UpdateAsync(task);

        //        consums.ForEach(c => c.ConfirmId = confirmId);
        //        await _consumRep.BatchAddAsync(consums);
        //        var consumsSum = consums.GroupBy(x => x.BarCode).Select(group => new
        //        {
        //            BarCode = group.Key,
        //            Quantity = group.Sum(x => x.EntryQuantity)
        //        });
        //        foreach (var item in consumsSum)
        //        {
        //            var temp = await _materialAddRep.GetByBarcodeAsync(item.BarCode);
        //            temp.LastQuantity -= item.Quantity;
        //            await _materialAddRep.UpdateAsync(temp);

        //        }

        //        await _productLinesideStockRep.AddAsync(productLinesideStock);

        //    });
        //}

        public async Task<List<WorkOrderTaskConfirm>> GetListByTaskIdAsync(int taskid)
        {
            return await _db.Queryable<WorkOrderTaskConfirm>().Where(confirm => confirm.TaskId == taskid).ToListAsync();
        }

    }
}
