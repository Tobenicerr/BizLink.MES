using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.DbContext;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class MaterialLineStockViewRepository : GenericRepository<V_MaterialLineStock>, IMaterialLineStockViewRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public MaterialLineStockViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }
        public async Task<V_MaterialLineStock> GetByBarcodeAsync(string barcode)
        {
            var query = await _dbs.Queryable<V_MaterialLineStock>()
                .Where(x => x.BarCode.Equals(barcode))
                .SingleAsync();
            return query;
        }
    }
}
