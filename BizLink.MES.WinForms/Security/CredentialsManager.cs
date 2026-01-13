using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.WinForms.Security
{
    public static class CredentialsManager
    {
        // 定义一个固定的“盐”，增加加密的复杂度。
        // 即使是不同的应用程序，只要盐不同，加密结果也不同。
        private static readonly byte[] s_entropy = Encoding.Unicode.GetBytes("BizLinkMES-Credentials-Salt");

        /// <summary>
        /// 加密并保存用户名和密码到应用程序设置
        /// </summary>
        public static void SaveCredentials(string username, string password)
        {
            var userBytes = Encoding.UTF8.GetBytes(username);
            var passBytes = Encoding.UTF8.GetBytes(password);

            // 使用 DPAPI 对数据进行加密
            var encryptedUser = ProtectedData.Protect(userBytes, s_entropy, DataProtectionScope.CurrentUser);
            var encryptedPass = ProtectedData.Protect(passBytes, s_entropy, DataProtectionScope.CurrentUser);

            // 将加密后的字节数组转换为 Base64 字符串以便存储
            Properties.Settings.Default.RememberedUser = Convert.ToBase64String(encryptedUser);
            Properties.Settings.Default.RememberedPassword = Convert.ToBase64String(encryptedPass);
            Properties.Settings.Default.Save(); // 保存更改
        }

        /// <summary>
        /// 从应用程序设置中加载并解密用户名和密码
        /// </summary>
        /// <returns>一个包含用户名和密码的元组。如果不存在或解密失败，则返回 null。</returns>
        public static (string? Username, string? Password) LoadCredentials()
        {
            try
            {
                var encryptedUser64 = Properties.Settings.Default.RememberedUser;
                var encryptedPass64 = Properties.Settings.Default.RememberedPassword;

                if (string.IsNullOrEmpty(encryptedUser64) || string.IsNullOrEmpty(encryptedPass64))
                {
                    return (null, null);
                }

                // 将 Base64 字符串转回字节数组
                var encryptedUser = Convert.FromBase64String(encryptedUser64);
                var encryptedPass = Convert.FromBase64String(encryptedPass64);

                // 使用 DPAPI 解密数据
                var userBytes = ProtectedData.Unprotect(encryptedUser, s_entropy, DataProtectionScope.CurrentUser);
                var passBytes = ProtectedData.Unprotect(encryptedPass, s_entropy, DataProtectionScope.CurrentUser);

                return (Encoding.UTF8.GetString(userBytes), Encoding.UTF8.GetString(passBytes));
            }
            catch
            {
                // 如果解密失败（例如，数据损坏或在另一台电脑上），则清除并返回 null
                ClearCredentials();
                return (null, null);
            }
        }

        /// <summary>
        /// 清除已保存的凭据
        /// </summary>
        public static void ClearCredentials()
        {
            Properties.Settings.Default.RememberedUser = string.Empty;
            Properties.Settings.Default.RememberedPassword = string.Empty;
            Properties.Settings.Default.Save();
        }
    }
}
