namespace BizLink.MES.WinForms.Forms
{
    partial class GoodsReceiptManagementForm
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
            AntdUI.StepsItem stepsItem7 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem8 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem9 = new AntdUI.StepsItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new AntdUI.Panel();
            checkbox1 = new AntdUI.Checkbox();
            DispatchDatePickerRange = new AntdUI.DatePickerRange();
            SearchButton = new AntdUI.Button();
            PrinterSelect = new AntdUI.Select();
            WorkCenterSelect = new AntdUI.Select();
            panel2 = new AntdUI.Panel();
            WorkOrderInput = new AntdUI.Input();
            panel3 = new AntdUI.Panel();
            OrderTable = new AntdUI.Table();
            panel5 = new AntdUI.Panel();
            ReceiptTable = new AntdUI.Table();
            panel4 = new AntdUI.Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel7 = new AntdUI.Panel();
            ReceiptedNumLabel = new AntdUI.Label();
            label5 = new AntdUI.Label();
            MaterialLabel = new AntdUI.Label();
            WorkOrderLabel = new AntdUI.Label();
            panel8 = new AntdUI.Panel();
            ReceiptInputNumber = new AntdUI.InputNumber();
            label1 = new AntdUI.Label();
            panel9 = new AntdUI.Panel();
            SapBatchInput = new AntdUI.Input();
            label2 = new AntdUI.Label();
            panel10 = new AntdUI.Panel();
            SubmitButton = new AntdUI.Button();
            panel11 = new AntdUI.Panel();
            ReprintButton = new AntdUI.Button();
            panel12 = new AntdUI.Panel();
            ResetButton = new AntdUI.Button();
            panel6 = new AntdUI.Panel();
            ReceiptSteps = new AntdUI.Steps();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel5.SuspendLayout();
            panel4.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel7.SuspendLayout();
            panel8.SuspendLayout();
            panel9.SuspendLayout();
            panel10.SuspendLayout();
            panel11.SuspendLayout();
            panel12.SuspendLayout();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel3, 0, 2);
            tableLayoutPanel1.Controls.Add(panel5, 1, 3);
            tableLayoutPanel1.Controls.Add(panel4, 1, 2);
            tableLayoutPanel1.Controls.Add(panel6, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 350F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1066, 597);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(checkbox1);
            panel1.Controls.Add(DispatchDatePickerRange);
            panel1.Controls.Add(SearchButton);
            panel1.Controls.Add(PrinterSelect);
            panel1.Controls.Add(WorkCenterSelect);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(1060, 44);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // checkbox1
            // 
            checkbox1.BackColor = Color.White;
            checkbox1.Font = new Font("Microsoft YaHei UI", 11F);
            checkbox1.Location = new Point(716, 10);
            checkbox1.Name = "checkbox1";
            checkbox1.Size = new Size(118, 23);
            checkbox1.TabIndex = 5;
            checkbox1.Text = "打印内袋标签";
            checkbox1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // DispatchDatePickerRange
            // 
            DispatchDatePickerRange.AllowClear = true;
            DispatchDatePickerRange.Font = new Font("Microsoft YaHei UI", 11F);
            DispatchDatePickerRange.Location = new Point(0, 0);
            DispatchDatePickerRange.Name = "DispatchDatePickerRange";
            DispatchDatePickerRange.Size = new Size(240, 45);
            DispatchDatePickerRange.TabIndex = 4;
            DispatchDatePickerRange.TextAlign = HorizontalAlignment.Center;
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 11F);
            SearchButton.Location = new Point(840, 0);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(100, 45);
            SearchButton.TabIndex = 3;
            SearchButton.Text = "确认";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += SearchButton_Click;
            // 
            // PrinterSelect
            // 
            PrinterSelect.AllowClear = true;
            PrinterSelect.Font = new Font("Microsoft YaHei UI", 11F);
            PrinterSelect.Location = new Point(476, 0);
            PrinterSelect.Name = "PrinterSelect";
            PrinterSelect.Size = new Size(240, 45);
            PrinterSelect.TabIndex = 2;
            PrinterSelect.SelectedValueChanged += PrinterSelect_SelectedValueChanged;
            // 
            // WorkCenterSelect
            // 
            WorkCenterSelect.AllowClear = true;
            WorkCenterSelect.Font = new Font("Microsoft YaHei UI", 11F);
            WorkCenterSelect.Location = new Point(238, 0);
            WorkCenterSelect.Name = "WorkCenterSelect";
            WorkCenterSelect.Size = new Size(240, 45);
            WorkCenterSelect.TabIndex = 1;
            WorkCenterSelect.SelectedValueChanged += WorkCenterSelect_SelectedValueChanged;
            // 
            // panel2
            // 
            panel2.Controls.Add(WorkOrderInput);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(580, 49);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // WorkOrderInput
            // 
            WorkOrderInput.Dock = DockStyle.Fill;
            WorkOrderInput.Font = new Font("Microsoft YaHei UI", 11F);
            WorkOrderInput.Location = new Point(0, 0);
            WorkOrderInput.Name = "WorkOrderInput";
            WorkOrderInput.Size = new Size(580, 49);
            WorkOrderInput.TabIndex = 0;
            WorkOrderInput.KeyPress += WorkOrderInput_KeyPress;
            // 
            // panel3
            // 
            panel3.Controls.Add(OrderTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 108);
            panel3.Name = "panel3";
            tableLayoutPanel1.SetRowSpan(panel3, 2);
            panel3.Size = new Size(580, 486);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // OrderTable
            // 
            OrderTable.BackColor = Color.White;
            OrderTable.Dock = DockStyle.Fill;
            OrderTable.Font = new Font("Microsoft YaHei UI", 10F);
            OrderTable.Gap = 12;
            OrderTable.Location = new Point(0, 0);
            OrderTable.Name = "OrderTable";
            OrderTable.Size = new Size(580, 486);
            OrderTable.TabIndex = 0;
            OrderTable.Text = "table1";
            // 
            // panel5
            // 
            panel5.Controls.Add(ReceiptTable);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(589, 458);
            panel5.Name = "panel5";
            panel5.Size = new Size(474, 136);
            panel5.TabIndex = 4;
            panel5.Text = "panel5";
            // 
            // ReceiptTable
            // 
            ReceiptTable.BackColor = Color.White;
            ReceiptTable.Dock = DockStyle.Fill;
            ReceiptTable.Font = new Font("Microsoft YaHei UI", 10F);
            ReceiptTable.Gap = 12;
            ReceiptTable.Location = new Point(0, 0);
            ReceiptTable.Name = "ReceiptTable";
            ReceiptTable.Size = new Size(474, 136);
            ReceiptTable.TabIndex = 0;
            ReceiptTable.Text = "table2";
            // 
            // panel4
            // 
            panel4.Controls.Add(tableLayoutPanel2);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(589, 108);
            panel4.Name = "panel4";
            panel4.Size = new Size(474, 344);
            panel4.TabIndex = 3;
            panel4.Text = "panel4";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = SystemColors.Control;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(panel7, 0, 0);
            tableLayoutPanel2.Controls.Add(panel8, 0, 1);
            tableLayoutPanel2.Controls.Add(panel9, 0, 2);
            tableLayoutPanel2.Controls.Add(panel10, 0, 3);
            tableLayoutPanel2.Controls.Add(panel11, 0, 4);
            tableLayoutPanel2.Controls.Add(panel12, 1, 4);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 5;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
            tableLayoutPanel2.Size = new Size(474, 344);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // panel7
            // 
            panel7.BackColor = SystemColors.Control;
            tableLayoutPanel2.SetColumnSpan(panel7, 2);
            panel7.Controls.Add(ReceiptedNumLabel);
            panel7.Controls.Add(label5);
            panel7.Controls.Add(MaterialLabel);
            panel7.Controls.Add(WorkOrderLabel);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(3, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(468, 68);
            panel7.TabIndex = 0;
            panel7.Text = "panel7";
            // 
            // ReceiptedNumLabel
            // 
            ReceiptedNumLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            ReceiptedNumLabel.BackColor = Color.White;
            ReceiptedNumLabel.Font = new Font("Microsoft YaHei UI", 17F, FontStyle.Bold);
            ReceiptedNumLabel.ForeColor = Color.Red;
            ReceiptedNumLabel.Location = new Point(387, 36);
            ReceiptedNumLabel.Name = "ReceiptedNumLabel";
            ReceiptedNumLabel.Size = new Size(75, 32);
            ReceiptedNumLabel.TabIndex = 3;
            ReceiptedNumLabel.Text = "0 PCS";
            ReceiptedNumLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.BackColor = Color.White;
            label5.Font = new Font("Microsoft YaHei UI", 9F);
            label5.ForeColor = Color.Silver;
            label5.Location = new Point(387, 0);
            label5.Name = "label5";
            label5.Size = new Size(75, 23);
            label5.TabIndex = 2;
            label5.Text = "待入库数";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // MaterialLabel
            // 
            MaterialLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MaterialLabel.BackColor = Color.White;
            MaterialLabel.Font = new Font("Microsoft YaHei UI", 10F);
            MaterialLabel.ForeColor = Color.LightSlateGray;
            MaterialLabel.Location = new Point(6, 42);
            MaterialLabel.Name = "MaterialLabel";
            MaterialLabel.Size = new Size(286, 23);
            MaterialLabel.TabIndex = 1;
            MaterialLabel.Text = "";
            // 
            // WorkOrderLabel
            // 
            WorkOrderLabel.BackColor = Color.White;
            WorkOrderLabel.Font = new Font("Microsoft YaHei UI", 14F, FontStyle.Bold);
            WorkOrderLabel.ForeColor = Color.Blue;
            WorkOrderLabel.Location = new Point(6, 2);
            WorkOrderLabel.Name = "WorkOrderLabel";
            WorkOrderLabel.Size = new Size(215, 23);
            WorkOrderLabel.TabIndex = 0;
            WorkOrderLabel.Text = "";
            // 
            // panel8
            // 
            tableLayoutPanel2.SetColumnSpan(panel8, 2);
            panel8.Controls.Add(ReceiptInputNumber);
            panel8.Controls.Add(label1);
            panel8.Dock = DockStyle.Fill;
            panel8.Location = new Point(3, 77);
            panel8.Name = "panel8";
            panel8.Size = new Size(468, 74);
            panel8.TabIndex = 1;
            panel8.Text = "panel8";
            // 
            // ReceiptInputNumber
            // 
            ReceiptInputNumber.Dock = DockStyle.Bottom;
            ReceiptInputNumber.Font = new Font("Microsoft YaHei UI", 11F);
            ReceiptInputNumber.Location = new Point(0, 29);
            ReceiptInputNumber.Name = "ReceiptInputNumber";
            ReceiptInputNumber.Size = new Size(468, 45);
            ReceiptInputNumber.Status = AntdUI.TType.Info;
            ReceiptInputNumber.SuffixText = "PCS";
            ReceiptInputNumber.TabIndex = 1;
            ReceiptInputNumber.Text = "0";
            ReceiptInputNumber.ValueChanged += ReceiptInputNumber_ValueChanged;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label1.BackColor = Color.White;
            label1.Location = new Point(3, 3);
            label1.Name = "label1";
            label1.Size = new Size(75, 23);
            label1.TabIndex = 0;
            label1.Text = "入库数量：";
            // 
            // panel9
            // 
            tableLayoutPanel2.SetColumnSpan(panel9, 2);
            panel9.Controls.Add(SapBatchInput);
            panel9.Controls.Add(label2);
            panel9.Dock = DockStyle.Fill;
            panel9.Location = new Point(3, 157);
            panel9.Name = "panel9";
            panel9.Size = new Size(468, 74);
            panel9.TabIndex = 2;
            panel9.Text = "panel9";
            // 
            // SapBatchInput
            // 
            SapBatchInput.Dock = DockStyle.Bottom;
            SapBatchInput.Font = new Font("Microsoft YaHei UI", 11F);
            SapBatchInput.Location = new Point(0, 29);
            SapBatchInput.Name = "SapBatchInput";
            SapBatchInput.ReadOnly = true;
            SapBatchInput.Size = new Size(468, 45);
            SapBatchInput.TabIndex = 1;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label2.BackColor = Color.White;
            label2.Location = new Point(3, 3);
            label2.Name = "label2";
            label2.Size = new Size(75, 23);
            label2.TabIndex = 0;
            label2.Text = "入库批次：";
            // 
            // panel10
            // 
            tableLayoutPanel2.SetColumnSpan(panel10, 2);
            panel10.Controls.Add(SubmitButton);
            panel10.Dock = DockStyle.Fill;
            panel10.Location = new Point(3, 237);
            panel10.Name = "panel10";
            panel10.Size = new Size(468, 49);
            panel10.TabIndex = 3;
            panel10.Text = "panel10";
            // 
            // SubmitButton
            // 
            SubmitButton.Dock = DockStyle.Fill;
            SubmitButton.Font = new Font("Microsoft YaHei UI", 11F);
            SubmitButton.Location = new Point(0, 0);
            SubmitButton.Name = "SubmitButton";
            SubmitButton.Size = new Size(468, 49);
            SubmitButton.TabIndex = 0;
            SubmitButton.Text = "入库并打标签";
            SubmitButton.Type = AntdUI.TTypeMini.Primary;
            SubmitButton.Click += SubmitButton_Click;
            // 
            // panel11
            // 
            panel11.Controls.Add(ReprintButton);
            panel11.Dock = DockStyle.Fill;
            panel11.Location = new Point(3, 292);
            panel11.Name = "panel11";
            panel11.Size = new Size(231, 49);
            panel11.TabIndex = 4;
            panel11.Text = "panel11";
            // 
            // ReprintButton
            // 
            ReprintButton.Dock = DockStyle.Fill;
            ReprintButton.Font = new Font("Microsoft YaHei UI", 11F);
            ReprintButton.Location = new Point(0, 0);
            ReprintButton.Name = "ReprintButton";
            ReprintButton.Size = new Size(231, 49);
            ReprintButton.TabIndex = 0;
            ReprintButton.Text = "补打标签";
            ReprintButton.Type = AntdUI.TTypeMini.Success;
            ReprintButton.Click += ReprintButton_Click;
            // 
            // panel12
            // 
            panel12.Controls.Add(ResetButton);
            panel12.Dock = DockStyle.Fill;
            panel12.Location = new Point(240, 292);
            panel12.Name = "panel12";
            panel12.Size = new Size(231, 49);
            panel12.TabIndex = 5;
            panel12.Text = "panel12";
            // 
            // ResetButton
            // 
            ResetButton.BorderWidth = 1F;
            ResetButton.Dock = DockStyle.Fill;
            ResetButton.Font = new Font("Microsoft YaHei UI", 11F);
            ResetButton.Location = new Point(0, 0);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(231, 49);
            ResetButton.TabIndex = 0;
            ResetButton.Text = "重置";
            ResetButton.Click += ResetButton_Click;
            // 
            // panel6
            // 
            panel6.Controls.Add(ReceiptSteps);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(589, 53);
            panel6.Name = "panel6";
            panel6.Size = new Size(474, 49);
            panel6.TabIndex = 5;
            panel6.Text = "panel6";
            // 
            // ReceiptSteps
            // 
            ReceiptSteps.BackColor = Color.White;
            ReceiptSteps.Dock = DockStyle.Fill;
            ReceiptSteps.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            ReceiptSteps.ForeColor = SystemColors.Highlight;
            stepsItem7.Title = "订单扫描";
            stepsItem8.Title = "Sap入库";
            stepsItem9.Title = "标签打印";
            ReceiptSteps.Items.Add(stepsItem7);
            ReceiptSteps.Items.Add(stepsItem8);
            ReceiptSteps.Items.Add(stepsItem9);
            ReceiptSteps.Location = new Point(0, 0);
            ReceiptSteps.Name = "ReceiptSteps";
            ReceiptSteps.Size = new Size(474, 49);
            ReceiptSteps.TabIndex = 0;
            ReceiptSteps.Text = "steps1";
            // 
            // GoodsReceiptManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1066, 597);
            Controls.Add(tableLayoutPanel1);
            Name = "GoodsReceiptManagementForm";
            Text = "GoodsReceiptManagementForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel4.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel12.ResumeLayout(false);
            panel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Panel panel4;
        private AntdUI.Panel panel5;
        private AntdUI.Select PrinterSelect;
        private AntdUI.Select WorkCenterSelect;
        private AntdUI.Button SearchButton;
        private AntdUI.Input WorkOrderInput;
        private AntdUI.Panel panel6;
        private AntdUI.Steps ReceiptSteps;
        private TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Panel panel7;
        private AntdUI.Panel panel8;
        private AntdUI.Panel panel9;
        private AntdUI.Panel panel10;
        private AntdUI.Button SubmitButton;
        private AntdUI.Panel panel11;
        private AntdUI.Button ReprintButton;
        private AntdUI.Panel panel12;
        private AntdUI.Button ResetButton;
        private AntdUI.Table OrderTable;
        private AntdUI.Table ReceiptTable;
        private AntdUI.DatePickerRange DispatchDatePickerRange;
        private AntdUI.Label label1;
        private AntdUI.Label label2;
        private AntdUI.InputNumber ReceiptInputNumber;
        private AntdUI.Input SapBatchInput;
        private AntdUI.Label MaterialLabel;
        private AntdUI.Label WorkOrderLabel;
        private AntdUI.Label ReceiptedNumLabel;
        private AntdUI.Label label5;
        private AntdUI.Checkbox checkbox1;
    }
}