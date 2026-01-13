using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Services.ApiServices;
using Microsoft.AspNetCore.Mvc;

namespace BizLink.MES.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderScrapDeclarationController : ControllerBase
    {

        private readonly IOrderScrapDeclarationApiService _orderScrapDeclarationApiService;

        public OrderScrapDeclarationController(IOrderScrapDeclarationApiService orderScrapDeclarationApiService)
        {
            _orderScrapDeclarationApiService = orderScrapDeclarationApiService;
        }

        [HttpPost("GetList")]
        public async Task<ActionResult<ApiResponse<List<SapOrderScrapDeclarationDto>>>> GetOrderScrapDeclarationsAsync([FromBody] SapOrderScrapDeclarationRequest request)
        {
            try
            {
                var result = await _orderScrapDeclarationApiService.GetOrderScrapDeclarationsAsync(request);
                if(result != null && result.Count() > 0)
                    return Ok(ApiResponse<List<SapOrderScrapDeclarationDto>>.Success(result));
                else
                    return Ok(ApiResponse<List<SapOrderScrapDeclarationDto>>.Success(null, "未找到报废申报单数据"));
            }
            catch (Exception ex)
            {

                return BadRequest(ApiResponse<List<SapOrderScrapDeclarationDto>>.Fail(ex.Message));

            }
        }
    }
}
