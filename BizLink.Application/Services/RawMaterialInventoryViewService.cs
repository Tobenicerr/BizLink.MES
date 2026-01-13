using AutoMapper;
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
    public class RawMaterialInventoryViewService : IRawMaterialInventoryViewService
    {
        private readonly IRawMaterialInventoryViewRepository _rawMaterialInventoryViewRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper
        public RawMaterialInventoryViewService(IRawMaterialInventoryViewRepository rawMaterialInventoryViewRepository,IMapper mapper)
        {
            _rawMaterialInventoryViewRepository = rawMaterialInventoryViewRepository;
            _mapper = mapper;
        }

        public async Task<List<RawMaterialInventoryViewDto>> GetListByMaterialCodeAsync(string factorycode, string materialCode)
        {
            var entities = await _rawMaterialInventoryViewRepository.GetListByMaterialCodeAsync(factorycode, materialCode);
            return entities.Select(x => _mapper.Map<RawMaterialInventoryViewDto>(x)).ToList();
        }

        public async Task<List<RawMaterialInventoryViewDto>> GetListByMaterialCodeAsync(string factorycode, List<string> materialCode)
        {
            var entities = await _rawMaterialInventoryViewRepository.GetListByMaterialCodeAsync(factorycode, materialCode);
            return _mapper.Map<List<RawMaterialInventoryViewDto>>(entities);
        }
    }
}
