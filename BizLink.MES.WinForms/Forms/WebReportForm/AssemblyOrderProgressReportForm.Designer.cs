namespace BizLink.MES.WinForms.Forms.WebReportForm
{
    partial class AssemblyOrderProgressReportForm
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
            DispatchDatePicker = new AntdUI.DatePicker();
            ExportButton = new AntdUI.Button();
            SearchButton = new AntdUI.Button();
            WorkOrderInput = new AntdUI.Input();
            WorkCenterSelect = new AntdUI.Select();
            WorkCenterGroupSelect = new AntdUI.Select();
            ConfirmDatePickerRange = new AntdUI.DatePickerRange();
            DispatchDatePickerRange = new AntdUI.DatePickerRange();
            panel2 = new AntdUI.Panel();
            TableControl = new AntdUI.Table();
            panel4 = new AntdUI.Panel();
            PaginationControl = new AntdUI.Pagination();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel4, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutPanel1.Size = new Size(944, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(DispatchDatePicker);
            panel1.Controls.Add(ExportButton);
            panel1.Controls.Add(SearchButton);
            panel1.Controls.Add(WorkOrderInput);
            panel1.Controls.Add(WorkCenterSelect);
            panel1.Controls.Add(WorkCenterGroupSelect);
            panel1.Controls.Add(ConfirmDatePickerRange);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(938, 94);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // DispatchDatePicker
            // 
            DispatchDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            DispatchDatePicker.Location = new Point(3, 3);
            DispatchDatePicker.Name = "DispatchDatePicker";
            DispatchDatePicker.Size = new Size(250, 45);
            DispatchDatePicker.TabIndex = 7;
            // 
            // ExportButton
            // 
            ExportButton.Font = new Font("Microsoft YaHei UI", 11F);
            ExportButton.Location = new Point(606, 49);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(85, 45);
            ExportButton.TabIndex = 6;
            ExportButton.Text = "导出";
            ExportButton.Type = AntdUI.TTypeMini.Success;
            ExportButton.Click += ExportButton_Click;
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 11F);
            SearchButton.Location = new Point(515, 49);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(85, 45);
            SearchButton.TabIndex = 5;
            SearchButton.Text = "查询";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += SearchButton_Click;
            // 
            // WorkOrderInput
            // 
            WorkOrderInput.AllowClear = true;
            WorkOrderInput.Font = new Font("Microsoft YaHei UI", 11F);
            WorkOrderInput.Location = new Point(515, 3);
            WorkOrderInput.Multiline = true;
            WorkOrderInput.Name = "WorkOrderInput";
            WorkOrderInput.Size = new Size(250, 45);
            WorkOrderInput.TabIndex = 4;
            // 
            // WorkCenterSelect
            // 
            WorkCenterSelect.AllowClear = true;
            WorkCenterSelect.Font = new Font("Microsoft YaHei UI", 11F);
            WorkCenterSelect.Location = new Point(259, 48);
            WorkCenterSelect.Name = "WorkCenterSelect";
            WorkCenterSelect.Size = new Size(250, 45);
            WorkCenterSelect.TabIndex = 3;
            // 
            // WorkCenterGroupSelect
            // 
            WorkCenterGroupSelect.AllowClear = true;
            WorkCenterGroupSelect.Font = new Font("Microsoft YaHei UI", 11F);
            WorkCenterGroupSelect.Location = new Point(3, 48);
            WorkCenterGroupSelect.Name = "WorkCenterGroupSelect";
            WorkCenterGroupSelect.Size = new Size(250, 45);
            WorkCenterGroupSelect.TabIndex = 2;
            WorkCenterGroupSelect.SelectedValueChanged += WorkCenterGroupSelect_SelectedValueChanged;
            // 
            // ConfirmDatePickerRange
            // 
            ConfirmDatePickerRange.AllowClear = true;
            ConfirmDatePickerRange.Font = new Font("Microsoft YaHei UI", 11F);
            ConfirmDatePickerRange.Location = new Point(259, 3);
            ConfirmDatePickerRange.Name = "ConfirmDatePickerRange";
            ConfirmDatePickerRange.Size = new Size(250, 45);
            ConfirmDatePickerRange.TabIndex = 1;
            ConfirmDatePickerRange.TextAlign = HorizontalAlignment.Center;
            // 
            // DispatchDatePickerRange
            // 
            DispatchDatePickerRange.AllowClear = true;
            DispatchDatePickerRange.Font = new Font("Microsoft YaHei UI", 11F);
            DispatchDatePickerRange.Location = new Point(679, 3);
            DispatchDatePickerRange.Name = "DispatchDatePickerRange";
            DispatchDatePickerRange.Size = new Size(98, 45);
            DispatchDatePickerRange.TabIndex = 0;
            DispatchDatePickerRange.TextAlign = HorizontalAlignment.Center;
            DispatchDatePickerRange.Visible = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(TableControl);
            panel2.Controls.Add(DispatchDatePickerRange);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 103);
            panel2.Name = "panel2";
            panel2.Size = new Size(938, 306);
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
            TableControl.Size = new Size(938, 306);
            TableControl.TabIndex = 0;
            TableControl.Text = "table1";
            // 
            // panel4
            // 
            panel4.Controls.Add(PaginationControl);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 415);
            panel4.Name = "panel4";
            panel4.Size = new Size(938, 32);
            panel4.TabIndex = 3;
            panel4.Text = "panel4";
            // 
            // PaginationControl
            // 
            PaginationControl.BackColor = Color.White;
            PaginationControl.Dock = DockStyle.Fill;
            PaginationControl.Location = new Point(0, 0);
            PaginationControl.Name = "PaginationControl";
            PaginationControl.PageSize = 1000;
            PaginationControl.PageSizeOptions = new int[]
    {
    1000,
    2000,
    5000,
    10000
    };
            PaginationControl.ShowSizeChanger = true;
            PaginationControl.Size = new Size(938, 32);
            PaginationControl.TabIndex = 0;
            PaginationControl.Text = "pagination1";
            PaginationControl.ValueChanged += PaginationControl_ValueChanged;
            // 
            // AssemblyOrderProgressReportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(944, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "AssemblyOrderProgressReportForm";
            Text = "AssemblyOrderProgressReportForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel4;
        private AntdUI.Button SearchButton;
        private AntdUI.Input WorkOrderInput;
        private AntdUI.Select WorkCenterSelect;
        private AntdUI.Select WorkCenterGroupSelect;
        private AntdUI.DatePickerRange ConfirmDatePickerRange;
        private AntdUI.DatePickerRange DispatchDatePickerRange;
        private AntdUI.Table TableControl;
        private AntdUI.Pagination PaginationControl;
        private AntdUI.Button ExportButton;
        private AntdUI.DatePicker DispatchDatePicker;
    }
}