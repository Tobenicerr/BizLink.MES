using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class MaterialLineStockViewService : IMaterialLineStockViewService
    {

        private readonly IMaterialLineStockViewRepository _materialLineStockViewRepository;
        public MaterialLineStockViewService(IMaterialLineStockViewRepository materialLineStockViewRepository)
        {
            _materialLineStockViewRepository = materialLineStockViewRepository;
        }
        public async Task<V_MaterialLineStock> GetByBarcodeAsync(string barcode)
        {
            return await _materialLineStockViewRepository.GetByBarcodeAsync(barcode);
        }
    }
}
