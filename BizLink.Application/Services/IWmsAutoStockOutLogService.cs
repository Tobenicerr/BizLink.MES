using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWmsAutoStockOutLogService : IGenericService<WmsAutoStockOutLogDto, WmsAutoStockOutLogCreateDto, WmsAutoStockOutLogUpdateDto>
    {
        Task<List<WmsAutoStockOutLogDto>> GetFailListAsync(string factoryCode,string? keyword = null);
    }
}
