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
    public class CuttingParamViewRepository : GenericRepository<V_CableCutParam>, ICuttingParamViewRepository
    {

        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public CuttingParamViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }
        public async Task<List<V_CableCutParam>> GetBySemiMaterialCodeAsync(string semiMaterialCode)
        {
            return await _dbs.Queryable<V_CableCutParam>().Where(x => x.SemiMaterialCode == semiMaterialCode).ToListAsync();
        }
    }
}
