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
    public partial class CommonSelectForm : MesWindowForm
    {
        public MenuItem? SelectedValue
        {
            get; private set;
        }

        private readonly List<MenuItem> _menuItems = new List<MenuItem>();

        public CommonSelectForm(string titleText, List<MenuItem> menuItems)
        {
            InitializeComponent();
            TitleLabel.Text = titleText;
            _menuItems = menuItems;

        }

        // 2. 重写 OnLoad
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            TargetSelect.Items.Clear();
            TargetSelect.Items.AddRange(_menuItems.ToArray());
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            // 优先取 SelectedValue (如果 AntdUI Select 绑定了对象)，其次取 Text (如果是直接输入的)
            // 注意：AntdUI Select.Items.Add 字符串时，SelectedValue 通常也是字符串
            var value = (MenuItem)TargetSelect.SelectedValue;

            if (value == null)
            {
                AntdUI.Message.error(this, "未选择选项，无法提交！");
                return;
            }

            // 设置结果
            this.SelectedValue = value;
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