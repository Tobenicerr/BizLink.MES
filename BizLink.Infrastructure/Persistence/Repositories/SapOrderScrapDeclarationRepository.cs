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
    public class SapOrderScrapDeclarationRepository : GenericRepository<SapOrderScrapDeclaration>, ISapOrderScrapDeclarationRepository
    {
        public SapOrderScrapDeclarationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<SapOrderScrapDeclaration>> GetListByOperationAsync(string workOrderNo, string operationNo)
        {
            return await _db.Queryable<SapOrderScrapDeclaration>().Where(x => x.WorkOrderNo == workOrderNo && x.OperationNo == operationNo && x.IsActive).ToListAsync();
        }

        public async Task<List<SapOrderScrapDeclaration>> GetListByOrderNosAsync(List<string> workOrderNos)
        {
            return await _db.Queryable<SapOrderScrapDeclaration>().Where(x => workOrderNos.Contains(x.WorkOrderNo) && x.IsActive).ToListAsync();
        }

        public async Task<List<SapOrderScrapDeclaration>> GetListByWorkCenterAsync(string factorycode, List<string>? workcentercodes, DateTime? startdate, DateTime? enddate, List<string>? workorders)
        {
            if (workorders != null && workorders.Count > 0)
            {
                return await _db.Queryable<SapOrderScrapDeclaration>().Where(x => x.FactoryCode == factorycode && x.IsActive).WhereIF(workorders != null && workorders.Count > 0, x => workorders.Contains(x.WorkOrderNo)).ToListAsync();
            }
            else
            {
                return await _db.Queryable<SapOrderScrapDeclaration>()
                    .Where(x => x.IsActive && x.FactoryCode == factorycode).WhereIF(workcentercodes != null && workcentercodes.Count > 0, x => workcentercodes.Contains(x.WorkCenterCode))
                    .WhereIF(startdate != null, x => x.CreatedOn >= startdate)
                    .WhereIF(enddate != null, x => x.CreatedOn <= enddate)
                    .ToListAsync();
            }
        }

        public async Task<(List<SapOrderScrapDeclaration>, int)> GetPageListAsync(int pageIndex, int pageSize, string factoryCode, string? keyword, DateTime? createdDate)
        {
            var query = _db.Queryable<SapOrderScrapDeclaration>().Where(s => s.FactoryCode == factoryCode && s.IsActive).WhereIF(!string.IsNullOrWhiteSpace(keyword),s => s.WorkOrderNo.Contains(keyword) || s.WorkCenterCode.Contains(keyword) || s.ScrapMaterialCode.Contains(keyword) || s.CreatedBy.Contains(keyword))
                .WhereIF(createdDate != null, s => ((DateTime)s.CreatedOn).Date == ((DateTime)createdDate).Date).OrderByDescending(s => s.Id);

            var count = await query.CountAsync();
            var list = await query.ToPageListAsync(pageIndex, pageSize);
            return (list, count);
        }
    }
}
