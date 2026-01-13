using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.DbContext;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using Dm;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkOrderViewRepository : GenericRepository<V_WorkOrder>, IWorkOrderViewRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public WorkOrderViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }

        public async Task<V_WorkOrder> GetByOrderIdAsync(int orderid)
        {
            return await _dbs.Queryable<V_WorkOrder>()
                             .Where(v => v.Id == orderid)
                             .FirstAsync();
        }
        public async Task<(IEnumerable<V_WorkOrder> Orders, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, string keyword, DateTime? dispatchDate, DateTime? startDate,string factorycode)
        {
            // 查询视图就像查询一个普通的表一样
            var query = _dbs.Queryable<V_WorkOrder>()
                            .WhereIF(!string.IsNullOrEmpty(keyword), v => v.OrderNumber.Contains(keyword))
                            .WhereIF(dispatchDate.HasValue, v => v.DispatchDate == dispatchDate.Value)
                            .WhereIF(startDate.HasValue, v => v.StartTime == startDate.Value)
                            .WhereIF(!string.IsNullOrEmpty(factorycode), v => v.FactoryCode == factorycode)
                            .OrderBy(v => v.DispatchDate, OrderByType.Desc);

            var totalCount = await query.CountAsync();
            var orders = await query.ToPageListAsync(pageIndex, pageSize);

            return (orders, totalCount);
        }

        public async Task UpdateJyWmsRequrementTaskAsync()
        {
            await _dbs.Ado.ExecuteCommandAsync(@"UPDATE WMSTask set BILLSTATUS = '关闭',VALIDUSER = 'Client',VALIDTIME = GETDATE() where BILLTYPE = '断线领料' and PLANT = 'CN11' and BILLSTATUS in ('未执行','执行中')");
        }

        public async Task<int> GetPickMtrStockByWorkOrderAsync(string workorderno)
        {
            return await _dbs.Ado.UseStoredProcedure().ExecuteCommandAsync("P_SyncWMSStockByOrderNoToBizMes", new SugarParameter("@orderNo", workorderno));
        }
    }
}
