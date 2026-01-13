using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderComponentsTransferService
    {
        Task<PagedResultDto<WorkOrderComponentsTransferDto>> GetPageListAsync(int pageIndex,int pageSize,int factoryId,string? keyword,List<string>? workorders,DateTime? startdate,DateTime?dispatchdate);
    }
}
