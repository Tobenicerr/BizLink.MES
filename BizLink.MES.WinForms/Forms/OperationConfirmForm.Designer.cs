namespace BizLink.MES.WinForms.Forms
{
    partial class OperationConfirmForm
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
            operationSelect = new AntdUI.SelectMultiple();
            exportButton = new AntdUI.Button();
            queryButton = new AntdUI.Button();
            statusSelect = new AntdUI.Select();
            ordersInput = new AntdUI.Input();
            panel3 = new AntdUI.Panel();
            confirmTable = new AntdUI.Table();
            pagination = new AntdUI.Pagination();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel3, 0, 1);
            tableLayoutPanel1.Controls.Add(pagination, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(916, 580);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(operationSelect);
            panel1.Controls.Add(exportButton);
            panel1.Controls.Add(queryButton);
            panel1.Controls.Add(statusSelect);
            panel1.Controls.Add(ordersInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(910, 44);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // operationSelect
            // 
            operationSelect.Font = new Font("Microsoft YaHei UI", 11F);
            operationSelect.Location = new Point(332, 2);
            operationSelect.Name = "operationSelect";
            operationSelect.Size = new Size(160, 43);
            operationSelect.TabIndex = 5;
            // 
            // exportButton
            // 
            exportButton.Font = new Font("Microsoft YaHei UI", 11F);
            exportButton.Location = new Point(569, 2);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(75, 42);
            exportButton.TabIndex = 4;
            exportButton.Text = "导出";
            exportButton.Type = AntdUI.TTypeMini.Success;
            exportButton.Click += exportButton_Click;
            // 
            // queryButton
            // 
            queryButton.Font = new Font("Microsoft YaHei UI", 11F);
            queryButton.Location = new Point(498, 2);
            queryButton.Name = "queryButton";
            queryButton.Size = new Size(75, 42);
            queryButton.TabIndex = 2;
            queryButton.Text = "查询";
            queryButton.Type = AntdUI.TTypeMini.Primary;
            queryButton.Click += queryButton_Click;
            // 
            // statusSelect
            // 
            statusSelect.Font = new Font("Microsoft YaHei UI", 11F);
            statusSelect.Location = new Point(166, 2);
            statusSelect.Name = "statusSelect";
            statusSelect.Size = new Size(160, 43);
            statusSelect.TabIndex = 1;
            // 
            // ordersInput
            // 
            ordersInput.Font = new Font("Microsoft YaHei UI", 11F);
            ordersInput.Location = new Point(0, 2);
            ordersInput.Multiline = true;
            ordersInput.Name = "ordersInput";
            ordersInput.Size = new Size(160, 43);
            ordersInput.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(confirmTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 53);
            panel3.Name = "panel3";
            panel3.Size = new Size(910, 486);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // confirmTable
            // 
            confirmTable.BackColor = Color.White;
            confirmTable.Dock = DockStyle.Fill;
            confirmTable.EditMode = AntdUI.TEditMode.DoubleClick;
            confirmTable.Font = new Font("Microsoft YaHei UI", 10F);
            confirmTable.Gap = 12;
            confirmTable.Location = new Point(0, 0);
            confirmTable.Name = "confirmTable";
            confirmTable.Size = new Size(910, 486);
            confirmTable.TabIndex = 0;
            confirmTable.Text = "table1";
            confirmTable.TreeArrowStyle = AntdUI.TableTreeStyle.Arrow;
            confirmTable.CellButtonClick += confirmTable_CellButtonClick;
            confirmTable.ExpandChanged += confirmTable_ExpandChanged;
            // 
            // pagination
            // 
            pagination.BackColor = Color.White;
            pagination.Current = 0;
            pagination.Dock = DockStyle.Fill;
            pagination.Font = new Font("Microsoft YaHei UI", 9F);
            pagination.Location = new Point(3, 545);
            pagination.Name = "pagination";
            pagination.PageSize = 100;
            pagination.PageSizeOptions = new int[]
    {
    50,
    100,
    200,
    500
    };
            pagination.ShowSizeChanger = true;
            pagination.Size = new Size(910, 32);
            pagination.TabIndex = 3;
            pagination.Total = 1;
            pagination.ValueChanged += pagination_ValueChanged;
            // 
            // OperationConfirmForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(916, 580);
            Controls.Add(tableLayoutPanel1);
            Name = "OperationConfirmForm";
            Text = "OperationConfirmForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Button queryButton;
        private AntdUI.Select statusSelect;
        private AntdUI.Input ordersInput;
        private AntdUI.Panel panel3;
        private AntdUI.Table confirmTable;
        private AntdUI.Button exportButton;
        private AntdUI.SelectMultiple operationSelect;
        private AntdUI.Pagination pagination;
    }
}