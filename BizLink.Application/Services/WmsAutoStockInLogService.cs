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
    public class WmsAutoStockInLogService : IWmsAutoStockInLogService
    {
        private readonly IWmsAutoStockInLogRepository _repo;
        private readonly IMapper _mapper;

        public WmsAutoStockInLogService(IWmsAutoStockInLogRepository repo, IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<WmsAutoStockInLogDto> CreateAsync(WmsAutoStockInLogCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WmsAutoStockInLogCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WmsAutoStockInLogDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WmsAutoStockInLogDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WmsAutoStockInLogDto>> GetFailListAsync(string factoryCode, string? keyword = null)
        {
            var entities = await _repo.GetFailListAsync(factoryCode, keyword);
            return _mapper.Map<List<WmsAutoStockInLogDto>>(entities);
        }

        public async Task<bool> UpdateAsync(WmsAutoStockInLogUpdateDto updateDto)
        {
           var entity = await _repo.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _repo.UpdateAsync(entity);

        }
    }
}
