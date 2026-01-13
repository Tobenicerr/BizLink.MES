namespace BizLink.MES.WinForms.Forms.WebReportForm
{
    partial class WmsMaterialStockReportForm
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
            BatchInput = new AntdUI.Input();
            MaterialInput = new AntdUI.Input();
            ExportButton = new AntdUI.Button();
            SearchButton = new AntdUI.Button();
            keywordInput = new AntdUI.Input();
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
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(BatchInput);
            panel1.Controls.Add(MaterialInput);
            panel1.Controls.Add(ExportButton);
            panel1.Controls.Add(SearchButton);
            panel1.Controls.Add(keywordInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(794, 39);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // BatchInput
            // 
            BatchInput.Font = new Font("Microsoft YaHei UI", 11F);
            BatchInput.Location = new Point(313, -3);
            BatchInput.Multiline = true;
            BatchInput.Name = "BatchInput";
            BatchInput.Size = new Size(160, 43);
            BatchInput.TabIndex = 4;
            // 
            // MaterialInput
            // 
            MaterialInput.Font = new Font("Microsoft YaHei UI", 11F);
            MaterialInput.Location = new Point(157, -3);
            MaterialInput.Multiline = true;
            MaterialInput.Name = "MaterialInput";
            MaterialInput.Size = new Size(160, 43);
            MaterialInput.TabIndex = 3;
            // 
            // ExportButton
            // 
            ExportButton.Font = new Font("Microsoft YaHei UI", 11F);
            ExportButton.Location = new Point(552, -3);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(75, 42);
            ExportButton.TabIndex = 2;
            ExportButton.Text = "导出";
            ExportButton.Type = AntdUI.TTypeMini.Success;
            ExportButton.Click += ExportButton_Click;
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 11F);
            SearchButton.Location = new Point(479, -3);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(75, 42);
            SearchButton.TabIndex = 1;
            SearchButton.Text = "查询";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += SearchButton_Click;
            // 
            // keywordInput
            // 
            keywordInput.Font = new Font("Microsoft YaHei UI", 11F);
            keywordInput.Location = new Point(0, -3);
            keywordInput.Name = "keywordInput";
            keywordInput.Size = new Size(160, 43);
            keywordInput.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(SpinControl);
            panel2.Controls.Add(TableControl);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 48);
            panel2.Name = "panel2";
            panel2.Size = new Size(794, 361);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // SpinControl
            // 
            SpinControl.Anchor = AnchorStyles.None;
            SpinControl.BackColor = Color.White;
            SpinControl.Location = new Point(365, 146);
            SpinControl.Name = "SpinControl";
            SpinControl.Size = new Size(75, 57);
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
            TableControl.Size = new Size(794, 361);
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
    50,
    100,
    200,
    500,
    1000,
    500000
    };
            PaginationControl.ShowSizeChanger = true;
            PaginationControl.Size = new Size(794, 32);
            PaginationControl.TabIndex = 0;
            PaginationControl.Text = "pagination1";
            PaginationControl.ValueChanged += PaginationControl_ValueChanged;
            // 
            // WmsMaterialStockReportForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "WmsMaterialStockReportForm";
            Text = "WmsMaterialStockReportForm";
            Load += WmsMaterialStockReportForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Table TableControl;
        private AntdUI.Panel panel3;
        private AntdUI.Pagination PaginationControl;
        private AntdUI.Input keywordInput;
        private AntdUI.Button ExportButton;
        private AntdUI.Button SearchButton;
        private AntdUI.Input MaterialInput;
        private AntdUI.Input BatchInput;
        private AntdUI.Spin SpinControl;
    }
}