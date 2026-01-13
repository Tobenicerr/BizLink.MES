using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.WinForms.Common;
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
    public partial class WorkCenterEditForm : BaseForm
    {

        private readonly IWorkCenterService _workCenterService;

        public WorkCenterEditForm(IWorkCenterService workCenterService)
        {
            InitializeComponent();
            _workCenterService = workCenterService;
        }

        private void WorkCenterEditForm_Load(object sender, EventArgs e)
        {
            
        }

        private void groupSwitch_CheckedChanged(object sender, BoolEventArgs e)
        {
            if (e.Value)
            {
                addButtom.Enabled = true;

            }
            else
            {
                addButtom.Enabled = false;
            }
        }

        private async void saveButtom_Click(object sender, EventArgs e)
        {
            var isGroup = groupSwitch.Checked;
            var createDto = new WorkCenterCreateDto()
            {
                WorkCenterCode = workCenterCodeInput.Text.Trim(),
                WorkCenterName = workCenterNameInput.Text.Trim(),
                WorkCenterDesc = workCenterDescInput.Text.Trim(),
                IsGroup = groupSwitch.Checked,
                CreatedBy = AppSession.CurrentUser.EmployeeId,
                FactoryId = AppSession.CurrentFactoryId,

            };


            await _workCenterService.CreateAsync(createDto);
        }
    }
}
