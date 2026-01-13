using AntdUI;
using AutoUpdaterDotNET;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Forms;
using BizLink.MES.WinForms.Infrastructure;
using BizLink.MES.WinForms.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms
{
    public partial class LoginForm : MesWindowForm
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IFormFactory _formFactory;

        // 构造函数极大简化，不再需要注入子窗体的依赖
        public LoginForm(
            IAuthenticationService authenticationService,
            IFormFactory formFactory)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _formFactory = formFactory;
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            var username = usernameInput.Text.Trim().ToUpper();
            var password = passwordInput.Text.Trim();

            // 1. 基础校验
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                AntdUI.Modal.open(this, "提示", "请输入用户名和密码。", AntdUI.TType.Warn);
                return;
            }

            // 2. 使用 RunAsync 统一处理 Loading 状态和异常捕获
            await RunAsync(button1, async () =>
            {
                // 执行登录
                var result = await _authenticationService.LoginAsync(username, password);

                // 处理结果
                HandleLoginResult(result, username, password);
            });
        }

        private void HandleLoginResult(LoginResult result, string username, string password)
        {
            switch (result.ResultType)
            {
                case LoginResultType.Success:
                    // --- 登录成功 ---
                    if (chkRememberMe.Checked)
                    {
                        CredentialsManager.SaveCredentials(username, password);
                    }
                    else
                    {
                        CredentialsManager.ClearCredentials();
                    }

                    AppSession.CurrentUser = result.User;

                    // 设置结果并关闭，触发 Program.cs 中的后续逻辑
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    break;

                case LoginResultType.PasswordSetupRequired:
                    // --- 首次登录需设置密码 ---
                    AntdUI.Modal.open(this, "首次登录", "检测到您是首次登录，请设置您的密码。", AntdUI.TType.Info);

                    // 假设 SetPasswordForm 尚未重构为 DI 模式，仍保持原样
                    using (var setPasswordForm = new SetPasswordForm(_authenticationService, username))
                    {
                        setPasswordForm.ShowDialog();
                        // 密码设置成功后，用户通常需要重新点击登录，所以这里不做额外操作
                    }
                    break;

                case LoginResultType.AdUserNotInDb:
                    // --- 域用户未授权 ---
                    AntdUI.Modal.open(this, "未授权", "域用户验证成功，但您尚未被授权访问本系统，请先补充员工信息后重新登陆。", AntdUI.TType.Warn);

                    // 【核心重构点】：使用 Factory 打开 UserManagementEditForm
                    // 1. 自动创建 Scope 解决依赖注入
                    // 2. 使用 InitForDomainSupplement 进入补充信息模式
                    _formFactory.Show<UserManagementEditForm>(form =>
                    {
                        form.InitForDomainSupplement(username);
                    }, isModal: true);
                    break;

                case LoginResultType.InvalidCredentials:
                case LoginResultType.UserNotFound:
                default:
                    // --- 登录失败 ---
                    // 抛出异常，MesBaseForm.RunAsync 会自动捕获并显示 AntdUI.Message.error
                    throw new Exception("用户名或密码错误。");
            }
        }

        // 退出按钮事件 (原 button1_Click)
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            System.Windows.Forms.Application.Exit();
        }

        // 回车键触发登录
        private void passwordInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                button1_ClickAsync(sender, e);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            var (username, password) = CredentialsManager.LoadCredentials();
            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                usernameInput.Text = username;
                passwordInput.Text = password;
                chkRememberMe.Checked = true;
            }
        }
    }
}
