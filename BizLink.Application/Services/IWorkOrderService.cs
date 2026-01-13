using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderService
    {
        Task CreateBySAPAsync(string factorycode,List<WorkOrderCreateDto> workorders, List<WorkOrderProcessCreateDto> processes, List<WorkOrderBomItemCreateDto> boms, IProgress<float> progress);

        Task<List<WorkOrderDto>> GetListByDispatchDateAsync(DateTime? dispatchdate, DateTime? startdate, List<string>? ordernos, int factoryid);

        Task<WorkOrderDto> GetByIdAsync(int orderid);

        Task<List<WorkOrderDto>> GetListByBomMaterialAsync(string bommaterialcode);

        Task<int> CountByOrderNosNoLockAsync(List<string> ordernos);

        Task<WorkOrderDto> GetByOrdrNoAsync(string orderno);

        Task<List<WorkOrderDto>> GetByOrdrNoAsync(List<string> ordernos);

        Task<(List<WorkOrderDto>, List<WorkOrderProcessDto>, List<WorkOrderTaskDto>)> GetCableTaskConfirmListAsync(List<string>? orders,DateTime? starttime,int? workcenterid,int? workstationid);

        Task<List<int>> CreateBatchAsync(List<WorkOrderCreateDto> createDtos);

        Task<bool> UpdateBatchAsync(List<WorkOrderUpdateDto> updateDtos);

        Task<bool> UpdateAsync(WorkOrderUpdateDto updateDto);

        Task<List<WorkOrderDto>> GetListByDispatchDateEndAsync(int factoryid, DateTime startdate);
    }
}
