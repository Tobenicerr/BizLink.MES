using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkOrderOperationConfirmRepository : IGenericRepository<WorkOrderOperationConfirm>
    {

        Task<WorkOrderOperationConfirm> GetConfirmWitemConsumeptionAsync(int confirmid);

        Task<(List<WorkOrderOperationConfirm>, int TotalCount)> GetOperationConfirmPageListAsync(int pageIndex, int pageSize, List<string>? orders, string? status, List<string>? operation);

        Task<List<WorkOrderOperationConfirm>> GetListByProcessIdAsync(int processid);

        Task<List<WorkOrderOperationConfirm>> GetListByProcessIdAsync(List<int> processids);
    }
}
