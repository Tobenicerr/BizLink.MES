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
    public class WorkOrderTaskConsumService : IWorkOrderTaskConsumService
    {

        private readonly IWorkOrderTaskConsumRepository _workOrderTaskConsumRepository;

        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkOrderTaskConsumService(IWorkOrderTaskConsumRepository workOrderTaskConsumRepository, IMapper mapper)
        {
            _workOrderTaskConsumRepository = workOrderTaskConsumRepository;
            _mapper = mapper;
        }
        public async Task BatchCreateAsync(List<WorkOrderTaskConsumCreateDto> input)
        {
            var entity = input.Select(x => _mapper.Map<WorkOrderTaskConsum>(x)).ToList();

            await _workOrderTaskConsumRepository.BatchAddAsync(entity);
        }

        public async Task<List<WorkOrderTaskConsumDto>> GetCableListByProcessIdsAsync(List<int> processids)
        {
            var entities = await _workOrderTaskConsumRepository.GetCableListByProcessIdsAsync(processids);
            return _mapper.Map<List<WorkOrderTaskConsumDto>>(entities);
        }

        public async Task<List<WorkOrderTaskConsumDto>> GetListByConfirmIdAsync(int confirmid)
        {
            var entities = await _workOrderTaskConsumRepository.GetListByConfirmIdAsync(confirmid);   
            return entities.Select(x => _mapper.Map<WorkOrderTaskConsumDto>(x)).ToList();
        }
    }
}
