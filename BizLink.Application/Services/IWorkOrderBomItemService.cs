using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkOrderBomItemService : IGenericService<WorkOrderBomItemDto, WorkOrderBomItemCreateDto, WorkOrderBomItemUpdateDto>
    {

        Task<List<WorkOrderBomItemDto>> GetListByOrderNoAync(string orderno);

        Task<List<WorkOrderBomItemDto>> GetListByOrderNoAync(List<string> orderno);


        Task<List<WorkOrderBomItemDto>> GetListByOrderIdsAync(List<int> orderids);

        Task<List<WorkOrderBomItemDto>> GetListByOrderIdAync(int orderid);

        Task<bool> UpdateWmsStatusAsync(List<WorkOrderBomItemUpdateDto> updateDtos);

        Task<bool> CreateBatchAsync(List<WorkOrderBomItemCreateDto> createDtos);

        Task<List<WorkOrderBomItemDto>> GetListByProcessIdsAsync(List<int> processids);

        Task<List<WorkOrderBomItemDto>> GetOngoingCableListByProcessIdsAsync(List<int> processids);

        Task<List<WorkOrderBomItemDto>> GetOngoingCableListByDateAsync(int factoryid,DateTime startdate);

        Task<bool> UpdateBatchAsync(List<WorkOrderBomItemUpdateDto> updateDtos);

    }
}
