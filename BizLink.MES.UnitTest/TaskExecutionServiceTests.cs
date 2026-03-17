using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.UnitTest
{
    public class TaskExecutionServiceTests
    {
        // 核心被测服务
        private readonly TaskExecutionService _service;

        // 所有依赖的 Mock 对象
        private readonly Mock<IWorkOrderMaterialTaskRepository> _mockTaskRepo;
        private readonly Mock<IWorkOrderStepTaskRepository> _mockStepRepo;
        private readonly Mock<IWorkOrderOperationTaskRepository> _mockOpTaskRepo;
        private readonly Mock<IStationMaterialLoadingRepository> _mockLoadingRepo;
        private readonly Mock<IRawLinesideStockRepository> _mockInventoryRepo;
        private readonly Mock<IWorkOrderTaskExecuteLogRepository> _mockExeLogRepo;
        private readonly Mock<IWorkOrderTaskExecuteConsumpRepository> _mockExeConsumpRepo;
        private readonly Mock<IWorkStationRepository> _mockStationRepo;
        private readonly Mock<IWorkCenterRepository> _mockCenterRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IServiceScopeFactory> _mockScopeFactory;
        private readonly Mock<ISerialSequenceRepository> _mockSerialRepo;
        
        private readonly Mock<IWorkOrderRepository> _mockWorkOrderRepo;
        private readonly Mock<IWorkOrderBomItemRepository> _mockBomRepo;
        private readonly Mock<IWorkOrderProcessRepository> _mockProcessRepo;
        private readonly Mock<IMaterialViewService> _mockMaterialService;
        private readonly Mock<IFactoryRepository> _mockFactoryRepo;
        private readonly Mock<IWmsWorkOrderPickingLogRepository> _mockPickingLogRepo;

        private readonly Mock<IWorkOrderKittingItemRepository> _mockKittingItemRepo;


        public TaskExecutionServiceTests()
        {
            // 1. 初始化所有 Mocks
            _mockTaskRepo = new Mock<IWorkOrderMaterialTaskRepository>();
            _mockStepRepo = new Mock<IWorkOrderStepTaskRepository>();
            _mockOpTaskRepo = new Mock<IWorkOrderOperationTaskRepository>();
            _mockKittingItemRepo = new Mock<IWorkOrderKittingItemRepository>();
            _mockLoadingRepo = new Mock<IStationMaterialLoadingRepository>();
            _mockInventoryRepo = new Mock<IRawLinesideStockRepository>();
            _mockExeLogRepo = new Mock<IWorkOrderTaskExecuteLogRepository>();
            _mockExeConsumpRepo = new Mock<IWorkOrderTaskExecuteConsumpRepository>();
            _mockStationRepo = new Mock<IWorkStationRepository>();
            _mockCenterRepo = new Mock<IWorkCenterRepository>();
            _mockSerialRepo = new Mock<ISerialSequenceRepository>();
            _mockFactoryRepo = new Mock<IFactoryRepository>();
            _mockWorkOrderRepo = new Mock<IWorkOrderRepository>();
            _mockBomRepo = new Mock<IWorkOrderBomItemRepository>();
            _mockProcessRepo = new Mock<IWorkOrderProcessRepository>();
            _mockMaterialService = new Mock<IMaterialViewService>();
            _mockPickingLogRepo = new Mock<IWmsWorkOrderPickingLogRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockScopeFactory = new Mock<IServiceScopeFactory>();

            // 2. 初始化 Service
            // 注意：这里按照 CodeReview 建议，添加了 ISapIntegrationService
            _service = new TaskExecutionService(
                _mockTaskRepo.Object,
                _mockStepRepo.Object,
                _mockOpTaskRepo.Object,
                _mockLoadingRepo.Object,
                _mockInventoryRepo.Object,
                _mockExeLogRepo.Object,
                _mockExeConsumpRepo.Object,
                _mockStationRepo.Object,
                _mockCenterRepo.Object,
                _mockUnitOfWork.Object,
                _mockScopeFactory.Object,
                _mockSerialRepo.Object,
                _mockWorkOrderRepo.Object,
                _mockBomRepo.Object,
                _mockProcessRepo.Object,
                _mockMaterialService.Object,
                _mockFactoryRepo.Object,
                _mockPickingLogRepo.Object,
                _mockKittingItemRepo.Object
            );
        }

        #region Kitting 业务测试

        [Fact(DisplayName = "GetKittingListAsync: 正常流程应返回正确的齐套预览")]
        public async Task GetKittingListAsync_ValidOrder_ReturnsCorrectSummary()
        {
            // Arrange
            string orderNo = "9891664";
            string rawOrderNo = "9891664";
            int orderId = 61444;
            int factoryId = 2;
            string factoryCode = "CN11";

            // 模拟基础数据
            _mockWorkOrderRepo.Setup(x => x.GetByOrderNoAsync(orderNo))
                .ReturnsAsync(new WorkOrder { Id = orderId, OrderNumber = orderNo, FactoryId = factoryId });

            _mockFactoryRepo.Setup(x => x.GetByIdAsync(factoryId))
                .ReturnsAsync(new Factory { FactoryCode = factoryCode });

            _mockProcessRepo.Setup(x => x.GetListByOrderIdAync(orderId))
                .ReturnsAsync(new List<WorkOrderProcess> { new WorkOrderProcess { Id = 100, Operation = "OP10" } });

            // 模拟 BOM 数据 (包含一个 Cable 和一个 原材料)
            var bomItems = new List<WorkOrderBomItem>
            {
                new WorkOrderBomItem { MaterialCode = "CABLE-01", ConsumeType = (int)ConsumeType.CableMaterial, RequiredQuantity = 100, MovementAllowed = true, WorkOrderProcessId = 100 },
                new WorkOrderBomItem { MaterialCode = "RAW-01", ConsumeType = (int)ConsumeType.OrderBasedMaterial, RequiredQuantity = 50, MovementAllowed = true, WorkOrderProcessId = 100 }
            };
            _mockBomRepo.Setup(x => x.GetListByProcessIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(bomItems);

            // 模拟 MaterialService 返回属性
            _mockMaterialService.Setup(x => x.GetListByCodesAsync(factoryCode, It.IsAny<List<string>>()))
                .ReturnsAsync(new List<MaterialViewDto>
                {
                    new MaterialViewDto { MaterialCode = "CABLE-01", LabelName = "断线物料" },
                    new MaterialViewDto { MaterialCode = "RAW-01", LabelName = "ROH原材料" }
                });

            // 模拟库存/拣配记录 (RAW-01 已拣配 30，缺 20)
            _mockPickingLogRepo.Setup(x => x.GetListByWorkOrderAsync(orderNo))
                .ReturnsAsync(new List<V_WmsWorkOrderPickingLog>
                {
                    new V_WmsWorkOrderPickingLog { MaterialCode = "RAW-01", Quantity = 30 }
                });

            // 模拟断线任务进度 (CABLE-01 任务量 100，已完成 80)
            _mockTaskRepo.Setup(x => x.GetListByBomIdAsync(It.IsAny<List<int>>(), TaskCategories.CableCut, TaskReasonCodes.BomRequirement))
                .ReturnsAsync(new List<WorkOrderMaterialTask>
                {
                    new WorkOrderMaterialTask { MaterialCode = "CABLE-01", TargetQuantity = 100, CompletedQuantity = 80 }
                });

            // Act
            var result = await _service.GetKittingListAsync(rawOrderNo);

            // Assert
            result.Should().NotBeNull();
            result.OrderNo.Should().Be(orderNo);

            // 验证原材料 (RAW-01)
            var rawItem = result.CenterStockItems.FirstOrDefault(x => x.MaterialCode == "RAW-01");
            rawItem.Should().NotBeNull();
            rawItem.Quantity.Should().Be(50); // BOM 需求
            rawItem.CompletedQuantity.Should().Be(30); // 拣配量

            // 验证断线 (CABLE-01)
            var cableItem = result.CableItems.FirstOrDefault(x => x.MaterialCode == "CABLE-01");
            cableItem.Should().NotBeNull();
            cableItem.CompletedQuantity.Should().Be(80); // 任务完成量
        }

        [Fact(DisplayName = "ConfirmKittingAsync: 如果原材料拣配数量不足，应抛出异常")]
        public async Task ConfirmKittingAsync_InsufficientMaterial_ThrowsException()
        {
            // Arrange
            string orderNo = "WO20231001";
            var workOrder = new WorkOrder { Id = 1, OrderNumber = orderNo, FactoryId = 10 };

            _mockWorkOrderRepo.Setup(x => x.GetByOrderNoAsync(orderNo)).ReturnsAsync(workOrder);
            _mockFactoryRepo.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(new Factory { FactoryCode = "CN11" });
            _mockProcessRepo.Setup(x => x.GetListByOrderIdAync(1)).ReturnsAsync(new List<WorkOrderProcess> { new WorkOrderProcess { Id = 100 } });

            // BOM 需求 100 个原材料
            _mockBomRepo.Setup(x => x.GetListByProcessIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync(new List<WorkOrderBomItem>
                {
                    new WorkOrderBomItem { MaterialCode = "RAW-01", ConsumeType = (int)ConsumeType.OrderBasedMaterial, RequiredQuantity = 100, MovementAllowed = true, WorkOrderProcessId = 100 }
                });

            // 实际库存只有 90 (模拟缺料)
            _mockPickingLogRepo.Setup(x => x.GetListByWorkOrderAsync(orderNo))
                .ReturnsAsync(new List<V_WmsWorkOrderPickingLog>
                {
                    new V_WmsWorkOrderPickingLog { MaterialCode = "RAW-01", Quantity = 90 }
                });

            // Act & Assert
            // 期望抛出 InvalidOperationException，且消息包含缺料信息
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.ConfirmKittingAsync(workOrder.Id));
        }

        //[Fact(DisplayName = "ConfirmKittingAsync: 齐套成功后应提交事务并调用 SAP")]
        //public async Task ConfirmKittingAsync_Success_CommitsTransactionAndCallsSap()
        //{
        //    // Arrange
        //    string orderNo = "WO20231001";
        //    var workOrder = new WorkOrder { Id = 1, OrderNumber = orderNo, FactoryId = 10 };

        //    _mockWorkOrderRepo.Setup(x => x.GetByOrderNoAsync(orderNo)).ReturnsAsync(workOrder);
        //    _mockFactoryRepo.Setup(x => x.GetByIdAsync(10)).ReturnsAsync(new Factory { FactoryCode = "CN11" });
        //    var processes = new List<WorkOrderProcess> { new WorkOrderProcess { Id = 100, WorkOrderId = 1, WorkOrderNo = orderNo } };
        //    _mockProcessRepo.Setup(x => x.GetListByOrderIdAync(1)).ReturnsAsync(processes);

        //    // BOM: 需要 50 个 RAW-01
        //    _mockBomRepo.Setup(x => x.GetListByProcessIdsAsync(It.IsAny<List<int>>()))
        //        .ReturnsAsync(new List<WorkOrderBomItem>
        //        {
        //            new WorkOrderBomItem { Id = 10, MaterialCode = "RAW-01", ConsumeType = (int)ConsumeType.OrderBasedMaterial, RequiredQuantity = 50, MovementAllowed = true, WorkOrderProcessId = 100 }
        //        });

        //    // Stock: 有 50 个 RAW-01 (满足)
        //    _mockPickingLogRepo.Setup(x => x.GetListByWorkOrderAsync(orderNo))
        //        .ReturnsAsync(new List<V_WmsWorkOrderPickingLog>
        //        {
        //            new V_WmsWorkOrderPickingLog { MaterialCode = "RAW-01", Quantity = 50, BatchCode = "BATCH001" }
        //        });

        //    // 模拟相关任务
        //    _mockOpTaskRepo.Setup(x => x.GetListByProcessIdAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<WorkOrderOperationTask> { new WorkOrderOperationTask { Id = 500 } });
        //    _mockStepRepo.Setup(x => x.GetListByOperationIdAsync(It.IsAny<List<int>>()))
        //        .ReturnsAsync(new List<WorkOrderStepTask>
        //        {
        //            new WorkOrderStepTask { TaskCategory = TaskCategories.Kitting, Status = WorkTaskStatus.New, Quantity = 1 }
        //        });

        //    // Act
        //    await _service.ConfirmKittingAsync(orderNo);

        //    // Assert
        //    // 1. 验证事务提交
        //    _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(), Times.Once);
        //    _mockUnitOfWork.Verify(x => x.CommitAsync(), Times.Once);

        //    // 2. 验证 KittingItem 插入
        //    _mockKittingItemRepo.Verify(x => x.AddBulkAsync(It.Is<List<WorkOrderKittingItem>>(list =>
        //        list.Count == 1 && list[0].MaterialCode == "RAW-01" && list[0].Quantity == 50
        //    )), Times.Once);

        //    // 3. 验证 Picking/Kitting 任务状态更新
        //    _mockStepRepo.Verify(x => x.UpdateAsync(It.Is<WorkOrderStepTask>(t => t.Status == WorkTaskStatus.Completed)), Times.AtLeastOnce);

        //    // 4. 验证 SAP 调用 (假设 CodeReview 建议已实施)
        //    //_mockSapService.Verify(x => x.TransferForKittingAsync(It.IsAny<SapMaterialTransferDto>()), Times.Once);
        //}

        #endregion

        #region 任务执行测试 (非 Scope 部分)

        [Fact(DisplayName = "ClaimAndStartTasksAsync: 应更新任务状态为进行中")]
        public async Task ClaimAndStartTasksAsync_ValidTasks_UpdatesStatus()
        {
            // Arrange
            int stationId = 5;
            string empId = "EMP001";
            var taskIds = new List<int> { 10, 11 };

            // 模拟任务
            var tasks = new List<WorkOrderMaterialTask>
            {
                new WorkOrderMaterialTask { Id = 10, Status = WorkTaskStatus.Released },
                new WorkOrderMaterialTask { Id = 11, Status = WorkTaskStatus.New }
            };
            _mockTaskRepo.Setup(x => x.GetByIdsAsync(taskIds)).ReturnsAsync(tasks);

            // 模拟工位和工作中心
            _mockStationRepo.Setup(x => x.GetByIdAsync(stationId))
                .ReturnsAsync(new WorkStation { Id = stationId, WorkStationCode = "ST01", WorkCenterId = 99 });
            _mockCenterRepo.Setup(x => x.GetByIdAsync(99))
                .ReturnsAsync(new WorkCenter { Id = 99, WorkCenterCode = "WC01" });

            // Act
            await _service.ClaimAndStartTasksAsync(taskIds, stationId, empId);

            // Assert
            _mockTaskRepo.Verify(x => x.UpdateBulkAsync(It.Is<List<WorkOrderMaterialTask>>(list =>
                list.All(t =>
                    t.Status == WorkTaskStatus.InProgress &&
                    t.ActualWorkStationId == stationId &&
                    t.ActualWorkCenterCode == "WC01"
                )
            )), Times.Once);
        }

        #endregion

        #region 开工校验测试

        [Fact(DisplayName = "ValidateMaterialReadinessAsync: 缺料时应抛出异常")]
        public async Task ValidateMaterialReadinessAsync_MissingMaterial_ThrowsException()
        {
            // Arrange
            int stationId = 1;
            var taskIds = new List<int> { 10 };

            // 任务需要 CABLE-01
            _mockTaskRepo.Setup(x => x.GetByIdsAsync(taskIds))
                .ReturnsAsync(new List<WorkOrderMaterialTask>
                {
                    new WorkOrderMaterialTask { Id = 10, TaskSubType = TaskCategories.CableCut, MaterialCode = "CABLE-01", TargetQuantity = 10, TargetValue = 1 } // 需 10 米
                });

            // 机台当前没有上料 (Active Loadings 为空)
            _mockLoadingRepo.Setup(x => x.GetListByStationIdAsync(stationId,null,"1"))
                .ReturnsAsync(new List<StationMaterialLoading>());

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _service.ValidateMaterialReadinessAsync(taskIds, stationId));
            ex.Message.Should().Contain("缺料");
            ex.Message.Should().Contain("CABLE-01");
        }

        #endregion
    }
}
