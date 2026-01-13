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
    internal class WorkOrderComponentsTransferService : IWorkOrderComponentsTransferService
    {

        private readonly IWorkOrderComponentsTransferRepository _workOrderComponentsTransferRepository;

        private readonly IMapper _mapper; // 2. 声明 IMapper

        public WorkOrderComponentsTransferService(IWorkOrderComponentsTransferRepository workOrderComponentsTransferRepository, IMapper mapper) // 3. 通过构造函数注入 IMapper
        {
            _workOrderComponentsTransferRepository = workOrderComponentsTransferRepository;
            _mapper = mapper; // 4. 初始化 IMapper
        }
        public async Task<PagedResultDto<WorkOrderComponentsTransferDto>> GetPageListAsync(int pageIndex, int pageSize, int factoryId, string? keyword, List<string>? workorders, DateTime? startdate, DateTime? dispatchdate)
        {
            var (result,totalcount) =  await _workOrderComponentsTransferRepository.GetPageListAsync(pageIndex, pageSize, factoryId, keyword, workorders, startdate, dispatchdate);
            return new PagedResultDto<WorkOrderComponentsTransferDto>
            {
                Items = _mapper.Map<List<WorkOrderComponentsTransferDto>>(result), 
                TotalCount = totalcount
            };
        }
    }
}
