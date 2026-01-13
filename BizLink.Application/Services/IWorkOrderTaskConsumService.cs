using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderTaskConsumService
    {
        Task BatchCreateAsync(List<WorkOrderTaskConsumCreateDto> input);
        Task<List<WorkOrderTaskConsumDto>> GetListByConfirmIdAsync(int confirmid);

        Task<List<WorkOrderTaskConsumDto>> GetCableListByProcessIdsAsync(List<int> processids);
    }
}
