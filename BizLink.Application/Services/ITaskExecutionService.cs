using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Response;
using BizLink.MES.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface ITaskExecutionService
    {
        Task VerifyScannedMaterialMatchAsync(int factoryId, string barcode, List<int> taskIds);

        Task ValidateMaterialReadinessAsync(List<int> taskIds, int stationId);

        Task<bool> VerifyCuttingStepTaskFirstConfirmAsync(int materialTaskId);

        Task ClaimAndStartTasksAsync(List<int> taskIds, int stationId, string employeeId);

        Task ReleaseTaskAsync(int taskId, string employeeId);

        Task ReportMaterialTaskProductionAsync(int taskId, decimal outputQty, decimal scrapQty,
         int stationId, string stationCode, string employeeId, string? remark = null, string? movementReason = null, DateTime? startTime = null, DateTime? endTime = null);
        Task ReportStepProductionAsync(int stepId, decimal outputQty, int stationId, string stationCode, string employeeId, string? remark = null, string movementType = "261", string movementReason = null, DateTime? startTime = null, DateTime? endTime = null);

        Task ReportStepProductionAsync(int stepId, decimal outputQty, string employeeId, string? remark = null, DateTime? startTime = null, DateTime? endTime = null);

        Task ReportStepProductionAsync(List<int> stepIds, decimal outputQty, string employeeId, string? remark = null, DateTime? startTime = null, DateTime? endTime = null);
        
        Task<KittingResponse> GetKittingListAsync(string orderNoRaw);

        Task ConfirmKittingAsync(int workorderId, int? processId = null, string employeeId = "PDA");

        Task ApproveWmsPickTaskAsync(List<string> wmsPickNos);

        Task LoadMaterialAsync(int factoryId, string barcode, int stationId, string employeeId, SupplyMode supplyMode = SupplyMode.Local);

        Task UnloadMaterialAsync(int loadingId, string employeeId, string reason = "Manual-Unload");

        Task SuspendTaskAsync(int taskId, string employeeId);

        Task<string> DetermineMaterialFlagToProcessLabelAsync(int orderId);

        Task ReEntryPushSapConfirmAsync(int sapConfirmId);

        Task BatchReEntryPushSapConfirmAsync(List<int> sapConfirmIds);

        Task<FinishedGoodsReceiptDto> FinishedGoodsReceiptToSapAsync(FinishedGoodsReceiptDto dto);

    }
}
