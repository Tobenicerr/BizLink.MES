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
    public class AutoMaterialStockRepository : GenericRepository<V_AutoMaterialStock>, IAutoMaterialStockRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public AutoMaterialStockRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("FosiConnection");
        }

        public async Task<List<V_AutoMaterialStock>> GetListByMaterialCodeAsync(List<string> materialcodes)
        {
            return await _dbs.Queryable<V_AutoMaterialStock>().Where(a => materialcodes.Contains(a.MaterialCode) && a.Quantity > 0 && a.IsLocked == false).ToListAsync();
        }
    }
}
