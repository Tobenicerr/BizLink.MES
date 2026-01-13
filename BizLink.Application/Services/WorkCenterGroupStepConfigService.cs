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
    public class WorkCenterGroupStepConfigService : IWorkCenterGroupStepConfigService
    {
        private readonly IWorkCenterGroupStepConfigRepository _workCenterGroupStepConfigRepository;
        private readonly IMapper _mapper;

        public WorkCenterGroupStepConfigService(IWorkCenterGroupStepConfigRepository workCenterGroupStepConfigRepository, IMapper mapper)
        {
            _workCenterGroupStepConfigRepository = workCenterGroupStepConfigRepository;
            _mapper = mapper;
        }

        public async Task<WorkCenterGroupStepConfigDto> CreateAsync(WorkCenterGroupStepConfigCreateDto createDto)
        {
            var entity = await _workCenterGroupStepConfigRepository.AddAsync(_mapper.Map<WorkCenterGroupStepConfig>(createDto));
            return _mapper.Map<WorkCenterGroupStepConfigDto>(entity);

        }

        public async Task<List<int>> CreateBatchAsync(List<WorkCenterGroupStepConfigCreateDto> createDto)
        {
            return await _workCenterGroupStepConfigRepository.AddBulkAsync(_mapper.Map<List<WorkCenterGroupStepConfig>>(createDto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workCenterGroupStepConfigRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkCenterGroupStepConfigDto>> GetAllAsync()
        {
            var entities = await  _workCenterGroupStepConfigRepository.GetAllAsync();
            return _mapper.Map<List<WorkCenterGroupStepConfigDto>>(entities);
        }

        public async Task<WorkCenterGroupStepConfigDto> GetByIdAsync(int id)
        {
            var entity = await _workCenterGroupStepConfigRepository.GetByIdAsync(id);
            return _mapper.Map<WorkCenterGroupStepConfigDto>(entity);
        }

        public async Task<bool> UpdateAsync(WorkCenterGroupStepConfigUpdateDto updateDto)
        {
            var entity = await _workCenterGroupStepConfigRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workCenterGroupStepConfigRepository.UpdateAsync(entity);
        }
    }
}
