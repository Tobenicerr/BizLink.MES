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
    public class WorkOrderTreeOptimizedService : IWorkOrderTreeOptimizedService
    {

        private readonly IWorkOrderTreeOptimizedRepository _workOrderTreeOptimizedRepository;
        private readonly IMapper _mapper;
        
        public WorkOrderTreeOptimizedService(IWorkOrderTreeOptimizedRepository workOrderTreeOptimizedRepository, IMapper mapper)
        {
            _workOrderTreeOptimizedRepository = workOrderTreeOptimizedRepository;
            _mapper = mapper;
        }
        public Task<WorkOrderTreeOptimizedDto> CreateAsync(WorkOrderTreeOptimizedCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WorkOrderTreeOptimizedCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderTreeOptimizedDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WorkOrderTreeOptimizedDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(WorkOrderTreeOptimizedUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
