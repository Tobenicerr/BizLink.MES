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
    public class RawLinesideStockService : IRawLinesideStockService
    {

        private readonly IRawLinesideStockRepository _rawLinesideStockRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper


        public RawLinesideStockService(IRawLinesideStockRepository rawLinesideStockRepository, IMapper mapper)
        {
            _rawLinesideStockRepository = rawLinesideStockRepository;
            _mapper = mapper;
        }

        public async Task<int> BatchUpdateAsync(List<RawLinesideStockUpdateDto> updateDtos)
        {
            // 1. 快速检查：如果没有数据，直接返回
            if (updateDtos == null || !updateDtos.Any())
            {
                return 0;
            }

            // 2. 提取 ID 列表用于批量查询
            var ids = updateDtos.Select(x => x.Id).ToList();

            // 3. 从数据库批量获取实体
            var entities = await _rawLinesideStockRepository.GetListByIdsAsync(ids);

            if (entities == null || !entities.Any())
            {
                return 0;
            }

            // 4. 【核心优化】：将 updateDtos 转换为字典 (Dictionary)
            // 这样在循环中查找 DTO 的时间复杂度从 O(N) 降低到 O(1)
            var updateDtoDict = updateDtos.ToDictionary(x => x.Id);

            // 5. 遍历实体并映射更新
            foreach (var entity in entities)
            {
                // 使用 TryGetValue 进行 O(1) 高效查找
                if (updateDtoDict.TryGetValue(entity.Id, out var updateDto))
                {
                    _mapper.Map(updateDto, entity);
                }
            }

            // 6. 批量保存到数据库
            return await _rawLinesideStockRepository.BatchUpdateAsync(entities);
        }

        public async Task<RawLinesideStockDto> CreateAsync(RawLinesideStockCreateDto createDto)
        {
            var entity = _mapper.Map<RawLinesideStock>(createDto);
            return _mapper.Map<RawLinesideStockDto>(await _rawLinesideStockRepository.AddAsync(entity));
        }

        public Task<List<int>> CreateBatchAsync(List<RawLinesideStockCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RawLinesideStockDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<RawLinesideStockDto>> GetAllAsync(int factoryid)
        {
            var entities =  await _rawLinesideStockRepository.GetAllAsync(factoryid);
            return entities.Select(x => _mapper.Map<RawLinesideStockDto>(x)).ToList();
        }

        public async Task<RawLinesideStockDto> GetByBarCodeAsync(int factoryid, string barcode)
        {
            var entity = await _rawLinesideStockRepository.GetByBarCodeAsync(factoryid, barcode);
            return _mapper.Map<RawLinesideStockDto>(entity);
        }

        public async Task<List<RawLinesideStockDto>> GetByBarCodeAsync(int factoryid, List<string> barcode)
        {
            var entities = await  _rawLinesideStockRepository.GetByBarCodeAsync(factoryid, barcode);
            return _mapper.Map<List<RawLinesideStockDto>>(entities);
        }

        public async Task<RawLinesideStockDto> GetByIdAsync(int id)
        {
            var entity = await _rawLinesideStockRepository.GetByIdAsync(id);
            return _mapper.Map<RawLinesideStockDto>(entity);
        }

        public async Task<List<RawLinesideStockDto>> GetListByMaterialCodeAsync(int factoryid, string materialcode)
        {
            var entities = await _rawLinesideStockRepository.GetListByMaterialCodeAsync(factoryid, materialcode);
            return entities.Select(x => _mapper.Map<RawLinesideStockDto>(x)).ToList();
        }

        public async Task<List<RawLinesideStockDto>> GetListByMaterialCodeAsync(int factoryid, List<string> materialcode)
        {
            var entities = await _rawLinesideStockRepository.GetListByMaterialCodeAsync(factoryid, materialcode);
            return _mapper.Map<List<RawLinesideStockDto>>(entities);
        }

        public async Task<PagedResultDto<RawLinesideStockDto>> GetBatchPageListAsync(int pageIndex, int pageSize, int factoryid, string? keyword, bool quantitySwitch = true, List<string>? materialcodes = null, List<string>? batchcodes = null)
        {
            var (result,totalcount) = await _rawLinesideStockRepository.GetBatchPageListAsync(pageIndex, pageSize, factoryid, keyword, quantitySwitch, materialcodes, batchcodes);
            return new PagedResultDto<RawLinesideStockDto>
            {
                Items = result.Select(x => _mapper.Map<RawLinesideStockDto>(x)).ToList(),
                TotalCount = totalcount
            };
        }

        public async Task<bool> UpdateAsync(RawLinesideStockUpdateDto updateDto)
        {
            var entity = await _rawLinesideStockRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _rawLinesideStockRepository.UpdateAsync(entity);
        }
    }
}
