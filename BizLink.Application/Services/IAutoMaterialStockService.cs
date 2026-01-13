using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IAutoMaterialStockService : IGenericService<AutoMaterialStockDto, AutoMaterialStockCreateDto, AutoMaterialStockUpdateDto>
    {
        Task<List<AutoMaterialStockDto>> GetListByMaterialCodeAsync(List<string> materialcodes);
    }
}
