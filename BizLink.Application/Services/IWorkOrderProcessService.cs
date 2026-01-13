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
    public  interface IWorkOrderProcessService : IGenericService<WorkOrderProcessDto, WorkOrderProcessCreateDto, WorkOrderProcessUpdateDto>
    {
        Task<WorkOrderProcessDto> GetByOrderNo(string orderno, string operation);
        Task<List<WorkOrderProcessDto>> GetListByOrderIdAync(int orderid);

        Task<List<WorkOrderProcessDto>> GetListByOrderIdsAync(List<int> orderids);

        Task<List<WorkOrderProcessDto>> GetListByOrderNos(List<string> ordernos);

        Task<List<WorkOrderProcessDto>> GetListByDispatchDateAsync(int factoryid,DateTime startdate, string? operation = null,string? status = null, string? nostatus = null);

        Task<List<string>> GetAllOperationAsync();

        Task<List<WorkOrderProcessDto>> GetByIdAsync(List<int> id);


        Task<bool> UpdateBatchAsync(List<WorkOrderProcessUpdateDto> updateDto);

    }
}
