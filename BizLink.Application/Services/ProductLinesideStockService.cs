using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class ProductLinesideStockService : IProductLinesideStockService
    {

        private readonly IProductLinesideStockRepository _productLinesideStockRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper
        public ProductLinesideStockService(IProductLinesideStockRepository productLinesideStockRepository, IMapper mapper)
        {
            _productLinesideStockRepository = productLinesideStockRepository;
            _mapper = mapper; // 3. 初始化 IMapper
        }

        public async Task<ProductLinesideStockDto> CreateAsync(ProductLinesideStockCreateDto createDto)
        {

            var entity = _mapper.Map<ProductLinesideStock>(createDto);

            var rtn =  await _productLinesideStockRepository.AddAsync(entity);
            return _mapper.Map<ProductLinesideStockDto>(rtn);
        }

        public Task<List<int>> CreateBatchAsync(List<ProductLinesideStockCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productLinesideStockRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<ProductLinesideStockDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ProductLinesideStockDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductLinesideStockDto>> GetListByOrderNoAsync(string orderno)
        {
            var entities =  await _productLinesideStockRepository.GetListByOrderNoAsync(orderno);
            return _mapper.Map<List<ProductLinesideStockDto>>(entities);
        }

        public async Task<List<ProductLinesideStockDto>> GetListByOrderNoAsync(List<string> orderno)
        {
            var entities = await _productLinesideStockRepository.GetListByOrderNoAsync(orderno);
            return _mapper.Map<List<ProductLinesideStockDto>>(entities);
        }

        public Task<bool> UpdateAsync(ProductLinesideStockUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateStatusAsync(List<ProductLinesideStockUpdateDto> updateDtos)
        {
            var entities = updateDtos.Select(x => _mapper.Map<ProductLinesideStock>(x)).ToList();
            return await _productLinesideStockRepository.UpdateStatusAsync(entities);
        }
    }
}
