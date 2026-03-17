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
    public class PendingOperationTaskService : IPendingOperationTaskService
    {
        private readonly IPendingOperationTaskRepository _repos;
        private readonly IMapper _mapper;

        public PendingOperationTaskService(IPendingOperationTaskRepository repos, IMapper mapper) 
        {
            _repos = repos;
            _mapper = mapper;
        }
        public Task<PendingOperationTaskDto> CreateAsync(PendingOperationTaskCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<PendingOperationTaskCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PendingOperationTaskDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PendingOperationTaskDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PendingOperationTaskDto>> GetListByWorkCentersAsync(int factoryId, List<string> workcenters, DateTime startDate, DateTime endDate)
        {
            var entities = await _repos.GetListByWorkCentersAsync(factoryId, workcenters, startDate, endDate);
            return _mapper.Map<List<PendingOperationTaskDto>>(entities);
        }

        public Task<bool> UpdateAsync(PendingOperationTaskUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }

    }
}
