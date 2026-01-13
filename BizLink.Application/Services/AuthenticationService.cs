using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    /// <summary>
    /// Orchestrates the login process by combining AD and database authentication.
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAdAuthService _adAuthService;
        private readonly IUserRepository _userRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public AuthenticationService(IAdAuthService adAuthService, IUserRepository userRepository, IMapper mapper, IActivityLogRepository activityLogRepository)
        {
            _adAuthService = adAuthService;
            _userRepository = userRepository;
            _mapper = mapper;
            _activityLogRepository = activityLogRepository;
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            try
            {
                // 1. 尝试域(AD)验证
                if (_adAuthService.ValidateCredentials(username, password))
                {
                    var user = await _userRepository.GetByDomainAccountAsync(username);
                    if (user == null)
                    {
                        // 域用户在本地数据库中不存在
                        return new LoginResult { ResultType = LoginResultType.AdUserNotInDb };
                    }
                    // 域用户验证成功

                    _activityLogRepository.AddAsync(new ActivityLog()
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        LogType = "LOGINFO",
                        LogContent = $"[{Environment.MachineName}]-[{Environment.UserName}] Login Success",
                    });
                    return new LoginResult { ResultType = LoginResultType.Success, User = _mapper.Map<UserDto>(user) };
                }

                // 2. 如果域验证失败，则回退到数据库验证
                var dbUser = await _userRepository.GetByEmployeeIdAsync(username);
                try
                {
                    if (dbUser == null)
                    {
                        return new LoginResult { ResultType = LoginResultType.UserNotFound };
                    }

                    // 检查是否是首次登录 (数据库中没有密码哈希)
                    if (string.IsNullOrEmpty(dbUser.PasswordHash))
                    {
                        return new LoginResult { ResultType = LoginResultType.PasswordSetupRequired, User = _mapper.Map<UserDto>(dbUser) };
                    }

                    // 验证数据库中的密码
                    if (BCrypt.Net.BCrypt.Verify(password, dbUser.PasswordHash))
                    {
                        _activityLogRepository.AddAsync(new ActivityLog()
                        {
                            UserId = dbUser.Id,
                            UserName = dbUser.UserName,
                            LogType = "LOGINFO",
                            LogContent = $"[{Environment.MachineName}]-[{Environment.UserName}] Login Success",
                        });
                        return new LoginResult { ResultType = LoginResultType.Success, User = _mapper.Map<UserDto>(dbUser) };
                    }
                }
                catch (BCrypt.Net.SaltParseException)
                {
                    return new LoginResult { ResultType = LoginResultType.PasswordSetupRequired, User = _mapper.Map<UserDto>(dbUser) };
                }

            }

            catch (Exception)
            {

                //throw;
            }

            

            // 用户名或密码错误
            return new LoginResult { ResultType = LoginResultType.InvalidCredentials };
        }

        public async Task<bool> SetPasswordAsync(string username, string newPassword)
        {
            var user = await _userRepository.GetByEmployeeIdAsync(username);
            if (user == null) return false;

            // 使用 BCrypt 哈希密码
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
