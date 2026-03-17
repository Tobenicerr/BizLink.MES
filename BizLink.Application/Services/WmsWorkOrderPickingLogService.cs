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
    public class WmsWorkOrderPickingLogService : IWmsWorkOrderPickingLogService
    {
        private readonly IWmsWorkOrderPickingLogRepository _repo;
        private readonly IMapper _mapper;
        public WmsWorkOrderPickingLogService(IWmsWorkOrderPickingLogRepository repo, IMapper mapper) 
        {
            _repo = repo;
            _mapper = mapper;
        }
        public Task<WmsWorkOrderPickingLogDto> CreateAsync(WmsWorkOrderPickingLogCreateDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> CreateBatchAsync(List<WmsWorkOrderPickingLogCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WmsWorkOrderPickingLogDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<WmsWorkOrderPickingLogDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<WmsWorkOrderPickingLogDto>> GetListByWorkOrdersAsync(List<string> workOrders)
        {
            var entities = await _repo.GetListByWorkOrderAsync(workOrders);
            return _mapper.Map<List<WmsWorkOrderPickingLogDto>>(entities);
        }

        public async Task<List<WmsWorkOrderPickingLogDto>> GetListByWorkOrdersAsync(string workOrder)
        {
            var entities = await _repo.GetListByWorkOrderAsync(workOrder);
            return _mapper.Map<List<WmsWorkOrderPickingLogDto>>(entities);
        }

        public Task<bool> UpdateAsync(WmsWorkOrderPickingLogUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
