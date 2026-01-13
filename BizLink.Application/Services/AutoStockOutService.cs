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
    public class AutoStockOutService : IAutoStockOutService
    {
        private readonly IAutoStockOutRepository _autoStockOutRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper


        public AutoStockOutService(IAutoStockOutRepository autoStockOutRepository, IMapper mapper)
        {
            _autoStockOutRepository = autoStockOutRepository;
            _mapper = mapper;
        }

        public Task<AutoStockOutDto> CreateAsync(AutoStockOutCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<AutoStockOutCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AutoStockOutDto>> GetAllAsync()
        {
            var result = await _autoStockOutRepository.GetListAsync();
            return _mapper.Map<IEnumerable<AutoStockOutDto>>(result);
        }

        public Task<AutoStockOutDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AutoStockOutDto>> GetListByWorkOrderAsync(string workorder)
        {
            var result = await _autoStockOutRepository.GetListByWorkOrderAsync(workorder);
            return _mapper.Map<List<AutoStockOutDto>>(result);
        }

        public async Task<List<AutoStockOutDto>> GetListByWorkOrderAsync(List<string> workorder)
        {
            var result = await _autoStockOutRepository.GetListByWorkOrderAsync(workorder);
            return _mapper.Map<List<AutoStockOutDto>>(result);
        }

        public Task<bool> UpdateAsync(AutoStockOutUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
