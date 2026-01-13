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
    public class ParameterGroupService : IParameterGroupService
    {
        private readonly IParameterGroupRepository _parameterGroupRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper

        public ParameterGroupService(IParameterGroupRepository parameterGroupRepository,IMapper mapper)
        {
            _parameterGroupRepository = parameterGroupRepository;
            _mapper = mapper;
        }
        public async Task<ParameterGroupDto> GetGroupWithItemsAsync(string groupKey)
        {
            var entity = await _parameterGroupRepository.GetGroupWithItemsAsync(groupKey);
            return _mapper.Map<ParameterGroupDto>(entity);
        }
    }
}
