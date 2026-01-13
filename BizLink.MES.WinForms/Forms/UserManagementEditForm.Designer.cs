namespace BizLink.MES.WinForms.Forms
{
    partial class UserManagementEditForm
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new AntdUI.Panel();
            this.label9 = new AntdUI.Label();
            this.label8 = new AntdUI.Label();
            this.panel2 = new AntdUI.Panel();
            this.empCodeInput = new AntdUI.Input();
            this.label1 = new AntdUI.Label();
            this.panel3 = new AntdUI.Panel();
            this.label2 = new AntdUI.Label();
            this.domainInput = new AntdUI.Input();
            this.panel4 = new AntdUI.Panel();
            this.label3 = new AntdUI.Label();
            this.nameInput = new AntdUI.Input();
            this.panel5 = new AntdUI.Panel();
            this.label4 = new AntdUI.Label();
            this.passwordInput = new AntdUI.Input();
            this.panel6 = new AntdUI.Panel();
            this.label5 = new AntdUI.Label();
            this.passwordConfirmInput = new AntdUI.Input();
            this.panel7 = new AntdUI.Panel();
            this.FactorySelect = new AntdUI.Select();
            this.label6 = new AntdUI.Label();
            this.divider1 = new AntdUI.Divider();
            this.panel8 = new AntdUI.Panel();
            this.btnCancel = new AntdUI.Button();
            this.btnSave = new AntdUI.Button();
            this.panel9 = new AntdUI.Panel();
            this.statusSwitch = new AntdUI.Switch();
            this.label7 = new AntdUI.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel6, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.panel7, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.divider1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.panel8, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.panel9, 1, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 18.28955F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28414F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28414F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28414F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28414F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28414F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.28975F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(633, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.panel1, 2);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(627, 76);
            this.panel1.TabIndex = 0;
            this.panel1.Text = "panel1";
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label9.Location = new System.Drawing.Point(9, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(160, 22);
            this.label9.TabIndex = 1;
            this.label9.Text = "请填写用户详细信息";
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(9, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(252, 32);
            this.label8.TabIndex = 0;
            this.label8.Text = "新增用户";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.empCodeInput);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 85);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(310, 58);
            this.panel2.TabIndex = 1;
            this.panel2.Text = "panel2";
            // 
            // empCodeInput
            // 
            this.empCodeInput.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.empCodeInput.Location = new System.Drawing.Point(92, 13);
            this.empCodeInput.Name = "empCodeInput";
            this.empCodeInput.Size = new System.Drawing.Size(189, 36);
            this.empCodeInput.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label1.Location = new System.Drawing.Point(9, 18);
            this.label1.Name = "label1";
            this.label1.Prefix = "*";
            this.label1.Size = new System.Drawing.Size(63, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "工      号";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.domainInput);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(319, 85);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(311, 58);
            this.panel3.TabIndex = 8;
            this.panel3.Text = "panel3";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Prefix = "";
            this.label2.Size = new System.Drawing.Size(54, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = " 域 账 号";
            // 
            // domainInput
            // 
            this.domainInput.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.domainInput.Location = new System.Drawing.Point(89, 11);
            this.domainInput.Name = "domainInput";
            this.domainInput.Size = new System.Drawing.Size(189, 38);
            this.domainInput.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.nameInput);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 149);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(310, 58);
            this.panel4.TabIndex = 9;
            this.panel4.Text = "panel4";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label3.Location = new System.Drawing.Point(9, 16);
            this.label3.Name = "label3";
            this.label3.Prefix = "*";
            this.label3.Size = new System.Drawing.Size(63, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "姓      名";
            // 
            // nameInput
            // 
            this.nameInput.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.nameInput.Location = new System.Drawing.Point(92, 13);
            this.nameInput.Name = "nameInput";
            this.nameInput.Size = new System.Drawing.Size(189, 38);
            this.nameInput.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.passwordInput);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 213);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(310, 58);
            this.panel5.TabIndex = 10;
            this.panel5.Text = "panel5";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label4.Location = new System.Drawing.Point(9, 16);
            this.label4.Name = "label4";
            this.label4.Prefix = "*";
            this.label4.Size = new System.Drawing.Size(63, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "密      码";
            // 
            // passwordInput
            // 
            this.passwordInput.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.passwordInput.Location = new System.Drawing.Point(92, 13);
            this.passwordInput.Name = "passwordInput";
            this.passwordInput.Size = new System.Drawing.Size(189, 38);
            this.passwordInput.TabIndex = 4;
            this.passwordInput.UseSystemPasswordChar = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label5);
            this.panel6.Controls.Add(this.passwordConfirmInput);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(319, 213);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(311, 58);
            this.panel6.TabIndex = 11;
            this.panel6.Text = "panel6";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label5.Location = new System.Drawing.Point(12, 16);
            this.label5.Name = "label5";
            this.label5.Prefix = "*";
            this.label5.Size = new System.Drawing.Size(63, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "确认密码";
            // 
            // passwordConfirmInput
            // 
            this.passwordConfirmInput.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.passwordConfirmInput.Location = new System.Drawing.Point(89, 13);
            this.passwordConfirmInput.Name = "passwordConfirmInput";
            this.passwordConfirmInput.Size = new System.Drawing.Size(189, 35);
            this.passwordConfirmInput.TabIndex = 5;
            this.passwordConfirmInput.UseSystemPasswordChar = true;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.FactorySelect);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 277);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(310, 58);
            this.panel7.TabIndex = 13;
            this.panel7.Text = "panel7";
            // 
            // FactorySelect
            // 
            this.FactorySelect.Location = new System.Drawing.Point(92, 10);
            this.FactorySelect.Name = "FactorySelect";
            this.FactorySelect.Size = new System.Drawing.Size(189, 35);
            this.FactorySelect.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label6.Location = new System.Drawing.Point(9, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 23);
            this.label6.TabIndex = 12;
            this.label6.Text = " 所属工厂";
            // 
            // divider1
            // 
            this.divider1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.divider1, 2);
            this.divider1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.divider1.Location = new System.Drawing.Point(3, 341);
            this.divider1.Name = "divider1";
            this.divider1.Size = new System.Drawing.Size(627, 58);
            this.divider1.TabIndex = 14;
            this.divider1.Text = "";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnCancel);
            this.panel8.Controls.Add(this.btnSave);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel8.Location = new System.Drawing.Point(319, 405);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(311, 42);
            this.panel8.TabIndex = 15;
            this.panel8.Text = "panel8";
            // 
            // btnCancel
            // 
            this.btnCancel.BorderWidth = 1F;
            this.btnCancel.Location = new System.Drawing.Point(87, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 37);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(186, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 37);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.Type = AntdUI.TTypeMini.Primary;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.statusSwitch);
            this.panel9.Controls.Add(this.label7);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(319, 277);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(311, 58);
            this.panel9.TabIndex = 16;
            this.panel9.Text = "panel9";
            // 
            // statusSwitch
            // 
            this.statusSwitch.BackColor = System.Drawing.Color.Transparent;
            this.statusSwitch.Checked = true;
            this.statusSwitch.CheckedText = "启用";
            this.statusSwitch.Location = new System.Drawing.Point(89, 15);
            this.statusSwitch.Name = "statusSwitch";
            this.statusSwitch.Size = new System.Drawing.Size(65, 30);
            this.statusSwitch.TabIndex = 1;
            this.statusSwitch.Text = "switch1";
            this.statusSwitch.UnCheckedText = "禁用";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F);
            this.label7.Location = new System.Drawing.Point(12, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 23);
            this.label7.TabIndex = 0;
            this.label7.Text = " 状      态";
            // 
            // UserManagementEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserManagementEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UserManagementEditForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Input nameInput;
        private AntdUI.Panel panel1;
        private AntdUI.Panel panel2;
        private AntdUI.Input empCodeInput;
        private AntdUI.Label label1;
        private AntdUI.Input passwordInput;
        private AntdUI.Input passwordConfirmInput;
        private AntdUI.Input domainInput;
        private AntdUI.Label label2;
        private AntdUI.Panel panel3;
        private AntdUI.Panel panel4;
        private AntdUI.Label label3;
        private AntdUI.Panel panel5;
        private AntdUI.Label label4;
        private AntdUI.Panel panel6;
        private AntdUI.Label label5;
        private AntdUI.Panel panel7;
        private AntdUI.Label label6;
        private AntdUI.Divider divider1;
        private AntdUI.Panel panel8;
        private AntdUI.Button btnCancel;
        private AntdUI.Button btnSave;
        private AntdUI.Panel panel9;
        private AntdUI.Switch statusSwitch;
        private AntdUI.Label label7;
        private AntdUI.Label label9;
        private AntdUI.Label label8;
        private AntdUI.Select FactorySelect;
    }
}