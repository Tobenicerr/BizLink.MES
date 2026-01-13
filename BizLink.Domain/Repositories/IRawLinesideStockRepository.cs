using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IRawLinesideStockRepository : IGenericRepository<RawLinesideStock>
    {

        Task<List<RawLinesideStock>> GetListByMaterialCodeAsync(int factoryid,string materialcode);


        Task<List<RawLinesideStock>> GetListByMaterialCodeAsync(int factoryid, List<string> materialcode);
        Task<List<RawLinesideStock>> GetAllAsync(int factoryid);

        Task<RawLinesideStock> GetByBarCodeAsync(int factoryid, string barcode);

        Task<List<RawLinesideStock>> GetByBarCodeAsync(int factoryid, List<string> barcode);

        Task<List<RawLinesideStock>> GetListByIdsAsync(List<int> ids);

        Task<int> BatchUpdateAsync(List<RawLinesideStock> input);

        Task<(List<RawLinesideStock>, int totalCount)> GetBatchPageListAsync(int pageIndex, int pageSize, int factoryid, string? keyword, bool quantitySwitch = true, List<string>? materialcodes = null, List<string>? batchcodes = null);


    }
}
