using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderStepTaskService : IWorkOrderStepTaskService
    {
        private readonly IWorkOrderStepTaskRepository _workOrderStepTaskRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper
        public WorkOrderStepTaskService(IWorkOrderStepTaskRepository workOrderStepTaskRepository, IMapper mapper)
        {
            _workOrderStepTaskRepository = workOrderStepTaskRepository;
            _mapper = mapper;
        }

        public async Task<WorkOrderStepTaskDto> CreateAsync(WorkOrderStepTaskCreateDto createDto)
        {
            var entity = await _workOrderStepTaskRepository.AddAsync(_mapper.Map<BizLink.MES.Domain.Entities.WorkOrderStepTask>(createDto));
            return _mapper.Map<WorkOrderStepTaskDto>(entity);
        }

        public async Task<List<int>> CreateBatchAsync(List<WorkOrderStepTaskCreateDto> createDto)
        {
            return await _workOrderStepTaskRepository.AddBulkAsync(_mapper.Map<List<BizLink.MES.Domain.Entities.WorkOrderStepTask>>(createDto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workOrderStepTaskRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkOrderStepTaskDto>> GetAllAsync()
        {
            var entities = await _workOrderStepTaskRepository.GetAllAsync();
            return _mapper.Map<List<WorkOrderStepTaskDto>>(entities);
        }

        public async Task<WorkOrderStepTaskDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderStepTaskRepository.GetByIdAsync(id);
            return _mapper.Map<WorkOrderStepTaskDto>(entity);
        }

        public async Task<bool> UpdateAsync(WorkOrderStepTaskUpdateDto updateDto)
        {
            var entity = await _workOrderStepTaskRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderStepTaskRepository.UpdateAsync(entity);
        }
    }
}
