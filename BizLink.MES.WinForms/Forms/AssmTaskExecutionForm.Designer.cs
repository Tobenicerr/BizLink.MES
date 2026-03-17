namespace BizLink.MES.WinForms.Forms
{
    partial class AssmTaskExecutionForm
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
            AntdUI.Tabs.StyleLine styleLine2 = new AntdUI.Tabs.StyleLine();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new AntdUI.Panel();
            DispathchDatePickerRange = new AntdUI.DatePickerRange();
            WorkCenterSelect = new AntdUI.Select();
            TaskCategorySelect = new AntdUI.SelectMultiple();
            SearchButton = new AntdUI.Button();
            WorkCenterGroupSelect = new AntdUI.Select();
            panel2 = new AntdUI.Panel();
            OrderScanInput = new AntdUI.Input();
            panel3 = new AntdUI.Panel();
            TaskTable = new AntdUI.Table();
            panel4 = new AntdUI.Panel();
            tabs1 = new AntdUI.Tabs();
            tabPage1 = new AntdUI.TabPage();
            BomTable = new AntdUI.Table();
            panel5 = new AntdUI.Panel();
            OpLabel = new AntdUI.Label();
            MatdLabel = new AntdUI.Label();
            MatcLabel = new AntdUI.Label();
            TaskStatusTag = new AntdUI.Tag();
            ProgressLabel = new AntdUI.Label();
            OrderNoLabel = new AntdUI.Label();
            panel6 = new AntdUI.Panel();
            EmpSelect = new AntdUI.Select();
            label3 = new AntdUI.Label();
            SubmitButton = new AntdUI.Button();
            RemarkInput = new AntdUI.Input();
            label4 = new AntdUI.Label();
            ScrapLenInputNumber = new AntdUI.InputNumber();
            label2 = new AntdUI.Label();
            ReportInputNumber = new AntdUI.InputNumber();
            label1 = new AntdUI.Label();
            panel7 = new AntdUI.Panel();
            button1 = new AntdUI.Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            tabs1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel5.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 48F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 280F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel3, 0, 2);
            tableLayoutPanel1.Controls.Add(panel4, 1, 2);
            tableLayoutPanel1.Controls.Add(panel5, 2, 1);
            tableLayoutPanel1.Controls.Add(panel6, 2, 3);
            tableLayoutPanel1.Controls.Add(panel7, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(876, 569);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            tableLayoutPanel1.SetColumnSpan(panel1, 3);
            panel1.Controls.Add(DispathchDatePickerRange);
            panel1.Controls.Add(WorkCenterSelect);
            panel1.Controls.Add(TaskCategorySelect);
            panel1.Controls.Add(SearchButton);
            panel1.Controls.Add(WorkCenterGroupSelect);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(870, 84);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // DispathchDatePickerRange
            // 
            DispathchDatePickerRange.Font = new Font("Microsoft YaHei UI", 10F);
            DispathchDatePickerRange.Location = new Point(4, 0);
            DispathchDatePickerRange.Name = "DispathchDatePickerRange";
            DispathchDatePickerRange.Size = new Size(280, 42);
            DispathchDatePickerRange.TabIndex = 28;
            DispathchDatePickerRange.TextAlign = HorizontalAlignment.Center;
            // 
            // WorkCenterSelect
            // 
            WorkCenterSelect.AllowClear = true;
            WorkCenterSelect.CloseIcon = true;
            WorkCenterSelect.Font = new Font("Microsoft YaHei UI", 10F);
            WorkCenterSelect.ListAutoWidth = true;
            WorkCenterSelect.Location = new Point(563, 0);
            WorkCenterSelect.Name = "WorkCenterSelect";
            WorkCenterSelect.Size = new Size(280, 42);
            WorkCenterSelect.TabIndex = 27;
            // 
            // TaskCategorySelect
            // 
            TaskCategorySelect.Font = new Font("Microsoft YaHei UI", 10F);
            TaskCategorySelect.Location = new Point(4, 42);
            TaskCategorySelect.Name = "TaskCategorySelect";
            TaskCategorySelect.Size = new Size(752, 42);
            TaskCategorySelect.TabIndex = 25;
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 10F);
            SearchButton.Location = new Point(759, 44);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(84, 40);
            SearchButton.TabIndex = 24;
            SearchButton.Text = "查询";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += SearchButton_Click;
            // 
            // WorkCenterGroupSelect
            // 
            WorkCenterGroupSelect.AllowClear = true;
            WorkCenterGroupSelect.CloseIcon = true;
            WorkCenterGroupSelect.Font = new Font("Microsoft YaHei UI", 10F);
            WorkCenterGroupSelect.ListAutoWidth = true;
            WorkCenterGroupSelect.Location = new Point(284, 0);
            WorkCenterGroupSelect.Name = "WorkCenterGroupSelect";
            WorkCenterGroupSelect.Size = new Size(280, 42);
            WorkCenterGroupSelect.TabIndex = 23;
            WorkCenterGroupSelect.SelectedValueChanged += WorkCenterGroupSelect_SelectedValueChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(OrderScanInput);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 93);
            panel2.Name = "panel2";
            panel2.Size = new Size(280, 44);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // OrderScanInput
            // 
            OrderScanInput.Dock = DockStyle.Fill;
            OrderScanInput.Font = new Font("Microsoft YaHei UI", 10F);
            OrderScanInput.Location = new Point(0, 0);
            OrderScanInput.Name = "OrderScanInput";
            OrderScanInput.Size = new Size(280, 44);
            OrderScanInput.TabIndex = 0;
            OrderScanInput.KeyPress += OrderScanInput_KeyPress;
            // 
            // panel3
            // 
            panel3.Controls.Add(TaskTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 143);
            panel3.Name = "panel3";
            tableLayoutPanel1.SetRowSpan(panel3, 5);
            panel3.Size = new Size(280, 423);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // TaskTable
            // 
            TaskTable.BackColor = Color.White;
            TaskTable.Dock = DockStyle.Fill;
            TaskTable.EditMode = AntdUI.TEditMode.DoubleClick;
            TaskTable.Font = new Font("Microsoft YaHei UI", 10F);
            TaskTable.Gap = 12;
            TaskTable.Location = new Point(0, 0);
            TaskTable.Name = "TaskTable";
            TaskTable.Size = new Size(280, 423);
            TaskTable.TabIndex = 0;
            TaskTable.Text = "table1";
            TaskTable.CellClick += TaskTable_CellClick;
            // 
            // panel4
            // 
            panel4.Controls.Add(tabs1);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(289, 143);
            panel4.Name = "panel4";
            tableLayoutPanel1.SetRowSpan(panel4, 5);
            panel4.Size = new Size(303, 423);
            panel4.TabIndex = 3;
            panel4.Text = "panel4";
            // 
            // tabs1
            // 
            tabs1.BackColor = Color.White;
            tabs1.Controls.Add(tabPage1);
            tabs1.Dock = DockStyle.Fill;
            tabs1.Location = new Point(0, 0);
            tabs1.Name = "tabs1";
            tabs1.Pages.Add(tabPage1);
            tabs1.Size = new Size(303, 423);
            tabs1.Style = styleLine2;
            tabs1.TabIndex = 1;
            tabs1.Text = "tabs1";
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(BomTable);
            tabPage1.Dock = DockStyle.Fill;
            tabPage1.Location = new Point(0, 30);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(303, 393);
            tabPage1.TabIndex = 1;
            tabPage1.Text = "订单Bom";
            // 
            // BomTable
            // 
            BomTable.Dock = DockStyle.Fill;
            BomTable.Font = new Font("Microsoft YaHei UI", 10F);
            BomTable.Gap = 12;
            BomTable.Location = new Point(0, 0);
            BomTable.Name = "BomTable";
            BomTable.Size = new Size(303, 393);
            BomTable.TabIndex = 0;
            BomTable.Text = "table2";
            // 
            // panel5
            // 
            panel5.Controls.Add(OpLabel);
            panel5.Controls.Add(MatdLabel);
            panel5.Controls.Add(MatcLabel);
            panel5.Controls.Add(TaskStatusTag);
            panel5.Controls.Add(ProgressLabel);
            panel5.Controls.Add(OrderNoLabel);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(598, 93);
            panel5.Name = "panel5";
            tableLayoutPanel1.SetRowSpan(panel5, 2);
            panel5.Size = new Size(275, 138);
            panel5.TabIndex = 4;
            panel5.Text = "panel5";
            // 
            // OpLabel
            // 
            OpLabel.BackColor = Color.White;
            OpLabel.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
            OpLabel.Location = new Point(3, 38);
            OpLabel.Name = "OpLabel";
            OpLabel.Size = new Size(70, 23);
            OpLabel.TabIndex = 18;
            OpLabel.Text = "0100";
            // 
            // MatdLabel
            // 
            MatdLabel.BackColor = Color.White;
            MatdLabel.Font = new Font("Microsoft YaHei UI", 10F);
            MatdLabel.Location = new Point(3, 96);
            MatdLabel.Name = "MatdLabel";
            MatdLabel.Size = new Size(272, 42);
            MatdLabel.TabIndex = 17;
            MatdLabel.Text = "CABLE L-YY  8X1X0.86  VZN BK UL2464";
            MatdLabel.TextAlign = ContentAlignment.TopLeft;
            // 
            // MatcLabel
            // 
            MatcLabel.BackColor = Color.White;
            MatcLabel.Font = new Font("Microsoft YaHei UI", 13F);
            MatcLabel.Location = new Point(3, 67);
            MatcLabel.Name = "MatcLabel";
            MatcLabel.Size = new Size(269, 23);
            MatcLabel.TabIndex = 16;
            MatcLabel.Text = "840894";
            // 
            // TaskStatusTag
            // 
            TaskStatusTag.Font = new Font("Microsoft YaHei UI", 10F);
            TaskStatusTag.Location = new Point(202, 38);
            TaskStatusTag.Name = "TaskStatusTag";
            TaskStatusTag.Size = new Size(70, 25);
            TaskStatusTag.TabIndex = 15;
            TaskStatusTag.Text = "";
            // 
            // ProgressLabel
            // 
            ProgressLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ProgressLabel.BackColor = Color.White;
            ProgressLabel.Font = new Font("Microsoft YaHei UI", 15F);
            ProgressLabel.Location = new Point(153, 3);
            ProgressLabel.Name = "ProgressLabel";
            ProgressLabel.Size = new Size(119, 29);
            ProgressLabel.TabIndex = 14;
            ProgressLabel.Text = "11/50";
            ProgressLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // OrderNoLabel
            // 
            OrderNoLabel.BackColor = Color.White;
            OrderNoLabel.Font = new Font("Microsoft YaHei UI", 15F);
            OrderNoLabel.Location = new Point(3, 3);
            OrderNoLabel.Name = "OrderNoLabel";
            OrderNoLabel.Size = new Size(93, 29);
            OrderNoLabel.TabIndex = 13;
            OrderNoLabel.Text = "3600000";
            // 
            // panel6
            // 
            panel6.Controls.Add(EmpSelect);
            panel6.Controls.Add(label3);
            panel6.Controls.Add(SubmitButton);
            panel6.Controls.Add(RemarkInput);
            panel6.Controls.Add(label4);
            panel6.Controls.Add(ScrapLenInputNumber);
            panel6.Controls.Add(label2);
            panel6.Controls.Add(ReportInputNumber);
            panel6.Controls.Add(label1);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(598, 237);
            panel6.Name = "panel6";
            tableLayoutPanel1.SetRowSpan(panel6, 4);
            panel6.Size = new Size(275, 329);
            panel6.TabIndex = 5;
            panel6.Text = "panel6";
            // 
            // EmpSelect
            // 
            EmpSelect.Dock = DockStyle.Top;
            EmpSelect.Font = new Font("Microsoft YaHei UI", 10F);
            EmpSelect.Location = new Point(0, 227);
            EmpSelect.Name = "EmpSelect";
            EmpSelect.ReadOnly = true;
            EmpSelect.Size = new Size(275, 45);
            EmpSelect.TabIndex = 17;
            EmpSelect.DoubleClick += EmpSelect_DoubleClick;
            // 
            // label3
            // 
            label3.BackColor = Color.White;
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(0, 204);
            label3.Name = "label3";
            label3.Size = new Size(275, 23);
            label3.TabIndex = 16;
            label3.Text = "报工人员";
            // 
            // SubmitButton
            // 
            SubmitButton.Dock = DockStyle.Bottom;
            SubmitButton.Font = new Font("Microsoft YaHei UI", 10F);
            SubmitButton.Location = new Point(0, 284);
            SubmitButton.Name = "SubmitButton";
            SubmitButton.Size = new Size(275, 45);
            SubmitButton.TabIndex = 15;
            SubmitButton.Text = "报工提交";
            SubmitButton.Type = AntdUI.TTypeMini.Primary;
            SubmitButton.Click += SubmitButton_Click;
            // 
            // RemarkInput
            // 
            RemarkInput.Dock = DockStyle.Top;
            RemarkInput.Font = new Font("Microsoft YaHei UI", 10F);
            RemarkInput.Location = new Point(0, 159);
            RemarkInput.Name = "RemarkInput";
            RemarkInput.Size = new Size(275, 45);
            RemarkInput.TabIndex = 14;
            // 
            // label4
            // 
            label4.BackColor = Color.White;
            label4.Dock = DockStyle.Top;
            label4.Location = new Point(0, 136);
            label4.Name = "label4";
            label4.Size = new Size(275, 23);
            label4.TabIndex = 13;
            label4.Text = "生产备注";
            // 
            // ScrapLenInputNumber
            // 
            ScrapLenInputNumber.Dock = DockStyle.Top;
            ScrapLenInputNumber.Font = new Font("Microsoft YaHei UI", 10F);
            ScrapLenInputNumber.Location = new Point(0, 91);
            ScrapLenInputNumber.Name = "ScrapLenInputNumber";
            ScrapLenInputNumber.ReadOnly = true;
            ScrapLenInputNumber.Size = new Size(275, 45);
            ScrapLenInputNumber.SuffixText = "PCS";
            ScrapLenInputNumber.TabIndex = 12;
            ScrapLenInputNumber.Text = "0";
            // 
            // label2
            // 
            label2.BackColor = Color.White;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(0, 68);
            label2.Name = "label2";
            label2.Size = new Size(275, 23);
            label2.TabIndex = 11;
            label2.Text = "报废数量";
            // 
            // ReportInputNumber
            // 
            ReportInputNumber.Dock = DockStyle.Top;
            ReportInputNumber.Font = new Font("Microsoft YaHei UI", 10F);
            ReportInputNumber.Location = new Point(0, 23);
            ReportInputNumber.Name = "ReportInputNumber";
            ReportInputNumber.Size = new Size(275, 45);
            ReportInputNumber.SuffixText = "PCS";
            ReportInputNumber.TabIndex = 10;
            ReportInputNumber.Text = "0";
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(275, 23);
            label1.TabIndex = 9;
            label1.Text = "报工数量";
            // 
            // panel7
            // 
            panel7.Controls.Add(button1);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(289, 93);
            panel7.Name = "panel7";
            panel7.Size = new Size(303, 44);
            panel7.TabIndex = 6;
            panel7.Text = "panel7";
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.Font = new Font("Microsoft YaHei UI", 10F);
            button1.Location = new Point(0, 0);
            button1.Name = "button1";
            button1.Size = new Size(303, 44);
            button1.TabIndex = 0;
            button1.Text = "WI文件查看";
            button1.Type = AntdUI.TTypeMini.Success;
            // 
            // AssmTaskExecutionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(876, 569);
            Controls.Add(tableLayoutPanel1);
            Name = "AssmTaskExecutionForm";
            Text = "AssmTaskExecutionForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            tabs1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Panel panel4;
        private AntdUI.Panel panel5;
        private AntdUI.Panel panel6;
        private AntdUI.Select WorkCenterSelect;
        private AntdUI.SelectMultiple TaskCategorySelect;
        private AntdUI.Button SearchButton;
        private AntdUI.Select WorkCenterGroupSelect;
        private AntdUI.Input OrderScanInput;
        private AntdUI.Table TaskTable;
        private AntdUI.Table BomTable;
        private AntdUI.Tag TaskStatusTag;
        private AntdUI.Label ProgressLabel;
        private AntdUI.Label OrderNoLabel;
        private AntdUI.Label MatcLabel;
        private AntdUI.Label MatdLabel;
        private AntdUI.Button SubmitButton;
        private AntdUI.Input RemarkInput;
        private AntdUI.Label label4;
        private AntdUI.InputNumber ScrapLenInputNumber;
        private AntdUI.Label label2;
        private AntdUI.InputNumber ReportInputNumber;
        private AntdUI.Label label1;
        private AntdUI.Tabs tabs1;
        private AntdUI.TabPage tabPage1;
        private AntdUI.DatePickerRange DispathchDatePickerRange;
        private AntdUI.Select EmpSelect;
        private AntdUI.Label label3;
        private AntdUI.Label OpLabel;
        private AntdUI.Panel panel7;
        private AntdUI.Button button1;
    }
}