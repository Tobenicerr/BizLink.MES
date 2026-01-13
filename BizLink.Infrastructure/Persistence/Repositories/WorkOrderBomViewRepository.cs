using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.DbContext;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderBomViewRepository : GenericRepository<V_WorkOrderBom>, IWorkOrderBomViewRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public WorkOrderBomViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }

        public async Task<List<V_WorkOrderBom>> GetListByOrderIdAsync(int orderid)
        {
            return await _dbs.Queryable<V_WorkOrderBom>()
                                      .Where( v => v.WorkOrderId == orderid).ToListAsync();
        }

        public async Task<(List<V_WorkOrderBom> boms, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, int orderId)
        {
            // 查询视图就像查询一个普通的表一样
            var pageModel = await _dbs.Queryable<V_WorkOrderBom>()
                                      .WhereIF(orderId > 0, v => v.WorkOrderId == orderId)
                                      .ToPageListAsync(pageIndex, pageSize);
            return (pageModel, pageModel.Count);
        }
    }
}
