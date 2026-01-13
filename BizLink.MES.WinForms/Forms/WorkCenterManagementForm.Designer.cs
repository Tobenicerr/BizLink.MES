namespace BizLink.MES.WinForms.Forms
{
    partial class WorkCenterManagementForm
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
            button2 = new AntdUI.Button();
            button1 = new AntdUI.Button();
            select1 = new AntdUI.Select();
            label2 = new AntdUI.Label();
            input1 = new AntdUI.Input();
            label1 = new AntdUI.Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel2 = new AntdUI.Panel();
            addButton = new AntdUI.Button();
            panel3 = new AntdUI.Panel();
            workcenterTable = new AntdUI.Table();
            pagination1 = new AntdUI.Pagination();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Controls.Add(pagination1, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutPanel1.Size = new Size(800, 450);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(select1);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(input1);
            panel1.Controls.Add(label1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(794, 44);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // button2
            // 
            button2.BorderWidth = 0.5F;
            button2.Location = new Point(692, 3);
            button2.Name = "button2";
            button2.Size = new Size(75, 40);
            button2.TabIndex = 5;
            button2.Text = "重置";
            // 
            // button1
            // 
            button1.Location = new Point(611, 3);
            button1.Name = "button1";
            button1.Size = new Size(75, 40);
            button1.TabIndex = 4;
            button1.Text = "查询";
            button1.Type = AntdUI.TTypeMini.Primary;
            // 
            // select1
            // 
            select1.Location = new Point(314, 3);
            select1.Name = "select1";
            select1.Size = new Size(140, 40);
            select1.TabIndex = 3;
            // 
            // label2
            // 
            label2.Location = new Point(233, 10);
            label2.Name = "label2";
            label2.Size = new Size(75, 23);
            label2.TabIndex = 2;
            label2.Text = "工作中心组";
            // 
            // input1
            // 
            input1.Location = new Point(87, 3);
            input1.Name = "input1";
            input1.Size = new Size(140, 40);
            input1.TabIndex = 1;
            // 
            // label1
            // 
            label1.Location = new Point(6, 10);
            label1.Name = "label1";
            label1.Size = new Size(75, 23);
            label1.TabIndex = 0;
            label1.Text = "关键字搜索";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(panel2, 0, 0);
            tableLayoutPanel2.Controls.Add(panel3, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 53);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(794, 359);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(addButton);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(788, 34);
            panel2.TabIndex = 0;
            panel2.Text = "panel2";
            // 
            // addButton
            // 
            addButton.Location = new Point(686, -3);
            addButton.Name = "addButton";
            addButton.Size = new Size(75, 40);
            addButton.TabIndex = 5;
            addButton.Text = "新增";
            addButton.Type = AntdUI.TTypeMini.Primary;
            addButton.Click += addButton_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(workcenterTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 43);
            panel3.Name = "panel3";
            panel3.Size = new Size(788, 313);
            panel3.TabIndex = 1;
            panel3.Text = "panel3";
            // 
            // workcenterTable
            // 
            workcenterTable.BackColor = Color.Transparent;
            workcenterTable.Dock = DockStyle.Fill;
            workcenterTable.Gap = 12;
            workcenterTable.Location = new Point(0, 0);
            workcenterTable.Name = "workcenterTable";
            workcenterTable.Size = new Size(788, 313);
            workcenterTable.TabIndex = 0;
            workcenterTable.Text = "table1";
            // 
            // pagination1
            // 
            pagination1.Location = new Point(3, 418);
            pagination1.Name = "pagination1";
            pagination1.ShowSizeChanger = true;
            pagination1.Size = new Size(794, 29);
            pagination1.TabIndex = 2;
            pagination1.Text = "pagination1";
            // 
            // WorkCenterManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tableLayoutPanel1);
            Name = "WorkCenterManagementForm";
            Text = "WorkCenterManagementForm";
            Load += WorkCenterManagementForm_Load;
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Select select1;
        private AntdUI.Label label2;
        private AntdUI.Input input1;
        private AntdUI.Label label1;
        private AntdUI.Button button2;
        private AntdUI.Button button1;
        private TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Panel panel2;
        private AntdUI.Button addButton;
        private AntdUI.Pagination pagination1;
        private AntdUI.Panel panel3;
        private AntdUI.Table workcenterTable;
    }
}