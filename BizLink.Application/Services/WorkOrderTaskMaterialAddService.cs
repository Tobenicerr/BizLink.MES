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
    public class WorkOrderTaskMaterialAddService : IWorkOrderTaskMaterialAddService
    {

        private readonly IWorkOrderTaskMaterialAddRepository _workOrderTaskMaterialAddRepository;

        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkOrderTaskMaterialAddService(IWorkOrderTaskMaterialAddRepository workOrderTaskMaterialAddRepository, IMapper mapper)
        {
            _workOrderTaskMaterialAddRepository = workOrderTaskMaterialAddRepository;
            _mapper = mapper;
        }
        public async Task<WorkOrderTaskMaterialAddDto> CreateAsync(WorkOrderTaskMaterialAddCreateDto input)
        {
            var entity = _mapper.Map<WorkOrderTaskMaterialAdd>(input);
            var result = await _workOrderTaskMaterialAddRepository.AddAsync(entity);
            return _mapper.Map<WorkOrderTaskMaterialAddDto>(result);
        }

        public async Task<WorkOrderTaskMaterialAddDto> GetByBarcodeAsync(string barcode)
        {
            var entity = await _workOrderTaskMaterialAddRepository.GetByBarcodeAsync(barcode);
            return _mapper.Map<WorkOrderTaskMaterialAddDto>(entity);
        }

        public async Task<WorkOrderTaskMaterialAddDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderTaskMaterialAddRepository.GetByIdAsync(id);
            return _mapper.Map<WorkOrderTaskMaterialAddDto>(entity);
        }

        public async Task<List<WorkOrderTaskMaterialAddDto>> GetByTaskIdAsync(int taskid)
        {
            var entity = await _workOrderTaskMaterialAddRepository.GetByTaskIdAsync(taskid);
            return entity.Select(x => _mapper.Map<WorkOrderTaskMaterialAddDto>(x)).ToList();
        }

        public async Task<WorkOrderTaskMaterialAddDto> GetLastByWorkStationAsync(int workstationid)
        {
            var entity = await _workOrderTaskMaterialAddRepository.GetLastByWorkStationAsync(workstationid);
            return _mapper.Map<WorkOrderTaskMaterialAddDto>(entity);

        }

        public async Task UpdateAsync(WorkOrderTaskMaterialAddUpdateDto input)
        {
            var entity = await _workOrderTaskMaterialAddRepository.GetByIdAsync(input.Id);
            _mapper.Map(input, entity);
            await _workOrderTaskMaterialAddRepository.UpdateAsync(entity);
        }
    }
}
