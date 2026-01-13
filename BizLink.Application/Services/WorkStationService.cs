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
    public class WorkStationService : IWorkStationService
    {

        private readonly IWorkStationRepository _workStationRepository;

        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkStationService(IWorkStationRepository workStationRepository, IMapper mapper)
        {
            _workStationRepository = workStationRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync(WorkStationCreateDto workStation)
        {
            var entity = _mapper.Map<WorkStation>(workStation);
            await _workStationRepository.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _workStationRepository.DeleteAsync(id);
        }

        public async Task<List<WorkStationDto>> GetAllAsync(int factoryid)
        {
            var entities = await _workStationRepository.GetAllAsync(factoryid);
            return entities.Select(x => _mapper.Map<WorkStationDto>(x)).ToList();
        }

        public async Task<WorkStationDto> GetByCodeAsync(string code)
        {
            var entity = await _workStationRepository.GetByCodeAsync(code);
            return _mapper.Map<WorkStationDto>(entity);
        }

        public async Task<WorkStationDto> GetByIdAsync(int id)
        {
            var entity = await _workStationRepository.GetByIdAsync(id);
            return _mapper.Map<WorkStationDto>(entity);
        }

        public async Task<List<WorkStationDto>> GetByIdAsync(List<int> id)
        {
            var entities = await _workStationRepository.GetByIdAsync(id);
            return _mapper.Map<List<WorkStationDto>>(entities);
        }

        public async Task<List<WorkStationDto>> GetByWorkcenterGroupCodeAsync(string groupcode)
        {
            var entities = await _workStationRepository.GetByWorkcenterGroupCodeAsync(groupcode);
            return entities.Select(x => _mapper.Map<WorkStationDto>(x)).ToList();
        }

        public async Task<List<WorkStationDto>> GetByWorkcenterIdAsync(int workcenterId)
        {
            var entities = await _workStationRepository.GetByWorkcenterIdAsync(workcenterId);
            return entities.Select(x => _mapper.Map<WorkStationDto>(x)).ToList();
        }

        public async Task UpdateAsync(WorkStationUpdateDto workStation)
        {
            var entity = _mapper.Map<WorkStation>(workStation);
            await _workStationRepository.UpdateAsync(entity);
        }
    }
}
