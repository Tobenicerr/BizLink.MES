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
    public class WorkOrderTaskExecuteLogService : IWorkOrderTaskExecuteLogService
    {
        private readonly IWorkOrderTaskExecuteLogRepository _workOrderTaskExecuteLogRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkOrderTaskExecuteLogService(IWorkOrderTaskExecuteLogRepository workOrderTaskExecuteLogRepository, IMapper mapper)
        {
            _workOrderTaskExecuteLogRepository = workOrderTaskExecuteLogRepository;
            _mapper = mapper;
        }

        public async Task<WorkOrderTaskExecuteLogDto> CreateAsync(WorkOrderTaskExecuteLogCreateDto createDto)
        {
            var entity = await _workOrderTaskExecuteLogRepository.AddAsync(_mapper.Map<WorkOrderTaskExecuteLog>(createDto));
            return _mapper.Map<WorkOrderTaskExecuteLogDto>(entity);
        }

        public async Task<List<int>> CreateBatchAsync(List<WorkOrderTaskExecuteLogCreateDto> createDto)
        {
            return await _workOrderTaskExecuteLogRepository.AddBulkAsync(_mapper.Map<List<WorkOrderTaskExecuteLog>>(createDto));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _workOrderTaskExecuteLogRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<WorkOrderTaskExecuteLogDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderTaskExecuteLogDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(WorkOrderTaskExecuteLogUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
