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
    public class AutoStockOutRepository : GenericRepository<V_AutoStockOut>, IAutoStockOutRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public AutoStockOutRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("FosiConnection");
        }

        public async Task<List<V_AutoStockOut>> GetListByWorkOrderAsync(string workorder)
        {
            return await _dbs.Queryable<V_AutoStockOut>().Where(x => x.WorkOrderNo == workorder).ToListAsync();
        }

        public async Task<List<V_AutoStockOut>> GetListByWorkOrderAsync(List<string> workorder)
        {
            return await _dbs.Queryable<V_AutoStockOut>().Where(x => workorder.Contains(x.WorkOrderNo)).ToListAsync();
        }

        public async Task<List<V_AutoStockOut>> GetListAsync()
        {
            return await _dbs.Queryable<V_AutoStockOut>().ToListAsync();
        }
    }
}
