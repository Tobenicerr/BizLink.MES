using AntdUI;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.WinForms.Infrastructure; // 引用基础架构
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace BizLink.MES.WinForms
{
    // 1. 继承 MesWindowForm (统一弹窗基类)
    public partial class PrinterSelectForm : MesWindowForm
    {
        public string? SelectedPrinterName
        {
            get; private set;
        }

        public PrinterSelectForm()
        {
            InitializeComponent();
        }

        // 2. 重写 OnLoad
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 初始化 UI
            if (label2 != null)
                label2.PrefixColor = Color.Red;

            LoadPrinters();
        }

        private void LoadPrinters()
        {
            printerSelect.Items.Clear();

            // 获取本地安装的所有打印机
            foreach (string printerName in PrinterSettings.InstalledPrinters)
            {
                // 简单的网络/本地打印机判断逻辑
                if (printerName.StartsWith(@"\\"))
                {
                    string shortName = printerName.Substring(printerName.LastIndexOf('\\') + 1);
                    printerSelect.Items.Add($"{shortName}-{PrinterType.NetworkPrinter.GetDescription()}");
                }
                else
                {
                    printerSelect.Items.Add($"{printerName}-{PrinterType.LocalPrinter.GetDescription()}");
                }
            }
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            // 优先取 SelectedValue (如果 AntdUI Select 绑定了对象)，其次取 Text (如果是直接输入的)
            // 注意：AntdUI Select.Items.Add 字符串时，SelectedValue 通常也是字符串
            string value = printerSelect.SelectedValue?.ToString();

            if (string.IsNullOrWhiteSpace(value))
            {
                value = printerSelect.Text?.Trim();
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                AntdUI.Message.error(this, "打印机未选择，请先选择打印机！");
                return;
            }

            // 设置结果
            this.SelectedPrinterName = value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}