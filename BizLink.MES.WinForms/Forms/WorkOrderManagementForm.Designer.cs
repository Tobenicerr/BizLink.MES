namespace BizLink.MES.WinForms.Forms
{
    partial class WorkOrderManagementForm
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new AntdUI.Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            startDatePicker = new AntdUI.DatePicker();
            materialRequistButton = new AntdUI.Button();
            processcardPrintButton = new AntdUI.Button();
            dispatchDatePicker = new AntdUI.DatePicker();
            orderListInput = new AntdUI.Input();
            QueryButton = new AntdUI.Button();
            pickOrderRequistButton = new AntdUI.Button();
            splitter1 = new AntdUI.Splitter();
            requistProgress = new AntdUI.Progress();
            SpinControl = new AntdUI.Spin();
            orderTable = new AntdUI.Table();
            tabs1 = new AntdUI.Tabs();
            tabPage1 = new AntdUI.TabPage();
            tableLayoutPanel4 = new TableLayoutPanel();
            panel5 = new AntdUI.Panel();
            processSpin = new AntdUI.Spin();
            processTable = new AntdUI.Table();
            tabPage2 = new AntdUI.TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            panel7 = new AntdUI.Panel();
            bomSpin = new AntdUI.Spin();
            bomTable = new AntdUI.Table();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitter1).BeginInit();
            splitter1.Panel1.SuspendLayout();
            splitter1.Panel2.SuspendLayout();
            splitter1.SuspendLayout();
            tabs1.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel5.SuspendLayout();
            tabPage2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel7.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(splitter1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(984, 634);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Font = new Font("Microsoft YaHei UI", 11F);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(978, 49);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.White;
            tableLayoutPanel2.ColumnCount = 8;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.48909F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.4891014F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.4891014F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.2897177F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.3738327F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.2897177F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.2897177F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.2897177F));
            tableLayoutPanel2.Controls.Add(startDatePicker, 0, 0);
            tableLayoutPanel2.Controls.Add(materialRequistButton, 7, 0);
            tableLayoutPanel2.Controls.Add(processcardPrintButton, 6, 0);
            tableLayoutPanel2.Controls.Add(dispatchDatePicker, 1, 0);
            tableLayoutPanel2.Controls.Add(orderListInput, 2, 0);
            tableLayoutPanel2.Controls.Add(QueryButton, 3, 0);
            tableLayoutPanel2.Controls.Add(pickOrderRequistButton, 5, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(978, 49);
            tableLayoutPanel2.TabIndex = 12;
            // 
            // startDatePicker
            // 
            startDatePicker.Dock = DockStyle.Fill;
            startDatePicker.Location = new Point(3, 3);
            startDatePicker.Name = "startDatePicker";
            startDatePicker.Size = new Size(145, 43);
            startDatePicker.TabIndex = 9;
            // 
            // materialRequistButton
            // 
            materialRequistButton.Dock = DockStyle.Right;
            materialRequistButton.Font = new Font("Microsoft YaHei UI", 11F);
            materialRequistButton.Location = new Point(877, 3);
            materialRequistButton.Name = "materialRequistButton";
            materialRequistButton.Size = new Size(98, 43);
            materialRequistButton.TabIndex = 6;
            materialRequistButton.Text = "断线领料";
            materialRequistButton.Type = AntdUI.TTypeMini.Primary;
            materialRequistButton.Click += materialRequistButton_Click;
            // 
            // processcardPrintButton
            // 
            processcardPrintButton.Dock = DockStyle.Right;
            processcardPrintButton.Font = new Font("Microsoft YaHei UI", 11F);
            processcardPrintButton.Location = new Point(777, 3);
            processcardPrintButton.Name = "processcardPrintButton";
            processcardPrintButton.Size = new Size(94, 43);
            processcardPrintButton.TabIndex = 7;
            processcardPrintButton.Text = "流转卡打印";
            processcardPrintButton.Type = AntdUI.TTypeMini.Success;
            processcardPrintButton.Click += processcardPrintButton_Click;
            // 
            // dispatchDatePicker
            // 
            dispatchDatePicker.Dock = DockStyle.Fill;
            dispatchDatePicker.Location = new Point(154, 3);
            dispatchDatePicker.Name = "dispatchDatePicker";
            dispatchDatePicker.Size = new Size(145, 43);
            dispatchDatePicker.TabIndex = 5;
            // 
            // orderListInput
            // 
            orderListInput.AcceptsTab = true;
            orderListInput.AllowClear = true;
            orderListInput.Dock = DockStyle.Fill;
            orderListInput.Font = new Font("Microsoft YaHei UI", 11F);
            orderListInput.Location = new Point(305, 3);
            orderListInput.Multiline = true;
            orderListInput.Name = "orderListInput";
            orderListInput.PlaceholderText = "";
            orderListInput.Size = new Size(145, 43);
            orderListInput.TabIndex = 10;
            // 
            // QueryButton
            // 
            QueryButton.Dock = DockStyle.Left;
            QueryButton.Font = new Font("Microsoft YaHei UI", 11F);
            QueryButton.Location = new Point(456, 3);
            QueryButton.Name = "QueryButton";
            QueryButton.Size = new Size(84, 43);
            QueryButton.TabIndex = 1;
            QueryButton.Text = "查询";
            QueryButton.Type = AntdUI.TTypeMini.Primary;
            QueryButton.Click += QueryButton_Click;
            // 
            // pickOrderRequistButton
            // 
            pickOrderRequistButton.Dock = DockStyle.Right;
            pickOrderRequistButton.Font = new Font("Microsoft YaHei UI", 11F);
            pickOrderRequistButton.Location = new Point(677, 3);
            pickOrderRequistButton.Name = "pickOrderRequistButton";
            pickOrderRequistButton.Size = new Size(94, 43);
            pickOrderRequistButton.TabIndex = 11;
            pickOrderRequistButton.Text = "按单领料";
            pickOrderRequistButton.Type = AntdUI.TTypeMini.Primary;
            pickOrderRequistButton.Click += pickOrderRequistButton_Click;
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Fill;
            splitter1.Location = new Point(3, 58);
            splitter1.Name = "splitter1";
            splitter1.Orientation = Orientation.Horizontal;
            // 
            // splitter1.Panel1
            // 
            splitter1.Panel1.BackColor = Color.White;
            splitter1.Panel1.Controls.Add(requistProgress);
            splitter1.Panel1.Controls.Add(SpinControl);
            splitter1.Panel1.Controls.Add(orderTable);
            // 
            // splitter1.Panel2
            // 
            splitter1.Panel2.BackColor = Color.White;
            splitter1.Panel2.Controls.Add(tabs1);
            splitter1.Size = new Size(978, 573);
            splitter1.SplitterDistance = 344;
            splitter1.SplitterSize = 50;
            splitter1.TabIndex = 3;
            // 
            // requistProgress
            // 
            requistProgress.Anchor = AnchorStyles.None;
            requistProgress.Font = new Font("Microsoft YaHei UI", 10F);
            requistProgress.Location = new Point(452, 129);
            requistProgress.Name = "requistProgress";
            requistProgress.Radius = 4;
            requistProgress.Shape = AntdUI.TShapeProgress.Circle;
            requistProgress.Size = new Size(75, 51);
            requistProgress.TabIndex = 2;
            requistProgress.Text = "progress1";
            requistProgress.Visible = false;
            // 
            // SpinControl
            // 
            SpinControl.Anchor = AnchorStyles.None;
            SpinControl.BackColor = Color.White;
            SpinControl.Location = new Point(452, 141);
            SpinControl.Name = "SpinControl";
            SpinControl.Size = new Size(75, 66);
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
            orderTable.RowHeight = 40;
            orderTable.RowHeightHeader = 25;
            orderTable.Size = new Size(978, 344);
            orderTable.TabIndex = 0;
            orderTable.Text = "table1";
            orderTable.CellClick += orderTable_CellClick;
            orderTable.CellButtonClick += orderTable_CellButtonClick;
            // 
            // tabs1
            // 
            tabs1.Controls.Add(tabPage1);
            tabs1.Controls.Add(tabPage2);
            tabs1.Dock = DockStyle.Fill;
            tabs1.Gap = 15;
            tabs1.Location = new Point(0, 0);
            tabs1.Name = "tabs1";
            tabs1.Pages.Add(tabPage1);
            tabs1.Pages.Add(tabPage2);
            tabs1.Size = new Size(978, 225);
            tabs1.Style = styleLine1;
            tabs1.TabIndex = 1;
            tabs1.Text = "tabs1";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel4);
            tabPage1.Location = new Point(3, 34);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(972, 188);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "订单工序";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(panel5, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 99F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 1F));
            tableLayoutPanel4.Size = new Size(972, 188);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // panel5
            // 
            panel5.Controls.Add(processSpin);
            panel5.Controls.Add(processTable);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(3, 3);
            panel5.Name = "panel5";
            panel5.Size = new Size(966, 180);
            panel5.TabIndex = 1;
            panel5.Text = "panel5";
            // 
            // processSpin
            // 
            processSpin.Anchor = AnchorStyles.None;
            processSpin.BackColor = Color.White;
            processSpin.Location = new Point(446, 60);
            processSpin.Name = "processSpin";
            processSpin.Size = new Size(75, 63);
            processSpin.TabIndex = 1;
            processSpin.Text = "加载中...";
            processSpin.Visible = false;
            // 
            // processTable
            // 
            processTable.BackColor = Color.White;
            processTable.Dock = DockStyle.Fill;
            processTable.Font = new Font("Microsoft YaHei UI", 10F);
            processTable.Gap = 12;
            processTable.Location = new Point(0, 0);
            processTable.Name = "processTable";
            processTable.Size = new Size(966, 180);
            processTable.TabIndex = 0;
            processTable.Text = "table2";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(tableLayoutPanel3);
            tabPage2.Location = new Point(-972, -207);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(972, 207);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "订单BOM";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(panel7, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(972, 207);
            tableLayoutPanel3.TabIndex = 1;
            // 
            // panel7
            // 
            panel7.Controls.Add(bomSpin);
            panel7.Controls.Add(bomTable);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(3, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(966, 201);
            panel7.TabIndex = 2;
            panel7.Text = "panel7";
            // 
            // bomSpin
            // 
            bomSpin.Anchor = AnchorStyles.None;
            bomSpin.BackColor = Color.White;
            bomSpin.Location = new Point(446, 64);
            bomSpin.Name = "bomSpin";
            bomSpin.Size = new Size(75, 62);
            bomSpin.TabIndex = 1;
            bomSpin.Text = "加载中...";
            bomSpin.Visible = false;
            // 
            // bomTable
            // 
            bomTable.BackColor = Color.White;
            bomTable.Dock = DockStyle.Fill;
            bomTable.Font = new Font("Microsoft YaHei UI", 10F);
            bomTable.Gap = 12;
            bomTable.Location = new Point(0, 0);
            bomTable.Name = "bomTable";
            bomTable.Size = new Size(966, 201);
            bomTable.TabIndex = 0;
            bomTable.Text = "table3";
            // 
            // WorkOrderManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(984, 634);
            Controls.Add(tableLayoutPanel1);
            Name = "WorkOrderManagementForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "WorkOrderManagementForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            splitter1.Panel1.ResumeLayout(false);
            splitter1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitter1).EndInit();
            splitter1.ResumeLayout(false);
            tabs1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Button QueryButton;
        private AntdUI.Table orderTable;
        private AntdUI.Tabs tabs1;
        private AntdUI.TabPage tabPage1;
        private AntdUI.TabPage tabPage2;
        private AntdUI.Table processTable;
        private AntdUI.Table bomTable;
        private TableLayoutPanel tableLayoutPanel3;
        private AntdUI.Spin SpinControl;
        private AntdUI.Panel panel5;
        private TableLayoutPanel tableLayoutPanel4;
        private AntdUI.Spin processSpin;
        private AntdUI.Panel panel7;
        private AntdUI.Spin bomSpin;
        private AntdUI.DatePicker dispatchDatePicker;
        private AntdUI.Button materialRequistButton;
        private AntdUI.Button processcardPrintButton;
        private AntdUI.DatePicker startDatePicker;
        private AntdUI.Input orderListInput;
        private AntdUI.Button pickOrderRequistButton;
        private AntdUI.Progress requistProgress;
        private AntdUI.Splitter splitter1;
        private TableLayoutPanel tableLayoutPanel2;
    }
}