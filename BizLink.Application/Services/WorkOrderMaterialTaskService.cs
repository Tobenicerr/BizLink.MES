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
    public class WorkOrderMaterialTaskService : IWorkOrderMaterialTaskService
    {
        private readonly IWorkOrderMaterialTaskRepository _workOrderMaterialTaskRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkOrderMaterialTaskService(IWorkOrderMaterialTaskRepository workOrderMaterialTaskRepository, IMapper mapper)
        {
            _workOrderMaterialTaskRepository = workOrderMaterialTaskRepository;
            _mapper = mapper;
        }

        public async Task<WorkOrderMaterialTaskDto> CreateAsync(WorkOrderMaterialTaskCreateDto createDto)
        {
            var entity = await _workOrderMaterialTaskRepository.AddAsync(_mapper.Map<WorkOrderMaterialTask>(createDto)); //使用 IMapper 进行映射
            return _mapper.Map<WorkOrderMaterialTaskDto>(entity);
        }

        public async Task<List<int>> CreateBatchAsync(List<WorkOrderMaterialTaskCreateDto> createDto)
        {
            return await _workOrderMaterialTaskRepository.AddBulkAsync(_mapper.Map<List<WorkOrderMaterialTask>>(createDto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workOrderMaterialTaskRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkOrderMaterialTaskDto>> GetAllAsync()
        {
            var entities = await _workOrderMaterialTaskRepository.GetAllAsync();
            return _mapper.Map<List<WorkOrderMaterialTaskDto>>(entities);
        }

        public async Task<WorkOrderMaterialTaskDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderMaterialTaskRepository.GetByIdAsync(id);
            return _mapper.Map<WorkOrderMaterialTaskDto>(entity);
        }

        public async Task<List<WorkOrderMaterialTaskDto>> GetByIdAsync(List<int> ids)
        {
            var entities = await _workOrderMaterialTaskRepository.GetByIdsAsync(ids);
            return _mapper.Map<List<WorkOrderMaterialTaskDto>>(entities);
        }

        public async Task<WorkOrderMaterialTaskDto> GetCuttingViewByBomIdAsync(int bomId)
        {
            var entity = await _workOrderMaterialTaskRepository.GetCuttingViewByBomIdAsync(bomId);
            return _mapper.Map<WorkOrderMaterialTaskDto>(entity);

        }

        public async Task<List<WorkOrderMaterialTaskDto>> GetCuttingViewByProcessIdAsync(List<int> processIds)
        {
            var entities = await _workOrderMaterialTaskRepository.GetCuttingViewByProcessIdAsync(processIds);
            return _mapper.Map<List<WorkOrderMaterialTaskDto>>(entities);
        }

        public async Task<List<WorkOrderMaterialTaskDto>> GetListByStepTaskIdAsync(int stepId, string? TaskSubType = null)
        {
            var entities = await _workOrderMaterialTaskRepository.GetListByStepTaskIdAsync(stepId, TaskSubType);
            return _mapper.Map<List<WorkOrderMaterialTaskDto>>(entities);
        }

        public async Task<List<WorkOrderMaterialTaskDto>> GetListByStepTaskIdAsync(List<int> stepIds, string? TaskSubType = null)
        {
            var entities = await _workOrderMaterialTaskRepository.GetListByStepTaskIdAsync(stepIds, TaskSubType);
            return _mapper.Map<List<WorkOrderMaterialTaskDto>>(entities);
        }

        public async Task<bool> UpdateAsync(WorkOrderMaterialTaskUpdateDto updateDto)
        {
            var entity = await _workOrderMaterialTaskRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderMaterialTaskRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateBatchAsync(List<WorkOrderMaterialTaskUpdateDto> updateDto)
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
            var entityList = await _workOrderMaterialTaskRepository.GetByIdsAsync(dtoDictionary.Keys.ToList());

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
            return await _workOrderMaterialTaskRepository.UpdateBulkAsync(entityList);
        }
    }
}
