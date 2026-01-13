using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.MAUI.Model;
using BizLink.MES.Shared.Services;
using BizLink.MES.WinForms.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.MAUI.ViewModel
{
    public partial class LineStockTransferViewModel : ObservableObject
    {
        private readonly IMesApiClient _apiClient;
        private readonly IWarehouseLocationService _warehouseLocationService;
        private readonly MesApiSettings _apiSettings;


        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SubmitCommand))]
        private string _targetLocation;

        [ObservableProperty]
        private string _materialTag; // 绑定物料标签扫描框

        public ObservableCollection<MaterialTagInfo> ScannedTags { get; } = new();

        public LineStockTransferViewModel(IMesApiClient apiClient, IOptions<MesApiSettings> apiSettings)
        {
            _apiClient = apiClient;
            _apiSettings  = apiSettings.Value;
            ScannedTags.CollectionChanged += (s, e) => SubmitCommand.NotifyCanExecuteChanged();
        }

        [RelayCommand]
        private async void ProcessMaterialTagScan()
        {
            if (string.IsNullOrWhiteSpace(_materialTag))
                return;

            // 检查标签是否重复扫描
            if (ScannedTags.Any(tag => tag.BarCode == _materialTag))
            {
                await Shell.Current.DisplayAlert("重复扫描", $"标签 '{_materialTag}' 已经被扫描过了。", "确定");
                MaterialTag = string.Empty; // 清空输入
                return;
            }

            var requestUrl = _apiSettings.Endpoints["GetLineStockInfoByBarcode"];
            requestUrl = $"{requestUrl}?factoryid=2&barcode={_materialTag.TrimEnd()}";
            var response = await _apiClient.GetAsync<MaterialTagInfo>(requestUrl);
            if (!response.IsSuccess)
            {
                await Shell.Current.DisplayAlert("扫描出错", $"标签 '{_materialTag.TrimEnd()}' 扫描出错，\r\n错误信息：{response.Message}!", "确定");
                MaterialTag = string.Empty; // 清空输入
                return;
            }
            else
            {
                ScannedTags.Insert(0, response.Data);
                MaterialTag = string.Empty; // 清空输入
            }



        }

        private bool CanSubmit()
        {
            // 只有当库位已扫描并且至少有一个物料标签时，才允许提交
            return !string.IsNullOrWhiteSpace(_targetLocation) && ScannedTags.Any();

        }

        [RelayCommand(CanExecute = nameof(CanSubmit))]
        private async Task Submit()
        {
            // 检查是否已扫描库位
            if (string.IsNullOrWhiteSpace(_targetLocation))
            {
                await Shell.Current.DisplayAlert("提示", "请先扫描库位！", "确定");
                MaterialTag = string.Empty; // 清空输入
                return;
            }

            var requestDto = new TransferStockRequestDto
            {
                LocationCode = _targetLocation.TrimEnd(),
                BarCodes = ScannedTags.Select(tag => tag.BarCode).ToList(),
            };

            try
            {
                var requestUrl = _apiSettings.Endpoints["LineStockTransfer"];

                var response = await _apiClient.PostAsync<TransferStockRequestDto, bool>(requestUrl, requestDto);

                if (response.IsSuccess)
                {
                    await Shell.Current.DisplayAlert("成功", "移库操作完成！", "确定");
                    Clear();
                }
                else
                {
                    var errorMessage = response.Message;
                    await Shell.Current.DisplayAlert("失败", $"错误: {errorMessage}", "确定");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("网络错误", $"无法连接到服务器: {ex.Message}", "确定");
            }
        }
        [RelayCommand]
        private void Clear()
        {
            //TargetLocation = string.Empty;
            ScannedTags.Clear();
            MaterialTag = string.Empty;
        }
    }
}
