namespace BizLink.MES.WinForms.Forms
{
    partial class CuttingParamModifyForm
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
            cancelButton = new AntdUI.Button();
            saveButton = new AntdUI.Button();
            panel3 = new AntdUI.Panel();
            cutqtyLabel = new AntdUI.Label();
            lendslLabel = new AntdUI.Label();
            lenuslLabel = new AntdUI.Label();
            cutlenLable = new AntdUI.Label();
            cableSelect = new AntdUI.Select();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 2);
            tableLayoutPanel1.Controls.Add(panel3, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.Size = new Size(644, 338);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(638, 34);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Microsoft YaHei UI", 15F);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(638, 34);
            label1.TabIndex = 0;
            label1.Text = "  选择断线参数";
            // 
            // panel2
            // 
            panel2.Controls.Add(cancelButton);
            panel2.Controls.Add(saveButton);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 296);
            panel2.Name = "panel2";
            panel2.Size = new Size(638, 39);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Right;
            cancelButton.BorderWidth = 1F;
            cancelButton.Font = new Font("Microsoft YaHei UI", 10F);
            cancelButton.Location = new Point(479, -1);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 40);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "关闭";
            cancelButton.Click += cancelButton_Click;
            // 
            // saveButton
            // 
            saveButton.Anchor = AnchorStyles.Right;
            saveButton.Font = new Font("Microsoft YaHei UI", 10F);
            saveButton.Location = new Point(560, 0);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(75, 40);
            saveButton.TabIndex = 0;
            saveButton.Text = "保存";
            saveButton.Type = AntdUI.TTypeMini.Primary;
            saveButton.Click += saveButton_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(cutqtyLabel);
            panel3.Controls.Add(lendslLabel);
            panel3.Controls.Add(lenuslLabel);
            panel3.Controls.Add(cutlenLable);
            panel3.Controls.Add(cableSelect);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 43);
            panel3.Name = "panel3";
            panel3.Size = new Size(638, 247);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // cutqtyLabel
            // 
            cutqtyLabel.Anchor = AnchorStyles.Top;
            cutqtyLabel.BackColor = Color.White;
            cutqtyLabel.Font = new Font("Microsoft YaHei UI", 11F);
            cutqtyLabel.Location = new Point(12, 159);
            cutqtyLabel.Name = "cutqtyLabel";
            cutqtyLabel.Size = new Size(620, 30);
            cutqtyLabel.TabIndex = 5;
            cutqtyLabel.Text = "断线数量：";
            // 
            // lendslLabel
            // 
            lendslLabel.Anchor = AnchorStyles.Top;
            lendslLabel.BackColor = Color.White;
            lendslLabel.Font = new Font("Microsoft YaHei UI", 11F);
            lendslLabel.Location = new Point(12, 123);
            lendslLabel.Name = "lendslLabel";
            lendslLabel.Size = new Size(620, 30);
            lendslLabel.TabIndex = 3;
            lendslLabel.Text = "长度下限：";
            // 
            // lenuslLabel
            // 
            lenuslLabel.Anchor = AnchorStyles.Top;
            lenuslLabel.BackColor = Color.White;
            lenuslLabel.Font = new Font("Microsoft YaHei UI", 11F);
            lenuslLabel.Location = new Point(12, 87);
            lenuslLabel.Name = "lenuslLabel";
            lenuslLabel.Size = new Size(620, 30);
            lenuslLabel.TabIndex = 2;
            lenuslLabel.Text = "长度上限：";
            // 
            // cutlenLable
            // 
            cutlenLable.Anchor = AnchorStyles.Top;
            cutlenLable.BackColor = Color.White;
            cutlenLable.Font = new Font("Microsoft YaHei UI", 11F);
            cutlenLable.Location = new Point(12, 51);
            cutlenLable.Name = "cutlenLable";
            cutlenLable.Size = new Size(620, 30);
            cutlenLable.TabIndex = 1;
            cutlenLable.Text = "断线长度：";
            // 
            // cableSelect
            // 
            cableSelect.Dock = DockStyle.Top;
            cableSelect.Font = new Font("Microsoft YaHei UI", 12F);
            cableSelect.Location = new Point(0, 0);
            cableSelect.Name = "cableSelect";
            cableSelect.Size = new Size(638, 45);
            cableSelect.TabIndex = 0;
            cableSelect.SelectedValueChanged += cableSelect_SelectedValueChanged;
            // 
            // CuttingParamModifyForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(644, 338);
            Controls.Add(tableLayoutPanel1);
            Name = "CuttingParamModifyForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CuttingParamModifyForm";
            Load += CuttingParamModifyForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Label label1;
        private AntdUI.Panel panel2;
        private AntdUI.Button cancelButton;
        private AntdUI.Button saveButton;
        private AntdUI.Panel panel3;
        private AntdUI.Label cutlenLable;
        private AntdUI.Select cableSelect;
        private AntdUI.Label cutqtyLabel;
        private AntdUI.Label lendslLabel;
        private AntdUI.Label lenuslLabel;
    }
}