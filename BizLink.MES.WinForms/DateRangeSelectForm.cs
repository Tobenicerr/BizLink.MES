using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BizLink.MES.WinForms
{
    public partial class DateRangeSelectForm : Window
    {

        public DateTime? StartSelectedDate
        {
            get; private set;
        }

        public DateTime? EndSelectedDate
        {
            get; private set;
        }
        public DateRangeSelectForm(string startLabel, string endLabel)
        {
            InitializeComponent();
            startSelectLabel.Text = startLabel;
            endStartLabel.Text = endLabel;
        }

        private void DateRangeSelectForm_Load(object sender, EventArgs e)
        {
            startSelectLabel.PrefixColor = Color.Red;
            endStartLabel.PrefixColor = Color.Red;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void submitButton_Click(object sender, EventArgs e)
        {
            if (startDatePicker.Value == null)
            {
                AntdUI.Message.error(this, $"{startSelectLabel}未选择，请先选择日期！");
                return;
            }

            if (endDatePicker.Value == null)
            {
                AntdUI.Message.error(this, $"{endStartLabel}未选择，请先选择日期！");
                return;
            }
            else
            {
                // 将选中的值赋给公共属性
                this.StartSelectedDate = startDatePicker.Value;
                this.EndSelectedDate = endDatePicker.Value;
                // 设置对话框结果为 OK，这会自动关闭窗体
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
