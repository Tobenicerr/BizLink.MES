using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    /// <summary>
    /// 通用的 API 响应类
    /// </summary>
    /// <typeparam name="T">响应数据的类型</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// 指示操作是否成功
        /// </summary>
        public bool IsSuccess
        {
            get; set;
        }

        /// <summary>
        /// 响应消息，可以是成功信息或错误信息
        /// </summary>
        public string Message
        {
            get; set;
        }

        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data
        {
            get; set;
        }

        /// <summary>
        /// 创建一个成功的响应
        /// </summary>
        /// <param name="data">要返回的数据</param>
        /// <param name="message">成功消息</param>
        /// <returns>成功的 ApiResponse 实例</returns>
        public static ApiResponse<T> Success(T data, string message = "操作成功")
        {
            return new ApiResponse<T>
            {
                IsSuccess = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// 创建一个失败的响应
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <returns>失败的 ApiResponse 实例</returns>
        public static ApiResponse<T> Fail(string message)
        {
            // 对于失败的响应，Data 通常是其类型的默认值 (例如 null)
            return new ApiResponse<T>
            {
                IsSuccess = false,
                Message = message,
                Data = default(T)
            };
        }
    }
}
