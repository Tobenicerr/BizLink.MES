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
    public interface IRawLinesideStockService : IGenericService<RawLinesideStockDto, RawLinesideStockCreateDto, RawLinesideStockUpdateDto>
    {
        Task<List<RawLinesideStockDto>> GetAllAsync(int factoryid);

        Task<PagedResultDto<RawLinesideStockDto>> GetBatchPageListAsync(int pageIndex, int pageSize, int factoryid, string? keyword, bool quantitySwitch = true, List<string>? materialcodes = null, List<string>? batchcodes = null);

        Task<List<RawLinesideStockDto>> GetListByMaterialCodeAsync(int factoryid, string materialcode);

        Task<List<RawLinesideStockDto>> GetListByMaterialCodeAsync(int factoryid, List<string> materialcode);

        Task<RawLinesideStockDto> GetByBarCodeAsync(int factoryid, string barcode);

        Task<List<RawLinesideStockDto>> GetByBarCodeAsync(int factoryid, List<string> barcode);

        Task<int> BatchUpdateAsync(List<RawLinesideStockUpdateDto> updateDtos);
    }
}
