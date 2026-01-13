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
    public class MaterialRepository : GenericRepository<Material>, IMaterialRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public MaterialRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }

        public async Task<int> CreateBatchAsync(List<Material> materials)
        {
            return await _dbs.Insertable(materials).ExecuteCommandAsync();
        }

        public async Task<List<Material>> GetListByMaterialCodesAsync(List<string> materialCodes)
        {
            return await _dbs.Queryable<Material>()
                                      .Where(m => materialCodes.Contains(m.MaterialCode)).ToListAsync();
        }

        public async Task<int> UpdateBatchAsync(List<Material> materials)
        {
            return await _dbs.Updateable(materials).ExecuteCommandAsync();
        }
    }
}
