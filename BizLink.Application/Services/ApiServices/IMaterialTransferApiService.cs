using BizLink.MES.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IMaterialTransferApiService
    {
        /// <summary>
        /// 重新处理并发送物料转移日志到 SAP。
        /// </summary>
        /// <param name="transferIds">要重传的日志 ID 列表。</param>
        /// <returns>一个包含操作结果的对象。</returns>
        Task<(bool, string)> RetransferLogsToSAPAsync(List<int> transferIds);

        Task<(bool, string)> TransferMaterialToSAPAsync(TransferSapRequest request);

        Task<(bool, string)> AdjustInventoryInSAPAsync(TransferSapRequest request);
    }
}
