namespace BizLink.MES.WinForms.Forms
{
    partial class CuttingTaskExecutionForm
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            AntdUI.Tabs.StyleLine styleLine2 = new AntdUI.Tabs.StyleLine();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new AntdUI.Panel();
            SearchButton = new AntdUI.Button();
            PrinterSelect = new AntdUI.Select();
            WorkStationSelect = new AntdUI.Select();
            WorkCenterSelect = new AntdUI.Select();
            StartDatePicker = new AntdUI.DatePicker();
            tabs2 = new AntdUI.Tabs();
            tabPage2 = new AntdUI.TabPage();
            BomTable = new AntdUI.Table();
            tabPage3 = new AntdUI.TabPage();
            ExecuteTable = new AntdUI.Table();
            panel3 = new AntdUI.Panel();
            SubmitButton = new AntdUI.Button();
            RemarkInput = new AntdUI.Input();
            label4 = new AntdUI.Label();
            ScrapReasonSelect = new AntdUI.Select();
            label3 = new AntdUI.Label();
            ScrapLenInputNumber = new AntdUI.InputNumber();
            label2 = new AntdUI.Label();
            ReportInputNumber = new AntdUI.InputNumber();
            label1 = new AntdUI.Label();
            TaskTable = new AntdUI.Table();
            panel6 = new AntdUI.Panel();
            TaskStatusTag = new AntdUI.Tag();
            CutLendLabel = new AntdUI.Label();
            CutLendtLabel = new AntdUI.Label();
            SuspendButton = new AntdUI.Button();
            WorkStartButton = new AntdUI.Button();
            CutLenuLabel = new AntdUI.Label();
            CutLenutLabel = new AntdUI.Label();
            CutLenLabel = new AntdUI.Label();
            CutLentLabel = new AntdUI.Label();
            ProgressLabel = new AntdUI.Label();
            MatdLabel = new AntdUI.Label();
            MatcLabel = new AntdUI.Label();
            OrderNoLabel = new AntdUI.Label();
            panel8 = new AntdUI.Panel();
            OrderScanInput = new AntdUI.Input();
            panel2 = new AntdUI.Panel();
            InspectLabelButton = new AntdUI.Button();
            ProcessCardPrintButton = new AntdUI.Button();
            AdjustButton = new AntdUI.Button();
            panel5 = new AntdUI.Panel();
            tabs1 = new AntdUI.Tabs();
            tabPage4 = new AntdUI.TabPage();
            AllLoadingTable = new AntdUI.Table();
            tabPage1 = new AntdUI.TabPage();
            tableLayoutPanel2 = new TableLayoutPanel();
            LoadingTable = new AntdUI.Table();
            BarCodeScanInput = new AntdUI.Input();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            tabs2.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            panel3.SuspendLayout();
            panel6.SuspendLayout();
            panel8.SuspendLayout();
            panel2.SuspendLayout();
            panel5.SuspendLayout();
            tabs1.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 0);
            tableLayoutPanel1.Controls.Add(tabs2, 1, 4);
            tableLayoutPanel1.Controls.Add(panel3, 3, 3);
            tableLayoutPanel1.Controls.Add(TaskTable, 0, 2);
            tableLayoutPanel1.Controls.Add(panel6, 1, 3);
            tableLayoutPanel1.Controls.Add(panel8, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 3, 1);
            tableLayoutPanel1.Controls.Add(panel5, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 160F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(942, 575);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            tableLayoutPanel1.SetColumnSpan(panel1, 4);
            panel1.Controls.Add(SearchButton);
            panel1.Controls.Add(PrinterSelect);
            panel1.Controls.Add(WorkStationSelect);
            panel1.Controls.Add(WorkCenterSelect);
            panel1.Controls.Add(StartDatePicker);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(936, 39);
            panel1.TabIndex = 0;
            panel1.Text = "panel1";
            // 
            // SearchButton
            // 
            SearchButton.Font = new Font("Microsoft YaHei UI", 10F);
            SearchButton.Location = new Point(797, -1);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(75, 42);
            SearchButton.TabIndex = 4;
            SearchButton.Text = "查询";
            SearchButton.Type = AntdUI.TTypeMini.Primary;
            SearchButton.Click += SearchButton_Click;
            // 
            // PrinterSelect
            // 
            PrinterSelect.Font = new Font("Microsoft YaHei UI", 10F);
            PrinterSelect.ListAutoWidth = true;
            PrinterSelect.Location = new Point(597, -1);
            PrinterSelect.Name = "PrinterSelect";
            PrinterSelect.Size = new Size(200, 42);
            PrinterSelect.TabIndex = 3;
            PrinterSelect.SelectedValueChanged += PrinterSelect_SelectedValueChanged;
            // 
            // WorkStationSelect
            // 
            WorkStationSelect.Font = new Font("Microsoft YaHei UI", 10F);
            WorkStationSelect.ListAutoWidth = true;
            WorkStationSelect.Location = new Point(398, -1);
            WorkStationSelect.Name = "WorkStationSelect";
            WorkStationSelect.Size = new Size(200, 42);
            WorkStationSelect.TabIndex = 2;
            WorkStationSelect.SelectedValueChanged += WorkStationSelect_SelectedValueChanged;
            // 
            // WorkCenterSelect
            // 
            WorkCenterSelect.Font = new Font("Microsoft YaHei UI", 10F);
            WorkCenterSelect.ListAutoWidth = true;
            WorkCenterSelect.Location = new Point(199, -1);
            WorkCenterSelect.Name = "WorkCenterSelect";
            WorkCenterSelect.Size = new Size(200, 42);
            WorkCenterSelect.TabIndex = 1;
            WorkCenterSelect.SelectedValueChanged += WorkCenterSelect_SelectedValueChanged;
            // 
            // StartDatePicker
            // 
            StartDatePicker.Font = new Font("Microsoft YaHei UI", 10F);
            StartDatePicker.Location = new Point(3, -1);
            StartDatePicker.Name = "StartDatePicker";
            StartDatePicker.Size = new Size(200, 42);
            StartDatePicker.TabIndex = 0;
            // 
            // tabs2
            // 
            tabs2.BackColor = Color.White;
            tableLayoutPanel1.SetColumnSpan(tabs2, 2);
            tabs2.Controls.Add(tabPage2);
            tabs2.Controls.Add(tabPage3);
            tabs2.Dock = DockStyle.Fill;
            tabs2.Location = new Point(334, 418);
            tabs2.Name = "tabs2";
            tabs2.Pages.Add(tabPage2);
            tabs2.Pages.Add(tabPage3);
            tabs2.Size = new Size(384, 154);
            tabs2.Style = styleLine1;
            tabs2.TabIndex = 3;
            tabs2.Text = "tabs2";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(BomTable);
            tabPage2.Dock = DockStyle.Fill;
            tabPage2.Location = new Point(0, 30);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(384, 124);
            tabPage2.TabIndex = 0;
            tabPage2.Text = "生产报工";
            // 
            // BomTable
            // 
            BomTable.Dock = DockStyle.Fill;
            BomTable.EditMode = AntdUI.TEditMode.DoubleClick;
            BomTable.Font = new Font("Microsoft YaHei UI", 10F);
            BomTable.Gap = 12;
            BomTable.Location = new Point(0, 0);
            BomTable.Name = "BomTable";
            BomTable.Size = new Size(384, 124);
            BomTable.TabIndex = 0;
            BomTable.Text = "table2";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(ExecuteTable);
            tabPage3.Dock = DockStyle.Fill;
            tabPage3.Location = new Point(0, 30);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(384, 124);
            tabPage3.TabIndex = 1;
            tabPage3.Text = "历史记录";
            // 
            // ExecuteTable
            // 
            ExecuteTable.Dock = DockStyle.Fill;
            ExecuteTable.Gap = 12;
            ExecuteTable.Location = new Point(0, 0);
            ExecuteTable.Name = "ExecuteTable";
            ExecuteTable.Size = new Size(384, 124);
            ExecuteTable.TabIndex = 0;
            ExecuteTable.Text = "table1";
            ExecuteTable.CellButtonClick += ExecuteTable_CellButtonClick;
            // 
            // panel3
            // 
            panel3.Controls.Add(SubmitButton);
            panel3.Controls.Add(RemarkInput);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(ScrapReasonSelect);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(ScrapLenInputNumber);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(ReportInputNumber);
            panel3.Controls.Add(label1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(724, 258);
            panel3.Name = "panel3";
            tableLayoutPanel1.SetRowSpan(panel3, 2);
            panel3.Size = new Size(215, 314);
            panel3.TabIndex = 5;
            panel3.Text = "panel3";
            // 
            // SubmitButton
            // 
            SubmitButton.Dock = DockStyle.Bottom;
            SubmitButton.Font = new Font("Microsoft YaHei UI", 10F);
            SubmitButton.Location = new Point(0, 269);
            SubmitButton.Name = "SubmitButton";
            SubmitButton.Size = new Size(215, 45);
            SubmitButton.TabIndex = 8;
            SubmitButton.Text = "报工提交";
            SubmitButton.Type = AntdUI.TTypeMini.Primary;
            SubmitButton.Click += SubmitButton_Click;
            // 
            // RemarkInput
            // 
            RemarkInput.Dock = DockStyle.Top;
            RemarkInput.Font = new Font("Microsoft YaHei UI", 10F);
            RemarkInput.Location = new Point(0, 227);
            RemarkInput.Name = "RemarkInput";
            RemarkInput.Size = new Size(215, 45);
            RemarkInput.TabIndex = 7;
            // 
            // label4
            // 
            label4.BackColor = Color.White;
            label4.Dock = DockStyle.Top;
            label4.Location = new Point(0, 204);
            label4.Name = "label4";
            label4.Size = new Size(215, 23);
            label4.TabIndex = 6;
            label4.Text = "生产备注";
            // 
            // ScrapReasonSelect
            // 
            ScrapReasonSelect.Dock = DockStyle.Top;
            ScrapReasonSelect.Font = new Font("Microsoft YaHei UI", 10F);
            ScrapReasonSelect.Location = new Point(0, 159);
            ScrapReasonSelect.Name = "ScrapReasonSelect";
            ScrapReasonSelect.ReadOnly = true;
            ScrapReasonSelect.Size = new Size(215, 45);
            ScrapReasonSelect.TabIndex = 5;
            ScrapReasonSelect.DoubleClick += ScrapReasonSelect_DoubleClick;
            // 
            // label3
            // 
            label3.BackColor = Color.White;
            label3.Dock = DockStyle.Top;
            label3.Location = new Point(0, 136);
            label3.Name = "label3";
            label3.Size = new Size(215, 23);
            label3.TabIndex = 4;
            label3.Text = "报废原因";
            // 
            // ScrapLenInputNumber
            // 
            ScrapLenInputNumber.Dock = DockStyle.Top;
            ScrapLenInputNumber.Font = new Font("Microsoft YaHei UI", 10F);
            ScrapLenInputNumber.Location = new Point(0, 91);
            ScrapLenInputNumber.Name = "ScrapLenInputNumber";
            ScrapLenInputNumber.ReadOnly = true;
            ScrapLenInputNumber.Size = new Size(215, 45);
            ScrapLenInputNumber.SuffixText = "mm";
            ScrapLenInputNumber.TabIndex = 3;
            ScrapLenInputNumber.Text = "0";
            ScrapLenInputNumber.DoubleClick += ScrapLenInputNumber_DoubleClick;
            // 
            // label2
            // 
            label2.BackColor = Color.White;
            label2.Dock = DockStyle.Top;
            label2.Location = new Point(0, 68);
            label2.Name = "label2";
            label2.Size = new Size(215, 23);
            label2.TabIndex = 2;
            label2.Text = "报废数量";
            // 
            // ReportInputNumber
            // 
            ReportInputNumber.Dock = DockStyle.Top;
            ReportInputNumber.Font = new Font("Microsoft YaHei UI", 10F);
            ReportInputNumber.Location = new Point(0, 23);
            ReportInputNumber.Name = "ReportInputNumber";
            ReportInputNumber.Size = new Size(215, 45);
            ReportInputNumber.SuffixText = "PCS";
            ReportInputNumber.TabIndex = 1;
            ReportInputNumber.Text = "0";
            // 
            // label1
            // 
            label1.BackColor = Color.White;
            label1.Dock = DockStyle.Top;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(215, 23);
            label1.TabIndex = 0;
            label1.Text = "报工数量";
            // 
            // TaskTable
            // 
            TaskTable.BackColor = Color.White;
            TaskTable.Dock = DockStyle.Fill;
            TaskTable.EditMode = AntdUI.TEditMode.DoubleClick;
            TaskTable.Font = new Font("Microsoft YaHei UI", 10F);
            TaskTable.Gap = 12;
            TaskTable.Location = new Point(3, 98);
            TaskTable.Name = "TaskTable";
            tableLayoutPanel1.SetRowSpan(TaskTable, 3);
            TaskTable.Size = new Size(325, 474);
            TaskTable.TabIndex = 1;
            TaskTable.Text = "table1";
            TaskTable.CellClick += TaskTable_CellClick;
            TaskTable.FilterDataChanged += TaskTable_FilterDataChanged;
            // 
            // panel6
            // 
            tableLayoutPanel1.SetColumnSpan(panel6, 2);
            panel6.Controls.Add(TaskStatusTag);
            panel6.Controls.Add(CutLendLabel);
            panel6.Controls.Add(CutLendtLabel);
            panel6.Controls.Add(SuspendButton);
            panel6.Controls.Add(WorkStartButton);
            panel6.Controls.Add(CutLenuLabel);
            panel6.Controls.Add(CutLenutLabel);
            panel6.Controls.Add(CutLenLabel);
            panel6.Controls.Add(CutLentLabel);
            panel6.Controls.Add(ProgressLabel);
            panel6.Controls.Add(MatdLabel);
            panel6.Controls.Add(MatcLabel);
            panel6.Controls.Add(OrderNoLabel);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(334, 258);
            panel6.Name = "panel6";
            panel6.Size = new Size(384, 154);
            panel6.TabIndex = 8;
            panel6.Text = "panel6";
            // 
            // TaskStatusTag
            // 
            TaskStatusTag.Font = new Font("Microsoft YaHei UI", 10F);
            TaskStatusTag.Location = new Point(112, 12);
            TaskStatusTag.Name = "TaskStatusTag";
            TaskStatusTag.Size = new Size(57, 23);
            TaskStatusTag.TabIndex = 12;
            TaskStatusTag.Text = "";
            // 
            // CutLendLabel
            // 
            CutLendLabel.Anchor = AnchorStyles.Left;
            CutLendLabel.BackColor = Color.White;
            CutLendLabel.Font = new Font("Microsoft YaHei UI", 11F);
            CutLendLabel.Location = new Point(92, 128);
            CutLendLabel.Name = "CutLendLabel";
            CutLendLabel.Size = new Size(95, 23);
            CutLendLabel.TabIndex = 11;
            CutLendLabel.Text = "5200.00 mm";
            // 
            // CutLendtLabel
            // 
            CutLendtLabel.Anchor = AnchorStyles.Left;
            CutLendtLabel.BackColor = Color.White;
            CutLendtLabel.Font = new Font("Microsoft YaHei UI", 11F);
            CutLendtLabel.Location = new Point(10, 128);
            CutLendtLabel.Name = "CutLendtLabel";
            CutLendtLabel.Size = new Size(77, 23);
            CutLendtLabel.TabIndex = 10;
            CutLendtLabel.Text = "最小段长：";
            // 
            // SuspendButton
            // 
            SuspendButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SuspendButton.Font = new Font("Microsoft YaHei UI", 10F);
            SuspendButton.Location = new Point(287, 67);
            SuspendButton.Name = "SuspendButton";
            SuspendButton.Size = new Size(88, 45);
            SuspendButton.TabIndex = 4;
            SuspendButton.Text = "工单暂停";
            SuspendButton.Type = AntdUI.TTypeMini.Error;
            SuspendButton.Click += SuspendButton_Click;
            // 
            // WorkStartButton
            // 
            WorkStartButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            WorkStartButton.Font = new Font("Microsoft YaHei UI", 10F);
            WorkStartButton.Location = new Point(287, 111);
            WorkStartButton.Name = "WorkStartButton";
            WorkStartButton.Size = new Size(88, 45);
            WorkStartButton.TabIndex = 3;
            WorkStartButton.Text = "工单开工";
            WorkStartButton.Type = AntdUI.TTypeMini.Success;
            WorkStartButton.Click += WorkStartButton_Click;
            // 
            // CutLenuLabel
            // 
            CutLenuLabel.BackColor = Color.White;
            CutLenuLabel.Font = new Font("Microsoft YaHei UI", 11F);
            CutLenuLabel.Location = new Point(92, 99);
            CutLenuLabel.Name = "CutLenuLabel";
            CutLenuLabel.Size = new Size(95, 23);
            CutLenuLabel.TabIndex = 7;
            CutLenuLabel.Text = "5200.00 mm";
            // 
            // CutLenutLabel
            // 
            CutLenutLabel.BackColor = Color.White;
            CutLenutLabel.Font = new Font("Microsoft YaHei UI", 11F);
            CutLenutLabel.Location = new Point(10, 99);
            CutLenutLabel.Name = "CutLenutLabel";
            CutLenutLabel.Size = new Size(77, 23);
            CutLenutLabel.TabIndex = 6;
            CutLenutLabel.Text = "最大段长：";
            // 
            // CutLenLabel
            // 
            CutLenLabel.BackColor = Color.White;
            CutLenLabel.Font = new Font("Microsoft YaHei UI", 11F);
            CutLenLabel.Location = new Point(92, 70);
            CutLenLabel.Name = "CutLenLabel";
            CutLenLabel.Size = new Size(95, 23);
            CutLenLabel.TabIndex = 5;
            CutLenLabel.Text = "5200.00 mm";
            // 
            // CutLentLabel
            // 
            CutLentLabel.BackColor = Color.White;
            CutLentLabel.Font = new Font("Microsoft YaHei UI", 11F);
            CutLentLabel.Location = new Point(10, 70);
            CutLentLabel.Name = "CutLentLabel";
            CutLentLabel.Size = new Size(77, 23);
            CutLentLabel.TabIndex = 4;
            CutLentLabel.Text = "断线长度：";
            // 
            // ProgressLabel
            // 
            ProgressLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ProgressLabel.BackColor = Color.White;
            ProgressLabel.Font = new Font("Microsoft YaHei UI", 15F);
            ProgressLabel.Location = new Point(256, 6);
            ProgressLabel.Name = "ProgressLabel";
            ProgressLabel.Size = new Size(119, 29);
            ProgressLabel.TabIndex = 3;
            ProgressLabel.Text = "11/50";
            ProgressLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // MatdLabel
            // 
            MatdLabel.BackColor = Color.White;
            MatdLabel.Font = new Font("Microsoft YaHei UI", 10F);
            MatdLabel.Location = new Point(81, 41);
            MatdLabel.Name = "MatdLabel";
            MatdLabel.Size = new Size(293, 23);
            MatdLabel.TabIndex = 2;
            MatdLabel.Text = "CABLE L-YY  8X1X0.86  VZN BK UL2464";
            // 
            // MatcLabel
            // 
            MatcLabel.BackColor = Color.White;
            MatcLabel.Font = new Font("Microsoft YaHei UI", 13F);
            MatcLabel.Location = new Point(10, 41);
            MatcLabel.Name = "MatcLabel";
            MatcLabel.Size = new Size(65, 23);
            MatcLabel.TabIndex = 1;
            MatcLabel.Text = "840894";
            // 
            // OrderNoLabel
            // 
            OrderNoLabel.BackColor = Color.White;
            OrderNoLabel.Font = new Font("Microsoft YaHei UI", 15F);
            OrderNoLabel.Location = new Point(10, 6);
            OrderNoLabel.Name = "OrderNoLabel";
            OrderNoLabel.Size = new Size(93, 29);
            OrderNoLabel.TabIndex = 0;
            OrderNoLabel.Text = "3600000";
            // 
            // panel8
            // 
            panel8.Controls.Add(OrderScanInput);
            panel8.Dock = DockStyle.Fill;
            panel8.Location = new Point(3, 48);
            panel8.Name = "panel8";
            panel8.Size = new Size(325, 44);
            panel8.TabIndex = 9;
            panel8.Text = "panel8";
            // 
            // OrderScanInput
            // 
            OrderScanInput.Dock = DockStyle.Fill;
            OrderScanInput.Font = new Font("Microsoft YaHei UI", 11F);
            OrderScanInput.Location = new Point(0, 0);
            OrderScanInput.Name = "OrderScanInput";
            OrderScanInput.Size = new Size(325, 44);
            OrderScanInput.TabIndex = 0;
            OrderScanInput.KeyPress += OrderScanInput_KeyPress;
            // 
            // panel2
            // 
            panel2.Controls.Add(InspectLabelButton);
            panel2.Controls.Add(ProcessCardPrintButton);
            panel2.Controls.Add(AdjustButton);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(724, 48);
            panel2.Name = "panel2";
            tableLayoutPanel1.SetRowSpan(panel2, 2);
            panel2.Size = new Size(215, 204);
            panel2.TabIndex = 4;
            panel2.Text = "panel2";
            // 
            // InspectLabelButton
            // 
            InspectLabelButton.Dock = DockStyle.Bottom;
            InspectLabelButton.Font = new Font("Microsoft YaHei UI", 10F);
            InspectLabelButton.Location = new Point(0, 159);
            InspectLabelButton.Name = "InspectLabelButton";
            InspectLabelButton.Size = new Size(215, 45);
            InspectLabelButton.TabIndex = 3;
            InspectLabelButton.Text = "首检标签打印";
            InspectLabelButton.Type = AntdUI.TTypeMini.Primary;
            InspectLabelButton.Click += InspectLabelButton_Click;
            // 
            // ProcessCardPrintButton
            // 
            ProcessCardPrintButton.BorderWidth = 1F;
            ProcessCardPrintButton.Dock = DockStyle.Top;
            ProcessCardPrintButton.Font = new Font("Microsoft YaHei UI", 10F);
            ProcessCardPrintButton.Location = new Point(0, 45);
            ProcessCardPrintButton.Name = "ProcessCardPrintButton";
            ProcessCardPrintButton.Size = new Size(215, 45);
            ProcessCardPrintButton.TabIndex = 2;
            ProcessCardPrintButton.Text = "流转卡打印";
            ProcessCardPrintButton.Type = AntdUI.TTypeMini.Primary;
            ProcessCardPrintButton.Click += ProcessCardPrintButton_Click;
            // 
            // AdjustButton
            // 
            AdjustButton.Dock = DockStyle.Top;
            AdjustButton.Font = new Font("Microsoft YaHei UI", 10F);
            AdjustButton.Location = new Point(0, 0);
            AdjustButton.Name = "AdjustButton";
            AdjustButton.Size = new Size(215, 45);
            AdjustButton.TabIndex = 6;
            AdjustButton.Text = "盘盈盘亏";
            AdjustButton.Type = AntdUI.TTypeMini.Warn;
            AdjustButton.Click += AdjustButton_Click;
            // 
            // panel5
            // 
            tableLayoutPanel1.SetColumnSpan(panel5, 2);
            panel5.Controls.Add(tabs1);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(334, 48);
            panel5.Name = "panel5";
            tableLayoutPanel1.SetRowSpan(panel5, 2);
            panel5.Size = new Size(384, 204);
            panel5.TabIndex = 7;
            panel5.Text = "panel5";
            // 
            // tabs1
            // 
            tabs1.BackColor = Color.White;
            tabs1.Controls.Add(tabPage1);
            tabs1.Controls.Add(tabPage4);
            tabs1.Dock = DockStyle.Fill;
            tabs1.Location = new Point(0, 0);
            tabs1.Name = "tabs1";
            tabs1.Pages.Add(tabPage1);
            tabs1.Pages.Add(tabPage4);
            tabs1.Size = new Size(384, 204);
            tabs1.Style = styleLine2;
            tabs1.TabIndex = 2;
            tabs1.Text = "tabs1";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(AllLoadingTable);
            tabPage4.Dock = DockStyle.Fill;
            tabPage4.Location = new Point(0, 30);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new Size(384, 174);
            tabPage4.TabIndex = 1;
            tabPage4.Text = "上料记录";
            // 
            // AllLoadingTable
            // 
            AllLoadingTable.Dock = DockStyle.Fill;
            AllLoadingTable.Gap = 12;
            AllLoadingTable.Location = new Point(0, 0);
            AllLoadingTable.Name = "AllLoadingTable";
            AllLoadingTable.Size = new Size(384, 174);
            AllLoadingTable.TabIndex = 1;
            AllLoadingTable.Text = "table3";
            AllLoadingTable.CellButtonClick += AllLoadingTable_CellButtonClick;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(tableLayoutPanel2);
            tabPage1.Dock = DockStyle.Fill;
            tabPage1.Font = new Font("Microsoft YaHei UI", 10F);
            tabPage1.Location = new Point(0, 30);
            tabPage1.Name = "tabPage1";
            tabPage1.Size = new Size(384, 174);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "工位库存";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Controls.Add(LoadingTable, 0, 1);
            tableLayoutPanel2.Controls.Add(BarCodeScanInput, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(384, 174);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // LoadingTable
            // 
            LoadingTable.Dock = DockStyle.Fill;
            LoadingTable.Gap = 12;
            LoadingTable.Location = new Point(3, 53);
            LoadingTable.Name = "LoadingTable";
            LoadingTable.Size = new Size(378, 118);
            LoadingTable.TabIndex = 0;
            LoadingTable.Text = "table3";
            LoadingTable.CellButtonClick += LoadingTable_CellButtonClick;
            // 
            // BarCodeScanInput
            // 
            BarCodeScanInput.Dock = DockStyle.Fill;
            BarCodeScanInput.Font = new Font("Microsoft YaHei UI", 11F);
            BarCodeScanInput.Location = new Point(3, 3);
            BarCodeScanInput.Name = "BarCodeScanInput";
            BarCodeScanInput.Size = new Size(378, 44);
            BarCodeScanInput.TabIndex = 1;
            BarCodeScanInput.KeyPress += BarCodeScanInput_KeyPress;
            // 
            // CuttingTaskExecutionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(942, 575);
            Controls.Add(tableLayoutPanel1);
            Name = "CuttingTaskExecutionForm";
            Text = "CuttingTaskExecutionForm";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            tabs2.ResumeLayout(false);
            tabPage2.ResumeLayout(false);
            tabPage3.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel5.ResumeLayout(false);
            tabs1.ResumeLayout(false);
            tabPage4.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private AntdUI.Panel panel1;
        private AntdUI.Select PrinterSelect;
        private AntdUI.Select WorkStationSelect;
        private AntdUI.Select WorkCenterSelect;
        private AntdUI.DatePicker StartDatePicker;
        private AntdUI.Button SearchButton;
        private AntdUI.Table TaskTable;
        private AntdUI.Tabs tabs1;
        private AntdUI.Tabs tabs2;
        private AntdUI.TabPage tabPage1;
        private AntdUI.TabPage tabPage2;
        private AntdUI.TabPage tabPage3;
        private AntdUI.Panel panel2;
        private AntdUI.Panel panel3;
        private AntdUI.Button InspectLabelButton;
        private AntdUI.Button ProcessCardPrintButton;
        private AntdUI.Button AdjustButton;
        private AntdUI.InputNumber ScrapLenInputNumber;
        private AntdUI.Label label2;
        private AntdUI.InputNumber ReportInputNumber;
        private AntdUI.Label label1;
        private AntdUI.Select ScrapReasonSelect;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Button SubmitButton;
        private AntdUI.Input RemarkInput;
        private AntdUI.Button SuspendButton;
        private AntdUI.Button WorkStartButton;
        private AntdUI.Panel panel5;
        private AntdUI.Panel panel6;
        private AntdUI.Label OrderNoLabel;
        private AntdUI.Label ProgressLabel;
        private AntdUI.Label MatdLabel;
        private AntdUI.Label MatcLabel;
        private AntdUI.Label CutLenuLabel;
        private AntdUI.Label CutLenutLabel;
        private AntdUI.Label CutLenLabel;
        private AntdUI.Label CutLentLabel;
        private AntdUI.Table BomTable;
        private AntdUI.Table LoadingTable;
        private AntdUI.Panel panel8;
        private AntdUI.Input OrderScanInput;
        private AntdUI.Input BarCodeScanInput;
        private TableLayoutPanel tableLayoutPanel2;
        private AntdUI.Label CutLendLabel;
        private AntdUI.Label CutLendtLabel;
        private AntdUI.Tag TaskStatusTag;
        private AntdUI.Table ExecuteTable;
        private AntdUI.TabPage tabPage4;
        private AntdUI.Table AllLoadingTable;
    }
}