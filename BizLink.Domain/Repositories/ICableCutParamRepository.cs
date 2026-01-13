using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface ICableCutParamRepository : IGenericRepository<CableCutParam>
    {
        Task<List<CableCutParam>> GetListBySimiMaterialCodeAsync(string semiMaterialCode);

        Task<bool> AddBatchAsync(List<CableCutParam> input);

        Task<bool> DeleteByIdsAsync(List<int> ids);

        Task<List<CableCutParam>> GetListBySimiMaterialCodeAsync(List<string> semiMaterialCodes);
    }
}
