using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IRawLinesideStockLogService : IGenericService<RawLinesideStockLogDto, RawLinesideStockLogCreateDto, RawLinesideStockLogUpdateDto>
    {

        Task<List<RawLinesideStockLogDto>> GetListByKeywordAsync(List<string> keywords);
    }
}
