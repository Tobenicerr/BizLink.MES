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
    public class WmsAutoStockOutLogService : IWmsAutoStockOutLogService
    {
        private readonly IWmsAutoStockOutLogRepository _wmsAutoStockOutLogRepository;
        private readonly IMapper _mapper;


        public WmsAutoStockOutLogService(IWmsAutoStockOutLogRepository wmsAutoStockOutLogRepository, IMapper mapper)
        {
            _wmsAutoStockOutLogRepository = wmsAutoStockOutLogRepository;
            _mapper = mapper;
        }




        public Task<WmsAutoStockOutLogDto> CreateAsync(WmsAutoStockOutLogCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WmsAutoStockOutLogCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WmsAutoStockOutLogDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WmsAutoStockOutLogDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WmsAutoStockOutLogDto>> GetFailListAsync(string factoryCode, string? keyword = null)
        {
            var entities =  await _wmsAutoStockOutLogRepository.GetFailListAsync(factoryCode, keyword);
            return _mapper.Map<List<WmsAutoStockOutLogDto>>(entities);
        }

        public async Task<bool> UpdateAsync(WmsAutoStockOutLogUpdateDto updateDto)
        {
            var entity = await _wmsAutoStockOutLogRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _wmsAutoStockOutLogRepository.UpdateAsync(entity);
        }
    }
}
