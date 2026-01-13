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
    public class WorkOrderInProgressViewService : IWorkOrderInProgressViewService
    {
        private readonly IWorkOrderInProgressViewRepository _workOrderInProgressViewRepository;
        public WorkOrderInProgressViewService(IWorkOrderInProgressViewRepository workOrderInProgressViewRepository)
        {
            _workOrderInProgressViewRepository = workOrderInProgressViewRepository;
        }

        public async  Task<List<V_WorkOrderInProgress>> GetByOrderNoAsync(string orderno)
        {
            return await _workOrderInProgressViewRepository.GetByOrderNoAsync(orderno);
        }

        public async Task<PagedResultDto<V_WorkOrderInProgress>> GetCableTaskPageListAsync(int pageIndex, int pageSize, string? keyword = null, List<string>? workOrderNo = null, DateTime? startTime = null, int? workcenterId = null, int? workStationId = null, string? status = null)
        {
            var (result,totalCount) =  await _workOrderInProgressViewRepository.GetCableTaskPageListAsync(pageIndex, pageSize, keyword, workOrderNo, startTime, workcenterId, workStationId,status);

            return new PagedResultDto<V_WorkOrderInProgress> { Items = result, TotalCount = totalCount };
        }

        public async Task<List<V_WorkOrderInProgress>> GetListByWorkCenterGroupAsync(int factoryid, int workcentergroupid, DateTime datetimeStart, DateTime datetimeEnd)
        {
            return await _workOrderInProgressViewRepository.GetListByWorkCenterGroupAsync(factoryid, workcentergroupid, datetimeStart, datetimeEnd);
        }

        public async Task<List<V_WorkOrderInProgress>> GetListAsync(List<string> workCenters, DateTime? startTime, DateTime? endTime = null)
        {
            return await _workOrderInProgressViewRepository.GetListAsync(workCenters,startTime, endTime);
        }

        public async Task<List<V_WorkOrderInProgress>> GetListByProcessIdAsync(List<int> processIds)
        {
            return await _workOrderInProgressViewRepository.GetListByProcessIdAsync(processIds);
        }

        public async Task<List<V_WorkOrderInProgress>> GetOngoingCableTaskListByDateAsync(int factoryid, DateTime datetime)
        {
            return await _workOrderInProgressViewRepository.GetOngoingCableTaskListByDateAsync(factoryid,datetime);
        }

        public async Task<List<V_WorkOrderInProgress>> GetOverdueCableTaskListByDateAsync(int factoryid, string? keyword)
        {
            return await _workOrderInProgressViewRepository.GetOverdueCableTaskListByDateAsync(factoryid, keyword);
        }
    }
}
