using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services.ApiServices
{
    public interface ILineStockApiService
    {
        Task<RawLinesideStockDto> GetStockByBarcodeAsync(int factoryId, string barcode);

        Task<List<RawLinesideStockDto>> GetListByMaterialCodeAsync(int factoryId, string materialCode);

        Task<RawLinesideStockDto> GetNeedSyncStockBarcodeAsync(int factoryId, string barcode);

        Task<(bool, string)> TransferStockAsync(TransferStockRequest request);

        Task<(bool, string)> SyncStockToSapAsync(TransferStockRequest request);
    }
}
