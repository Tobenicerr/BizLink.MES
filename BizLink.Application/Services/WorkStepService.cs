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
    public class WorkStepService : IWorkStepService
    {
        private readonly IWorkStepRepository _workStepRepository;

        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkStepService(IWorkStepRepository workStepRepository, IMapper mapper)
        {
            _workStepRepository = workStepRepository;
            _mapper = mapper;
        }

        public async Task<WorkStepDto> CreateAsync(WorkStepCreateDto createDto)
        {
            var entity = await _workStepRepository.AddAsync(_mapper.Map<WorkStep>(createDto));
            return _mapper.Map<WorkStepDto>(entity);
        }

        public async Task<List<int>> CreateBatchAsync(List<WorkStepCreateDto> createDto)
        {
            return await _workStepRepository.AddBulkAsync(_mapper.Map<List<WorkStep>>(createDto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workStepRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkStepDto>> GetAllAsync()
        {
            var entities = await _workStepRepository.GetAllAsync();
            return _mapper.Map<List<WorkStepDto>>(entities);
        }

        public async Task<WorkStepDto> GetByIdAsync(int id)
        {
            var entity = await _workStepRepository.GetByIdAsync(id);
            return _mapper.Map<WorkStepDto>(entity);
        }

        public async Task<bool> UpdateAsync(WorkStepUpdateDto updateDto)
        {
            var entity = await _workStepRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workStepRepository.UpdateAsync(entity);
        }
    }
}
