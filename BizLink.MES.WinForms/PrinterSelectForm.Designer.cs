namespace BizLink.MES.WinForms
{
    partial class PrinterSelectForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new AntdUI.Panel();
            label1 = new AntdUI.Label();
            panel2 = new AntdUI.Panel();
            divider1 = new AntdUI.Divider();
            panel3 = new AntdUI.Panel();
            label2 = new AntdUI.Label();
            printerSelect = new AntdUI.Select();
            panel4 = new AntdUI.Panel();
            cancelButton = new AntdUI.Button();
            submitButton = new AntdUI.Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel3, 0, 2);
            tableLayoutPanel1.Controls.Add(panel4, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.Size = new Size(450, 180);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(444, 29);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.BackColor = Color.White;
            label1.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
            label1.Location = new Point(7, 3);
            label1.Name = "label1";
            label1.Size = new Size(152, 23);
            label1.TabIndex = 0;
            label1.Text = "选择打印机";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(divider1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 38);
            panel2.Name = "panel2";
            panel2.Size = new Size(444, 9);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // divider1
            // 
            divider1.BackColor = Color.White;
            divider1.ColorSplit = SystemColors.ControlLight;
            divider1.Dock = DockStyle.Fill;
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Size = new Size(444, 9);
            divider1.TabIndex = 0;
            divider1.Text = "";
            // 
            // panel3
            // 
            panel3.Controls.Add(label2);
            panel3.Controls.Add(printerSelect);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 53);
            panel3.Name = "panel3";
            panel3.Size = new Size(444, 79);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // label2
            // 
            label2.BackColor = Color.White;
            label2.Font = new Font("Microsoft YaHei UI", 10F);
            label2.Location = new Point(10, 3);
            label2.Name = "label2";
            label2.Prefix = "*";
            label2.Size = new Size(75, 23);
            label2.TabIndex = 6;
            label2.Text = "打印机列表";
            // 
            // printerSelect
            // 
            printerSelect.Font = new Font("Microsoft YaHei UI", 11F);
            printerSelect.Location = new Point(3, 26);
            printerSelect.Name = "printerSelect";
            printerSelect.Size = new Size(432, 40);
            printerSelect.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(cancelButton);
            panel4.Controls.Add(submitButton);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 138);
            panel4.Name = "panel4";
            panel4.Size = new Size(444, 39);
            panel4.TabIndex = 3;
            panel4.Text = "panel4";
            // 
            // cancelButton
            // 
            cancelButton.BorderWidth = 1F;
            cancelButton.Location = new Point(287, -3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 40);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "取消";
            cancelButton.Click += cancelButton_Click;
            // 
            // submitButton
            // 
            submitButton.Location = new Point(358, -3);
            submitButton.Name = "submitButton";
            submitButton.Size = new Size(75, 40);
            submitButton.TabIndex = 0;
            submitButton.Text = "确定";
            submitButton.Type = AntdUI.TTypeMini.Primary;
            submitButton.Click += submitButton_Click;
            // 
            // PrinterSelectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 180);
            Controls.Add(tableLayoutPanel1);
            Name = "PrinterSelectForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "PrinterSelectForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Label label1;
        private AntdUI.Panel panel2;
        private AntdUI.Divider divider1;
        private AntdUI.Panel panel3;
        private AntdUI.Label label2;
        private AntdUI.Label label4;
        private AntdUI.Label label3;
        private AntdUI.Input remarkInput;
        private AntdUI.InputNumber repInputNumber;
        private AntdUI.Select printerSelect;
        private AntdUI.Panel panel4;
        private AntdUI.Button cancelButton;
        private AntdUI.Button submitButton;
    }
}