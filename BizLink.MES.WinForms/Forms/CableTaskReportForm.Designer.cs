namespace BizLink.MES.WinForms.Forms
{
    partial class CableTaskReportForm
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
            keywordInput = new AntdUI.Input();
            statusSelect = new AntdUI.Select();
            workstationSelect = new AntdUI.Select();
            workcenterSelect = new AntdUI.Select();
            SearchButton = new AntdUI.Button();
            startDatePicker = new AntdUI.DatePicker();
            OrderInput = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
            SpinControl = new AntdUI.Spin();
            TableControl = new AntdUI.Table();
            panel3 = new AntdUI.Panel();
            PaginationControl = new AntdUI.Pagination();
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
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 96F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutPanel1.Size = new Size(934, 548);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(keywordInput);
            panel1.Controls.Add(statusSelect);
            panel1.Controls.Add(workstationSelect);
            panel1.Controls.Add(workcenterSelect);
            panel1.Controls.Add(SearchButton);
            panel1.Controls.Add(startDatePicker);
            panel1.Controls.Add(OrderInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(928, 90);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // keywordInput
            // 
            keywordInput.Anchor = AnchorStyles.Left;
            keywordInput.Font = new Font("Microsoft YaHei UI", 11F);
            keywordInput.Location = new Point(3, 4);
            keywordInput.Name = "keywordInput";
            keywordInput.Size = new Size(200, 40);
            keywordInput.TabIndex = 6;
            // 
            // statusSelect
            // 
            statusSelect.Anchor = AnchorStyles.Left;
            statusSelect.Font = new Font("Microsoft YaHei UI", 11F);
            statusSelect.Location = new Point(616, 4);
            statusSelect.Name = "statusSelect";
            statusSelect.Size = new Size(200, 40);
            statusSelect.TabIndex = 5;
            // 
            // workstationSelect
            // 
            workstationSelect.Anchor = AnchorStyles.Left;
            workstationSelect.Font = new Font("Microsoft YaHei UI", 11F);
            workstationSelect.Location = new Point(411, 47);
            workstationSelect.Name = "workstationSelect";
            workstationSelect.Size = new Size(405, 40);
            workstationSelect.TabIndex = 4;
            // 
            // workcenterSelect
            // 
            workcenterSelect.Anchor = AnchorStyles.Left;
            workcenterSelect.Font = new Font("Microsoft YaHei UI", 11F);
            workcenterSelect.Location = new Point(3, 47);
            workcenterSelect.Name = "workcenterSelect";
            workcenterSelect.Size = new Size(402, 40);
            workcenterSelect.TabIndex = 3;
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 11F);
            SearchButton.Location = new Point(818, 4);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(75, 40);
            SearchButton.TabIndex = 2;
            SearchButton.Text = "查询";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += queryButton_Click;
            // 
            // startDatePicker
            // 
            startDatePicker.Anchor = AnchorStyles.Left;
            startDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            startDatePicker.Location = new Point(411, 4);
            startDatePicker.Name = "startDatePicker";
            startDatePicker.Size = new Size(200, 40);
            startDatePicker.TabIndex = 1;
            // 
            // OrderInput
            // 
            OrderInput.Anchor = AnchorStyles.Left;
            OrderInput.Font = new Font("Microsoft YaHei UI", 11F);
            OrderInput.Location = new Point(205, 4);
            OrderInput.Name = "OrderInput";
            OrderInput.Size = new Size(200, 40);
            OrderInput.TabIndex = 0;
            OrderInput.TextChanged += OrderInput_TextChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(SpinControl);
            panel2.Controls.Add(TableControl);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 99);
            panel2.Name = "panel2";
            panel2.Size = new Size(928, 408);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // SpinControl
            // 
            SpinControl.Anchor = AnchorStyles.None;
            SpinControl.BackColor = Color.Transparent;
            SpinControl.Location = new Point(425, 183);
            SpinControl.Name = "SpinControl";
            SpinControl.Size = new Size(75, 58);
            SpinControl.TabIndex = 1;
            SpinControl.Text = "加载中...";
            SpinControl.Visible = false;
            // 
            // TableControl
            // 
            TableControl.BackColor = Color.Transparent;
            TableControl.Dock = DockStyle.Fill;
            TableControl.EditMode = AntdUI.TEditMode.DoubleClick;
            TableControl.Font = new Font("Microsoft YaHei UI", 10F);
            TableControl.Gap = 12;
            TableControl.Location = new Point(0, 0);
            TableControl.Name = "TableControl";
            TableControl.Size = new Size(928, 408);
            TableControl.TabIndex = 0;
            TableControl.Text = "table1";
            TableControl.TreeArrowStyle = AntdUI.TableTreeStyle.Arrow;
            TableControl.CellButtonClick += orderTable_CellButtonClick;
            TableControl.ExpandChanged += orderTable_ExpandChanged;
            // 
            // panel3
            // 
            panel3.Controls.Add(PaginationControl);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 513);
            panel3.Name = "panel3";
            panel3.Size = new Size(928, 32);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // PaginationControl
            // 
            PaginationControl.BackColor = Color.White;
            PaginationControl.Current = 0;
            PaginationControl.Dock = DockStyle.Fill;
            PaginationControl.Location = new Point(0, 0);
            PaginationControl.Name = "PaginationControl";
            PaginationControl.PageSize = 100;
            PaginationControl.PageSizeOptions = new int[]
    {
    100,
    200,
    500,
    1000
    };
            PaginationControl.ShowSizeChanger = true;
            PaginationControl.Size = new Size(928, 32);
            PaginationControl.TabIndex = 0;
            PaginationControl.Text = "pagination1";
            PaginationControl.ValueChanged += PaginationControl_ValueChanged;
            // 
            // CableTaskReportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(934, 548);
            Controls.Add(tableLayoutPanel1);
            Name = "CableTaskReportForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "CableTaskReportForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Button SearchButton;
        private AntdUI.DatePicker startDatePicker;
        private AntdUI.Input OrderInput;
        private AntdUI.Panel panel2;
        private AntdUI.Spin SpinControl;
        private AntdUI.Table TableControl;
        private AntdUI.Select workstationSelect;
        private AntdUI.Select workcenterSelect;
        private AntdUI.Select statusSelect;
        private AntdUI.Input keywordInput;
        private AntdUI.Panel panel3;
        private AntdUI.Pagination PaginationControl;
    }
}