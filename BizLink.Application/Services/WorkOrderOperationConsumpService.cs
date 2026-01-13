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
    public class WorkOrderOperationConsumpService : IWorkOrderOperationConsumpService
    {
        private readonly IWorkOrderOperationConsumpRepository _workOrderOperationConsumpRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper
        public WorkOrderOperationConsumpService(IWorkOrderOperationConsumpRepository workOrderOperationConsumpRepository, IMapper mapper)
        {
            _workOrderOperationConsumpRepository = workOrderOperationConsumpRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(List<WorkOrderOperationConsumpCreateDto> createDtos)
        {
            return await _workOrderOperationConsumpRepository.AddAsync(createDtos.Select(x => _mapper.Map<WorkOrderOperationConsump>(x)).ToList());
        }

        public Task<WorkOrderOperationConsumpDto> CreateAsync(WorkOrderOperationConsumpCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WorkOrderOperationConsumpCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(List<int> ids)
        {
            return await _workOrderOperationConsumpRepository.DeleteAsync(ids);
        }

        public Task<IEnumerable<WorkOrderOperationConsumpDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderOperationConsumpDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkOrderOperationConsumpDto>> GetListByProcessIdAsync(int processid)
        {
            var entities = await _workOrderOperationConsumpRepository.GetListByProcessIdAsync(processid);
            return _mapper.Map<List<WorkOrderOperationConsumpDto>>(entities);
        }

        public Task<bool> UpdateAsync(WorkOrderOperationConsumpUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(List<WorkOrderOperationConsumpUpdateDto> updateDtos)
        {
            var entities = await _workOrderOperationConsumpRepository.GetByIdsAsync(updateDtos.Select(x => x.Id).ToList());
            var dtoDictionary = updateDtos.ToDictionary(dto => dto.Id);

            foreach (var entity in entities)
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
            return await _workOrderOperationConsumpRepository.UpdateBulkAsync(entities);
        }
    }
}
