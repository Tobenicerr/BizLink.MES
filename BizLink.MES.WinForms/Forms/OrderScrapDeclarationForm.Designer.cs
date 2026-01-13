namespace BizLink.MES.WinForms.Forms
{
    partial class OrderScrapDeclarationForm
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
            SapBomTable = new AntdUI.Table();
            panel1 = new AntdUI.Panel();
            WorkOrderInput = new AntdUI.Input();
            WorkCenterSelectMultiple = new AntdUI.SelectMultiple();
            SearchButton = new AntdUI.Button();
            DispatchDatePicker = new AntdUI.DatePicker();
            panel2 = new AntdUI.Panel();
            KeywordInput = new AntdUI.Input();
            panel6 = new AntdUI.Panel();
            ScrapInputTable = new AntdUI.Table();
            RemarkInput = new AntdUI.Input();
            ScrapInputNumber = new AntdUI.InputNumber();
            ScrapReasonSelect = new AntdUI.Select();
            BomSelect = new AntdUI.Select();
            panel3 = new AntdUI.Panel();
            SapOrderTable = new AntdUI.Table();
            panel4 = new AntdUI.Panel();
            SubmitButton = new AntdUI.Button();
            ResetButton = new AntdUI.Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel6.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 400F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(SapBomTable, 1, 4);
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel6, 1, 1);
            tableLayoutPanel1.Controls.Add(panel3, 0, 2);
            tableLayoutPanel1.Controls.Add(panel4, 1, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 65F));
            tableLayoutPanel1.Size = new Size(950, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // SapBomTable
            // 
            SapBomTable.BackColor = Color.White;
            SapBomTable.Dock = DockStyle.Fill;
            SapBomTable.EditAutoHeight = true;
            SapBomTable.EditMode = AntdUI.TEditMode.Click;
            SapBomTable.Font = new Font("Microsoft YaHei UI", 10F);
            SapBomTable.Gap = 12;
            SapBomTable.Location = new Point(403, 253);
            SapBomTable.Name = "SapBomTable";
            SapBomTable.Size = new Size(544, 194);
            SapBomTable.TabIndex = 1;
            SapBomTable.Text = "table2";
            SapBomTable.TreeArrowStyle = AntdUI.TableTreeStyle.Arrow;
            SapBomTable.CellButtonClick += SapBomTable_CellButtonClick;
            SapBomTable.ExpandChanged += SapBomTable_ExpandChanged;
            // 
            // panel1
            // 
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(WorkOrderInput);
            panel1.Controls.Add(WorkCenterSelectMultiple);
            panel1.Controls.Add(SearchButton);
            panel1.Controls.Add(DispatchDatePicker);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(944, 42);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // WorkOrderInput
            // 
            WorkOrderInput.AllowClear = true;
            WorkOrderInput.Font = new Font("Microsoft YaHei UI", 11F);
            WorkOrderInput.Location = new Point(503, 0);
            WorkOrderInput.Multiline = true;
            WorkOrderInput.Name = "WorkOrderInput";
            WorkOrderInput.Size = new Size(250, 45);
            WorkOrderInput.TabIndex = 4;
            // 
            // WorkCenterSelectMultiple
            // 
            WorkCenterSelectMultiple.AllowClear = true;
            WorkCenterSelectMultiple.Font = new Font("Microsoft YaHei UI", 11F);
            WorkCenterSelectMultiple.Location = new Point(253, 0);
            WorkCenterSelectMultiple.Name = "WorkCenterSelectMultiple";
            WorkCenterSelectMultiple.Size = new Size(250, 45);
            WorkCenterSelectMultiple.TabIndex = 3;
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 11F);
            SearchButton.Location = new Point(753, 2);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(75, 40);
            SearchButton.TabIndex = 2;
            SearchButton.Text = "查询";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += SearchButton_Click;
            // 
            // DispatchDatePicker
            // 
            DispatchDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            DispatchDatePicker.Location = new Point(3, 0);
            DispatchDatePicker.Name = "DispatchDatePicker";
            DispatchDatePicker.Size = new Size(250, 45);
            DispatchDatePicker.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(KeywordInput);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 51);
            panel2.Name = "panel2";
            panel2.Size = new Size(394, 44);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // KeywordInput
            // 
            KeywordInput.Dock = DockStyle.Fill;
            KeywordInput.Font = new Font("Microsoft YaHei UI", 11F);
            KeywordInput.Location = new Point(0, 0);
            KeywordInput.Name = "KeywordInput";
            KeywordInput.Size = new Size(394, 44);
            KeywordInput.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.Controls.Add(ScrapInputTable);
            panel6.Controls.Add(RemarkInput);
            panel6.Controls.Add(ScrapInputNumber);
            panel6.Controls.Add(ScrapReasonSelect);
            panel6.Controls.Add(BomSelect);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(403, 51);
            panel6.Name = "panel6";
            tableLayoutPanel1.SetRowSpan(panel6, 2);
            panel6.Size = new Size(544, 151);
            panel6.TabIndex = 5;
            panel6.Text = "panel6";
            // 
            // ScrapInputTable
            // 
            ScrapInputTable.BackColor = Color.White;
            ScrapInputTable.Dock = DockStyle.Fill;
            ScrapInputTable.EditAutoHeight = true;
            ScrapInputTable.EditMode = AntdUI.TEditMode.Click;
            ScrapInputTable.Font = new Font("Microsoft YaHei UI", 10F);
            ScrapInputTable.Gap = 12;
            ScrapInputTable.Location = new Point(0, 0);
            ScrapInputTable.Name = "ScrapInputTable";
            ScrapInputTable.Size = new Size(544, 151);
            ScrapInputTable.TabIndex = 5;
            ScrapInputTable.Text = "table2";
            ScrapInputTable.TreeArrowStyle = AntdUI.TableTreeStyle.Arrow;
            // 
            // RemarkInput
            // 
            RemarkInput.Font = new Font("Microsoft YaHei UI", 11F);
            RemarkInput.Location = new Point(229, 49);
            RemarkInput.Name = "RemarkInput";
            RemarkInput.Size = new Size(220, 45);
            RemarkInput.TabIndex = 4;
            RemarkInput.Visible = false;
            // 
            // ScrapInputNumber
            // 
            ScrapInputNumber.Font = new Font("Microsoft YaHei UI", 11F);
            ScrapInputNumber.Location = new Point(229, 3);
            ScrapInputNumber.Name = "ScrapInputNumber";
            ScrapInputNumber.Size = new Size(220, 45);
            ScrapInputNumber.TabIndex = 2;
            ScrapInputNumber.Text = "0";
            ScrapInputNumber.Visible = false;
            // 
            // ScrapReasonSelect
            // 
            ScrapReasonSelect.Font = new Font("Microsoft YaHei UI", 11F);
            ScrapReasonSelect.Location = new Point(3, 49);
            ScrapReasonSelect.Name = "ScrapReasonSelect";
            ScrapReasonSelect.Size = new Size(220, 45);
            ScrapReasonSelect.TabIndex = 1;
            ScrapReasonSelect.Visible = false;
            // 
            // BomSelect
            // 
            BomSelect.Font = new Font("Microsoft YaHei UI", 11F);
            BomSelect.Location = new Point(3, 3);
            BomSelect.Name = "BomSelect";
            BomSelect.Size = new Size(220, 45);
            BomSelect.TabIndex = 0;
            BomSelect.Visible = false;
            // 
            // panel3
            // 
            panel3.Controls.Add(SapOrderTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 101);
            panel3.Name = "panel3";
            tableLayoutPanel1.SetRowSpan(panel3, 3);
            panel3.Size = new Size(394, 346);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // SapOrderTable
            // 
            SapOrderTable.BackColor = Color.White;
            SapOrderTable.Dock = DockStyle.Fill;
            SapOrderTable.Font = new Font("Microsoft YaHei UI", 10F);
            SapOrderTable.Gap = 12;
            SapOrderTable.Location = new Point(0, 0);
            SapOrderTable.Name = "SapOrderTable";
            SapOrderTable.Size = new Size(394, 346);
            SapOrderTable.TabIndex = 0;
            SapOrderTable.Text = "table1";
            SapOrderTable.CellClick += SapOrderTable_CellClick;
            // 
            // panel4
            // 
            panel4.Controls.Add(SubmitButton);
            panel4.Controls.Add(ResetButton);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(403, 208);
            panel4.Name = "panel4";
            panel4.Size = new Size(544, 39);
            panel4.TabIndex = 6;
            panel4.Text = "panel4";
            // 
            // SubmitButton
            // 
            SubmitButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            SubmitButton.Font = new Font("Microsoft YaHei UI", 11F);
            SubmitButton.Location = new Point(464, -1);
            SubmitButton.Name = "SubmitButton";
            SubmitButton.Size = new Size(80, 42);
            SubmitButton.TabIndex = 2;
            SubmitButton.Text = "提交";
            SubmitButton.Type = AntdUI.TTypeMini.Error;
            SubmitButton.Click += SubmitButton_Click;
            // 
            // ResetButton
            // 
            ResetButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ResetButton.BorderWidth = 1F;
            ResetButton.Font = new Font("Microsoft YaHei UI", 11F);
            ResetButton.Location = new Point(383, -1);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(80, 42);
            ResetButton.TabIndex = 3;
            ResetButton.Text = "重置";
            ResetButton.Click += ResetButton_Click;
            // 
            // OrderScrapDeclarationForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(950, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "OrderScrapDeclarationForm";
            Text = "OrderScrapDeclarationForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel6;
        private AntdUI.Panel panel7;
        private AntdUI.DatePicker DispatchDatePicker;
        private AntdUI.Button SearchButton;
        private AntdUI.Input KeywordInput;
        private AntdUI.Button ResetButton;
        private AntdUI.Button SubmitButton;
        private AntdUI.Table SapBomTable;
        private AntdUI.Panel panel3;
        private AntdUI.Table SapOrderTable;
        private AntdUI.SelectMultiple WorkCenterSelectMultiple;
        private AntdUI.Input WorkOrderInput;
        private AntdUI.InputNumber ScrapInputNumber;
        private AntdUI.Select ScrapReasonSelect;
        private AntdUI.Select BomSelect;
        private AntdUI.Input RemarkInput;
        private AntdUI.Panel panel4;
        private AntdUI.Table ScrapInputTable;
    }
}