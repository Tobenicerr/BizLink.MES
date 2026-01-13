namespace BizLink.MES.WinForms.Forms
{
    partial class UserPermissionEditForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new AntdUI.Panel();
            this.lblTitle = new AntdUI.Label();
            this.pnlFooter = new AntdUI.Panel();
            this.btnCancel = new AntdUI.Button();
            this.btnSave = new AntdUI.Button();
            this.menuTree = new AntdUI.Tree();
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(400, 50);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(300, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "分配权限";
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 500);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(400, 50);
            this.pnlFooter.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.BorderWidth = 1F;
            this.btnCancel.Location = new System.Drawing.Point(200, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 34);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(290, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 34);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "保存";
            this.btnSave.Type = AntdUI.TTypeMini.Primary;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // menuTree
            // 
            this.menuTree.Checkable = true; // 开启复选框
            this.menuTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuTree.Location = new System.Drawing.Point(0, 50);
            this.menuTree.Name = "menuTree";
            this.menuTree.Size = new System.Drawing.Size(400, 450);
            this.menuTree.TabIndex = 2;
            // 
            // UserPermissionEditForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 550);
            this.Controls.Add(this.menuTree);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Name = "UserPermissionEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "权限管理";
            this.pnlHeader.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private AntdUI.Panel pnlHeader;
        private AntdUI.Label lblTitle;
        private AntdUI.Panel pnlFooter;
        private AntdUI.Button btnCancel;
        private AntdUI.Button btnSave;
        private AntdUI.Tree menuTree;
    }
}