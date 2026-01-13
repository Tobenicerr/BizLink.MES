using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderTaskMaterialAddRepository : GenericRepository<WorkOrderTaskMaterialAdd>, IWorkOrderTaskMaterialAddRepository
    {
        public WorkOrderTaskMaterialAddRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<WorkOrderTaskMaterialAdd> GetByBarcodeAsync(string barcode)
        {
            return await _db.Queryable<WorkOrderTaskMaterialAdd>().Where(x => x.BarCode.Equals(barcode) && x.Status.Equals("1") ).FirstAsync();
        }


        public async Task<List<WorkOrderTaskMaterialAdd>> GetByTaskIdAsync(int taskid)
        {
            return await _db.Queryable<WorkOrderTaskMaterialAdd>().Where(x => x.TaskId == taskid).ToListAsync();
        }

        public async Task<WorkOrderTaskMaterialAdd> GetLastByWorkStationAsync(int workstationid)
        {
            return await _db.Queryable<WorkOrderTaskMaterialAdd>().Where(x => x.WorkStationId == workstationid && x.Status == "0").OrderByDescending(x => x.Id).FirstAsync();

        }

    }
}
