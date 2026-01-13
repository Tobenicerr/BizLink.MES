using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IAssemblyOrderProgressRepository : IGenericRepository<V_AssemblyOrderProgress>
    {
        Task<(List<V_AssemblyOrderProgress>, int totalCount)> GetPageListAsync(int pageIndex, int pageSize, string factoryCode, List<string>? orderNumber, List<string>? workCenter, DateTime? dispatchdateStart, DateTime? dispatchdateEnd, DateTime? confirmDateStart, DateTime? confirmDateEnd);
    }
}
