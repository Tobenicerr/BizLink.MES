using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.WinForms.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderKittingExecuteService: IGenericService<WorkOrderKittingExecuteDto, WorkOrderKittingExecuteCreateDto, WorkOrderKittingExecuteUpdateDto>
    {

        Task<PagedResultDto<WorkOrderKittingExecuteDto>> GetPageListAsync(int pageIndex, int pageSize, int factoryId, DateTime? startdateStart, DateTime? startdateEnd, List<string> workOrders, List<string> groupCodes);

        Task<List<WorkOrderKittingExecuteDto>> GetListByGroupCodeAsync(string groupCode);
    }
}
