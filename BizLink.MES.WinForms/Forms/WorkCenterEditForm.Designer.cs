namespace BizLink.MES.WinForms.Forms
{
    partial class WorkCenterEditForm
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
            workCenterCodeInput = new AntdUI.Input();
            saveButtom = new AntdUI.Button();
            groupSwitch = new AntdUI.Switch();
            workCenterDescInput = new AntdUI.Input();
            workCenterNameInput = new AntdUI.Input();
            label4 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            label1 = new AntdUI.Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel2 = new AntdUI.Panel();
            addButtom = new AntdUI.Button();
            select1 = new AntdUI.Select();
            label6 = new AntdUI.Label();
            table1 = new AntdUI.Table();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Location = new Point(5, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tableLayoutPanel1.Size = new Size(783, 445);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(workCenterCodeInput);
            panel1.Controls.Add(saveButtom);
            panel1.Controls.Add(groupSwitch);
            panel1.Controls.Add(workCenterDescInput);
            panel1.Controls.Add(workCenterNameInput);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(777, 194);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // workCenterCodeInput
            // 
            workCenterCodeInput.Font = new Font("Microsoft YaHei UI", 12F);
            workCenterCodeInput.Location = new Point(191, 6);
            workCenterCodeInput.Name = "workCenterCodeInput";
            workCenterCodeInput.Size = new Size(166, 40);
            workCenterCodeInput.TabIndex = 9;
            // 
            // saveButtom
            // 
            saveButtom.Location = new Point(686, 147);
            saveButtom.Name = "saveButtom";
            saveButtom.Size = new Size(88, 37);
            saveButtom.TabIndex = 8;
            saveButtom.Text = "保存";
            saveButtom.Type = AntdUI.TTypeMini.Primary;
            saveButtom.Click += saveButtom_Click;
            // 
            // groupSwitch
            // 
            groupSwitch.BackColor = Color.White;
            groupSwitch.Location = new Point(243, 154);
            groupSwitch.Name = "groupSwitch";
            groupSwitch.Size = new Size(47, 30);
            groupSwitch.TabIndex = 7;
            groupSwitch.Text = "switch1";
            groupSwitch.CheckedChanged += groupSwitch_CheckedChanged;
            // 
            // workCenterDescInput
            // 
            workCenterDescInput.Font = new Font("Microsoft YaHei UI", 12F);
            workCenterDescInput.Location = new Point(191, 98);
            workCenterDescInput.Name = "workCenterDescInput";
            workCenterDescInput.Size = new Size(166, 40);
            workCenterDescInput.TabIndex = 6;
            // 
            // workCenterNameInput
            // 
            workCenterNameInput.Font = new Font("Microsoft YaHei UI", 12F);
            workCenterNameInput.Location = new Point(191, 52);
            workCenterNameInput.Name = "workCenterNameInput";
            workCenterNameInput.Size = new Size(166, 40);
            workCenterNameInput.TabIndex = 5;
            // 
            // label4
            // 
            label4.BackColor = Color.White;
            label4.Font = new Font("Microsoft YaHei UI", 12F);
            label4.Location = new Point(43, 144);
            label4.Name = "label4";
            label4.Size = new Size(120, 40);
            label4.TabIndex = 3;
            label4.Text = "工作中心组：";
            // 
            // label3
            // 
            label3.BackColor = Color.White;
            label3.Font = new Font("Microsoft YaHei UI", 12F);
            label3.Location = new Point(43, 98);
            label3.Name = "label3";
            label3.Size = new Size(120, 40);
            label3.TabIndex = 2;
            label3.Text = "工作中心描述：";
            // 
            // label2
            // 
            label2.BackColor = Color.White;
            label2.Font = new Font("Microsoft YaHei UI", 12F);
            label2.Location = new Point(43, 52);
            label2.Name = "label2";
            label2.Size = new Size(120, 40);
            label2.TabIndex = 1;
            label2.Text = "工作中心名称：";
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Font = new Font("Microsoft YaHei UI", 12F);
            label1.Location = new Point(43, 6);
            label1.Name = "label1";
            label1.Size = new Size(120, 40);
            label1.TabIndex = 0;
            label1.Text = "工作中心代码：";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel2, 0, 0);
            tableLayoutPanel2.Controls.Add(table1, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 203);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(777, 239);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(addButtom);
            panel2.Controls.Add(select1);
            panel2.Controls.Add(label6);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(771, 44);
            panel2.TabIndex = 0;
            panel2.Text = "panel2";
            // 
            // addButtom
            // 
            addButtom.Location = new Point(358, 9);
            addButtom.Name = "addButtom";
            addButtom.Size = new Size(85, 35);
            addButtom.TabIndex = 2;
            addButtom.Text = "添加";
            addButtom.Type = AntdUI.TTypeMini.Primary;
            // 
            // select1
            // 
            select1.Location = new Point(188, 5);
            select1.Name = "select1";
            select1.Size = new Size(164, 38);
            select1.TabIndex = 1;
            // 
            // label6
            // 
            label6.BackColor = Color.White;
            label6.Font = new Font("Microsoft YaHei UI", 12F);
            label6.Location = new Point(40, 3);
            label6.Name = "label6";
            label6.Size = new Size(120, 40);
            label6.TabIndex = 0;
            label6.Text = "选择工作中心：";
            // 
            // table1
            // 
            table1.BackColor = Color.White;
            table1.Dock = DockStyle.Fill;
            table1.Gap = 12;
            table1.Location = new Point(3, 53);
            table1.Name = "table1";
            table1.Size = new Size(771, 183);
            table1.TabIndex = 1;
            table1.Text = "table1";
            // 
            // WorkCenterEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "WorkCenterEditForm";
            Text = "WorkCenterEditForm";
            Load += WorkCenterEditForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Switch groupSwitch;
        private AntdUI.Input workCenterDescInput;
        private AntdUI.Input workCenterNameInput;
        private AntdUI.Label label4;
        private AntdUI.Label label3;
        private AntdUI.Label label2;
        private AntdUI.Label label1;
        private AntdUI.Button saveButtom;
        private AntdUI.Input workCenterCodeInput;
        private TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Panel panel2;
        private AntdUI.Button addButtom;
        private AntdUI.Select select1;
        private AntdUI.Label label6;
        private AntdUI.Table table1;
    }
}