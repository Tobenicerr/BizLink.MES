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
    public interface IMaterialLineStockViewRepository : IGenericRepository<V_MaterialLineStock>
    {
        Task<V_MaterialLineStock> GetByBarcodeAsync(string barcode);
    }
}
