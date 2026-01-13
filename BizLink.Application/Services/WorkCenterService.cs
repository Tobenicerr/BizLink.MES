using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkCenterService : IWorkCenterService
    {


        private readonly IWorkCenterRepository _workCenterRepository;

        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkCenterService(IWorkCenterRepository workCenterRepository, IMapper mapper)
        {
            _workCenterRepository = workCenterRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(WorkCenterCreateDto workCenter)
        {
            var entity = _mapper.Map<WorkCenter>(workCenter);

            await _workCenterRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _workCenterRepository.DeleteAsync(id);
        }

        public async Task<List<WorkCenterDto>> GetAllAsync(int factoryid)
        {
            var entities = await _workCenterRepository.GetAllAsync(factoryid);

            return entities.Select(x => _mapper.Map<WorkCenterDto>(x)).ToList();
        }

        public async Task<WorkCenterDto> GetByCodeAsync(string code)
        {
            var entity = await _workCenterRepository.GetByCodeAsync(code);

            return _mapper.Map<WorkCenterDto>(entity);
        }

        public async Task<List<WorkCenterDto>> GetBygroupIdAsync(int groupid)
        {
            var entities = await _workCenterRepository.GetBygroupIdAsync(groupid);

            return entities.Select(x => _mapper.Map<WorkCenterDto>(x)).ToList();
        }

        public async Task<WorkCenterDto> GetByIdAsync(int id)
        {
            var entity = await _workCenterRepository.GetByIdAsync(id);

            return _mapper.Map<WorkCenterDto>(entity);
        }

        public async Task<List<WorkCenterDto>> GetListByGroupCodeAsync(string groupcode)
        {
            var entities = await _workCenterRepository.GetListByGroupCodeAsync(groupcode);
            return _mapper.Map<List<WorkCenterDto>>(entities);
        }

        public async Task<List<WorkCenterDto>> GetListByGroupCodeAsync(List<string> groupcode)
        {
            var entities = await _workCenterRepository.GetListByGroupCodeAsync(groupcode);
            return _mapper.Map<List<WorkCenterDto>>(entities);
        }

        public async Task UpdateAsync(WorkCenterUpdateDto workCenter)
        {
            var entity = _mapper.Map<WorkCenter>(workCenter);

            await _workCenterRepository.UpdateAsync(entity);
        }
    }
}
