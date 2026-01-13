namespace BizLink.MES.WinForms.Forms
{
    partial class InventoryAddForm
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
            label1 = new AntdUI.Label();
            panel2 = new AntdUI.Panel();
            divider1 = new AntdUI.Divider();
            panel3 = new AntdUI.Panel();
            changeTypeSelect = new AntdUI.Select();
            label4 = new AntdUI.Label();
            OldInputNumber = new AntdUI.InputNumber();
            label9 = new AntdUI.Label();
            OldbatchInput = new AntdUI.Input();
            label6 = new AntdUI.Label();
            ChangeInputNumber = new AntdUI.InputNumber();
            OldbarcodeInput = new AntdUI.Input();
            materialnput = new AntdUI.Input();
            label5 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            panel4 = new AntdUI.Panel();
            cancelButton = new AntdUI.Button();
            submitButton = new AntdUI.Button();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel3, 0, 2);
            tableLayoutPanel1.Controls.Add(panel4, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.Size = new Size(450, 329);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(444, 29);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.BackColor = Color.White;
            label1.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
            label1.Location = new Point(7, 3);
            label1.Name = "label1";
            label1.Size = new Size(152, 23);
            label1.TabIndex = 1;
            label1.Text = "MES标签调整";
            // 
            // panel2
            // 
            panel2.Controls.Add(divider1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 38);
            panel2.Name = "panel2";
            panel2.Size = new Size(444, 9);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // divider1
            // 
            divider1.BackColor = Color.White;
            divider1.ColorSplit = SystemColors.ControlLight;
            divider1.Dock = DockStyle.Fill;
            divider1.Location = new Point(0, 0);
            divider1.Name = "divider1";
            divider1.Size = new Size(444, 9);
            divider1.TabIndex = 1;
            divider1.Text = "";
            // 
            // panel3
            // 
            panel3.Controls.Add(changeTypeSelect);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(OldInputNumber);
            panel3.Controls.Add(label9);
            panel3.Controls.Add(OldbatchInput);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(ChangeInputNumber);
            panel3.Controls.Add(OldbarcodeInput);
            panel3.Controls.Add(materialnput);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 53);
            panel3.Name = "panel3";
            panel3.Size = new Size(444, 228);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // changeTypeSelect
            // 
            changeTypeSelect.Font = new Font("Microsoft YaHei UI", 11F);
            changeTypeSelect.Location = new Point(1, 178);
            changeTypeSelect.Name = "changeTypeSelect";
            changeTypeSelect.Size = new Size(200, 40);
            changeTypeSelect.TabIndex = 21;
            // 
            // label4
            // 
            label4.Font = new Font("Microsoft YaHei UI", 10F);
            label4.Location = new Point(7, 149);
            label4.Name = "label4";
            label4.Prefix = "*";
            label4.Size = new Size(75, 23);
            label4.TabIndex = 20;
            label4.Text = "调整类型";
            // 
            // OldInputNumber
            // 
            OldInputNumber.DecimalPlaces = 3;
            OldInputNumber.Enabled = false;
            OldInputNumber.Font = new Font("Microsoft YaHei UI", 11F);
            OldInputNumber.Location = new Point(233, 103);
            OldInputNumber.Name = "OldInputNumber";
            OldInputNumber.ReadOnly = true;
            OldInputNumber.SelectionStart = 5;
            OldInputNumber.Size = new Size(200, 40);
            OldInputNumber.TabIndex = 19;
            OldInputNumber.Text = "0.000";
            // 
            // label9
            // 
            label9.Font = new Font("Microsoft YaHei UI", 10F);
            label9.Location = new Point(241, 74);
            label9.Name = "label9";
            label9.Prefix = "";
            label9.Size = new Size(75, 23);
            label9.TabIndex = 17;
            label9.Text = "源数量";
            // 
            // OldbatchInput
            // 
            OldbatchInput.Enabled = false;
            OldbatchInput.Font = new Font("Microsoft YaHei UI", 11F);
            OldbatchInput.Location = new Point(1, 103);
            OldbatchInput.Name = "OldbatchInput";
            OldbatchInput.ReadOnly = true;
            OldbatchInput.Size = new Size(200, 40);
            OldbatchInput.TabIndex = 15;
            // 
            // label6
            // 
            label6.Font = new Font("Microsoft YaHei UI", 10F);
            label6.Location = new Point(9, 74);
            label6.Name = "label6";
            label6.Prefix = "";
            label6.Size = new Size(75, 23);
            label6.TabIndex = 13;
            label6.Text = "源批次";
            // 
            // ChangeInputNumber
            // 
            ChangeInputNumber.DecimalPlaces = 3;
            ChangeInputNumber.Font = new Font("Microsoft YaHei UI", 11F);
            ChangeInputNumber.Location = new Point(233, 178);
            ChangeInputNumber.Name = "ChangeInputNumber";
            ChangeInputNumber.SelectionStart = 5;
            ChangeInputNumber.Size = new Size(200, 40);
            ChangeInputNumber.TabIndex = 9;
            // 
            // OldbarcodeInput
            // 
            OldbarcodeInput.Font = new Font("Microsoft YaHei UI", 11F);
            OldbarcodeInput.Location = new Point(1, 32);
            OldbarcodeInput.Name = "OldbarcodeInput";
            OldbarcodeInput.Size = new Size(200, 40);
            OldbarcodeInput.TabIndex = 7;
            OldbarcodeInput.KeyPress += OldbarcodeInput_KeyPress;
            // 
            // materialnput
            // 
            materialnput.Enabled = false;
            materialnput.Font = new Font("Microsoft YaHei UI", 11F);
            materialnput.Location = new Point(233, 32);
            materialnput.Name = "materialnput";
            materialnput.ReadOnly = true;
            materialnput.Size = new Size(200, 40);
            materialnput.TabIndex = 6;
            // 
            // label5
            // 
            label5.Font = new Font("Microsoft YaHei UI", 10F);
            label5.Location = new Point(241, 149);
            label5.Name = "label5";
            label5.Prefix = "*";
            label5.Size = new Size(75, 23);
            label5.TabIndex = 3;
            label5.Text = "调整数量";
            // 
            // label3
            // 
            label3.Font = new Font("Microsoft YaHei UI", 10F);
            label3.Location = new Point(9, 3);
            label3.Name = "label3";
            label3.Prefix = "*";
            label3.Size = new Size(75, 23);
            label3.TabIndex = 1;
            label3.Text = "源标签";
            // 
            // label2
            // 
            label2.Font = new Font("Microsoft YaHei UI", 10F);
            label2.Location = new Point(241, 3);
            label2.Name = "label2";
            label2.Prefix = "";
            label2.Size = new Size(75, 23);
            label2.TabIndex = 0;
            label2.Text = "物料号";
            // 
            // panel4
            // 
            panel4.Controls.Add(cancelButton);
            panel4.Controls.Add(submitButton);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 287);
            panel4.Name = "panel4";
            panel4.Size = new Size(444, 39);
            panel4.TabIndex = 3;
            panel4.Text = "panel4";
            // 
            // cancelButton
            // 
            cancelButton.BorderWidth = 1F;
            cancelButton.Location = new Point(280, -1);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 40);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "取消";
            cancelButton.Click += cancelButton_Click;
            // 
            // submitButton
            // 
            submitButton.Location = new Point(360, -1);
            submitButton.Name = "submitButton";
            submitButton.Size = new Size(75, 40);
            submitButton.TabIndex = 2;
            submitButton.Text = "提交";
            submitButton.Type = AntdUI.TTypeMini.Primary;
            submitButton.Click += submitButton_Click;
            // 
            // InventoryAddForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(450, 329);
            Controls.Add(tableLayoutPanel1);
            Name = "InventoryAddForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "InventoryAddForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Label label1;
        private AntdUI.Panel panel2;
        private AntdUI.Divider divider1;
        private AntdUI.Panel panel3;
        private AntdUI.InputNumber ChangeInputNumber;
        private AntdUI.Input OldbarcodeInput;
        private AntdUI.Input materialnput;
        private AntdUI.Label label5;
        private AntdUI.Label label3;
        private AntdUI.Label label2;
        private AntdUI.Panel panel4;
        private AntdUI.Button cancelButton;
        private AntdUI.Button submitButton;
        private AntdUI.InputNumber OldInputNumber;
        private AntdUI.Label label9;
        private AntdUI.Input OldbatchInput;
        private AntdUI.Label label6;
        private AntdUI.Select changeTypeSelect;
        private AntdUI.Label label4;
    }
}