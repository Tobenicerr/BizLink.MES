using AntdUI;
using Azure;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.WinForms.Common.Helper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OscarClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace BizLink.MES.WinForms.Forms
{
    public partial class SapMaterialTransferLogForm : BaseForm
    {

        private readonly IMaterialTransferLogService _materialTransferLogService;
        private readonly Dictionary<string, ServiceEndpointSettings> _apiSettings;
        private readonly IMesApiClient _mesApiClient;
        private bool _isProgrammaticPageChange = false;

        public SapMaterialTransferLogForm(IMaterialTransferLogService materialTransferLogService, IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings, IMesApiClient mesApiClient)
        {
            InitializeComponent();
            InitializeTable();
            _materialTransferLogService = materialTransferLogService;
            _apiSettings = apiSettings.Value;
            _mesApiClient = mesApiClient;
        }

        private void SapMaterialTransferLogForm_Load(object sender, EventArgs e)
        {
            statusSelect.PlaceholderText = "请选择同步状态...";
            keywordInput.PlaceholderText = "请输入物料号或物料批次..";
            createOndatePickerRange.PlaceholderStart = "开始日期";
            createOndatePickerRange.PlaceholderEnd = "结束日期";


            statusSelect.Items.AddRange(new AntdUI.MenuItem[]
            {
                new AntdUI.MenuItem()
                {
                    Name = "", Text = ""
                },
                new AntdUI.MenuItem()
                {
                    Name = "-1", Text = "同步失败"
                },
                new AntdUI.MenuItem()
                {
                    Name = "0", Text = "未同步"
                },
                new AntdUI.MenuItem()
                {
                    Name = "1", Text = "已同步"
                }
            });

            statusSelect.SelectedIndex = 1;
        }

        private void InitializeTable()
        {

            sapTransferTable.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("Id", "Id", AntdUI.ColumnAlign.Center)
                {
                    Visible = false
                },
                new AntdUI.ColumnCheck("check").SetFixed(),
                new AntdUI.Column("TransferNo", "移库序号", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetSortOrder().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MaterialCode", "物料号", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("BatchCode", "物料批次", AntdUI.ColumnAlign.Center).SetFixed().SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Status", "状态", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, index) =>
                    {
                        switch (value as string)
                        {
                            case "-1":
                                return new AntdUI.CellTag("同步失败", AntdUI.TTypeMini.Error);
                            case "0":
                                return new AntdUI.CellTag("未同步", AntdUI.TTypeMini.Primary);

                            case "1":
                                return new AntdUI.CellTag("已同步", AntdUI.TTypeMini.Success);
                            default:
                                return null;
                        }
                    }
                }.SetFixed().SetLocalizationTitleID("Table.Column."),

               new AntdUI.Column("WorkOrderNo", "订单号", AntdUI.ColumnAlign.Center).SetWidth("auto").SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("PostingDate", "过账日期", AntdUI.ColumnAlign.Center).SetDisplayFormat("yyyy-MM-dd").SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("BaseUnit", "单位", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("FactoryCode", "工厂", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("FromLocationCode", "源库位", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("ToLocationCode", "目标库位", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Quantity", "移库数量", AntdUI.ColumnAlign.Center).SetWidth("auto").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("MovementType", "移动类型", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetWidth("auto").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("MessageType", "Sap返回类别", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetDefaultFilter().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("Message", "Sap返回信息", AntdUI.ColumnAlign.Center).SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("CreatedAt", "提交日期", AntdUI.ColumnAlign.Center).SetWidth("auto").SetSortOrder().SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),

                new AntdUI.Column("UpdateAt", "更新日期", AntdUI.ColumnAlign.Center).SetWidth("auto").SetSortOrder().SetDefaultFilter().SetDisplayFormat("yyyy-MM-dd HH:mm:ss").SetLocalizationTitleID("Table.Column."),
                new AntdUI.Column("TransferType", "事件类型", AntdUI.ColumnAlign.Center).SetDefaultFilter().SetWidth("auto").SetColAlign().SetLocalizationTitleID("Table.Column."),

               new AntdUI.Column("Btns", "操作", AntdUI.ColumnAlign.Center).SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Column."),

             };

        }

        private async Task<int> LoadData()
        {

            var pageIndex = paginationControl.Current;
            var pageSize = paginationControl.PageSize;
            var status = ((AntdUI.MenuItem)statusSelect.SelectedValue)?.Name ?? "";
            var keyword = keywordInput.Text.Trim();
            DateTime? createOnStart = null;
            DateTime? createOnEnd = null;
            if (createOndatePickerRange.Value != null && createOndatePickerRange.Value[0] != null)
            {
                createOnStart = createOndatePickerRange.Value[0];
            }

            if (createOndatePickerRange.Value != null && createOndatePickerRange.Value[1] != null)
            {
                createOnEnd = createOndatePickerRange.Value[1];
            }



            var result = await _materialTransferLogService.GetPagedListAsync(pageIndex, pageSize, keyword, status, createOnStart, createOnEnd);
            if (result != null && result.TotalCount > 0)
            {
                sapTransferTable.DataSource = result.Items.Select(x => new SapMaterialTransferLogView(x)).ToList();
                paginationControl.Total = result.TotalCount;
            }
            else
            {
                sapTransferTable.DataSource = null;
            }

            return result.TotalCount;
        }

        private async void queryButton_Click(object sender, EventArgs e)
        {
            try
            {
                queryButton.Enabled = false;
                queryButton.Loading = true;
                sapTransferTable.Enabled = false;
                _isProgrammaticPageChange = true;
                try
                {
                    // 这里的赋值可能会触发 ValueChanged 事件
                    // 但因为标志位为 true，事件内部会直接 return，不会执行查询
                    paginationControl.Current = 1;
                }
                finally
                {
                    // 2. 无论如何，必须关闭锁，恢复正常状态
                    _isProgrammaticPageChange = false;
                }

                var count = await LoadData();
                if (count == 0)
                {
                    throw new Exception("未查询到相关移库记录！");
                }
                AntdUI.Message.success(this, $"查询成功：共查询出{count}笔订单");
            }
            catch (Exception ex)
            {

                AntdUI.Message.error(this, $"查询出错：{ex.Message}");
            }
            finally
            {
                queryButton.Enabled = true;
                queryButton.Loading = false;
                sapTransferTable.Enabled = true;
            }
        }

        private async void sapTransferTable_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            try
            {
                if (e.Record is SapMaterialTransferLogView data)
                {
                    if (e.Btn.Id == "reentry")
                    {
                        if (data.Id == null)
                            throw new Exception("移库ID不能为空，无法重推！");
                        var transfer = await _materialTransferLogService.GetByIdAsync(data.Id.Value);
                        if (transfer.Status == "1")
                            throw new Exception("该记录已同步至SAP，无需重复重推送！");
                        List<int> transferIds = new List<int>() { data.Id.Value };
                        var sapRequestUrl = _apiSettings["MesApi"].Endpoints["LineStockReTransferToSAP"];
                        var response = await _mesApiClient.PostAsync<object, bool>(sapRequestUrl, transferIds);
                        if (response != null && response.IsSuccess)
                        {
                            AntdUI.Message.success(this, "重推成功！");
                            data.Status = "1";
                        }
                        else
                        {
                            AntdUI.Message.error(this, $"重推失败：{response.Message ?? "SAP返回信息为空"}！");
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                AntdUI.Message.error(this, $"重推出错：{ex.Message}");
            }
        }

        private async void batReentrybutton_Click(object sender, EventArgs e)
        {
            try
            {
                batReentrybutton.Enabled = false;
                batReentrybutton.Loading = true;
                sapTransferTable.Enabled = false;

                if (AntdUI.Modal.open(new AntdUI.Modal.Config(this.ParentForm, "提示", "即将批量进行重推，是否继续？", AntdUI.TType.Warn)) == DialogResult.OK)
                {
                    var data = sapTransferTable.DataSource as List<SapMaterialTransferLogView>;
                    if (data == null || data.Count == 0)
                        throw new Exception("当前无可用数据进行重推，请重新选择！");

                    // 2. 验证选中数量 (满足您的新需求)
                    var checkedItems = data.Where(x => x.check == true && x.Status != "1").ToList();
                    int totalCount = checkedItems.Count;
                    if (totalCount == 0)
                        throw new Exception("当前无可用数据进行重推，请重新选择！");
                    if (totalCount > 200)
                        throw new Exception($"选择的记录过多（{totalCount}条），请分批重推，每次不超过200条。");
                    //List<SapMaterialTransferLogView> transferIds = data.Where(x => x.check == true).ToList();

                    //List<int> successIds = new List<int>();
                    List<int> allCheckedIds = checkedItems.Select(item => (int)item.Id).ToList();
                    //Dictionary<string, string> failedItems = new Dictionary<string, string>();
                    progress.Visible = true;
                    progress.Value = 0.5F;
                    var sapRequestUrl = _apiSettings["MesApi"].Endpoints["LineStockReTransferToSAP"];
                    var response = await _mesApiClient.PostAsync<List<int>, bool>(sapRequestUrl, allCheckedIds);

                    progress.Value = 1F; // 完成
                                         // 6. 【核心修复】: 处理 *单个* 批量响应
                    if (response != null && response.IsSuccess)
                    {
                        // API 成功返回 (HTTP 200)，现在检查 API 内部的业务是否成功
                        if (response.Data) // 假设 Data=true 表示全部成功
                        {
                            AntdUI.Message.success(this.ParentForm, $"批量重推完成！共成功 {totalCount} 笔。");
                        }
                        else
                        {
                            // API 报告了业务失败（例如，部分成功）
                            AntdUI.Modal.open(this.ParentForm, "操作失败", response.Message ?? "API 报告业务失败，但未提供信息。");
                        }
                    }
                    else
                    {
                        // API 本身失败了 (HTTP 400/500) 或 response 为 null
                        string errorMsg = response?.Message ?? "API 调用失败，无响应。";

                        // 检查是否还是那个事务错误
                        //if (errorMsg.Contains("SqlTransaction has completed"))
                        //{
                        //    errorMsg = "API 内部事务错误，请联系管理员。（您已一次性提交，但 API 未能处理）";
                        //}

                        throw new Exception($"批量重推失败: {errorMsg}");
                    }
                    //foreach (var item in checkedItems)
                    //{
                    //    processedCount++;

                    //    try
                    //    {
                    //        var response = await _mesApiClient.PostAsync<object, bool>(sapRequestUrl, new List<int>() { (int)item.Id });

                    //        if (response == null || !response.IsSuccess)
                    //        {
                    //            failedItems.Add($"{item.Id}-{item.TransferNo}", response?.Message ?? "SAP返回信息为空");
                    //        }
                    //        else
                    //        {
                    //            successIds.Add((int)item.Id);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        // 捕获单次API调用本身的异常 (如网络超时)
                    //        failedItems.Add($"{item.Id}-{item.TransferNo}", $"API调用异常: {ex.Message}");
                    //    }
                    //    finally
                    //    {
                    //        progress.Value = (float)processedCount / checkedItems.Count();
                    //        await Task.Delay(500);
                    //    }



                    //}
                    //// 9. (关键) 循环结束后，提供统一的汇总报告
                    //if (failedItems.Count == 0 && successIds.Count > 0)
                    //{
                    //    // 全部成功
                    //    AntdUI.Message.success(this.ParentForm, $"批量重推完成！共成功 {successIds.Count} 笔。");
                    //}
                    //else if (successIds.Count > 0)
                    //{
                    //    // 部分成功
                    //    string warnMsg = $"重推部分完成：{successIds.Count} 笔成功，{failedItems.Count} 笔失败。\n失败单号: \n{string.Join(",\n ", failedItems.Keys.Take(5))}\n..."; // (只显示前5个失败的ID)
                    //    AntdUI.Modal.open(this.ParentForm, "部分成功", warnMsg); // 使用 Modal 显示更多信息
                    //}
                    //else
                    //{
                    //    // 全部失败
                    //    string errorMsg = $"批量重推全部失败！共 {failedItems.Count} 笔。\n首条错误: {failedItems.Values.FirstOrDefault()}";
                    //    AntdUI.Modal.open(this.ParentForm, "全部失败", errorMsg);
                    //}
                }


            }
            catch (Exception ex)
            {

                AntdUI.Message.error(this.ParentForm, $"重推出错：{ex.Message}");
            }
            finally
            {
                progress.Visible = false;
                progress.Value = 0;
                await LoadData();
                batReentrybutton.Enabled = true;
                batReentrybutton.Loading = false;
                sapTransferTable.Enabled = true;

            }
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (AntdUI.Modal.open(this.ParentForm, "提示", "是否导出当前列表中的数据源？", TType.Warn) == DialogResult.OK)
            {
                var data = sapTransferTable.DataSource as List<SapMaterialTransferLogView>;
                if (data != null)
                {
                    ExcelExportHelper.ExportToExcel(this.ParentForm, data, "SAP移库Log");
                }
                else
                {
                    AntdUI.Message.error(this, "当前无可用数据进行导出，请重新查询！");
                }
            }
        }

        private async void paginationControl_ValueChanged(object sender, PagePageEventArgs e)
        {
            if(_isProgrammaticPageChange)
                return; 
            await LoadData();
        }
    }

    public class SapMaterialTransferLogView : AntdUI.NotifyProperty
    {

        public SapMaterialTransferLogView(MaterialTransferLogDto dto)
        {
            _id = dto.Id;
            _transferNo = dto.TransferNo;
            _transferType = dto.TransferType;
            _postingDate = dto.PostingDate;
            _materialCode = dto.MaterialCode;
            _batchCode = dto.BatchCode;
            _baseUnit = dto.BaseUnit;
            _factoryCode = dto.FactoryCode;
            _workOrderNo = dto.WorkOrderNo;
            _fromLocationCode = dto.FromLocationCode;
            _toLocationCode = dto.ToLocationCode;
            _quantity = dto.Quantity;
            _status = dto.Status;
            _movementType = Enum.IsDefined(typeof(ConsumptionType), Convert.ToInt32(dto.MovementType)) ? ((ConsumptionType)Convert.ToInt32(dto.MovementType)).GetDescription() : "";
                
            _messageType = dto.MessageType;
            _message = SapErrorTranslator.ToChinese(dto.Message);
            _createdAt = dto.CreatedAt;
            _updateAt = dto.UpdatedAt;
            // 初始化按钮
            _btns = new AntdUI.CellLink[] {
                        new AntdUI.CellButton("reentry", "重推", AntdUI.TTypeMini.Primary)
            };
        }
        bool _check = false;
        public bool check
        {
            get => _check;
            set
            {
                if (_check == value)
                    return;
                _check = value;
                OnPropertyChanged();
            }
        }
        int? _id;
        public int? Id
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

        string? _transferNo;
        public string? TransferNo
        {
            get => _transferNo;
            set
            {
                if (_transferNo == value)
                    return;
                _transferNo = value;
                OnPropertyChanged();
            }
        }

        string? _workOrderNo;
        public string? WorkOrderNo
        {
            get => _workOrderNo;
            set
            {
                if (_workOrderNo == value)
                    return;
                _workOrderNo = value;
                OnPropertyChanged();
            }
        }

        string? _transferType;
        public string? TransferType
        {
            get => _transferType;
            set
            {
                if (_transferType == value)
                    return;
                _transferType = value;
                OnPropertyChanged();
            }
        }

        DateTime? _postingDate;
        public DateTime? PostingDate
        {
            get => _postingDate;
            set
            {
                if (_postingDate == value)
                    return;
                _postingDate = value;
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

        string? _baseUnit;
        public string? BaseUnit
        {
            get => _baseUnit;
            set
            {
                if (_baseUnit == value)
                    return;
                _baseUnit = value;
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

        string? _fromLocationCode;
        public string? FromLocationCode
        {
            get => _fromLocationCode;
            set
            {
                if (_fromLocationCode == value)
                    return;
                _fromLocationCode = value;
                OnPropertyChanged();
            }
        }

        string? _toLocationCode;
        public string? ToLocationCode
        {
            get => _toLocationCode;
            set
            {
                if (_toLocationCode == value)
                    return;
                _toLocationCode = value;
                OnPropertyChanged();
            }
        }

        decimal? _quantity;
        public decimal? Quantity
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

        string? _status;
        public string? Status
        {
            get => _status;
            set
            {
                if (_status == value)
                    return;
                _status = value;
                OnPropertyChanged();
            }
        }

        string? _movementType;
        public string? MovementType
        {
            get => _movementType;
            set
            {
                if (_movementType == value)
                    return;
                _movementType = value;
                OnPropertyChanged();
            }
        }

        string? _messageType;
        public string? MessageType
        {
            get => _messageType;
            set
            {
                if (_messageType == value)
                    return;
                _messageType = value;
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

        DateTime? _createdAt;
        public DateTime? CreatedAt
        {
            get => _createdAt;
            set
            {
                if (_createdAt == value)
                    return;
                _createdAt = value;
                OnPropertyChanged();
            }
        }

        DateTime? _updateAt;
        public DateTime? UpdateAt
        {
            get => _updateAt;
            set
            {
                if (_updateAt == value)
                    return;
                _updateAt = value;
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
