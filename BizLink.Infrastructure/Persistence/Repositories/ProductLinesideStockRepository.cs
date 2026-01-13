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
    public class ProductLinesideStockRepository : GenericRepository<ProductLinesideStock>, IProductLinesideStockRepository
    {
        public ProductLinesideStockRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<ProductLinesideStock>> GetListByOrderNoAsync(string orderno)
        {
            return await _db.Queryable<ProductLinesideStock>()
                .Where(p => p.WorkOrderNo == orderno)
                .ToListAsync();
        }

        public async Task<List<ProductLinesideStock>> GetListByOrderNoAsync(List<string> orderno)
        {
            return await _db.Queryable<ProductLinesideStock>().Where(x => orderno.Contains(x.WorkOrderNo)).ToListAsync();
        }

        public async Task<int> UpdateStatusAsync(List<ProductLinesideStock> productLinesideStocks)
        {
            return await _db.Updateable(productLinesideStocks).UpdateColumns(it => new { it.Status, it.UpdatedAt }).ExecuteCommandAsync();
        }
    }
}
