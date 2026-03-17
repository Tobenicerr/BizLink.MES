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
    public class WmsAutoStockInLogRepository : GenericRepository<WmsAutoStockInLog>, IWmsAutoStockInLogRepository
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public WmsAutoStockInLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }

        public async Task<List<WmsAutoStockInLog>> GetFailListAsync(string factoryCode, string? keyword = null)
        {
            return await _dbs.Queryable<WmsAutoStockInLog>()
                .Where(x => x.FactoryCode == factoryCode && x.ProcessFlag != 1)
                .WhereIF(!string.IsNullOrWhiteSpace(keyword), x => x.BillNo.Contains(keyword) || x.MaterialCode.Contains(keyword) || x.BatchCode.Contains(keyword))
                .OrderByDescending(x => x.CreateTime)
                .ToListAsync();
        }

        public override async Task<bool> UpdateAsync(WmsAutoStockInLog input)
        {
            return await _dbs.Updateable(input).ExecuteCommandHasChangeAsync();
        }

        public override async Task<WmsAutoStockInLog> GetByIdAsync(int id)
        {
            return await _dbs.Queryable<WmsAutoStockInLog>().InSingleAsync(id);
        }
    }
}
