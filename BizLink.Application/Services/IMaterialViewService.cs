using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IMaterialViewService
    {
        Task<MaterialViewDto> GetByCodeAsync(string factorycode, string materialcode);
        Task<List<MaterialViewDto>> GetListByCodesAsync(string factorycode, List<string> materialcodes);
    }
}
