using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkTaskCategoryService : IWorkTaskCategoryService
    {
        private readonly IWorkTaskCategoryRepository _workTaskCategoryRepository;
        private readonly IMapper _mapper;

        public WorkTaskCategoryService(IWorkTaskCategoryRepository workTaskCategoryRepository, IMapper mapper) 
        {
            _workTaskCategoryRepository = workTaskCategoryRepository;
            _mapper = mapper;
        }
        public async Task<WorkTaskCategoryDto> CreateAsync(WorkTaskCategoryCreateDto createDto)
        {
            var entity = _mapper.Map<WorkTaskCategory>(createDto);
            var result = await _workTaskCategoryRepository.AddAsync(entity);
            return _mapper.Map<WorkTaskCategoryDto>(result);
        }

        public async Task<List<int>> CreateBatchAsync(List<WorkTaskCategoryCreateDto> createDto)
        {
            var entities =  _mapper.Map<List<WorkTaskCategory>>(createDto);
            return await _workTaskCategoryRepository.AddBulkAsync(entities);

        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workTaskCategoryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<WorkTaskCategoryDto>> GetAllAsync()
        {
            var entities =  await _workTaskCategoryRepository.GetAllAsync();
            return _mapper.Map<List<WorkTaskCategoryDto>>(entities);
        }

        public async Task<WorkTaskCategoryDto> GetByCategoryCodeAsync(string categoryCode)
        {
            var entity = await _workTaskCategoryRepository.GetByCategoryCodeAsync(categoryCode);
            return _mapper.Map<WorkTaskCategoryDto>(entity);
        }

        public async Task<WorkTaskCategoryDto> GetByIdAsync(int id)
        {
            var entity = await _workTaskCategoryRepository.GetByIdAsync(id);
            return _mapper.Map<WorkTaskCategoryDto>(entity);
        }

        public async Task<List<WorkTaskCategoryDto>> GetListByStepTypeAsync(string stepType)
        {
            var entities = await _workTaskCategoryRepository.GetListByStepTypeAsync(stepType);
            return _mapper.Map<List<WorkTaskCategoryDto>>(entities);
        }

        public async Task<bool> UpdateAsync(WorkTaskCategoryUpdateDto updateDto)
        {
            var entity = await _workTaskCategoryRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workTaskCategoryRepository.UpdateAsync(entity);

        }
    }
}
