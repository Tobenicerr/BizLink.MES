namespace BizLink.MES.WinForms.Forms
{
    partial class InventoryTransferLogForm
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
            queryButton = new AntdUI.Button();
            keywordInput = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
            totalLabel = new AntdUI.Label();
            panel3 = new AntdUI.Panel();
            transferTable = new AntdUI.Table();
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
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel3, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(902, 542);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(queryButton);
            panel1.Controls.Add(keywordInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(896, 44);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // queryButton
            // 
            queryButton.Font = new Font("Microsoft YaHei UI", 11F);
            queryButton.Location = new Point(169, 2);
            queryButton.Name = "queryButton";
            queryButton.Size = new Size(75, 42);
            queryButton.TabIndex = 2;
            queryButton.Text = "查询";
            queryButton.Type = AntdUI.TTypeMini.Primary;
            queryButton.Click += queryButton_Click;
            // 
            // keywordInput
            // 
            keywordInput.Font = new Font("Microsoft YaHei UI", 11F);
            keywordInput.Location = new Point(3, 0);
            keywordInput.Multiline = true;
            keywordInput.Name = "keywordInput";
            keywordInput.Size = new Size(160, 45);
            keywordInput.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(totalLabel);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(896, 36);
            panel2.TabIndex = 2;
            panel2.Text = "panel2";
            // 
            // totalLabel
            // 
            totalLabel.Anchor = AnchorStyles.Left;
            totalLabel.BackColor = Color.White;
            totalLabel.Font = new Font("Microsoft YaHei UI", 12F);
            totalLabel.Location = new Point(0, -3);
            totalLabel.Name = "totalLabel";
            totalLabel.Size = new Size(233, 40);
            totalLabel.TabIndex = 6;
            totalLabel.Text = "  转移记录数：- 笔";
            // 
            // panel3
            // 
            panel3.Controls.Add(transferTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 95);
            panel3.Name = "panel3";
            panel3.Size = new Size(896, 444);
            panel3.TabIndex = 3;
            panel3.Text = "panel3";
            // 
            // transferTable
            // 
            transferTable.BackColor = Color.White;
            transferTable.Dock = DockStyle.Fill;
            transferTable.EditMode = AntdUI.TEditMode.DoubleClick;
            transferTable.Font = new Font("Microsoft YaHei UI", 10F);
            transferTable.Gap = 12;
            transferTable.Location = new Point(0, 0);
            transferTable.Name = "transferTable";
            transferTable.Size = new Size(896, 444);
            transferTable.TabIndex = 0;
            transferTable.Text = "table1";
            transferTable.CellButtonClick += transferTable_CellButtonClick;
            // 
            // InventoryTransferLogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(902, 542);
            Controls.Add(tableLayoutPanel1);
            Name = "InventoryTransferLogForm";
            Text = "InventoryTransferLogForm";
            Load += InventoryTransferLogForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Button queryButton;
        private AntdUI.Input keywordInput;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Label totalLabel;
        private AntdUI.Table transferTable;
    }
}