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
    public class PendingMaterialTaskCuttingService : IPendingMaterialTaskCuttingService
    {
        private readonly IPendingMaterialTaskCuttingRepository _repo;
        private readonly IMapper _mapper;

        public PendingMaterialTaskCuttingService(IPendingMaterialTaskCuttingRepository repo, IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }
        public Task<PendingMaterialTaskCuttingDto> CreateAsync(PendingMaterialTaskCuttingCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<PendingMaterialTaskCuttingCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PendingMaterialTaskCuttingDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PendingMaterialTaskCuttingDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<PendingMaterialTaskCuttingDto>> GetListByExecutionAsync(DateTime startdate, string workcenter)
        {
            var entities = await _repo.GetListByExecutionAsync(startdate, workcenter);
            return _mapper.Map<List<PendingMaterialTaskCuttingDto>>(entities);
        }

        public Task<bool> UpdateAsync(PendingMaterialTaskCuttingUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
