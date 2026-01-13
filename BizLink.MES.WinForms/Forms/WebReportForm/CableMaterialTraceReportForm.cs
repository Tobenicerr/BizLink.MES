using BizLink.MES.WinForms.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms.WebReportForm
{
    public partial class CableMaterialTraceReportForm : MesBaseForm
    {
        public CableMaterialTraceReportForm()
        {
            InitializeComponent();

            // 2. 【核心修复】禁用 AntdUI 窗体的阴影和边框

            // 确保 WebView 填充整个客户区
            webView.Dock = DockStyle.Fill;
        }

        // 3. 重写 OnLoad (比 Load 事件更稳定)
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 使用基类的 RunAsync 处理加载异常 (不需要 Loading 按钮)
            await RunAsync(async () =>
            {
                // 初始化 WebView2 环境
                await webView.EnsureCoreWebView2Async(null);

                // 导航到指定网址
                webView.CoreWebView2.Navigate("http://10.163.144.13:8100/#/de-link/54FEcygj");
            });
        }
    }
}
