using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Services;
using BizLink.MES.Application.Services.ApiServices;
using Microsoft.AspNetCore.Mvc;

namespace BizLink.MES.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrderTaskController : ControllerBase
    {
        private readonly IWorkOrderTaskApiService _workOrderTaskApiService;

        public WorkOrderTaskController(IWorkOrderTaskApiService workOrderTaskApiService) 
        {
            _workOrderTaskApiService = workOrderTaskApiService;
        }
        [HttpGet("GetInspectionByWorkOrder")]
        public async Task<ActionResult<ApiResponse<WorkOrderTaskDto>>> GetInspectionWorkOrderTaskAsync([FromQuery] string userCode,[FromQuery] string workOrderNo)
        {
            try
            {

                var InspectionTaskDto = await _workOrderTaskApiService.CreateInspectionTaskByWorkOrderAsync(userCode, workOrderNo);
                return Ok(ApiResponse<WorkOrderTaskDto>.Success(InspectionTaskDto));


            }
            catch (Exception ex)
            {

                return BadRequest(ApiResponse<WorkOrderTaskDto>.Fail(ex.Message));

            }


        }
        [HttpPost("ConfirmInspectionTask")]
        public async Task<ActionResult<ApiResponse<string>>> ConfirmInspectionTaskAsync([FromBody] TaskConfirmRequest request) 
        {
            try
            {
                var message = await _workOrderTaskApiService.ConfirmInspectionTaskAsync(request.UserCode, request.TaskId, request.CompletedQty, request.ResultFlag);
                return Ok(ApiResponse<string>.Success(message));
            }
            catch (Exception ex)
            {

                return BadRequest(ApiResponse<WorkOrderTaskDto>.Fail(ex.Message));

            }
        }
    }
}
