using AntdUI;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Facade;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using System;
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
    public partial class InventoryAddForm : MesWindowForm
    {
        private readonly InventoryModuleFacade _facade;

        // 事件回调 (可选，用于通知父窗体刷新)
        public event Action OnSuccess;

        // 2. 构造函数：只注入 Facade
        public InventoryAddForm(InventoryModuleFacade facade)
        {
            _facade = facade;
            InitializeComponent();
        }

        // 3. 窗体加载
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e); // 务必调用

            // UI 初始化
            label3.PrefixColor = Color.Red;
            label4.PrefixColor = Color.Red;
            label5.PrefixColor = Color.Red;

            changeTypeSelect.Items.AddRange(new List<MenuItem>
            {
                new MenuItem()
                {
                    Name = "1",
                    Text = "增加库存" 
                },
                new MenuItem()
                {
                    Name = "-1",
                    Text = "减少库存"
                }
            }.ToArray());

        }


        // 4. 提交逻辑
        private async void submitButton_Click(object sender, EventArgs e)
        {
            // 使用 RunAsync 自动处理 Loading、禁用、异常
            await RunAsync(submitButton, async () =>
            {
                // --- 1. 校验 ---
                var materialcode = materialnput.Text.Trim();
                if (string.IsNullOrWhiteSpace(materialcode))
                    throw new Exception("未获取到物料信息，请检查源标签！");

                var materialdto = await _facade.MaterialView.GetByCodeAsync(AppSession.CurrentUser.FactoryName, materialcode);
                if (materialdto == null)
                    throw new Exception("未查询到物料信息，请检查物料号！");

                var barcode = OldbarcodeInput.Text.Trim();

                if (string.IsNullOrWhiteSpace(barcode))
                    throw new Exception("未查询到源标签对应的批次，请扫描源标签！");

                if(changeTypeSelect.SelectedValue == null)
                    throw new Exception("请选择调整类型！");
                var changeType = int.Parse(((MenuItem)changeTypeSelect.SelectedValue).Name);

                var quantity = ChangeInputNumber.Value;
                if (quantity <= 0)
                    throw new Exception("请输入数量！");

                var batchinfo = await _facade.RawStock.GetByBarCodeAsync(AppSession.CurrentFactoryId, barcode);

                var rawstockupdate = new RawLinesideStockUpdateDto
                {
                    Id = batchinfo.Id,
                    LastQuantity = batchinfo.LastQuantity + (changeType * quantity),
                    UpdateBy = AppSession.CurrentUser.EmployeeId,
                    UpdatedAt = DateTime.Now
                };

                var rtn = await _facade.RawStock.UpdateAsync(rawstockupdate);

                var newrawStock = await _facade.RawStock.GetByIdAsync(batchinfo.Id);
                //if (locationSelect.SelectedValue == null)
                //    throw new Exception("请选择目标库位！");

                //var locationId = int.Parse(((MenuItem)locationSelect.SelectedValue).Name);
                //var location = await _facade.Location.GetByIdAsync(locationId);

                //if (location == null)
                //    throw new Exception("未查询到目标库位，请重新选择！");

                // --- 2. 创建库存 ---
                //var rawstockcreate = new RawLinesideStockCreateDto
                //{
                //    FactoryId = AppSession.CurrentFactoryId,
                //    MaterialId = materialdto.Id,
                //    MaterialCode = materialdto.MaterialCode,
                //    MaterialDesc = materialdto.MaterialName,
                //    BaseUnit = materialdto.BaseUnit,
                //    BatchCode = batchcode,
                //    BarCode = batchcode + "001", // 假设生成规则
                //    Quantity = quantity,
                //    LastQuantity = quantity,
                //    LocationId = location.Id,
                //    LocationCode = location.Code,
                //    LocationDesc = location.Name,
                //    CreatedBy = AppSession.CurrentUser.EmployeeId
                //};

                //var rtn = await _facade.RawStock.CreateAsync(rawstockcreate);
                var operationType = changeType == 1 ? StockOperationType.MesStockAdd : StockOperationType.MesStockReduce;
                var inoutStatus = changeType == 1 ? InOutStatus.In : InOutStatus.Out;

                if (newrawStock != null)
                {
                    // --- 3. 记录日志 ---
                    await _facade.StockLog.CreateAsync(new RawLinesideStockLogCreateDto
                    {
                        RawLinesideStockId = newrawStock.Id,
                        OperationType = operationType,
                        InOutStatus = inoutStatus,
                        ChangeQuantity = (decimal)quantity,
                        QuantityBefore = (decimal)newrawStock.LastQuantity - (decimal)quantity,
                        QuantityAfter = (decimal)newrawStock.LastQuantity,
                        MaterialCode = newrawStock.MaterialCode,
                        BarCode = newrawStock.BarCode,
                        BatchCode = newrawStock.BatchCode,
                        LocationId = (int)newrawStock.LocationId,
                        LocationCode = newrawStock.LocationCode,
                        CreateBy = AppSession.CurrentUser.EmployeeId,
                        Remark = operationType.GetDescription()
                    });
                }

                // --- 4. 成功回调 ---
                OnSuccess?.Invoke();
                this.DialogResult = DialogResult.OK;
                this.Close();

            }, successMsg: "库存调整成功！",confirmMsg:"即将对标签库存进行调整，是否继续？");
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void OldbarcodeInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            await RunAsync(async () =>
            {
                if (e.KeyChar == '\r')
                {
                    var barcode = OldbarcodeInput.Text.Trim();
                    var batchinfo = await _facade.RawStock.GetByBarCodeAsync(AppSession.CurrentFactoryId, barcode);
                    if (batchinfo == null)
                        throw new Exception("未查询到批次信息，请检查标签！");

                    //检查标签是否在使用中
                    var materialaddLog = await _facade.MaterialAdd.GetByBarcodeAsync(barcode);
                    if (materialaddLog != null)
                        throw new Exception("该标签正在使用中，无法调整库存！");

                    materialnput.Text = batchinfo.MaterialCode;
                    OldbatchInput.Text = batchinfo.BatchCode;
                    OldInputNumber.Value = (decimal)batchinfo.LastQuantity;
                }
            });

        }
    }
}
