using AntdUI;
using BizLink.MES.Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Message = AntdUI.Message; // <--- 添加这一行别名

namespace BizLink.MES.WinForms
{
    public partial class SetPasswordForm : Window
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly string _username;

        public SetPasswordForm(IAuthenticationService authenticationService, string username)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _username = username;
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            // 从 AntdUI.Input 控件获取文本
            var newPassword = inputNewPassword.Text;
            var confirmPassword = inputConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                // 使用 AntdUI 的全局提示
                Message.error( this,"密码不能为空。");
                return;
            }

            if (newPassword != confirmPassword)
            {
                Message.error(this, "两次输入的密码不一致。");
                return;
            }

            var success = await _authenticationService.SetPasswordAsync(_username, newPassword);

            if (success)
            {
                Message.success(this, "密码设置成功！请使用新密码重新登录。");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                Message.error(this, "密码设置失败，请联系管理员。");
            }
        }
    }
}
