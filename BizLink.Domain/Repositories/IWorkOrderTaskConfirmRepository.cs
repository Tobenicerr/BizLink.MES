using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderTaskConfirmRepository : IGenericRepository<WorkOrderTaskConfirm>
    {

        //void AddByTranAsync(WorkOrderTaskConfirm confirm, List<WorkOrderTaskConsum> consums, WorkOrderTask task, ProductLinesideStock productLinesideStock);

        Task<List<WorkOrderTaskConfirm>> GetListByTaskIdAsync(int taskid);

        Task<List<WorkOrderTaskConfirm>> GetListByOrderNoAsync(string orderno);

        Task<List<int>> BatchAddAsync(List<WorkOrderTaskConfirm> input);

        Task<WorkOrderTaskConfirm> GetByStationIdAsync(int prossid, int stationid, List<int> confirmids);

        Task<List<WorkOrderTaskConfirm>> GetByEndStationAsync(List<int> prossid, int stationid);

    }
}
