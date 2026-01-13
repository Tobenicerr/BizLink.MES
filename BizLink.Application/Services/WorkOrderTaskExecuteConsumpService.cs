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
    public class WorkOrderTaskExecuteConsumpService : IWorkOrderTaskExecuteConsumpService
    {
        private readonly IWorkOrderTaskExecuteConsumpRepository _workOrderTaskExecuteConsumpRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkOrderTaskExecuteConsumpService(IWorkOrderTaskExecuteConsumpRepository workOrderTaskExecuteConsumpRepository, IMapper mapper)
        {
            _workOrderTaskExecuteConsumpRepository = workOrderTaskExecuteConsumpRepository;
            _mapper = mapper;
        }

        public async Task<WorkOrderTaskExecuteConsumpDto> CreateAsync(WorkOrderTaskExecuteConsumpCreateDto createDto)
        {
            var entity = await _workOrderTaskExecuteConsumpRepository.AddAsync(_mapper.Map<BizLink.MES.Domain.Entities.WorkOrderTaskExecuteConsump>(createDto));
            return _mapper.Map<WorkOrderTaskExecuteConsumpDto>(entity);

        }

        public async Task<List<int>> CreateBatchAsync(List<WorkOrderTaskExecuteConsumpCreateDto> createDto)
        {
            return await _workOrderTaskExecuteConsumpRepository.AddBulkAsync(_mapper.Map<List<WorkOrderTaskExecuteConsump>>(createDto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workOrderTaskExecuteConsumpRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkOrderTaskExecuteConsumpDto>> GetAllAsync()
        {
            var entities = await _workOrderTaskExecuteConsumpRepository.GetAllAsync();
            return _mapper.Map<List<WorkOrderTaskExecuteConsumpDto>>(entities);
        }

        public async Task<WorkOrderTaskExecuteConsumpDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderTaskExecuteConsumpRepository.GetByIdAsync(id);
            return _mapper.Map<WorkOrderTaskExecuteConsumpDto>(entity);
        }

        public async Task<bool> UpdateAsync(WorkOrderTaskExecuteConsumpUpdateDto updateDto)
        {
            var entity = await _workOrderTaskExecuteConsumpRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderTaskExecuteConsumpRepository.UpdateAsync(entity);
        }
    }
}
