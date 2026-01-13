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
    public class WorkCenterGroupService : IWorkCenterGroupService
    {
        private readonly IWorkCenterGroupRepository _workCenterGroupRepository;
        private readonly IMapper _mapper;
        public WorkCenterGroupService(IWorkCenterGroupRepository workCenterGroupRepository, IMapper mapper)
        {
            _workCenterGroupRepository = workCenterGroupRepository;
            _mapper = mapper;
        }
        public Task<WorkCenterGroupDto> CreateAsync(WorkCenterGroupCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WorkCenterGroupCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WorkCenterGroupDto>> GetAllAsync()
        {
            var entities = await _workCenterGroupRepository.GetAllAsync();
            return _mapper.Map<List<WorkCenterGroupDto>>(entities);
        }

        public async Task<WorkCenterGroupDto> GetByIdAsync(int id)
        {
            var entity =  await _workCenterGroupRepository.GetByIdAsync(id);
            return _mapper.Map<WorkCenterGroupDto>(entity);
        }

        public async Task<List<WorkCenterGroupDto>> GetListByGroupTypeAsync(int factoryid,string grouptype)
        {
            var entities = await _workCenterGroupRepository.GetListByGroupTypeAsync(factoryid,grouptype);
            return _mapper.Map<List<WorkCenterGroupDto>>(entities);
        }

        public Task<bool> UpdateAsync(WorkCenterGroupUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
