using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace BizLink.MES.WinForms.Forms
{
    // 1. 继承 MesWindowForm (弹窗基类)
    public partial class InventoryTransferForm : MesWindowForm
    {
        private readonly InventoryModuleFacade _facade;

        private int _stockId;
        private RawLinesideStockDto _rawLinesideStockDto;

        // 事件回调 (可选，用于通知父窗体刷新)
        public event Action OnSuccess;

        // 2. 构造函数：只注入 Facade
        public InventoryTransferForm(InventoryModuleFacade facade)
        {
            _facade = facade;
            InitializeComponent();
        }

        // 3. 初始化数据入口
        public void InitData(int stockId)
        {
            _stockId = stockId;
        }

        // 4. 窗体加载
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e); // 务必调用

            label7.PrefixColor = Color.Red;

            // 使用 RunAsync 自动处理加载异常
            await RunAsync(async () =>
            {
                await LoadDataAsync();
            });
        }

        private async Task LoadDataAsync()
        {
            // 加载库存信息
            _rawLinesideStockDto = await _facade.RawStock.GetByIdAsync(_stockId);
            if (_rawLinesideStockDto == null)
            {
                throw new Exception("未查询到库存信息！");
            }

            // 绑定 UI
            materialnput.Text = _rawLinesideStockDto.MaterialCode;
            barcodeInput.Text = _rawLinesideStockDto.BarCode;
            batchInput.Text = _rawLinesideStockDto.BatchCode;
            originalInputNumber.Value = (decimal)_rawLinesideStockDto.LastQuantity;
            originalLocationInput.Text = _rawLinesideStockDto.LocationDesc;

            // 加载库位列表
            locationSelect.Items.Clear();
            var locations = await _facade.Location.GetAllBinAsync();

            if (locations != null)
            {
                var validLocations = locations
                    .Where(x => x.FactoryId == AppSession.CurrentFactoryId)
                    .Select(x => new MenuItem
                    {
                        Name = x.Id.ToString(),
                        Text = x.Name
                    }).ToArray();

                locationSelect.Items.AddRange(validLocations);
            }
        }

        // 5. 提交逻辑
        private async void submitButton_Click(object sender, EventArgs e)
        {
            // 使用 RunAsync 自动处理 Loading、禁用、异常、确认框
            await RunAsync(submitButton, async () =>
            {
                // --- 1. 校验 ---
                if (locationSelect.SelectedValue == null)
                    throw new Exception("目标库位未选择，请先选择目标库位！");

                var targetLocationId = int.Parse(((MenuItem)locationSelect.SelectedValue).Name);
                var targetLocationName = ((MenuItem)locationSelect.SelectedValue).Text;

                // --- 2. 验证库位有效性 ---
                var location = await _facade.Location.GetByIdAsync(targetLocationId);
                if (location == null)
                    throw new Exception($"未查询到{targetLocationName}的库位信息，请确认库位是否正确！");

                // --- 3. 执行移库业务 ---

                // A. 更新库存记录位置
                var updateDto = new RawLinesideStockUpdateDto
                {
                    Id = _stockId,
                    LocationId = location.Id,
                    LocationCode = location.Code,
                    LocationDesc = location.Name,
                    UpdateBy = AppSession.CurrentUser.EmployeeId,
                    UpdatedAt = DateTime.Now,
                };

                if (!await _facade.RawStock.UpdateAsync(updateDto))
                    throw new Exception("库存转移失败，请重试！");

                // B. 记录日志：转出 (TransferOut)
                await _facade.StockLog.CreateAsync(new RawLinesideStockLogCreateDto
                {
                    RawLinesideStockId = _rawLinesideStockDto.Id,
                    OperationType = StockOperationType.TransferOut,
                    InOutStatus = InOutStatus.Out,
                    ChangeQuantity = (decimal)_rawLinesideStockDto.Quantity,
                    QuantityBefore = (decimal)_rawLinesideStockDto.Quantity,
                    QuantityAfter = 0, // 对于原位置而言，数量变更为0 (或者视具体业务逻辑而定，这里沿用原代码逻辑)
                    MaterialCode = _rawLinesideStockDto.MaterialCode,
                    BarCode = _rawLinesideStockDto.BarCode,
                    BatchCode = _rawLinesideStockDto.BatchCode,
                    LocationId = (int)_rawLinesideStockDto.LocationId,
                    LocationCode = _rawLinesideStockDto.LocationCode,
                    CreateBy = AppSession.CurrentUser.EmployeeId,
                    Remark = StockOperationType.TransferOut.GetDescription()
                });

                // C. 记录日志：转入 (TransferIn)
                await _facade.StockLog.CreateAsync(new RawLinesideStockLogCreateDto
                {
                    RawLinesideStockId = _rawLinesideStockDto.Id,
                    OperationType = StockOperationType.TransferIn,
                    InOutStatus = InOutStatus.In,
                    ChangeQuantity = (decimal)_rawLinesideStockDto.Quantity,
                    QuantityBefore = 0,
                    QuantityAfter = (decimal)_rawLinesideStockDto.Quantity,
                    MaterialCode = _rawLinesideStockDto.MaterialCode,
                    BarCode = _rawLinesideStockDto.BarCode,
                    BatchCode = _rawLinesideStockDto.BatchCode,
                    LocationId = location.Id,
                    LocationCode = location.Code,
                    CreateBy = AppSession.CurrentUser.EmployeeId,
                    Remark = StockOperationType.TransferIn.GetDescription()
                });

                // --- 4. 成功回调 ---
                OnSuccess?.Invoke();
                this.DialogResult = DialogResult.OK;
                this.Close();

            },
            successMsg: "库存转移提交成功！",
            confirmMsg: "即将对库存进行移库，是否继续？");
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
