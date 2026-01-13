using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class MaterialTransferLogRepository : GenericRepository<MaterialTransferLog>, IMaterialTransferLogRepository
    {
        public MaterialTransferLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<MaterialTransferLog> GetByTransferNoAsync(string transferno,string materialcode, string batchcode)
        {
            return await _db.Queryable<MaterialTransferLog>()
                            .Where(m => m.TransferNo == transferno && m.MaterialCode == materialcode && m.BatchCode == batchcode)
                            .FirstAsync();
        }

        public async Task<List<MaterialTransferLog>> GetByTransferNoAsync(string transferno)
        {
            return await _db.Queryable<MaterialTransferLog>()
                            .Where(m => m.TransferNo == transferno)
                            .ToListAsync();
        }

        public async Task<List<MaterialTransferLog>> GetByTransferNoAsync(List<string> transfernos)
        {
            return await _db.Queryable<MaterialTransferLog>().Where(x => transfernos.Contains(x.TransferNo)).ToListAsync();
        }

        public async Task<List<MaterialTransferLog>> GetListByIdsAsync(List<int> transferIds)
        {
            return await _db.Queryable<MaterialTransferLog>()
                            .Where(m => transferIds.Contains(m.Id))
                            .ToListAsync();
        }

        public async Task<List<MaterialTransferLog>> GetListByStatusAsync(string? keyword, string? status)
        {
            return await _db.Queryable<MaterialTransferLog>().With(SqlWith.NoLock)
                            .WhereIF(!string.IsNullOrEmpty(keyword), m => m.TransferNo.Contains(keyword) || m.MaterialCode.Contains(keyword) || m.BatchCode.Contains(keyword))
                            .WhereIF(!string.IsNullOrEmpty(status), m => m.Status == status)
                            .OrderBy(x => x.Id)
                            .ToListAsync();
        }

        public async Task<List<MaterialTransferLog>> GetListByStockIdAsync(int stockid)
        {
            return await _db.Queryable<MaterialTransferLog>().Where(x => x.FromStockId == stockid).ToListAsync();
        }

        public async Task<List<MaterialTransferLog>> GetListByStockLogIdAsync(List<int> stockid)
        {
            return await _db.Queryable<MaterialTransferLog>().Where(x => stockid.Contains((int)x.StockLogId)).ToListAsync();
        }

        public async Task<(List<MaterialTransferLog> transferLogs, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, string? keyword, string? status, DateTime? createdStart, DateTime? createdEnd)
        {
            var query =  _db.Queryable<MaterialTransferLog>().With(SqlWith.NoLock)
                            .WhereIF(!string.IsNullOrEmpty(keyword), m => m.TransferNo.Contains(keyword) || m.MaterialCode.Contains(keyword) || m.BatchCode.Contains(keyword) || m.ToLocationCode.Contains(keyword) || m.BaseUnit.Contains(keyword))
                            .WhereIF(!string.IsNullOrEmpty(status), m => m.Status == status)
                            .WhereIF(createdStart != null, m => m.CreatedAt >= createdStart)
                            .WhereIF(createdEnd != null, m => m.CreatedAt <= createdEnd)
                            .OrderBy(x => x.Id);

            var totalCount = await query.CountAsync();
            var Logs = await query.ToPageListAsync(pageIndex, pageSize);

            return (Logs, totalCount);
        }

        public async Task<bool> UpdateListAsync(List<MaterialTransferLog> updateEntities)
        {
           var result =  await _db.Updateable(updateEntities).ExecuteCommandAsync();
            return result > 0 && result == updateEntities.Count;
        }
    }
}
