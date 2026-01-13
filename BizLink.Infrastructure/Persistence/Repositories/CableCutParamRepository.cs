using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class CableCutParamRepository : GenericRepository<CableCutParam>, ICableCutParamRepository
    {
        public CableCutParamRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public  async Task<bool> AddBatchAsync(List<CableCutParam> input)
        {
            
            return (await _db.Insertable(input).ExecuteCommandAsync()) >0;
        }

        public async Task<bool> DeleteByIdsAsync(List<int> ids)
        {
            return (await _db.Deleteable<CableCutParam>(ids).ExecuteCommandAsync()) > 0;
        }

        public async Task<List<CableCutParam>> GetListBySimiMaterialCodeAsync(string semiMaterialCode)
        {
           return await _db.Queryable<CableCutParam>().Where(x=> x.SemiMaterialCode == semiMaterialCode).ToListAsync();
        }

        public async Task<List<CableCutParam>> GetListBySimiMaterialCodeAsync(List<string> semiMaterialCodes)
        {
            return await _db.Queryable<CableCutParam>().Where(x => semiMaterialCodes.Contains(x.SemiMaterialCode)).ToListAsync();
        }
    }
}
