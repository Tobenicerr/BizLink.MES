using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWmsWorkOrderPickingLogService :IGenericService<WmsWorkOrderPickingLogDto, WmsWorkOrderPickingLogCreateDto, WmsWorkOrderPickingLogUpdateDto>
    {
        Task<List<WmsWorkOrderPickingLogDto>> GetListByWorkOrdersAsync(List<string> workOrders);

        Task<List<WmsWorkOrderPickingLogDto>> GetListByWorkOrdersAsync(string workOrder);

    }
}
