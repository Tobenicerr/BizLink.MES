using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public enum LoginResultType
    {
        Success,
        InvalidCredentials,
        UserNotFound,
        PasswordSetupRequired,
        AdUserNotInDb
    }

    public class LoginResult
    {
        public LoginResultType ResultType { get; set; }
        public UserDto User { get; set; }
    }

    public interface IAuthenticationService
    {
        Task<LoginResult> LoginAsync(string username, string password);
        Task<bool> SetPasswordAsync(string username, string newPassword);
    }
}
