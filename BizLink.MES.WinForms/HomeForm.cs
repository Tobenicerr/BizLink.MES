using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms
{
    public partial class HomeForm : BaseForm
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            label1.Text = string.Format(label1.Text, Common.AppSession.CurrentUser?.UserName ?? "未登录");
            label2.Text = string.Format(label2.Text, DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.DayOfWeek.ToString());
        }

        private void label6_Click(object sender, EventArgs e)
        {
            string url = "https://docs.google.com/document/d/17qYNcgxKZcNBEdaZ3guPVaUpCPMMS2xaYERBz2bY68Y/edit?usp=sharing"; // 您想要打开的网址

            try
            {
                // 这是在 .NET Core / .NET 8 中推荐的写法，
                // 它能确保在所有平台上都使用操作系统的 shell 来执行
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                // 处理异常，例如网址格式不正确或用户没有默认浏览器
                AntdUI.Message.error(this, $"无法打开网址: {ex.Message}");
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            string url = "http://10.163.144.13:8100/#/de-link/Vf16rgJc"; // 您想要打开的网址

            try
            {
                // 这是在 .NET Core / .NET 8 中推荐的写法，
                // 它能确保在所有平台上都使用操作系统的 shell 来执行
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                // 处理异常，例如网址格式不正确或用户没有默认浏览器
                AntdUI.Message.error(this, $"无法打开网址: {ex.Message}");
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            string url = "http://10.163.144.13:8100/#/de-link/nOn3NWyY"; // 您想要打开的网址

            try
            {
                // 这是在 .NET Core / .NET 8 中推荐的写法，
                // 它能确保在所有平台上都使用操作系统的 shell 来执行
                var psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                // 处理异常，例如网址格式不正确或用户没有默认浏览器
                AntdUI.Message.error(this, $"无法打开网址: {ex.Message}");
            }
        }
    }
}
