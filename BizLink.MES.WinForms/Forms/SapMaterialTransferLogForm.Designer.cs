namespace BizLink.MES.WinForms.Forms
{
    partial class SapMaterialTransferLogForm
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
            createOndatePickerRange = new AntdUI.DatePickerRange();
            exportButton = new AntdUI.Button();
            queryButton = new AntdUI.Button();
            statusSelect = new AntdUI.Select();
            keywordInput = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
            batReentrybutton = new AntdUI.Button();
            panel3 = new AntdUI.Panel();
            progress = new AntdUI.Progress();
            sapTransferTable = new AntdUI.Table();
            paginationControl = new AntdUI.Pagination();
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
            tableLayoutPanel1.Controls.Add(paginationControl, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(createOndatePickerRange);
            panel1.Controls.Add(exportButton);
            panel1.Controls.Add(queryButton);
            panel1.Controls.Add(statusSelect);
            panel1.Controls.Add(keywordInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(794, 44);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // createOndatePickerRange
            // 
            createOndatePickerRange.Font = new Font("Microsoft YaHei UI", 11F);
            createOndatePickerRange.Location = new Point(403, 2);
            createOndatePickerRange.Name = "createOndatePickerRange";
            createOndatePickerRange.PlaceholderStart = "";
            createOndatePickerRange.Size = new Size(240, 42);
            createOndatePickerRange.TabIndex = 3;
            createOndatePickerRange.TextAlign = HorizontalAlignment.Center;
            // 
            // exportButton
            // 
            exportButton.Font = new Font("Microsoft YaHei UI", 11F);
            exportButton.Location = new Point(718, 2);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(75, 42);
            exportButton.TabIndex = 2;
            exportButton.Text = "导出";
            exportButton.Type = AntdUI.TTypeMini.Success;
            exportButton.Click += exportButton_Click;
            // 
            // queryButton
            // 
            queryButton.Anchor = AnchorStyles.Left;
            queryButton.Font = new Font("Microsoft YaHei UI", 11F);
            queryButton.Location = new Point(646, 2);
            queryButton.Name = "queryButton";
            queryButton.Size = new Size(75, 42);
            queryButton.TabIndex = 0;
            queryButton.Text = "查询";
            queryButton.Type = AntdUI.TTypeMini.Primary;
            queryButton.Click += queryButton_Click;
            // 
            // statusSelect
            // 
            statusSelect.Font = new Font("Microsoft YaHei UI", 11F);
            statusSelect.Location = new Point(206, 2);
            statusSelect.Name = "statusSelect";
            statusSelect.Size = new Size(200, 42);
            statusSelect.TabIndex = 1;
            // 
            // keywordInput
            // 
            keywordInput.Font = new Font("Microsoft YaHei UI", 11F);
            keywordInput.Location = new Point(9, 2);
            keywordInput.Name = "keywordInput";
            keywordInput.Size = new Size(200, 42);
            keywordInput.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(batReentrybutton);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(794, 39);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // batReentrybutton
            // 
            batReentrybutton.Anchor = AnchorStyles.Right;
            batReentrybutton.Font = new Font("Microsoft YaHei UI", 11F);
            batReentrybutton.Location = new Point(711, 0);
            batReentrybutton.Name = "batReentrybutton";
            batReentrybutton.Size = new Size(80, 42);
            batReentrybutton.TabIndex = 7;
            batReentrybutton.Text = "批量重推";
            batReentrybutton.Type = AntdUI.TTypeMini.Primary;
            batReentrybutton.Click += batReentrybutton_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(progress);
            panel3.Controls.Add(sapTransferTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 98);
            panel3.Name = "panel3";
            panel3.Size = new Size(794, 307);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // progress
            // 
            progress.Anchor = AnchorStyles.None;
            progress.BackColor = Color.White;
            progress.Location = new Point(366, 129);
            progress.Name = "progress";
            progress.Radius = 4;
            progress.Shape = AntdUI.TShapeProgress.Circle;
            progress.Size = new Size(58, 50);
            progress.TabIndex = 8;
            progress.Text = "";
            progress.Visible = false;
            // 
            // sapTransferTable
            // 
            sapTransferTable.BackColor = Color.White;
            sapTransferTable.Dock = DockStyle.Fill;
            sapTransferTable.EditMode = AntdUI.TEditMode.DoubleClick;
            sapTransferTable.Font = new Font("Microsoft YaHei UI", 10F);
            sapTransferTable.Gap = 12;
            sapTransferTable.Location = new Point(0, 0);
            sapTransferTable.Name = "sapTransferTable";
            sapTransferTable.Size = new Size(794, 307);
            sapTransferTable.TabIndex = 0;
            sapTransferTable.Text = "table1";
            sapTransferTable.CellButtonClick += sapTransferTable_CellButtonClick;
            // 
            // paginationControl
            // 
            paginationControl.BackColor = Color.White;
            paginationControl.Current = 0;
            paginationControl.Dock = DockStyle.Fill;
            paginationControl.Location = new Point(3, 411);
            paginationControl.Name = "paginationControl";
            paginationControl.PageSize = 100;
            paginationControl.PageSizeOptions = new int[]
    {
    20,
    50,
    100,
    200
    };
            paginationControl.ShowSizeChanger = true;
            paginationControl.Size = new Size(794, 36);
            paginationControl.TabIndex = 3;
            paginationControl.Text = "pagination1";
            paginationControl.Total = 1;
            paginationControl.ValueChanged += paginationControl_ValueChanged;
            // 
            // SapMaterialTransferLogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "SapMaterialTransferLogForm";
            Text = "SapMaterialTransferLogForm";
            Load += SapMaterialTransferLogForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Select statusSelect;
        private AntdUI.Input keywordInput;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Button queryButton;
        private AntdUI.Table sapTransferTable;
        private AntdUI.Button batReentrybutton;
        private AntdUI.Button exportButton;
        private AntdUI.Progress progress;
        private AntdUI.Pagination paginationControl;
        private AntdUI.DatePickerRange createOndatePickerRange;
    }
}