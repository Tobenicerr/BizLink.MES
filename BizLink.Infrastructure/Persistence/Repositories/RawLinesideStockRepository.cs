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
    internal class RawLinesideStockRepository : GenericRepository<RawLinesideStock>, IRawLinesideStockRepository
    {
        public RawLinesideStockRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<RawLinesideStock>> GetListByMaterialCodeAsync(int factoryid,string materialcode)
        {
            return await _db.Queryable<RawLinesideStock>().LeftJoin<WorkOrderTaskMaterialAdd>((s,t) => s.BarCode == t.BarCode && t.Status == "1").Where((s, t) => s.Status == "1" && s.FactoryId == factoryid && s.MaterialCode == materialcode).Distinct().Select((s,t) => new RawLinesideStock() 
            {
                Id = s.Id,
                FactoryId = s.FactoryId,
                MaterialCode = s.MaterialCode,
                MaterialDesc = s.MaterialDesc,
                BaseUnit = s.BaseUnit,
                BarCode = s.BarCode,
                BatchCode = s.BatchCode,
                Quantity = s.Quantity,
                LastQuantity = s.LastQuantity,
                Status = t.BarCode == null ? "1" : "2",
                SapStatus = s.SapStatus,
                LocationCode = s.LocationCode,
                LocationId = s.LocationId,
                LocationDesc = s.LocationDesc,
                CreatedAt = s.CreatedAt,
                CreatedBy = s.CreatedBy,
                UpdateBy = s.UpdateBy,
                UpdatedAt = s.UpdatedAt

            }).ToListAsync();

        }

        public async Task<List<RawLinesideStock>> GetAllAsync(int factoryid)
        {
            return await _db.Queryable<RawLinesideStock>().With(SqlWith.NoLock).Where(x => x.FactoryId == factoryid).ToListAsync();
        }

        public async Task<RawLinesideStock> GetByBarCodeAsync(int factoryid, string barcode)
        {
            return (await _db.Queryable<RawLinesideStock>().Where(x => x.FactoryId == factoryid && x.BarCode == barcode && x.LastQuantity >0).ToListAsync()).FirstOrDefault();
        }

        public async Task<List<RawLinesideStock>> GetListByMaterialCodeAsync(int factoryid, List<string> materialcode)
        {
            return await _db.Queryable<RawLinesideStock>().Where(x => x.FactoryId == factoryid && materialcode.Contains(x.MaterialCode) && x.LastQuantity > 0 ).ToListAsync();
        }

        public async Task<List<RawLinesideStock>> GetListByIdsAsync(List<int> ids)
        {
            return await _db.Queryable<RawLinesideStock>().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<int> BatchUpdateAsync(List<RawLinesideStock> input)
        {
            return await _db.Updateable(input).ExecuteCommandAsync();
        }

        public async Task<List<RawLinesideStock>> GetByBarCodeAsync(int factoryid, List<string> barcode)
        {
            return await _db.Queryable<RawLinesideStock>().Where(x => x.FactoryId == factoryid && barcode.Contains(x.BarCode) && x.LastQuantity > 0).ToListAsync();
        }

        public async Task<(List<RawLinesideStock>, int totalCount)> GetBatchPageListAsync(int pageIndex, int pageSize, int factoryid, string? keyword, bool quantitySwitch = true, List<string>? materialcodes = null, List<string>? batchcodes = null)
        {
            var query = _db.Queryable<RawLinesideStock>().GroupBy(x => new { x.MaterialCode,x.BatchCode})
                .Where(x => x.FactoryId == factoryid)
                .WhereIF(!string.IsNullOrEmpty(keyword), x => x.MaterialCode.Contains(keyword) || x.BatchCode.Contains(keyword))
                .WhereIF(quantitySwitch, x => x.LastQuantity > 0)
                .WhereIF(!quantitySwitch, x => x.LastQuantity <= 0)
                .WhereIF(materialcodes != null && materialcodes.Count > 0, x => materialcodes.Contains(x.MaterialCode))
                .WhereIF(batchcodes != null && batchcodes.Count > 0, x => batchcodes.Contains(x.BatchCode))
                .Select( x => new RawLinesideStock() 
                {
                    MaterialCode = x.MaterialCode,
                    MaterialDesc = SqlFunc.AggregateMax(x.MaterialDesc),
                    BaseUnit = SqlFunc.AggregateMax(x.BaseUnit),
                    BatchCode = x.BatchCode,
                    Quantity = SqlFunc.AggregateSum(x.Quantity),
                    LocationDesc = SqlFunc.AggregateMax(x.LocationDesc),
                    LastQuantity = SqlFunc.AggregateSum(x.LastQuantity)
                });

            var totalCount = await query.CountAsync();
            var list = await query.OrderBy(x => x.MaterialCode)
                .OrderBy(x => x.BatchCode).ToPageListAsync(pageIndex, pageSize);
            return (list, totalCount);
        }
    }
}
