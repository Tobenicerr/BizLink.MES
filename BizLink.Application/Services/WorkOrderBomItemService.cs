using AutoMapper;
using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderBomItemService : IWorkOrderBomItemService
    {

        private readonly IWorkOrderBomItemRepository _workOrderBomItemRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper
        public WorkOrderBomItemService(IWorkOrderBomItemRepository workOrderBomItemRepository, IMapper mapper, IMaterialViewRepository materialViewRepository)
        {
            _workOrderBomItemRepository = workOrderBomItemRepository;
            _mapper = mapper; // 3. 初始化 IMapper
        }

        public async  Task<WorkOrderBomItemDto> CreateAsync(WorkOrderBomItemCreateDto createDto)
        {
            var entity = _mapper.Map<WorkOrderBomItem>(createDto);
            var result =  await  _workOrderBomItemRepository.AddAsync(entity);
            return _mapper.Map<WorkOrderBomItemDto>(result);
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderBomItemDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderBomItemDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkOrderBomItemDto>> GetListByOrderIdAync(int orderid)
        {
            var entities = await _workOrderBomItemRepository.GetListByOrderIdAync(orderid);
            return entities.Select(x => _mapper.Map<WorkOrderBomItemDto>(x)).ToList();
        }

        public async Task<List<WorkOrderBomItemDto>> GetListByOrderNoAync(string orderno)
        {
            var entities = await _workOrderBomItemRepository.GetListByOrderNoAync(orderno);
            return entities.Select(x => _mapper.Map<WorkOrderBomItemDto>(x)).ToList();
        }

        public async Task<List<WorkOrderBomItemDto>> GetListByOrderIdsAync(List<int> orderids)
        {
            var entities = await _workOrderBomItemRepository.GetListByOrderIdsAync(orderids);
            return entities.Select(x => _mapper.Map<WorkOrderBomItemDto>(x)).ToList();

        }

        public async Task<bool> UpdateAsync(WorkOrderBomItemUpdateDto updateDto)
        {
            var entity = await _workOrderBomItemRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderBomItemRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateWmsStatusAsync(List<WorkOrderBomItemUpdateDto> updateDtos)
        {
            var entities = updateDtos.Select(x => _mapper.Map<WorkOrderBomItem>(x)).ToList();
            return await _workOrderBomItemRepository.UpdateWmsStatusAsync(entities);
        }

        public async Task<List<WorkOrderBomItemDto>> GetListByOrderNoAync(List<string> orderno)
        {
            var entities = await _workOrderBomItemRepository.GetListByOrderNoAync(orderno);
            return entities.Select(x => _mapper.Map<WorkOrderBomItemDto>(x)).ToList();
        }

        public async Task<bool> CreateBatchAsync(List<WorkOrderBomItemCreateDto> createDtos)
        {
            return await  _workOrderBomItemRepository.CreateBatch(createDtos.Select(x => _mapper.Map<WorkOrderBomItem>(x)).ToList());
        }

        Task<List<int>> IGenericService<WorkOrderBomItemDto, WorkOrderBomItemCreateDto, WorkOrderBomItemUpdateDto>.CreateBatchAsync(List<WorkOrderBomItemCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkOrderBomItemDto>> GetListByProcessIdsAsync(List<int> processids)
        {
            var entities = await _workOrderBomItemRepository.GetListByProcessIdsAsync(processids);
            return _mapper.Map<List<WorkOrderBomItemDto>>(entities);
        }

        public async Task<List<WorkOrderBomItemDto>> GetOngoingCableListByProcessIdsAsync(List<int> processids)
        {
            var entities = await _workOrderBomItemRepository.GetOngoingCableListByProcessIdsAsync(processids);
            return _mapper.Map<List<WorkOrderBomItemDto>>(entities);
        }

        public async Task<List<WorkOrderBomItemDto>> GetOngoingCableListByDateAsync(int factoryid, DateTime startdate)
        {
            var entities = await _workOrderBomItemRepository.GetOngoingCableListByDateAsync(factoryid, startdate);
            return _mapper.Map<List<WorkOrderBomItemDto>>(entities);
        }

        public async Task<bool> UpdateBatchAsync(List<WorkOrderBomItemUpdateDto> updateDtos)
        {
            if (updateDtos == null || !updateDtos.Any())
            {
                return true;
            }

            var dtoDictionary = updateDtos.ToDictionary(dto => dto.Id);

            var entityList = await _workOrderBomItemRepository.GetByIdAsync(dtoDictionary.Keys.ToList());
            foreach (var entity in entityList)
            {
                // 5. 尝试从字典中获取匹配的 DTO
                if (dtoDictionary.TryGetValue(entity.Id, out var matchingDto))
                {
                    // 找到了，应用映射
                    _mapper.Map(matchingDto, entity);
                }

            }
            return await _workOrderBomItemRepository.UpdateBatchAsync(entityList);
        }
    }
}
