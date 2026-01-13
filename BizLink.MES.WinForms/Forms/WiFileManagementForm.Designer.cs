namespace BizLink.MES.WinForms.Forms
{
    partial class WiFileManagementForm
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
            splitter1 = new AntdUI.Splitter();
            panel2 = new AntdUI.Panel();
            DocVersionInput = new AntdUI.Input();
            MaterialDescInput = new AntdUI.Input();
            MaterialCodeInput = new AntdUI.Input();
            button1 = new AntdUI.Button();
            panel3 = new AntdUI.Panel();
            MaterialInput = new AntdUI.Input();
            CompareButton = new AntdUI.Button();
            DocVersionSelect = new AntdUI.Select();
            panel4 = new AntdUI.Panel();
            UploadButton = new AntdUI.Button();
            panel5 = new AntdUI.Panel();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitter1).BeginInit();
            splitter1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 1, 2);
            tableLayoutPanel1.Controls.Add(panel3, 0, 0);
            tableLayoutPanel1.Controls.Add(panel4, 1, 0);
            tableLayoutPanel1.Controls.Add(panel5, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(splitter1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 53);
            panel1.Name = "panel1";
            tableLayoutPanel1.SetRowSpan(panel1, 2);
            panel1.Size = new Size(544, 394);
            panel1.TabIndex = 2;
            panel1.Text = "panel1";
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Fill;
            splitter1.Location = new Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(544, 394);
            splitter1.SplitterDistance = 120;
            splitter1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(DocVersionInput);
            panel2.Controls.Add(MaterialDescInput);
            panel2.Controls.Add(MaterialCodeInput);
            panel2.Controls.Add(button1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(553, 103);
            panel2.Name = "panel2";
            panel2.Size = new Size(244, 344);
            panel2.TabIndex = 3;
            panel2.Text = "panel2";
            // 
            // DocVersionInput
            // 
            DocVersionInput.Dock = DockStyle.Top;
            DocVersionInput.Font = new Font("Microsoft YaHei UI", 11F);
            DocVersionInput.Location = new Point(0, 90);
            DocVersionInput.Name = "DocVersionInput";
            DocVersionInput.Size = new Size(244, 45);
            DocVersionInput.TabIndex = 6;
            // 
            // MaterialDescInput
            // 
            MaterialDescInput.Dock = DockStyle.Top;
            MaterialDescInput.Font = new Font("Microsoft YaHei UI", 11F);
            MaterialDescInput.Location = new Point(0, 45);
            MaterialDescInput.Name = "MaterialDescInput";
            MaterialDescInput.Size = new Size(244, 45);
            MaterialDescInput.TabIndex = 5;
            // 
            // MaterialCodeInput
            // 
            MaterialCodeInput.Dock = DockStyle.Top;
            MaterialCodeInput.Font = new Font("Microsoft YaHei UI", 11F);
            MaterialCodeInput.Location = new Point(0, 0);
            MaterialCodeInput.Name = "MaterialCodeInput";
            MaterialCodeInput.Size = new Size(244, 45);
            MaterialCodeInput.TabIndex = 4;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Bottom;
            button1.Font = new Font("Microsoft YaHei UI", 11F);
            button1.Location = new Point(0, 299);
            button1.Name = "button1";
            button1.Size = new Size(244, 45);
            button1.TabIndex = 2;
            button1.Text = "提交";
            button1.Type = AntdUI.TTypeMini.Primary;
            // 
            // panel3
            // 
            panel3.Controls.Add(MaterialInput);
            panel3.Controls.Add(CompareButton);
            panel3.Controls.Add(DocVersionSelect);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(544, 44);
            panel3.TabIndex = 5;
            panel3.Text = "panel3";
            // 
            // MaterialInput
            // 
            MaterialInput.Font = new Font("Microsoft YaHei UI", 11F);
            MaterialInput.Location = new Point(3, 0);
            MaterialInput.Name = "MaterialInput";
            MaterialInput.Size = new Size(200, 45);
            MaterialInput.TabIndex = 3;
            MaterialInput.KeyPress += MaterialInput_KeyPress;
            // 
            // CompareButton
            // 
            CompareButton.Font = new Font("Microsoft YaHei UI", 11F);
            CompareButton.Location = new Point(399, 0);
            CompareButton.Name = "CompareButton";
            CompareButton.Size = new Size(74, 45);
            CompareButton.TabIndex = 0;
            CompareButton.Text = "比较";
            CompareButton.Type = AntdUI.TTypeMini.Primary;
            CompareButton.Click += CompareButton_Click;
            // 
            // DocVersionSelect
            // 
            DocVersionSelect.Font = new Font("Microsoft YaHei UI", 11F);
            DocVersionSelect.Location = new Point(200, 0);
            DocVersionSelect.Name = "DocVersionSelect";
            DocVersionSelect.Size = new Size(200, 45);
            DocVersionSelect.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(UploadButton);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(553, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(244, 44);
            panel4.TabIndex = 6;
            panel4.Text = "panel4";
            // 
            // UploadButton
            // 
            UploadButton.Dock = DockStyle.Fill;
            UploadButton.Font = new Font("Microsoft YaHei UI", 11F);
            UploadButton.Location = new Point(0, 0);
            UploadButton.Name = "UploadButton";
            UploadButton.Size = new Size(244, 44);
            UploadButton.TabIndex = 4;
            UploadButton.Text = "文件上传";
            UploadButton.Type = AntdUI.TTypeMini.Primary;
            UploadButton.Click += UploadButton_Click;
            // 
            // panel5
            // 
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(553, 53);
            panel5.Name = "panel5";
            panel5.Size = new Size(244, 44);
            panel5.TabIndex = 7;
            panel5.Text = "panel5";
            // 
            // WiFileManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "WiFileManagementForm";
            Text = "WiFileManagementForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitter1).EndInit();
            splitter1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Splitter splitter1;
        private AntdUI.Panel panel1;
        private AntdUI.Button CompareButton;
        private AntdUI.Panel panel2;
        private AntdUI.Button button1;
        private AntdUI.Button UploadButton;
        private AntdUI.Select DocVersionSelect;
        private AntdUI.Input MaterialCodeInput;
        private AntdUI.Panel panel3;
        private AntdUI.Panel panel4;
        private AntdUI.Panel panel5;
        private AntdUI.Input DocVersionInput;
        private AntdUI.Input MaterialDescInput;
        private AntdUI.Input MaterialInput;
    }
}