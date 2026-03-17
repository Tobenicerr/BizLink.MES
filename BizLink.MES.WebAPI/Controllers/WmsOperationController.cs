using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using Microsoft.AspNetCore.Mvc;

namespace BizLink.MES.WebAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WmsOperationController : ControllerBase
    {
        [HttpPost("OLDInBoundConfirm")]
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

        [HttpPost("OLDOutBoundConfirm")]
        public async Task<ActionResult<ApiResponse<string>>> WorkOrderOperationOutReportToSAPAsync([FromBody] WorkOrderReportRequest request)
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
    }
}
