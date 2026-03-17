using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs.Response
{
    public class WmsApprovedResponse
    {
        public string Result
        {
            get; set;
        }
        public string? Message
        {
            get; set;
        }

        // 🌟 魔法在这里：定义如何隐式转换为 ApiResponse<WmsApprovedResponse>
        public static implicit operator ApiResponse<WmsApprovedResponse>(WmsApprovedResponse response)
        {
            if (response == null) return new ApiResponse<WmsApprovedResponse> { IsSuccess = false, Message = "响应为空" };

            return new ApiResponse<WmsApprovedResponse>
            {
                // 这里定义你判断成功的逻辑
                IsSuccess = response.Result == "S",
                Message = response.Message ?? "",
                Data = response
            };
        }
    }
}
