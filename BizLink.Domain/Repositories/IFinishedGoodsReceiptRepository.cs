using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IFinishedGoodsReceiptRepository : IGenericRepository<FinishedGoodsReceipt>
    {
        Task<List<FinishedGoodsReceipt>> GetListByWorkOrderProcessIdAsync(int processId);

        Task<List<FinishedGoodsReceipt>> GetListByWorkOrderProcessIdAsync(List<int> processId);
    }
}
