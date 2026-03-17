using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    /// <summary>
    /// 任务生成原因/来源类型 (RefReasonCode)
    /// </summary>
    public static class TaskReasonCodes
    {
        // --- 1. 标准计划 ---
        public const string BomRequirement = "BOM_REQ";
        public const string CoProduct = "CO_PRODUCT";

        // --- 2. 质量异常 ---
        public const string ScrapReplenishment = "SCRAP_REPL";
        public const string Rework = "REWORK";
        public const string Repair = "REPAIR";
        public const string YieldLoss = "YIELD_LOSS";

        // --- 3. 工程验证 ---
        public const string Sampling = "SAMPLING";
        public const string EcoAddition = "ECO_ADD";
        public const string NpiTrial = "NPI_TRIAL";

        // --- 4. 物料变动 ---
        public const string MaterialSubstitution = "MAT_SUB";
        public const string JitPull = "JIT_PULL";

        // --- 5. 调度拆分 ---
        public const string BatchSplit = "BATCH_SPLIT";
        public const string ManualAdd = "MANUAL_ADD";

    }
}
