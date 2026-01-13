using AntdUI;
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
    public partial class DateSelectForm : Window
    {
        private readonly string _labelstr = string.Empty;

        public DateTime? SelectedDate
        {
            get; private set;
        }

        public DateSelectForm(string labelstr)
        {
            InitializeComponent();
            _labelstr = labelstr;
        }

        private void DateSelectForm_Load(object sender, EventArgs e)
        {
            selectLabel.PrefixColor = Color.Red;
            selectLabel.Text = _labelstr;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (datePicker.Value == null)
            {
                AntdUI.Message.error(this, "日期未选择，请先选择日期！");
                return;
            }
            else
            {
                // 将选中的值赋给公共属性
                this.SelectedDate = datePicker.Value;
                // 设置对话框结果为 OK，这会自动关闭窗体
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
