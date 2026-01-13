using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
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
    public class WmsMaterialStockRepository : GenericRepository<V_WmsMaterialStock>, IWmsMaterialStockRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public WmsMaterialStockRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }

        public async Task<(List<V_WmsMaterialStock>, int totalCount)> GetBatchPageListAsync(int pageIndex, int pageSize, string factoryCode, string? keyword, List<string> materialcodes, List<string>? batchcodes, string? consumetype)
        {
            var query = _dbs.Queryable<V_WmsMaterialStock, Material>((w, m) => w.FactoryCode == m.FactoryCode && w.MaterialCode == m.MaterialCode)
                .GroupBy((w, m) => new { w.MaterialCode, w.BatchCode, m.ConsumeType })
               .Where((w, m) => w.FactoryCode == factoryCode  && m.IsDelete == false)
               .WhereIF(materialcodes != null && materialcodes.Count() > 0, (w, m) => materialcodes.Contains(w.MaterialCode))
               .WhereIF(batchcodes != null && batchcodes.Count() > 0, (w, m) => batchcodes.Contains(w.BatchCode))
               .WhereIF(!string.IsNullOrWhiteSpace(consumetype), (w, m) => m.ConsumeType ==  Convert.ToInt32(consumetype))
               .WhereIF(!string.IsNullOrEmpty(keyword), (w, m) => w.MaterialCode.Contains(keyword) || w.MaterialDesc.Contains(keyword) || w.BatchCode.Contains(keyword) || w.StockCode.Contains(keyword) || w.ShelveCode.Contains(keyword) || w.ShelveName.Contains(keyword) || w.StockName.Contains(keyword) || w.LocationName.Contains(keyword)
               ).OrderBy((w, m) => SqlFunc.IsNull(m.ConsumeType,99)).OrderBy((w, m) => w.MaterialCode).OrderBy((w, m) => w.BatchCode).Select((w, m) => new V_WmsMaterialStock()
               {
                   MaterialCode = w.MaterialCode,
                   MaterialDesc = SqlFunc.AggregateMax(w.MaterialDesc),
                   BaseUnit = SqlFunc.AggregateMax(w.BaseUnit),
                   BatchCode = w.BatchCode,
                   ConsumeType = m.ConsumeType,
                   UsageQuantity = SqlFunc.AggregateSum(w.UsageQuantity),
                   DownQuantity = SqlFunc.AggregateSum(w.DownQuantity),
                   LockQuantity = SqlFunc.AggregateSum(w.LockQuantity)
               });

            var totalCount = await query.CountAsync();
            var List = await query.ToPageListAsync(pageIndex, pageSize);

            return (List, totalCount);
        }

        public async Task<(List<V_WmsMaterialStock>, int totalCount)> GetPageListAsync(string factoryCode, int pageIndex, int pageSize, string? keyword, List<string>? materialcodes, List<string>? batchcodes)
        {
            var query = _dbs.Queryable<V_WmsMaterialStock>().Where(x => x.FactoryCode == factoryCode && x.UsageQuantity > 0)
                .WhereIF(materialcodes !=null && materialcodes.Count() > 0,x => materialcodes.Contains(x.MaterialCode))
                .WhereIF(batchcodes != null && batchcodes.Count() > 0, x => batchcodes.Contains(x.BatchCode))
                .WhereIF(!string.IsNullOrEmpty(keyword), x => x.MaterialCode.Contains(keyword) || x.MaterialDesc.Contains(keyword) || x.BarCode.Contains(keyword) || x.BatchCode.Contains(keyword) || x.StockCode.Contains(keyword) || x.ShelveCode.Contains(keyword) || x.ShelveName.Contains(keyword) || x.StockName.Contains(keyword) || x.LocationName.Contains(keyword)
                ).OrderBy(x => x.MaterialCode).OrderBy(x => x.BatchCode);

            var totalCount = await query.CountAsync();
            var List = await query.ToPageListAsync(pageIndex, pageSize);

            return (List, totalCount);
        }
    }
}
