using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
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

        public async Task<WorkOrderOperationTaskDto> CreateAsync(WorkCenterGroupStepConfigCreateDto createDto)
        {
            var entity = await _workOrderOperationTaskRepository.AddAsync(_mapper.Map<WorkOrderOperationTask>(createDto));
            return _mapper.Map<WorkOrderOperationTaskDto>(entity);
        }

        public async Task<List<int>> CreateBatchAsync(List<WorkCenterGroupStepConfigCreateDto> createDto)
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

        public async Task<bool> UpdateAsync(WorkOrderOperationTaskUpdateDto updateDto)
        {
            var entity = await _workOrderOperationTaskRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderOperationTaskRepository.UpdateAsync(entity);
        }
        // Implement service methods here
    }
}
