namespace BizLink.MES.WinForms.Forms
{
    partial class LinesideInventoryManagementForm
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
            sapSyncButton = new AntdUI.Button();
            exportButton = new AntdUI.Button();
            stockCreateButton = new AntdUI.Button();
            label1 = new AntdUI.Label();
            quantitySwitch = new AntdUI.Switch();
            queryButton = new AntdUI.Button();
            locationSelectMultiple = new AntdUI.SelectMultiple();
            keyboardInput = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
            stockTable = new AntdUI.Table();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(972, 573);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(sapSyncButton);
            panel1.Controls.Add(exportButton);
            panel1.Controls.Add(stockCreateButton);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(quantitySwitch);
            panel1.Controls.Add(queryButton);
            panel1.Controls.Add(locationSelectMultiple);
            panel1.Controls.Add(keyboardInput);
            panel1.Dock = DockStyle.Fill;
            panel1.Font = new Font("Microsoft YaHei UI", 11F);
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(966, 44);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // sapSyncButton
            // 
            sapSyncButton.Anchor = AnchorStyles.Right;
            sapSyncButton.Font = new Font("Microsoft YaHei UI", 11F);
            sapSyncButton.Location = new Point(692, 3);
            sapSyncButton.Name = "sapSyncButton";
            sapSyncButton.Size = new Size(87, 40);
            sapSyncButton.TabIndex = 7;
            sapSyncButton.Text = "SAP同步";
            sapSyncButton.Type = AntdUI.TTypeMini.Error;
            sapSyncButton.Visible = false;
            sapSyncButton.Click += sapSyncButton_Click;
            // 
            // exportButton
            // 
            exportButton.Anchor = AnchorStyles.Right;
            exportButton.Font = new Font("Microsoft YaHei UI", 11F);
            exportButton.Location = new Point(782, 3);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(87, 40);
            exportButton.TabIndex = 6;
            exportButton.Text = "库存导出";
            exportButton.Type = AntdUI.TTypeMini.Success;
            exportButton.Click += exportButton_Click;
            // 
            // stockCreateButton
            // 
            stockCreateButton.Anchor = AnchorStyles.Right;
            stockCreateButton.Font = new Font("Microsoft YaHei UI", 11F);
            stockCreateButton.Location = new Point(870, 3);
            stockCreateButton.Name = "stockCreateButton";
            stockCreateButton.Size = new Size(87, 40);
            stockCreateButton.TabIndex = 5;
            stockCreateButton.Text = "标签调整";
            stockCreateButton.Type = AntdUI.TTypeMini.Primary;
            stockCreateButton.Click += stockCreateButton_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.Location = new Point(375, 9);
            label1.Name = "label1";
            label1.Size = new Size(53, 24);
            label1.TabIndex = 4;
            label1.Text = "库存>0";
            // 
            // quantitySwitch
            // 
            quantitySwitch.Anchor = AnchorStyles.Left;
            quantitySwitch.Checked = true;
            quantitySwitch.CheckedText = "是";
            quantitySwitch.Location = new Point(434, 4);
            quantitySwitch.Name = "quantitySwitch";
            quantitySwitch.Size = new Size(63, 35);
            quantitySwitch.TabIndex = 3;
            quantitySwitch.Text = "switch1";
            quantitySwitch.UnCheckedText = "否";
            // 
            // queryButton
            // 
            queryButton.Font = new Font("Microsoft YaHei UI", 11F);
            queryButton.Location = new Point(503, 3);
            queryButton.Name = "queryButton";
            queryButton.Size = new Size(60, 40);
            queryButton.TabIndex = 2;
            queryButton.Text = "查询";
            queryButton.Type = AntdUI.TTypeMini.Primary;
            queryButton.Click += queryButton_Click;
            // 
            // locationSelectMultiple
            // 
            locationSelectMultiple.Font = new Font("Microsoft YaHei UI", 11F);
            locationSelectMultiple.Location = new Point(189, 3);
            locationSelectMultiple.Name = "locationSelectMultiple";
            locationSelectMultiple.Size = new Size(180, 40);
            locationSelectMultiple.TabIndex = 1;
            // 
            // keyboardInput
            // 
            keyboardInput.Font = new Font("Microsoft YaHei UI", 11F);
            keyboardInput.Location = new Point(3, 3);
            keyboardInput.Name = "keyboardInput";
            keyboardInput.Size = new Size(180, 40);
            keyboardInput.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.Controls.Add(stockTable);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(966, 517);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // stockTable
            // 
            stockTable.BackColor = Color.White;
            stockTable.Dock = DockStyle.Fill;
            stockTable.EditMode = AntdUI.TEditMode.DoubleClick;
            stockTable.Font = new Font("Microsoft YaHei UI", 11F);
            stockTable.Gap = 12;
            stockTable.Location = new Point(0, 0);
            stockTable.Name = "stockTable";
            stockTable.Size = new Size(966, 517);
            stockTable.TabIndex = 0;
            stockTable.Text = "table1";
            stockTable.TreeArrowStyle = AntdUI.TableTreeStyle.Arrow;
            stockTable.CellButtonClick += stockTable_CellButtonClick;
            stockTable.SetRowStyle += stockTable_SetRowStyle;
            stockTable.ExpandChanged += stockTable_ExpandChanged;
            // 
            // LinesideInventoryManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(972, 573);
            Controls.Add(tableLayoutPanel1);
            Name = "LinesideInventoryManagementForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "LinesideInventoryManagement";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Input keyboardInput;
        private AntdUI.SelectMultiple locationSelectMultiple;
        private AntdUI.Button queryButton;
        private AntdUI.Panel panel2;
        private AntdUI.Table stockTable;
        private AntdUI.Switch quantitySwitch;
        private AntdUI.Label label1;
        private AntdUI.Button stockCreateButton;
        private AntdUI.Button exportButton;
        private AntdUI.Button sapSyncButton;
    }
}