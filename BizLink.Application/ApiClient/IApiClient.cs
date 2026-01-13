using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.ApiClient
{
    public interface IApiClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string requestUri);
        Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string requestUri, TRequest data);
        Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string requestUri, TRequest data);
        Task<ApiResponse<T>> DeleteAsync<T>(string requestUri);

        Task<ApiResponse<T>> PostAsync<T>(string requestUri, String jsonstr);

        Task<ApiResponse<TResponse>> UploadFileAsync<TResponse>(string url, string filePath, Dictionary<string, string> additionalFormData = null);

    }
}
