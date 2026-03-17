using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BizLink.MES.Application.ApiClient
{
    public class ApiClient : IApiClient, IMesApiClient, IJyApiClient,IBartApiClient
    {
        private readonly HttpClient _httpClient;

        // =====================================================================
        // 关键：确保这个公共构造函数存在。
        // 依赖注入系统会通过它来传入配置好的 HttpClient 实例。
        // =====================================================================
        public ApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ApiResponse<T>> GetAsync<T>(string requestUri)
        {
            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<T>.Fail($"请求发生异常: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string requestUri, TRequest data)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(requestUri, data);
                return await HandleResponse<TResponse>(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<TResponse>.Fail($"请求发生异常: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string requestUri, TRequest data)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(requestUri, data);
                return await HandleResponse<TResponse>(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<TResponse>.Fail($"请求发生异常: {ex.Message}");
            }
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string requestUri)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(requestUri);
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<T>.Fail($"请求发生异常: {ex.Message}");
            }
        }

        private async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                // 创建一个包含忽略大小写选项的 JsonSerializerOptions 实例
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // 使用这个选项来反序列化
                var apiResponse = System.Text.Json.JsonSerializer.Deserialize<ApiResponse<T>>(jsonString, options);

                return apiResponse;
            }
            else
            {
                // 对于失败的请求, 返回一个包含错误信息的 Fail 响应
                var errorContent = await response.Content.ReadAsStringAsync();
                return ApiResponse<T>.Fail($"API 请求失败。状态码: {(int)response.StatusCode}。错误: {errorContent}");
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string requestUri, string jsonstr)
        {
            try
            {

                // 1. 创建一个 StringContent 对象
                //    - 第一个参数是你的 JSON 字符串
                //    - 第二个参数是编码，UTF-8 是标准选择
                //    - 第三个参数是媒体类型，对于 JSON 必须是 "application/json"
                var content = new StringContent(jsonstr, Encoding.UTF8, "application/json");

                // 2. 使用标准的 PostAsync 方法发送 StringContent
                var response = await _httpClient.PostAsync(requestUri, content);

                // 3. 后续的响应处理逻辑保持不变
                return await HandleResponse<T>(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<T>.Fail($"请求发生异常: {ex.Message}");
            }
        }

        public async Task<ApiResponse<T>> WmsPostAsync<T>(string requestUri, string jsonstr)
        {
            try
            {
                var content = new StringContent(jsonstr, Encoding.UTF8, "application/json");
                var wmsResult = await _httpClient.PostAsync(requestUri, content);
                string jsonString = await wmsResult.Content.ReadAsStringAsync();

                // 1. 防坑：先检查 HTTP 请求本身是否成功 (比如有没有报 404, 500)
                if (!wmsResult.IsSuccessStatusCode)
                {
                    return new ApiResponse<T>
                    {
                        IsSuccess = false,
                        Message = $"WMS接口异常, 状态码: {(int)wmsResult.StatusCode}, 详情: {jsonString}",
                        Data = default // 泛型的默认值
                    };
                }

                // 2. 将 JSON 字符串反序列化为对方的真实结构 T
                T wmsData = JsonConvert.DeserializeObject<T>(jsonString);

                // 3. 将真实结构 T 包装到你系统的 ApiResponse<T> 中
                return new ApiResponse<T>
                {
                    IsSuccess = true,      // 代表接口通信成功 (注意：具体的业务成功需要调用方去判断 Data 里面的 Result)
                    Message = "请求成功",
                    Data = wmsData         // 把反序列化好的对象塞进 Data 里
                };
            }
            catch (Exception ex)
            {
                // 捕获网络超时、反序列化失败等异常
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    Message = $"请求发生异常: {ex.Message}",
                    Data = default
                };
            }
        }

        /// <summary>
        /// 发送 XML 请求 (直接传入 XML 字符串)
        /// 适用于：你已经拼好了 XML 字符串，或者需要读取 XML 文件内容直接发送
        /// </summary>
        public async Task<TResponse> PostXmlAsync<TResponse>(string requestUri, string xmlContent)
        {
            try
            {
                // 核心：设置 Content-Type 为 application/xml
                var content = new StringContent(xmlContent, Encoding.UTF8, "application/xml");

                var response = await _httpClient.PostAsync(requestUri, content);

                var jsonString =  await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"API 请求失败 [Code:{(int)response.StatusCode}]: {jsonString}");
                }

                // 5. 处理空响应的情况
                if (string.IsNullOrWhiteSpace(jsonString))
                {
                    return default(TResponse);
                }

                // 6. 反序列化
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return System.Text.Json.JsonSerializer.Deserialize<TResponse>(jsonString, options);
            }
            catch (Exception ex)
            {
                throw new Exception($"PostXmlAsync 调用异常 ({requestUri}): {ex.Message}", ex);
            }
        }

        public async Task<ApiResponse<TResponse>> UploadFileAsync<TResponse>(string url, string filePath, Dictionary<string, string> additionalFormData = null)
        {
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    // A. 读取文件流
                    var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    var fileContent = new StreamContent(fileStream);

                    // 必须设置 Content-Type，否则服务端可能不识别
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                    // 添加文件参数，"file" 必须与服务端 Controller 的参数名一致 (public void Upload(IFormFile file))
                    content.Add(fileContent, "file", Path.GetFileName(filePath));

                    // B. 添加其他表单参数 (如单据类型、单据号等)
                    if (additionalFormData != null)
                    {
                        foreach (var kvp in additionalFormData)
                        {
                            content.Add(new StringContent(kvp.Value), kvp.Key);
                        }
                    }

                    // C. 发送请求
                    var response = await _httpClient.PostAsync(url, content);

                    return await HandleResponse<TResponse>(response);


                    //// 处理结果 (复用您现有的反序列化逻辑)
                    //var responseString = await response.Content.ReadAsStringAsync();
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    // 假设您的后端返回标准 ApiResponse 结构
                    //    // 如果后端直接返回对象，这里需要调整反序列化逻辑
                    //    return JsonConvert.DeserializeObject<ApiResponse<TResponse>>(responseString);

                    //    return await HandleResponse<T>(response);
                    //}

                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<TResponse> { IsSuccess = false, Message = ex.Message };
            }
        }
    }
}
