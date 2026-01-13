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
    internal class AssemblyOrderProgressRepository : GenericRepository<V_AssemblyOrderProgress>, IAssemblyOrderProgressRepository
    {
        public AssemblyOrderProgressRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<(List<V_AssemblyOrderProgress>, int totalCount)> GetPageListAsync(int pageIndex, int pageSize,string factoryCode, List<string>? orderNumber, List<string>? workCenter, DateTime? dispatchdateStart, DateTime? dispatchdateEnd, DateTime? confirmDateStart, DateTime? confirmDateEnd)
        {
            var query = _db.Queryable<V_AssemblyOrderProgress>().Where(v => v.FactoryCode == factoryCode)
                .WhereIF(orderNumber != null && orderNumber.Count() > 0, v => orderNumber.Contains(v.OrderNumber))
                .WhereIF(workCenter != null && workCenter.Count() > 0, v => workCenter.Contains(v.WorkCenter))
                .WhereIF(dispatchdateStart != null, v => v.DispatchDate >= dispatchdateStart)
                .WhereIF(dispatchdateEnd != null, v => v.DispatchDate <= dispatchdateEnd)
                .WhereIF(confirmDateStart != null, v => v.ConfirmDate >= confirmDateStart)
                .WhereIF(confirmDateEnd != null, v => v.ConfirmDate <= confirmDateEnd)
                .OrderBy(v => v.Id);


            var totalCount = await query.CountAsync();
            var list = await query.ToPageListAsync(pageIndex, pageSize);

            return (list, totalCount);
        }
    }
}
