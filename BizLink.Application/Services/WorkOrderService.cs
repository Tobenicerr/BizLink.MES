using AutoMapper;
using Azure;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using BizLink.MES.WinForms.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderService : IWorkOrderService
    {
        private readonly IWorkOrderRepository _workOrderRepository;
        private readonly IWorkOrderProcessRepository _workOrderProcessRepository;
        private readonly IWorkOrderBomItemRepository _workOrderBomItemRepository;
        private readonly IMaterialViewRepository _materialViewRepository;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IMapper _mapper; // 2. 声明 IMapper
        private readonly IUnitOfWork _unitOfWork;


        public WorkOrderService(IWorkOrderRepository workOrderRepository, IMapper mapper, IWorkOrderProcessRepository workOrderProcessRepository, IWorkOrderBomItemRepository workOrderBomItemRepository, IUnitOfWork unitOfWork, IMaterialViewRepository materialViewRepository, IServiceScopeFactory serviceScopeFactory)
        {
            _workOrderRepository = workOrderRepository;
            _workOrderProcessRepository = workOrderProcessRepository;
            _workOrderBomItemRepository = workOrderBomItemRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _materialViewRepository = materialViewRepository;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<int> CountByOrderNosNoLockAsync(List<string> ordernos)
        {
            return await _workOrderRepository.CountByOrderNosNoLockAsync(ordernos);
        }

        public async Task<List<int>> CreateBatchAsync(List<WorkOrderCreateDto> createDtos)
        {
            return await _workOrderRepository.AddBatchAsync(_mapper.Map<List<WorkOrder>>(createDtos));
        }

        public async Task CreateBySAPAsync(string factorycode, List<WorkOrderCreateDto> workorders, List<WorkOrderProcessCreateDto> processes, List<WorkOrderBomItemCreateDto> boms, IProgress<float> progress)
        {

            // --- 优化 1: 预处理列表 (保留) ---
            // 将工序列表转换为字典，以便通过 WorkOrderNo 进行 O(1) 快速查找
            var processGroups = processes.GroupBy(p => p.WorkOrderNo)
                                         .ToDictionary(g => g.Key, g => g.ToList());

            // 将BOM列表转换为字典，以便通过 WorkOrderNo 进行 O(1) 快速查找
            var bomGroups = boms.GroupBy(b => b.WorkOrderNo)
                                .ToDictionary(g => g.Key, g => g.ToList());

            int i = 0;
            // --- 优化 3: 收集错误 ---
            var errorMessages = new List<Exception>();

            foreach (var workOrderCreateDto in workorders)
            {

                // 【关键修复】：创建新的 Scope，彻底隔离每一次循环的数据库上下文
                // 这样每次循环拿到的 _unitOfWork 都是全新的，互不干扰
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // 从新 Scope 中获取所有需要的服务
                    // 注意：这里变量名用了 scoped 前缀，以区别于类的成员变量
                    var scopedUow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var scopedMaterialRepo = scope.ServiceProvider.GetRequiredService<IMaterialViewRepository>();
                    var scopedOrderRepo = scope.ServiceProvider.GetRequiredService<IWorkOrderRepository>();
                    var scopedProcessRepo = scope.ServiceProvider.GetRequiredService<IWorkOrderProcessRepository>();
                    var scopedBomRepo = scope.ServiceProvider.GetRequiredService<IWorkOrderBomItemRepository>();
                    // Mapper 可以沿用全局的，因为它是无状态的

                    try
                    {
                        // 开启新 Scope 的事务
                        await scopedUow.BeginTransactionAsync();

                        // --- 业务逻辑 (使用 scoped 变量) ---

                        // 1. 获取物料
                        var material = await scopedMaterialRepo.GetByCodeAsync(factorycode, workOrderCreateDto.MaterialCode);
                        if (material != null)
                        {
                            workOrderCreateDto.MaterialDesc = material.MaterialName;
                        }

                        // 2. 处理工单
                        var order = _mapper.Map<WorkOrder>(workOrderCreateDto);
                        var orderExist = await scopedOrderRepo.GetByOrderNoAsync(order.OrderNumber);

                        if (orderExist != null)
                        {
                            await scopedOrderRepo.DeleteAsync(orderExist.Id);
                            await scopedProcessRepo.DeleteByOrderIdAsync(orderExist.Id);
                            await scopedBomRepo.DeleteByOrderIdAsync(orderExist.Id);
                        }

                        var orderEntity = await scopedOrderRepo.AddAsync(order);

                        // 3. 处理工序
                        if (processGroups.TryGetValue(orderEntity.OrderNumber, out var orderProcesses))
                        {
                            orderProcesses.ForEach(p => p.WorkOrderId = orderEntity.Id);
                            var processEntities = _mapper.Map<List<WorkOrderProcess>>(orderProcesses);

                            if (!await scopedProcessRepo.CreateBatch(processEntities))
                            {
                                throw new Exception($"为工单 {orderEntity.OrderNumber} 插入工序失败");
                            }

                            // 3c. 获取新创建的工序
                            var newlyCreatedProcesses = await scopedProcessRepo.GetListByOrderIdAync(orderEntity.Id);

                            // 4. 处理BOM
                            if (bomGroups.TryGetValue(orderEntity.OrderNumber, out var orderBoms))
                            {
                                var bomsToCreate = orderBoms.Join(
                                    newlyCreatedProcesses,
                                    bom => bom.Operation,
                                    proc => proc.Operation,
                                    (bom, proc) => _mapper.Map<WorkOrderBomItem>(new WorkOrderBomItemCreateDto()
                                    {
                                        WorkOrderId = (int)proc.WorkOrderId,
                                        WorkOrderProcessId = proc.Id,
                                        WorkOrderNo = bom.WorkOrderNo,
                                        RequiredQuantity = bom.RequiredQuantity,
                                        BomItem = bom.BomItem,
                                        Operation = bom.Operation,
                                        ComponentScrap = bom.ComponentScrap,
                                        MaterialCode = bom.MaterialCode,
                                        MaterialDesc = bom.MaterialDesc,
                                        Unit = bom.Unit,
                                        ReservationItem = bom.ReservationItem,
                                        MovementAllowed = bom.MovementAllowed,
                                        QuantityIsFixed = bom.QuantityIsFixed,
                                        ConsumeType = bom.ConsumeType,
                                        SyncWMSStatus = bom.SyncWMSStatus,
                                        SuperMaterialCode = bom.SuperMaterialCode,
                                        CreateBy = bom.CreateBy
                                    })).ToList();

                                if (bomsToCreate.Any())
                                {
                                    if (!await scopedBomRepo.CreateBatch(bomsToCreate))
                                    {
                                        throw new Exception($"为工单 {orderEntity.OrderNumber} 插入BOM失败");
                                    }
                                }
                            }
                        }

                        // 提交当前 Scope 的事务
                        await scopedUow.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        // 回滚当前 Scope 的事务
                        await scopedUow.RollbackAsync();
                        errorMessages.Add(new Exception($"导入工单 {workOrderCreateDto.OrderNumber} 失败: {ex.Message}", ex));
                    }
                    finally
                    {
                        progress?.Report((float)++i / workorders.Count);
                    }
                } // using 结束，Scope 自动释放，SqlSugarClient 及其连接彻底销毁
            }

            // --- 优化 3: 在最后统一抛出所有收集到的错误 ---
            if (errorMessages.Any())
            {
                // 抛出一个聚合异常，告知上层有哪些工单失败了
                throw new AggregateException($"SAP 工单导入已完成，但有 {errorMessages.Count} 个工单失败。", errorMessages);
            }
            //try
            //{
            //    int i = 0;
            //    foreach (var workOrderCreateDto in workorders)
            //    {
            //        try
            //        {
            //            await _unitOfWork.BeginTransactionAsync();
            //            var material = await _materialViewRepository.GetByCodeAsync(factorycode, workOrderCreateDto.MaterialCode);
            //            if (material != null)
            //            {
            //                workOrderCreateDto.MaterialDesc = material.MaterialName;
            //            }
            //            var order = _mapper.Map<WorkOrder>(workOrderCreateDto);
            //            var orderExist = await _workOrderRepository.GetByOrderNoAsync(order.OrderNumber);
            //            if (orderExist != null)
            //            {
            //                await _workOrderRepository.DeleteAsync(orderExist.Id);
            //                await _workOrderProcessRepository.DeleteByOrderIdAsync(orderExist.Id);
            //                await _workOrderBomItemRepository.DeleteByOrderIdAsync(orderExist.Id);
            //            }
            //            var orderEntity = await _workOrderRepository.AddAsync(order);
            //            processes.Where(x => x.WorkOrderNo.Equals(orderEntity.OrderNumber)).ToList().ForEach(x => x.WorkOrderId = orderEntity.Id);
            //            var result = await _workOrderProcessRepository.CreateBatch(processes.Where(x => x.WorkOrderId.Equals(orderEntity.Id)).Select(x => _mapper.Map<WorkOrderProcess>(x)).ToList());
            //            if (!result)
            //                throw new Exception("Process Insert Fail");
            //            var processEntity = await _workOrderProcessRepository.GetListByOrderIdAync(orderEntity.Id);

            //            result = await _workOrderBomItemRepository.CreateBatch(boms.Join(processEntity, x => new { x.WorkOrderNo, x.Operation }, y => new { y.WorkOrderNo, y.Operation }, (x, y) => _mapper.Map<WorkOrderBomItem>(new WorkOrderBomItemCreateDto()
            //            {
            //                WorkOrderId = (int)y.WorkOrderId,
            //                WorkOrderProcessId = y.Id,
            //                WorkOrderNo = x.WorkOrderNo,
            //                RequiredQuantity = x.RequiredQuantity,
            //                BomItem = x.BomItem,
            //                Operation = x.Operation,
            //                ComponentScrap = x.ComponentScrap,
            //                MaterialCode = x.MaterialCode,
            //                MaterialDesc = x.MaterialDesc,
            //                Unit = x.Unit,
            //                ReservationItem = x.ReservationItem,
            //                MovementAllowed = x.MovementAllowed,
            //                QuantityIsFixed = x.QuantityIsFixed,
            //                ConsumeType = x.ConsumeType,
            //                SyncWMSStatus = x.SyncWMSStatus,
            //                SuperMaterialCode = x.SuperMaterialCode,
            //                CreateBy = x.CreateBy
            //            })).ToList());

            //            if (!result)
            //                throw new Exception("Bom Insert Fail");

            //            await _unitOfWork.CommitAsync();

            //        }
            //        catch (Exception)
            //        {
            //            await _unitOfWork.RollbackAsync();
            //        }
            //        finally 
            //        {
            //            progress?.Report((float)++i/ workorders.Count);
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    throw; // 重新抛出异常，让上层处理
            //}
        }

        public async Task<WorkOrderDto> GetByIdAsync(int orderid)
        {
            var entity = await _workOrderRepository.GetByIdAsync(orderid);
            return _mapper.Map<WorkOrderDto>(entity);
        }

        public async Task<WorkOrderDto> GetByOrdrNoAsync(string orderno)
        {
            var entity = await _workOrderRepository.GetByOrderNoAsync(orderno);
            return _mapper.Map<WorkOrderDto>(entity);
        }

        public async Task<List<WorkOrderDto>> GetByOrdrNoAsync(List<string> ordernos)
        {
            var result = await _workOrderRepository.GetByOrderNoAsync(ordernos);
            return _mapper.Map<List<WorkOrderDto>>(result);
        }

        public async Task<(List<WorkOrderDto>, List<WorkOrderProcessDto>, List<WorkOrderTaskDto>)> GetCableTaskConfirmListAsync(List<string>? orders, DateTime? starttime, int? workcenterid, int? workstationid)
        {
            var (workorder,process,task) = await _workOrderRepository.GetCableTaskConfirmListAsync(orders, starttime, workcenterid, workstationid);
            return (workorder.Select(x => _mapper.Map<WorkOrderDto>(x)).ToList(), process.Select(x => _mapper.Map<WorkOrderProcessDto>(x)).ToList(), task.Select(x => _mapper.Map<WorkOrderTaskDto>(x)).ToList());
        }

        public async Task<List<WorkOrderDto>> GetListByBomMaterialAsync(string bommaterialcode)
        {
            var entities = await _workOrderRepository.GetListByBomMaterialAsync(bommaterialcode);
            return entities.Select(x => _mapper.Map<WorkOrderDto>(x)).ToList();
        }

        public async Task<List<WorkOrderDto>> GetListByDispatchDateAsync(DateTime? dispatchdate,DateTime? startdate,List<string>? ordernos, int factoryid)
        {
            var entities = await _workOrderRepository.GetListByDispatchDateAsync(dispatchdate, startdate, ordernos, factoryid);
            return entities.Select(x => _mapper.Map<WorkOrderDto>(x)).ToList();
        }

        public async Task<List<WorkOrderDto>> GetListByDispatchDateEndAsync(int factoryid, DateTime startdate)
        {
            var entities = await _workOrderRepository.GetListByDispatchDateEndAsync(factoryid, startdate);
            return _mapper.Map<List<WorkOrderDto>>(entities);
        }

        public async Task<bool> UpdateAsync(WorkOrderUpdateDto updateDto)
        {
            var entity = await _workOrderRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateBatchAsync(List<WorkOrderUpdateDto> updateDtos)
        {
            if (updateDtos == null || !updateDtos.Any())
            {
                return true;
            }

            var dtoDictionary = updateDtos.ToDictionary(dto => dto.Id);

            var entityList = await _workOrderRepository.GetByIdAsync(dtoDictionary.Keys.ToList());
            foreach (var entity in entityList)
            {
                // 5. 尝试从字典中获取匹配的 DTO
                if (dtoDictionary.TryGetValue(entity.Id, out var matchingDto))
                {
                    // 找到了，应用映射
                    _mapper.Map(matchingDto, entity);
                }

            }

            return await _workOrderRepository.UpdateBatchAsync(entityList);
        }
    }
}
