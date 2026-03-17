namespace BizLink.MES.WinForms.Forms.WebReportForm
{
    partial class WorkOrderPickingReportForm
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
            ExportButton = new AntdUI.Button();
            SearchButton = new AntdUI.Button();
            GroupCodeInput = new AntdUI.Input();
            StartDatePickerRange = new AntdUI.DatePickerRange();
            WorkOrderInput = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
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
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutPanel1.Size = new Size(1114, 573);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(ExportButton);
            panel1.Controls.Add(SearchButton);
            panel1.Controls.Add(GroupCodeInput);
            panel1.Controls.Add(StartDatePickerRange);
            panel1.Controls.Add(WorkOrderInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1108, 42);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // ExportButton
            // 
            ExportButton.Font = new Font("Microsoft YaHei UI", 11F);
            ExportButton.Location = new Point(828, -1);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(75, 45);
            ExportButton.TabIndex = 4;
            ExportButton.Text = "导出";
            ExportButton.Type = AntdUI.TTypeMini.Success;
            ExportButton.Click += ExportButton_Click;
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 11F);
            SearchButton.Location = new Point(753, -1);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(75, 45);
            SearchButton.TabIndex = 3;
            SearchButton.Text = "查询";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += SearchButton_Click;
            // 
            // GroupCodeInput
            // 
            GroupCodeInput.Font = new Font("Microsoft YaHei UI", 11F);
            GroupCodeInput.Location = new Point(503, -1);
            GroupCodeInput.Multiline = true;
            GroupCodeInput.Name = "GroupCodeInput";
            GroupCodeInput.Size = new Size(250, 45);
            GroupCodeInput.TabIndex = 2;
            // 
            // StartDatePickerRange
            // 
            StartDatePickerRange.Font = new Font("Microsoft YaHei UI", 11F);
            StartDatePickerRange.Location = new Point(3, -1);
            StartDatePickerRange.Name = "StartDatePickerRange";
            StartDatePickerRange.Size = new Size(250, 45);
            StartDatePickerRange.TabIndex = 1;
            StartDatePickerRange.TextAlign = HorizontalAlignment.Center;
            // 
            // WorkOrderInput
            // 
            WorkOrderInput.Font = new Font("Microsoft YaHei UI", 11F);
            WorkOrderInput.Location = new Point(253, -1);
            WorkOrderInput.Multiline = true;
            WorkOrderInput.Name = "WorkOrderInput";
            WorkOrderInput.Size = new Size(250, 45);
            WorkOrderInput.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(TableControl);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 51);
            panel2.Name = "panel2";
            panel2.Size = new Size(1108, 481);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // TableControl
            // 
            TableControl.BackColor = Color.White;
            TableControl.Dock = DockStyle.Fill;
            TableControl.EditMode = AntdUI.TEditMode.DoubleClick;
            TableControl.Font = new Font("Microsoft YaHei UI", 10F);
            TableControl.Gap = 12;
            TableControl.Location = new Point(0, 0);
            TableControl.Name = "TableControl";
            TableControl.Size = new Size(1108, 481);
            TableControl.TabIndex = 0;
            TableControl.Text = "table1";
            // 
            // panel3
            // 
            panel3.Controls.Add(PaginationControl);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 538);
            panel3.Name = "panel3";
            panel3.Size = new Size(1108, 32);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // PaginationControl
            // 
            PaginationControl.BackColor = Color.White;
            PaginationControl.Dock = DockStyle.Fill;
            PaginationControl.Location = new Point(0, 0);
            PaginationControl.Name = "PaginationControl";
            PaginationControl.PageSize = 200;
            PaginationControl.PageSizeOptions = new int[]
    {
    200,
    500,
    1000,
    50000
    };
            PaginationControl.ShowSizeChanger = true;
            PaginationControl.Size = new Size(1108, 32);
            PaginationControl.TabIndex = 0;
            PaginationControl.Text = "pagination1";
            PaginationControl.ValueChanged += PaginationControl_ValueChanged;
            // 
            // WorkOrderPickingReportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1114, 573);
            Controls.Add(tableLayoutPanel1);
            Name = "WorkOrderPickingReportForm";
            Text = "WorkOrderPickingReportForm";
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
        private AntdUI.Input GroupCodeInput;
        private AntdUI.DatePickerRange StartDatePickerRange;
        private AntdUI.Input WorkOrderInput;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Table TableControl;
        private AntdUI.Pagination PaginationControl;
        private AntdUI.Button ExportButton;
    }
}