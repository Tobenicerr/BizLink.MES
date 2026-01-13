using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWmsMaterialStockRepository : IGenericRepository<V_WmsMaterialStock>
    {
        Task<(List<V_WmsMaterialStock>, int totalCount)> GetPageListAsync(string factoryCode, int pageIndex, int pageSize, string? keyword, List<string>? materialcodes, List<string>? batchcodes);

        Task<(List<V_WmsMaterialStock>, int totalCount)> GetBatchPageListAsync(int pageIndex, int pageSize, string factoryCode, string? keyword, List<string> materialcodes, List<string>? batchcodes, string? consumetype);
    }
}
