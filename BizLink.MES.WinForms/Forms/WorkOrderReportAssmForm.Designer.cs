namespace BizLink.MES.WinForms.Forms
{
    partial class WorkOrderReportAssmForm
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
            AntdUI.StepsItem stepsItem5 = new AntdUI.StepsItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WorkOrderReportAssmForm));
            AntdUI.StepsItem stepsItem6 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem7 = new AntdUI.StepsItem();
            AntdUI.StepsItem stepsItem8 = new AntdUI.StepsItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new AntdUI.Panel();
            currentWorkCenterSelect = new AntdUI.Select();
            endDatePicker = new AntdUI.DatePicker();
            workstationSelect = new AntdUI.SelectMultiple();
            stationConfirmButtom = new AntdUI.Button();
            workcenterSelect = new AntdUI.Select();
            startDatePicker = new AntdUI.DatePicker();
            splitter1 = new AntdUI.Splitter();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel2 = new AntdUI.Panel();
            orderInput = new AntdUI.Input();
            panel3 = new AntdUI.Panel();
            orderSpin = new AntdUI.Spin();
            orderTable = new AntdUI.Table();
            tableLayoutPanel3 = new TableLayoutPanel();
            panel4 = new AntdUI.Panel();
            steps1 = new AntdUI.Steps();
            panel5 = new AntdUI.Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            panel7 = new AntdUI.Panel();
            saveButton = new AntdUI.Button();
            input1 = new AntdUI.Input();
            label17 = new AntdUI.Label();
            panel8 = new AntdUI.Panel();
            executeButton = new AntdUI.Button();
            suspendButton = new AntdUI.Button();
            panel6 = new AntdUI.Panel();
            pickingTable = new AntdUI.Table();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitter1).BeginInit();
            splitter1.Panel1.SuspendLayout();
            splitter1.Panel2.SuspendLayout();
            splitter1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel7.SuspendLayout();
            panel8.SuspendLayout();
            panel6.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(splitter1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 90F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(971, 605);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(currentWorkCenterSelect);
            panel1.Controls.Add(endDatePicker);
            panel1.Controls.Add(workstationSelect);
            panel1.Controls.Add(stationConfirmButtom);
            panel1.Controls.Add(workcenterSelect);
            panel1.Controls.Add(startDatePicker);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(965, 84);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // currentWorkCenterSelect
            // 
            currentWorkCenterSelect.AllowClear = true;
            currentWorkCenterSelect.CloseIcon = true;
            currentWorkCenterSelect.Font = new Font("Microsoft YaHei UI", 11F);
            currentWorkCenterSelect.ListAutoWidth = true;
            currentWorkCenterSelect.Location = new Point(581, 3);
            currentWorkCenterSelect.Name = "currentWorkCenterSelect";
            currentWorkCenterSelect.Size = new Size(270, 40);
            currentWorkCenterSelect.TabIndex = 21;
            // 
            // endDatePicker
            // 
            endDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            endDatePicker.Location = new Point(154, 3);
            endDatePicker.Name = "endDatePicker";
            endDatePicker.Size = new Size(145, 40);
            endDatePicker.TabIndex = 20;
            // 
            // workstationSelect
            // 
            workstationSelect.Font = new Font("Microsoft YaHei UI", 11F);
            workstationSelect.Location = new Point(3, 44);
            workstationSelect.Name = "workstationSelect";
            workstationSelect.Size = new Size(752, 40);
            workstationSelect.TabIndex = 19;
            // 
            // stationConfirmButtom
            // 
            stationConfirmButtom.Font = new Font("Microsoft YaHei UI", 10F);
            stationConfirmButtom.Location = new Point(761, 44);
            stationConfirmButtom.Name = "stationConfirmButtom";
            stationConfirmButtom.Size = new Size(90, 40);
            stationConfirmButtom.TabIndex = 16;
            stationConfirmButtom.Text = "工位确认";
            stationConfirmButtom.Type = AntdUI.TTypeMini.Primary;
            stationConfirmButtom.Click += stationConfirmButtom_Click;
            // 
            // workcenterSelect
            // 
            workcenterSelect.AllowClear = true;
            workcenterSelect.CloseIcon = true;
            workcenterSelect.Font = new Font("Microsoft YaHei UI", 11F);
            workcenterSelect.ListAutoWidth = true;
            workcenterSelect.Location = new Point(305, 3);
            workcenterSelect.Name = "workcenterSelect";
            workcenterSelect.Size = new Size(270, 40);
            workcenterSelect.TabIndex = 11;
            workcenterSelect.SelectedValueChanged += workcenterSelect_SelectedValueChanged;
            // 
            // startDatePicker
            // 
            startDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            startDatePicker.Location = new Point(3, 3);
            startDatePicker.Name = "startDatePicker";
            startDatePicker.Size = new Size(145, 40);
            startDatePicker.TabIndex = 10;
            // 
            // splitter1
            // 
            splitter1.Dock = DockStyle.Fill;
            splitter1.Location = new Point(3, 93);
            splitter1.Name = "splitter1";
            splitter1.Orientation = Orientation.Horizontal;
            // 
            // splitter1.Panel1
            // 
            splitter1.Panel1.Controls.Add(tableLayoutPanel2);
            // 
            // splitter1.Panel2
            // 
            splitter1.Panel2.Controls.Add(tableLayoutPanel3);
            splitter1.Size = new Size(965, 509);
            splitter1.SplitterDistance = 280;
            splitter1.SplitterWidth = 6;
            splitter1.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel2, 0, 0);
            tableLayoutPanel2.Controls.Add(panel3, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(965, 280);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(orderInput);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(959, 44);
            panel2.TabIndex = 0;
            panel2.Text = "panel2";
            // 
            // orderInput
            // 
            orderInput.Dock = DockStyle.Fill;
            orderInput.Font = new Font("Microsoft YaHei UI", 12F);
            orderInput.Location = new Point(0, 0);
            orderInput.Name = "orderInput";
            orderInput.Size = new Size(959, 44);
            orderInput.TabIndex = 0;
            orderInput.KeyPress += orderInput_KeyPress;
            // 
            // panel3
            // 
            panel3.Controls.Add(orderSpin);
            panel3.Controls.Add(orderTable);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 53);
            panel3.Name = "panel3";
            panel3.Size = new Size(959, 224);
            panel3.TabIndex = 1;
            panel3.Text = "panel3";
            // 
            // orderSpin
            // 
            orderSpin.Anchor = AnchorStyles.None;
            orderSpin.BackColor = Color.White;
            orderSpin.Location = new Point(450, 75);
            orderSpin.Name = "orderSpin";
            orderSpin.Size = new Size(75, 60);
            orderSpin.TabIndex = 1;
            orderSpin.Text = "加载中...";
            orderSpin.Visible = false;
            // 
            // orderTable
            // 
            orderTable.BackColor = Color.White;
            orderTable.Dock = DockStyle.Fill;
            orderTable.Font = new Font("Microsoft YaHei UI", 11F);
            orderTable.Gap = 12;
            orderTable.Location = new Point(0, 0);
            orderTable.Name = "orderTable";
            orderTable.Size = new Size(959, 224);
            orderTable.TabIndex = 0;
            orderTable.Text = "table1";
            orderTable.CellClick += orderTable_CellClick;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.Controls.Add(panel4, 0, 0);
            tableLayoutPanel3.Controls.Add(panel5, 2, 0);
            tableLayoutPanel3.Controls.Add(panel6, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(965, 223);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // panel4
            // 
            panel4.Controls.Add(steps1);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(194, 217);
            panel4.TabIndex = 0;
            panel4.Text = "panel4";
            // 
            // steps1
            // 
            steps1.BackColor = Color.White;
            steps1.Current = 2;
            steps1.Dock = DockStyle.Right;
            steps1.Font = new Font("Microsoft YaHei UI", 11F);
            stepsItem5.Description = "断线";
            stepsItem5.Icon = (Image)resources.GetObject("stepsItem5.Icon");
            stepsItem5.Name = "Cut";
            stepsItem5.Title = "Finish";
            stepsItem6.Description = "原材料拣配";
            stepsItem6.Icon = (Image)resources.GetObject("stepsItem6.Icon");
            stepsItem6.Name = "Pick";
            stepsItem6.Title = "Finish";
            stepsItem7.Description = "装配";
            stepsItem7.Icon = (Image)resources.GetObject("stepsItem7.Icon");
            stepsItem7.Name = "Assembly";
            stepsItem7.Title = "In Process";
            stepsItem8.Description = "终检";
            stepsItem8.Icon = (Image)resources.GetObject("stepsItem8.Icon");
            stepsItem8.Name = "Quality";
            stepsItem8.Title = "Waiting";
            steps1.Items.Add(stepsItem5);
            steps1.Items.Add(stepsItem6);
            steps1.Items.Add(stepsItem7);
            steps1.Items.Add(stepsItem8);
            steps1.Location = new Point(15, 0);
            steps1.Name = "steps1";
            steps1.Size = new Size(179, 217);
            steps1.TabIndex = 0;
            steps1.Vertical = true;
            // 
            // panel5
            // 
            panel5.Controls.Add(tableLayoutPanel4);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(815, 3);
            panel5.Name = "panel5";
            panel5.Size = new Size(147, 217);
            panel5.TabIndex = 1;
            panel5.Text = "panel5";
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(panel7, 0, 0);
            tableLayoutPanel4.Controls.Add(panel8, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel4.Size = new Size(147, 217);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // panel7
            // 
            panel7.Controls.Add(saveButton);
            panel7.Controls.Add(input1);
            panel7.Controls.Add(label17);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(3, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(141, 111);
            panel7.TabIndex = 0;
            panel7.Text = "panel7";
            // 
            // saveButton
            // 
            saveButton.Dock = DockStyle.Bottom;
            saveButton.Font = new Font("Microsoft YaHei UI", 12F);
            saveButton.Location = new Point(0, 66);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(141, 45);
            saveButton.TabIndex = 7;
            saveButton.Text = "保存";
            saveButton.Type = AntdUI.TTypeMini.Primary;
            // 
            // input1
            // 
            input1.Dock = DockStyle.Top;
            input1.Location = new Point(0, 23);
            input1.Name = "input1";
            input1.Size = new Size(141, 40);
            input1.TabIndex = 6;
            // 
            // label17
            // 
            label17.BackColor = Color.White;
            label17.Dock = DockStyle.Top;
            label17.Font = new Font("Microsoft YaHei UI", 11F);
            label17.Location = new Point(0, 0);
            label17.Name = "label17";
            label17.Size = new Size(141, 23);
            label17.TabIndex = 5;
            label17.Text = "生产备注：";
            // 
            // panel8
            // 
            panel8.Controls.Add(executeButton);
            panel8.Controls.Add(suspendButton);
            panel8.Dock = DockStyle.Fill;
            panel8.Location = new Point(3, 120);
            panel8.Name = "panel8";
            panel8.Size = new Size(141, 94);
            panel8.TabIndex = 1;
            panel8.Text = "panel8";
            // 
            // executeButton
            // 
            executeButton.Dock = DockStyle.Bottom;
            executeButton.Font = new Font("Microsoft YaHei UI", 12F);
            executeButton.Location = new Point(0, 49);
            executeButton.Name = "executeButton";
            executeButton.Size = new Size(141, 45);
            executeButton.TabIndex = 2;
            executeButton.Text = "进入执行";
            executeButton.Type = AntdUI.TTypeMini.Primary;
            executeButton.Click += executeButton_Click;
            // 
            // suspendButton
            // 
            suspendButton.Font = new Font("Microsoft YaHei UI", 12F);
            suspendButton.Location = new Point(0, 3);
            suspendButton.Name = "suspendButton";
            suspendButton.Size = new Size(141, 45);
            suspendButton.TabIndex = 3;
            suspendButton.Text = "挂起";
            suspendButton.Type = AntdUI.TTypeMini.Error;
            suspendButton.Visible = false;
            // 
            // panel6
            // 
            panel6.Controls.Add(pickingTable);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(203, 3);
            panel6.Name = "panel6";
            panel6.Size = new Size(606, 217);
            panel6.TabIndex = 2;
            panel6.Text = "panel6";
            // 
            // pickingTable
            // 
            pickingTable.BackColor = Color.White;
            pickingTable.Dock = DockStyle.Fill;
            pickingTable.Font = new Font("Microsoft YaHei UI", 10F);
            pickingTable.Gap = 12;
            pickingTable.Location = new Point(0, 0);
            pickingTable.Name = "pickingTable";
            pickingTable.Size = new Size(606, 217);
            pickingTable.TabIndex = 0;
            pickingTable.Text = "table2";
            // 
            // WorkOrderReportAssmForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(971, 605);
            Controls.Add(tableLayoutPanel1);
            Name = "WorkOrderReportAssmForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "WorkOrderReportAssmForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            splitter1.Panel1.ResumeLayout(false);
            splitter1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitter1).EndInit();
            splitter1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Button stationConfirmButtom;
        private AntdUI.Select workcenterSelect;
        private AntdUI.DatePicker startDatePicker;
        private TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Panel panel2;
        private AntdUI.Input orderInput;
        private AntdUI.Panel panel3;
        private AntdUI.Table orderTable;
        private TableLayoutPanel tableLayoutPanel3;
        private AntdUI.Panel panel4;
        private AntdUI.Steps steps1;
        private AntdUI.Panel panel5;
        private AntdUI.Panel panel6;
        private TableLayoutPanel tableLayoutPanel4;
        private AntdUI.Panel panel7;
        private AntdUI.Panel panel8;
        private AntdUI.Button saveButton;
        private AntdUI.Input input1;
        private AntdUI.Label label17;
        private AntdUI.Button executeButton;
        private AntdUI.Button suspendButton;
        private AntdUI.Table pickingTable;
        private AntdUI.Spin orderSpin;
        private AntdUI.SelectMultiple workstationSelect;
        private AntdUI.DatePicker endDatePicker;
        private AntdUI.Splitter splitter1;
        private AntdUI.Select currentWorkCenterSelect;
    }
}