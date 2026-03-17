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
        private readonly ISapRfcService _sapRfcService;
        private readonly ITaskExecutionService _taskExecutionService;

        public WorkOrderProcessController(IWorkOrderProcessApiService workOrderSapService, ISapRfcService sapRfcService, ITaskExecutionService taskExecutionService)
        {
            _workOrderSapService = workOrderSapService;
            _sapRfcService = sapRfcService;
            _taskExecutionService = taskExecutionService;
        }

        [HttpPost("OperationReportToSAP")]
        public async Task<ActionResult<ApiResponse<string>>> WorkOrderOperationReportToSAPAsync([FromBody] WorkOrderReportRequest request)
        {
            try
            {
                //var message = await _workOrderSapService.ReportWorkOrderOperationToSapAsync(request);
                //return Ok(ApiResponse<string>.Success(message));
                return BadRequest(ApiResponse<object>.Fail("方法作废"));

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
                await _taskExecutionService.ReEntryPushSapConfirmAsync(confirmid);
                return Ok(ApiResponse<string>.Success("重推成功！"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }


        [HttpPost("ConfirmationBatchReentryToSAP")]
        public async Task<ActionResult<ApiResponse<string>>> BatchReentryOfConfirmationToSAPAsync([FromBody] List<int> confirmids)
        {
            try
            {
                await _taskExecutionService.BatchReEntryPushSapConfirmAsync(confirmids);
                return Ok(ApiResponse<string>.Success("重推成功"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }


        [HttpPost("CancelConfirmationToSAP")]
        public async Task<ActionResult<ApiResponse<List<WorkOrderOperationConfirmDto>>>> CancelConfirmationToSAPAsync([FromBody] List<int> confirmids)
        {
            try
            {
                var result = await _sapRfcService.CancelBatchConfirmToSAPAsync(confirmids);
                return Ok(ApiResponse<List<WorkOrderOperationConfirmDto>>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }


        [HttpPost("FinishedGoodsReceiptToSAP")]
        public async Task<ActionResult<ApiResponse<string>>> FinishedGoodsReceiptToSapAsync([FromQuery] int receiptId) 
        {
            try
            {
                var (result,message) = await _workOrderSapService.FinishedGoodsReceiptToSapAsync(receiptId);
                if (result)
                    return Ok(ApiResponse<string>.Success(message));
                else
                    return BadRequest(ApiResponse<string>.Fail(message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<object>.Fail(ex.Message));
            }
        }
    }

}
