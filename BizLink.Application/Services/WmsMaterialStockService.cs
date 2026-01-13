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
    public class WmsMaterialStockService : IWmsMaterialStockService
    {
        private readonly IWmsMaterialStockRepository _wmsMaterialStockRepository;
        private readonly IMapper _mapper;


        public WmsMaterialStockService(IWmsMaterialStockRepository wmsMaterialStockRepository, IMapper mapper)
        {
            _wmsMaterialStockRepository = wmsMaterialStockRepository;
            _mapper = mapper;
        }
        public Task<WmsMaterialStockDto> CreateAsync(WmsMaterialStockCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WmsMaterialStockCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WmsMaterialStockDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<WmsMaterialStockDto>> GetBatchPageListAsync(int pageIndex, int pageSize, string factoryCode, string? keyword, List<string> materialcodes, List<string>? batchcodes, string? consumetype)
        {
            var (entities, totalCount) = await _wmsMaterialStockRepository.GetBatchPageListAsync(pageIndex, pageSize, factoryCode, keyword, materialcodes, batchcodes,consumetype);
            return new PagedResultDto<WmsMaterialStockDto> { Items = _mapper.Map<List<WmsMaterialStockDto>>(entities), TotalCount = totalCount };
        }

        public Task<WmsMaterialStockDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<WmsMaterialStockDto>> GetPageListAsync(string factoryCode, int pageIndex, int pageSize, string? keyword, List<string>? materialcodes, List<string>? batchcodes)
        {
            var (entities,totalCount) = await _wmsMaterialStockRepository.GetPageListAsync(factoryCode, pageIndex, pageSize, keyword, materialcodes, batchcodes);
            return new PagedResultDto<WmsMaterialStockDto> { Items = _mapper.Map<List<WmsMaterialStockDto>>(entities), TotalCount = totalCount };
        }

        public Task<bool> UpdateAsync(WmsMaterialStockUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
