using BizLink.MES.Domain.Entities;
using SAP.Middleware.Connector;
using System.Data;

namespace BizLink.MES.Domain.Repositories
{
    public interface ISapRfcRepository
    {
        Task<(List<SapOrderOperation> sapOperation, List<SapOrderBom> sapBom)> GetWorkOrdersAsync(string plantcode, DateTime? dispatchdate, List<string> orders = null);

        Task<(List<SapOrderOperation> sapOperation, List<SapOrderBom> sapBom)> GetCN10WorkOrdersAsync(string plantcode, DateTime? dispatchdate, List<string> workcentercode, List<string> orders = null);

        Task<List<CableCutParam>> GetCableCutParamByMaterialsAsync(List<string> semimaterialcode);


        Task<List<MaterialTransferLog>> MaterialStockTransferToSAPAsync(List<MaterialTransferLog> input);

        Task<WorkOrderOperationConfirm?> ConfirmOrderCompletionToSAPAsync(WorkOrderOperationConfirm confirm);

        Task<List<Material>> GetSAPMaterialAsync(string factoryCode,List<string>? materialCodes,DateTime? startTime, DateTime? endTime);

        Task<List<SapRawMaterialStock>> GetRawMaterialStockFromSapAsync(List<SapRawMaterialStock> materialCodes);

        Task<List<MaterialTransferLog>> RawMaterialInventoryAdjustmentAsync(List<MaterialTransferLog> input);
    }
}
