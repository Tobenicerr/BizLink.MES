using Azure;
using Azure.Core;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Helper;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.WinForms.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Web;

namespace BizLink.MES.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkOrderPickVerfController : ControllerBase
    {

        private readonly IWorkOrderService _workOrderService;
        private readonly IWorkOrderViewService _workOrderViewService;
        private readonly IWorkOrderProcessService _workOrderProcessService;
        private readonly IWorkOrderTaskService _workOrderTaskService;
        private readonly IProductLinesideStockService _productLinesideStockService;
        private readonly IWorkOrderBomItemService _workOrderBomItemService;
        private readonly IMaterialViewService _materialViewService;
        private readonly IFactoryService _factoryService;
        private readonly IJyApiClient _jyApiClient;
        private readonly IMesApiClient _mesApiClient;
        private readonly Dictionary<string, ServiceEndpointSettings> _apiSettings;
        private readonly IWorkOrderOperationConsumptionRecordService _workOrderOperationConsumptionRecordService;
        private readonly IParameterGroupService _parameterGroupService;
        private readonly ISapRfcService _sapRfcService;

        private readonly ISerialHelperService _serialHelperService;
        private readonly IWorkOrderOperationConfirmService _workOrderOperationConfirmService;
        private readonly IWorkOrderOperationConsumpService _workOrderOperationConsumpService;

        private readonly IPackageVerfApiService _packageVerfService;




        public WorkOrderPickVerfController(IWorkOrderService workOrderService, IWorkOrderViewService workOrderViewService,IProductLinesideStockService productLinesideStockService, IWorkOrderBomItemService workOrderBomItemService, IMaterialViewService materialViewService, IWorkOrderTaskService workOrderTaskService, IWorkOrderProcessService workOrderProcessService, IJyApiClient jyApiClient, IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings, IWorkOrderOperationConsumptionRecordService workOrderOperationConsumptionRecordService, IMesApiClient mesApiClient, IFactoryService factoryService, IParameterGroupService parameterGroupService, ISapRfcService sapRfcService, ISerialHelperService serialHelperService, IWorkOrderOperationConfirmService workOrderOperationConfirmService, IWorkOrderOperationConsumpService workOrderOperationConsumpService, IPackageVerfApiService packageVerfService)
        {
            _workOrderService = workOrderService;
            _workOrderViewService = workOrderViewService;
            _productLinesideStockService = productLinesideStockService;
            _materialViewService = materialViewService;
            _workOrderBomItemService = workOrderBomItemService;
            _workOrderTaskService = workOrderTaskService;
            _workOrderProcessService = workOrderProcessService;
            _jyApiClient = jyApiClient;
            _workOrderOperationConsumptionRecordService = workOrderOperationConsumptionRecordService;
            _mesApiClient = mesApiClient;
            _apiSettings = apiSettings.Value;
            _factoryService = factoryService;
            _parameterGroupService = parameterGroupService;
            _sapRfcService = sapRfcService;
            _serialHelperService = serialHelperService;
            _workOrderOperationConfirmService = workOrderOperationConfirmService;
            _workOrderOperationConsumpService = workOrderOperationConsumpService;
            _packageVerfService = packageVerfService;        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> GetWorkOrderRawUnpackAsync(string orderno)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(orderno))
                {
                    throw new Exception("订单号不能为空");
                }
                var workorder = await _workOrderService.GetByOrdrNoAsync(orderno.Split("-")[0]);
                if (workorder == null)
                    throw new Exception("未查询到订单信息");
                var factory = await _factoryService.GetByIdAsync(workorder.FactoryId);
                var bomitems = (await _workOrderBomItemService.GetListByOrderIdAync(workorder.Id)).Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true).Where(x => x.ConsumeType == (int)ConsumeType.CableMaterial || x.ConsumeType == (int)ConsumeType.OrderBasedMaterial).ToList();
                if (bomitems == null || bomitems.Count == 0)
                    throw new Exception("未查询到订单待合箱BOM信息");
                var products = await _productLinesideStockService.GetListByOrderNoAsync(workorder.OrderNumber);
                var rtn = await _workOrderViewService.GetPickMtrStockByWorkOrderAsync(workorder.OrderNumber);
                var firstProcess = (await _workOrderProcessService.GetListByOrderIdAync(workorder.Id)).OrderBy(x => x.Operation).First();
                //if(firstProcess.Status == "4")
                //    throw new Exception("当前订单已合箱或无需合箱");
                var materials = await _materialViewService.GetListByCodesAsync("CN11",bomitems.Select(x => x.MaterialCode).Distinct().ToList());

                var bomrequire = bomitems.GroupJoin(materials, bom => bom.MaterialCode, mtr => mtr.MaterialCode, (bom, mtrgroup) => new { bom, mtrgroup }).SelectMany(t => t.mtrgroup.DefaultIfEmpty(),(x, y) => new WorkOrderPickVerfBomItem { ConsumeType = y.LabelName?? "ROH原材料", ItemNo = x.bom.BomItem, WorkOrderProcessId = x.bom.WorkOrderProcessId, MaterialCode = x.bom.MaterialCode, MaterialDesc = x.bom.MaterialDesc, Quantity = (decimal)x.bom.RequiredQuantity, CompletedQuantity = 0 }).ToList();

                foreach (var item in bomrequire)
                {
                    if (item.ConsumeType == "断线")
                    {
                        var task = await _workOrderTaskService.GetByProcessIdAsync(item.WorkOrderProcessId, item.ItemNo);
                        if (task != null)
                        {
                            item.Quantity = (decimal)task.Quantity;
                            item.CompletedQuantity = (decimal)task.CompletedQty;
                        }
                        else
                        {
                            item.Quantity = 0;
                            item.CompletedQuantity = 0;
                        }
                    }
                    else
                    {
                        foreach (var stock in products.Where(x => x.MaterialCode == item.MaterialCode))
                        {
                            if (item.Quantity > item.CompletedQuantity)
                            {
                                if (item.Quantity - item.CompletedQuantity > stock.Quantity)
                                {
                                    item.CompletedQuantity += (decimal)stock.Quantity;
                                    stock.Quantity = 0;
                                    continue;
                                }
                                else
                                {
                                    var temp = item.Quantity - item.CompletedQuantity;
                                    item.CompletedQuantity += temp;
                                    stock.Quantity -= temp;
                                    break;
                                }
                            }
                            else
                                break;
                        }
                    }
                }
                var result = new
                {
                    OrderId = workorder.Id,
                    OrderNo = workorder.OrderNumber,
                    OperationNo = bomitems.Select(x => x.Operation).FirstOrDefault(),
                    OperationStatus = firstProcess.Status,
                    CableItemCount = bomrequire.Where(x => x.ConsumeType == "断线").Count(),
                    RawItemCount = bomrequire.Where(x => x.ConsumeType != "断线").Count(),
                    RawMtrBatchCount= products.Where(x => x.BomItem == null).Count(),
                    LabelCount = workorder.LabelCount,
                    CableItems = bomrequire.Where(x => x.ConsumeType == "断线").Select(x => new { x.ItemNo,x.MaterialCode,x.MaterialDesc,x.Quantity,x.CompletedQuantity}).ToList(),
                    CenterStockItems = bomrequire.Where(x => x.ConsumeType == "ROH原材料").Select(x => new { x.ItemNo, x.MaterialCode, x.MaterialDesc, x.Quantity, x.CompletedQuantity }).ToList(),
                    AutoStockItems = bomrequire.Where(x => x.ConsumeType == "自动仓物料").Select(x => new { x.ItemNo, x.MaterialCode, x.MaterialDesc, x.Quantity, x.CompletedQuantity }).ToList(),
                };

                return Ok(ApiResponse<object>.Success(result));
            }
            catch (Exception ex)
            {

               return BadRequest(ApiResponse<object>.Fail($"扫码出错：{ex.Message}"));
            }


        }

        //[HttpPut]
        //public async Task<ActionResult<ApiResponse<bool>>> UpdateUnpackProcessAsync([FromBody] WorkOrderProcessUpdateDto dto,[FromQuery] string? LocationCode = null)
        //{
        //    try
        //    {
        //        if (dto == null || dto.WorkOrderId == null || dto.WorkOrderId <= 0)
        //            throw new Exception("订单ID为空，无法提交");
        //        var workorder = await _workOrderService.GetByIdAsync((int)dto.WorkOrderId);
        //        if (workorder == null)
        //            throw new Exception("未查询到订单信息");
        //        var factory = await _factoryService.GetByIdAsync(workorder.FactoryId);
        //        if (dto.Status == "verfSuccess")
        //        {
        //            //判断断线任务是否全部完成

        //            var process = (await _workOrderProcessService.GetListByOrderIdAync(workorder.Id)).OrderBy(x => x.Operation).First();
        //            if(process.Status == ((int)WorkOrderStatus.Finished).ToString())
        //                throw new Exception("当前工序已完成，无法重复提交！");
        //            var cableitems = (await _workOrderBomItemService.GetListByOrderIdAync(workorder.Id)).Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true).Where(x => x.ConsumeType == (int)ConsumeType.CableMaterial).ToList();

        //            var tasks = await _workOrderTaskService.GetByOrderNoAsync(workorder.OrderNumber);

        //            if (cableitems.GroupJoin(tasks, bom => new { ProcessId = bom.WorkOrderProcessId, BomItem = bom.BomItem }, task => new { ProcessId = task.OrderProcessId, BomItem = task.MaterialItem }, (bom, task) => new { bom, task })
        //                .SelectMany(temp => temp.task.DefaultIfEmpty(), (temp, taskNull) => new { temp.bom.Id, taskId = taskNull?.Id ?? null }).Where(x => x.taskId == null || x.taskId == 0).Count() > 0)
        //                throw new Exception("断线物料任务未全部完成，无法提交！");

        //            //判断按单物料是否全部拣货完成
        //            var products = await _productLinesideStockService.GetListByOrderNoAsync(workorder.OrderNumber);

        //            var orderpickitems = (await _workOrderBomItemService.GetListByOrderIdAync(workorder.Id)).Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true).Where(x => x.ConsumeType == (int)ConsumeType.OrderBasedMaterial)
        //                .GroupBy(x => x.MaterialCode).Select(g => new WorkOrderBomItemDto() { MaterialCode = g.Key, RequiredQuantity = g.Sum(item => item.RequiredQuantity) }).ToList();

        //            if (orderpickitems.GroupJoin(products.GroupBy(p => p.MaterialCode).Select(g => new ProductLinesideStockDto() { MaterialCode = g.Key, Quantity = g.Sum(item => item.Quantity) }), bom => bom.MaterialCode, product => product.MaterialCode,(bom,product) => new { bom,product}).SelectMany(temp => temp.product.DefaultIfEmpty(), (temp, productNull) => new { temp.bom.Id, temp.bom.RequiredQuantity ,productId = productNull?.Id ?? null ,proQuantity = productNull?.Quantity ?? 0 }).Where(x => x.productId == null || x.RequiredQuantity - x.proQuantity > 0).Count() > 0)
        //                throw new Exception("按单物料拣货未全部完成，无法提交！");


        //            var jpitem = products.Where(x => x.BomItem == null && x.Remark != null).Select(x => x.Remark.Split("-")[1]).Distinct().ToList();
        //            var productsToUpdate = products.Select(x => new ProductLinesideStockUpdateDto()
        //            {
        //                Id = x.Id,
        //                Status = "2",
        //                UpdatedAt = DateTime.Now,
        //            }).ToList();

        //            var result = await _productLinesideStockService.UpdateStatusAsync(productsToUpdate);
        //            if (result > 0)
        //            {
        //                var processToUpdate = new WorkOrderProcessUpdateDto()
        //                {
        //                    Id = process.Id,
        //                    CompletedQuantity = workorder.Quantity,
        //                    ActEndTime = DateTime.Now,
        //                    Status = ((int)WorkOrderStatus.Finished).ToString(),
        //                    UpdateOn = DateTime.Now,
        //                    UpdateBy = dto.UpdateBy
        //                };
        //                await _workOrderProcessService.UpdateAsync(processToUpdate);

        //                //第一道工序报工准备
        //                var materialPro = await _materialViewService.GetByCodeAsync(factory.FactoryCode, workorder.MaterialCode);
        //                var sapconfirm = await _workOrderOperationConfirmService.CreateAsync(new WorkOrderOperationConfirmCreateDto()
        //                {
        //                    WorkOrderId = workorder.Id,
        //                    ProcessId = process.Id,
        //                    SapConfirmationNo = process.ConfirmNo,
        //                    WorkOrderNo = workorder.OrderNumber.PadLeft(12, '0'),
        //                    OperationNo = process.Operation,
        //                    CompletedFlag = "X",
        //                    ConfirmSequence = _serialHelperService.GenerateNext($"{factory.FactoryCode}ConfirmReportToSAP"),
        //                    WorkCenterCode = process.WorkCenter,
        //                    PostingDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
        //                    //EmployeeId = request.EmployeeId,
        //                    FactoryCode = factory.FactoryCode,
        //                    BaseUnit = materialPro == null ? "ST" : materialPro.BaseUnit,
        //                    YieldQuantity = workorder.Quantity,
        //                    ScrapQuantity = 0,
        //                    //ActStartDate = ((DateTime)process.ActStartTime).ToString("yyyyMMdd"),
        //                    //ActStartTime = ((DateTime)process.ActStartTime).ToString("HH:mm:ss"),
        //                    ActFinishDate = ((DateTime)process.ActEndTime).ToString("yyyyMMdd"),
        //                    ActFinishTime = ((DateTime)process.ActEndTime).ToString("HH:mm:ss"),
        //                    //CreatedBy = request.EmployeeId,
        //                });

        //                var consumemes = await _workOrderOperationConsumptionRecordService.GetListByProcessIdAsync(process.Id);
        //                await _workOrderOperationConsumpService.CreateAsync(consumemes.GroupBy(g => new { g.ReservationItem, g.MaterialCode, g.BatchCode, g.BaseUnit, g.ConsumptionType, g.ConsumptionRemark }).Select(s => new WorkOrderOperationConsumpCreateDto()
        //                {
        //                    OperationConfirmId = sapconfirm.Id,
        //                    SapConfirmationNo = sapconfirm.SapConfirmationNo,
        //                    WorkOrderNo = workorder.OrderNumber.PadLeft(12, '0'),
        //                    ConfirmSequence = sapconfirm.ConfirmSequence,
        //                    ReservationNo = workorder.ReservationNo,
        //                    ReservationItem = s.Key.ReservationItem,
        //                    MaterialCode = s.Key.MaterialCode.StartsWith("E") ? s.Key.MaterialCode : s.Key.MaterialCode.PadLeft(18, '0'),
        //                    FactoryCode = factory.FactoryCode,
        //                    FromLocationCode = "2100",
        //                    BatchCode = s.Key.BatchCode,
        //                    MovementType = ((int)s.Key.ConsumptionType).ToString(),
        //                    MovementReason = s.Key.ConsumptionRemark ?? "",
        //                    Quantity = s.Sum(ss => ss.Quantity),
        //                    BaseUnit = s.Key.BaseUnit,
        //                    //CreatedBy = request.EmployeeId,
        //                }).ToList());

        //                var boms = (await _workOrderBomItemService.GetListByOrderIdAync(workorder.Id)).Where(x => x.MovementAllowed == true && x.ConsumeType == (int)ConsumeType.OrderBasedMaterial && x.RequiredQuantity > 0).ToList();
        //                var orderpickproducts = products.Where(x => x.BomItem == null && !string.IsNullOrWhiteSpace(x.Remark) && x.Status == "1").ToList();


        //                //将按单物料移库到2100库位
        //                var parameterGroup = await _parameterGroupService.GetGroupWithItemsAsync("CN11SAPStockLocation");
        //                var transferUrl = _apiSettings["MesApi"].Endpoints["LineStockTransferToSAP"];

        //                #region sap库存检查与分配
        //                var sapstocks = (await _sapRfcService.GetRawMaterialStockFromSapAsync(orderpickproducts.GroupBy(s => s.MaterialCode).Select(x => new SapRawMaterialStockDto()
        //                {
        //                    MaterialCode = x.Key,
        //                    FactoryCode = factory.FactoryCode,
        //                    LocationCode = parameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault() == null ? "1100" : (parameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault()).Value,
        //                }).ToList())).Select(x => new SapRawMaterialStockDto()
        //                {
        //                    MaterialCode = x.MaterialCode.TrimStart('0'),
        //                    FactoryCode = x.FactoryCode,
        //                    LocationCode = x.LocationCode,
        //                    BatchCode = x.BatchCode,
        //                    Quantity = x.Quantity
        //                }).ToList();
        //                bool allSatisfied = false;
        //                if (sapstocks != null && sapstocks.Count() > 0)
        //                {
        //                    //判断sap的物料总数能否满足当前
        //                    allSatisfied = true;
        //                    var stockSummary = sapstocks.GroupBy(s => s.MaterialCode).ToDictionary(group => group.Key, group => group.Sum(item => (decimal)item.Quantity));

        //                    // 步骤 B: 计算 ListB 中每种物料的总需求
        //                    var demandSummary = orderpickproducts.GroupBy(d => d.MaterialCode).ToDictionary(
        //                        group => group.Key, group => group.Sum(item => (decimal)item.Quantity));

        //                    foreach (var demandEntry in demandSummary)
        //                    {
        //                        string material = demandEntry.Key;
        //                        decimal totalDemand = demandEntry.Value;

        //                        // 尝试从库存字典中获取该物料的总库存
        //                        stockSummary.TryGetValue(material, out decimal totalStock);
        //                        // (如果 TriesGetValue 失败, totalStock 会默认为 0)

        //                        if (totalStock < totalDemand)
        //                        {
        //                            allSatisfied = false;
        //                        }
        //                    }
        //                }

        //                //需求满足后再对sap进行移库
        //                if (allSatisfied)
        //                {
        //                    var availableStock = new List<SapRawMaterialStockDto>();
        //                    foreach (var demand in orderpickproducts)
        //                    {
        //                        decimal neededQuantity = (decimal)demand.Quantity;
        //                        var allocationsForThisDemand = new List<SapRawMaterialStockDto>();
        //                        var sapstock = sapstocks.Where(x => x.MaterialCode == demand.MaterialCode && x.BatchCode == demand.BatchCode).FirstOrDefault();
        //                        if (sapstock != null && sapstock.Quantity > 0)
        //                        {
        //                            decimal takeQuantity = Math.Min(neededQuantity, (decimal)sapstock.Quantity);
        //                            allocationsForThisDemand.Add(new SapRawMaterialStockDto()
        //                            {
        //                                MaterialCode = sapstock.MaterialCode,
        //                                FactoryCode = sapstock.FactoryCode,
        //                                BatchCode = sapstock.BatchCode,
        //                                LocationCode = sapstock.LocationCode,
        //                                BaseUnit = demand.BaseUnit,
        //                                Quantity = takeQuantity
        //                            });
        //                            sapstock.Quantity -= takeQuantity;
        //                            neededQuantity -= takeQuantity;
        //                        }
        //                        if (neededQuantity > 0)
        //                        {
        //                            var otherBatches = sapstocks.Where(x => x.MaterialCode == demand.MaterialCode && x.BatchCode != demand.BatchCode && x.Quantity > 0).OrderBy(x => x.BatchCode).ToList();

        //                            foreach (var batch in otherBatches)
        //                            {
        //                                decimal takeQuantity = Math.Min(neededQuantity, (decimal)batch.Quantity);
        //                                allocationsForThisDemand.Add(new SapRawMaterialStockDto()
        //                                {
        //                                    MaterialCode = batch.MaterialCode,
        //                                    FactoryCode = batch.FactoryCode,
        //                                    BatchCode = batch.BatchCode,
        //                                    LocationCode = batch.LocationCode,
        //                                    Quantity = takeQuantity,
        //                                    BatchCodeNew = demand.BatchCode
        //                                });

        //                                batch.Quantity -= takeQuantity;
        //                                neededQuantity -= takeQuantity;

        //                                availableStock.AddRange(allocationsForThisDemand);
        //                                if (neededQuantity == 0)
        //                                    break;

        //                            }

        //                        }


        //                    }

        //                    if (availableStock.Count() > 0)
        //                    {
        //                        var saptranferRequest = new TransferSapRequest()
        //                        {

        //                            FactoryCode = factory.FactoryCode,
        //                            EmployeeId = string.Empty,
        //                            TransferType = MaterialTransferType.GoodsIssue,
        //                            ConsumptionType = ConsumptionType.MaterialTransfer,
        //                            Stocks = availableStock.Select(x => new TransferStock()
        //                            {
        //                                MaterialCode = x.MaterialCode,
        //                                BatchCode = x.BatchCode,
        //                                Quantity = (decimal)x.Quantity,
        //                                BaseUnit = x.BaseUnit ?? "ST",
        //                                BatchCodeRecive = x.BatchCodeNew
        //                            }).ToList(),
        //                            FromLocation = parameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault() == null ? "1100" : (parameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault()).Value,
        //                            ToLocation = parameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault() == null ? "1100" : (parameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault()).Value,
        //                        };

        //                        var jsonTemp = JsonConvert.SerializeObject(saptranferRequest);
        //                        var rtn = await _mesApiClient.PostAsync<object, object>(transferUrl, saptranferRequest);
        //                    }


        //                }
        //                #endregion

        //                var tranferRequest = new TransferSapRequest()
        //                {

        //                    FactoryCode = factory.FactoryCode,
        //                    EmployeeId = string.Empty,
        //                    TransferType = MaterialTransferType.GoodsIssue,
        //                    ConsumptionType = ConsumptionType.MaterialTransfer,
        //                    //WorkOrderId = workorder.Id,
        //                    //WorkOrderNo = workorder.OrderNumber,
        //                    Stocks = orderpickproducts.Select(x => new TransferStock()
        //                    {
        //                        MaterialCode = x.MaterialCode,
        //                        BatchCode = x.BatchCode,
        //                        Quantity = (decimal)x.Quantity,
        //                        BaseUnit = x.BaseUnit ?? "ST",
        //                    }).ToList(),
        //                    FromLocation = parameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault() == null ? "1100" : (parameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault()).Value,
        //                    ToLocation = parameterGroup.Items.Where(x => x.Key == "SAPLineStock").FirstOrDefault() == null ? "2100" : (parameterGroup.Items.Where(x => x.Key == "SAPLineStock").FirstOrDefault()).Value
        //                };

        //                var json = JsonConvert.SerializeObject(tranferRequest);
        //                await _mesApiClient.PostAsync<object, object>(transferUrl, tranferRequest);


        //                foreach (var item in boms)
        //                {
        //                    foreach (var stock in orderpickproducts.Where(x => x.MaterialCode == item.MaterialCode))
        //                    {
        //                        if (item.RequiredQuantity > 0 && stock.Quantity > 0)
        //                        {
        //                            var consumerecord = new WorkOrderOperationConsumptionRecordCreateDto()
        //                            {
        //                                WorkOrderId = workorder.Id,
        //                                WorkOrderProcessId = process.Id,
        //                                ReservationItem = item.ReservationItem,
        //                                MaterialCode = stock.MaterialCode,
        //                                BarCode = stock.BarCode,
        //                                BatchCode = stock.BatchCode,
        //                                Quantity = stock.Quantity,
        //                                BaseUnit = item.Unit,
        //                                ConsumptionType = (int)ConsumptionType.Consumption,
        //                            };
        //                            await _workOrderOperationConsumptionRecordService.CreateAsync(consumerecord);

        //                            if (item.RequiredQuantity > stock.Quantity)
        //                            {
        //                                item.RequiredQuantity -= stock.Quantity;
        //                                stock.Quantity = 0;
        //                                continue;
        //                            }
        //                            else
        //                            {
        //                                stock.Quantity -= item.RequiredQuantity;
        //                                break;
        //                            }
        //                        }

        //                    }
        //                }
                       

        //                var parameter = (await _parameterGroupService.GetGroupWithItemsAsync("TransferSAP")).Items.Where(x => x.Key == "IsEnabled").FirstOrDefault();
        //                if (parameter == null || bool.Parse(parameter.Value))
        //                {
        //                    var confirmResult = await _sapRfcService.ConfirmOrderCompletionToSAPAsync(sapconfirm.Id);
        //                    if (confirmResult != null && !string.IsNullOrWhiteSpace(confirmResult.MessageType) && confirmResult.MessageType.ToUpper() == "S")
        //                    {
        //                        await _workOrderOperationConfirmService.UpdateAsync(new WorkOrderOperationConfirmUpdateDto()
        //                        {
        //                            Id = sapconfirm.Id,
        //                            Message = confirmResult.Message,
        //                            MessageType = confirmResult.MessageType,
        //                            Status = "1",
        //                            //UpdatedBy = request.EmployeeId,
        //                            UpdatedAt = DateTime.Now,
        //                        });
        //                    }
   
        //                }

        //            }

        //            var requestUrl = _apiSettings["JyApi"].Endpoints["PickTaskApprove"];
        //            foreach (var item in jpitem)
        //            {
        //                var response = await _jyApiClient.PostAsync<object,object>(requestUrl, new { gid = Guid.NewGuid(), billcode = item,User = "admin",plant = "CN11" });
        //            }



        //        }

        //        return Ok(ApiResponse<bool>.Success(true));

        //    }
        //    catch (Exception ex)
        //    {

        //       return BadRequest(ApiResponse<object>.Fail($"提交出错：{ex.Message}"));
        //    }


        //}

        [HttpPut]
        public async Task<ActionResult<ApiResponse<bool>>> UpdateUnpackProcessAsync([FromBody] WorkOrderProcessUpdateDto dto, [FromQuery] string? LocationCode = null)
        {
            try
            {
                if (dto.Status == "verfSuccess")
                {
                    await _packageVerfService.PackageVerfUpdateAsync(dto);
                    //var result = await _packageVerfService.PackageVerfUpdateAsync(dto);
                    //if (result != null && result.Count() > 0)
                    //{
                    //    var requestUrl = _apiSettings["JyApi"].Endpoints["PickTaskApprove"];
                    //    foreach (var item in result)
                    //    {
                    //        var response = await _jyApiClient.PostAsync<object, object>(requestUrl, new
                    //        {
                    //            gid = Guid.NewGuid(),
                    //            billcode = item,
                    //            User = "admin",
                    //            plant = "CN11"
                    //        });
                    //    }
                    //}
                }

                else if (dto.Status == "verfReentry")
                {
                    await _packageVerfService.PackageVerfReentryAsync(dto);
                    //var result = await _packageVerfService.PackageVerfReentryAsync(dto);
                    //if (result != null && result.Count() > 0)
                    //{
                    //    var requestUrl = _apiSettings["JyApi"].Endpoints["PickTaskApprove"];
                    //    foreach (var item in result)
                    //    {
                    //        var response = await _jyApiClient.PostAsync<object, object>(requestUrl, new
                    //        {
                    //            gid = Guid.NewGuid(),
                    //            billcode = item,
                    //            User = "admin",
                    //            plant = "CN11"
                    //        });
                    //    }
                    //}
                }
                else
                    throw new Exception("合箱状态提交错误，无法提交！");

                return Ok(ApiResponse<bool>.Success(true));

            }
            catch (ArgumentException ex) // 处理特定的验证错误
            {
                return BadRequest(ApiResponse<object>.Fail($"提交无效：{ex.Message}"));
            }
            catch (KeyNotFoundException ex) // 处理 "未找到" 错误
            {
                return NotFound(ApiResponse<object>.Fail($"未找到：{ex.Message}"));
            }
            catch (InvalidOperationException ex) // 处理业务逻辑失败
            {
                return BadRequest(ApiResponse<object>.Fail($"操作失败：{ex.Message}"));
            }
            catch (Exception ex) // 处理所有其他意外错误
            {

                // 4. 向客户端返回通用的错误信息
                return BadRequest(ApiResponse<object>.Fail($"提交出错：{ex.Message}"));
            }


        }
    }

    public class WorkOrderPickVerfBomItem
    {
        public string ItemNo { get; set; }

        public int WorkOrderProcessId { get; set;}

        public string MaterialCode { get; set; }
        public string MaterialDesc { get; set; }
        public decimal Quantity { get; set; }
        public decimal CompletedQuantity { get; set; }
        public string ConsumeType { get; set; }
    }
}
