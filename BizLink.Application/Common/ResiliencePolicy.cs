using BizLink.MES.Application.DTOs;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Common
{
    public static class ResiliencePolicy
    {
        public static AsyncRetryPolicy<ApiResponse<T>> GetApiRetryPolicy<T>()
        {
            return Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .OrResult<ApiResponse<T>>(r => !r.IsSuccess) // 如果业务返回 Fail 也重试（需根据实际业务决定，通常只重试网络错误）
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        // 针对 Bartender 的 XML 调用策略
        public static AsyncRetryPolicy<T> GetBartenderRetryPolicy<T>()
        {
            return Policy<T>
               .Handle<Exception>()
               .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
