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
    public class MaterialViewRepository : GenericRepository<V_Material>, IMaterialViewRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public MaterialViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }

        public async Task<V_Material> GetByCodeAsync(string factorycode, string materialcode)
        {
            return await _dbs.Queryable<V_Material>().Where(x => x.FactoryCode.Equals(factorycode) && x.MaterialCode.Equals(materialcode)).FirstAsync();
        }

        public async Task<List<V_Material>> GetListByCodesAsync(string factorycode, List<string> materialcodes)
        {
            return await _dbs.Queryable<V_Material>()
                                      .Where(v =>v.FactoryCode.Equals(factorycode) && materialcodes.Contains(v.MaterialCode)).ToListAsync();
        }
    }
}
