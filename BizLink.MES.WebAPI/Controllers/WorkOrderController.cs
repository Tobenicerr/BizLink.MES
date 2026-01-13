using Azure;
using Azure.Core;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Helper;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;

namespace BizLink.MES.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrderProcessController : ControllerBase
    {
        // 核心：只注入一个封装好的 Service
        private readonly IWorkOrderProcessApiService _workOrderSapService;

        public WorkOrderProcessController(IWorkOrderProcessApiService workOrderSapService)
        {
            _workOrderSapService = workOrderSapService;
        }

        [HttpPost("OperationReportToSAP")]
        public async Task<ActionResult<ApiResponse<string>>> WorkOrderOperationReportToSAPAsync([FromBody] WorkOrderReportRequest request)
        {
            try
            {
                var message = await _workOrderSapService.ReportWorkOrderOperationToSapAsync(request);
                return Ok(ApiResponse<string>.Success(message));
            }
            catch (Exception ex)
            {
                // 建议：此处可以使用 Filter 或 Middleware 统一处理异常
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }

        [HttpPost("ConfirmationReentryToSAP")]
        public async Task<ActionResult<ApiResponse<string>>> ReentryOfConfirmationToSAPAsync([FromQuery] int confirmid)
        {
            try
            {
                var message = await _workOrderSapService.ReSendConfirmationToSapAsync(confirmid);
                return Ok(ApiResponse<string>.Success(message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }
    }

}
