using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public  interface IProductLinesideStockService : IGenericService<ProductLinesideStockDto, ProductLinesideStockCreateDto, ProductLinesideStockUpdateDto>
    {

        Task<List<ProductLinesideStockDto>> GetListByOrderNoAsync(string orderno);
        Task<List<ProductLinesideStockDto>> GetListByOrderNoAsync(List<string> orderno);


        Task<int> UpdateStatusAsync(List<ProductLinesideStockUpdateDto> updateDtos);
    }
}
