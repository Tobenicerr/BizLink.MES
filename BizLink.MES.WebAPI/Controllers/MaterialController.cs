using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BizLink.MES.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {

        private readonly ISapRfcService _sapRfcService;
        public MaterialController(ISapRfcService sapRfcService)
        {
            _sapRfcService = sapRfcService;
        }

        [HttpPost("SyncByCodes")]
        public async Task<ActionResult<ApiResponse<bool>>> SyncMaterialByCodesAsync([FromBody] MaterialSyncRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.FactoryCode))
                    throw new ArgumentException("工厂代码不能为空");
                if (request.MaterialCodes == null || request.MaterialCodes.Count() == 0)
                    throw new ArgumentException("物料号列表不能为空");
                var result = await _sapRfcService.SyncMaterialFromSAPAsync(request.FactoryCode, request.MaterialCodes,null,null);
                return Ok(ApiResponse<bool>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }

        [HttpPost("SyncByDate")]
        public async Task<ActionResult<ApiResponse<bool>>> SyncMaterialByDateAsync([FromBody] MaterialSyncRequest request)
        {
            try
            {
                if(string.IsNullOrWhiteSpace(request.FactoryCode ))
                    throw new ArgumentException("工厂代码不能为空");
                if (request.StartTime == null)
                    throw new ArgumentException("开始时间不能为空");
                var result = await _sapRfcService.SyncMaterialFromSAPAsync(request.FactoryCode, null, request.StartTime,request.EndTime);
                return Ok(ApiResponse<bool>.Success(result));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }
    }
}
