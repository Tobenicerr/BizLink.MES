using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderTaskMaterialAddService
    {
        Task<WorkOrderTaskMaterialAddDto> GetByIdAsync(int id);

        Task<WorkOrderTaskMaterialAddDto> GetLastByWorkStationAsync(int workstationid);

        Task<WorkOrderTaskMaterialAddDto> GetByBarcodeAsync(string barcode);

        Task<List<WorkOrderTaskMaterialAddDto>> GetByTaskIdAsync(int taskid);
        Task<WorkOrderTaskMaterialAddDto> CreateAsync(WorkOrderTaskMaterialAddCreateDto stock);
        Task UpdateAsync(WorkOrderTaskMaterialAddUpdateDto stock);
    }
}
