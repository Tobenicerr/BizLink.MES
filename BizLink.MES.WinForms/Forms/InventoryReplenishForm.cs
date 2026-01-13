using AntdUI;
using Azure.Core;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using Newtonsoft.Json;
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
    // 1. 继承 MesWindowForm (弹窗基类)
    public partial class InventoryReplenishForm : MesWindowForm
    {
        private readonly InventoryModuleFacade _facade;
        private RawLinesideStockDto _stockDto;

        // 事件回调 (可选，用于通知父窗体刷新)
        public event Action OnSuccess;

        // 2. 构造函数：只注入 Facade
        public InventoryReplenishForm(InventoryModuleFacade facade)
        {
            _facade = facade;
            InitializeComponent();
        }

        // 3. 初始化数据入口
        public void InitData(RawLinesideStockDto dto)
        {
            _stockDto = dto;
        }

        // 4. 窗体加载
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeUI();
        }

        private void InitializeUI()
        {
            label2.PrefixColor = Color.Red;
            label3.PrefixColor = Color.Red;

            if (_stockDto != null)
            {
                materialSelect.Items.Clear();
                materialSelect.Items.Add(new MenuItem()
                {
                    Name = _stockDto.MaterialId.ToString(),
                    Text = _stockDto.MaterialCode
                });
                materialSelect.SelectedIndex = 0;
            }
        }

        // 5. 提交逻辑
        private async void submitButton_Click(object sender, EventArgs e)
        {
            // 使用 RunAsync 自动处理 Loading、禁用、异常
            await RunAsync(submitButton, async () =>
            {
                // --- 1. 校验 ---
                if (materialSelect.SelectedValue == null)
                    throw new Exception("物料未选择，请先选择物料！");

                if (repInputNumber.Value <= 0)
                    throw new Exception("补料数量必须大于0，请修改补料数量！");

                var materialId = int.Parse(((MenuItem)materialSelect.SelectedValue).Name);

                // --- 2. 构建 JY (WMS) 请求 ---
                var materialEntries = new List<MaterialEntry>
                {
                    new MaterialEntry
                    {
                        MaterialId = materialId,
                        Qty = repInputNumber.Value
                    }
                };

                // 使用 _facade.ApiSettings 获取配置
                var requestUrl = _facade.ApiSettings["JyApi"].Endpoints["TransvouchCreate"];

                var request = new MaterialRequisitionRequest
                {
                    VoucherTypeCategoryCode = "VTC022",
                    VoucherTypeName = "999",
                    OWarehouseCode = "1100",
                    IWarehouseCode = "2100",
                    OStorageCode = "11004",
                    IStorageCode = "21001",
                    VirtualOPlantCode = AppSession.CurrentUser.FactoryName,
                    VirtualIPlantCode = AppSession.CurrentUser.FactoryName,
                    EntryDtos = materialEntries,
                    PlantCode = AppSession.CurrentUser.FactoryName,
                    IsActive = false,
                    IsActiveDisplay = "",
                    OperateUser = AppSession.CurrentUser.EmployeeId,
                    OperateUserName = AppSession.CurrentUser.UserName,
                    Remark = "断线"
                };

                // --- 3. 调用外部接口 ---
                // 序列化 (虽然 PostAsync 内部可能也会处理，但这里保留原逻辑)
                // var json = JsonConvert.SerializeObject(request, Formatting.Indented); 
                // 如果 _facade.JyApi.PostAsync 接受对象，建议直接传 request，这里沿用您的逻辑传对象

                var result = await _facade.JyApi.PostAsync<MaterialRequisitionRequest, object>(requestUrl, request);

                if (!result.IsSuccess)
                    throw new Exception($"补料提交失败: {result.Message}");

                // --- 4. 记录本地日志 ---
                await _facade.StockLog.CreateAsync(new RawLinesideStockLogCreateDto
                {
                    OperationType = StockOperationType.Replenishment,
                    InOutStatus = InOutStatus.In,
                    ChangeQuantity = repInputNumber.Value,
                    QuantityBefore = (decimal)(_stockDto?.Quantity ?? 0),
                    QuantityAfter = (decimal)(_stockDto?.Quantity ?? 0) + repInputNumber.Value,
                    MaterialCode = _stockDto?.MaterialCode,
                    CreateBy = AppSession.CurrentUser.EmployeeId,
                    Remark = string.IsNullOrWhiteSpace(remarkInput.Text.Trim())
                        ? StockOperationType.Replenishment.GetDescription()
                        : remarkInput.Text.Trim()
                });

                // --- 5. 成功回调 ---
                OnSuccess?.Invoke();

                this.DialogResult = DialogResult.OK;
                this.Close();

            }, successMsg: "补料申请提交成功！"); // 自动弹出成功提示
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
