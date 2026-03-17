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
            panel3 = new AntdUI.Panel();
            ConstructionNoSelect = new AntdUI.Select();
            CompareButton = new AntdUI.Button();
            DocumentNoSelect = new AntdUI.Select();
            panel4 = new AntdUI.Panel();
            UploadButton = new AntdUI.Button();
            panel2 = new AntdUI.Panel();
            DocNameInput = new AntdUI.Input();
            label5 = new AntdUI.Label();
            RemarkInput = new AntdUI.Input();
            label4 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            label1 = new AntdUI.Label();
            ProcessDescInput = new AntdUI.Input();
            ConstructionNoInput = new AntdUI.Input();
            DocumentNoInput = new AntdUI.Input();
            SubmitButton = new AntdUI.Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitter1).BeginInit();
            splitter1.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 250F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Controls.Add(panel3, 0, 0);
            tableLayoutPanel1.Controls.Add(panel4, 1, 0);
            tableLayoutPanel1.Controls.Add(panel2, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(800, 547);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(splitter1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 53);
            panel1.Name = "panel1";
            tableLayoutPanel1.SetRowSpan(panel1, 2);
            panel1.Size = new Size(544, 491);
            panel1.TabIndex = 2;
            panel1.Text = "panel1";
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Fill;
            splitter1.Location = new Point(0, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(544, 491);
            splitter1.SplitterDistance = 120;
            splitter1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(ConstructionNoSelect);
            panel3.Controls.Add(CompareButton);
            panel3.Controls.Add(DocumentNoSelect);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(544, 44);
            panel3.TabIndex = 5;
            panel3.Text = "panel3";
            // 
            // ConstructionNoSelect
            // 
            ConstructionNoSelect.Font = new Font("Microsoft YaHei UI", 10F);
            ConstructionNoSelect.Location = new Point(0, 0);
            ConstructionNoSelect.Name = "ConstructionNoSelect";
            ConstructionNoSelect.Size = new Size(200, 45);
            ConstructionNoSelect.TabIndex = 3;
            ConstructionNoSelect.SelectedValueChanged += ConstructionNoSelect_SelectedValueChanged;
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
            // DocumentNoSelect
            // 
            DocumentNoSelect.Font = new Font("Microsoft YaHei UI", 10F);
            DocumentNoSelect.Location = new Point(200, 0);
            DocumentNoSelect.Name = "DocumentNoSelect";
            DocumentNoSelect.Size = new Size(200, 45);
            DocumentNoSelect.TabIndex = 2;
            DocumentNoSelect.SelectedValueChanged += DocumentNoSelect_SelectedValueChanged;
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
            UploadButton.Type = AntdUI.TTypeMini.Warn;
            UploadButton.Click += UploadButton_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(DocNameInput);
            panel2.Controls.Add(label5);
            panel2.Controls.Add(RemarkInput);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(ProcessDescInput);
            panel2.Controls.Add(ConstructionNoInput);
            panel2.Controls.Add(DocumentNoInput);
            panel2.Controls.Add(SubmitButton);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(553, 53);
            panel2.Name = "panel2";
            tableLayoutPanel1.SetRowSpan(panel2, 2);
            panel2.Size = new Size(244, 491);
            panel2.TabIndex = 3;
            panel2.Text = "panel2";
            // 
            // DocNameInput
            // 
            DocNameInput.Font = new Font("Microsoft YaHei UI", 11F);
            DocNameInput.Location = new Point(3, 29);
            DocNameInput.Name = "DocNameInput";
            DocNameInput.Size = new Size(240, 45);
            DocNameInput.TabIndex = 13;
            // 
            // label5
            // 
            label5.BackColor = Color.White;
            label5.Location = new Point(3, 0);
            label5.Name = "label5";
            label5.Size = new Size(238, 23);
            label5.TabIndex = 12;
            label5.Text = "  文件名";
            // 
            // RemarkInput
            // 
            RemarkInput.Font = new Font("Microsoft YaHei UI", 11F);
            RemarkInput.Location = new Point(3, 349);
            RemarkInput.Name = "RemarkInput";
            RemarkInput.Size = new Size(240, 45);
            RemarkInput.TabIndex = 11;
            // 
            // label4
            // 
            label4.BackColor = Color.White;
            label4.Location = new Point(3, 320);
            label4.Name = "label4";
            label4.Size = new Size(238, 23);
            label4.TabIndex = 10;
            label4.Text = "  备注";
            // 
            // label3
            // 
            label3.BackColor = Color.White;
            label3.Location = new Point(3, 240);
            label3.Name = "label3";
            label3.Size = new Size(238, 23);
            label3.TabIndex = 9;
            label3.Text = "  工艺名称";
            // 
            // label2
            // 
            label2.BackColor = Color.White;
            label2.Location = new Point(3, 160);
            label2.Name = "label2";
            label2.Size = new Size(238, 23);
            label2.TabIndex = 8;
            label2.Text = "  文件编号";
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Location = new Point(3, 80);
            label1.Name = "label1";
            label1.Size = new Size(238, 23);
            label1.TabIndex = 7;
            label1.Text = "  图纸号";
            // 
            // ProcessDescInput
            // 
            ProcessDescInput.Font = new Font("Microsoft YaHei UI", 11F);
            ProcessDescInput.Location = new Point(3, 269);
            ProcessDescInput.Name = "ProcessDescInput";
            ProcessDescInput.Size = new Size(240, 45);
            ProcessDescInput.TabIndex = 6;
            // 
            // ConstructionNoInput
            // 
            ConstructionNoInput.Font = new Font("Microsoft YaHei UI", 11F);
            ConstructionNoInput.Location = new Point(3, 109);
            ConstructionNoInput.Name = "ConstructionNoInput";
            ConstructionNoInput.Size = new Size(240, 45);
            ConstructionNoInput.TabIndex = 5;
            // 
            // DocumentNoInput
            // 
            DocumentNoInput.Font = new Font("Microsoft YaHei UI", 11F);
            DocumentNoInput.Location = new Point(3, 189);
            DocumentNoInput.Name = "DocumentNoInput";
            DocumentNoInput.Size = new Size(240, 45);
            DocumentNoInput.TabIndex = 4;
            // 
            // SubmitButton
            // 
            SubmitButton.Dock = DockStyle.Bottom;
            SubmitButton.Font = new Font("Microsoft YaHei UI", 11F);
            SubmitButton.Location = new Point(0, 446);
            SubmitButton.Name = "SubmitButton";
            SubmitButton.Size = new Size(244, 45);
            SubmitButton.TabIndex = 2;
            SubmitButton.Text = "提交";
            SubmitButton.Type = AntdUI.TTypeMini.Primary;
            SubmitButton.Click += SubmitButton_Click;
            // 
            // WiFileManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 547);
            Controls.Add(tableLayoutPanel1);
            Name = "WiFileManagementForm";
            Text = "WiFileManagementForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitter1).EndInit();
            splitter1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Splitter splitter1;
        private AntdUI.Panel panel1;
        private AntdUI.Button CompareButton;
        private AntdUI.Panel panel2;
        private AntdUI.Button SubmitButton;
        private AntdUI.Button UploadButton;
        private AntdUI.Select DocumentNoSelect;
        private AntdUI.Input DocumentNoInput;
        private AntdUI.Panel panel3;
        private AntdUI.Panel panel4;
        private AntdUI.Input ProcessDescInput;
        private AntdUI.Input ConstructionNoInput;
        private AntdUI.Label label4;
        private AntdUI.Label label3;
        private AntdUI.Label label2;
        private AntdUI.Label label1;
        private AntdUI.Input RemarkInput;
        private AntdUI.Input DocNameInput;
        private AntdUI.Label label5;
        private AntdUI.Select ConstructionNoSelect;
    }
}