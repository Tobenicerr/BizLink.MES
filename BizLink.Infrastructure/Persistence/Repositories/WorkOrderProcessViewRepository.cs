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
    public class WorkOrderProcessViewRepository : GenericRepository<V_WorkOrderProcess>, IWorkOrderProcessViewRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ISqlSugarClient _dbs;
        public WorkOrderProcessViewRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _dbs = _unitOfWork.GetDbClient("JyConnection");
        }
        public async Task<(IEnumerable<V_WorkOrderProcess> Processes, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, int orderId)
        {
            // 查询视图就像查询一个普通的表一样
            var query = _dbs.Queryable<V_WorkOrderProcess>()
                            .WhereIF(orderId > 0, v => v.WorkOrderId == orderId)
                            .OrderBy(v => v.OperationId);

            var totalCount = await query.CountAsync();
            var processes = await query.ToPageListAsync(pageIndex, pageSize);

            return (processes, totalCount);
        }

        public async Task<V_WorkOrderProcess> GetByOperationIdAsync(int id)
        {
            return await _dbs.Queryable<V_WorkOrderProcess>().Where(x => x.OperationId == id).SingleAsync();
        }


        /// <summary>
        /// 临时方法，更新检验任务状态。待注释
        /// </summary>
        /// <param name="orderno"></param>
        /// <param name="bomitem"></param>
        /// <param name="batchcode"></param>
        /// <returns></returns>
        public async Task UpdateJyTaskAsync(string orderno, string bomitem, string batchcode,string barcode)
        {
            await _dbs.Ado.UseStoredProcedure().ExecuteCommandAsync("P_Biz_UpdateJyCuttingTask", new
            {
                OrderNo = orderno,
                BomItem = bomitem,
                BatchCode = batchcode,
                BarCode = barcode
            });
        }
    }
}
