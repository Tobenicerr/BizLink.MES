namespace BizLink.MES.WinForms
{
    partial class SetPasswordForm
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
            panel1 = new AntdUI.Panel();
            label2 = new AntdUI.Label();
            label1 = new AntdUI.Label();
            inputConfirmPassword = new AntdUI.Input();
            inputNewPassword = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
            btnSave = new AntdUI.Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(inputConfirmPassword);
            panel1.Controls.Add(inputNewPassword);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(410, 148);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft YaHei UI", 12F);
            label2.Location = new Point(30, 105);
            label2.Name = "label2";
            label2.Size = new Size(80, 23);
            label2.TabIndex = 3;
            label2.Text = "密码确认";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft YaHei UI", 12F);
            label1.Location = new Point(30, 55);
            label1.Name = "label1";
            label1.Size = new Size(80, 23);
            label1.TabIndex = 2;
            label1.Text = "密       码";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // inputConfirmPassword
            // 
            inputConfirmPassword.Font = new Font("Microsoft YaHei UI", 12F);
            inputConfirmPassword.Location = new Point(120, 100);
            inputConfirmPassword.Name = "inputConfirmPassword";
            inputConfirmPassword.PlaceholderText = "请再次输入新密码";
            inputConfirmPassword.Size = new Size(250, 36);
            inputConfirmPassword.TabIndex = 1;
            inputConfirmPassword.UseSystemPasswordChar = true;
            // 
            // inputNewPassword
            // 
            inputNewPassword.Font = new Font("Microsoft YaHei UI", 12F);
            inputNewPassword.Location = new Point(120, 50);
            inputNewPassword.Name = "inputNewPassword";
            inputNewPassword.PlaceholderText = "请输入新密码";
            inputNewPassword.Size = new Size(250, 36);
            inputNewPassword.TabIndex = 0;
            inputNewPassword.UseSystemPasswordChar = true;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnSave);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 154);
            panel2.Name = "panel2";
            panel2.Size = new Size(410, 66);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Microsoft YaHei UI", 12F);
            btnSave.Location = new Point(77, 13);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(250, 41);
            btnSave.TabIndex = 0;
            btnSave.Text = "更新";
            btnSave.Type = AntdUI.TTypeMini.Primary;
            btnSave.Click += btnSave_Click;
            // 
            // SetPasswordForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(410, 220);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "SetPasswordForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "首次登录 - 设置密码";
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel1;
        private AntdUI.Label label2;
        private AntdUI.Label label1;
        private AntdUI.Input inputConfirmPassword;
        private AntdUI.Input inputNewPassword;
        private AntdUI.Panel panel2;
        private AntdUI.Button btnSave;
    }
}