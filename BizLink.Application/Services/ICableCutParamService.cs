using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ICableCutParamService : IGenericService<CableCutParamDto, CableCutParamCreateDto, CableCutParamUpdateDto>
    {
        Task<List<CableCutParamDto>> GetListBySimiMaterialCodeAsync(string semiMaterialCode);

        Task<bool> CreateBatchAsync(List<CableCutParamCreateDto> input);

        Task<List<CableCutParamDto>> GetListBySimiMaterialCodeAsync(List<string> semiMaterialCode);

        Task<bool> DeleteByIdsAsync(List<int> ids);
    }
}
