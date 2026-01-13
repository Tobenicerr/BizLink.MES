using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class RawLinesideStockLogRepository : GenericRepository<RawLinesideStockLog>, IRawLinesideStockLogRepository
    {
        public RawLinesideStockLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<RawLinesideStockLog>> GetListByKeywordAsync(List<string> keywords)
        {
            return await _db.Queryable<RawLinesideStockLog>()
                                      .Where(m => keywords.Contains(m.MaterialCode) || keywords.Contains(m.BatchCode))
                                      .OrderByDescending(m => m.CreateOn)
                                      .ToListAsync();
        }
    }
}
