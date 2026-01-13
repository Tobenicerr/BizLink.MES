using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.WebAPI.Controllers.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BizLink.MES.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrderViewController : ControllerBase
    {
        private readonly ISapRfcService _sapRfcService;
        private readonly IFactoryService _factoryService;
        public WorkOrderViewController(ISapRfcService sapRfcService, IFactoryService factoryService)
        {
            _sapRfcService = sapRfcService;
            _factoryService = factoryService;
        }

        [HttpGet("byDispatchDate")]
        public  async Task<ActionResult<ApiResponse<SapOrderDto>>> GetWorkOrderByDispatchDateAsync([FromQuery] string factoryCode,
    [FromQuery] string dispatchDate)
        {
            DateTime parsedDate;
            try
            {
                var factory = (await _factoryService.GetAllAsync()).Where(x => x.FactoryCode.Equals(factoryCode)).FirstOrDefault();
                if (factory == null)
                {
                    throw new Exception("未查询到工厂信息");
                }
                // 尝试解析，如果成功，结果会存入 parsedDate，并返回 true
                if (DateTime.TryParse(dispatchDate, out parsedDate))
                {
                    var orders = await _sapRfcService.GetWorkOrdersAsync(factoryCode, parsedDate);
                    return Ok(ApiResponse<SapOrderDto>.Success(orders));
                }
                else
                {
                    throw new Exception("日期参数格式不正确");
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ApiResponse<SapOrderDto>.Fail(ex.Message));

            }


        }
        [HttpPost("byOrderNos")]
        public async Task<ActionResult<ApiResponse<SapOrderDto>>> GetWorkOrderByOrderNoAsync([FromBody] SapOrderRequest request)
        {
            try
            {
                var factory = (await _factoryService.GetAllAsync()).Where(x => x.FactoryCode.Equals(request.FactoryCode)).FirstOrDefault();
                if (factory == null)
                {
                    throw new Exception("未查询到工厂信息");
                }
                if (request.OrderNos == null || request.OrderNos.Count == 0)
                {
                    throw new Exception("订单参数为空，无法查询！");
                }

                var orders = request.OrderNos.Select(x => x.PadLeft(12,'0')).ToList();
                var result = await _sapRfcService.GetWorkOrdersAsync(request.FactoryCode, null, orders);
                return Ok(ApiResponse<SapOrderDto>.Success(result));
            }
            catch (Exception ex)
            {

                return BadRequest(ApiResponse<SapOrderDto>.Fail(ex.Message));

            }

        }

        [HttpPost("GetCN10WorkOrders")]
        public async Task<ActionResult<ApiResponse<SapOrderDto>>> GetCN10WorkOrderAsync([FromBody] SapOrderRequest request)
        {
            try
            {
                var factory = (await _factoryService.GetAllAsync()).Where(x => x.FactoryCode.Equals(request.FactoryCode)).FirstOrDefault();
                if (factory == null)
                {
                    throw new Exception("未查询到工厂信息");
                }

                if (request.OrderNos != null && request.OrderNos.Count > 0)
                {
                    return Ok(ApiResponse<SapOrderDto>.Success(await _sapRfcService.GetCN10WorkOrdersAsync(request.FactoryCode, null, null, request.OrderNos.Select(x => x.PadLeft(12, '0')).ToList())));
                }
                else
                {
                    return Ok(ApiResponse<SapOrderDto>.Success(await _sapRfcService.GetCN10WorkOrdersAsync(request.FactoryCode, request.DispatchDate, request.WorkCenterCode, null)));
                }                
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<SapOrderDto>.Fail(ex.Message));
            }
        }
    }
}
