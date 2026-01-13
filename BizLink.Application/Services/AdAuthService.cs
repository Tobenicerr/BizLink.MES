using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace BizLink.MES.Application.Services
{
    /// <summary>
    /// Provides Active Directory authentication services.
    /// </summary>
    public class AdAuthService : IAdAuthService
    {
        private readonly IConfiguration _configuration;

        public AdAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Validates user credentials against Active Directory.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True if credentials are valid, otherwise false.</returns>
        public bool ValidateCredentials(string username, string password)
        {
            // 从 appsettings.json 读取域的配置
            var domain = _configuration["ActiveDirectory:Domain"];
            if (string.IsNullOrEmpty(domain))
            {
                // 如果没有配置域，则跳过域验证
                return false;
            }

            try
            {
                // 使用 PrincipalContext 进行域验证
                using (var context = new PrincipalContext(ContextType.Domain, domain))
                {
                    return context.ValidateCredentials(username, password);
                }
            }
            catch (PrincipalServerDownException)
            {
                // 无法连接到域控制器
                // 在这里可以添加日志记录
                return false;
            }
            catch (Exception)
            {
                // 其他异常
                // 在这里可以添加日志记录
                return false;
            }
        }
    }
}
