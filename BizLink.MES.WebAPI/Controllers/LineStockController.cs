using Azure.Core;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Helper;
using BizLink.MES.Application.Services;
using BizLink.MES.Application.Services.ApiServices;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.WinForms.Common;
using Dm.util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Linq;
using System.Text;

namespace BizLink.MES.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LineStockController : ControllerBase
    {

        //private readonly IWarehouseLocationService _warehouseLocationService;
        //private readonly IRawLinesideStockService _rawLinesideStockService;
        //private readonly ISerialHelperService _serialHelperService;
        //private readonly ISapRfcService _sapRfcService;
        //private readonly IMaterialTransferLogService _materialTransferLogService;
        //private readonly IParameterGroupService _parameterGroupService;
        //private readonly IFactoryService _factoryService;
        //private readonly IRawLinesideStockLogService _rawLinesideStockLogService;
        private readonly ILineStockApiService _lineStockApiService;
        private readonly IMaterialTransferApiService _materialTransferApiService;



        public LineStockController( ILineStockApiService lineStockApiService,IMaterialTransferApiService materialTransferApiService)
        {
            _materialTransferApiService = materialTransferApiService;
            _lineStockApiService = lineStockApiService;
        }

        [HttpGet("byBarcode")]
        public async Task<ActionResult<ApiResponse<RawLinesideStockDto>>> GetLineStockByBarcodeAsync([FromQuery] int factoryid, [FromQuery] string barcode)
        {
            try
            {
                var result = await _lineStockApiService.GetStockByBarcodeAsync(factoryid, barcode);
                return Ok(ApiResponse<RawLinesideStockDto>.Success(result));
            }
            catch (Exception ex)
            {
                // 捕获业务逻辑抛出的异常，返回 400
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }

        [HttpGet("byMaterialCode")]
        public async Task<ActionResult<ApiResponse<List<RawLinesideStockDto>>>> GetLineStockByMaterialCodeAsync([FromQuery] int factoryid, [FromQuery] string materialcode)
        {
            try
            {
                var result = await _lineStockApiService.GetListByMaterialCodeAsync(factoryid, materialcode.Trim());
                return Ok(ApiResponse<List<RawLinesideStockDto>>.Success(result));
            }
            catch (Exception ex)
            {
                // 捕获业务逻辑抛出的异常，返回 400
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }

        [HttpGet("GetHandoverListByBarcode")]
        public async Task<ActionResult<ApiResponse<RawLinesideStockDto>>> GetGetWaitSyncByBarcodeAsync([FromQuery] int factoryid, [FromQuery] string barcode)
        {
            try
            {
                var result = await _lineStockApiService.GetNeedSyncStockBarcodeAsync(factoryid, barcode);
                return Ok(ApiResponse<RawLinesideStockDto>.Success(result));
            }
            catch (Exception ex)
            {
                // 捕获业务逻辑抛出的异常，返回 400
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<ApiResponse<bool>>> TransferStock([FromBody] TransferStockRequest request)
        {
            try
            {
                var (result, message) = await _lineStockApiService.TransferStockAsync(request);
                // ApiResponse.Success 的第二个参数通常是 message
                return Ok(ApiResponse<bool>.Success(result, message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }

        [HttpPost("HandoverConfirm")]
        public async Task<ActionResult<ApiResponse<bool>>> MaterialHandoverConfirmAsync([FromBody] TransferStockRequest request)
        {
            try
            {
                var (result, message) = await _lineStockApiService.SyncStockToSapAsync(request);
                // ApiResponse.Success 的第二个参数通常是 message
                return Ok(ApiResponse<bool>.Success(result, message));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }
        }

        [HttpPost("transferToSAP")] 
        public async Task<ActionResult<ApiResponse<bool>>> TransferMaterialToSAPAsync([FromBody] TransferSapRequest request)
        {
            try
            {
                // 2. 调用 Service 层处理所有业务逻辑
                var (result, message) = await _materialTransferApiService.TransferMaterialToSAPAsync(request);

                // 3. 将 Service 层的友好结果 转换为 API 响应
                if (result)
                {
                    // 注意：即使部分失败 (IsSuccess = true)，也将消息返回给前端
                    return Ok(ApiResponse<bool>.Success(true, message));
                }
                else
                {
                    // 只有在 Service 层明确返回失败时才返回 BadRequest
                    return BadRequest(ApiResponse<bool>.Fail(message));
                }
            }
            catch (Exception ex)
            {
                // 4. 捕获任何未预料到的(基础设施)异常
                // (例如：服务本身无法解析、数据库连接字符串错误等)
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }

            //try
            //{
            //    if (request.Stocks.Count() == 0 || request.Stocks.Where(x => !string.IsNullOrWhiteSpace(x.BatchCode)).Count() == 0 )
            //        throw new Exception("待移库信息为空，无法移库");

            //    if (!Enum.IsDefined(typeof(ConsumptionType), request.ConsumptionType))
            //        throw new Exception("货物移动类型不存在，无法移库");

            //    if (!Enum.IsDefined(typeof(MaterialTransferType), request.TransferType))
            //        throw new Exception("事件类型不存在，无法移库");
            //    else
            //    {
            //        if (request.TransferType == MaterialTransferType.RawMaterialScrapAgainstOrder)
            //        {
            //            if (request.WorkOrderId == null || request.WorkOrderId == 0)
            //                throw new Exception("原材料报废必须关联工单，无法移库");

            //            if(string.IsNullOrWhiteSpace(request.MovementReason))
            //                throw new Exception("原材料报废必须填写移动原因，无法移库");
            //        }
            //    }

            //    List<MaterialTransferLogDto> materialTransferLogDtos = new List<MaterialTransferLogDto>();
            //    var TransferNo = _serialHelperService.GenerateNext($"{request.FactoryCode}MaterialTransferToSAPSerial");
            //    foreach (var stock in request.Stocks)
            //    {
            //        var transferdto = await _materialTransferLogService.CreateAsync(new MaterialTransferLogCreateDto()
            //        {
            //            TransferNo = TransferNo,
            //            TransferType = request.TransferType.GetDescription(),
            //            PostingDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
            //            DocumentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
            //            FactoryCode = request.FactoryCode,
            //            MaterialCode = stock.MaterialCode,
            //            FromLocationCode = request.FromLocation,
            //            BatchCode = stock.BatchCode,
            //            WorkOrderNo = request.WorkOrderNo,
            //            WorkOrderId = request.WorkOrderId,
            //            MovementType = ((int)request.ConsumptionType).toString(),
            //            Quantity = stock.Quantity,
            //            BaseUnit = stock.BaseUnit,
            //            MovementReason = request.MovementReason,
            //            ReceivingMaterialCode = stock.MaterialCode,
            //            ReceivingBatchCode = stock.BatchCodeRecive ?? stock.BatchCode,
            //            ToFactoryCode = request.FactoryCode,
            //            FromStockId = stock.StockId ?? null,
            //            StockLogId = stock.StockLogId ?? null,
            //            ToLocationCode = request.ToLocation,
            //            CreatedBy = request.EmployeeId,
            //        });

            //        materialTransferLogDtos.Add(transferdto);
            //    }
                
            //    var parameter = (await _parameterGroupService.GetGroupWithItemsAsync("TransferSAP")).Items.Where(x => x.Key == "IsEnabled").FirstOrDefault();
            //    if (parameter == null || bool.Parse(parameter.Value))
            //    {
            //        var result = await _sapRfcService.MaterialStockTransferToSAPAsync(materialTransferLogDtos);
            //        if (result != null && result.Count() > 0)
            //        {
            //            foreach (var item in result)
            //            {
            //                var temp = await _materialTransferLogService.GetByTransferNoAsync(item.TransferNo, item.MaterialCode, item.BatchCode);
            //                if (temp != null)
            //                {
            //                    await _materialTransferLogService.UpdateAsync(new MaterialTransferLogUpdateDto()
            //                    {
            //                        Id = temp.Id,
            //                        Message = item.Message,
            //                        MessageType = item.MessageType,
            //                        Status = item.MessageType.ToUpper() == "S" ? "1" : "-1",
            //                        UpdatedBy = request.EmployeeId,
            //                        UpdatedAt = DateTime.Now
            //                    });
            //                }

            //            }

            //            if (result.Where(x => x.MessageType.ToUpper() != "S").Count() == 0)
            //                return Ok(ApiResponse<bool>.Success(true, "库存转移成功！"));
            //            else
            //            {
            //                var errorMessages = string.Join("; ", result.Where(x => x.MessageType.ToUpper() != "S").Select(x => x.Message));

            //                return Ok(ApiResponse<bool>.Success(true, $"库存转移部分失败，失败原因：{errorMessages}"));
            //            }

            //        }
            //        else
            //        {
            //            throw new Exception("调用SAP接口失败，返回结果为空");
            //        }
            //    }
            //    else
            //        return Ok(ApiResponse<bool>.Success(true, "库存转移成功！"));

            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            //}
        }


        [HttpPost("adjustInventoryInSAP")]
        public async Task<ActionResult<ApiResponse<bool>>> AdjustInventoryInSAPAsync([FromBody] TransferSapRequest request)
        {
            try
            {
                // 2. 调用 Service 层处理所有业务逻辑
                var (result, message) = await _materialTransferApiService.AdjustInventoryInSAPAsync(request);

                // 3. 将 Service 层的友好结果 转换为 API 响应
                if (result)
                {
                    // 注意：即使部分失败 (IsSuccess = true)，也将消息返回给前端
                    return Ok(ApiResponse<bool>.Success(true, message));
                }
                else
                {
                    // 只有在 Service 层明确返回失败时才返回 BadRequest
                    return BadRequest(ApiResponse<bool>.Fail(message));
                }
            }
            catch (Exception ex)
            {
                // 4. 捕获任何未预料到的(基础设施)异常
                // (例如：服务本身无法解析、数据库连接字符串错误等)
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }

        }


        [HttpPost("RetransferToSAP")]
        public async Task<ActionResult<ApiResponse<bool>>> RetransferMaterialToSAPAsync([FromBody] List<int> transferIds)
        {
            if (transferIds == null || transferIds.Count == 0)
            {
                return BadRequest(ApiResponse<bool>.Fail("未提供任何需要重传的ID。"));
            }

            try
            {
                // 1. 将所有工作委托给服务层
                var (result,message) = await _materialTransferApiService.RetransferLogsToSAPAsync(transferIds);

                // 2. 根据服务层返回的详细结果进行响应
                if (result)
                {
                    return Ok(ApiResponse<bool>.Success(true, message));
                }
                else
                {
                    // 即使是部分失败，也可能返回 200 OK，但附带错误信息
                    return BadRequest(ApiResponse<bool>.Fail(message));
                }
            }
            catch (Exception ex) // 捕获服务层抛出的其他异常 (如验证失败)
            {
                // 4. 向客户端返回错误信息
                return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            }

            //try
            //{
            //    var parameter = (await _parameterGroupService.GetGroupWithItemsAsync("TransferSAP")).Items.Where(x => x.Key == "IsEnabled").FirstOrDefault();
            //    if (parameter == null || bool.Parse(parameter.Value))
            //    {
            //        var materialTransferLogDtos = await _materialTransferLogService.GetListByIdsAsync(transferIds);
            //        if(materialTransferLogDtos == null || materialTransferLogDtos.Count == 0)
            //            throw new Exception("未查询到待重传的库存转移记录，无法重传！");
            //        var factoryCode = materialTransferLogDtos.GroupBy(x => x.FactoryCode).Select(g => g.Key);
            //        if (factoryCode.Count() > 1)
            //            throw new Exception("待重传的库存转移记录包含多个工厂代码，无法重传！");
            //        if (factoryCode.FirstOrDefault() == null)
            //            throw new Exception("未查询到待重传记录中的工厂信息，无法重传！");
            //        //var TransferNo = string.Empty;
            //        //if (materialTransferLogDtos.Count() == 1)
            //        //{
            //        //    var transferLogs = await _materialTransferLogService.GetByTransferNoAsync(materialTransferLogDtos.FirstOrDefault().TransferNo);
            //        //    if (transferLogs == null || transferLogs.Count() > 1 || string.IsNullOrWhiteSpace(materialTransferLogDtos.FirstOrDefault().TransferNo))
            //        //        TransferNo = _serialHelperService.GenerateNext($"{factoryCode.FirstOrDefault()}MaterialTransferToSAPSerial");
            //        //    else
            //        //        TransferNo = materialTransferLogDtos.FirstOrDefault().TransferNo;
            //        //}
            //        //else
            //        //    TransferNo = _serialHelperService.GenerateNext($"{factoryCode.FirstOrDefault()}MaterialTransferToSAPSerial");

            //        // 1. 获取 SAP 原始库存
            //        var locationGroup = await _parameterGroupService.GetGroupWithItemsAsync("SAPRawMtrStock");
            //        string fromLocation = locationGroup?.Items.FirstOrDefault(x => x.Key == "SAPRawMtrStock")?.Value ?? "1100";
            //        var sapStocks = (await _sapRfcService.GetRawMaterialStockFromSapAsync(
            //           materialTransferLogDtos.GroupBy(s => s.MaterialCode).Select(x => new SapRawMaterialStockDto()
            //            {
            //                MaterialCode = x.Key,
            //                FactoryCode = factoryCode.FirstOrDefault(),
            //                LocationCode = fromLocation,
            //            }).ToList()
            //        )).Select(x => new SapRawMaterialStockDto()
            //        {
            //            MaterialCode = x.MaterialCode.TrimStart('0'),
            //            FactoryCode = x.FactoryCode,
            //            LocationCode = x.LocationCode,
            //            BatchCode = x.BatchCode,
            //            Quantity = x.Quantity
            //        }).ToList();

            //        var transferUpdate = new List<MaterialTransferLogDto>();
            //        var transferCreate = new List<MaterialTransferLogDto>();
            //        var waitTransferIds = new List<int>();

            //        foreach (var transferitem in materialTransferLogDtos)
            //        {
            //            //判断当前记录传输号是否存在多笔记录
            //            var transferLogs = await _materialTransferLogService.GetByTransferNoAsync(transferitem.TransferNo);
            //            if (transferLogs.Count() > 1)
            //                //如果存在多笔记录，给当前记录赋值新的传输号
            //                transferitem.TransferNo = _serialHelperService.GenerateNext($"{factoryCode.FirstOrDefault()}MaterialTransferToSAPSerial");

            //            var materialSummery = sapStocks.Where(x => x.MaterialCode == transferitem.MaterialCode ).Sum(x => x.Quantity);
            //            var requiredQuantity = transferitem.Quantity;
            //            //库存总数大于当前行需求时
            //            if (materialSummery > requiredQuantity)
            //            {
            //                var batchSummery = sapStocks.Where(x => x.MaterialCode == transferitem.MaterialCode && x.BatchCode == transferitem.BatchCode).Sum(x => x.Quantity);
            //                //当前需求批次有库存时优先使用当前批次
            //                foreach (var batchitem in sapStocks.Where(x => x.MaterialCode == transferitem.MaterialCode && x.BatchCode == transferitem.BatchCode))
            //                {
            //                    decimal takeQuantity = Math.Min((decimal)requiredQuantity, (decimal)batchitem.Quantity);
            //                    requiredQuantity -= takeQuantity;
            //                    batchSummery -= takeQuantity;
            //                    materialSummery -= takeQuantity;
            //                    batchitem.Quantity -= takeQuantity;
            //                    if (requiredQuantity == 0)
            //                        break;

            //                }
            //                //如果需求仍然未满足，按FIFO循环其他可用批次
            //                if (requiredQuantity > 0)
            //                {
            //                    //将原记录的数量更新
            //                    transferUpdate.add(new MaterialTransferLogDto()
            //                    {
            //                        Id = transferitem.Id,
            //                        TransferNo = transferitem.TransferNo,
            //                        Quantity = batchSummery
            //                    });
            //                    foreach (var batchitem in sapStocks.Where(x => x.MaterialCode == transferitem.MaterialCode && x.BatchCode != transferitem.BatchCode).OrderBy(x => x.BatchCode))
            //                    {
            //                        decimal takeQuantity = Math.Min((decimal)requiredQuantity, (decimal)batchitem.Quantity);
            //                        requiredQuantity -= takeQuantity;
            //                        materialSummery -= takeQuantity;
            //                        batchitem.Quantity -= takeQuantity;

            //                        //创建一条批次新增记录
            //                        var transferItemNew = transferitem;
            //                        transferItemNew.Quantity = takeQuantity;
            //                        transferCreate.Add(transferItemNew);

            //                        if (requiredQuantity == 0)
            //                            break;

            //                    }
            //                }


            //            }


            //        }

            //        //Update更新

            //        foreach (var update in transferUpdate)
            //        {
            //            waitTransferIds.add(update.Id);
            //            await _materialTransferLogService.UpdateAsync(new MaterialTransferLogUpdateDto() { Id = update.Id,Quantity = update.Quantity });
            //        }

            //        //Create创建

            //        foreach (var create in transferCreate)
            //        {
            //            var temp = await _materialTransferLogService.CreateAsync(new MaterialTransferLogCreateDto()
            //            {
            //                TransferNo = _serialHelperService.GenerateNext($"{create.FactoryCode}MaterialTransferToSAPSerial"),
            //                TransferType = create.TransferType,
            //                PostingDate = create.PostingDate,
            //                DocumentDate = create.DocumentDate,
            //                WorkOrderId = create.WorkOrderId,
            //                WorkOrderNo = create.WorkOrderNo,
            //                MaterialCode = create.MaterialCode,
            //                FactoryCode = create.FactoryCode,
            //                FromLocationCode = create.FromLocationCode,
            //                BatchCode = create.BatchCode,
            //                MovementType = create.MovementType,
            //                Quantity = create.Quantity,
            //                BaseUnit = create.BaseUnit,
            //                ReceivingMaterialCode = create.ReceivingMaterialCode,
            //                ToLocationCode = create.ToLocationCode,
            //                ToFactoryCode = create.ToFactoryCode,
            //                ReceivingBatchCode = create.ReceivingBatchCode,
            //                Remark = create.Remark,
            //                CreatedBy = create.CreatedBy,
            //            });

            //            waitTransferIds.add(temp.Id);
            //        }

            //        if (waitTransferIds.Count() > 0)
            //        {
            //            var materialTransferLogDtosNew = await _materialTransferLogService.GetListByIdsAsync(waitTransferIds);
            //            var result = await _sapRfcService.MaterialStockTransferToSAPAsync(materialTransferLogDtosNew);
            //            if (result != null && result.Count() > 0)
            //            {
            //                foreach (var item in result)
            //                {
            //                    var temp = await _materialTransferLogService.GetByTransferNoAsync(item.TransferNo, item.MaterialCode, item.BatchCode);
            //                    if (temp != null)
            //                    {
            //                        await _materialTransferLogService.UpdateAsync(new MaterialTransferLogUpdateDto()
            //                        {
            //                            Id = temp.Id,
            //                            Message = item.Message,
            //                            MessageType = item.MessageType,
            //                            Status = item.MessageType.ToUpper() == "S" ? "1" : "-1",
            //                            UpdatedAt = DateTime.Now
            //                        });
            //                    }

            //                }

            //                if (result.Where(x => x.MessageType.ToUpper() != "S").Count() == 0)
            //                    return Ok(ApiResponse<bool>.Success(true, "库存转移成功！"));
            //                else
            //                {
            //                    var errorMessages = string.Join("; ", result.Where(x => x.MessageType.ToUpper() != "S").Select(x => x.Message));
            //                    throw new Exception($"库存转移部分失败，失败原因：{errorMessages}");
            //                }

            //            }
            //            else
            //            {
            //                throw new Exception("调用SAP接口失败，返回结果为空");
            //            }
            //        }
            //        else
            //        {
            //            throw new Exception("更新重传的库存转移记录失败，无法重传！");
            //        }

            //    }
            //    else
            //        return Ok(ApiResponse<bool>.Success(true, "库存转移重推成功！"));


            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ApiResponse<bool>.Fail(ex.Message));
            //}
        }

        [HttpPost("returnToWMS")]
        public async Task<ActionResult<ApiResponse<bool>>> returnMaterialToWMSAsync()
        {
            try
            {

                return Ok(ApiResponse<bool>.Success(true, "退料任务创建成功！"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<bool>.Fail($"提交失败：{ex.Message}"));
            }
        }
    }

}
