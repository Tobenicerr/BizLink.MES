namespace BizLink.MES.WinForms.Forms
{
    partial class WorkOrderReportInspectionForm
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
            tableLayoutPanel5 = new TableLayoutPanel();
            panel14 = new AntdUI.Panel();
            OrderScanInput = new AntdUI.Input();
            panel8 = new AntdUI.Panel();
            OrderTable = new AntdUI.Table();
            panel2 = new AntdUI.Panel();
            SearchButton = new AntdUI.Button();
            WorkcenterSelect = new AntdUI.Select();
            WorkcenterGroupSelect = new AntdUI.Select();
            DispathchDatePickerRange = new AntdUI.DatePickerRange();
            panel3 = new AntdUI.Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel6 = new AntdUI.Panel();
            Matlabel = new AntdUI.Label();
            Orderlabel = new AntdUI.Label();
            Desclabel = new AntdUI.Label();
            panel7 = new AntdUI.Panel();
            progressControl = new AntdUI.Progress();
            LeadMatlabel = new AntdUI.Label();
            confLabel = new AntdUI.Label();
            panel4 = new AntdUI.Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            label4 = new AntdUI.Label();
            panel13 = new AntdUI.Panel();
            ConfTable = new AntdUI.Table();
            panel5 = new AntdUI.Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            label5 = new AntdUI.Label();
            panel10 = new AntdUI.Panel();
            label7 = new AntdUI.Label();
            inputNumber2 = new AntdUI.InputNumber();
            label6 = new AntdUI.Label();
            ConfInputNumber = new AntdUI.InputNumber();
            panel12 = new AntdUI.Panel();
            ResetButton = new AntdUI.Button();
            SubmitButton = new AntdUI.Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            panel14.SuspendLayout();
            panel8.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            panel4.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel13.SuspendLayout();
            panel5.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel10.SuspendLayout();
            panel12.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 7;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 24.9981251F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.0006256F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.0006256F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25.0006256F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(panel3, 2, 1);
            tableLayoutPanel1.Controls.Add(panel4, 2, 2);
            tableLayoutPanel1.Controls.Add(panel5, 5, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(951, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            tableLayoutPanel1.SetColumnSpan(panel1, 2);
            panel1.Controls.Add(tableLayoutPanel5);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 48);
            panel1.Name = "panel1";
            tableLayoutPanel1.SetRowSpan(panel1, 4);
            panel1.Size = new Size(396, 399);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = SystemColors.Control;
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Controls.Add(panel14, 0, 0);
            tableLayoutPanel5.Controls.Add(panel8, 0, 1);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Size = new Size(396, 399);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // panel14
            // 
            panel14.Controls.Add(OrderScanInput);
            panel14.Dock = DockStyle.Fill;
            panel14.Location = new Point(3, 3);
            panel14.Name = "panel14";
            panel14.Size = new Size(390, 44);
            panel14.TabIndex = 0;
            panel14.Text = "panel14";
            // 
            // OrderScanInput
            // 
            OrderScanInput.Dock = DockStyle.Fill;
            OrderScanInput.Font = new Font("Microsoft YaHei UI", 11F);
            OrderScanInput.Location = new Point(0, 0);
            OrderScanInput.Name = "OrderScanInput";
            OrderScanInput.Size = new Size(390, 44);
            OrderScanInput.TabIndex = 0;
            OrderScanInput.KeyPress += OrderScanInput_KeyPress;
            // 
            // panel8
            // 
            panel8.Controls.Add(OrderTable);
            panel8.Dock = DockStyle.Fill;
            panel8.Location = new Point(3, 53);
            panel8.Name = "panel8";
            panel8.Size = new Size(390, 343);
            panel8.TabIndex = 1;
            panel8.Text = "panel8";
            // 
            // OrderTable
            // 
            OrderTable.BackColor = Color.White;
            OrderTable.Dock = DockStyle.Fill;
            OrderTable.EditMode = AntdUI.TEditMode.DoubleClick;
            OrderTable.Font = new Font("Microsoft YaHei UI", 10F);
            OrderTable.Gap = 12;
            OrderTable.Location = new Point(0, 0);
            OrderTable.Name = "OrderTable";
            OrderTable.Size = new Size(390, 343);
            OrderTable.TabIndex = 0;
            OrderTable.Text = "table2";
            // 
            // panel2
            // 
            tableLayoutPanel1.SetColumnSpan(panel2, 7);
            panel2.Controls.Add(SearchButton);
            panel2.Controls.Add(WorkcenterSelect);
            panel2.Controls.Add(WorkcenterGroupSelect);
            panel2.Controls.Add(DispathchDatePickerRange);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(945, 39);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 10F);
            SearchButton.Location = new Point(811, 0);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(90, 42);
            SearchButton.TabIndex = 3;
            SearchButton.Text = "工位确认";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += SearchButton_Click;
            // 
            // WorkcenterSelect
            // 
            WorkcenterSelect.Font = new Font("Microsoft YaHei UI", 11F);
            WorkcenterSelect.Location = new Point(555, -1);
            WorkcenterSelect.Name = "WorkcenterSelect";
            WorkcenterSelect.Size = new Size(250, 42);
            WorkcenterSelect.TabIndex = 2;
            WorkcenterSelect.SelectedValueChanged += WorkcenterSelect_SelectedValueChanged;
            // 
            // WorkcenterGroupSelect
            // 
            WorkcenterGroupSelect.Font = new Font("Microsoft YaHei UI", 11F);
            WorkcenterGroupSelect.Location = new Point(304, -1);
            WorkcenterGroupSelect.Name = "WorkcenterGroupSelect";
            WorkcenterGroupSelect.Size = new Size(250, 42);
            WorkcenterGroupSelect.TabIndex = 1;
            WorkcenterGroupSelect.SelectedValueChanged += WorkcenterGroupSelect_SelectedValueChanged;
            // 
            // DispathchDatePickerRange
            // 
            DispathchDatePickerRange.Font = new Font("Microsoft YaHei UI", 11F);
            DispathchDatePickerRange.Location = new Point(3, -1);
            DispathchDatePickerRange.Name = "DispathchDatePickerRange";
            DispathchDatePickerRange.Size = new Size(300, 42);
            DispathchDatePickerRange.TabIndex = 0;
            DispathchDatePickerRange.TextAlign = HorizontalAlignment.Center;
            // 
            // panel3
            // 
            tableLayoutPanel1.SetColumnSpan(panel3, 5);
            panel3.Controls.Add(tableLayoutPanel2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(405, 48);
            panel3.Name = "panel3";
            panel3.Size = new Size(543, 94);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.White;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(panel6, 0, 0);
            tableLayoutPanel2.Controls.Add(panel7, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(543, 94);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.Controls.Add(Matlabel);
            panel6.Controls.Add(Orderlabel);
            panel6.Controls.Add(Desclabel);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(3, 3);
            panel6.Name = "panel6";
            panel6.Size = new Size(265, 88);
            panel6.TabIndex = 0;
            panel6.Text = "panel6";
            // 
            // Matlabel
            // 
            Matlabel.BackColor = Color.White;
            Matlabel.Font = new Font("Microsoft YaHei UI", 12F);
            Matlabel.Location = new Point(0, 33);
            Matlabel.Name = "Matlabel";
            Matlabel.Size = new Size(220, 22);
            Matlabel.TabIndex = 2;
            Matlabel.Text = "  物料号：";
            // 
            // Orderlabel
            // 
            Orderlabel.BackColor = Color.White;
            Orderlabel.Font = new Font("Microsoft YaHei UI", 12F);
            Orderlabel.Location = new Point(0, 5);
            Orderlabel.Name = "Orderlabel";
            Orderlabel.Size = new Size(220, 22);
            Orderlabel.TabIndex = 0;
            Orderlabel.Text = "  订单号：";
            // 
            // Desclabel
            // 
            Desclabel.BackColor = Color.White;
            Desclabel.Font = new Font("Microsoft YaHei UI", 12F);
            Desclabel.Location = new Point(0, 61);
            Desclabel.Name = "Desclabel";
            Desclabel.Size = new Size(220, 22);
            Desclabel.TabIndex = 0;
            Desclabel.Text = "  物料描述：";
            // 
            // panel7
            // 
            panel7.Controls.Add(progressControl);
            panel7.Controls.Add(LeadMatlabel);
            panel7.Controls.Add(confLabel);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(274, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(266, 88);
            panel7.TabIndex = 1;
            panel7.Text = "panel7";
            // 
            // progressControl
            // 
            progressControl.BackColor = Color.White;
            progressControl.Dock = DockStyle.Bottom;
            progressControl.Font = new Font("Microsoft YaHei UI", 12F);
            progressControl.Location = new Point(0, 72);
            progressControl.Name = "progressControl";
            progressControl.Radius = 1;
            progressControl.Size = new Size(266, 16);
            progressControl.State = AntdUI.TType.Success;
            progressControl.TabIndex = 0;
            progressControl.Text = "";
            // 
            // LeadMatlabel
            // 
            LeadMatlabel.BackColor = Color.White;
            LeadMatlabel.Font = new Font("Microsoft YaHei UI", 12F);
            LeadMatlabel.Location = new Point(0, 5);
            LeadMatlabel.Name = "LeadMatlabel";
            LeadMatlabel.Size = new Size(220, 22);
            LeadMatlabel.TabIndex = 1;
            LeadMatlabel.Text = "  成品料号：";
            // 
            // confLabel
            // 
            confLabel.BackColor = Color.White;
            confLabel.Font = new Font("Microsoft YaHei UI", 12F);
            confLabel.Location = new Point(0, 33);
            confLabel.Name = "confLabel";
            confLabel.Size = new Size(220, 27);
            confLabel.TabIndex = 0;
            confLabel.Text = "  报工数量：";
            // 
            // panel4
            // 
            tableLayoutPanel1.SetColumnSpan(panel4, 3);
            panel4.Controls.Add(tableLayoutPanel4);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(405, 148);
            panel4.Name = "panel4";
            tableLayoutPanel1.SetRowSpan(panel4, 3);
            panel4.Size = new Size(300, 299);
            panel4.TabIndex = 3;
            panel4.Text = "panel4";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = SystemColors.Control;
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(label4, 0, 0);
            tableLayoutPanel4.Controls.Add(panel13, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(300, 299);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // label4
            // 
            label4.BackColor = Color.White;
            label4.Dock = DockStyle.Fill;
            label4.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            label4.Location = new Point(3, 3);
            label4.Name = "label4";
            label4.Size = new Size(294, 29);
            label4.TabIndex = 0;
            label4.Text = "  报工记录";
            // 
            // panel13
            // 
            panel13.Controls.Add(ConfTable);
            panel13.Dock = DockStyle.Fill;
            panel13.Location = new Point(3, 38);
            panel13.Name = "panel13";
            panel13.Size = new Size(294, 258);
            panel13.TabIndex = 1;
            panel13.Text = "panel13";
            // 
            // ConfTable
            // 
            ConfTable.BackColor = Color.White;
            ConfTable.Dock = DockStyle.Fill;
            ConfTable.Font = new Font("Microsoft YaHei UI", 10F);
            ConfTable.Gap = 12;
            ConfTable.Location = new Point(0, 0);
            ConfTable.Name = "ConfTable";
            ConfTable.Size = new Size(294, 258);
            ConfTable.TabIndex = 0;
            ConfTable.Text = "table1";
            // 
            // panel5
            // 
            tableLayoutPanel1.SetColumnSpan(panel5, 2);
            panel5.Controls.Add(tableLayoutPanel3);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(711, 148);
            panel5.Name = "panel5";
            tableLayoutPanel1.SetRowSpan(panel5, 3);
            panel5.Size = new Size(237, 299);
            panel5.TabIndex = 4;
            panel5.Text = "panel5";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = SystemColors.Control;
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(label5, 0, 0);
            tableLayoutPanel3.Controls.Add(panel10, 0, 1);
            tableLayoutPanel3.Controls.Add(panel12, 0, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel3.Size = new Size(237, 299);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // label5
            // 
            label5.BackColor = Color.White;
            label5.Dock = DockStyle.Fill;
            label5.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
            label5.Location = new Point(3, 3);
            label5.Name = "label5";
            label5.Size = new Size(231, 29);
            label5.TabIndex = 0;
            label5.Text = "  报工录入";
            // 
            // panel10
            // 
            panel10.Controls.Add(label7);
            panel10.Controls.Add(inputNumber2);
            panel10.Controls.Add(label6);
            panel10.Controls.Add(ConfInputNumber);
            panel10.Dock = DockStyle.Fill;
            panel10.Location = new Point(3, 38);
            panel10.Name = "panel10";
            panel10.Size = new Size(231, 158);
            panel10.TabIndex = 5;
            panel10.Text = "panel10";
            // 
            // label7
            // 
            label7.BackColor = Color.White;
            label7.Font = new Font("Microsoft YaHei UI", 10F);
            label7.Location = new Point(0, 78);
            label7.Name = "label7";
            label7.Size = new Size(231, 31);
            label7.TabIndex = 2;
            label7.Text = "   报废数量：";
            // 
            // inputNumber2
            // 
            inputNumber2.Font = new Font("Microsoft YaHei UI", 11F);
            inputNumber2.Location = new Point(0, 107);
            inputNumber2.Name = "inputNumber2";
            inputNumber2.Size = new Size(228, 48);
            inputNumber2.TabIndex = 3;
            inputNumber2.Text = "0";
            inputNumber2.TextAlign = HorizontalAlignment.Right;
            // 
            // label6
            // 
            label6.BackColor = Color.White;
            label6.Dock = DockStyle.Top;
            label6.Font = new Font("Microsoft YaHei UI", 10F);
            label6.Location = new Point(0, 0);
            label6.Name = "label6";
            label6.Size = new Size(231, 31);
            label6.TabIndex = 0;
            label6.Text = "   报工数量：";
            // 
            // ConfInputNumber
            // 
            ConfInputNumber.Font = new Font("Microsoft YaHei UI", 11F);
            ConfInputNumber.Location = new Point(0, 29);
            ConfInputNumber.Name = "ConfInputNumber";
            ConfInputNumber.Size = new Size(228, 48);
            ConfInputNumber.TabIndex = 1;
            ConfInputNumber.Text = "0";
            ConfInputNumber.TextAlign = HorizontalAlignment.Right;
            // 
            // panel12
            // 
            panel12.Controls.Add(ResetButton);
            panel12.Controls.Add(SubmitButton);
            panel12.Dock = DockStyle.Fill;
            panel12.Location = new Point(3, 202);
            panel12.Name = "panel12";
            panel12.Size = new Size(231, 94);
            panel12.TabIndex = 7;
            panel12.Text = "panel12";
            // 
            // ResetButton
            // 
            ResetButton.Anchor = AnchorStyles.None;
            ResetButton.BorderWidth = 1F;
            ResetButton.Font = new Font("Microsoft YaHei UI", 11F);
            ResetButton.Location = new Point(-2, 49);
            ResetButton.Name = "ResetButton";
            ResetButton.Size = new Size(231, 45);
            ResetButton.TabIndex = 1;
            ResetButton.Text = "重置";
            // 
            // SubmitButton
            // 
            SubmitButton.Anchor = AnchorStyles.None;
            SubmitButton.Font = new Font("Microsoft YaHei UI", 11F);
            SubmitButton.Location = new Point(-2, 3);
            SubmitButton.Name = "SubmitButton";
            SubmitButton.Size = new Size(231, 45);
            SubmitButton.TabIndex = 0;
            SubmitButton.Text = "确认报工";
            SubmitButton.Type = AntdUI.TTypeMini.Primary;
            SubmitButton.Click += SubmitButton_Click;
            // 
            // WorkOrderReportInspectionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(951, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "WorkOrderReportInspectionForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            panel14.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel4.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            panel13.ResumeLayout(false);
            panel5.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel12.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Panel panel4;
        private AntdUI.Panel panel5;
        private AntdUI.Button SearchButton;
        private AntdUI.Select WorkcenterSelect;
        private AntdUI.Select WorkcenterGroupSelect;
        private AntdUI.DatePickerRange DispathchDatePickerRange;
        private TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Panel panel6;
        private AntdUI.Label Orderlabel;
        private AntdUI.Label Desclabel;
        private AntdUI.Label confLabel;
        private AntdUI.Progress progressControl;
        private TableLayoutPanel tableLayoutPanel4;
        private AntdUI.Label label4;
        private TableLayoutPanel tableLayoutPanel3;
        private AntdUI.Label label5;
        private AntdUI.Panel panel10;
        private AntdUI.Label label6;
        private AntdUI.InputNumber ConfInputNumber;
        private AntdUI.Label label7;
        private AntdUI.InputNumber inputNumber2;
        private AntdUI.Panel panel12;
        private AntdUI.Button ResetButton;
        private AntdUI.Button SubmitButton;
        private TableLayoutPanel tableLayoutPanel5;
        private AntdUI.Panel panel14;
        private AntdUI.Panel panel13;
        private AntdUI.Table ConfTable;
        private AntdUI.Input OrderScanInput;
        private AntdUI.Label Matlabel;
        private AntdUI.Label LeadMatlabel;
        private AntdUI.Panel panel8;
        private AntdUI.Table OrderTable;
        private AntdUI.Panel panel7;
    }
}