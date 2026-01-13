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
    public class MaterialViewService : IMaterialViewService
    {
        private readonly IMaterialViewRepository _materialViewRepository;
        private readonly IMapper _mapper;

        public MaterialViewService(IMaterialViewRepository materialViewRepository ,IMapper mapper)
        {
            _materialViewRepository = materialViewRepository;
            _mapper = mapper;
        }

        public async Task<MaterialViewDto> GetByCodeAsync(string factorycode, string materialcode)
        {
            var entity = await _materialViewRepository.GetByCodeAsync(factorycode, materialcode);
            return _mapper.Map<MaterialViewDto>(entity);
        }

        public async Task<List<MaterialViewDto>> GetListByCodesAsync(string factorycode, List<string> materialcodes)
        {
            var entities =  await _materialViewRepository.GetListByCodesAsync(factorycode,materialcodes);
            return entities.Select(x => _mapper.Map<MaterialViewDto>(x)).ToList();
        }
    }
}
