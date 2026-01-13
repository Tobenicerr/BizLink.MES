namespace BizLink.MES.WinForms.Forms
{
    partial class SapOrderViewForm
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
            tableLayoutPanel2 = new TableLayoutPanel();
            dispatchdatePicker = new AntdUI.DatePicker();
            queryButton = new AntdUI.Button();
            orderInput = new AntdUI.Input();
            cableCutParamButton = new AntdUI.Button();
            panel2 = new AntdUI.Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            totalLabel = new AntdUI.Label();
            orderSyncButton = new AntdUI.Button();
            panel3 = new AntdUI.Panel();
            syncProgress = new AntdUI.Progress();
            orderSpin = new AntdUI.Spin();
            sapOrderTable = new AntdUI.Table();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
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
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(746, 440);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(740, 49);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.White;
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel2.Controls.Add(dispatchdatePicker, 0, 0);
            tableLayoutPanel2.Controls.Add(queryButton, 2, 0);
            tableLayoutPanel2.Controls.Add(orderInput, 1, 0);
            tableLayoutPanel2.Controls.Add(cableCutParamButton, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(740, 49);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // dispatchdatePicker
            // 
            dispatchdatePicker.Dock = DockStyle.Fill;
            dispatchdatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            dispatchdatePicker.Location = new Point(3, 3);
            dispatchdatePicker.Name = "dispatchdatePicker";
            dispatchdatePicker.PrefixText = "";
            dispatchdatePicker.Size = new Size(179, 43);
            dispatchdatePicker.TabIndex = 1;
            dispatchdatePicker.ValueChanged += dispatchdatePicker_ValueChanged;
            // 
            // queryButton
            // 
            queryButton.Anchor = AnchorStyles.Left;
            queryButton.Enabled = false;
            queryButton.Font = new Font("Microsoft YaHei UI", 11F);
            queryButton.Location = new Point(373, 3);
            queryButton.Name = "queryButton";
            queryButton.Size = new Size(86, 42);
            queryButton.TabIndex = 2;
            queryButton.Text = "查询";
            queryButton.Type = AntdUI.TTypeMini.Primary;
            queryButton.Click += queryButton_Click;
            // 
            // orderInput
            // 
            orderInput.AcceptsTab = true;
            orderInput.AllowClear = true;
            orderInput.Dock = DockStyle.Fill;
            orderInput.Font = new Font("Microsoft YaHei UI", 11F);
            orderInput.Location = new Point(188, 3);
            orderInput.Multiline = true;
            orderInput.Name = "orderInput";
            orderInput.PlaceholderText = "";
            orderInput.Size = new Size(179, 43);
            orderInput.TabIndex = 3;
            orderInput.TextChanged += orderInput_TextChanged;
            // 
            // cableCutParamButton
            // 
            cableCutParamButton.Dock = DockStyle.Right;
            cableCutParamButton.Font = new Font("Microsoft YaHei UI", 10F);
            cableCutParamButton.Location = new Point(639, 3);
            cableCutParamButton.Name = "cableCutParamButton";
            cableCutParamButton.Size = new Size(98, 43);
            cableCutParamButton.TabIndex = 4;
            cableCutParamButton.Text = "断线参数同步";
            cableCutParamButton.Type = AntdUI.TTypeMini.Success;
            cableCutParamButton.Click += cableCutParamButton_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(tableLayoutPanel3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 58);
            panel2.Name = "panel2";
            panel2.Size = new Size(740, 49);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.White;
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.Controls.Add(totalLabel, 0, 0);
            tableLayoutPanel3.Controls.Add(orderSyncButton, 3, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(740, 49);
            tableLayoutPanel3.TabIndex = 5;
            // 
            // totalLabel
            // 
            totalLabel.Anchor = AnchorStyles.Left;
            totalLabel.BackColor = Color.White;
            totalLabel.Font = new Font("Microsoft YaHei UI", 12F);
            totalLabel.Location = new Point(3, 5);
            totalLabel.Name = "totalLabel";
            totalLabel.Size = new Size(179, 38);
            totalLabel.TabIndex = 0;
            totalLabel.Text = "  订单总数：- 笔";
            // 
            // orderSyncButton
            // 
            orderSyncButton.Anchor = AnchorStyles.Right;
            orderSyncButton.Font = new Font("Microsoft YaHei UI", 11F);
            orderSyncButton.Location = new Point(639, 3);
            orderSyncButton.Name = "orderSyncButton";
            orderSyncButton.Size = new Size(98, 43);
            orderSyncButton.TabIndex = 4;
            orderSyncButton.Text = "订单同步";
            orderSyncButton.Type = AntdUI.TTypeMini.Primary;
            orderSyncButton.Click += orderSyncButton_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(syncProgress);
            panel3.Controls.Add(orderSpin);
            panel3.Controls.Add(sapOrderTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 113);
            panel3.Name = "panel3";
            panel3.Size = new Size(740, 324);
            panel3.TabIndex = 2;
            panel3.Text = "查询";
            // 
            // syncProgress
            // 
            syncProgress.Anchor = AnchorStyles.None;
            syncProgress.BackColor = Color.White;
            syncProgress.Font = new Font("Microsoft YaHei UI", 11F);
            syncProgress.IconCircleAngle = true;
            syncProgress.Location = new Point(331, 121);
            syncProgress.Name = "syncProgress";
            syncProgress.Radius = 5;
            syncProgress.Shape = AntdUI.TShapeProgress.Circle;
            syncProgress.Size = new Size(75, 64);
            syncProgress.TabIndex = 2;
            syncProgress.Text = "progress1";
            syncProgress.Visible = false;
            // 
            // orderSpin
            // 
            orderSpin.Anchor = AnchorStyles.None;
            orderSpin.BackColor = Color.White;
            orderSpin.Location = new Point(331, 121);
            orderSpin.Name = "orderSpin";
            orderSpin.Size = new Size(75, 64);
            orderSpin.TabIndex = 1;
            orderSpin.Text = "加载中...";
            orderSpin.Visible = false;
            // 
            // sapOrderTable
            // 
            sapOrderTable.BackColor = Color.White;
            sapOrderTable.Dock = DockStyle.Fill;
            sapOrderTable.Font = new Font("Microsoft YaHei UI", 11F);
            sapOrderTable.Gap = 12;
            sapOrderTable.Location = new Point(0, 0);
            sapOrderTable.Name = "sapOrderTable";
            sapOrderTable.Size = new Size(740, 324);
            sapOrderTable.TabIndex = 0;
            sapOrderTable.Text = "table1";
            // 
            // SapOrderViewForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(746, 440);
            Controls.Add(tableLayoutPanel1);
            Name = "SapOrderViewForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "SapOrderViewForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Button queryButton;
        private AntdUI.DatePicker dispatchdatePicker;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Table sapOrderTable;
        private AntdUI.Spin orderSpin;
        private AntdUI.Label totalLabel;
        private AntdUI.Button orderSyncButton;
        private AntdUI.Progress syncProgress;
        private AntdUI.Input orderInput;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private AntdUI.Button cableCutParamButton;
    }
}