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
    public class StationMaterialLoadingService : IStationMaterialLoadingService
    {
        private readonly IStationMaterialLoadingRepository _stationMaterialLoadingRepository;
        private readonly IMapper _mapper;
        public StationMaterialLoadingService(IStationMaterialLoadingRepository stationMaterialLoadingRepository, IMapper mapper)
        {
            _stationMaterialLoadingRepository = stationMaterialLoadingRepository;
            _mapper = mapper;
        }

        public async Task<StationMaterialLoadingDto> CreateAsync(StationMaterialLoadingCreateDto createDto)
        {
            var entity = _mapper.Map<StationMaterialLoading>(createDto);
            var result = await _stationMaterialLoadingRepository.AddAsync(entity);
            return _mapper.Map<StationMaterialLoadingDto>(result);
        }

        public async Task<List<int>> CreateBatchAsync(List<StationMaterialLoadingCreateDto> createDto)
        {
            var entities = _mapper.Map<List<StationMaterialLoading>>(createDto);
            return await _stationMaterialLoadingRepository.AddBulkAsync(entities);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _stationMaterialLoadingRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<StationMaterialLoadingDto>> GetAllAsync()
        {
            var result = await _stationMaterialLoadingRepository.GetAllAsync();
            return _mapper.Map<List<StationMaterialLoadingDto>>(result);
        }

        public async Task<StationMaterialLoadingDto> GetByIdAsync(int id)
        {
            var entity = await _stationMaterialLoadingRepository.GetByIdAsync(id);
            return _mapper.Map<StationMaterialLoadingDto>(entity);

        }

        public async Task<List<StationMaterialLoadingDto>> GetListByFactoryIdAsync(int factoryId, string? materialCode = null, string? status = "1")
        {
            var entities = await _stationMaterialLoadingRepository.GetListByFactoryIdAsync(factoryId, materialCode, status);
            return _mapper.Map<List<StationMaterialLoadingDto>>(entities);
        }

        public async Task<List<StationMaterialLoadingDto>> GetListByStationIdAsync(int stationId, string? materialCode = null, string? status = "1")
        {
            var entities = await _stationMaterialLoadingRepository.GetListByStationIdAsync(stationId, materialCode, status);
            return _mapper.Map<List<StationMaterialLoadingDto>>(entities);
        }

        public async Task<bool> UpdateAsync(StationMaterialLoadingUpdateDto updateDto)
        {
            var entity = await _stationMaterialLoadingRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _stationMaterialLoadingRepository.UpdateAsync(entity);
        }
    }
}
