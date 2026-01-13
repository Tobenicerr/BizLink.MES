using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ICuttingParamViewService
    {
        Task<List<V_CableCutParam>> GetBySemiMaterialCodeAsync(string semiMaterialCode);
    }
}
