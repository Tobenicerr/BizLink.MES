using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IMaterialTransferLogRepository :IGenericRepository<MaterialTransferLog>
    {

        Task<MaterialTransferLog> GetByTransferNoAsync(string transferno, string materialcode, string batchcode);

        Task<List<MaterialTransferLog>> GetByTransferNoAsync(string transferno);

        Task<List<MaterialTransferLog>> GetByTransferNoAsync(List<string> transfernos);

        Task<List<MaterialTransferLog>> GetListByStatusAsync(string? keyword, string? status);

        Task<List<MaterialTransferLog>> GetListByIdsAsync(List<int> transferIds);

        Task<bool> UpdateListAsync(List<MaterialTransferLog> updateEntities);

        Task<List<MaterialTransferLog>> GetListByStockIdAsync(int stockid);

        Task<List<MaterialTransferLog>> GetListByStockLogIdAsync(List<int> stockid);

        Task<(List<MaterialTransferLog> transferLogs, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, string? keyword, string? status, DateTime? createdStart, DateTime? createdEnd);
    }
}
