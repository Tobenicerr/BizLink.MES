using BizLink.MES.Application.DTOs;
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

namespace BizLink.MES.WinForms.Forms
{
    public partial class WmsAutoStockOutErrorForm : MesBaseForm
    {
        private readonly IFactoryService _factoryService;
        private readonly IWmsAutoStockOutLogService _wmsAutoStockOutLogService;
        public WmsAutoStockOutErrorForm(IFactoryService factoryService, IWmsAutoStockOutLogService wmsAutoStockOutLogService)
        {
            InitializeComponent();
            InitializeTable();
            _factoryService = factoryService;
            _wmsAutoStockOutLogService = wmsAutoStockOutLogService;
        }

        private void InitializeTable()
        {

            TableControl.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("Id", "Id", AntdUI.ColumnAlign.Center)
                {
                    Visible = false
                },
                new AntdUI.Column("BillNo", "业务单号", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("BatchCode", "物料批次", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ProcessFlag", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        switch (value as string)
                        {
                            case "同步失败":
                                return new AntdUI.CellTag("同步失败", AntdUI.TTypeMini.Error);
                            case "未同步":
                                return new AntdUI.CellTag("未同步", AntdUI.TTypeMini.Primary);

                            case "已同步":
                                return new AntdUI.CellTag("已同步", AntdUI.TTypeMini.Success);
                            default:
                               return new AntdUI.CellTag("同步失败", AntdUI.TTypeMini.Error);
                        }
                    }
                }.SetFixed().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "数量", AntdUI.ColumnAlign.Right).SetDisplayFormat("0.###").SetLocalizationTitleID("Table.Column."),
               new AntdUI.Column("BarCode", "物料标签", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("BarCodeNew", "物料标签（新）", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("FactoryCode", "工厂", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("Message", "接口信息", AntdUI.ColumnAlign.Left).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CreateTime", "创建日期", AntdUI.ColumnAlign.Center).SetWidth("auto").SetSortOrder().SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("UpdateTime", "传输日期", AntdUI.ColumnAlign.Center).SetWidth("auto").SetSortOrder().SetDefaultFilter().SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),

               new AntdUI.Column("Btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),

             };

        }

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            KeyWordInput.PlaceholderText = "请输入关键字...";

            var count = await LoadDataAsync();
            AntdUI.Message.success(this, $"查询成功：共查询出{count}笔记录");
        }

        private async void SearchButton_Click(object sender, EventArgs e)
        {
            await RunAsync(SearchButton, async () =>
            {
                var count = await LoadDataAsync();
                AntdUI.Message.success(this, $"查询成功：共查询出{count}笔记录");
            });
        }

        private async Task<int> LoadDataAsync()
        {
            try
            {
                SpinControl.Visible = true;
                TableControl.DataSource = null;

                var keyword = KeyWordInput.Text.Trim();

                var factory = await _factoryService.GetByIdAsync(AppSession.CurrentFactoryId);
                var result = await _wmsAutoStockOutLogService.GetFailListAsync(factory.FactoryCode, keyword);
                if (result != null)
                {
                    TableControl.DataSource = result.Select(x => new WmsAutoStockOutErrorViewModel(x)).ToList();
                    return result.Count();
                }
                else
                {
                    TableControl.DataSource = null;
                }

                return 0;

            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                SpinControl.Visible = false;
            }
        }

        private async void TableControl_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {

            await RunAsync(null, async () =>
            {
                if (e.Record is WmsAutoStockOutErrorViewModel data)
                {
                    var updateDto = new WmsAutoStockOutLogUpdateDto()
                    {
                        Id = data.Id,
                    };
                    if (e.Btn.Id == "reentry")
                    {
                        updateDto.ProcessFlag = 0;
                        updateDto.Message = string.Empty;


                    }
                    else if (e.Btn.Id == "close")
                    {
                        updateDto.ProcessFlag = 1;
                        updateDto.Message = "手动关闭";
                    }

                    await _wmsAutoStockOutLogService.UpdateAsync(updateDto);
                    await LoadDataAsync();
                }
            }, successMsg: "操作成功！", confirmMsg: "是否进行操作？");
        }
    }

