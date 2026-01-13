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
    public interface ISapOrderScrapDeclarationRepository : IGenericRepository<SapOrderScrapDeclaration>
    {
        Task<List<SapOrderScrapDeclaration>> GetListByOperationAsync(string workOrderNo, string operationNo);

        Task<List<SapOrderScrapDeclaration>> GetListByOrderNosAsync(List<string> workOrderNos);

        Task<List<SapOrderScrapDeclaration>> GetListByWorkCenterAsync(string factorycode, List<string>? workcentercodes, DateTime? startdate, DateTime? enddate, List<string>? workorders);

        Task<(List<SapOrderScrapDeclaration>, int)> GetPageListAsync(int pageIndex, int pageSize, string factoryCode, string? keyword, DateTime? createdDate);
    }
}
