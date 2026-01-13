using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderTaskMaterialAddRepository : IGenericRepository<WorkOrderTaskMaterialAdd>
    {

        Task<WorkOrderTaskMaterialAdd> GetByBarcodeAsync(string barcode);

        Task<List<WorkOrderTaskMaterialAdd>> GetByTaskIdAsync(int taskid);

        Task<WorkOrderTaskMaterialAdd> GetLastByWorkStationAsync(int workstationid);

    }
}
