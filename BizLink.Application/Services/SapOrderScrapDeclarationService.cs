using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class SapOrderScrapDeclarationService : ISapOrderScrapDeclarationService
    {
        private readonly ISapOrderScrapDeclarationRepository _sapOrderScrapDeclarationRepository;
        private readonly IMapper _mapper;
        public SapOrderScrapDeclarationService(ISapOrderScrapDeclarationRepository sapOrderScrapDeclarationRepository, IMapper mapper)
        {
            _sapOrderScrapDeclarationRepository = sapOrderScrapDeclarationRepository;
            _mapper = mapper;
        }
        public async Task<SapOrderScrapDeclarationDto> CreateAsync(SapOrderScrapDeclarationCreateDto createDto)
        {
            var entity = await _sapOrderScrapDeclarationRepository.AddAsync(_mapper.Map<SapOrderScrapDeclaration>(createDto));
            return _mapper.Map<SapOrderScrapDeclarationDto>(entity);

        }

        public async Task<List<int>> CreateBatchAsync(List<SapOrderScrapDeclarationCreateDto> createDto)
        {
            return await _sapOrderScrapDeclarationRepository.AddBulkAsync(_mapper.Map<List<SapOrderScrapDeclaration>>(createDto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _sapOrderScrapDeclarationRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<SapOrderScrapDeclarationDto>> GetAllAsync()
        {
            var entities = await _sapOrderScrapDeclarationRepository.GetAllAsync();
            return _mapper.Map<List<SapOrderScrapDeclarationDto>>(entities);
        }

        public async Task<SapOrderScrapDeclarationDto> GetByIdAsync(int id)
        {
            var entity = await _sapOrderScrapDeclarationRepository.GetByIdAsync(id);
            return _mapper.Map<SapOrderScrapDeclarationDto>(entity);
        }

        public async Task<List<SapOrderScrapDeclarationDto>> GetListByOperationAsync(string workOrderNo, string operationNo)
        {
            var entities = await _sapOrderScrapDeclarationRepository.GetListByOperationAsync(workOrderNo, operationNo);
            return _mapper.Map<List<SapOrderScrapDeclarationDto>>(entities);
        }

        public async Task<List<SapOrderScrapDeclarationDto>> GetListByOrderNosAsync(List<string> workOrderNos)
        {
            var entities = await _sapOrderScrapDeclarationRepository.GetListByOrderNosAsync(workOrderNos);
            return _mapper.Map<List<SapOrderScrapDeclarationDto>>(entities);
        }

        public async Task<List<SapOrderScrapDeclarationDto>> GetListByWorkCenterAsync(string factorycode, List<string>? workcentercodes, DateTime? startdate, DateTime? enddate, List<string>? workorders)
        {
            var entities = await _sapOrderScrapDeclarationRepository.GetListByWorkCenterAsync( factorycode, workcentercodes, startdate, enddate, workorders);
            return _mapper.Map<List<SapOrderScrapDeclarationDto>>(entities);
        }

        public async Task<PagedResultDto<SapOrderScrapDeclarationDto>> GetPageListAsync(int pageIndex, int pageSize,string factoryCode, string? keyword, DateTime? createdDate)
        {
            var (entities,totalCount) = await _sapOrderScrapDeclarationRepository.GetPageListAsync(pageIndex, pageSize, factoryCode, keyword, createdDate);
            return new PagedResultDto<SapOrderScrapDeclarationDto>
            {
                Items = _mapper.Map<List<SapOrderScrapDeclarationDto>>(entities),
                TotalCount = totalCount
            };
        }

        public async Task<bool> UpdateAsync(SapOrderScrapDeclarationUpdateDto updateDto)
        {
            var entity = await _sapOrderScrapDeclarationRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity); 
            return await _sapOrderScrapDeclarationRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateBatchAsync(List<SapOrderScrapDeclarationUpdateDto> updateDtos)
        {
            var entities = await _sapOrderScrapDeclarationRepository.GetByIdsAsync(updateDtos.Select(x => x.Id).ToList());

            var dtoDictionary = updateDtos.ToDictionary(dto => dto.Id);

            foreach (var entity in entities)
            {
                // 5. 尝试从字典中获取匹配的 DTO
                if (dtoDictionary.TryGetValue(entity.Id, out var matchingDto))
                {
                    // 找到了，应用映射
                    _mapper.Map(matchingDto, entity);
                }

            }
            return await _sapOrderScrapDeclarationRepository.UpdateBulkAsync(entities);
        }
    }
}
