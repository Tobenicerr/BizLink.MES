using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Helper;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Enums;
using Dm.util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class MaterialTransferApiService : IMaterialTransferApiService
    {
        private readonly IParameterGroupService _parameterGroupService;
        private readonly IMaterialTransferLogService _materialTransferLogService;
        private readonly ISerialHelperService _serialHelperService;
        private readonly ISapRfcService _sapRfcService;
        private readonly IUnitOfWork _unitOfWork; // 3. 注入 IUnitOfWork
        private readonly IRawLinesideStockService _rawLinesideStockService;

        public MaterialTransferApiService(
            IParameterGroupService parameterGroupService,
            IMaterialTransferLogService materialTransferLogService,
            ISerialHelperService serialHelperService,
             IUnitOfWork unitOfWork, // 3. 注入 IUnitOfWork
            ISapRfcService sapRfcService,
            IRawLinesideStockService rawLinesideStockService)
        {
            _parameterGroupService = parameterGroupService;
            _materialTransferLogService = materialTransferLogService;
            _serialHelperService = serialHelperService;
            _sapRfcService = sapRfcService;
            _unitOfWork = unitOfWork; // 3. 注入 IUnitOfWork
            _rawLinesideStockService = rawLinesideStockService;
        }

        public async Task<(bool, string)> AdjustInventoryInSAPAsync(TransferSapRequest request)
        {
            var stocksToTransfer = request.Stocks.Where(x => !string.IsNullOrWhiteSpace(x.BatchCode)).ToList();

            if (stocksToTransfer.Count == 0)
            {
                return (false, "待移库信息为空(无有效批次)，无法移库");
            }

            // --- 1. 准备数据 ---
            //var TransferNo = ;
            var currentDate = DateTime.Today;
            var TransferNo = _serialHelperService.GenerateNext($"{request.FactoryCode}MaterialTransferToSAPSerial");
            var createDtos = stocksToTransfer.Select(stock => new MaterialTransferLogCreateDto()
            {
                TransferNo = TransferNo,
                PostingDate = currentDate,
                DocumentDate = currentDate,
                FactoryCode = request.FactoryCode,
                MaterialCode = stock.MaterialCode.StartsWith('E') ? stock.MaterialCode : stock.MaterialCode.PadLeft(18, '0'),
                FromLocationCode = request.FromLocation,
                BatchCode = stock.BatchCode,
                Quantity = stock.Quantity,
                BaseUnit = stock.BaseUnit,
                ReceivingMaterialCode = stock.MaterialCode.StartsWith('E') ? stock.MaterialCode : stock.MaterialCode.PadLeft(18, '0'),
                ReceivingBatchCode = stock.BatchCodeRecive ?? stock.BatchCode,
                ToFactoryCode = request.FactoryCode,
                ToLocationCode = request.ToLocation,
                CreatedBy = "INTERFACE",
            }).ToList();

            List<MaterialTransferLogDto> transferLogs = null;

            // --- 2. 【短事务区间 A】创建初始日志并提交 ---
            // 目的：无论后续 SAP 成功与否，先在数据库占位，避免“只调了接口没留记录”的情况
            var transferIds = await _materialTransferLogService.CreateBatchAsync(createDtos);
            transferLogs = await _materialTransferLogService.GetListByIdsAsync(transferIds);

            // --- 3. 【无事务区间】调用 SAP 接口 ---
            // 这是一个耗时操作，现在它不会持有任何数据库锁
            List<MaterialTransferLogDto> sapResults = new List<MaterialTransferLogDto>();
            try
            {
                // 调用外部服务
                sapResults = await _sapRfcService.RawMaterialInventoryAdjustmentAsync(transferLogs);

                if (sapResults == null || sapResults.Count == 0)
                {
                    // 异常防御：接口返回了空，手动标记为错误
                    sapResults = transferLogs.Select(log => {
                        log.MessageType = "E";
                        log.Message = "调用SAP接口返回空结果";
                        return log;
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // 捕获 SAP 调用本身的异常（如网络不通），不让程序崩溃，而是去更新数据库状态为失败
                sapResults = transferLogs.Select(log =>
                {
                    log.Message = $"SAP接口调用异常: {ex.Message}";
                    log.MessageType = "E";
                    return log;
                }).ToList();
            }

            // --- 4. 【短事务区间 B】更新日志结果 ---
            // 重新开启一个短事务，将 SAP 的结果回写到数据库
            var updateDtos = sapResults.Select(item => new MaterialTransferLogUpdateDto
            {
                Id = item.Id,
                Message = item.Message,
                MessageType = item.MessageType,
                Status = item.MessageType != null && item.MessageType.ToUpper() == "S" ? "1" : "-1",
                UpdatedBy = request.EmployeeId,
                UpdatedAt = DateTime.Now
            }).ToList();

            await _materialTransferLogService.UpdateListAsync(updateDtos);

            var failedMessages = sapResults.Where(x => x.MessageType.ToUpper() != "S").ToList();
            if (failedMessages.Count == 0)
            {
                return (true, "库存转移成功！");
            }
            else
            {
                var errorMessages = string.Join("; ", failedMessages.Select(x => x.Message));
                // 即使部分失败，整体流程也算结束了，返回 True 表示“调用动作完成”，通过 message 提示用户失败项
                return (true, $"库存转移操作完成，但存在失败项：{errorMessages}");
            }
        }

        /// <summary>
        /// 【核心更改】优化后的重传逻辑：DB操作与 SAP 远程调用分离
        /// 1. 验证数据
        /// 2. 获取 SAP 库存 (无事务)
        /// 3. 计算拆分逻辑 (内存)
        /// 4. 保存拆分结果 (短事务 A)
        /// 5. 调用 SAP 移库接口 (无事务)
        /// 6. 保存最终结果 (短事务 B)
        /// </summary>
        public async Task<(bool, string)> RetransferLogsToSAPAsync(List<int> transferIds)
        {
            // 1. 验证和获取初始数据 (只读，无事务)
            var parameter = (await _parameterGroupService.GetGroupWithItemsAsync("TransferSAP"))?.Items.FirstOrDefault(x => x.Key == "IsEnabled");
            if (parameter != null && !bool.Parse(parameter.Value))
            {
                return (true, "库存转移被禁用，停止操作。");
            }

            var materialTransferLogDtos = await _materialTransferLogService.GetListByIdsAsync(transferIds);
            if (materialTransferLogDtos == null || materialTransferLogDtos.Count == 0)
            {
                throw new Exception("未查询到待重传的库存转移记录，无法重传！");
            }

            var factoryCodeGrouping = materialTransferLogDtos.GroupBy(x => x.FactoryCode).Select(g => g.Key).ToList();
            if (factoryCodeGrouping.Count > 1)
            {
                throw new Exception("待重传的库存转移记录包含多个工厂代码，无法重传！");
            }
            string factoryCode = factoryCodeGrouping.FirstOrDefault();
            if (factoryCode == null)
            {
                throw new Exception("未查询到待重传记录中的工厂信息，无法重传！");
            }
            var locationGroup = await _parameterGroupService.GetGroupWithItemsAsync("CN11SAPStockLocation");
            //string tolocation = locationGroup?.Items.FirstOrDefault(x => x.Key == "SAPLineStock")?.Value ?? "2100";
            string cableLineLocation = locationGroup?.Items.FirstOrDefault(x => x.Key == "CableRawLineStock")?.Value ?? "2200";

            #region 废弃：不通过SAP库存去补足批次
            //// 2. 获取 SAP 原始库存 (耗时操作，移出事务)

            //List<SapRawMaterialStockDto> sapStocks = new List<SapRawMaterialStockDto>();
            //try
            //{
            //    //不查询断线物料的sap库存
            //    var stockQueryParams = materialTransferLogDtos
            //        .Where(x => x.FromLocationCode != cableLineLocation && x.ToLocationCode != cableLineLocation)
            //        .GroupBy(s => new { s.MaterialCode, s.FromLocationCode })
            //        .Select(x => new SapRawMaterialStockDto()
            //        {
            //            MaterialCode = x.Key.MaterialCode,
            //            FactoryCode = factoryCode,
            //            LocationCode = x.Key.FromLocationCode,
            //        }).ToList();

            //    if (stockQueryParams.Any())
            //    {
            //        var sapStockResult = await _sapRfcService.GetRawMaterialStockFromSapAsync(stockQueryParams);
            //        sapStocks = sapStockResult.Select(x => new SapRawMaterialStockDto()
            //        {
            //            MaterialCode = x.MaterialCode.TrimStart('0'),
            //            FactoryCode = x.FactoryCode,
            //            LocationCode = x.LocationCode,
            //            BatchCode = x.BatchCode,
            //            Quantity = x.Quantity
            //        }).ToList();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return (false, $"获取SAP库存失败：{ex.Message}");
            //}
            #endregion

            // 3. 逻辑计算 & 准备数据 (内存操作)
            var transferUpdate = new List<MaterialTransferLogDto>(); // 需更新的记录
            var transferCreate = new List<MaterialTransferLogCreateDto>(); // 需创建的新记录
            var waitTransferIds = new List<int>(); // 最终需发送给 SAP 的 ID 集合

            // 预加载 TransferNo 用于检查重复 (虽然这里只是逻辑判断)
            var uniqueTransferNos = materialTransferLogDtos
                .Select(l => l.TransferNo)
                .Where(tn => !string.IsNullOrEmpty(tn))
                .Distinct()
                .ToList();

            // 预加载同 TransferNo 的记录，用于判断是否需要生成新单号
            Dictionary<string, List<MaterialTransferLogDto>> logsByTransferNo = new Dictionary<string, List<MaterialTransferLogDto>>();
            if (uniqueTransferNos.Any())
            {
                var allLogs = await _materialTransferLogService.GetByTransferNoAsync(uniqueTransferNos);
                logsByTransferNo = allLogs.GroupBy(l => l.TransferNo).ToDictionary(g => g.Key, g => g.ToList());
            }

            foreach (var transferitem in materialTransferLogDtos)
            {
                // 检查是否需要生成新单号 (如果该单号下有多条记录，重传当前这条时，给它个新号，避免混淆)
                if (logsByTransferNo.TryGetValue(transferitem.TransferNo, out var siblings) && siblings.Count > 1)
                {
                    transferitem.TransferNo = _serialHelperService.GenerateNext($"{factoryCode}MaterialTransferToSAPSerial");
                    if (!transferUpdate.Any(u => u.Id == transferitem.Id))
                        transferUpdate.Add(transferitem);
                }

                waitTransferIds.Add(transferitem.Id);

                #region 废弃：不通过SAP库存去补足批次
                // 匹配 SAP 库存逻辑
                //var materialStocks = sapStocks.Where(x => x.MaterialCode == transferitem.MaterialCode).ToList();
                //var materialSummery = materialStocks.Sum(x => x.Quantity);
                //var requiredQuantity = transferitem.Quantity;

                //// 逻辑：库存充足 (>=) 才进行批次拆分逻辑；库存不足则不做拆分直接发(原逻辑)
                //if (materialSummery >= requiredQuantity)
                //{
                //    decimal originalRequired = (decimal)requiredQuantity;
                //    decimal takenFromMatchingBatch = 0;

                //    // A. 优先消耗匹配的批次
                //    var matchingBatches = materialStocks.Where(x => x.BatchCode == transferitem.BatchCode).ToList();
                //    foreach (var batchitem in matchingBatches)
                //    {
                //        decimal takeQuantity = Math.Min((decimal)requiredQuantity, (decimal)batchitem.Quantity);
                //        requiredQuantity -= takeQuantity;
                //        takenFromMatchingBatch += takeQuantity;
                //        batchitem.Quantity -= takeQuantity; // 内存扣减
                //        if (requiredQuantity == 0)
                //            break;
                //    }

                //    // B. 不足部分，FIFO 消耗其他批次 -> 产生拆分(Create)
                //    if (requiredQuantity > 0)
                //    {
                //        var otherBatches = materialStocks
                //            .Where(x => x.BatchCode != transferitem.BatchCode && x.Quantity > 0)
                //            .OrderBy(x => x.BatchCode)
                //            .ToList();

                //        foreach (var batchitem in otherBatches)
                //        {
                //            decimal takeQuantity = Math.Min((decimal)requiredQuantity, (decimal)batchitem.Quantity);
                //            requiredQuantity -= takeQuantity;
                //            batchitem.Quantity -= takeQuantity;

                //            // 创建拆分记录
                //            var createDto = CreateDtoFromLog(transferitem);
                //            createDto.Quantity = takeQuantity;
                //            createDto.BatchCode = batchitem.BatchCode;
                //            // 为新拆分的记录生成新单号
                //            createDto.TransferNo = _serialHelperService.GenerateNext($"{factoryCode}MaterialTransferToSAPSerial");

                //            transferCreate.Add(createDto);

                //            if (requiredQuantity == 0)
                //                break;
                //        }
                //    }
                //    else
                //    {
                //        // 完全由匹配批次满足，不需要拆分，但需要加入等待列表
                //        waitTransferIds.Add(transferitem.Id);
                //    }

                //    // C. 更新原记录 (可能数量减少了，或者仅仅是加入了更新列表)
                //    var existingUpdate = transferUpdate.FirstOrDefault(u => u.Id == transferitem.Id);
                //    if (existingUpdate != null)
                //    {
                //        existingUpdate.Quantity = takenFromMatchingBatch;
                //    }
                //    else
                //    {
                //        // 创建一个副本用于更新
                //        var updateDto = new MaterialTransferLogDto
                //        {
                //            Id = transferitem.Id,
                //            TransferNo = transferitem.TransferNo,
                //            Quantity = takenFromMatchingBatch,
                //            Status = transferitem.Status,
                //            MessageType = transferitem.MessageType
                //        };
                //        transferUpdate.Add(updateDto);
                //    }
                //}
                //else
                //{
                //    // 库存不足，原样发送
                //    waitTransferIds.Add(transferitem.Id);
                //}
                #endregion
            }

            // 4. 【短事务 A】 保存拆分结果
            // 4. 【DB 操作区域】 保存拆分结果
            // 注意：已移除 _unitOfWork.BeginTransactionAsync() 和 CommitAsync()
            // 原因：_materialTransferLogService 内部方法已包含事务提交逻辑，外部再次提交会导致 "No active transaction" 错误。
            try
            {
                // A. 处理 Updates
                if (transferUpdate.Any())
                {
                    var updateDtos = transferUpdate.Select(x => new MaterialTransferLogUpdateDto()
                    {
                        Id = x.Id,
                        Quantity = x.Quantity,
                        TransferNo = x.TransferNo,
                        Status = x.Quantity == 0 ? "1" : x.Status,
                        MessageType = x.Quantity == 0 ? "S" : x.MessageType,
                        UpdatedAt = DateTime.Now
                    }).ToList();

                    await _materialTransferLogService.UpdateListAsync(updateDtos);

                    var validUpdates = transferUpdate.Where(x => x.Quantity > 0).Select(x => x.Id);
                    waitTransferIds.AddRange(validUpdates);
                }

                // B. 处理 Creates
                if (transferCreate.Any())
                {
                    foreach (var create in transferCreate)
                    {
                        if (string.IsNullOrEmpty(create.TransferNo))
                        {
                            create.TransferNo = _serialHelperService.GenerateNext($"{create.FactoryCode}MaterialTransferToSAPSerial");
                        }
                    }

                    var newLogIds = await _materialTransferLogService.CreateBatchAsync(transferCreate);

                    if (newLogIds != null)
                    {
                        waitTransferIds.AddRange(newLogIds);
                    }
                }
            }
            catch (Exception ex)
            {
                // 无法回滚已提交的事务，直接返回错误
                return (false, $"保存拆分记录失败：{ex.Message}");
            }

            // 5. 调用 SAP 接口 (无事务)
            List<MaterialTransferLogDto> sapResults = new List<MaterialTransferLogDto>();
            var finalIds = waitTransferIds.Distinct().ToList();

            if (finalIds.Count > 0)
            {
                try
                {
                    // 重新查询最新的 DTO，确保数据一致性
                    var logsToSend = await _materialTransferLogService.GetListByIdsAsync(finalIds);
                    sapResults = await _sapRfcService.MaterialStockTransferToSAPAsync(logsToSend);

                    if (sapResults == null || sapResults.Count == 0)
                    {
                        return (false, "调用SAP接口失败，返回结果为空");
                    }
                }
                catch (Exception ex)
                {
                    // 记录 SAP 调用失败，但不回滚之前的拆分，因为拆分是合理的业务动作
                    return (false, $"调用SAP接口异常：{ex.Message}");
                }
            }
            else
            {
                return (true, "没有需要发送到 SAP 的有效记录（可能全部被拆分或数量为0）。");
            }

            // 6. 【短事务 B】 保存 SAP 结果
            await _unitOfWork.BeginTransactionAsync();
            bool allSuccess = true;
            List<string> errorMessages = new List<string>();
            try
            {
                var logsToUpdate = new List<MaterialTransferLogUpdateDto>();

                foreach (var item in sapResults)
                {
                    bool success = item.MessageType.ToUpper() == "S" ||
                                   (!string.IsNullOrEmpty(item.Message) && item.Message.Contains($"WMS Number:{item.TransferNo} is existed and successful!"));

                    if (!success)
                    {
                        allSuccess = false;
                        errorMessages.Add($"({item.MaterialCode}/{item.BatchCode}): {item.Message}");
                    }
                    else
                    {
                        //电缆发料更新线边库状态
                        if (item.ToLocationCode == cableLineLocation && item.FromStockId != null)
                        {
                            await _rawLinesideStockService.UpdateAsync(new RawLinesideStockUpdateDto()
                            {
                                Id = (int)item.FromStockId,
                                SapStatus = "2"
                            });
                        }
                    }

                        logsToUpdate.Add(new MaterialTransferLogUpdateDto
                        {
                            Id = item.Id,
                            Message = item.Message,
                            MessageType = success ? "S" : item.MessageType,
                            Status = success ? "1" : "-1",
                            UpdatedAt = DateTime.Now
                        });
                }

                if (logsToUpdate.Any())
                {
                    await _materialTransferLogService.UpdateListAsync(logsToUpdate);
                }

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                try
                {
                    await _unitOfWork.RollbackAsync();
                }
                catch { }
                return (false, $"保存SAP返回结果失败：{ex.Message}");
            }

            // 7. 返回最终结果
            if (allSuccess)
            {
                return (true, "库存转移成功！");
            }
            else
            {
                return (false, $"库存转移部分失败：{string.Join("; ", errorMessages)}");
            }
        }


        /// <summary>
        /// 【核心更改】优化后的移库逻辑：DB操作与 SAP 远程调用分离
        /// 防止 SAP 调用超时导致数据库事务锁死
        /// </summary>
        public async Task<(bool, string)> TransferMaterialToSAPAsync(TransferSapRequest request)
        {
            var stocksToTransfer = request.Stocks.Where(x => !string.IsNullOrWhiteSpace(x.BatchCode)).ToList();

            if (stocksToTransfer.Count == 0)
            {
                return (false, "待移库信息为空(无有效批次)，无法移库");
            }
            if (!Enum.IsDefined(typeof(ConsumptionType), request.ConsumptionType))
                return (false, "货物移动类型不存在，无法移库");

            if (!Enum.IsDefined(typeof(MaterialTransferType), request.TransferType))
                return (false, "事件类型不存在，无法移库");

            string movementReason = string.Empty;
            string remark = string.Empty;
            if (request.TransferType == MaterialTransferType.RawMaterialScrapAgainstOrder && (request.ConsumptionType == ConsumptionType.Scrap || request.ConsumptionType == ConsumptionType.ScrapReversal))
            {
                if (request.WorkOrderId == null || request.WorkOrderId == 0)
                    return (false, "原材料报废必须关联工单，无法移库");
                if (string.IsNullOrWhiteSpace(request.MovementReason))
                    return (false, "原材料报废必须填写移动原因，无法移库");

                movementReason = request.MovementReason;
            }

            if (request.ConsumptionType == ConsumptionType.AdjustStockGain || request.ConsumptionType == ConsumptionType.AdjustStockLoss)
            {
                if (string.IsNullOrWhiteSpace(request.MovementReason))
                    return (false, "盘盈盘亏必须填写移动原因，无法移库");
                remark = request.MovementReason;
            }

            if (request.ConsumptionType == ConsumptionType.GoodsIssuetoCostCenter || request.ConsumptionType == ConsumptionType.ReversalofGoodsIssuetoCostCenter)
            {
                if (string.IsNullOrWhiteSpace(request.CostCenterCode))
                {
                    throw new Exception("成本中心移库必须填写科目代码，无法移库");
                }
            }

            // --- 1. 准备数据 ---
            //var TransferNo = ;
            var currentDate = DateTime.Today;

            var createDtos = stocksToTransfer.Select(stock => new MaterialTransferLogCreateDto()
            {
                TransferNo = _serialHelperService.GenerateNext($"{request.FactoryCode}MaterialTransferToSAPSerial"),
                TransferType = request.TransferType.GetDescription(),
                PostingDate = currentDate,
                DocumentDate = currentDate,
                FactoryCode = request.FactoryCode,
                MaterialCode = stock.MaterialCode,
                FromLocationCode = request.FromLocation,
                BatchCode = stock.BatchCode,
                WorkOrderNo = request.WorkOrderNo,
                WorkOrderId = request.WorkOrderId,
                MovementType = ((int)request.ConsumptionType).ToString(),
                Quantity = stock.Quantity,
                BaseUnit = stock.BaseUnit,
                MovementReason = movementReason,
                Remark = remark,
                ReceivingMaterialCode = stock.MaterialCode,
                ReceivingBatchCode = stock.BatchCodeRecive ?? stock.BatchCode,
                ToFactoryCode = request.FactoryCode,
                FromStockId = stock.StockId ?? null,
                StockLogId = stock.StockLogId ?? null,
                ToLocationCode = request.ToLocation,
                CreatedBy = request.EmployeeId,
                CostCenterCode = request.CostCenterCode
            }).ToList();

            List<MaterialTransferLogDto> transferLogs = null;

            // --- 2. 【短事务区间 A】创建初始日志并提交 ---
            // 目的：无论后续 SAP 成功与否，先在数据库占位，避免“只调了接口没留记录”的情况
            var transferIds = await _materialTransferLogService.CreateBatchAsync(createDtos);
            transferLogs = await _materialTransferLogService.GetListByIdsAsync(transferIds);

            // --- 3. 【无事务区间】调用 SAP 接口 ---
            // 这是一个耗时操作，现在它不会持有任何数据库锁
            List<MaterialTransferLogDto> sapResults = new List<MaterialTransferLogDto>();
            try
            {
                var parameter = (await _parameterGroupService.GetGroupWithItemsAsync("TransferSAP")).Items.FirstOrDefault(x => x.Key == "IsEnabled");

                if (parameter == null || bool.Parse(parameter.Value))
                {
                    // 调用外部服务
                    sapResults = await _sapRfcService.MaterialStockTransferToSAPAsync(transferLogs);

                    if (sapResults == null || sapResults.Count == 0)
                    {
                        // 异常防御：接口返回了空，手动标记为错误
                        sapResults = transferLogs.Select(log => {
                            log.MessageType = "E";
                            log.Message = "调用SAP接口返回空结果";
                            return log;
                        }).ToList();
                    }
                }
                else
                {
                    // 模拟成功
                    sapResults = transferLogs.Select(log => {
                        log.Message = "SAP Call Disabled";
                        log.MessageType = "S";
                        return log;
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // 捕获 SAP 调用本身的异常（如网络不通），不让程序崩溃，而是去更新数据库状态为失败
                sapResults = transferLogs.Select(log =>
                {
                    log.Message = $"SAP接口调用异常: {ex.Message}";
                    log.MessageType = "E";
                    return log;
                }).ToList();
            }

            // --- 4. 【短事务区间 B】更新日志结果 ---
            // 重新开启一个短事务，将 SAP 的结果回写到数据库
            var updateDtos = sapResults.Select(item => new MaterialTransferLogUpdateDto
            {
                Id = item.Id,
                Message = item.Message,
                MessageType = item.MessageType,
                Status = item.MessageType.ToUpper() == "S" ? "1" : "-1",
                UpdatedBy = request.EmployeeId,
                UpdatedAt = DateTime.Now
            }).ToList();

            await _materialTransferLogService.UpdateListAsync(updateDtos);

            // --- 4.1  如果发往2200库时，更新线边库状态 ---

            var toLocation2200 = (await _parameterGroupService.GetGroupWithItemsAsync("CN11SAPStockLocation"))?.Items.FirstOrDefault(x => x.Key == "CableRawLineStock")?.Value ?? "2200"; 

            if (request.ToLocation == toLocation2200)
            {
                var update2200Dtos = sapResults
                    .Where(x => x.MessageType.ToUpper() == "S")
                    .Select(item => new RawLinesideStockUpdateDto
                    {
                        Id = (int)item.FromStockId,
                        SapStatus = "2",
                        UpdatedAt = DateTime.Now
                    }).ToList();
                if (update2200Dtos.Any())
                {
                    await _rawLinesideStockService.BatchUpdateAsync(update2200Dtos);
                }
            }
            // --- 5. 返回结果 ---
            var failedMessages = sapResults.Where(x => x.MessageType.ToUpper() != "S").ToList();
            if (failedMessages.Count == 0)
            {
                return (true, "库存转移成功！");
            }
            else
            {
                var errorMessages = string.Join("; ", failedMessages.Select(x => x.Message));
                // 即使部分失败，整体流程也算结束了，返回 True 表示“调用动作完成”，通过 message 提示用户失败项
                return (true, $"库存转移操作完成，但存在失败项：{errorMessages}");
            }
        }

        /// <summary>
        /// 【BUG 修复】: 帮助方法，用于从现有日志创建新的 Create DTO (深拷贝)
        /// </summary>
        private MaterialTransferLogCreateDto CreateDtoFromLog(MaterialTransferLogDto log)
        {
            // 这是一个浅拷贝，但由于我们只修改值类型 (Quantity) 和字符串 (BatchCode)，这是安全的。
            // 如果 MaterialTransferLogDto 包含复杂的对象，您需要使用 AutoMapper 或手动深拷贝。
            return new MaterialTransferLogCreateDto
            {
                TransferType = log.TransferType,
                PostingDate = log.PostingDate,
                DocumentDate = log.DocumentDate,
                WorkOrderId = log.WorkOrderId,
                WorkOrderNo = log.WorkOrderNo,
                MaterialCode = log.MaterialCode,
                FactoryCode = log.FactoryCode,
                FromLocationCode = log.FromLocationCode,
                BatchCode = log.BatchCode, // 将被覆盖
                MovementType = log.MovementType,
                Quantity = log.Quantity, // 将被覆盖
                BaseUnit = log.BaseUnit,
                ReceivingMaterialCode = log.ReceivingMaterialCode,
                ToLocationCode = log.ToLocationCode,
                ToFactoryCode = log.ToFactoryCode,
                ReceivingBatchCode = log.ReceivingBatchCode,
                Remark = log.Remark,
                CreatedBy = log.CreatedBy
                // TransferNo 将在创建循环中单独分配
            };
        }
    }
}
