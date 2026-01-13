using BizLink.MES.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderProcessApiService
    {
        /// <summary>
        /// 执行工单报工并上传至 SAP
        /// </summary>
        /// <param name="request">报工请求</param>
        /// <returns>操作结果消息</returns>
        Task<string> ReportWorkOrderOperationToSapAsync(WorkOrderReportRequest request);

        /// <summary>
        /// 重新推送报工记录到 SAP
        /// </summary>
        /// <param name="confirmId">报工记录ID</param>
        /// <returns>操作结果消息</returns>
        Task<string> ReSendConfirmationToSapAsync(int confirmId);
    }
}
