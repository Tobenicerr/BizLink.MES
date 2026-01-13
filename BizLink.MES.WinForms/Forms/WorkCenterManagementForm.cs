using AntdUI;
using BizLink.MES.Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class WorkCenterManagementForm : BaseForm
    {

        private readonly IWorkCenterService _workCenterService;
        public WorkCenterManagementForm(IWorkCenterService workCenterService)
        {
            InitializeComponent();
            InitializeTable();
            _workCenterService = workCenterService;
        }

        private void WorkCenterManagementForm_Load(object sender, EventArgs e)
        {
        }

        private void InitializeTable()
        {
            workcenterTable.Columns = new AntdUI.ColumnCollection
            {
                //new AntdUI.ColumnCheck("check").SetFixed(),
                new AntdUI.Column("WorkCenterCode", "工作中心代码", AntdUI.ColumnAlign.Center).SetFixed().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenterName", "工作中心名称", AntdUI.ColumnAlign.Center).SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("WorkCenterDesc", "工作中心描述", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("IsGroup", "是否组", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Column."),
            };

        }

        private void addButton_Click(object sender, EventArgs e)
        {
            WorkCenterEditForm detailPage = new WorkCenterEditForm(_workCenterService);
            detailPage.Dock = DockStyle.Fill;
            AntdUI.Drawer.Config config = new AntdUI.Drawer.Config(this.ParentForm, detailPage);
            config.Mask = true;
            AntdUI.Drawer.open(config);
        }
    }
}