    public class WmsAutoStockOutErrorViewModel : AntdUI.NotifyProperty
    {
        public WmsAutoStockOutErrorViewModel(WmsAutoStockOutLogDto dto) 
        {

            _id = dto.Id;
            _billNo = dto.BillNo;
            _materialCode = dto.MaterialCode;
            _batchCode = dto.BatchCode;
            _quantity = dto.Quantity;
            _barcode = dto.BarCode;
            _barCodeNew = dto.BarCodeNew;
            _factoryCode = dto.FactoryCode;
            _processFlag = dto.ProcessFlag == 0 ? "未同步" : "同步失败";
            _message = (dto.Message??string.Empty).Contains(" at") ? dto.Message.Split(" at")[0] : dto.Message;
            _createTime = dto.CreateTime;
            _updateTime = dto.UpdateTime;
            _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("reentry", "重推", AntdUI.TTypeMini.Primary),
                        new AntdUI.CellButton("close", "关闭", AntdUI.TTypeMini.Error)
            };
        }


        int _id;
        public int Id
        {
            get => _id;
            set
            {
                if (_id == value)
                    return;
                _id = value;
                OnPropertyChanged();
            }
        }

        string? _billNo;
        public string? BillNo
        {
            get => _billNo;
            set
            {
                if (_billNo == value)
                    return;
                _billNo = value;
                OnPropertyChanged();
            }
        }

        string? _materialCode;
        public string? MaterialCode
        {
            get => _materialCode;
            set
            {
                if (_materialCode == value)
                    return;
                _materialCode = value;
                OnPropertyChanged();
            }
        }

        string? _batchCode;
        public string? BatchCode
        {
            get => _batchCode;
            set
            {
                if (_batchCode == value)
                    return;
                _batchCode = value;
                OnPropertyChanged();
            }
        }

        decimal _quantity;
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value)
                    return;
                _quantity = value;
                OnPropertyChanged();
            }
        }

        string? _barcode;
        public string? BarCode
        {
            get => _barcode;
            set
            {
                if (_barcode == value)
                    return;
                _barcode = value;
                OnPropertyChanged();
            }
        }

        string? _barCodeNew;
        public string? BarCodeNew
        {
            get => _barCodeNew;
            set
            {
                if (_barCodeNew == value)
                    return;
                _barCodeNew = value;
                OnPropertyChanged();
            }
        }

        string? _factoryCode;
        public string? FactoryCode
        {
            get => _factoryCode;
            set
            {
                if (_factoryCode == value)
                    return;
                _factoryCode = value;
                OnPropertyChanged();
            }
        }

        string _processFlag;
        public string ProcessFlag
        {
            get => _processFlag;
            set
            {
                if (_processFlag == value)
                    return;
                _processFlag = value;
                OnPropertyChanged();
            }
        }

        string? _message;
        public string? Message
        {
            get => _message;
            set
            {
                if (_message == value)
                    return;
                _message = value;
                OnPropertyChanged();
            }
        }

        DateTime? _createTime;
        public DateTime? CreateTime
        {
            get => _createTime;
            set
            {
                if (_createTime == value)
                    return;
                _createTime = value;
                OnPropertyChanged();
            }
        }

        DateTime? _updateTime;
        public DateTime? UpdateTime
        {
            get => _updateTime;
            set
            {
                if (_updateTime == value)
                    return;
                _updateTime = value;
                OnPropertyChanged();
            }
        }

        AntdUI.CellLink[] _btns;
        public AntdUI.CellLink[] Btns
        {
            get => _btns;
            set
            {
                _btns = value;
                OnPropertyChanged();
            }
        }
    }
}
