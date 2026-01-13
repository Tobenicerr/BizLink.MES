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
    public class WarehouseLocationService : IWarehouseLocationService
    {


        private readonly IWarehouseLocationRepository _warehouseLocationRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper


        public WarehouseLocationService(IWarehouseLocationRepository warehouseLocationRepository, IMapper mapper)
        {
            _warehouseLocationRepository = warehouseLocationRepository;
            _mapper = mapper;
        }
        public Task<WarehouseLocationDto> CreateAsync(WarehouseLocationCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WarehouseLocationCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WarehouseLocationDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<WarehouseLocationDto>> GetAllBinAsync()
        {
            var entities =  await _warehouseLocationRepository.GetAllBinAsync();
            return entities.Select(x => _mapper.Map<WarehouseLocationDto>(x)).ToList();
        }

        public async Task<WarehouseLocationDto> GetByIdAsync(int id)
        {
           var entity = await _warehouseLocationRepository.GetByIdAsync(id);
            return _mapper.Map<WarehouseLocationDto>(entity);
        }

        public Task<bool> UpdateAsync(WarehouseLocationUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
