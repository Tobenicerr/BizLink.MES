using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface ICuttingParamViewRepository : IGenericRepository<V_CableCutParam>
    {
        Task<List<V_CableCutParam>> GetBySemiMaterialCodeAsync(string semiMaterialCode);
    }
}
