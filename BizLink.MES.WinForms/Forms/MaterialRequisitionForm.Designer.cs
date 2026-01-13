namespace BizLink.MES.WinForms.Forms
{
    partial class MaterialRequisitionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialRequisitionForm));
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new AntdUI.Panel();
            backPicture = new PictureBox();
            label2 = new AntdUI.Label();
            dispatchLabel = new AntdUI.Label();
            divider1 = new AntdUI.Divider();
            panel2 = new AntdUI.Panel();
            pushButton = new AntdUI.Button();
            exportButton = new AntdUI.Button();
            queryButton = new AntdUI.Button();
            consumetypeSelect = new AntdUI.Select();
            panel3 = new AntdUI.Panel();
            SpinControl = new AntdUI.Spin();
            pushProgress = new AntdUI.Progress();
            materialGroupTable = new AntdUI.Table();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)backPicture).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.White;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(divider1, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 0, 2);
            tableLayoutPanel1.Controls.Add(panel3, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(777, 593);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(backPicture);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(dispatchLabel);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(771, 64);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // backPicture
            // 
            backPicture.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            backPicture.Image = (Image)resources.GetObject("backPicture.Image");
            backPicture.Location = new Point(731, 0);
            backPicture.Name = "backPicture";
            backPicture.Size = new Size(40, 40);
            backPicture.SizeMode = PictureBoxSizeMode.StretchImage;
            backPicture.TabIndex = 3;
            backPicture.TabStop = false;
            backPicture.Click += backPicture_Click;
            // 
            // label2
            // 
            label2.Font = new Font("Microsoft YaHei UI", 10F);
            label2.ForeColor = SystemColors.ActiveBorder;
            label2.Location = new Point(5, 41);
            label2.Name = "label2";
            label2.Size = new Size(247, 23);
            label2.TabIndex = 1;
            label2.Text = "汇总指定日期的所有生产订单物料需求。";
            // 
            // dispatchLabel
            // 
            dispatchLabel.Font = new Font("Microsoft YaHei UI", 18F, FontStyle.Bold);
            dispatchLabel.Location = new Point(5, 0);
            dispatchLabel.Name = "dispatchLabel";
            dispatchLabel.Size = new Size(277, 39);
            dispatchLabel.TabIndex = 0;
            dispatchLabel.Text = "计划领料清单";
            // 
            // divider1
            // 
            divider1.BackColor = Color.White;
            divider1.ColorSplit = SystemColors.ControlDark;
            divider1.Dock = DockStyle.Fill;
            divider1.ForeColor = SystemColors.Control;
            divider1.Location = new Point(3, 73);
            divider1.Name = "divider1";
            divider1.Size = new Size(771, 4);
            divider1.TabIndex = 1;
            divider1.Text = "";
            divider1.Thickness = 0.4F;
            // 
            // panel2
            // 
            panel2.Controls.Add(pushButton);
            panel2.Controls.Add(exportButton);
            panel2.Controls.Add(queryButton);
            panel2.Controls.Add(consumetypeSelect);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 83);
            panel2.Name = "panel2";
            panel2.Size = new Size(771, 39);
            panel2.TabIndex = 3;
            panel2.Text = "panel2";
            // 
            // pushButton
            // 
            pushButton.Anchor = AnchorStyles.Right;
            pushButton.Font = new Font("Microsoft YaHei UI", 11F);
            pushButton.Location = new Point(676, -2);
            pushButton.Name = "pushButton";
            pushButton.Size = new Size(92, 42);
            pushButton.TabIndex = 2;
            pushButton.Text = "推送";
            pushButton.Type = AntdUI.TTypeMini.Primary;
            pushButton.Click += pushButton_Click;
            // 
            // exportButton
            // 
            exportButton.Anchor = AnchorStyles.Right;
            exportButton.BackColor = Color.LimeGreen;
            exportButton.Font = new Font("Microsoft YaHei UI", 11F);
            exportButton.Location = new Point(578, -2);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(92, 42);
            exportButton.TabIndex = 3;
            exportButton.Text = "导出";
            exportButton.Type = AntdUI.TTypeMini.Primary;
            exportButton.Click += exportButton_Click;
            // 
            // queryButton
            // 
            queryButton.Anchor = AnchorStyles.Right;
            queryButton.BackColor = SystemColors.ControlText;
            queryButton.Font = new Font("Microsoft YaHei UI", 11F);
            queryButton.Location = new Point(131, -2);
            queryButton.Name = "queryButton";
            queryButton.Size = new Size(92, 42);
            queryButton.TabIndex = 4;
            queryButton.Text = "查询";
            queryButton.Type = AntdUI.TTypeMini.Primary;
            queryButton.Visible = false;
            queryButton.Click += queryButton_Click;
            // 
            // consumetypeSelect
            // 
            consumetypeSelect.Font = new Font("Microsoft YaHei UI", 10F);
            consumetypeSelect.Location = new Point(0, 0);
            consumetypeSelect.Name = "consumetypeSelect";
            consumetypeSelect.SelectionStart = 2;
            consumetypeSelect.Size = new Size(125, 40);
            consumetypeSelect.TabIndex = 2;
            consumetypeSelect.Text = "断线";
            consumetypeSelect.SelectedIndexChanged += consumetypeSelect_SelectedIndexChanged;
            // 
            // panel3
            // 
            panel3.Controls.Add(SpinControl);
            panel3.Controls.Add(pushProgress);
            panel3.Controls.Add(materialGroupTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 128);
            panel3.Name = "panel3";
            panel3.Size = new Size(771, 462);
            panel3.TabIndex = 4;
            panel3.Text = "panel3";
            // 
            // SpinControl
            // 
            SpinControl.Anchor = AnchorStyles.None;
            SpinControl.Location = new Point(350, 195);
            SpinControl.Name = "SpinControl";
            SpinControl.Size = new Size(85, 64);
            SpinControl.TabIndex = 5;
            SpinControl.Text = "加载中...";
            SpinControl.Visible = false;
            // 
            // pushProgress
            // 
            pushProgress.Anchor = AnchorStyles.None;
            pushProgress.BackColor = Color.Transparent;
            pushProgress.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 2);
            pushProgress.IconCircleAngle = true;
            pushProgress.Loading = true;
            pushProgress.LoadingFull = true;
            pushProgress.Location = new Point(350, 195);
            pushProgress.Name = "pushProgress";
            pushProgress.Radius = 5;
            pushProgress.Shape = AntdUI.TShapeProgress.Circle;
            pushProgress.Size = new Size(75, 64);
            pushProgress.TabIndex = 1;
            pushProgress.Text = "progress1";
            pushProgress.UseTextCenter = true;
            pushProgress.ValueRatio = 0.5F;
            pushProgress.Visible = false;
            // 
            // materialGroupTable
            // 
            materialGroupTable.Dock = DockStyle.Fill;
            materialGroupTable.Font = new Font("Microsoft YaHei UI", 11F);
            materialGroupTable.Gap = 12;
            materialGroupTable.Location = new Point(0, 0);
            materialGroupTable.Name = "materialGroupTable";
            materialGroupTable.Size = new Size(771, 462);
            materialGroupTable.TabIndex = 0;
            materialGroupTable.Text = "table1";
            // 
            // MaterialRequisitionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            ClientSize = new Size(777, 593);
            Controls.Add(tableLayoutPanel1);
            Name = "MaterialRequisitionForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "MaterialRequisitionForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)backPicture).EndInit();
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Label dispatchLabel;
        private AntdUI.Button exportButton;
        private AntdUI.Button pushButton;
        private AntdUI.Label label2;
        private AntdUI.Divider divider1;
        private AntdUI.Panel panel2;
        private AntdUI.Select consumetypeSelect;
        private AntdUI.Button queryButton;
        private AntdUI.Panel panel3;
        private AntdUI.Table materialGroupTable;
        private PictureBox backPicture;
        private AntdUI.Progress pushProgress;
        private AntdUI.Spin SpinControl;
    }
}