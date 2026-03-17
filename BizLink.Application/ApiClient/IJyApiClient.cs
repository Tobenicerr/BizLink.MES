using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.ApiClient
{
    public interface IJyApiClient : IApiClient
    {
        Task<ApiResponse<T>> WmsPostAsync<T>(string requestUri, string jsonstr);
    }
}
