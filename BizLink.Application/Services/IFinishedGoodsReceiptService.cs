using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IFinishedGoodsReceiptService : IGenericService<FinishedGoodsReceiptDto, FinishedGoodsReceiptCreateDto, FinishedGoodsReceiptUpdateDto>
    {

        Task<List<FinishedGoodsReceiptDto>> GetListByWorkOrderProcessIdAsync(int processId);

        Task<List<FinishedGoodsReceiptDto>> GetListByWorkOrderProcessIdAsync(List<int> processIds);
    }
}
