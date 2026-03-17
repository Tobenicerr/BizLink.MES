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
    public class FinishedGoodsReceiptRepository : GenericRepository<FinishedGoodsReceipt>, IFinishedGoodsReceiptRepository
    {
        public FinishedGoodsReceiptRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<FinishedGoodsReceipt>> GetListByWorkOrderProcessIdAsync(int processId)
        {
            return await _db.Queryable<FinishedGoodsReceipt>().Where(x => x.WorkOrderProcessId == processId).ToListAsync();
        }

        public async Task<List<FinishedGoodsReceipt>> GetListByWorkOrderProcessIdAsync(List<int> processId)
        {
            return await _db.Queryable<FinishedGoodsReceipt>().Where(x => processId.Contains(x.WorkOrderProcessId)).ToListAsync();
        }
    }
}
