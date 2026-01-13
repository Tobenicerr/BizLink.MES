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
    public class RawMaterialInventoryViewRepository : GenericRepository<V_RawMaterialInventory>, IRawMaterialInventoryViewRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public RawMaterialInventoryViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }

        public async Task<List<V_RawMaterialInventory>> GetListByMaterialCodeAsync(string factorycode, string materialCode)
        {
            return await _dbs.Queryable<V_RawMaterialInventory>().Where(x => x.FactoryCode == factorycode && x.MaterialCode == materialCode).ToListAsync();
        }

        public async Task<List<V_RawMaterialInventory>> GetListByMaterialCodeAsync(string factorycode, List<string> materialCode)
        {
            return await _dbs.Queryable<V_RawMaterialInventory>().Where(x => x.FactoryCode == factorycode && materialCode.Contains(x.MaterialCode)).ToListAsync();
        }
    }
}
