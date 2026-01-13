using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class CuttingParamViewService : ICuttingParamViewService
    {

        private readonly ICuttingParamViewRepository _cuttingParamViewRepository;
        public CuttingParamViewService(ICuttingParamViewRepository cuttingParamViewRepository)
        {
            _cuttingParamViewRepository = cuttingParamViewRepository;
        }
        public async Task<List<V_CableCutParam>> GetBySemiMaterialCodeAsync(string semiMaterialCode)
        {
            return await _cuttingParamViewRepository.GetBySemiMaterialCodeAsync(semiMaterialCode);
        }
    }
}
