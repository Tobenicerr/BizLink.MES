using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    internal class CenterStockOutService : ICenterStockOutService
    {

        private readonly ICenterStockOutRepository _centerStockOutRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper


        public CenterStockOutService(ICenterStockOutRepository centerStockOutRepository, IMapper mapper)
        {
            _centerStockOutRepository = centerStockOutRepository;
            _mapper = mapper;
        }

        public Task<CenterStockOutDto> CreateAsync(CenterStockOutCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<CenterStockOutCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CenterStockOutDto>> GetAllAsync()
        {
            var result = await _centerStockOutRepository.GetListAsync();
            return _mapper.Map<IEnumerable<CenterStockOutDto>>(result);
        }

        public Task<CenterStockOutDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CenterStockOutDto>> GetListByWorkOrderAsync(string workorder)
        {
            var result = await _centerStockOutRepository.GetListByWorkOrderAsync(workorder);
            return _mapper.Map<List<CenterStockOutDto>>(result);
        }

        public async Task<List<CenterStockOutDto>> GetListByWorkOrderAsync(List<string> workorder)
        {
            var result = await _centerStockOutRepository.GetListByWorkOrderAsync(workorder);
            return _mapper.Map<List<CenterStockOutDto>>(result);
        }

        public Task<bool> UpdateAsync(CenterStockOutUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
