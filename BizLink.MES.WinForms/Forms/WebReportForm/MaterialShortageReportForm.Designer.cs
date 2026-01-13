namespace BizLink.MES.WinForms.Forms.WebReportForm
{
    partial class MaterialShortageReportForm
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
            exportButton = new AntdUI.Button();
            queryButton = new AntdUI.Button();
            keywordInput = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
            spin = new AntdUI.Spin();
            orderTable = new AntdUI.Table();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(exportButton);
            panel1.Controls.Add(queryButton);
            panel1.Controls.Add(keywordInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(794, 39);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // exportButton
            // 
            exportButton.Font = new Font("Microsoft YaHei UI", 11F);
            exportButton.Location = new Point(234, -3);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(75, 43);
            exportButton.TabIndex = 2;
            exportButton.Text = "导出";
            exportButton.Type = AntdUI.TTypeMini.Success;
            exportButton.Click += exportButton_Click;
            // 
            // queryButton
            // 
            queryButton.Font = new Font("Microsoft YaHei UI", 11F);
            queryButton.Location = new Point(160, -3);
            queryButton.Name = "queryButton";
            queryButton.Size = new Size(75, 43);
            queryButton.TabIndex = 1;
            queryButton.Text = "查询";
            queryButton.Type = AntdUI.TTypeMini.Primary;
            queryButton.Click += queryButton_Click;
            // 
            // keywordInput
            // 
            keywordInput.Font = new Font("Microsoft YaHei UI", 11F);
            keywordInput.Location = new Point(-3, -3);
            keywordInput.Name = "keywordInput";
            keywordInput.Size = new Size(160, 45);
            keywordInput.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(spin);
            panel2.Controls.Add(orderTable);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 48);
            panel2.Name = "panel2";
            panel2.Size = new Size(794, 399);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // spin
            // 
            spin.Anchor = AnchorStyles.None;
            spin.BackColor = Color.White;
            spin.Location = new Point(360, 168);
            spin.Name = "spin";
            spin.Size = new Size(75, 58);
            spin.TabIndex = 2;
            spin.Text = "加载中...";
            spin.Visible = false;
            // 
            // orderTable
            // 
            orderTable.BackColor = Color.White;
            orderTable.Dock = DockStyle.Fill;
            orderTable.EditMode = AntdUI.TEditMode.DoubleClick;
            orderTable.Font = new Font("Microsoft YaHei UI", 10F);
            orderTable.Gap = 12;
            orderTable.Location = new Point(0, 0);
            orderTable.Name = "orderTable";
            orderTable.Size = new Size(794, 399);
            orderTable.TabIndex = 0;
            orderTable.Text = "table1";
            // 
            // MaterialShortageReportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "MaterialShortageReportForm";
            Text = "MaterialShortageReportForm";
            Load += MaterialShortageReportForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Table orderTable;
        private AntdUI.Input keywordInput;
        private AntdUI.Button queryButton;
        private AntdUI.Spin spin;
        private AntdUI.Button exportButton;
    }
}