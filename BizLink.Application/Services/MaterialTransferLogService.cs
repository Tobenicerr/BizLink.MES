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
    public class MaterialTransferLogService : IMaterialTransferLogService
    {

        private readonly IMaterialTransferLogRepository _materialTransferLogRepository;

        private readonly IMapper _mapper; // 2. 声明 IMapper

        public MaterialTransferLogService(IMaterialTransferLogRepository materialTransferLogRepository, IMapper mapper) // 3. 通过构造函数注入 IMapper
        {
            _materialTransferLogRepository = materialTransferLogRepository;
            _mapper = mapper; // 4. 初始化 IMapper
        }
        public async Task<MaterialTransferLogDto> CreateAsync(MaterialTransferLogCreateDto createDto)
        {
            var entity = _mapper.Map<MaterialTransferLog>(createDto);
            var result =  await _materialTransferLogRepository.AddAsync(entity);
            return _mapper.Map<MaterialTransferLogDto>(result);
        }

        public async Task<List<int>> CreateBatchAsync(List<MaterialTransferLogCreateDto> createDto)
        {
            var entities = _mapper.Map<List<MaterialTransferLog>>(createDto);
            return await _materialTransferLogRepository.AddBulkAsync(entities);
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MaterialTransferLogDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<MaterialTransferLogDto> GetByIdAsync(int id)
        {
            var entity = await   _materialTransferLogRepository.GetByIdAsync(id);
            return _mapper.Map<MaterialTransferLogDto>(entity);
        }

        public async Task<MaterialTransferLogDto> GetByTransferNoAsync(string transferno, string materialcode, string batchcode)
        {
            var entity =  await _materialTransferLogRepository.GetByTransferNoAsync(transferno, materialcode,batchcode);
            return _mapper.Map<MaterialTransferLogDto>(entity);
        }

        public async Task<List<MaterialTransferLogDto>> GetByTransferNoAsync(string transferno)
        {
            var entities = await  _materialTransferLogRepository.GetByTransferNoAsync(transferno);
            return _mapper.Map<List<MaterialTransferLogDto>>(entities);
        }

        public async Task<List<MaterialTransferLogDto>> GetByTransferNoAsync(List<string> transfernos)
        {
            var entities = await _materialTransferLogRepository.GetByTransferNoAsync(transfernos);
            return _mapper.Map<List<MaterialTransferLogDto>>(entities);
        }

        public async Task<List<MaterialTransferLogDto>> GetListByIdsAsync(List<int> transferIds)
        {
            var entities = await  _materialTransferLogRepository.GetListByIdsAsync(transferIds);
            return _mapper.Map<List<MaterialTransferLogDto>>(entities);
        }

        public async Task<List<MaterialTransferLogDto>> GetListByStatusAsync(string? keyword, string? status)
        {
            var entities = await _materialTransferLogRepository.GetListByStatusAsync(keyword, status);
            return _mapper.Map<List<MaterialTransferLogDto>>(entities);
        }

        public Task<List<MaterialTransferLogDto>> GetListByStatusAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<MaterialTransferLogDto>> GetListByStockIdAsync(int stockid)
        {
            var entities = await _materialTransferLogRepository.GetListByStockIdAsync(stockid);
            return _mapper.Map<List<MaterialTransferLogDto>>(entities);
        }

        public async Task<List<MaterialTransferLogDto>> GetListByStockLogIdAsync(List<int> stockid)
        {
            var entities = await _materialTransferLogRepository.GetListByStockLogIdAsync(stockid);
            return _mapper.Map<List<MaterialTransferLogDto>>(entities);
        }

        public async Task<PagedResultDto<MaterialTransferLogDto>> GetPagedListAsync(int pageIndex, int pageSize, string? keyword, string? status, DateTime? createdStart, DateTime? createdEnd)
        {
            var (entities, totalCount) = await _materialTransferLogRepository.GetPagedListAsync(pageIndex, pageSize, keyword, status, createdStart, createdEnd);

            return new PagedResultDto<MaterialTransferLogDto> { Items = _mapper.Map<List<MaterialTransferLogDto>>(entities), TotalCount = totalCount };
        }

        public async Task<bool> UpdateAsync(MaterialTransferLogUpdateDto updateDto)
        {
            var entity = await _materialTransferLogRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity); // 5. 使用 IMapper 进行映射
            return await _materialTransferLogRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateListAsync(List<MaterialTransferLogUpdateDto> updateDto)
        {
            //var entityList = await _materialTransferLogRepository.GetListByIdsAsync(updateDto.Select(x => x.Id).ToList());
            //foreach (var entity in entityList)
            //{
            //    _mapper.Map(updateDto.Where(x => x.Id == entity.Id).First(), entity);
            //}
            //return await _materialTransferLogRepository.UpdateListAsync(entityList);

            // 1. 检查 DTO 列表是否为空，避免不必要的数据库查询
            if (updateDto == null || !updateDto.Any())
            {
                return true;
            }

            // 2. 将 DTO 列表转换为字典，以便 O(1) 快速查找
            //    键 = Id, 值 = D TO 对象
            //    注意：这假设 updateDtoList 中的 Id 是唯一的。
            //    如果 Id 不唯一，使用 .ToLookup() 或 .GroupBy().ToDictionary()
            var dtoDictionary = updateDto.ToDictionary(dto => dto.Id);

            // 3. 从仓储中获取所有需要更新的实体
            //    我们使用字典的 Key (即所有 ID) 来获取
            var entityList = await _materialTransferLogRepository.GetListByIdsAsync(dtoDictionary.Keys.ToList());

            // 4. 使用 O(n) 循环 + O(1) 字典查找来应用更改
            foreach (var entity in entityList)
            {
                // 5. 尝试从字典中获取匹配的 DTO
                if (dtoDictionary.TryGetValue(entity.Id, out var matchingDto))
                {
                    // 找到了，应用映射
                    _mapper.Map(matchingDto, entity);
                }
                // （如果没找到，您可能需要记录一个警告，但这不应该发生，
                //   因为我们是根据 DTO 的 ID 去获取实体的）
            }

            // 6. 一次性将所有更改提交到仓储
            return await _materialTransferLogRepository.UpdateListAsync(entityList);
        }
    }
}
