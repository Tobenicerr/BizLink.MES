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
    public interface IWorkOrderOperationConfirmService : IGenericService<WorkOrderOperationConfirmDto, WorkOrderOperationConfirmCreateDto, WorkOrderOperationConfirmUpdateDto>
    {

        Task<WorkOrderOperationConfirmDto> GetConfirmWitemConsumeptionAsync(int confirmid);

        Task<PagedResultDto<WorkOrderOperationConfirmDto>> GetOperationConfirmPageListAsync(int pageIndex, int pageSize, List<string>? orders, string? status, List<string>? operation);

        Task<List<WorkOrderOperationConfirmDto>> GetListByProcessIdAsync(int processid);
        Task<List<WorkOrderOperationConfirmDto>> GetListByProcessIdAsync(List<int> processids);

        Task<bool> SplitOperationConfirmAsync(int confirmId, decimal splitQuantity, string confirmSeq);
    }
}
