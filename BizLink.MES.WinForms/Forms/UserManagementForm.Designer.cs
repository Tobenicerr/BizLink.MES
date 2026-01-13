namespace BizLink.MES.WinForms.Forms
{
    partial class UserManagementForm
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
            this.resetButton = new AntdUI.Button();
            this.searchButton = new AntdUI.Button();
            this.label2 = new AntdUI.Label();
            this.label1 = new AntdUI.Label();
            this.statusSelect = new AntdUI.Select();
            this.keywordInput = new AntdUI.Input();
            this.panel2 = new AntdUI.Panel();
            this.panel4 = new AntdUI.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.paginationControl = new AntdUI.Pagination();
            this.panel3 = new AntdUI.Panel();
            this.label3 = new AntdUI.Label();
            this.btnNewUser = new AntdUI.Button();
            this.panel5 = new AntdUI.Panel();
            this.userTable = new AntdUI.Table();
            this.SpinControl = new AntdUI.Spin();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(968, 579);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.resetButton);
            this.panel1.Controls.Add(this.searchButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.statusSelect);
            this.panel1.Controls.Add(this.keywordInput);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(962, 109);
            this.panel1.TabIndex = 0;
            this.panel1.Text = "panel1";
            // 
            // resetButton
            // 
            this.resetButton.BorderWidth = 1F;
            this.resetButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.resetButton.Location = new System.Drawing.Point(536, 35);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(94, 41);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "重置";
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.searchButton.Location = new System.Drawing.Point(436, 35);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(94, 41);
            this.searchButton.TabIndex = 4;
            this.searchButton.Text = "查询";
            this.searchButton.Type = AntdUI.TTypeMini.Primary;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.label2.Location = new System.Drawing.Point(217, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "账户状态";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.label1.Location = new System.Drawing.Point(19, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "工号/姓名";
            // 
            // statusSelect
            // 
            this.statusSelect.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.statusSelect.Location = new System.Drawing.Point(207, 41);
            this.statusSelect.Name = "statusSelect";
            this.statusSelect.Size = new System.Drawing.Size(137, 35);
            this.statusSelect.TabIndex = 1;
            // 
            // keywordInput
            // 
            this.keywordInput.Font = new System.Drawing.Font("Microsoft YaHei UI", 11F);
            this.keywordInput.Location = new System.Drawing.Point(15, 41);
            this.keywordInput.Name = "keywordInput";
            this.keywordInput.Size = new System.Drawing.Size(137, 35);
            this.keywordInput.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 118);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(962, 458);
            this.panel2.TabIndex = 1;
            this.panel2.Text = "panel2";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tableLayoutPanel2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(962, 458);
            this.panel4.TabIndex = 7;
            this.panel4.Text = "panel4";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.paginationControl, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel5, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 82F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(962, 458);
            this.tableLayoutPanel2.TabIndex = 12;
            // 
            // paginationControl
            // 
            this.paginationControl.BackColor = System.Drawing.Color.Transparent;
            this.paginationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paginationControl.Location = new System.Drawing.Point(3, 423);
            this.paginationControl.Name = "paginationControl";
            this.paginationControl.ShowSizeChanger = true;
            this.paginationControl.Size = new System.Drawing.Size(956, 32);
            this.paginationControl.TabIndex = 6;
            this.paginationControl.Text = "pagination1";
            this.paginationControl.ValueChanged += new AntdUI.PageValueEventHandler(this.paginationControl_ValueChanged);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Transparent;
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.btnNewUser);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(956, 39);
            this.panel3.TabIndex = 8;
            this.panel3.Text = "panel3";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(16, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 35);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户列表";
            // 
            // btnNewUser
            // 
            this.btnNewUser.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.btnNewUser.Location = new System.Drawing.Point(864, 0);
            this.btnNewUser.Name = "btnNewUser";
            this.btnNewUser.Size = new System.Drawing.Size(92, 39);
            this.btnNewUser.TabIndex = 5;
            this.btnNewUser.Text = "新增用户";
            this.btnNewUser.Type = AntdUI.TTypeMini.Primary;
            this.btnNewUser.Click += new System.EventHandler(this.btnNewUser_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.userTable);
            this.panel5.Controls.Add(this.SpinControl);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 48);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(956, 369);
            this.panel5.TabIndex = 9;
            this.panel5.Text = "panel5";
            // 
            // userTable
            // 
            this.userTable.BackColor = System.Drawing.SystemColors.Control;
            this.userTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userTable.Gap = 12;
            this.userTable.Location = new System.Drawing.Point(0, 0);
            this.userTable.Name = "userTable";
            this.userTable.Size = new System.Drawing.Size(956, 369);
            this.userTable.TabIndex = 0;
            this.userTable.Text = "table1";
            this.userTable.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.userTable_CellButtonClick);
            // 
            // SpinControl
            // 
            this.SpinControl.Location = new System.Drawing.Point(370, 105);
            this.SpinControl.Name = "SpinControl";
            this.SpinControl.Size = new System.Drawing.Size(75, 62);
            this.SpinControl.TabIndex = 11;
            this.SpinControl.Text = "等待中...";
            this.SpinControl.Visible = false; // 默认隐藏
            // 
            // UserManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 579);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UserManagementForm";
            this.ShowInTaskbar = false;
            this.Text = "UserManagementForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Select statusSelect;
        private AntdUI.Input keywordInput;
        private AntdUI.Panel panel2;
        private AntdUI.Button resetButton;
        private AntdUI.Button searchButton;
        private AntdUI.Label label2;
        private AntdUI.Label label1;
        private AntdUI.Panel panel4;
        private AntdUI.Label label3;
        private AntdUI.Button btnNewUser;
        private AntdUI.Panel panel3;
        private AntdUI.Pagination paginationControl;
        private AntdUI.Spin SpinControl;
        private AntdUI.Table userTable;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Panel panel5;
    }
}