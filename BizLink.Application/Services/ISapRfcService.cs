using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ISapRfcService
    {

        /// <summary>
        /// 根据工单号从 SAP 获取工单详细信息
        /// </summary>
        /// <param name="workOrderNumber">工单号</param>
        /// <returns>工单数据传输对象</returns>
        Task<SapOrderDto> GetWorkOrdersAsync(string plantcode,DateTime? dispatchdate, List<string> orders = null);

        Task<SapOrderDto> GetCN10WorkOrdersAsync(string plantcode, DateTime? dispatchdate, List<string> workcentercode, List<string> orders = null);

        Task<List<CableCutParamCreateDto>> GetCableCutParamByMaterialsAsync(List<string> semimaterialcode);

        Task<List<MaterialTransferLogDto>> MaterialStockTransferToSAPAsync(List<MaterialTransferLogDto> input);

        Task<WorkOrderOperationConfirmDto> ConfirmOrderCompletionToSAPAsync(int sapconfirmid);

        Task<bool> SyncMaterialFromSAPAsync(string factoryCode, List<string>? materialCodes, DateTime? startTime, DateTime? endTime);

        Task<List<SapRawMaterialStockDto>> GetRawMaterialStockFromSapAsync(List<SapRawMaterialStockDto> materialCodes);

        Task<List<MaterialTransferLogDto>> RawMaterialInventoryAdjustmentAsync(List<MaterialTransferLogDto> input);
    }
}
