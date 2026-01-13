using Azure.Core;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Helper;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using Dm;
using Dm.util;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class PackageVerfApiService : IPackageVerfApiService
    {
        private const string StatusVerifySuccess = "verfSuccess";
        private const string ProductStockStatusCompleted = "2";
        private const string ProductStockStatusPending = "1";
        private const string SapSuccessMessageType = "S";
        private const string DefaultSapLocation = "2100";
        private const string DefaultBaseUnit = "ST";
        private const string SapParamGroup = "CN11SAPStockLocation";
        private const string SapRawMaterialStockKey = "SAPRawMtrStock";
        private const string SapLineStockKey = "SAPLineStock";
        private const string SapTransferParamGroup = "TransferSAP";
        private const string SapTransferEnabledKey = "IsEnabled";
        private const string WmsClosePickingMaterialStatus = "2";

        private const string PackConfirmWorkCenterParamGroup = "CN11AllowedConfirmWorkCenter";
        private const string PackConfirmWorkCenterKey = "PackWorkCenter";
        private readonly IWorkOrderService _workOrderService;
        private readonly IFactoryService _factoryService;
        private readonly IWorkOrderProcessService _workOrderProcessService;
        private readonly IWorkOrderBomItemService _workOrderBomItemService;
        private readonly IWorkOrderTaskService _workOrderTaskService;
        private readonly IProductLinesideStockService _productLinesideStockService;
        private readonly IMaterialViewService _materialViewService;
        private readonly IWorkOrderOperationConfirmService _workOrderOperationConfirmService;
        private readonly ISerialHelperService _serialHelperService;
        private readonly IWorkOrderOperationConsumptionRecordService _workOrderOperationConsumptionRecordService;
        private readonly IWorkOrderOperationConsumpService _workOrderOperationConsumpService;
        private readonly IParameterGroupService _parameterGroupService;
        private readonly ISapRfcService _sapRfcService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaterialTransferLogService _materialTransferLogService;
        private readonly IWorkOrderViewService _workOrderViewService;
        private readonly ICenterStockOutService _centerStockOutService;
        private readonly IJyApiClient _jyApiClient;
        private readonly Dictionary<string, ServiceEndpointSettings> _apiSettings;
        private readonly IActivityLogService _activityLogService;


        public PackageVerfApiService(
        IWorkOrderService workOrderService,
        IFactoryService factoryService,
        IWorkOrderProcessService workOrderProcessService,
        IWorkOrderBomItemService workOrderBomItemService,
        IWorkOrderTaskService workOrderTaskService,
        IProductLinesideStockService productLinesideStockService,
        IMaterialViewService materialViewService,
        IWorkOrderOperationConfirmService workOrderOperationConfirmService,
        ISerialHelperService serialHelperService,
        IWorkOrderOperationConsumptionRecordService workOrderOperationConsumptionRecordService,
        IWorkOrderOperationConsumpService workOrderOperationConsumpService,
        IParameterGroupService parameterGroupService,
        ISapRfcService sapRfcService, IUnitOfWork unitOfWork, IMaterialTransferLogService materialTransferLogService, IWorkOrderViewService workOrderViewService, ICenterStockOutService centerStockOutService, IJyApiClient jyApiClient,IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings, IActivityLogService activityLogService)
        {
            _workOrderService = workOrderService;
            _factoryService = factoryService;
            _workOrderProcessService = workOrderProcessService;
            _workOrderBomItemService = workOrderBomItemService;
            _workOrderTaskService = workOrderTaskService;
            _productLinesideStockService = productLinesideStockService;
            _materialViewService = materialViewService;
            _workOrderOperationConfirmService = workOrderOperationConfirmService;
            _serialHelperService = serialHelperService;
            _workOrderOperationConsumptionRecordService = workOrderOperationConsumptionRecordService;
            _workOrderOperationConsumpService = workOrderOperationConsumpService;
            _parameterGroupService = parameterGroupService;
            _sapRfcService = sapRfcService;
             _unitOfWork = unitOfWork;
            _materialTransferLogService = materialTransferLogService;
            _workOrderViewService = workOrderViewService;
            _centerStockOutService = centerStockOutService;
            _jyApiClient = jyApiClient;
            _apiSettings = apiSettings.Value;
            _activityLogService = activityLogService;

        }
        public async Task PackageVerfUpdateAsync(WorkOrderProcessUpdateDto dto, string? locationCode = null)
        {

            // 1. 验证输入并获取核心数据
            var workorder = await ValidateAndGetWorkOrderAsync(dto);

            var factory = await _factoryService.GetByIdAsync(workorder.FactoryId);
            if (factory == null)
            {
                throw new KeyNotFoundException($"未找到工厂 ID: {workorder.FactoryId}");
            }

            // 1.1 再次从wms同步按单物料
            var workorders = string.Join(",", workorder.OrderNumber).TrimEnd(',');
            var rtn = await _workOrderViewService.GetPickMtrStockByWorkOrderAsync(workorders);

            // 2. 一次性获取通用数据集
            var allBomItems = await _workOrderBomItemService.GetListByOrderIdAync(workorder.Id);
            var allProcesses = await _workOrderProcessService.GetListByOrderIdAync(workorder.Id);
            var pickingClosed = (await _centerStockOutService.GetListByWorkOrderAsync(workorder.OrderNumber)).Where(x => x.Status == WmsClosePickingMaterialStatus).ToList();

            var linesideProducts = await _productLinesideStockService.GetListByOrderNoAsync(workorder.OrderNumber);

            if (!allProcesses.Any())
            {
                throw new InvalidOperationException($"订单 {workorder.Id} 没有任何工序。");
            }
            var firstProcess = allProcesses.OrderBy(x => x.Operation).First();


            try
            {
                // 3. 执行业务逻辑检查（快速失败） - 您的验证点
                CheckProcessNotAlreadyFinished(firstProcess); // 检查是否已完成
                await CheckCableTasksCompletedAsync(workorder.OrderNumber, allBomItems); // 检查断线
                CheckOrderPickingCompleted(allBomItems, linesideProducts, pickingClosed); // 检查拣配

                // --- 所有检查通过，开始执行操作 ---

                var orderPickProducts = linesideProducts
                    .Where(x => x.BomItem == null && !string.IsNullOrWhiteSpace(x.Remark))
                    .ToList();

                // 4. 【核心更改】在一个事务中执行所有的数据库更新
                // 此方法现在包含创建报工和消耗记录的所有逻辑。
                var (OpConfirmId,transferLogDtos) = await RunDatabaseUpdatesInTransactionAsync(
                    firstProcess, workorder, factory, dto.UpdateBy, linesideProducts, allBomItems, orderPickProducts);


                if (transferLogDtos != null && transferLogDtos.Count() > 0)
                {
                    // 5. 【外部 API 调用】在数据库事务成功后，执行外部 API 调用

                    // 5a. 处理 SAP 库存移库 (调用 MES API)
                    await HandleSapStockTransfersAsync(transferLogDtos);

                    //订单非单道工序且在工作中心白名单中进行报工
                //    var allowconfirmworkcenters = (await _parameterGroupService.GetGroupWithItemsAsync(PackConfirmWorkCenterParamGroup))
                //?.Items.FirstOrDefault(x => x.Key == PackConfirmWorkCenterKey).Value;

                //    if (allProcesses.Count() > 1 && allowconfirmworkcenters.Split(",").Contains(firstProcess.WorkCenter))
                //    {
                //        await ConfirmCompletionToSapAsync(sapConfirm);
                //    }

                    // 5c. 通知外部 JY API
                    //return linesideProducts
                    //    .Where(x => x.BomItem == null && x.Remark != null)
                    //    .Select(x => x.Remark.Split("-")[1])
                    //    .Distinct()
                    //    .ToList();

                    var requestUrl = _apiSettings["JyApi"].Endpoints["PickTaskApprove"];
                    List<ActivityLogCreateDto> logs = new List<ActivityLogCreateDto>();
                    foreach (var item in linesideProducts
                        .Where(x => x.BomItem == null && x.Remark != null)
                        .Select(x => x.Remark.Split("-")[1])
                        .Distinct()
                        .ToList())
                    {
                        var response = await _jyApiClient.PostAsync<object, object>(requestUrl, new
                        {
                            gid = Guid.NewGuid(),
                            billcode = item,
                            User = "admin",
                            plant = "CN11"
                        });
                        logs.Add(new ActivityLogCreateDto
                        {
                            LogType = "APIINFO",
                            LogContent = "WMSPickTaskApprove",
                            Details
                            = $"单据号: {item}，响应: {response.ToString()}",
                            UserName = dto.UpdateBy,
                            Timestamp = DateTime.Now
                        });
                    }

                    if (logs != null)
                    {
                        await _activityLogService.CreateBatchAsync(logs);
                    }
                }

                // 6. 向 SAP 提报完工 (调用 SAP RFC)

                if (OpConfirmId != null && OpConfirmId > 0)
                {
                    var sapConfirm = await _workOrderOperationConfirmService.GetByIdAsync((int)OpConfirmId);
                    await ConfirmCompletionToSapAsync(sapConfirm);
                }

            }
            catch (Exception)
            {

                throw;
            }
          
        }

        public async Task PackageVerfReentryAsync(WorkOrderProcessUpdateDto dto, string? locationCode = null)
        {

            // 1. 验证输入并获取核心数据
            var workorder = await ValidateAndGetWorkOrderAsync(dto);

            var factory = await _factoryService.GetByIdAsync(workorder.FactoryId);
            if (factory == null)
            {
                throw new KeyNotFoundException($"未找到工厂 ID: {workorder.FactoryId}");
            }

            // 2. 一次性获取通用数据集
            var allBomItems = await _workOrderBomItemService.GetListByOrderIdAync(workorder.Id);
            var allProcesses = await _workOrderProcessService.GetListByOrderIdAync(workorder.Id);
            var linesideProducts = await _productLinesideStockService.GetListByOrderNoAsync(workorder.OrderNumber);
            var pickingClosed = (await _centerStockOutService.GetListByWorkOrderAsync(workorder.OrderNumber)).Where(x => x.Status == WmsClosePickingMaterialStatus).ToList();

            if (!allProcesses.Any())
            {
                throw new InvalidOperationException($"订单 {workorder.Id} 没有任何工序。");
            }
            var firstProcess = allProcesses.OrderBy(x => x.Operation).First();


            try
            {
                // 3. 执行业务逻辑检查（快速失败） - 您的验证点
                //CheckProcessNotAlreadyFinished(firstProcess); // 检查是否已完成
                await CheckCableTasksCompletedAsync(workorder.OrderNumber, allBomItems); // 检查断线
                CheckOrderPickingCompleted(allBomItems, linesideProducts, pickingClosed); // 检查拣配

                // --- 所有检查通过，开始执行操作 ---

                var orderPickProducts = linesideProducts
                    .Where(x => x.BomItem == null && !string.IsNullOrWhiteSpace(x.Remark))
                    .ToList();

                // 4. 【核心更改】在一个事务中执行所有的数据库更新
                // 此方法现在包含创建报工和消耗记录的所有逻辑。
                var (OpconfirmId,transferLogDtos) = await RunDatabaseUpdatesInTransactionAsync(
                      firstProcess, workorder, factory, dto.UpdateBy, linesideProducts, allBomItems, orderPickProducts);


                if (transferLogDtos != null)
                {
                    // 5. 【外部 API 调用】在数据库事务成功后，执行外部 API 调用

                    // 5a. 处理 SAP 库存移库 (调用 MES API)
                    await HandleSapStockTransfersAsync(transferLogDtos);

                    // 5b. 向 SAP 提报完工 (调用 SAP RFC)

                    //订单非单道工序且在工作中心白名单中进行报工
                //    var allowconfirmworkcenters = (await _parameterGroupService.GetGroupWithItemsAsync(PackConfirmWorkCenterParamGroup))
                //?.Items.FirstOrDefault(x => x.Key == PackConfirmWorkCenterKey).Value;

                //    if (allProcesses.Count() > 1 && allowconfirmworkcenters.Split(",").Contains(firstProcess.WorkCenter))
                //    {
                //        await ConfirmCompletionToSapAsync(sapConfirm);
                //    }

                    // 5c. 通知外部 JY API
                    //return linesideProducts
                    //    .Where(x => x.BomItem == null && x.Remark != null)
                    //    .Select(x => x.Remark.Split("-")[1])
                    //    .Distinct()
                    //    .ToList();

                    var requestUrl = _apiSettings["JyApi"].Endpoints["PickTaskApprove"];
                    foreach (var item in linesideProducts
                        .Where(x => x.BomItem == null && x.Remark != null)
                        .Select(x => x.Remark.Split("-")[1])
                        .Distinct()
                        .ToList())
                    {
                        var response = await _jyApiClient.PostAsync<object, object>(requestUrl, new
                        {
                            gid = Guid.NewGuid(),
                            billcode = item,
                            User = "admin",
                            plant = "CN11"
                        });
                    }
                }

                // 6. 向 SAP 提报完工 (调用 SAP RFC)

                if (OpconfirmId != null && OpconfirmId > 0)
                {
                    var sapConfirm = await _workOrderOperationConfirmService.GetByIdAsync((int)OpconfirmId);
                    await ConfirmCompletionToSapAsync(sapConfirm);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        #region 私有验证和检查方法
        // ... (ValidateAndGetWorkOrderAsync, CheckProcessNotAlreadyFinished, CheckCableTasksCompletedAsync, CheckOrderPickingCompleted 保持不变)
        /// <summary>
        /// 验证 DTO 并获取工单和工厂信息。
        /// </summary>
        private async Task<WorkOrderDto> ValidateAndGetWorkOrderAsync(WorkOrderProcessUpdateDto dto)
        {
            if (dto == null || dto.WorkOrderId == null || dto.WorkOrderId <= 0)
            {
                throw new ArgumentException("订单ID为空，无法提交");
            }

            var workorder = await _workOrderService.GetByIdAsync((int)dto.WorkOrderId);
            if (workorder == null)
            {
                throw new KeyNotFoundException($"未查询到订单信息，ID: {dto.WorkOrderId}");
            }

            return workorder;
        }

        /// <summary>
        /// 检查工序是否已经完成。
        /// </summary>
        private void CheckProcessNotAlreadyFinished(WorkOrderProcessDto process)
        {
            if (process.Status == ((int)WorkOrderStatus.Finished).ToString())
            {
                throw new InvalidOperationException("当前工序已完成，无法重复提交！");
            }
        }

        /// <summary>
        /// 检查断线物料任务是否全部完成。
        /// </summary>
        private async Task CheckCableTasksCompletedAsync(string orderNumber, IEnumerable<WorkOrderBomItemDto> allBomItems)
        {
            var cableItems = allBomItems
                .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true && x.ConsumeType == (int)ConsumeType.CableMaterial)
                .ToList();

            if (!cableItems.Any())
            {
                return; // 没有需要检查的断线物料
            }

            var tasks = (await _workOrderTaskService.GetByOrderNoAsync(orderNumber)).ToList();

            // 使用更清晰的 LINQ 或循环来检查
            bool allTasksCompleted = true;
            foreach (var item in cableItems)
            {
                // 检查是否存在匹配的任务
                bool taskExists = tasks.Any(t => t.OrderProcessId == item.WorkOrderProcessId && t.MaterialItem == item.BomItem);
                if (!taskExists)
                {
                    allTasksCompleted = false;
                    break;
                }
                //断线任务挂起不进入合箱验证
                bool taskUnCompleted = tasks.Any(t => t.OrderProcessId == item.WorkOrderProcessId && t.MaterialItem == item.BomItem && t.Quantity != t.CompletedQty && t.Status != ((int)WorkOrderStatus.Paused).ToString());
                if (taskUnCompleted)
                {
                    allTasksCompleted = false;
                    break;
                }
            }

            if (!allTasksCompleted)
            {
                throw new InvalidOperationException("断线物料任务未全部完成，无法提交！");
            }
        }

        /// <summary>
        /// 检查按单物料是否全部拣货完成。
        /// </summary>
        private void CheckOrderPickingCompleted(IEnumerable<WorkOrderBomItemDto> allBomItems, IEnumerable<ProductLinesideStockDto> linesideProducts,List<CenterStockOutDto>? centerStocks = null)
        {
            var orderPickItems = allBomItems
                .Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true && x.ConsumeType == (int)ConsumeType.OrderBasedMaterial)
                .GroupBy(x => x.MaterialCode)
                .Select(g => new { MaterialCode = g.Key, RequiredQuantity = g.Sum(item => item.RequiredQuantity) })
                .ToList();

            if (!orderPickItems.Any())
            {
                return; // 没有需要检查的按单物料
            }

            var productSummary = linesideProducts
                .GroupBy(p => p.MaterialCode)
                .ToDictionary(g => g.Key, g => g.Sum(item => item.Quantity));

            foreach (var item in orderPickItems)
            {
                //wms强制关闭的物料，不进行物料数量验证
                if (centerStocks != null && centerStocks.Count() > 0 && centerStocks.Any(x => x.MaterialCode == item.MaterialCode))
                {
                    continue;
                }
                else
                {
                    if (!productSummary.TryGetValue(item.MaterialCode, out decimal? pickedQuantity) || item.RequiredQuantity > pickedQuantity)
                    {
                        throw new InvalidOperationException($"按单物料拣货未全部完成 (物料: {item.MaterialCode}，需求: {item.RequiredQuantity}，已拣: {(pickedQuantity == null || pickedQuantity == 0 ? 0 : pickedQuantity)})，无法提交！");
                    }
                }

            }
        }

        #endregion

        #region 私有操作方法

        /// <summary>
        /// 【新方法】将所有数据库操作合并到此方法中，以便于事务管理。
        /// 此方法应在 TransactionScope 或 Unit of Work 中调用。
        /// </summary>
        private async Task<(int?, List<MaterialTransferLogDto>)> RunDatabaseUpdatesInTransactionAsync(
            WorkOrderProcessDto process, WorkOrderDto workorder, FactoryDto factory, string updateUser,
            List<ProductLinesideStockDto> linesideProducts,
            List<WorkOrderBomItemDto> allBomItems,
            List<ProductLinesideStockDto> orderPickProducts)
        {

            await _unitOfWork.BeginTransactionAsync();
            try
            {
                // 1. 更新线边库存状态
                var productsToUpdate = linesideProducts.Where(x => x.Status == ProductStockStatusPending).Select(x => new ProductLinesideStockUpdateDto()
                {
                    Id = x.Id,
                    Status = ProductStockStatusCompleted, // "2"
                    UpdatedAt = DateTime.Now,
                }).ToList();

                if (productsToUpdate.Any())
                {
                    await _productLinesideStockService.UpdateStatusAsync(productsToUpdate);
                }

                // 2. 更新工序状态
                var processToUpdate = new WorkOrderProcessUpdateDto()
                {
                    Id = process.Id,
                    //CompletedQuantity = workorder.Quantity,
                    ActEndTime = DateTime.Now,
                    Status = ((int)WorkOrderStatus.Finished).ToString(),
                    UpdateOn = DateTime.Now,
                    UpdateBy = updateUser
                };
                await _workOrderProcessService.UpdateAsync(processToUpdate);

                // 3. 按单物料创建移库记录

                var parameterGroup = await _parameterGroupService.GetGroupWithItemsAsync(SapParamGroup);
                string fromLocation = parameterGroup?.Items.FirstOrDefault(x => x.Key == SapRawMaterialStockKey)?.Value ?? "1100";
                string toLocation = parameterGroup?.Items.FirstOrDefault(x => x.Key == SapLineStockKey)?.Value ?? DefaultSapLocation; // "2100"
                var materialTransferLogIds = new List<int>();
                var materialTransferLogDtos = new List<MaterialTransferLogDto>();
                var transferNews = new List<MaterialTransferLogCreateDto>();
                foreach (var item in orderPickProducts)
                {
                    //var transferdto = new MaterialTransferLogDto();

                    var transferExists = await _materialTransferLogService.GetListByStockIdAsync(item.Id);
                    if (transferExists != null && transferExists.Where(x => x.WorkOrderNo == item.WorkOrderNo && x.FromLocationCode == fromLocation && x.ToLocationCode == toLocation).Count() > 0)
                    {
                        materialTransferLogIds.Add(transferExists.Where(x => x.WorkOrderNo == item.WorkOrderNo && x.FromLocationCode == fromLocation && x.ToLocationCode == toLocation).First().Id);
                        //transferdto = transferExists.Where(x => x.WorkOrderNo == item.WorkOrderNo && x.FromLocationCode == fromLocation && x.ToLocationCode == toLocation).First();
                    }
                    else
                    {
                        transferNews.Add(new MaterialTransferLogCreateDto()
                        {
                            TransferNo = _serialHelperService.GenerateNext($"{factory.FactoryCode}MaterialTransferToSAPSerial"),
                            TransferType = "03",
                            PostingDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                            DocumentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                            FactoryCode = factory.FactoryCode,
                            MaterialCode = item.MaterialCode,
                            BatchCode = item.BatchCode,
                            FromStockId = item.Id,
                            WorkOrderId = item.WorkOrderId,
                            WorkOrderNo = item.WorkOrderNo,
                            MovementType = ((int)ConsumptionType.MaterialTransfer).ToString(),
                            Quantity = item.Quantity,
                            BaseUnit = item.BaseUnit,
                            ReceivingMaterialCode = item.MaterialCode,
                            ReceivingBatchCode = item.BatchCode,
                            ToFactoryCode = factory.FactoryCode,
                            FromLocationCode = fromLocation,
                            ToLocationCode = toLocation,
                        });
                        //transferdto = await _materialTransferLogService.CreateAsync(new MaterialTransferLogCreateDto()
                        //{
                        //    TransferNo = _serialHelperService.GenerateNext($"{factory.FactoryCode}MaterialTransferToSAPSerial"),
                        //    TransferType = "03",
                        //    PostingDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                        //    DocumentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                        //    FactoryCode = factory.FactoryCode,
                        //    MaterialCode = item.MaterialCode,
                        //    BatchCode = item.BatchCode,
                        //    FromStockId = item.Id,
                        //    WorkOrderId = item.WorkOrderId,
                        //    WorkOrderNo = item.WorkOrderNo,
                        //    MovementType = ((int)ConsumptionType.MaterialTransfer).ToString(),
                        //    Quantity = item.Quantity,
                        //    BaseUnit = item.BaseUnit,
                        //    ReceivingMaterialCode = item.MaterialCode,
                        //    ReceivingBatchCode = item.BatchCode,
                        //    ToFactoryCode = factory.FactoryCode,
                        //    FromLocationCode = fromLocation,
                        //    ToLocationCode = toLocation,
                        //});
                    }
                    //materialTransferLogDtos.Add(transferdto);
                }

                var transferTempIds = await _materialTransferLogService.CreateBatchAsync(transferNews);
                materialTransferLogIds.AddRange(transferTempIds);
                materialTransferLogDtos = await _materialTransferLogService.GetListByIdsAsync(materialTransferLogIds);



                // 5. 为按单物料创建消耗记录（原 CreateOrderBasedConsumptionRecordsAsync 的逻辑）
                var boms = allBomItems
                    .Where(x => x.MovementAllowed == true && x.ConsumeType == (int)ConsumeType.OrderBasedMaterial && x.RequiredQuantity > 0)
                    .ToList();

                var availableProducts = orderPickProducts
                    .Select(p => new ProductLinesideStock { MaterialCode = p.MaterialCode, BatchCode = p.BatchCode, Quantity = p.Quantity, BarCode = p.BarCode, BaseUnit = p.BaseUnit })
                    .ToList();

                var consumptionRecordsToCreate = new List<WorkOrderOperationConsumptionRecordCreateDto>();
                var consumptionRecordsExist = await _workOrderOperationConsumptionRecordService.GetListByProcessIdAsync(process.Id);
                foreach (var bomItem in boms)
                {
                    decimal neededForBom = (decimal)bomItem.RequiredQuantity;

                    var matchingProducts = availableProducts
                        .Where(p => p.MaterialCode == bomItem.MaterialCode && p.Quantity > 0)
                        .OrderBy(p => p.BatchCode)
                        .ToList();

                    foreach (var stock in matchingProducts)
                    {
                        if (neededForBom == 0)
                            break;

                        if (consumptionRecordsExist.Exists(x => x.ReservationItem == bomItem.ReservationItem && x.MaterialCode == stock.MaterialCode && x.BatchCode == stock.BatchCode && x.BarCode == stock.BarCode))
                        {
                            await _workOrderOperationConsumptionRecordService.DeleteAsync(consumptionRecordsExist.Where(x => x.ReservationItem == bomItem.ReservationItem && x.MaterialCode == stock.MaterialCode && x.BatchCode == stock.BatchCode && x.BarCode == stock.BarCode).Select(x => x.Id).ToList());
                        }
                        decimal consumeQuantity = Math.Min(neededForBom, (decimal)stock.Quantity);

                        consumptionRecordsToCreate.Add(new WorkOrderOperationConsumptionRecordCreateDto()
                        {
                            WorkOrderId = workorder.Id,
                            WorkOrderProcessId = process.Id,
                            ReservationItem = bomItem.ReservationItem,
                            MaterialCode = stock.MaterialCode,
                            BarCode = stock.BarCode,
                            BatchCode = stock.BatchCode,
                            Quantity = consumeQuantity,
                            BaseUnit = bomItem.Unit,
                            ConsumptionType = (int)ConsumptionType.Consumption,
                        });

                        neededForBom -= consumeQuantity;
                        stock.Quantity -= consumeQuantity;
                    }
                }

                if (consumptionRecordsToCreate.Any())
                {
                    foreach (var record in consumptionRecordsToCreate)
                    {
                        await _workOrderOperationConsumptionRecordService.CreateAsync(record);
                    }
                }


                #region 创建报工记录 
                //未创建过报工记录&只有一道工序&只有断线物料，创建报工记录
                int? OpConfirmId = null;
                var sapConfirm = (await _workOrderOperationConfirmService.GetListByProcessIdAsync(process.Id)).FirstOrDefault();
                var processes = await _workOrderProcessService.GetListByOrderIdAync(workorder.Id);
                if (processes != null && processes.Count() == 1 && allBomItems.Where(b => b.ConsumeType != (int)ConsumeType.CableMaterial).Count() == 0)
                {
                    if (sapConfirm == null)
                    {
                        // 3. 创建 SAP 报工主记录
                        var materialPro = await _materialViewService.GetByCodeAsync(factory.FactoryCode, workorder.MaterialCode);
                        sapConfirm = await _workOrderOperationConfirmService.CreateAsync(new WorkOrderOperationConfirmCreateDto()
                        {
                            WorkOrderId = workorder.Id,
                            ProcessId = process.Id,
                            SapConfirmationNo = process.ConfirmNo,
                            WorkOrderNo = workorder.OrderNumber.PadLeft(12, '0'),
                            OperationNo = process.Operation,
                            CompletedFlag = "X",
                            ConfirmSequence = _serialHelperService.GenerateNext($"{factory.FactoryCode}ConfirmReportToSAP"),
                            WorkCenterCode = process.WorkCenter,
                            PostingDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                            FactoryCode = factory.FactoryCode,
                            BaseUnit = materialPro?.BaseUnit ?? DefaultBaseUnit,
                            YieldQuantity = workorder.Quantity,
                            ScrapQuantity = 0,
                            ActFinishDate = ((DateTime)processToUpdate.ActEndTime).ToString("yyyyMMdd"),
                            ActFinishTime = ((DateTime)processToUpdate.ActEndTime).ToString("HH:mm:ss"),
                        });

  
                    }
                    else
                    {
                        var confirmwithConsume = await _workOrderOperationConfirmService.GetConfirmWitemConsumeptionAsync(sapConfirm.Id);
                        await _workOrderOperationConsumpService.DeleteAsync(confirmwithConsume.Consumps.Select(x => x.Id).ToList());
                    }
                    OpConfirmId = sapConfirm.Id;
                    // 4. 创建 SAP 消耗记录（针对已有的 MES 消耗）
                    var consumemes = await _workOrderOperationConsumptionRecordService.GetListByProcessIdAsync(process.Id);
                    if (consumemes.Any())
                    {
                        var consumptionDtos = consumemes.GroupBy(g => new
                        {
                            g.ReservationItem,
                            g.MaterialCode,
                            g.BatchCode,
                            g.BaseUnit,
                            g.ConsumptionType,
                            g.ConsumptionRemark
                        })
                        .Select(s => new WorkOrderOperationConsumpCreateDto()
                        {
                            OperationConfirmId = sapConfirm.Id,
                            SapConfirmationNo = sapConfirm.SapConfirmationNo,
                            WorkOrderNo = workorder.OrderNumber.PadLeft(12, '0'),
                            ConfirmSequence = sapConfirm.ConfirmSequence,
                            ReservationNo = workorder.ReservationNo,
                            ReservationItem = s.Key.ReservationItem,
                            MaterialCode = s.Key.MaterialCode.StartsWith("E") ? s.Key.MaterialCode : s.Key.MaterialCode.PadLeft(18, '0'),
                            FactoryCode = factory.FactoryCode,
                            FromLocationCode = DefaultSapLocation, // "2100"
                            BatchCode = s.Key.BatchCode,
                            MovementType = ((int)s.Key.ConsumptionType).ToString(),
                            MovementReason = s.Key.ConsumptionRemark ?? "",
                            Quantity = s.Sum(ss => ss.Quantity),
                            BaseUnit = s.Key.BaseUnit,
                        }).ToList();
                        await _workOrderOperationConsumpService.CreateAsync(consumptionDtos);
                    }
                }






                #endregion


                await _unitOfWork.CommitAsync();
                // 6. 返回 SAP 报工主记录，供后续 API 调用使用
                return (OpConfirmId,materialTransferLogDtos);
            }
            catch (Exception ex)
            {
                try
                {
                    await _unitOfWork.RollbackAsync();
                }
                catch
                {
                }

                // 【修改点】：不要只抛出字符串，把 ex.Message 带上，
                // 这样前端就能看到是 "违反唯一约束" 还是 "超时" 还是 "空引用"
                throw new Exception($"报工记录创建失败：{ex.Message}", ex);
            }

        }

        /// <summary>
        /// 处理 SAP 库存检查和移库（包括批次分配）。
        /// 【外部 API 调用】
        /// </summary>
        private async Task HandleSapStockTransfersAsync(FactoryDto factory, WorkOrderOperationConfirmDto confirm,List<ProductLinesideStockDto> orderPickProducts)
        {

            if (!confirm.Consumps.Any())
            {
                return;
            }

            var parameterGroup = await _parameterGroupService.GetGroupWithItemsAsync(SapParamGroup);
            string fromLocation = parameterGroup?.Items.FirstOrDefault(x => x.Key == SapRawMaterialStockKey)?.Value ?? "1100";
            string toLocation = parameterGroup?.Items.FirstOrDefault(x => x.Key == SapLineStockKey)?.Value ?? DefaultSapLocation; // "2100"
            //var consumeRecord = await _workOrderOperationConsumptionRecordService.GetListByProcessIdAsync((int)confirm.ProcessId);
            var materialTransferLogDtos = new List<MaterialTransferLogDto>();
            foreach (var item in orderPickProducts)
            {
                var transferdto = new MaterialTransferLogDto();
                var transferExists = await _materialTransferLogService.GetListByStockIdAsync(item.Id);
                if (transferExists != null && transferExists.Where(x => x.WorkOrderNo == item.WorkOrderNo && x.FromLocationCode == fromLocation && x.ToLocationCode == toLocation).Count() > 0)
                {
                    transferdto = transferExists.Where(x => x.WorkOrderNo == item.WorkOrderNo && x.FromLocationCode == fromLocation && x.ToLocationCode == toLocation).First();
                }
                else
                {
                    transferdto = await _materialTransferLogService.CreateAsync(new MaterialTransferLogCreateDto()
                    {
                        TransferNo = _serialHelperService.GenerateNext($"{factory.FactoryCode}MaterialTransferToSAPSerial"),
                        TransferType = "03",
                        PostingDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                        DocumentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
                        FactoryCode = factory.FactoryCode,
                        MaterialCode = item.MaterialCode,
                        BatchCode = item.BatchCode,
                        FromStockId = item.Id,
                        WorkOrderId = item.WorkOrderId,
                        WorkOrderNo = item.WorkOrderNo,
                        MovementType = ((int)ConsumptionType.MaterialTransfer).ToString(),
                        Quantity = item.Quantity,
                        BaseUnit = item.BaseUnit,
                        ReceivingMaterialCode = item.MaterialCode,
                        ReceivingBatchCode = item.BatchCode,
                        ToFactoryCode = factory.FactoryCode,
                        FromLocationCode = fromLocation,
                        ToLocationCode = toLocation,
                    });
                }
                materialTransferLogDtos.Add(transferdto);
            }


            #region 停止使用
            // 1. 获取 SAP 原始库存
            //var sapStocks = (await _sapRfcService.GetRawMaterialStockFromSapAsync(
            //    confirm.Consumps.GroupBy(s => s.MaterialCode).Select(x => new SapRawMaterialStockDto()
            //    {
            //        MaterialCode = x.Key,
            //        FactoryCode = factory.FactoryCode,
            //        LocationCode = fromLocation,
            //    }).ToList()
            //)).Select(x => new SapRawMaterialStockDto()
            //{
            //    MaterialCode = x.MaterialCode.TrimStart('0'),
            //    FactoryCode = x.FactoryCode,
            //    LocationCode = x.LocationCode,
            //    BatchCode = x.BatchCode,
            //    Quantity = x.Quantity
            //}).ToList();

            //// 2. 检查总库存是否满足总需求
            //if (CheckSapStockAvailability(confirm.Consumps, sapStocks))
            //{
            //    // 3. 分配库存并更新批次（批次到批次）
            //    var batchToBatchTransfers = await AllocateStockForBatchTransfers(confirm.Consumps, sapStocks);
            //    if (batchToBatchTransfers.Any())
            //    {
            //        var stockParameterGroup =  await _parameterGroupService.GetGroupWithItemsAsync("CN11SAPStockLocation");
            //        //var TransferNo = _serialHelperService.GenerateNext($"{factory.FactoryCode}MaterialTransferToSAPSerial");
            //        foreach (var stock in batchToBatchTransfers)
            //        {
            //            var transferdto = await _materialTransferLogService.CreateAsync(new MaterialTransferLogCreateDto()
            //            {
            //                TransferNo = _serialHelperService.GenerateNext($"{factory.FactoryCode}MaterialTransferToSAPSerial"),
            //                TransferType ="03",
            //                PostingDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
            //                DocumentDate = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")),
            //                FactoryCode = factory.FactoryCode,
            //                MaterialCode = stock.MaterialCode,
            //                BatchCode = stock.BatchCode,
            //                WorkOrderNo = stock.WorkOrderNo,
            //                MovementType = ((int)ConsumptionType.MaterialTransfer).ToString(),
            //                Quantity = stock.Quantity,
            //                BaseUnit = stock.BaseUnit,
            //                ReceivingMaterialCode = stock.MaterialCode,
            //                ReceivingBatchCode = stock.BatchCode,
            //                ToFactoryCode = factory.FactoryCode,
            //                FromLocationCode = stockParameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault() == null ? "1100" : (stockParameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault()).Value,
            //                ToLocationCode = stockParameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault() == null ? "1100" : (stockParameterGroup.Items.Where(x => x.Key == "SAPRawMtrStock").FirstOrDefault()).Value,
            //            });

            //            materialTransferLogDtos.Add(transferdto);
            //        }



            //        await _workOrderOperationConsumpService.DeleteAsync(confirm.Consumps.Select(X => X.Id).ToList());
            //        await _workOrderOperationConsumpService.CreateAsync(batchToBatchTransfers);
            //    }
            
            //}
            #endregion

            //3. 将sap库存从1100 移动至2100
            var result = await _sapRfcService.MaterialStockTransferToSAPAsync(materialTransferLogDtos);

            //更新log记录
            await _materialTransferLogService.UpdateListAsync(result.Select(x => new MaterialTransferLogUpdateDto()
            {
                Id = x.Id,
                Status = x.MessageType.ToUpper() == "S"?"1":"-1",
                Message = x.Message,
                MessageType = x.MessageType,
                UpdatedAt = DateTime.Now
            }).ToList());

        }

        private async Task HandleSapStockTransfersAsync(List<MaterialTransferLogDto> materialTransferLogs)
        {

            if (!materialTransferLogs.Any())
            {
                return;
            }
            //3. 将sap库存从1100 移动至2100
            var result = await _sapRfcService.MaterialStockTransferToSAPAsync(materialTransferLogs);

            //更新log记录
            try
            {
                await _materialTransferLogService.UpdateListAsync(result.Select(x => new MaterialTransferLogUpdateDto()
                {
                    Id = x.Id,
                    Status = x.MessageType.ToUpper() == "S" ? "1" : "-1",
                    Message = x.Message,
                    MessageType = x.MessageType,
                    UpdatedAt = DateTime.Now
                }).ToList());
            }
            catch (Exception)
            {
                throw;
            }

        }

        // ... (CheckSapStockAvailability 和 AllocateStockForBatchTransfers 保持不变)
        /// <summary>
        /// 检查 SAP 总库存是否满足需求。
        /// </summary>
        private bool CheckSapStockAvailability(List<WorkOrderOperationConsumpDto> demands, List<SapRawMaterialStockDto> stocks)
        {
            var stockSummary = stocks.GroupBy(s => s.MaterialCode)
                                     .ToDictionary(g => g.Key, g => g.Sum(item => (decimal)item.Quantity));

            var demandSummary = demands.GroupBy(d => d.MaterialCode)
                                       .ToDictionary(g => g.Key, g => g.Sum(item => (decimal)item.Quantity));

            foreach (var demandEntry in demandSummary)
            {
                stockSummary.TryGetValue(demandEntry.Key, out decimal totalStock);
                if (totalStock < demandEntry.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckSapStockAvailability(List<WorkOrderOperationConsumpDto> demands, List<SapRawMaterialStockDto> stocks, string materialCode)
        {
            var stockSummary = stocks.Where(x => x.MaterialCode == materialCode).GroupBy(s => s.MaterialCode == materialCode).Select(g => g.Sum(item => (decimal)item.Quantity)).First();

            var demandSummary = demands.Where(x => x.MaterialCode == materialCode).GroupBy(s => s.MaterialCode == materialCode).Select(g => g.Sum(item => (decimal)item.Quantity)).First();


            if (stockSummary > demandSummary)
            {
                return true;
            }
            return false;

      
        }

        /// <summary>
        /// 为批次到批次的移库分配库存。
        /// </summary>
        private async Task<List<WorkOrderOperationConsumpCreateDto>> AllocateStockForBatchTransfers(List<WorkOrderOperationConsumpDto> demands, List<SapRawMaterialStockDto> sapStocks)
        {
            var WorkOrderOperationConsumpNew = new List<WorkOrderOperationConsumpCreateDto>();

            // 创建一个可修改的库存列表副本，以便跟踪剩余数量
            var availableStocks = sapStocks.Select(s => new SapRawMaterialStockDto
            {
                MaterialCode = s.MaterialCode,
                BatchCode = s.BatchCode,
                Quantity = s.Quantity
            }).ToList();

            foreach (var demand in demands)
            {

                if (CheckSapStockAvailability(demands, sapStocks, demand.MaterialCode))
                {

                }
                decimal neededQuantity = (decimal)demand.Quantity;

                // 1. 尝试从完全匹配的批次中扣除
                var matchingBatch = availableStocks.FirstOrDefault(s =>
                    s.MaterialCode == demand.MaterialCode &&
                    s.BatchCode == demand.BatchCode &&
                    s.Quantity > 0);

                if (matchingBatch != null)
                {
                    decimal takeQuantity = Math.Min(neededQuantity, (decimal)matchingBatch.Quantity);
                    matchingBatch.Quantity -= takeQuantity;
                    neededQuantity -= takeQuantity;
                    WorkOrderOperationConsumpNew.Add(new WorkOrderOperationConsumpCreateDto() 
                    {
                        OperationConfirmId = demand.OperationConfirmId,
                        SapConfirmationNo = demand.SapConfirmationNo,
                        WorkOrderNo = demand.WorkOrderNo,
                        ConfirmSequence = demand.ConfirmSequence,
                        ReservationNo = demand.ReservationNo,
                        ReservationItem = demand.ReservationItem,
                        MaterialCode = demand.MaterialCode,
                        BatchCode = demand.BatchCode,
                        Quantity = takeQuantity,
                        BaseUnit = demand.BaseUnit,
                        FromLocationCode = demand.FromLocationCode,
                        MovementType = demand.MovementType,
                        MovementReason = demand.MovementReason,
                        FactoryCode = demand.FactoryCode,
                        CreatedBy = demand.CreatedBy,

                    });
                }

                // 2. 如果还需要，从其他批次中扣除（FIFO - 假设按批次代码排序）
                if (neededQuantity > 0)
                {
                    var otherBatches = availableStocks.Where(s =>
                        s.MaterialCode == demand.MaterialCode &&
                        s.BatchCode != demand.BatchCode &&
                        s.Quantity > 0)
                        .OrderBy(s => s.BatchCode) // FIFO 或其他业务规则
                        .ToList();

                    foreach (var batch in otherBatches)
                    {
                        decimal takeQuantity = Math.Min(neededQuantity, (decimal)batch.Quantity);

                        WorkOrderOperationConsumpNew.Add(new WorkOrderOperationConsumpCreateDto()
                        {
                            OperationConfirmId = demand.OperationConfirmId,
                            SapConfirmationNo = demand.SapConfirmationNo,
                            WorkOrderNo = demand.WorkOrderNo,
                            ConfirmSequence = demand.ConfirmSequence,
                            ReservationNo = demand.ReservationNo,
                            ReservationItem = demand.ReservationItem,
                            MaterialCode = demand.MaterialCode,
                            BatchCode = batch.BatchCode,
                            Quantity = takeQuantity,
                            BaseUnit = demand.BaseUnit,
                            FromLocationCode = batch.LocationCode,
                            MovementType = demand.MovementType,
                            MovementReason = demand.MovementReason,
                            FactoryCode = batch.FactoryCode,
                            CreatedBy = demand.CreatedBy,

                        });

                        batch.Quantity -= takeQuantity;
                        neededQuantity -= takeQuantity;

                        if (neededQuantity == 0)
                            break;
                    }
                }

                //if (neededQuantity > 0)
                //{
                //    // 这不应该发生，因为我们已经在 CheckSapStockAvailability 中检查了总数
                //    throw new Exception($"物料 {demand.MaterialCode} 的库存分配逻辑失败。仍需 {neededQuantity}。");
                //    // 考虑抛出异常
                //}
            }

            return WorkOrderOperationConsumpNew;
        }


        /// <summary>
        /// 向 SAP 提报完工。
        /// 【外部 API 调用】
        /// </summary>
        private async Task ConfirmCompletionToSapAsync(WorkOrderOperationConfirmDto sapConfirm)
        {
            var parameter = (await _parameterGroupService.GetGroupWithItemsAsync(SapTransferParamGroup))
                ?.Items.FirstOrDefault(x => x.Key == SapTransferEnabledKey);

            if (parameter == null || bool.Parse(parameter.Value))
            {
                try
                {
                    var confirmResult = await _sapRfcService.ConfirmOrderCompletionToSAPAsync(sapConfirm.Id);

                    if (confirmResult != null &&
                        !string.IsNullOrWhiteSpace(confirmResult.MessageType) )
                    {
                        await _workOrderOperationConfirmService.UpdateAsync(new WorkOrderOperationConfirmUpdateDto()
                        {
                            Id = sapConfirm.Id,
                            Message = confirmResult.Message,
                            MessageType = confirmResult.MessageType,
                            Status = confirmResult.MessageType.ToUpper() == SapSuccessMessageType ? "1": "-1", // "1"
                            UpdatedAt = DateTime.Now,
                        });

                        if (confirmResult.MessageType.ToUpper() != SapSuccessMessageType)
                        {
                            throw new InvalidOperationException($"SAP 报工失败或返回非成功消息。ConfirmID: {sapConfirm.Id}, Message: {confirmResult?.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await _workOrderOperationConfirmService.UpdateAsync(new WorkOrderOperationConfirmUpdateDto()
                    {
                        Id = sapConfirm.Id,
                        Message = ex.Message,
                        Status = "-1", // "1"""
                        UpdatedAt = DateTime.Now,
                    });
                }
            }
        }



        /// <summary>
        /// 通知 JY API。
        /// 【外部 API 调用】
        /// </summary>
        //private async Task NotifyJyApiAsync(List<string> jpItems, string factoryCode)
        //{
        //    if (!jpItems.Any())
        //    {
        //        return;
        //    }

        //    var requestUrl = _apiSettings["JyApi:Endpoints:PickTaskApprove"];
        //    if (string.IsNullOrEmpty(requestUrl))
        //    {
        //        _logger.LogError("未配置 JY API 的 PickTaskApprove 终结点。");
        //        return;
        //    }

        //    foreach (var item in jpItems)
        //    {
        //        try
        //        {
        //            var response = await _jyApiClient.PostAsync<object, object>(
        //                requestUrl,
        //                new
        //                {
        //                    gid = Guid.NewGuid(),
        //                    billcode = item,
        //                    User = "admin",
        //                    plant = factoryCode
        //                });

        //            _logger.LogInformation($"已成功通知 JY API 拣货任务: {item}。");
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, $"通知 JY API 失败，拣货任务: {item}。");
        //            // 事务已提交，记录错误
        //        }
        //    }
        //}

        #endregion
    }
}
