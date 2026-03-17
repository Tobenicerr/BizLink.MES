using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs.Response
{
    public class BartenderResponse
    {
        /// <summary>
        /// 打印任务的唯一标识符 (GUID)
        /// </summary>
        [JsonPropertyName("Id")]
        public string? Id { get; set; }

        /// <summary>
        /// 任务当前状态 
        /// 常见值: "WaitingToRun"(等待运行), "Queued"(排队中), "Sent"(已发送), "Faulted"(出错)
        /// </summary>
        [JsonPropertyName("Status")]
        public string? Status { get; set; }

        /// <summary>
        /// 用于后续轮询查询该任务状态的完整 URL
        /// </summary>
        [JsonPropertyName("StatusUrl")]
        public string? StatusUrl { get; set; }

        // =========================================================
        // 辅助属性 (不参与 JSON 反序列化，仅供代码逻辑使用)
        // =========================================================

        /// <summary>
        /// 判断任务是否已成功提交到服务器队列
        /// </summary>
        [JsonIgnore]
        public bool IsQueuedSuccess =>
            !string.IsNullOrEmpty(Id) &&
            (Status == "WaitingToRun" || Status == "Queued" || Status == "Sent" || Status == "Running");
    }
}
