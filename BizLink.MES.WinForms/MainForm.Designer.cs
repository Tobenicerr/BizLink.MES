namespace BizLink.MES.WinForms
{
    partial class MainForm
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

            System.Windows.Forms.Application.Exit();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            AntdUI.Tabs.StyleCard styleCard1 = new AntdUI.Tabs.StyleCard();
            tabs1 = new AntdUI.Tabs();
            panel1 = new AntdUI.Panel();
            pageHeader1 = new AntdUI.PageHeader();
            panel2 = new AntdUI.Panel();
            labelTime1 = new AntdUI.LabelTime();
            dropDownFactory = new AntdUI.Dropdown();
            labelUserInfo = new AntdUI.Label();
            splitter1 = new AntdUI.Splitter();
            mainMenu = new AntdUI.Menu();
            mainTabs = new AntdUI.Tabs();
            breadcrumb = new AntdUI.Breadcrumb();
            panel1.SuspendLayout();
            pageHeader1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitter1).BeginInit();
            splitter1.Panel1.SuspendLayout();
            splitter1.Panel2.SuspendLayout();
            splitter1.SuspendLayout();
            SuspendLayout();
            // 
            // tabs1
            // 
            tabs1.Location = new Point(152, 142);
            tabs1.Name = "tabs1";
            tabs1.Size = new Size(75, 23);
            tabs1.Style = styleLine1;
            tabs1.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.Controls.Add(pageHeader1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1280, 60);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // pageHeader1
            // 
            pageHeader1.BackColor = Color.White;
            pageHeader1.Controls.Add(panel2);
            pageHeader1.Dock = DockStyle.Fill;
            pageHeader1.Font = new Font("Microsoft YaHei UI", 15F);
            pageHeader1.Icon = (Image)resources.GetObject("pageHeader1.Icon");
            pageHeader1.Location = new Point(0, 0);
            pageHeader1.Name = "pageHeader1";
            pageHeader1.ShowButton = true;
            pageHeader1.ShowIcon = true;
            pageHeader1.Size = new Size(1280, 60);
            pageHeader1.SubFont = new Font("Microsoft YaHei UI Light", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pageHeader1.SubText = "";
            pageHeader1.TabIndex = 0;
            pageHeader1.Text = "BizLink Client";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            panel2.BackColor = Color.White;
            panel2.Controls.Add(labelTime1);
            panel2.Controls.Add(dropDownFactory);
            panel2.Location = new Point(455, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(665, 60);
            panel2.TabIndex = 2;
            panel2.Text = "panel2";
            // 
            // labelTime1
            // 
            labelTime1.Location = new Point(458, 5);
            labelTime1.Name = "labelTime1";
            labelTime1.Size = new Size(210, 48);
            labelTime1.TabIndex = 0;
            labelTime1.Text = "labelTime1";
            // 
            // dropDownFactory
            // 
            dropDownFactory.BorderWidth = 1F;
            dropDownFactory.Font = new Font("Microsoft YaHei UI", 12F);
            dropDownFactory.Icon = (Image)resources.GetObject("dropDownFactory.Icon");
            dropDownFactory.IconGap = 0.1F;
            dropDownFactory.IconRatio = 1F;
            dropDownFactory.Location = new Point(215, 10);
            dropDownFactory.Name = "dropDownFactory";
            dropDownFactory.ShowArrow = true;
            dropDownFactory.Size = new Size(212, 40);
            dropDownFactory.TabIndex = 1;
            dropDownFactory.Text = "当前工厂:                  ";
            dropDownFactory.TextCenterHasIcon = true;
            dropDownFactory.SelectedValueChanged += dropDownFactory_SelectedValueChanged;
            // 
            // labelUserInfo
            // 
            labelUserInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            labelUserInfo.BackColor = Color.White;
            labelUserInfo.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
            labelUserInfo.Location = new Point(623, -1);
            labelUserInfo.Name = "labelUserInfo";
            labelUserInfo.RightToLeft = RightToLeft.No;
            labelUserInfo.Size = new Size(190, 22);
            labelUserInfo.TabIndex = 2;
            labelUserInfo.Text = "当前用户：";
            labelUserInfo.Visible = false;
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Fill;
            splitter1.Location = new Point(0, 60);
            splitter1.Name = "splitter1";
            // 
            // splitter1.Panel1
            // 
            splitter1.Panel1.Controls.Add(mainMenu);
            // 
            // splitter1.Panel2
            // 
            splitter1.Panel2.Controls.Add(labelUserInfo);
            splitter1.Panel2.Controls.Add(mainTabs);
            splitter1.Panel2.Controls.Add(breadcrumb);
            splitter1.Size = new Size(1280, 660);
            splitter1.SplitterDistance = 294;
            splitter1.SplitterWidth = 6;
            splitter1.TabIndex = 1;
            // 
            // mainMenu
            // 
            mainMenu.BackColor = Color.White;
            mainMenu.Dock = DockStyle.Fill;
            mainMenu.Font = new Font("Microsoft YaHei UI Light", 11F, FontStyle.Bold);
            mainMenu.Indent = true;
            mainMenu.Location = new Point(0, 0);
            mainMenu.Name = "mainMenu";
            mainMenu.Size = new Size(294, 660);
            mainMenu.TabIndex = 0;
            mainMenu.Text = "menu1";
            mainMenu.SelectChanged += mainMenu_SelectChanged;
            // 
            // mainTabs
            // 
            mainTabs.Dock = DockStyle.Fill;
            mainTabs.Location = new Point(0, 23);
            mainTabs.Name = "mainTabs";
            mainTabs.Size = new Size(980, 637);
            styleCard1.Closable = true;
            mainTabs.Style = styleCard1;
            mainTabs.TabIndex = 1;
            mainTabs.Text = "tabs2";
            mainTabs.Type = AntdUI.TabType.Card;
            mainTabs.SelectedIndexChanged += mainTabs_SelectedIndexChanged;
            // 
            // breadcrumb
            // 
            breadcrumb.BackColor = Color.White;
            breadcrumb.Dock = DockStyle.Top;
            breadcrumb.ForeActive = Color.Black;
            breadcrumb.Location = new Point(0, 0);
            breadcrumb.Name = "breadcrumb";
            breadcrumb.Size = new Size(980, 23);
            breadcrumb.TabIndex = 0;
            breadcrumb.Text = "breadcrumb1";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1280, 720);
            Controls.Add(splitter1);
            Controls.Add(panel1);
            EnableHitTest = false;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BizLink Client";
            FormClosed += MainForm_FormClosed;
            Load += MainForm_Load;
            panel1.ResumeLayout(false);
            pageHeader1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            splitter1.Panel1.ResumeLayout(false);
            splitter1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitter1).EndInit();
            splitter1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private AntdUI.Tabs tabs1;
        private AntdUI.Panel panel1;
        private AntdUI.Splitter splitter1;
        private AntdUI.Menu mainMenu;
        private AntdUI.Tabs mainTabs;
        private AntdUI.Breadcrumb breadcrumb;
        private AntdUI.PageHeader pageHeader1;
        private AntdUI.Dropdown dropDownFactory;
        private AntdUI.Panel panel2;
        private AntdUI.LabelTime labelTime1;
        private AntdUI.Label labelUserInfo;
    }
}