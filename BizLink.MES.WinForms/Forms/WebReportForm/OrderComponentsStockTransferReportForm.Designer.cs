namespace BizLink.MES.WinForms.Forms.WebReportForm
{
    partial class OrderComponentsStockTransferReportForm
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
            SearchButton = new AntdUI.Button();
            DispatcDatePicker = new AntdUI.DatePicker();
            StartDatePicker = new AntdUI.DatePicker();
            OrderInput = new AntdUI.Input();
            KeywordInput = new AntdUI.Input();
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
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
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
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(SearchButton);
            panel1.Controls.Add(DispatcDatePicker);
            panel1.Controls.Add(StartDatePicker);
            panel1.Controls.Add(OrderInput);
            panel1.Controls.Add(KeywordInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(794, 42);
            panel1.TabIndex = 1;
            panel1.Text = "panel1";
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 11F);
            SearchButton.Location = new Point(667, -1);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(75, 45);
            SearchButton.TabIndex = 4;
            SearchButton.Text = "查询";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += SearchButton_Click;
            // 
            // DispatcDatePicker
            // 
            DispatcDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            DispatcDatePicker.Location = new Point(501, -1);
            DispatcDatePicker.Name = "DispatcDatePicker";
            DispatcDatePicker.Size = new Size(160, 45);
            DispatcDatePicker.TabIndex = 3;
            // 
            // StartDatePicker
            // 
            StartDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            StartDatePicker.Location = new Point(335, -1);
            StartDatePicker.Name = "StartDatePicker";
            StartDatePicker.Size = new Size(160, 45);
            StartDatePicker.TabIndex = 2;
            // 
            // OrderInput
            // 
            OrderInput.Font = new Font("Microsoft YaHei UI", 11F);
            OrderInput.Location = new Point(168, -1);
            OrderInput.Multiline = true;
            OrderInput.Name = "OrderInput";
            OrderInput.Size = new Size(160, 45);
            OrderInput.TabIndex = 1;
            // 
            // KeywordInput
            // 
            KeywordInput.Font = new Font("Microsoft YaHei UI", 11F);
            KeywordInput.Location = new Point(3, -1);
            KeywordInput.Name = "KeywordInput";
            KeywordInput.Size = new Size(160, 45);
            KeywordInput.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(SpinControl);
            panel2.Controls.Add(TableControl);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 51);
            panel2.Name = "panel2";
            panel2.Size = new Size(794, 358);
            panel2.TabIndex = 2;
            panel2.Text = "panel2";
            // 
            // SpinControl
            // 
            SpinControl.Anchor = AnchorStyles.None;
            SpinControl.BackColor = Color.White;
            SpinControl.Location = new Point(357, 140);
            SpinControl.Name = "SpinControl";
            SpinControl.Size = new Size(75, 59);
            SpinControl.TabIndex = 1;
            SpinControl.Text = "加载中...";
            SpinControl.Visible = false;
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
            TableControl.Size = new Size(794, 358);
            TableControl.TabIndex = 0;
            TableControl.Text = "table1";
            // 
            // panel3
            // 
            panel3.Controls.Add(PaginationControl);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 415);
            panel3.Name = "panel3";
            panel3.Size = new Size(794, 32);
            panel3.TabIndex = 3;
            panel3.Text = "panel3";
            // 
            // PaginationControl
            // 
            PaginationControl.BackColor = Color.White;
            PaginationControl.Current = 0;
            PaginationControl.Dock = DockStyle.Fill;
            PaginationControl.Location = new Point(0, 0);
            PaginationControl.Name = "PaginationControl";
            PaginationControl.PageSize = 200;
            PaginationControl.PageSizeOptions = new int[]
    {
    100,
    200,
    500,
    1000
    };
            PaginationControl.ShowSizeChanger = true;
            PaginationControl.Size = new Size(794, 32);
            PaginationControl.TabIndex = 0;
            PaginationControl.Text = "pagination1";
            PaginationControl.ValueChanged += PaginationControl_ValueChanged;
            // 
            // OrderComponentsStockTransferReportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "OrderComponentsStockTransferReportForm";
            Text = "OrderComponentsStockTransferReportForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Input KeywordInput;
        private AntdUI.Panel panel2;
        private AntdUI.Table TableControl;
        private AntdUI.Panel panel3;
        private AntdUI.Pagination PaginationControl;
        private AntdUI.Button SearchButton;
        private AntdUI.DatePicker DispatcDatePicker;
        private AntdUI.DatePicker StartDatePicker;
        private AntdUI.Input OrderInput;
        private AntdUI.Spin SpinControl;
    }
}