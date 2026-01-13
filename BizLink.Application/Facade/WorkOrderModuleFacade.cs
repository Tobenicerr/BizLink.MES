using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Application.Helper;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Enums;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Facade
{
    public class WorkOrderModuleFacade : BaseAppFacade
    {
        // --- 模块特有服务 ---
        public IWorkOrderViewService WorkOrderView
        {
            get;
        }
        public IWorkOrderTaskConsumService TaskConsum
        {
            get;
        }
        public IJyApiClient JyApi
        {
            get;
        }
        public IMaterialViewService MaterialView
        {
            get;
        }
        public IRawLinesideStockService RawStock
        {
            get;
        }
        public IRawMaterialInventoryViewService RawInventoryView
        {
            get;
        }
        public IWorkOrderInProgressViewService WorkOrderInProgressView
        {
            get;
        } // 注意：虽然 BaseAppFacade 有 WorkStation，但 WorkCenter 可能还是独立的
        public IWarehouseLocationService Location
        {
            get;
        }
        public ISerialHelperService Serial
        {
            get;
        }


        public WorkOrderModuleFacade(
            // 模块特有
            IWorkOrderViewService workOrderView,
            IWorkOrderTaskConsumService taskConsum,
            IJyApiClient jyApi,
            IMaterialViewService materialView,
            IRawLinesideStockService rawStock,
            IRawMaterialInventoryViewService rawInventoryView,
            IWarehouseLocationService location,
            ISerialHelperService serial,
            IWorkOrderInProgressViewService workOrderInProgressView,

            // 2. 基础服务 (透传给基类 BaseAppFacade)
            IParameterGroupService paramsService,
            IMesApiClient mesApi,
            IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings,
            IFactoryService factoryService,
            IWorkCenterGroupService workCenterGroupService,
            IWorkCenterService workCenter,
            IWorkStationService workStation,
            IWorkOrderService workOrderService,
            IWorkOrderProcessService workOrderProcessService,
            IWorkOrderBomItemService workOrderBomItemService,
            IUserService userService,
            IMenuService menuService,
            IPermissionService permissionService,
            IRoleService roleService,
            IActivityLogService activityLogService
            ) : base(
                paramsService,
                mesApi,
                apiSettings, // 这里如果不需要 ApiSettings 可以传 null，或者补充注入
                factoryService,
                workCenterGroupService,
                workCenter,
                workStation,
                workOrderService,
                workOrderProcessService,
                workOrderBomItemService,
                userService,
                menuService,
                permissionService,
                roleService,
                activityLogService)
        {
            WorkOrderView = workOrderView;
            TaskConsum = taskConsum;
            JyApi = jyApi;
            MaterialView = materialView;
            RawStock = rawStock;
            RawInventoryView = rawInventoryView;
            WorkOrderInProgressView = workOrderInProgressView;
            Location = location;
            Serial = serial;
        }

        // --- 封装复杂的业务逻辑 (领料申请) ---
        public async Task<string> ExecuteMaterialRequisitionAsync(WorkOrderDto order,UserDto user)
        {
            var workorder = await WorkOrderService.GetByIdAsync(order.Id);
            var boms = await WorkOrderBomItemService.GetListByOrderIdAync(order.Id);
            var validBoms = boms.Where(x => x.RequiredQuantity > 0 && x.MovementAllowed == true && x.ConsumeType == 2 && x.SyncWMSStatus == 0).ToList();

            if (!validBoms.Any())
                return null; // 无需领料
            var factory = await FactoryService.GetByIdAsync(workorder.FactoryId);

            var syncwms = validBoms.Max(x => x.SyncWMSStatus) + 1;
            var materials = await MaterialView.GetListByCodesAsync(factory.FactoryName, validBoms.Select(x => x.MaterialCode).ToList());
            var materialDic = materials.ToDictionary(x => x.MaterialCode);

            foreach (var item in validBoms)
            {
                if (materialDic.TryGetValue(item.MaterialCode, out var matDto))
                    item.MaterialId = matDto.Id;
            }

            var request = new MaterialRequisitionRequest
            {
                VoucherNo = $"{order.OrderNumber}-{syncwms}",
                VoucherTypeCategoryCode = "VTC022",
                VoucherTypeName = "999",
                OWarehouseCode = "1100",
                IWarehouseCode = "2100",
                OStorageCode = "11004",
                IStorageCode = "21001",
                VirtualOPlantCode = "CN11",
                VirtualIPlantCode = "CN11",
                EntryDtos = validBoms.GroupBy(x => x.MaterialId)
                                     .Select(g => new MaterialEntry { MaterialId = (int)g.Key, Qty = (decimal)g.Sum(x => x.RequiredQuantity) })
                                     .ToList(),
                PlantCode = "CN11",
                IsActive = false,
                OperateUser = user.EmployeeId,
                OperateUserName = user.UserName,
                Remark = "按单"
            };

            var url = ApiSettings["JyApi"].Endpoints["TransvouchCreate"];
            var result = await JyApi.PostAsync<MaterialRequisitionRequest, object>(url, request);

            if (!result.IsSuccess)
                throw new Exception($"{order.OrderNumber}按单领料推送失败：" + result.Message);

            // 更新本地状态
            var update = validBoms.Select(x => new WorkOrderBomItemUpdateDto
            {
                Id = x.Id,
                SyncWMSStatus = x.SyncWMSStatus + 1
            }).ToList();

            await WorkOrderBomItemService.UpdateWmsStatusAsync(update);

            return $"{order.OrderNumber} 推送成功";
        }


        #region Export Logic

        // 定义 DTO 用于传输
        public class MaterialRequisitionDemandDto
        {
            public string MaterialCode
            {
                get; set;
            }
            public string MaterialDesc
            {
                get; set;
            }
            public string Operation
            {
                get; set;
            }
            public decimal? ReqQuantity
            {
                get; set;
            }
            public string BaseUnit
            {
                get; set;
            }
            public string ConsumeType
            {
                get; set;
            }
            public string ConsumeMaxWorkCenter
            {
                get; set;
            }
        }

        public class MaterialRequisitionExportDto : MaterialRequisitionDemandDto
        {
            public decimal? LinesideReqQuantity
            {
                get; set;
            }
            public decimal? LinesideStockQuantity
            {
                get; set;
            }
            public string LinesideStockLocation
            {
                get; set;
            }
            public string LinesideStockBarcode
            {
                get; set;
            }
            public decimal? rawReqQuantity
            {
                get; set;
            }
            public decimal? rawStockQuantity
            {
                get; set;
            }
            public string rawStockLocation
            {
                get; set;
            }
            public string rawStockBarcode
            {
                get; set;
            }
        }

        private class AllocationResult
        {
            public decimal OriginalQuantity
            {
                get; set;
            }
            public decimal QuantityTaken
            {
                get; set;
            }
            public string Location
            {
                get; set;
            }
            public string Barcode
            {
                get; set;
            }
        }

        public async Task<List<MaterialRequisitionExportDto>> GenerateMaterialRequisitionExportDataAsync(List<MaterialRequisitionDemandDto> demands,FactoryDto factoryDto)
        {
            // 1. 筛选出需要处理的需求和物料
            var cableDemands = demands.Where(x => x.ConsumeType == ConsumeType.CableMaterial.GetDescription()).ToList();
            var materialCodes = cableDemands.Select(d => d.MaterialCode).Distinct().ToList();

            if (!materialCodes.Any())
            {
                return new List<MaterialRequisitionExportDto>();
            }

            // 2. 批量获取库存数据
            var lineStockTask = await RawStock.GetListByMaterialCodeAsync(factoryDto.Id, materialCodes);
            var rawStockTask = await RawInventoryView.GetListByMaterialCodeAsync(factoryDto.FactoryCode, materialCodes);

           // await Task.WhenAll(lineStockTask, rawStockTask);

            // 3. 数据预处理 (O(1) 查找)
            var lineStockLookup = lineStockTask
                .Where(x => x.LastQuantity > 0)
                .OrderBy(x => x.BarCode)
                .ToLookup(x => x.MaterialCode);

            var rawStockLookup = rawStockTask
                .OrderBy(x => x.BatchCode)
                .ToLookup(x => x.MaterialCode);

            var finalExportList = new List<MaterialRequisitionExportDto>();

            // 4. 内存分配逻辑
            foreach (var item in cableDemands)
            {
                decimal neededQuantity = item.ReqQuantity ?? 0;

                var lineAllocations = new List<AllocationResult>();
                var rawAllocations = new List<AllocationResult>();

                // A. 从断线库分配
                var availableLineStock = lineStockLookup[item.MaterialCode];
                if (availableLineStock.Any())
                {
                    neededQuantity = AllocateLineStock(availableLineStock, neededQuantity, lineAllocations);
                }

                // B. 从原材料库分配
                if (neededQuantity > 0)
                {
                    var availableRawStock = rawStockLookup[item.MaterialCode];
                    if (availableRawStock.Any())
                    {
                        AllocateRawStock(availableRawStock, neededQuantity, rawAllocations);
                    }
                }

                // C. 构建导出结果
                if (!lineAllocations.Any() && !rawAllocations.Any())
                {
                    finalExportList.Add(CreateExportRow(item, null, null));
                }
                else
                {
                    foreach (var la in lineAllocations)
                    {
                        finalExportList.Add(CreateExportRow(item, la, null));
                    }
                    foreach (var ra in rawAllocations)
                    {
                        finalExportList.Add(CreateExportRow(item, null, ra));
                    }
                }
            }

            return finalExportList;
        }

        private decimal AllocateLineStock(IEnumerable<RawLinesideStockDto> availableStock, decimal neededQuantity, List<AllocationResult> allocations)
        {
            decimal? originalStock = null;
            foreach (var stockInfo in availableStock)
            {
                if (originalStock == null)
                    originalStock = stockInfo.LastQuantity;
                if (neededQuantity <= 0)
                    break;

                var quantityToTake = Math.Min(neededQuantity, (decimal)stockInfo.LastQuantity);
                allocations.Add(new AllocationResult
                {
                    OriginalQuantity = originalStock ?? 0,
                    QuantityTaken = quantityToTake,
                    Location = stockInfo.LocationCode,
                    Barcode = stockInfo.BarCode
                });

                neededQuantity -= quantityToTake;
                stockInfo.LastQuantity -= quantityToTake; // 扣减内存中的库存
            }
            return neededQuantity;
        }

        private void AllocateRawStock(IEnumerable<RawMaterialInventoryViewDto> availableStock, decimal neededQuantity, List<AllocationResult> allocations)
        {
            decimal? originalStock = null;
            foreach (var stockInfo in availableStock)
            {
                if (originalStock == null)
                    originalStock = stockInfo.Quantity;
                if (neededQuantity <= 0)
                    break;

                if (stockInfo.Quantity > 0)
                {
                    var quantityToTake = Math.Min(neededQuantity, (decimal)stockInfo.Quantity);
                    allocations.Add(new AllocationResult
                    {
                        OriginalQuantity = originalStock ?? 0,
                        QuantityTaken = quantityToTake,
                        Location = stockInfo.RawLocationName,
                        Barcode = stockInfo.BarCode
                    });

                    neededQuantity -= quantityToTake;
                    stockInfo.Quantity -= quantityToTake; // 扣减内存中的库存
                }
            }
        }

        private MaterialRequisitionExportDto CreateExportRow(MaterialRequisitionDemandDto demand, AllocationResult lineAlloc, AllocationResult rawAlloc)
        {
            return new MaterialRequisitionExportDto
            {
                MaterialCode = demand.MaterialCode,
                MaterialDesc = demand.MaterialDesc,
                Operation = demand.Operation,
                ReqQuantity = demand.ReqQuantity,
                BaseUnit = demand.BaseUnit,
                ConsumeType = demand.ConsumeType,
                ConsumeMaxWorkCenter = demand.ConsumeMaxWorkCenter,

                LinesideReqQuantity = lineAlloc?.QuantityTaken ?? (rawAlloc == null ? demand.ReqQuantity : 0),
                LinesideStockQuantity = lineAlloc?.OriginalQuantity ?? 0,
                LinesideStockLocation = lineAlloc?.Location ?? string.Empty,
                LinesideStockBarcode = lineAlloc?.Barcode ?? string.Empty,

                rawReqQuantity = rawAlloc?.QuantityTaken ?? 0,
                rawStockQuantity = rawAlloc?.OriginalQuantity,
                rawStockLocation = rawAlloc?.Location ?? string.Empty,
                rawStockBarcode = rawAlloc?.Barcode ?? string.Empty,
            };
        }

        #endregion
    }
}
