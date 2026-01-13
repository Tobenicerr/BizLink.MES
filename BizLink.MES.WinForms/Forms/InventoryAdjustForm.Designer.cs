namespace BizLink.MES.WinForms.Forms
{
    partial class InventoryAdjustForm
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
            costCenterSelect = new AntdUI.Select();
            label10 = new AntdUI.Label();
            adjustReasonSelect = new AntdUI.Select();
            label9 = new AntdUI.Label();
            barcodeSelect = new AntdUI.Select();
            batchSelect = new AntdUI.Select();
            materialSelect = new AntdUI.Select();
            orderSelect = new AntdUI.Select();
            label8 = new AntdUI.Label();
            adjustSelect = new AntdUI.Select();
            changeInputNumber = new AntdUI.InputNumber();
            originalInputNumber = new AntdUI.InputNumber();
            label7 = new AntdUI.Label();
            label6 = new AntdUI.Label();
            label5 = new AntdUI.Label();
            label4 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            label2 = new AntdUI.Label();
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
            tableLayoutPanel1.Size = new Size(450, 480);
            tableLayoutPanel1.TabIndex = 0;
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
            label1.TabIndex = 1;
            label1.Text = "库存调整";
            // 
            // panel2
            // 
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
            divider1.TabIndex = 1;
            divider1.Text = "";
            // 
            // panel3
            // 
            panel3.Controls.Add(costCenterSelect);
            panel3.Controls.Add(label10);
            panel3.Controls.Add(adjustReasonSelect);
            panel3.Controls.Add(label9);
            panel3.Controls.Add(barcodeSelect);
            panel3.Controls.Add(batchSelect);
            panel3.Controls.Add(materialSelect);
            panel3.Controls.Add(orderSelect);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(adjustSelect);
            panel3.Controls.Add(changeInputNumber);
            panel3.Controls.Add(originalInputNumber);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 53);
            panel3.Name = "panel3";
            panel3.Size = new Size(444, 379);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // costCenterSelect
            // 
            costCenterSelect.Font = new Font("Microsoft YaHei UI", 11F);
            costCenterSelect.Location = new Point(3, 312);
            costCenterSelect.Name = "costCenterSelect";
            costCenterSelect.Size = new Size(200, 40);
            costCenterSelect.TabIndex = 22;
            // 
            // label10
            // 
            label10.Font = new Font("Microsoft YaHei UI", 10F);
            label10.Location = new Point(11, 288);
            label10.Name = "label10";
            label10.Prefix = "*";
            label10.Size = new Size(75, 23);
            label10.TabIndex = 19;
            label10.Text = "成本中心";
            // 
            // adjustReasonSelect
            // 
            adjustReasonSelect.Font = new Font("Microsoft YaHei UI", 11F);
            adjustReasonSelect.Location = new Point(226, 171);
            adjustReasonSelect.Name = "adjustReasonSelect";
            adjustReasonSelect.Size = new Size(200, 40);
            adjustReasonSelect.TabIndex = 18;
            // 
            // label9
            // 
            label9.Font = new Font("Microsoft YaHei UI", 10F);
            label9.Location = new Point(234, 146);
            label9.Name = "label9";
            label9.Prefix = "*";
            label9.Size = new Size(75, 23);
            label9.TabIndex = 17;
            label9.Text = "调整原因";
            // 
            // barcodeSelect
            // 
            barcodeSelect.Font = new Font("Microsoft YaHei UI", 11F);
            barcodeSelect.Location = new Point(228, 100);
            barcodeSelect.Name = "barcodeSelect";
            barcodeSelect.Size = new Size(200, 40);
            barcodeSelect.TabIndex = 16;
            barcodeSelect.SelectedIndexChanged += barcodeSelect_SelectedIndexChanged;
            // 
            // batchSelect
            // 
            batchSelect.Font = new Font("Microsoft YaHei UI", 11F);
            batchSelect.Location = new Point(228, 29);
            batchSelect.Name = "batchSelect";
            batchSelect.Size = new Size(200, 40);
            batchSelect.TabIndex = 15;
            batchSelect.SelectedIndexChanged += batchSelect_SelectedIndexChanged;
            // 
            // materialSelect
            // 
            materialSelect.Font = new Font("Microsoft YaHei UI", 11F);
            materialSelect.Location = new Point(3, 29);
            materialSelect.Name = "materialSelect";
            materialSelect.Size = new Size(200, 40);
            materialSelect.TabIndex = 14;
            materialSelect.SelectedIndexChanged += materialSelect_SelectedIndexChanged;
            // 
            // orderSelect
            // 
            orderSelect.Font = new Font("Microsoft YaHei UI", 11F);
            orderSelect.Location = new Point(3, 242);
            orderSelect.Name = "orderSelect";
            orderSelect.Size = new Size(200, 40);
            orderSelect.TabIndex = 13;
            // 
            // label8
            // 
            label8.Font = new Font("Microsoft YaHei UI", 10F);
            label8.Location = new Point(11, 217);
            label8.Name = "label8";
            label8.Prefix = "*";
            label8.Size = new Size(75, 23);
            label8.TabIndex = 12;
            label8.Text = "订单号";
            // 
            // adjustSelect
            // 
            adjustSelect.Font = new Font("Microsoft YaHei UI", 11F);
            adjustSelect.Location = new Point(3, 171);
            adjustSelect.Name = "adjustSelect";
            adjustSelect.Size = new Size(200, 40);
            adjustSelect.TabIndex = 11;
            adjustSelect.SelectedValueChanged += adjustSelect_SelectedValueChanged;
            // 
            // changeInputNumber
            // 
            changeInputNumber.DecimalPlaces = 3;
            changeInputNumber.Font = new Font("Microsoft YaHei UI", 11F);
            changeInputNumber.Location = new Point(228, 242);
            changeInputNumber.Name = "changeInputNumber";
            changeInputNumber.SelectionStart = 5;
            changeInputNumber.Size = new Size(200, 40);
            changeInputNumber.TabIndex = 10;
            changeInputNumber.Text = "0.000";
            changeInputNumber.TextAlign = HorizontalAlignment.Right;
            // 
            // originalInputNumber
            // 
            originalInputNumber.DecimalPlaces = 3;
            originalInputNumber.Font = new Font("Microsoft YaHei UI", 11F);
            originalInputNumber.Location = new Point(3, 100);
            originalInputNumber.Name = "originalInputNumber";
            originalInputNumber.ReadOnly = true;
            originalInputNumber.SelectionStart = 5;
            originalInputNumber.Size = new Size(200, 40);
            originalInputNumber.TabIndex = 9;
            originalInputNumber.Text = "0.000";
            originalInputNumber.TextAlign = HorizontalAlignment.Right;
            // 
            // label7
            // 
            label7.Font = new Font("Microsoft YaHei UI", 10F);
            label7.Location = new Point(234, 217);
            label7.Name = "label7";
            label7.Prefix = "*";
            label7.Size = new Size(75, 23);
            label7.TabIndex = 5;
            label7.Text = "调整数量";
            // 
            // label6
            // 
            label6.Font = new Font("Microsoft YaHei UI", 10F);
            label6.Location = new Point(11, 146);
            label6.Name = "label6";
            label6.Prefix = "*";
            label6.Size = new Size(75, 23);
            label6.TabIndex = 4;
            label6.Text = "调整类型";
            // 
            // label5
            // 
            label5.Font = new Font("Microsoft YaHei UI", 10F);
            label5.Location = new Point(11, 75);
            label5.Name = "label5";
            label5.Size = new Size(75, 23);
            label5.TabIndex = 3;
            label5.Text = "数量";
            // 
            // label4
            // 
            label4.Font = new Font("Microsoft YaHei UI", 10F);
            label4.Location = new Point(234, 75);
            label4.Name = "label4";
            label4.Prefix = "*";
            label4.Size = new Size(75, 23);
            label4.TabIndex = 2;
            label4.Text = "标签";
            // 
            // label3
            // 
            label3.Font = new Font("Microsoft YaHei UI", 10F);
            label3.Location = new Point(234, 4);
            label3.Name = "label3";
            label3.Prefix = "*";
            label3.Size = new Size(75, 23);
            label3.TabIndex = 1;
            label3.Text = "批次号";
            // 
            // label2
            // 
            label2.Font = new Font("Microsoft YaHei UI", 10F);
            label2.Location = new Point(11, 4);
            label2.Name = "label2";
            label2.Prefix = "*";
            label2.Size = new Size(75, 23);
            label2.TabIndex = 0;
            label2.Text = "物料号";
            // 
            // panel4
            // 
            panel4.Controls.Add(cancelButton);
            panel4.Controls.Add(submitButton);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 438);
            panel4.Name = "panel4";
            panel4.Size = new Size(444, 39);
            panel4.TabIndex = 3;
            panel4.Text = "panel4";
            // 
            // cancelButton
            // 
            cancelButton.BorderWidth = 1F;
            cancelButton.Location = new Point(283, -1);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 40);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "取消";
            cancelButton.Click += cancelButton_Click;
            // 
            // submitButton
            // 
            submitButton.Location = new Point(360, -1);
            submitButton.Name = "submitButton";
            submitButton.Size = new Size(75, 40);
            submitButton.TabIndex = 2;
            submitButton.Text = "提交";
            submitButton.Type = AntdUI.TTypeMini.Primary;
            submitButton.Click += submitButton_Click;
            // 
            // InventoryAdjustForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 480);
            Controls.Add(tableLayoutPanel1);
            Name = "InventoryAdjustForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "InventoryAdjustForm";
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
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Panel panel4;
        private AntdUI.Label label1;
        private AntdUI.Divider divider1;
        private AntdUI.Button cancelButton;
        private AntdUI.Button submitButton;
        private AntdUI.Select adjustSelect;
        private AntdUI.InputNumber changeInputNumber;
        private AntdUI.InputNumber originalInputNumber;
        private AntdUI.Label label7;
        private AntdUI.Label label6;
        private AntdUI.Label label5;
        private AntdUI.Label label4;
        private AntdUI.Label label3;
        private AntdUI.Label label2;
        private AntdUI.Select orderSelect;
        private AntdUI.Label label8;
        private AntdUI.Select barcodeSelect;
        private AntdUI.Select batchSelect;
        private AntdUI.Select materialSelect;
        private AntdUI.Select adjustReasonSelect;
        private AntdUI.Label label9;
        private AntdUI.Label label10;
        private AntdUI.Select costCenterSelect;
    }
}