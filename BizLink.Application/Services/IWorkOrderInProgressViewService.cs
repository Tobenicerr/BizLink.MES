using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Data.OscarClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderInProgressViewService
    {
        Task<List<V_WorkOrderInProgress>> GetListAsync(List<string> workCenters, DateTime? startTime, DateTime? endTime = null);

        Task<List<V_WorkOrderInProgress>> GetByOrderNoAsync(string orderno);

        Task<List<V_WorkOrderInProgress>> GetListByProcessIdAsync(List<int> processIds);

        Task<List<V_WorkOrderInProgress>> GetOngoingCableTaskListByDateAsync(int factoryid,DateTime datetime);

        Task<PagedResultDto<V_WorkOrderInProgress>> GetCableTaskPageListAsync(int pageIndex,int pageSize,string? keyword = null,List<string>? workOrderNo = null,DateTime? startTime = null,int? workcenterId = null,int? workStationId = null, string? status = null);

        Task<List<V_WorkOrderInProgress>> GetOverdueCableTaskListByDateAsync(int factoryid, string? keyword);

        Task<List<V_WorkOrderInProgress>> GetListByWorkCenterGroupAsync(int factoryid, int workcentergroupid, DateTime datetimeStart, DateTime datetimeEnd);

    }
}
