using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services.ApiServices
{
    public class LineStockApiService : ILineStockApiService
    {
        private readonly IRawLinesideStockService _rawLinesideStockService;
        private readonly IRawLinesideStockLogService _rawLinesideStockLogService;
        private readonly IWarehouseLocationService _warehouseLocationService;
        private readonly IParameterGroupService _parameterGroupService;
        private readonly IMaterialTransferLogService _materialTransferLogService;
        private readonly IMaterialTransferApiService _materialTransferApiService;
        private readonly IFactoryService _factoryService;

        public LineStockApiService(
            IRawLinesideStockService rawLinesideStockService,
            IRawLinesideStockLogService rawLinesideStockLogService,
            IWarehouseLocationService warehouseLocationService,
            IParameterGroupService parameterGroupService,
            IMaterialTransferLogService materialTransferLogService,
            IMaterialTransferApiService materialTransferApiService,
            IFactoryService factoryService)
        {
            _rawLinesideStockService = rawLinesideStockService;
            _rawLinesideStockLogService = rawLinesideStockLogService;
            _warehouseLocationService = warehouseLocationService;
            _parameterGroupService = parameterGroupService;
            _materialTransferLogService = materialTransferLogService;
            _materialTransferApiService = materialTransferApiService;
            _factoryService = factoryService;
        }

        public async Task<RawLinesideStockDto> GetStockByBarcodeAsync(int factoryId, string barcode)
        {
            var stock = await _rawLinesideStockService.GetByBarCodeAsync(factoryId, barcode);

            if (stock == null)
            {
                throw new Exception("未查询到标签对应的库存信息，请检查库存！");
            }

            return new RawLinesideStockDto
            {
                MaterialCode = stock.MaterialCode,
                BatchCode = stock.BatchCode,
                BarCode = stock.BarCode,
                Quantity = stock.LastQuantity, // 注意：原代码是 LastQuantity，请确认是否应为 Quantity
                LocationCode = stock.LocationCode
            };
        }

        public async Task<RawLinesideStockDto> GetNeedSyncStockBarcodeAsync(int factoryId, string barcode)
        {
            var stock = await _rawLinesideStockService.GetByBarCodeAsync(factoryId, barcode);

            if (stock == null)
            {
                throw new Exception("未查询到标签对应的库存信息，请检查库存！");
            }

            if (stock.SapStatus == "2")
            {
                throw new Exception("当前标签已同步至SAP，无需再次同步！");
            }

            return stock;
        }

        public async Task<(bool, string)> TransferStockAsync(TransferStockRequest request)
        {
            // 1. 基础校验
            if (string.IsNullOrWhiteSpace(request.LocationCode))
                throw new Exception("未扫描目标库存信息，请重新扫描！");

            if (request.BarCodes == null || !request.BarCodes.Any())
                throw new Exception("未扫描物料标签，请重新扫描！");

            // 2. 验证目标库位
            var allLocations = await _warehouseLocationService.GetAllBinAsync();
            var targetLocation = allLocations.FirstOrDefault(x => x.Code == request.LocationCode);

            if (targetLocation == null)
                throw new Exception("目标库位无效，请重新扫描");

            StringBuilder messageBuilder = new StringBuilder();
            bool hasAnySuccess = false; // 标记是否至少有一条成功

            // 3. 循环处理
            foreach (var barcode in request.BarCodes)
            {
                // 注意：原代码中 factoryId 硬编码为 2，这里建议确认是否需要改为动态传参
                // 假设 GetByBarCodeAsync 第一个参数是 FactoryId
                var stockTemp = await _rawLinesideStockService.GetByBarCodeAsync(2, barcode);

                if (stockTemp == null || stockTemp.Quantity <= 0)
                {
                    messageBuilder.AppendLine($"未查询到标签{barcode}的有效库存，无法移库！");
                    continue;
                }

                // 4. 更新库存位置
                var stockUpdate = new RawLinesideStockUpdateDto()
                {
                    Id = stockTemp.Id,
                    LocationId = targetLocation.Id,
                    LocationCode = targetLocation.Code,
                    LocationDesc = targetLocation.Name,
                };

                bool updateResult = await _rawLinesideStockService.UpdateAsync(stockUpdate);

                if (!updateResult)
                {
                    messageBuilder.AppendLine($"标签{barcode}移库失败，请重新操作！");
                    continue;
                }

                // 5. 记录日志 (移出 + 移入)
                // 建议：此处最好包裹在 TransactionScope 中以保证数据一致性
                hasAnySuccess = true;

                // Log: TransferOut
                await _rawLinesideStockLogService.CreateAsync(new RawLinesideStockLogCreateDto()
                {
                    RawLinesideStockId = stockTemp.Id,
                    OperationType = StockOperationType.TransferOut,
                    InOutStatus = InOutStatus.Out,
                    ChangeQuantity = (decimal)stockTemp.Quantity,
                    QuantityBefore = (decimal)stockTemp.Quantity,
                    QuantityAfter = 0, // 移出后原位置逻辑上归零（虽然是同一条记录更新，但Log表示流转）
                    MaterialCode = stockTemp.MaterialCode,
                    BarCode = stockTemp.BarCode,
                    BatchCode = stockTemp.BatchCode,
                    LocationId = (int)stockTemp.LocationId, // 原库位ID
                    LocationCode = stockTemp.LocationCode, // 原库位Code
                    Remark = StockOperationType.TransferOut.GetDescription()
                });

                // Log: TransferIn
                await _rawLinesideStockLogService.CreateAsync(new RawLinesideStockLogCreateDto()
                {
                    RawLinesideStockId = stockTemp.Id,
                    OperationType = StockOperationType.TransferIn,
                    InOutStatus = InOutStatus.In,
                    ChangeQuantity = (decimal)stockTemp.Quantity,
                    QuantityBefore = 0,
                    QuantityAfter = (decimal)stockTemp.Quantity,
                    MaterialCode = stockTemp.MaterialCode,
                    BarCode = stockTemp.BarCode,
                    BatchCode = stockTemp.BatchCode,
                    LocationId = targetLocation.Id, // 新库位ID
                    LocationCode = targetLocation.Code, // 新库位Code
                    Remark = StockOperationType.TransferIn.GetDescription()
                });
            }

            return (hasAnySuccess, messageBuilder.ToString());
        }

        public async Task<(bool, string)> SyncStockToSapAsync(TransferStockRequest request)
        {
            if (request.BarCodes == null || !request.BarCodes.Any())
                throw new Exception("未扫描物料标签，请重新扫描！");
            var stockList = await _rawLinesideStockService.GetByBarCodeAsync(2, request.BarCodes);

            var factoryIds = stockList.Select(s => s.FactoryId).Distinct().ToList();
            if(factoryIds.Count() > 1)
                throw new Exception("所选标签对应不同工厂库存，无法批量同步至SAP，请单独同步！");
            var factory = await _factoryService.GetByIdAsync(factoryIds.First());
            if (factory == null)
                throw new Exception("未查询到对应工厂信息，无法同步至SAP！");

            var notExistsBarcodes = request.BarCodes.Except(stockList.Select(s => s.BarCode)).ToList();
            if (notExistsBarcodes.Any())
            {
                throw new Exception($"以下标签未查询到库存信息，无法同步至SAP：\r\n{string.Join(",\r\n", notExistsBarcodes)}");
            }
            var syncedStocks = request.BarCodes.Where(b => stockList.Where(x => x.SapStatus == "2").Select(s => s.BarCode).Contains(b)).ToList();
            if (syncedStocks.Any())
            {
                throw new Exception($"以下标签已同步至SAP，无需再次同步：\r\n{string.Join(",\r\n", syncedStocks)}");
            }

            var parameterGroup = await _parameterGroupService.GetGroupWithItemsAsync("CN11SAPStockLocation");
            string cableLineLocation = parameterGroup?.Items.FirstOrDefault(x => x.Key == "CableRawLineStock")?.Value ?? "2200";

            //判断当前是否已存在SAP同步失败记录
            var needSyncIds = new List<int>();
            foreach (var stock in stockList)
            {
                var existsLog = await _materialTransferLogService.GetListByStockIdAsync(stock.Id);
                if (existsLog.Any(l => l.MaterialCode == stock.MaterialCode && l.ToLocationCode == cableLineLocation && l.BatchCode == stock.BatchCode && l.Status != "1"))
                {
                    continue;
                }
                needSyncIds.Add(stock.Id);
            }
            var result = false;
            var message = string.Empty;
            if (needSyncIds.Any())
            {
                var transferRequest = new TransferSapRequest
                {
                    FactoryCode = factory.FactoryCode,
                    TransferType = MaterialTransferType.GoodsIssue,
                    ConsumptionType = ConsumptionType.MaterialTransfer,
                    Stocks = stockList.Where(s => needSyncIds.Contains(s.Id)).Select(stock => new TransferStock()
                    {
                        StockId = stock.Id,
                        MaterialCode = stock.MaterialCode,
                        BatchCode = stock.BatchCode,
                        Quantity = (decimal)stock.LastQuantity,
                        BaseUnit = stock.BaseUnit
                    }).ToList(),
                    FromLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == "SAPRawMtrStock")?.Value ?? "1100",
                    ToLocation = parameterGroup.Items.FirstOrDefault(x => x.Key == "CableRawLineStock")?.Value ?? "2200"
                };
                (result, message) = await _materialTransferApiService.TransferMaterialToSAPAsync(transferRequest);
            }
            else
            {
                result = true;
            }
            if (result)
                message = "标签收货确认完成！";
            return (result, message);
        }

        public async Task<List<RawLinesideStockDto>> GetListByMaterialCodeAsync(int factoryId, string materialCode)
        {
            var result =  await _rawLinesideStockService.GetListByMaterialCodeAsync(factoryId, materialCode);
            return result.Where(x => x.LastQuantity > 0).ToList();
        }
    }
}
