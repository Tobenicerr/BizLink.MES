using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.WinForms.Common;
using Microsoft.AspNetCore.Mvc;

namespace BizLink.MES.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse<UserDto>>> Login([FromBody] LoginDto login )
        {
            try
            {
                var result = await _authenticationService.LoginAsync(login.UserName, login.Password);
                switch (result.ResultType)
                {
                    case LoginResultType.Success:
                        // 登录成功
                        return Ok(ApiResponse<UserDto>.Success(result.User, "验证成功"));

                    case LoginResultType.PasswordSetupRequired:
                    case LoginResultType.AdUserNotInDb:
                    case LoginResultType.InvalidCredentials:
                        throw new Exception("用户名或密码错误。");
                    case LoginResultType.UserNotFound:
                        throw new Exception("用户不存在，请先添加用户！");
                    default:
                        throw new Exception("用户名或密码错误。");
                }

            }
            catch (Exception ex)
            {

                return BadRequest(ApiResponse<bool>.Fail($"验证失败：{ex.Message}"));
            }
        }
    }
}
