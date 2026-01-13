namespace BizLink.MES.WinForms
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            panel1 = new AntdUI.Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            loginButton = new AntdUI.Button();
            button1 = new AntdUI.Button();
            passwordInput = new AntdUI.Input();
            usernameInput = new AntdUI.Input();
            label3 = new Label();
            label2 = new Label();
            pictureBox1 = new PictureBox();
            label1 = new AntdUI.Label();
            chkRememberMe = new AntdUI.Checkbox();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(550, 450);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 38F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 38F));
            tableLayoutPanel1.Controls.Add(loginButton, 1, 7);
            tableLayoutPanel1.Controls.Add(button1, 2, 7);
            tableLayoutPanel1.Controls.Add(passwordInput, 1, 5);
            tableLayoutPanel1.Controls.Add(usernameInput, 1, 4);
            tableLayoutPanel1.Controls.Add(label3, 1, 3);
            tableLayoutPanel1.Controls.Add(label2, 1, 2);
            tableLayoutPanel1.Controls.Add(pictureBox1, 1, 1);
            tableLayoutPanel1.Controls.Add(label1, 2, 6);
            tableLayoutPanel1.Controls.Add(chkRememberMe, 1, 6);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 9;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 19.9720268F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 11.8869972F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.756235F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15.6275635F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15.6275635F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5020523F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15.6275635F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutPanel1.Size = new Size(550, 450);
            tableLayoutPanel1.TabIndex = 100;
            // 
            // loginButton
            // 
            loginButton.Dock = DockStyle.Fill;
            loginButton.Font = new Font("Microsoft YaHei UI", 11F);
            loginButton.Location = new Point(41, 357);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(231, 55);
            loginButton.TabIndex = 3;
            loginButton.Text = "登录";
            loginButton.Type = AntdUI.TTypeMini.Primary;
            loginButton.Click += button1_ClickAsync;
            // 
            // button1
            // 
            button1.BorderWidth = 1F;
            button1.Dock = DockStyle.Fill;
            button1.Font = new Font("Microsoft YaHei UI", 11F);
            button1.Location = new Point(278, 357);
            button1.Name = "button1";
            button1.Size = new Size(231, 55);
            button1.TabIndex = 4;
            button1.Text = "取消";
            button1.Click += button1_Click;
            // 
            // passwordInput
            // 
            tableLayoutPanel1.SetColumnSpan(passwordInput, 2);
            passwordInput.Dock = DockStyle.Fill;
            passwordInput.Font = new Font("Microsoft YaHei UI", 11F);
            passwordInput.IconGap = 0.5F;
            passwordInput.IconRatio = 1F;
            passwordInput.Location = new Point(41, 247);
            passwordInput.Name = "passwordInput";
            passwordInput.Prefix = (Image)resources.GetObject("passwordInput.Prefix");
            passwordInput.Size = new Size(468, 55);
            passwordInput.TabIndex = 2;
            passwordInput.UseSystemPasswordChar = true;
            passwordInput.KeyPress += passwordInput_KeyPress;
            // 
            // usernameInput
            // 
            tableLayoutPanel1.SetColumnSpan(usernameInput, 2);
            usernameInput.Dock = DockStyle.Fill;
            usernameInput.Font = new Font("Microsoft YaHei UI", 11F);
            usernameInput.IconGap = 0.5F;
            usernameInput.IconRatio = 1F;
            usernameInput.Location = new Point(41, 186);
            usernameInput.Name = "usernameInput";
            usernameInput.Prefix = (Image)resources.GetObject("usernameInput.Prefix");
            usernameInput.Size = new Size(468, 55);
            usernameInput.TabIndex = 1;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(label3, 2);
            label3.Font = new Font("Microsoft YaHei UI Light", 10F);
            label3.Location = new Point(207, 156);
            label3.Name = "label3";
            label3.Size = new Size(135, 20);
            label3.TabIndex = 99;
            label3.Text = "登录以继续您的操作";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.None;
            label2.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(label2, 2);
            label2.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            label2.Location = new Point(220, 110);
            label2.Name = "label2";
            label2.Size = new Size(110, 31);
            label2.TabIndex = 99;
            label2.Text = "欢迎回来";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.SetColumnSpan(pictureBox1, 2);
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(185, 44);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(180, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left;
            label1.Font = new Font("Microsoft YaHei UI", 11F);
            label1.ForeColor = Color.Blue;
            label1.Location = new Point(278, 318);
            label1.Name = "label1";
            label1.Size = new Size(74, 23);
            label1.TabIndex = 10;
            label1.Text = "忘记密码?";
            label1.Visible = false;
            // 
            // chkRememberMe
            // 
            chkRememberMe.Anchor = AnchorStyles.Left;
            chkRememberMe.BackColor = SystemColors.ControlLightLight;
            chkRememberMe.Font = new Font("Microsoft YaHei UI", 11F);
            chkRememberMe.Location = new Point(41, 318);
            chkRememberMe.Name = "chkRememberMe";
            chkRememberMe.Size = new Size(112, 23);
            chkRememberMe.TabIndex = 9;
            chkRememberMe.Text = "记住我";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(550, 450);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LoginForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Load += LoginForm_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private AntdUI.Panel panel1;
        private AntdUI.Button loginButton;
        private AntdUI.Label label1;
        private AntdUI.Checkbox chkRememberMe;
        private AntdUI.Input passwordInput;
        private AntdUI.Input usernameInput;
        private Label label3;
        private Label label2;
        private AntdUI.Button button1;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox1;
    }
}