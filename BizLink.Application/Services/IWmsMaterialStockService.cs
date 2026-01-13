using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.WinForms.Common;
using System;
using System.Collections.Generic;
using System.Data.OscarClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWmsMaterialStockService : IGenericService<WmsMaterialStockDto, WmsMaterialStockCreateDto, WmsMaterialStockUpdateDto>
    {
        Task<PagedResultDto<WmsMaterialStockDto>> GetPageListAsync(string factoryCode,int pageIndex,int pageSize,string? keyword,List<string>? materialcodes,List<string>? batchcodes);

        Task<PagedResultDto<WmsMaterialStockDto>> GetBatchPageListAsync(int pageIndex, int pageSize, string factoryCode, string? keyword, List<string> materialcodes, List<string>? batchcodes,string? consumetype);
    }
}
