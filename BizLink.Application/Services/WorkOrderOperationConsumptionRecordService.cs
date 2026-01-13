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
    public class WorkOrderOperationConsumptionRecordService : IWorkOrderOperationConsumptionRecordService
    {
        private readonly IWorkOrderOperationConsumptionRecordRepository _workOrderOperationConsumptionRecordrepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper


        public WorkOrderOperationConsumptionRecordService(IWorkOrderOperationConsumptionRecordRepository workOrderOperationConsumptionRecordrepository, IMapper mapper)
        {
            _workOrderOperationConsumptionRecordrepository = workOrderOperationConsumptionRecordrepository;
            _mapper = mapper;
        }

        public async Task<WorkOrderOperationConsumptionRecordDto> CreateAsync(WorkOrderOperationConsumptionRecordCreateDto createDto)
        {
            var entity = _mapper.Map<Domain.Entities.WorkOrderOperationConsumptionRecord>(createDto);
            var result = await _workOrderOperationConsumptionRecordrepository.AddAsync(entity);
            return _mapper.Map<WorkOrderOperationConsumptionRecordDto>(result);
        }

        public Task<List<int>> CreateBatchAsync(List<WorkOrderOperationConsumptionRecordCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(List<int> ids)
        {
            return await _workOrderOperationConsumptionRecordrepository.DeleteAsync(ids);
        }

        public Task<IEnumerable<WorkOrderOperationConsumptionRecordDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderOperationConsumptionRecordDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WorkOrderOperationConsumptionRecordDto>> GetListByProcessIdAsync(int processid)
        {
            var entities = await _workOrderOperationConsumptionRecordrepository.GetListByProcessIdAsync(processid);
            return entities.Select(e => _mapper.Map<WorkOrderOperationConsumptionRecordDto>(e)).ToList();
        }

        public Task<bool> UpdateAsync(WorkOrderOperationConsumptionRecordUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
