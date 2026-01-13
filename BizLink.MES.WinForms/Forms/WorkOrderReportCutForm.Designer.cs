namespace BizLink.MES.WinForms.Forms
{
    partial class WorkOrderReportCutForm
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
            stationConfirmButtom = new AntdUI.Button();
            select3 = new AntdUI.Select();
            label5 = new AntdUI.Label();
            workstationSelect = new AntdUI.Select();
            workcenterSelect = new AntdUI.Select();
            startDatePicker = new AntdUI.DatePicker();
            panel4 = new AntdUI.Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel3 = new AntdUI.Panel();
            panel10 = new AntdUI.Panel();
            orderSpin = new AntdUI.Spin();
            orderTable = new AntdUI.Table();
            panel7 = new AntdUI.Panel();
            orderInput = new AntdUI.Input();
            tableLayoutPanel3 = new TableLayoutPanel();
            panel5 = new AntdUI.Panel();
            detailSpin = new AntdUI.Spin();
            tableLayoutPanel4 = new TableLayoutPanel();
            planRemarkLabel = new AntdUI.Label();
            cableDslLabel = new AntdUI.Label();
            cableUslLabel = new AntdUI.Label();
            cableLengthLabel = new AntdUI.Label();
            startTimeLabel = new AntdUI.Label();
            orderLabel = new AntdUI.Label();
            cableCodeLabel = new AntdUI.Label();
            productCodeLabel = new AntdUI.Label();
            productDescLabel = new AntdUI.Label();
            planQtyLabel = new AntdUI.Label();
            dispatchDateLabel = new AntdUI.Label();
            completeQtyLabel = new AntdUI.Label();
            panel6 = new AntdUI.Panel();
            tableLayoutPanel5 = new TableLayoutPanel();
            panel8 = new AntdUI.Panel();
            executeButton = new AntdUI.Button();
            suspendButton = new AntdUI.Button();
            panel9 = new AntdUI.Panel();
            saveButton = new AntdUI.Button();
            RemarkInput = new AntdUI.Input();
            label17 = new AntdUI.Label();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel3.SuspendLayout();
            panel10.SuspendLayout();
            panel7.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel5.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel6.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            panel8.SuspendLayout();
            panel9.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(panel4, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 250F));
            tableLayoutPanel1.Size = new Size(972, 590);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(stationConfirmButtom);
            panel1.Controls.Add(select3);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(workstationSelect);
            panel1.Controls.Add(workcenterSelect);
            panel1.Controls.Add(startDatePicker);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(966, 44);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // stationConfirmButtom
            // 
            stationConfirmButtom.Font = new Font("Microsoft YaHei UI", 10F);
            stationConfirmButtom.Location = new Point(602, 4);
            stationConfirmButtom.Name = "stationConfirmButtom";
            stationConfirmButtom.Size = new Size(94, 40);
            stationConfirmButtom.TabIndex = 9;
            stationConfirmButtom.Text = "工位确认";
            stationConfirmButtom.Type = AntdUI.TTypeMini.Primary;
            stationConfirmButtom.Click += stationConfirmButtom_Click;
            // 
            // select3
            // 
            select3.Anchor = AnchorStyles.Right;
            select3.Location = new Point(840, 4);
            select3.Name = "select3";
            select3.Size = new Size(120, 40);
            select3.TabIndex = 8;
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Right;
            label5.BackColor = Color.White;
            label5.Font = new Font("Microsoft YaHei UI", 11F);
            label5.Location = new Point(775, 12);
            label5.Name = "label5";
            label5.Size = new Size(59, 23);
            label5.TabIndex = 7;
            label5.Text = "当前班次";
            // 
            // workstationSelect
            // 
            workstationSelect.Font = new Font("Microsoft YaHei UI", 11F);
            workstationSelect.Location = new Point(376, 4);
            workstationSelect.Name = "workstationSelect";
            workstationSelect.Size = new Size(220, 40);
            workstationSelect.TabIndex = 2;
            // 
            // workcenterSelect
            // 
            workcenterSelect.AllowClear = true;
            workcenterSelect.CloseIcon = true;
            workcenterSelect.Font = new Font("Microsoft YaHei UI", 11F);
            workcenterSelect.ListAutoWidth = true;
            workcenterSelect.Location = new Point(150, 4);
            workcenterSelect.Name = "workcenterSelect";
            workcenterSelect.Size = new Size(220, 40);
            workcenterSelect.TabIndex = 1;
            workcenterSelect.SelectedValueChanged += workcenterSelect_SelectedValueChanged;
            // 
            // startDatePicker
            // 
            startDatePicker.Font = new Font("Microsoft YaHei UI", 11F);
            startDatePicker.Location = new Point(6, 4);
            startDatePicker.Name = "startDatePicker";
            startDatePicker.Size = new Size(138, 40);
            startDatePicker.TabIndex = 0;
            startDatePicker.ValueChanged += startDatePicker_ValueChanged;
            // 
            // panel4
            // 
            panel4.Controls.Add(tableLayoutPanel2);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 53);
            panel4.Name = "panel4";
            panel4.Size = new Size(966, 284);
            panel4.TabIndex = 2;
            panel4.Text = "panel4";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel3, 0, 1);
            tableLayoutPanel2.Controls.Add(panel7, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(966, 284);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(panel10);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 50);
            panel3.Name = "panel3";
            panel3.Size = new Size(960, 231);
            panel3.TabIndex = 1;
            panel3.Text = "panel3";
            // 
            // panel10
            // 
            panel10.Controls.Add(orderSpin);
            panel10.Controls.Add(orderTable);
            panel10.Dock = DockStyle.Fill;
            panel10.Location = new Point(0, 0);
            panel10.Name = "panel10";
            panel10.Size = new Size(960, 231);
            panel10.TabIndex = 1;
            panel10.Text = "panel10";
            // 
            // orderSpin
            // 
            orderSpin.Anchor = AnchorStyles.None;
            orderSpin.BackColor = Color.White;
            orderSpin.Location = new Point(447, 75);
            orderSpin.Name = "orderSpin";
            orderSpin.Size = new Size(75, 69);
            orderSpin.TabIndex = 1;
            orderSpin.Text = "加载中...";
            orderSpin.Visible = false;
            // 
            // orderTable
            // 
            orderTable.BackColor = Color.White;
            orderTable.ClipboardCopy = false;
            orderTable.Dock = DockStyle.Fill;
            orderTable.EditMode = AntdUI.TEditMode.Click;
            orderTable.Font = new Font("Microsoft YaHei UI", 11F);
            orderTable.Gap = 12;
            orderTable.Location = new Point(0, 0);
            orderTable.Name = "orderTable";
            orderTable.Size = new Size(960, 231);
            orderTable.TabIndex = 0;
            orderTable.Text = "table1";
            orderTable.CellClick += orderTable_CellClick;
            orderTable.CellButtonClick += orderTable_CellButtonClick;
            // 
            // panel7
            // 
            panel7.Controls.Add(orderInput);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(3, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(960, 41);
            panel7.TabIndex = 2;
            panel7.Text = "panel7";
            // 
            // orderInput
            // 
            orderInput.Dock = DockStyle.Fill;
            orderInput.Font = new Font("Microsoft YaHei UI", 11F);
            orderInput.Location = new Point(0, 0);
            orderInput.Name = "orderInput";
            orderInput.Size = new Size(960, 41);
            orderInput.TabIndex = 0;
            orderInput.KeyPress += orderInput_KeyPress;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 5;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.Controls.Add(panel5, 0, 0);
            tableLayoutPanel3.Controls.Add(panel6, 4, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 343);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(966, 244);
            tableLayoutPanel3.TabIndex = 3;
            // 
            // panel5
            // 
            panel5.BackColor = Color.White;
            tableLayoutPanel3.SetColumnSpan(panel5, 4);
            panel5.Controls.Add(detailSpin);
            panel5.Controls.Add(tableLayoutPanel4);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(3, 3);
            panel5.Name = "panel5";
            panel5.Size = new Size(766, 238);
            panel5.TabIndex = 0;
            panel5.Text = "panel5";
            // 
            // detailSpin
            // 
            detailSpin.Anchor = AnchorStyles.None;
            detailSpin.Location = new Point(344, 90);
            detailSpin.Name = "detailSpin";
            detailSpin.Size = new Size(75, 62);
            detailSpin.TabIndex = 8;
            detailSpin.Text = "加载中...";
            detailSpin.Visible = false;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.BackColor = Color.White;
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(planRemarkLabel, 0, 6);
            tableLayoutPanel4.Controls.Add(cableDslLabel, 1, 5);
            tableLayoutPanel4.Controls.Add(cableUslLabel, 0, 5);
            tableLayoutPanel4.Controls.Add(cableLengthLabel, 1, 4);
            tableLayoutPanel4.Controls.Add(startTimeLabel, 0, 2);
            tableLayoutPanel4.Controls.Add(orderLabel, 0, 0);
            tableLayoutPanel4.Controls.Add(cableCodeLabel, 0, 4);
            tableLayoutPanel4.Controls.Add(productCodeLabel, 0, 1);
            tableLayoutPanel4.Controls.Add(productDescLabel, 1, 1);
            tableLayoutPanel4.Controls.Add(planQtyLabel, 0, 3);
            tableLayoutPanel4.Controls.Add(dispatchDateLabel, 1, 2);
            tableLayoutPanel4.Controls.Add(completeQtyLabel, 1, 3);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Font = new Font("Microsoft YaHei UI", 10F);
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 7;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 17.9431877F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 13.6761332F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 13.676136F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 13.676136F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 13.676136F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 13.676136F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 13.6761379F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(766, 238);
            tableLayoutPanel4.TabIndex = 7;
            // 
            // planRemarkLabel
            // 
            planRemarkLabel.Dock = DockStyle.Fill;
            planRemarkLabel.Font = new Font("Microsoft YaHei UI", 11F);
            planRemarkLabel.Location = new Point(3, 205);
            planRemarkLabel.Name = "planRemarkLabel";
            planRemarkLabel.Size = new Size(377, 30);
            planRemarkLabel.TabIndex = 17;
            planRemarkLabel.Text = "  订单备注：";
            // 
            // cableDslLabel
            // 
            cableDslLabel.Dock = DockStyle.Fill;
            cableDslLabel.Font = new Font("Microsoft YaHei UI", 11F);
            cableDslLabel.ForeColor = Color.Red;
            cableDslLabel.Location = new Point(386, 173);
            cableDslLabel.Name = "cableDslLabel";
            cableDslLabel.Size = new Size(377, 26);
            cableDslLabel.TabIndex = 16;
            cableDslLabel.Text = "  最小断长：";
            // 
            // cableUslLabel
            // 
            cableUslLabel.Dock = DockStyle.Fill;
            cableUslLabel.Font = new Font("Microsoft YaHei UI", 11F);
            cableUslLabel.ForeColor = Color.Red;
            cableUslLabel.Location = new Point(3, 173);
            cableUslLabel.Name = "cableUslLabel";
            cableUslLabel.Size = new Size(377, 26);
            cableUslLabel.TabIndex = 15;
            cableUslLabel.Text = "  最大断长：";
            // 
            // cableLengthLabel
            // 
            cableLengthLabel.Dock = DockStyle.Fill;
            cableLengthLabel.Font = new Font("Microsoft YaHei UI", 11F);
            cableLengthLabel.Location = new Point(386, 141);
            cableLengthLabel.Name = "cableLengthLabel";
            cableLengthLabel.Size = new Size(377, 26);
            cableLengthLabel.TabIndex = 14;
            cableLengthLabel.Text = "  断线长度：";
            // 
            // startTimeLabel
            // 
            startTimeLabel.Dock = DockStyle.Fill;
            startTimeLabel.Font = new Font("Microsoft YaHei UI", 11F);
            startTimeLabel.Location = new Point(3, 77);
            startTimeLabel.Name = "startTimeLabel";
            startTimeLabel.Size = new Size(377, 26);
            startTimeLabel.TabIndex = 13;
            startTimeLabel.Text = "  开工日期：";
            // 
            // orderLabel
            // 
            orderLabel.BackColor = Color.White;
            tableLayoutPanel4.SetColumnSpan(orderLabel, 2);
            orderLabel.Dock = DockStyle.Fill;
            orderLabel.Font = new Font("Microsoft YaHei UI", 13F, FontStyle.Bold);
            orderLabel.Location = new Point(3, 3);
            orderLabel.Name = "orderLabel";
            orderLabel.Size = new Size(760, 36);
            orderLabel.TabIndex = 0;
            orderLabel.Text = " 订单概览：";
            // 
            // cableCodeLabel
            // 
            cableCodeLabel.Dock = DockStyle.Fill;
            cableCodeLabel.Font = new Font("Microsoft YaHei UI", 11F);
            cableCodeLabel.ForeColor = Color.SlateBlue;
            cableCodeLabel.Location = new Point(3, 141);
            cableCodeLabel.Name = "cableCodeLabel";
            cableCodeLabel.Size = new Size(377, 26);
            cableCodeLabel.TabIndex = 12;
            cableCodeLabel.Text = "  断线物料：";
            // 
            // productCodeLabel
            // 
            productCodeLabel.Dock = DockStyle.Fill;
            productCodeLabel.Font = new Font("Microsoft YaHei UI", 11F);
            productCodeLabel.Location = new Point(3, 45);
            productCodeLabel.Name = "productCodeLabel";
            productCodeLabel.Size = new Size(377, 26);
            productCodeLabel.TabIndex = 1;
            productCodeLabel.Text = "  成品物料：";
            // 
            // productDescLabel
            // 
            productDescLabel.Dock = DockStyle.Fill;
            productDescLabel.Font = new Font("Microsoft YaHei UI", 11F);
            productDescLabel.Location = new Point(386, 45);
            productDescLabel.Name = "productDescLabel";
            productDescLabel.Size = new Size(377, 26);
            productDescLabel.TabIndex = 2;
            productDescLabel.Text = "  物料描述：";
            // 
            // planQtyLabel
            // 
            planQtyLabel.Dock = DockStyle.Fill;
            planQtyLabel.Font = new Font("Microsoft YaHei UI", 11F);
            planQtyLabel.ForeColor = SystemColors.HotTrack;
            planQtyLabel.Location = new Point(3, 109);
            planQtyLabel.Name = "planQtyLabel";
            planQtyLabel.Size = new Size(377, 26);
            planQtyLabel.TabIndex = 3;
            planQtyLabel.Text = "  计划数量：";
            // 
            // dispatchDateLabel
            // 
            dispatchDateLabel.Dock = DockStyle.Fill;
            dispatchDateLabel.Font = new Font("Microsoft YaHei UI", 11F);
            dispatchDateLabel.Location = new Point(386, 77);
            dispatchDateLabel.Name = "dispatchDateLabel";
            dispatchDateLabel.Size = new Size(377, 26);
            dispatchDateLabel.TabIndex = 5;
            dispatchDateLabel.Text = "  排产日期：";
            // 
            // completeQtyLabel
            // 
            completeQtyLabel.Dock = DockStyle.Fill;
            completeQtyLabel.Font = new Font("Microsoft YaHei UI", 11F);
            completeQtyLabel.ForeColor = Color.MediumSeaGreen;
            completeQtyLabel.Location = new Point(386, 109);
            completeQtyLabel.Name = "completeQtyLabel";
            completeQtyLabel.Size = new Size(377, 26);
            completeQtyLabel.TabIndex = 4;
            completeQtyLabel.Text = "  完成数量：";
            // 
            // panel6
            // 
            panel6.Controls.Add(tableLayoutPanel5);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(775, 3);
            panel6.Name = "panel6";
            panel6.Size = new Size(188, 238);
            panel6.TabIndex = 1;
            panel6.Text = "panel6";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Controls.Add(panel8, 0, 1);
            tableLayoutPanel5.Controls.Add(panel9, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 100F));
            tableLayoutPanel5.Size = new Size(188, 238);
            tableLayoutPanel5.TabIndex = 2;
            // 
            // panel8
            // 
            panel8.Controls.Add(executeButton);
            panel8.Controls.Add(suspendButton);
            panel8.Dock = DockStyle.Fill;
            panel8.Location = new Point(3, 141);
            panel8.Name = "panel8";
            panel8.Size = new Size(182, 94);
            panel8.TabIndex = 1;
            panel8.Text = "panel8";
            // 
            // executeButton
            // 
            executeButton.Dock = DockStyle.Bottom;
            executeButton.Font = new Font("Microsoft YaHei UI", 12F);
            executeButton.Location = new Point(0, 49);
            executeButton.Name = "executeButton";
            executeButton.Size = new Size(182, 45);
            executeButton.TabIndex = 0;
            executeButton.Text = "进入执行";
            executeButton.Type = AntdUI.TTypeMini.Primary;
            executeButton.Click += executeButton_Click;
            // 
            // suspendButton
            // 
            suspendButton.Dock = DockStyle.Top;
            suspendButton.Font = new Font("Microsoft YaHei UI", 12F);
            suspendButton.Location = new Point(0, 0);
            suspendButton.Name = "suspendButton";
            suspendButton.Size = new Size(182, 45);
            suspendButton.TabIndex = 1;
            suspendButton.Text = "挂起";
            suspendButton.Type = AntdUI.TTypeMini.Error;
            suspendButton.Click += suspendButton_Click;
            // 
            // panel9
            // 
            panel9.Controls.Add(saveButton);
            panel9.Controls.Add(RemarkInput);
            panel9.Controls.Add(label17);
            panel9.Dock = DockStyle.Fill;
            panel9.Location = new Point(3, 3);
            panel9.Name = "panel9";
            panel9.Size = new Size(182, 132);
            panel9.TabIndex = 2;
            panel9.Text = "panel9";
            // 
            // saveButton
            // 
            saveButton.Dock = DockStyle.Bottom;
            saveButton.Font = new Font("Microsoft YaHei UI", 12F);
            saveButton.Location = new Point(0, 87);
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(182, 45);
            saveButton.TabIndex = 4;
            saveButton.Text = "保存";
            saveButton.Type = AntdUI.TTypeMini.Primary;
            // 
            // RemarkInput
            // 
            RemarkInput.Dock = DockStyle.Top;
            RemarkInput.Location = new Point(0, 23);
            RemarkInput.Name = "RemarkInput";
            RemarkInput.Size = new Size(182, 40);
            RemarkInput.TabIndex = 3;
            // 
            // label17
            // 
            label17.BackColor = Color.White;
            label17.Dock = DockStyle.Top;
            label17.Font = new Font("Microsoft YaHei UI", 11F);
            label17.Location = new Point(0, 0);
            label17.Name = "label17";
            label17.Size = new Size(182, 23);
            label17.TabIndex = 2;
            label17.Text = "生产备注：";
            // 
            // WorkOrderReportCutForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(972, 590);
            Controls.Add(tableLayoutPanel1);
            Name = "WorkOrderReportCutForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "WorkOrderReportForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel10.ResumeLayout(false);
            panel7.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel5.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            panel6.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel9.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Select workcenterSelect;
        private AntdUI.DatePicker startDatePicker;
        private AntdUI.Select workstationSelect;
        private TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Panel panel3;
        private AntdUI.Table orderTable;
        private AntdUI.Panel panel4;
        private AntdUI.Select select3;
        private AntdUI.Label label5;
        private TableLayoutPanel tableLayoutPanel3;
        private AntdUI.Panel panel5;
        private AntdUI.Label orderLabel;
        private AntdUI.Panel panel6;
        private AntdUI.Label productDescLabel;
        private AntdUI.Label productCodeLabel;
        private AntdUI.Label dispatchDateLabel;
        private AntdUI.Label completeQtyLabel;
        private AntdUI.Label planQtyLabel;
        private TableLayoutPanel tableLayoutPanel4;
        private AntdUI.Label cableCodeLabel;
        private AntdUI.Button suspendButton;
        private AntdUI.Button executeButton;
        private TableLayoutPanel tableLayoutPanel5;
        private AntdUI.Panel panel8;
        private AntdUI.Panel panel9;
        private AntdUI.Button saveButton;
        private AntdUI.Input RemarkInput;
        private AntdUI.Label label17;
        private AntdUI.Panel panel10;
        private AntdUI.Spin orderSpin;
        private AntdUI.Label startTimeLabel;
        private AntdUI.Spin detailSpin;
        private AntdUI.Label planRemarkLabel;
        private AntdUI.Label cableDslLabel;
        private AntdUI.Label cableUslLabel;
        private AntdUI.Label cableLengthLabel;
        private AntdUI.Button stationConfirmButtom;
        private AntdUI.Panel panel7;
        private AntdUI.Input orderInput;
    }
}