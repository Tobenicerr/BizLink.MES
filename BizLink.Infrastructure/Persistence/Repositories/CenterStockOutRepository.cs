using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
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
    public class CenterStockOutRepository : GenericRepository<V_CenterStockOut>, ICenterStockOutRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public CenterStockOutRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }

        public async Task<List<V_CenterStockOut>> GetListAsync()
        {
            return await _dbs.Queryable<V_CenterStockOut>().ToListAsync();
        }

        public async Task<List<V_CenterStockOut>> GetListByWorkOrderAsync(string workorder)
        {
            return await _dbs.Queryable<V_CenterStockOut>().Where(x => x.WorkOrderNo == workorder).ToListAsync();
        }

        public async Task<List<V_CenterStockOut>> GetListByWorkOrderAsync(List<string> workorder)
        {
            return await _dbs.Queryable<V_CenterStockOut>().Where(x => workorder.Contains(x.WorkOrderNo)).ToListAsync();
        }
    }
}
