using AutoMapper;
using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class WorkOrderViewService : IWorkOrderViewService
    {
        private readonly IWorkOrderViewRepository _workOrderViewRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper


        public WorkOrderViewService(IWorkOrderViewRepository workOrderViewRepository, IMapper mapper)
        {
            _workOrderViewRepository = workOrderViewRepository;
            _mapper = mapper;
        }

        public async Task<WorkOrderViewDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderViewRepository.GetByOrderIdAsync(id);
            return _mapper.Map<WorkOrderViewDto>(entity);
        }

        public async Task<PagedResultDto<WorkOrderViewDto>> GetPagedListAsync(int pageIndex, int pageSize, string keyword, DateTime? dispatchDate, DateTime? startDate, string factorycode)
        {
            var (orders, totalCount) = await _workOrderViewRepository.GetPagedListAsync(pageIndex, pageSize, keyword, dispatchDate, startDate, factorycode);

            return new PagedResultDto<WorkOrderViewDto> { Items = orders.Select(x => _mapper.Map<WorkOrderViewDto>(x)), TotalCount = totalCount };

        }

        public async Task UpdateJyWmsRequrementTaskAsync()
        {
            await _workOrderViewRepository.UpdateJyWmsRequrementTaskAsync();
        }


        public Task<WorkOrderViewDto> CreateAsync(WorkOrderViewCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderViewDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }


        public Task<bool> UpdateAsync(WorkOrderViewUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WorkOrderViewCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetPickMtrStockByWorkOrderAsync(string workorderno)
        {
            return await _workOrderViewRepository.GetPickMtrStockByWorkOrderAsync(workorderno);
        }
    }
}
