using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Data.OscarClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    internal class AssemblyOrderProgressService : IAssemblyOrderProgressService
    {
        private readonly IAssemblyOrderProgressRepository _assemblyOrderProgressRepository;
        private readonly IMapper _mapper;

        public AssemblyOrderProgressService(IAssemblyOrderProgressRepository assemblyOrderProgressRepository, IMapper mapper)
        {
            _assemblyOrderProgressRepository = assemblyOrderProgressRepository;
            _mapper = mapper;
        }
        public Task<AssemblyOrderProgressDto> CreateAsync(AssemblyOrderProgressCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<AssemblyOrderProgressCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AssemblyOrderProgressDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AssemblyOrderProgressDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResultDto<AssemblyOrderProgressDto>> GetPageListAsync(int pageIndex, int pageSize, string factoryCode, List<string>? orderNumber, List<string>? workCenter, DateTime? dispatchdateStart, DateTime? dispatchdateEnd, DateTime? confirmDateStart, DateTime? confirmDateEnd)
        {
            var (entities, totalCount) = await _assemblyOrderProgressRepository.GetPageListAsync(pageIndex, pageSize, factoryCode, orderNumber, workCenter, dispatchdateStart, dispatchdateEnd, confirmDateStart, confirmDateEnd);
            return new PagedResultDto<AssemblyOrderProgressDto> { Items = _mapper.Map<List<AssemblyOrderProgressDto>>(entities), TotalCount = totalCount };
        }

        public Task<bool> UpdateAsync(AssemblyOrderProgressUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
