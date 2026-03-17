using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderOperationTaskService : IWorkOrderOperationTaskService
    {
        private readonly IWorkOrderOperationTaskRepository _workOrderOperationTaskRepository;
        private readonly IMapper _mapper;
        public WorkOrderOperationTaskService(
            IWorkOrderOperationTaskRepository workOrderOperationTaskRepository,
            IMapper mapper)
        {
            _workOrderOperationTaskRepository = workOrderOperationTaskRepository;
            _mapper = mapper;
        }

        public async Task<WorkOrderOperationTaskDto> CreateAsync(WorkOrderOperationTaskCreateDto createDto)
        {
            var entity = await _workOrderOperationTaskRepository.AddAsync(_mapper.Map<WorkOrderOperationTask>(createDto));
            return _mapper.Map<WorkOrderOperationTaskDto>(entity);
        }

        public async Task<List<int>> CreateBatchAsync(List<WorkOrderOperationTaskCreateDto> createDto)
        {
            return await _workOrderOperationTaskRepository.AddBulkAsync(_mapper.Map<List<WorkOrderOperationTask>>(createDto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workOrderOperationTaskRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkOrderOperationTaskDto>> GetAllAsync()
        {
            var entities = await _workOrderOperationTaskRepository.GetAllAsync();
            return _mapper.Map<List<WorkOrderOperationTaskDto>>(entities);
        }

        public async Task<WorkOrderOperationTaskDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderOperationTaskRepository.GetByIdAsync(id);
            return _mapper.Map<WorkOrderOperationTaskDto>(entity);
        }

        public async Task<List<WorkOrderOperationTaskDto>> GetByIdAsync(List<int> ids)
        {
            var entities = await _workOrderOperationTaskRepository.GetByIdAsync(ids);
            return _mapper.Map<List<WorkOrderOperationTaskDto>>(entities);
        }

        public async Task<List<WorkOrderOperationTaskDto>> GetListByProcessIdAsync(List<int> processIds)
        {
            var entities = await _workOrderOperationTaskRepository.GetListByProcessIdAsync(processIds);
            return _mapper.Map<List<WorkOrderOperationTaskDto>>(entities);
        }

        public async Task<bool> UpdateAsync(WorkOrderOperationTaskUpdateDto updateDto)
        {
            var entity = await _workOrderOperationTaskRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderOperationTaskRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateBatchAsync(List<WorkOrderOperationTaskUpdateDto> updateDto)
        {
            if (updateDto == null || !updateDto.Any())
            {
                return true;
            }

            // 2. 将 DTO 列表转换为字典，以便 O(1) 快速查找
            //    键 = Id, 值 = D TO 对象
            //    注意：这假设 updateDtoList 中的 Id 是唯一的。
            //    如果 Id 不唯一，使用 .ToLookup() 或 .GroupBy().ToDictionary()
            var dtoDictionary = updateDto.ToDictionary(dto => dto.Id);

            // 3. 从仓储中获取所有需要更新的实体
            //    我们使用字典的 Key (即所有 ID) 来获取
            var entityList = await _workOrderOperationTaskRepository.GetByIdsAsync(dtoDictionary.Keys.ToList());

            // 4. 使用 O(n) 循环 + O(1) 字典查找来应用更改
            foreach (var entity in entityList)
            {
                // 5. 尝试从字典中获取匹配的 DTO
                if (dtoDictionary.TryGetValue(entity.Id, out var matchingDto))
                {
                    // 找到了，应用映射
                    _mapper.Map(matchingDto, entity);
                }
                // （如果没找到，您可能需要记录一个警告，但这不应该发生，
                //   因为我们是根据 DTO 的 ID 去获取实体的）
            }

            // 6. 一次性将所有更改提交到仓储
            return await _workOrderOperationTaskRepository.UpdateBulkAsync(entityList);
        }
        // Implement service methods here
    }
}
