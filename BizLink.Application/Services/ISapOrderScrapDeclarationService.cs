using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ISapOrderScrapDeclarationService : IGenericService<SapOrderScrapDeclarationDto, SapOrderScrapDeclarationCreateDto, SapOrderScrapDeclarationUpdateDto>
    {
        Task<List<SapOrderScrapDeclarationDto>> GetListByOperationAsync(string workOrderNo, string operationNo);

        Task<List<SapOrderScrapDeclarationDto>> GetListByOrderNosAsync(List<string> workOrderNos);

        Task<bool> UpdateBatchAsync(List<SapOrderScrapDeclarationUpdateDto> updateDtos);

        Task<List<SapOrderScrapDeclarationDto>> GetListByWorkCenterAsync(string factorycode, List<string>? workcentercodes, DateTime? startdate, DateTime? enddate, List<string>? workorders);

        Task<PagedResultDto<SapOrderScrapDeclarationDto>> GetPageListAsync(int pageIndex, int pageSize, string factoryCode, string? keyword, DateTime? createdDate);
    }
}
