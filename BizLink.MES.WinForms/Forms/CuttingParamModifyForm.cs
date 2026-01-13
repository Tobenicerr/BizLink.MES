using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using Dm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class CuttingParamModifyForm : AntdUI.Window
    {
        public delegate void CutParamPassHandler(CableCutParamDto param, int processid, string cableitem);

        public event CutParamPassHandler? CutParamPassed;

        private readonly int _processId;
        private readonly string _cableItem;

        private readonly List<CableCutParamDto> _cableCutParams = new List<CableCutParamDto>();
        public CuttingParamModifyForm(List<CableCutParamDto> cableCutParams ,int processid,string cableitem)
        {
            _cableCutParams = cableCutParams;
            _processId = processid;
            _cableItem = cableitem;
            InitializeComponent();
        }

        private void CuttingParamModifyForm_Load(object sender, EventArgs e)
        {
            cableSelect.Items.AddRange(_cableCutParams.OrderBy(x => x.PositionItem).Select(s => new MenuItem
            {
                Name = s.Id.ToString(),
                Text = s.PositionItem + "-" + s.CableMaterialCode
            }).ToArray());
        }

        private void cableSelect_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            var selectedParam = _cableCutParams.FirstOrDefault(f => f.Id == Convert.ToInt32(((MenuItem)cableSelect.SelectedValue).Name));
            if (selectedParam != null)
            {
                cutlenLable.Text = cutlenLable.Text.Split('：')[0] + "：" + Math.Round((decimal)selectedParam.CuttingLength,1).ToString() + " mm";
                lenuslLabel.Text = lenuslLabel.Text.Split('：')[0] + "：" + Math.Round((decimal)(selectedParam .BomLength+selectedParam.UpTol),1).ToString() + " mm";
                lendslLabel.Text = lendslLabel.Text.Split('：')[0] + "：" + Math.Round((decimal)(selectedParam.BomLength+selectedParam.DownTol),1).ToString() + " mm";
                cutqtyLabel.Text = cutqtyLabel.Text.Split('：')[0] + "：" + Convert.ToInt32(selectedParam.CablePcs).ToString() + " PCS";
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (AntdUI.Modal.open(this.ParentForm, "提示", "即将替换断线参数，是否继续？", AntdUI.TType.Warn) == DialogResult.OK)
            {
                var selectedParam = _cableCutParams.FirstOrDefault(f => f.Id == Convert.ToInt32(((MenuItem)cableSelect.SelectedValue).Name));
                if (selectedParam != null)
                    CutParamPassed?.Invoke(selectedParam, _processId, _cableItem);
                this.Close();
            }

        }
    }
}
