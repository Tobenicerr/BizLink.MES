namespace BizLink.MES.WinForms
{
    partial class HomeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HomeForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new AntdUI.Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new AntdUI.Label();
            label2 = new AntdUI.Label();
            panel2 = new AntdUI.Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            pictureBox1 = new PictureBox();
            label4 = new AntdUI.Label();
            label3 = new AntdUI.Label();
            panel3 = new AntdUI.Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            pictureBox2 = new PictureBox();
            label6 = new AntdUI.Label();
            label5 = new AntdUI.Label();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel2, 0, 1);
            tableLayoutPanel1.Controls.Add(panel3, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.Size = new Size(889, 507);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            tableLayoutPanel1.SetColumnSpan(panel1, 4);
            panel1.Controls.Add(tableLayoutPanel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(883, 95);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(label1, 1, 0);
            tableLayoutPanel2.Controls.Add(label2, 1, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tableLayoutPanel2.Size = new Size(883, 95);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            label1.Location = new Point(23, 3);
            label1.Name = "label1";
            label1.Size = new Size(585, 51);
            label1.TabIndex = 0;
            label1.Text = "欢迎，{0}！";
            // 
            // label2
            // 
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Microsoft YaHei UI", 11F);
            label2.Location = new Point(23, 60);
            label2.Name = "label2";
            label2.Size = new Size(639, 30);
            label2.TabIndex = 1;
            label2.Text = "今天是{0}年{1}月{2}日，{3}。祝您拥有高效的一天！";
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(tableLayoutPanel3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 104);
            panel2.Name = "panel2";
            panel2.Radius = 10;
            panel2.ShadowColor = Color.Gainsboro;
            panel2.Size = new Size(216, 95);
            panel2.TabIndex = 1;
            panel2.Text = "panel2";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(label4, 1, 1);
            tableLayoutPanel3.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel3.Controls.Add(label3, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(216, 95);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.None;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(26, 20);
            pictureBox1.Name = "pictureBox1";
            tableLayoutPanel3.SetRowSpan(pictureBox1, 2);
            pictureBox1.Size = new Size(55, 55);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.None;
            label4.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold);
            label4.Location = new Point(120, 50);
            label4.Name = "label4";
            label4.Size = new Size(83, 42);
            label4.TabIndex = 2;
            label4.Text = "用户管理";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.None;
            label3.Font = new Font("Microsoft YaHei UI", 11F);
            label3.Location = new Point(124, 12);
            label3.Name = "label3";
            label3.Size = new Size(75, 23);
            label3.TabIndex = 1;
            label3.Text = "常用功能";
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(tableLayoutPanel4);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(225, 104);
            panel3.Name = "panel3";
            panel3.Radius = 10;
            panel3.Size = new Size(216, 95);
            panel3.TabIndex = 2;
            panel3.Text = "panel3";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(label6, 1, 1);
            tableLayoutPanel4.Controls.Add(pictureBox2, 0, 0);
            tableLayoutPanel4.Controls.Add(label5, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(216, 95);
            tableLayoutPanel4.TabIndex = 3;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.None;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(26, 20);
            pictureBox2.Name = "pictureBox2";
            tableLayoutPanel4.SetRowSpan(pictureBox2, 2);
            pictureBox2.Size = new Size(55, 55);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.None;
            label6.Font = new Font("Microsoft YaHei UI", 15F, FontStyle.Bold | FontStyle.Underline);
            label6.ForeColor = SystemColors.Highlight;
            label6.Location = new Point(120, 50);
            label6.Name = "label6";
            label6.Size = new Size(84, 41);
            label6.TabIndex = 2;
            label6.Text = "查看文档";
            label6.Click += label6_Click;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.None;
            label5.Font = new Font("Microsoft YaHei UI", 11F);
            label5.Location = new Point(124, 12);
            label5.Name = "label5";
            label5.Size = new Size(75, 23);
            label5.TabIndex = 1;
            label5.Text = "需要帮助？";
            // 
            // HomeForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(889, 507);
            Controls.Add(tableLayoutPanel1);
            Name = "HomeForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "HomeForm";
            Load += HomeForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Label label2;
        private AntdUI.Label label1;
        private AntdUI.Panel panel2;
        private PictureBox pictureBox1;
        private AntdUI.Label label4;
        private AntdUI.Label label3;
        private AntdUI.Panel panel3;
        private AntdUI.Label label6;
        private AntdUI.Label label5;
        private PictureBox pictureBox2;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel4;
    }
}