using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ICenterStockOutService : IGenericService<CenterStockOutDto, CenterStockOutCreateDto, CenterStockOutUpdateDto>
    {
        Task<List<CenterStockOutDto>> GetListByWorkOrderAsync(string workorder);

        Task<List<CenterStockOutDto>> GetListByWorkOrderAsync(List<string> workorder);
    }
}
