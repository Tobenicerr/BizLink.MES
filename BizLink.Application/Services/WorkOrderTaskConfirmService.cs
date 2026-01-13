using AutoMapper;
using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using BizLink.MES.WinForms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderTaskConfirmService : IWorkOrderTaskConfirmService
    {

        private readonly IWorkOrderTaskConfirmRepository _workOrderTaskConfirmRepository;

        private readonly IWorkOrderProcessRepository _workOrderProcessViewRepository;
        private readonly IWorkOrderBomItemRepository _workOrderBomItemRepository;
        private readonly IWorkOrderTaskRepository _workOrderTaskRepository;

        private readonly IWorkOrderTaskConsumRepository _workOrderTaskConsumRepository;

        private readonly IWorkOrderTaskMaterialAddRepository _workOrderTaskMaterialAddRepository;
        private  readonly IRawLinesideStockRepository _rollingStockRepository;
        private readonly IProductLinesideStockRepository _productLinesideStockRepository;
        private readonly IWorkOrderOperationConsumptionRecordRepository _workOrderOperationConsumptionRecordRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkOrderTaskConfirmService(IWorkOrderTaskConfirmRepository workOrderTaskConfirmRepository, IWorkOrderProcessRepository workOrderProcessViewRepository, IWorkOrderTaskConsumRepository workOrderTaskConsumRepository, IWorkOrderTaskMaterialAddRepository workOrderTaskMaterialAddRepository, IProductLinesideStockRepository productLinesideStockRepository, IMapper mapper, IWorkOrderTaskRepository workOrderTaskRepository, IRawLinesideStockRepository rollingStockRepository, IUnitOfWork unitOfWork, IWorkOrderOperationConsumptionRecordRepository workOrderOperationConsumptionRecordRepository, IWorkOrderBomItemRepository workOrderBomItemRepository)
        {
            _workOrderTaskConfirmRepository = workOrderTaskConfirmRepository;
            _workOrderProcessViewRepository = workOrderProcessViewRepository;
            _workOrderTaskConsumRepository = workOrderTaskConsumRepository;
            _workOrderTaskMaterialAddRepository = workOrderTaskMaterialAddRepository;
            _productLinesideStockRepository = productLinesideStockRepository;
            _mapper = mapper;
            _workOrderTaskRepository = workOrderTaskRepository;
            _unitOfWork = unitOfWork;
            _rollingStockRepository = rollingStockRepository;
            _workOrderOperationConsumptionRecordRepository = workOrderOperationConsumptionRecordRepository;
            _workOrderBomItemRepository = workOrderBomItemRepository;
        }
        public async Task<WorkOrderTaskConfirmDto> CreateAsync(WorkOrderTaskConfirmCreateDto input)
        {
            var entity = _mapper.Map<WorkOrderTaskConfirm>(input);
            var result = await _workOrderTaskConfirmRepository.AddAsync(entity);
            return _mapper.Map<WorkOrderTaskConfirmDto>(result);
        }

        public Task<List<int>> CreateBatchAsync(List<WorkOrderTaskConfirmCreateDto> createDto)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 批量处理装配报工 (优化版)
        /// </summary>
        public async Task<List<int>> CreateByAssmAsync(List<WorkOrderTaskConfirmCreateDto> confirms, List<WorkOrderTaskConsumCreateDto> consums, List<WorkOrderTaskUpdateDto> tasks)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // --- 1. 批量更新任务状态 (优化：减少数据库交互) ---
                if (tasks != null && tasks.Any())
                {

                    // 1.1 提取所有 ID
                    var taskIds = tasks.Select(t => t.Id).ToList();

                    // 1.2 一次性查出所有实体 (假设仓储有 GetListAsync(Expression) 方法)
                    // 如果没有，请在仓储中添加: Task<List<T>> GetListAsync(Expression<Func<T, bool>> predicate);
                    var taskEntities = await _workOrderTaskRepository.GetListByIdsAsync(taskIds);

                    // 1.3 在内存中进行映射更新
                    foreach (var taskUpdateDto in tasks)
                    {
                        var entity = taskEntities.FirstOrDefault(x => x.Id == taskUpdateDto.Id);
                        if (entity != null)
                        {
                            _mapper.Map(taskUpdateDto, entity); // 将 DTO 的变更(如 Status, CompletedQty) 应用到实体
                        }
                    }

                    // 1.4 批量更新回数据库
                    // SqlSugar 仓储通常支持 UpdateRangeAsync(List<T>)
                    if (taskEntities.Any())
                    {
                        await _workOrderTaskRepository.BatchUpdateAsync(taskEntities);
                    }
                }

                // --- 2. 批量插入报工确认 (优化：使用 BatchAdd) ---
                var confirmIds = new List<int>();
                if (confirms != null && confirms.Any())
                {
                    var confirmEntities = _mapper.Map<List<WorkOrderTaskConfirm>>(confirms);
                    // 使用 BatchAddAsync 替代循环 AddAsync
                    confirmIds = await _workOrderTaskConfirmRepository.BatchAddAsync(confirmEntities);
                }

                // --- 3. 批量插入物料消耗 (补全逻辑) ---
                // 注意：原代码中遗漏了 Consums 的处理，这里补上
                //if (consums != null && consums.Any())
                //{
                //    // 假设消耗记录不需要关联 ConfirmId (因为装配可能是批量报工，关系较弱)，直接插入
                //    // 如果需要关联 ConfirmId，则不能使用简单的 BatchAdd，需要维护关系
                //    var consumEntities = _mapper.Map<List<WorkOrderTaskConsum>>(consums);
                //    await _workOrderTaskConsumRepository.BatchAddAsync(consumEntities);
                //}

                await _unitOfWork.CommitAsync();
                return confirmIds;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task CreateByCableAsync(WorkOrderTaskConfirmCreateDto confirm, List<WorkOrderTaskConsumCreateDto> Consums, WorkOrderTaskUpdateDto task,int factoryId)
        {


            // 1. 在工作单元上开启事务
            await _unitOfWork.BeginTransactionAsync();
            // 注意：事务的边界是在服务层定义的，而不是在仓储层
            try
            {
                var taskEntity = await _workOrderTaskRepository.GetByIdAsync(task.Id);
                _mapper.Map(task, taskEntity);
                var consumsEntity = Consums.Select(x => _mapper.Map<WorkOrderTaskConsum>(x)).ToList();
                var confirmEntity = _mapper.Map<WorkOrderTaskConfirm>(confirm);
                var process = await _workOrderProcessViewRepository.GetByIdAsync((int)taskEntity.OrderProcessId);
                // 2. 调用第一个仓储来创建用户
                await _workOrderTaskRepository.UpdateAsync(taskEntity);
                var confirmEn = await _workOrderTaskConfirmRepository.AddAsync(confirmEntity);
                consumsEntity.ForEach(c => c.ConfirmId = confirmEn.Id);
                if (confirmEn != null)
                {
                    await _workOrderTaskConsumRepository.BatchAddAsync(consumsEntity);
                    var bom = (await _workOrderBomItemRepository.GetListByOrderIdAync((int)taskEntity.OrderId)).Where(x => x.BomItem == taskEntity.MaterialItem && x.MaterialCode == taskEntity.MaterialCode).FirstOrDefault();
                    if (bom == null)
                    {
                        bom = (await _workOrderBomItemRepository.GetListByOrderIdAync((int)taskEntity.OrderId)).Where(x => x.MaterialCode == taskEntity.MaterialCode).FirstOrDefault();
                    }

                    foreach (var consum in consumsEntity)
                    {
                        //记录工序维度物料消耗记录
                        var consumerecord = new WorkOrderOperationConsumptionRecord
                        {
                            WorkOrderId = (int)taskEntity.OrderId,
                            WorkOrderProcessId = (int)taskEntity.OrderProcessId,
                            ReservationItem = bom != null ? bom.ReservationItem : null,
                            MaterialCode = taskEntity.MaterialCode,
                            BatchCode = consum.BatchCode,
                            BarCode = consum.BarCode,
                            Quantity = consum.EntryQuantity,
                            BaseUnit = consum.EntryUnitCode,
                            ConsumptionType = Enum.TryParse<ConsumptionType>(consum.MovementType, out ConsumptionType status) ? status: ConsumptionType.Consumption,
                            ConsumptionRemark = consum.MovementRemark,
                            CreatedBy = confirm.EmployerCode,
                        };
                        await _workOrderOperationConsumptionRecordRepository.AddAsync(consumerecord);
                    }


                    var consumsSum = consumsEntity.GroupBy(x => x.BarCode).Select(group => new
                    {
                        BarCode = group.Key,
                        Quantity = group.Sum(x => x.EntryQuantity)
                    });
                    foreach (var item in consumsSum)
                    {
                        var temp = (await _workOrderTaskMaterialAddRepository.GetByTaskIdAsync(taskEntity.Id)).Where(x => x.BarCode == item.BarCode && x.Status == "1").First();
                        temp.LastQuantity -= item.Quantity;

                        //任务完成自动下料
                        if (confirmEn.Status == "1")
                        {
                            temp.Status = "0";
                        }
                        await _workOrderTaskMaterialAddRepository.UpdateAsync(temp);

                        var rawstock = await _rollingStockRepository.GetByBarCodeAsync(factoryId, item.BarCode);
                        rawstock.LastQuantity -= item.Quantity;

                        await _rollingStockRepository.UpdateAsync(rawstock);
                    }

                    var productstockEntity = new ProductLinesideStock
                    {
                        WorkOrderId = taskEntity.OrderId,
                        WorkOrderProcessId = taskEntity.OrderProcessId,
                        WorkOrderNo = taskEntity.OrderNumber,
                        BomItem = taskEntity.MaterialItem,
                        Operation = taskEntity.Operation,
                        WorkCenterCode = process.WorkCenter,
                        MaterialCode = taskEntity.MaterialCode,
                        MaterialDesc = taskEntity.MaterialDesc,
                        BatchCode = consumsEntity.FirstOrDefault().BatchCode,
                        BarCode = confirmEntity.ConfirmNumber,
                        Quantity = confirmEntity.ConfirmQuantity,
                        CreatedBy = confirm.EmployerCode,
                    };

                    await _productLinesideStockRepository.AddAsync(productstockEntity);

                    //if (confirmEn.Status == "1")
                    //{
                    //    await _workOrderProcessViewRepository.UpdateJyTaskAsync(taskEntity.OrderNumber, taskEntity.MaterialItem, consumsEntity.FirstOrDefault().BarCode, confirmEntity.ConfirmNumber);
                    //}

                }
                // 4. 如果所有操作都成功，提交事务
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                // 5. 如果有任何异常，回滚事务
                await _unitOfWork.RollbackAsync();
                throw; // 重新抛出异常，让上层处理
            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderTaskConfirmDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<WorkOrderTaskConfirmDto> GetByStationIdAsync(int prossid, int stationid, List<int> confirmids)
        {
            var result = await _workOrderTaskConfirmRepository.GetByStationIdAsync(prossid, stationid, confirmids);
            return _mapper.Map<WorkOrderTaskConfirmDto>(result);
        }

        public async Task<List<WorkOrderTaskConfirmDto>> GetByEndStationAsync(List<int> prossid, int stationid)
        {
            var result = await _workOrderTaskConfirmRepository.GetByEndStationAsync(prossid, stationid);
            return _mapper.Map<List<WorkOrderTaskConfirmDto>>(result);
        }

        public async Task<WorkOrderTaskConfirmDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderTaskConfirmRepository.GetByIdAsync(id);
            return _mapper.Map<WorkOrderTaskConfirmDto>(entity);
        }

        public async Task<List<WorkOrderTaskConfirmDto>> GetListByOrderNoAsync(string orderno)
        {
            var entities = await _workOrderTaskConfirmRepository.GetListByOrderNoAsync(orderno);
            return entities.Select(x => _mapper.Map<WorkOrderTaskConfirmDto>(x)).ToList();

        }

        public async Task<List<WorkOrderTaskConfirmDto>> GetListByTaskIdAsync(int taskid)
        {
            var entities = await _workOrderTaskConfirmRepository.GetListByTaskIdAsync(taskid);
            return entities.Select(x => _mapper.Map<WorkOrderTaskConfirmDto>(x)).ToList();
        }

        public async Task<bool> UpdateAsync(WorkOrderTaskConfirmUpdateDto updateDto)
        {
            var entity = await _workOrderTaskConfirmRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderTaskConfirmRepository.UpdateAsync(entity);

        }

    }
}
