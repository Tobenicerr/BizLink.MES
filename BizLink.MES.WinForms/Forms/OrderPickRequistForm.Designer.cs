namespace BizLink.MES.WinForms.Forms
{
    partial class OrderPickRequistForm
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
            finishDatePicker = new AntdUI.DatePicker();
            exportButton = new AntdUI.Button();
            queryButton = new AntdUI.Button();
            startDatePicker = new AntdUI.DatePicker();
            keyboardInput = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
            requistProgress = new AntdUI.Progress();
            SpinControl = new AntdUI.Spin();
            orderTable = new AntdUI.Table();
            panel3 = new AntdUI.Panel();
            totalLabel = new AntdUI.Label();
            packButton = new AntdUI.Button();
            requistButton = new AntdUI.Button();
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
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(finishDatePicker);
            panel1.Controls.Add(exportButton);
            panel1.Controls.Add(queryButton);
            panel1.Controls.Add(startDatePicker);
            panel1.Controls.Add(keyboardInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(794, 42);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // finishDatePicker
            // 
            finishDatePicker.AllowClear = true;
            finishDatePicker.Anchor = AnchorStyles.Left;
            finishDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            finishDatePicker.Location = new Point(335, -1);
            finishDatePicker.Name = "finishDatePicker";
            finishDatePicker.Size = new Size(160, 43);
            finishDatePicker.TabIndex = 6;
            // 
            // exportButton
            // 
            exportButton.Font = new Font("Microsoft YaHei UI", 11F);
            exportButton.Location = new Point(575, 1);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(75, 40);
            exportButton.TabIndex = 5;
            exportButton.Text = "导出";
            exportButton.Type = AntdUI.TTypeMini.Success;
            exportButton.Click += exportButton_Click;
            // 
            // queryButton
            // 
            queryButton.Font = new Font("Microsoft YaHei UI", 11F);
            queryButton.Location = new Point(501, 1);
            queryButton.Name = "queryButton";
            queryButton.Size = new Size(75, 40);
            queryButton.TabIndex = 4;
            queryButton.Text = "查询";
            queryButton.Type = AntdUI.TTypeMini.Info;
            queryButton.Click += queryButton_Click;
            // 
            // startDatePicker
            // 
            startDatePicker.AllowClear = true;
            startDatePicker.Anchor = AnchorStyles.Left;
            startDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            startDatePicker.Location = new Point(169, -1);
            startDatePicker.Name = "startDatePicker";
            startDatePicker.Size = new Size(160, 43);
            startDatePicker.TabIndex = 1;
            // 
            // keyboardInput
            // 
            keyboardInput.AllowClear = true;
            keyboardInput.Anchor = AnchorStyles.Left;
            keyboardInput.Font = new Font("Microsoft YaHei UI", 11F);
            keyboardInput.Location = new Point(3, -1);
            keyboardInput.Multiline = true;
            keyboardInput.Name = "keyboardInput";
            keyboardInput.Size = new Size(160, 43);
            keyboardInput.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(requistProgress);
            panel2.Controls.Add(SpinControl);
            panel2.Controls.Add(orderTable);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 96);
            panel2.Name = "panel2";
            panel2.Size = new Size(794, 351);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // requistProgress
            // 
            requistProgress.Anchor = AnchorStyles.None;
            requistProgress.BackColor = Color.Transparent;
            requistProgress.Font = new Font("Microsoft YaHei UI", 10F);
            requistProgress.Location = new Point(355, 137);
            requistProgress.Name = "requistProgress";
            requistProgress.Radius = 4;
            requistProgress.Shape = AntdUI.TShapeProgress.Circle;
            requistProgress.Size = new Size(60, 58);
            requistProgress.TabIndex = 2;
            requistProgress.Text = "progress1";
            requistProgress.Visible = false;
            // 
            // SpinControl
            // 
            SpinControl.Anchor = AnchorStyles.None;
            SpinControl.BackColor = Color.Transparent;
            SpinControl.Location = new Point(355, 137);
            SpinControl.Name = "SpinControl";
            SpinControl.Size = new Size(75, 58);
            SpinControl.TabIndex = 1;
            SpinControl.Text = "加载中...";
            SpinControl.Visible = false;
            // 
            // orderTable
            // 
            orderTable.BackColor = Color.White;
            orderTable.Dock = DockStyle.Fill;
            orderTable.Font = new Font("Microsoft YaHei UI", 10F);
            orderTable.Gap = 12;
            orderTable.Location = new Point(0, 0);
            orderTable.Name = "orderTable";
            orderTable.Size = new Size(794, 351);
            orderTable.TabIndex = 0;
            orderTable.Text = "table1";
            orderTable.TreeArrowStyle = AntdUI.TableTreeStyle.Arrow;
            orderTable.ExpandChanged += orderTable_ExpandChanged;
            // 
            // panel3
            // 
            panel3.Controls.Add(totalLabel);
            panel3.Controls.Add(packButton);
            panel3.Controls.Add(requistButton);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 51);
            panel3.Name = "panel3";
            panel3.Size = new Size(794, 39);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // totalLabel
            // 
            totalLabel.Anchor = AnchorStyles.Left;
            totalLabel.BackColor = Color.White;
            totalLabel.Font = new Font("Microsoft YaHei UI", 12F);
            totalLabel.Location = new Point(0, 0);
            totalLabel.Name = "totalLabel";
            totalLabel.Size = new Size(233, 40);
            totalLabel.TabIndex = 4;
            totalLabel.Text = "  订单总数：- 笔";
            // 
            // packButton
            // 
            packButton.Anchor = AnchorStyles.Right;
            packButton.BackColor = Color.LimeGreen;
            packButton.Font = new Font("Microsoft YaHei UI", 11F);
            packButton.Location = new Point(614, 0);
            packButton.Name = "packButton";
            packButton.Size = new Size(90, 40);
            packButton.TabIndex = 3;
            packButton.Text = "订单合箱";
            packButton.Type = AntdUI.TTypeMini.Primary;
            packButton.Click += packButton_Click;
            // 
            // requistButton
            // 
            requistButton.Anchor = AnchorStyles.Right;
            requistButton.BackColor = Color.MidnightBlue;
            requistButton.Font = new Font("Microsoft YaHei UI", 11F);
            requistButton.Location = new Point(701, -1);
            requistButton.Name = "requistButton";
            requistButton.Size = new Size(90, 40);
            requistButton.TabIndex = 2;
            requistButton.Text = "按单领料";
            requistButton.Type = AntdUI.TTypeMini.Primary;
            requistButton.Click += requistButton_Click;
            // 
            // OrderPickRequistForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "OrderPickRequistForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "OrderPickRequistForm";
            Load += OrderPickRequistForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.DatePicker startDatePicker;
        private AntdUI.Input keyboardInput;
        private AntdUI.Panel panel2;
        private AntdUI.Table orderTable;
        private AntdUI.Button queryButton;
        private AntdUI.Button packButton;
        private AntdUI.Button requistButton;
        private AntdUI.Panel panel3;
        private AntdUI.Label totalLabel;
        private AntdUI.Spin SpinControl;
        private AntdUI.Progress requistProgress;
        private AntdUI.Button exportButton;
        private AntdUI.DatePicker finishDatePicker;
    }
}