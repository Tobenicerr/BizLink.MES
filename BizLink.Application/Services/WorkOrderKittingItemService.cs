using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderKittingItemService : IWorkOrderKittingItemService
    {
        private readonly IWorkOrderKittingItemRepository _repo;
        private readonly IMapper _mapper;
        public WorkOrderKittingItemService(IWorkOrderKittingItemRepository repo, IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<WorkOrderKittingItemDto> CreateAsync(WorkOrderKittingItemCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WorkOrderKittingItemCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderKittingItemDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderKittingItemDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkOrderKittingItemDto>> GetListByProcessIdAsync(int processId)
        {
            var entities = await _repo.GetListByProcessIdAsync(processId);
            return _mapper.Map<List<WorkOrderKittingItemDto>>(entities);
        }

        public async Task<List<WorkOrderKittingItemDto>> GetListByProcessIdAsync(List<int> processIds)
        {
            var entities = await _repo.GetListByProcessIdAsync(processIds);
            return _mapper.Map<List<WorkOrderKittingItemDto>>(entities);
        }

        public Task<bool> UpdateAsync(WorkOrderKittingItemUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
