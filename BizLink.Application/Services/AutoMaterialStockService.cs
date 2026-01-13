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
    public class AutoMaterialStockService : IAutoMaterialStockService
    {
        private readonly IAutoMaterialStockRepository _autoMaterialStockRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper

        public AutoMaterialStockService(IAutoMaterialStockRepository autoMaterialStockRepository, IMapper mapper)
        {
            _autoMaterialStockRepository = autoMaterialStockRepository;
            _mapper = mapper;
        }

        public Task<AutoMaterialStockDto> CreateAsync(AutoMaterialStockCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<AutoMaterialStockCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AutoMaterialStockDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AutoMaterialStockDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AutoMaterialStockDto>> GetListByMaterialCodeAsync(List<string> materialcodes)
        {
            var result = await _autoMaterialStockRepository.GetListByMaterialCodeAsync(materialcodes);
            return _mapper.Map<List<AutoMaterialStockDto>>(result);
        }

        public Task<bool> UpdateAsync(AutoMaterialStockUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
