using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderProcessService : IWorkOrderProcessService
    {

        private readonly IWorkOrderProcessRepository _workOrderProcessRepository;


        private readonly IMapper _mapper; // 2. 声明 IMapper
        private readonly IUnitOfWork _unitOfWork;
        public WorkOrderProcessService( IMapper mapper, IUnitOfWork unitOfWork, IWorkOrderProcessRepository workOrderProcessRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _workOrderProcessRepository = workOrderProcessRepository;
        }

        public async Task<List<WorkOrderProcessDto>> GetListByOrderIdAync(int orderid)
        {
            var entities = await _workOrderProcessRepository.GetListByOrderIdAync(orderid);
            return entities.Select(x => _mapper.Map<WorkOrderProcessDto>(x)).ToList();
        }

        public async Task<WorkOrderProcessDto> GetByOrderNo(string orderno, string operation)
        {
            var entity = await _workOrderProcessRepository.GetByOrderNo(orderno, operation);
            return _mapper.Map<WorkOrderProcessDto>(entity);
        }

        public async Task<List<WorkOrderProcessDto>> GetListByOrderNos(List<string> ordernos)
        {
            var entities = await _workOrderProcessRepository.GetListByOrderNos(ordernos);
            return entities.Select(x => _mapper.Map<WorkOrderProcessDto>(x)).ToList();
        }

        public async Task<List<WorkOrderProcessDto>> GetListByOrderIdsAync(List<int> orderids)
        {
            var entities = await _workOrderProcessRepository.GetListByOrderIdsAync(orderids);
            return entities.Select(x => _mapper.Map<WorkOrderProcessDto>(x)).ToList();
        }

        public async Task<WorkOrderProcessDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderProcessRepository.GetByIdAsync(id);
            return _mapper.Map<WorkOrderProcessDto>(entity);
        }

        public async Task<List<WorkOrderProcessDto>> GetByIdAsync(List<int> id)
        {
            var entity = await _workOrderProcessRepository.GetByIdAsync(id);
            return _mapper.Map<List<WorkOrderProcessDto>>(entity);
        }

        public Task<IEnumerable<WorkOrderProcessDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderProcessDto> CreateAsync(WorkOrderProcessCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync(WorkOrderProcessUpdateDto updateDto)
        {
            var entity = await _workOrderProcessRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderProcessRepository.UpdateAsync(entity);
        }

        public async Task<bool> UpdateBatchAsync(List<WorkOrderProcessUpdateDto> updateDtos)
        {
            if (updateDtos == null || !updateDtos.Any())
            {
                return true;
            }

            var dtoDictionary = updateDtos.ToDictionary(dto => dto.Id);

            var entityList = await _workOrderProcessRepository.GetByIdAsync(dtoDictionary.Keys.ToList());
            foreach (var entity in entityList)
            {
                // 5. 尝试从字典中获取匹配的 DTO
                if (dtoDictionary.TryGetValue(entity.Id, out var matchingDto))
                {
                    // 找到了，应用映射
                    _mapper.Map(matchingDto, entity);
                }

            }
            return await _workOrderProcessRepository.UpdateBatchAsync(entityList);
        }



        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GetAllOperationAsync()
        {
            return await _workOrderProcessRepository.GetAllOperationAsync();    
        }

        public async Task<List<WorkOrderProcessDto>> GetListByDispatchDateAsync(int factoryid, DateTime startdate, string? operation = null, string? status = null, string? nostatus = null)
        {
            var entities = await _workOrderProcessRepository.GetListByDispatchDateAsync(factoryid, startdate, operation, status, nostatus);
            return _mapper.Map<List<WorkOrderProcessDto>>(entities);
        }

        public async Task<List<int>> CreateBatchAsync(List<WorkOrderProcessCreateDto> createDto)
        {
            return await _workOrderProcessRepository.AddBulkAsync(_mapper.Map<List<WorkOrderProcess>>(createDto));
        }
    }
}
