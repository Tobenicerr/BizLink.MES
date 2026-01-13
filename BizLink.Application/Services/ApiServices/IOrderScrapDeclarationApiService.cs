using Azure.Core;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services.ApiServices
{
    public interface IOrderScrapDeclarationApiService
    {
        Task<List<SapOrderScrapDeclarationDto>> GetOrderScrapDeclarationsAsync(SapOrderScrapDeclarationRequest request);
    }
}
