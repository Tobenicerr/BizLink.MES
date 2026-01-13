using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderTaskConfirmService : IGenericService<WorkOrderTaskConfirmDto,WorkOrderTaskConfirmCreateDto, WorkOrderTaskConfirmUpdateDto>
    {
        Task CreateByCableAsync(WorkOrderTaskConfirmCreateDto confirm, List<WorkOrderTaskConsumCreateDto> Consums, WorkOrderTaskUpdateDto task, int factoryId);

        Task<List<int>> CreateByAssmAsync(List<WorkOrderTaskConfirmCreateDto> confirms, List<WorkOrderTaskConsumCreateDto> Consums, List<WorkOrderTaskUpdateDto> tasks);


        Task<List<WorkOrderTaskConfirmDto>> GetListByTaskIdAsync(int taskid);

        Task<List<WorkOrderTaskConfirmDto>> GetListByOrderNoAsync(string orderno);

        Task<WorkOrderTaskConfirmDto> GetByStationIdAsync(int prossid, int stationid, List<int> confirmids);

        Task<List<WorkOrderTaskConfirmDto>> GetByEndStationAsync(List<int> prossid, int stationid);

    }
}
