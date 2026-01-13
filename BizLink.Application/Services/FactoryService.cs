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
    public class FactoryService : IFactoryService
    {
        private readonly IFactoryRepository _factoryRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper

        public FactoryService(IFactoryRepository factoryRepository, IMapper mapper)
        {
            _factoryRepository = factoryRepository;
            _mapper = mapper;
        }

        public async Task<PagedResultDto<FactoryDto>> GetPagedListAsync(int pageIndex, int pageSize, string keyword, bool? isActive)
        {
            // 1. 从仓储层获取分页的实体数据
            var (factorys, totalCount) = await _factoryRepository.GetPagedListAsync(pageIndex, pageSize, keyword, isActive);


            //var factoryDtos = _mapper.Map<IEnumerable<FactoryDto>>(factorys);
            // 2. 将实体列表映射为 DTO 列表
            var factoryDtos = factorys.Select(f => new FactoryDto
            {
                Id = f.Id,
                FactoryCode = f.FactoryCode,
                FactoryName = f.FactoryName,
                Address = f.Address,
                IsActive = f.IsActive
            }).ToList();

            // 3. 创建并返回分页结果的 DTO (已更新为新的结构)
            return new PagedResultDto<FactoryDto>
            {
                Items = factoryDtos,
                TotalCount = totalCount
            };
        }

        // GetFactoryByIdAsync, CreateFactoryAsync, UpdateFactoryAsync, DeleteFactoryAsync 方法保持不变...

        public async Task<FactoryDto> GetByIdAsync(int factoryId)
        {
            var entity = await _factoryRepository.GetByIdAsync(factoryId);
            return _mapper.Map<FactoryDto>(entity);
        }

        public async Task CreateAsync(FactoryCreateDto input)
        {
            var factory = _mapper.Map<Factory>(input);

            factory.CreatedAt = DateTime.Now; // 手动设置 DTO 中没有的属性

            await _factoryRepository.AddAsync(factory);
        }

        public async Task UpdateAsync(FactoryUpdateDto input)
        {
            var factoryToUpdate = await _factoryRepository.GetByIdAsync(input.Id);
            if (factoryToUpdate == null)
            {
                throw new Exception("工厂不存在");
            }

            // 6. 使用 AutoMapper 将更新DTO中的值映射到已存在的实体上
            _mapper.Map(input, factoryToUpdate);

            await _factoryRepository.UpdateAsync(factoryToUpdate);
        }

        public async Task DeleteAsync(int id)
        {
            await _factoryRepository.DeleteAsync(id);
        }

        public async Task<List<FactoryDto>> GetAllAsync()
        {
            var entity =  await _factoryRepository.GetAllAsync();
            return entity.Select(x => _mapper.Map<FactoryDto>(x)).ToList();
        }
    }
}
