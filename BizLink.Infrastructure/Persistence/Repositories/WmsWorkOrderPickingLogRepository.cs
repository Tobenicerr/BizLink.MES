using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities.Views;
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
    public class WmsWorkOrderPickingLogRepository : GenericRepository<V_WmsWorkOrderPickingLog>, IWmsWorkOrderPickingLogRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public WmsWorkOrderPickingLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }

        public async Task<List<V_WmsWorkOrderPickingLog>> GetListByWorkOrderAsync(string workOrderNo)
        {
            return await _dbs.Queryable<V_WmsWorkOrderPickingLog>().Where(v => v.WorkOrderNo == workOrderNo).ToListAsync();
        }

        public async Task<List<V_WmsWorkOrderPickingLog>> GetListByWorkOrderAsync(List<string> workOrderNos)
        {
            return await _dbs.Queryable<V_WmsWorkOrderPickingLog>().Where(v => workOrderNos.Contains(v.WorkOrderNo)).ToListAsync();
        }
    }
}
