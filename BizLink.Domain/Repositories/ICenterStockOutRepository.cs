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
    public interface ICenterStockOutRepository : IGenericRepository<V_CenterStockOut>
    {
        Task<List<V_CenterStockOut>> GetListByWorkOrderAsync(string workorder);

        Task<List<V_CenterStockOut>> GetListByWorkOrderAsync(List<string> workorder);

        Task<List<V_CenterStockOut>> GetListAsync();
    }
}
