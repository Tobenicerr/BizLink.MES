using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BizLink.MES.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CableCutParamController : ControllerBase
    {
        private readonly ISapRfcService _sapRfcService;
        private readonly ICableCutParamService _cableCutParamService;

        public CableCutParamController(ISapRfcService sapRfcService, ICableCutParamService cableCutParamService)
        {
            _sapRfcService = sapRfcService;
            _cableCutParamService = cableCutParamService;
        }
        [HttpPost("byMaterialCodes")]
        public async Task<ActionResult<ApiResponse<List<CableCutParamCreateDto>>>> GetCableCutParamByMaterialCodesAsync([FromBody] CableCutParamRequest request)
        {
            try
            {
                 var result = await _sapRfcService.GetCableCutParamByMaterialsAsync(request.SemiMaterialCode);
                return Ok(ApiResponse<List<CableCutParamCreateDto>>.Success(result));

            }
            catch (Exception ex)
            {

                return BadRequest(ApiResponse<List<CableCutParamCreateDto>>.Fail(ex.Message));
            }
         
        }
    }

    public class CableCutParamRequest
    {
        public List<string> SemiMaterialCode { get; set; }
    }
}
