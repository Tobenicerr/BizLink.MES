namespace BizLink.MES.WinForms.Forms
{
    partial class GoodsReceiptManagementForm
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
            panel2 = new AntdUI.Panel();
            panel3 = new AntdUI.Panel();
            panel4 = new AntdUI.Panel();
            panel5 = new AntdUI.Panel();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel3, 0, 2);
            tableLayoutPanel1.Controls.Add(panel4, 1, 1);
            tableLayoutPanel1.Controls.Add(panel5, 1, 3);
            tableLayoutPanel1.Location = new Point(3, 5);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Size = new Size(923, 558);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(917, 44);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // panel2
            // 
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(501, 44);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // panel3
            // 
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 103);
            panel3.Name = "panel3";
            tableLayoutPanel1.SetRowSpan(panel3, 2);
            panel3.Size = new Size(501, 452);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // panel4
            // 
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(510, 53);
            panel4.Name = "panel4";
            tableLayoutPanel1.SetRowSpan(panel4, 2);
            panel4.Size = new Size(410, 364);
            panel4.TabIndex = 3;
            panel4.Text = "panel4";
            // 
            // panel5
            // 
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(510, 423);
            panel5.Name = "panel5";
            panel5.Size = new Size(410, 132);
            panel5.TabIndex = 4;
            panel5.Text = "panel5";
            // 
            // GoodsReceiptManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(929, 575);
            Controls.Add(tableLayoutPanel1);
            Name = "GoodsReceiptManagementForm";
            Text = "GoodsReceiptManagementForm";
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Panel panel4;
        private AntdUI.Panel panel5;
    }
}