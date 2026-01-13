using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IProductLinesideStockRepository : IGenericRepository<ProductLinesideStock>
    {
        Task<List<ProductLinesideStock>> GetListByOrderNoAsync(string orderno);

        Task<List<ProductLinesideStock>> GetListByOrderNoAsync(List<string> orderno);

        Task<int> UpdateStatusAsync(List<ProductLinesideStock> productLinesideStocks);

    }
}
