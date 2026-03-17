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
    public interface IPendingOperationTaskService : IGenericService<PendingOperationTaskDto, PendingOperationTaskCreateDto, PendingOperationTaskUpdateDto>
    {

        Task<List<PendingOperationTaskDto>> GetListByWorkCentersAsync(int factoryId, List<string> workcenters, DateTime startDate, DateTime endDate);
    }
}
