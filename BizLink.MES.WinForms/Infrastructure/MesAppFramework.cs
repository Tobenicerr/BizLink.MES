using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.WinForms.Infrastructure
{
    // ========================================================================
    // 1. 窗体工厂接口
    // ========================================================================
    public interface IFormFactory
    {
        void Show<TForm>(Action<TForm> setup = null, bool isModal = false) where TForm : Form;
        void OpenDrawer<TForm>(Form parent, Action<TForm> setup = null) where TForm : Form;
    }

    // ========================================================================
    // 2. 窗体工厂实现
    // ========================================================================
    public class ScopedFormFactory : IFormFactory
    {
        private readonly IServiceProvider _rootProvider;

        public ScopedFormFactory(IServiceProvider rootProvider)
        {
            _rootProvider = rootProvider;
        }

        public void Show<TForm>(Action<TForm> setup = null, bool isModal = false) where TForm : Form
        {
            var scope = _rootProvider.CreateScope();
            try
            {
                // 【修改点】：使用 ActivatorUtilities 创建实例
                // 这样创建的 Form 不会被 Scope 追踪，Scope 释放时不会再次调用 Form.Dispose()
                // 从而避免了 "Form关闭 -> 释放Scope -> Scope释放Form -> Form再次Dispose" 的死循环
                var form = ActivatorUtilities.CreateInstance<TForm>(scope.ServiceProvider);

                setup?.Invoke(form);

                // 窗体关闭时，只需要释放 Scope (清理 Service、DbContext)，不需要管 Form (UI 会自己销毁)
                form.FormClosed += (s, e) => scope.Dispose();

                if (isModal)
                    form.ShowDialog();
                else
                    form.Show();
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }

        public void OpenDrawer<TForm>(Form parent, Action<TForm> setup = null) where TForm : Form
        {
            var scope = _rootProvider.CreateScope();
            try
            {
                // 【修改点】：同上，使用 ActivatorUtilities
                var form = ActivatorUtilities.CreateInstance<TForm>(scope.ServiceProvider);

                setup?.Invoke(form);

                // Drawer 关闭时释放 Scope
                form.FormClosed += (s, e) => scope.Dispose();

                var config = new AntdUI.Drawer.Config(parent, form) { Mask = true };
                AntdUI.Drawer.open(config);
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }

    // ========================================================================
    // 3. MesBaseForm (原有基类)
    // 适用于：主界面、嵌入式页面、普通文档窗体
    // 继承自：BizLink.MES.WinForms.Common.BaseForm
    // ========================================================================
    public class MesBaseForm : AntdUI.BaseForm
    {
        /// <summary>
        /// 完整版 RunAsync (带按钮Loading)
        /// </summary>
        protected async Task RunAsync(AntdUI.Button triggerBtn, Func<Task> action, string successMsg = null, string confirmMsg = null)
        {
            await FormHelper.RunAsync(this, triggerBtn, action, successMsg, confirmMsg);
        }

        /// <summary>
        /// 简化版 RunAsync (无按钮Loading，支持可选的成功/确认消息)
        /// 【修复】合并了原来的两个重载，将 successMsg 设为可选，支持命名参数调用
        /// </summary>
        protected async Task RunAsync(Func<Task> action, string successMsg = null, string confirmMsg = null)
        {
            await RunAsync(null, action, successMsg, confirmMsg);
        }
    }

    // ========================================================================
    // 4. 【新增】MesWindowForm (弹窗基类)
    // 适用于：登录框、编辑框、设置框等模态窗口
    // 继承自：AntdUI.Window (自带阴影、圆角、自定义标题栏效果)
    // ========================================================================
    public class MesWindowForm : AntdUI.Window
    {
        /// <summary>
        /// 完整版 RunAsync (带按钮Loading)
        /// </summary>
        protected async Task RunAsync(AntdUI.Button triggerBtn, Func<Task> action, string successMsg = null, string confirmMsg = null)
        {
            await FormHelper.RunAsync(this, triggerBtn, action, successMsg, confirmMsg);
        }

        /// <summary>
        /// 简化版 RunAsync (无按钮Loading，支持可选的成功/确认消息)
        /// 【修复】同上，合并重载
        /// </summary>
        protected async Task RunAsync(Func<Task> action, string successMsg = null, string confirmMsg = null)
        {
            await RunAsync(null, action, successMsg, confirmMsg);
        }
    }

    // ========================================================================
    // 5. 内部帮助类 (避免代码重复)
    // ========================================================================
    internal static class FormHelper
    {
        public static async Task RunAsync(Form form, AntdUI.Button triggerBtn, Func<Task> action, string successMsg, string confirmMsg)
        {
            if (!string.IsNullOrEmpty(confirmMsg))
            {
                if (AntdUI.Modal.open(new AntdUI.Modal.Config(form.ParentForm?? form, "确认", confirmMsg, AntdUI.TType.Warn)) != DialogResult.OK)
                    return;
            }

            if (triggerBtn != null)
                triggerBtn.Loading = true;
            form.Enabled = false;

            try
            {
                await action();
                if (!string.IsNullOrEmpty(successMsg))
                    AntdUI.Message.success(form, successMsg);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (msg.Contains("See the inner exception"))
                    msg = ex.InnerException?.Message ?? msg;
                AntdUI.Message.error(form, msg);
            }
            finally
            {
                if (triggerBtn != null)
                    triggerBtn.Loading = false;
                form.Enabled = true;
            }
        }
    }
}
