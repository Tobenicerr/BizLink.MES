using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{

    public interface IFactoryRepository : IGenericRepository<Factory>
    {

        Task<(IEnumerable<Factory> Factorys, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, string keyword, bool? isActive);

    }
}
