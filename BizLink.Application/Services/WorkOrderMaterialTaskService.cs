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

        public async Task<bool> UpdateAsync(WorkOrderMaterialTaskUpdateDto updateDto)
        {
            var entity = await _workOrderMaterialTaskRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderMaterialTaskRepository.UpdateAsync(entity);
        }
    }
}
