using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public  interface IMaterialLineStockViewService
    {
        Task<V_MaterialLineStock> GetByBarcodeAsync(string barcode);
    }
}
