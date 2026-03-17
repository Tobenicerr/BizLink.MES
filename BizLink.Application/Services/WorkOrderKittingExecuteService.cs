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
    public class WorkOrderKittingExecuteService : IWorkOrderKittingExecuteService
    {

        private readonly IWorkOrderKittingExecuteRepository _workOrderKittingExecuteRepository;
        private readonly IMapper _mapper;

        public WorkOrderKittingExecuteService(IWorkOrderKittingExecuteRepository workOrderKittingExecuteRepository,IMapper mapper) 
        {
            _workOrderKittingExecuteRepository = workOrderKittingExecuteRepository;
            _mapper = mapper;
        }
        public Task<WorkOrderKittingExecuteDto> CreateAsync(WorkOrderKittingExecuteCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WorkOrderKittingExecuteCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderKittingExecuteDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderKittingExecuteDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkOrderKittingExecuteDto>> GetListByGroupCodeAsync(string groupCode)
        {
            var entities = await _workOrderKittingExecuteRepository.GetListByGroupCodeAsync(groupCode);
            return _mapper.Map<List<WorkOrderKittingExecuteDto>>(entities);
        }

        public async Task<PagedResultDto<WorkOrderKittingExecuteDto>> GetPageListAsync(int pageIndex, int pageSize, int factoryId, DateTime? startdateStart, DateTime? startdateEnd, List<string> workOrders, List<string> groupCodes)
        {
            var (entities, totalCount) = await _workOrderKittingExecuteRepository.GetPageListAsync(pageIndex, pageSize, factoryId, startdateStart, startdateEnd, workOrders, groupCodes);
            return new PagedResultDto<WorkOrderKittingExecuteDto>
            {
                Items = _mapper.Map<List<WorkOrderKittingExecuteDto>>(entities),
                TotalCount = totalCount,
            };
        }

        public Task<bool> UpdateAsync(WorkOrderKittingExecuteUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
