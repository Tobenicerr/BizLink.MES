using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_PendingMaterialTaskCutting")]

    public class V_PendingMaterialTaskCutting
    {
        public int WorkOrderId { get; set; }
        public string WorkOrderNo { get; set; }
        public string ProfitCenter { get; set; }
        public DateTime ScheduledStartDate { get; set; }
        public decimal Quantity { get; set; }
        public int WorkOrderProcessId { get; set; }
        public string WorkCenter { get; set; }
        public string Operation { get; set; }
        public int BomId { get; set; }
        public string MaterialCode { get; set; }
        public string BomItem { get; set; }
        public string MaterialDesc { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string Unit { get; set; }
        public int MaterialTaskId { get; set; }
        public int StepTaskId { get; set; }
        public decimal? TargetQuantity { get; set; }
        public decimal? TargetValue { get; set; }
        public string? TargetUnit { get; set; }

        public decimal? CompletedQuantity { get; set; }


        [SugarColumn(ColumnName = "ExtAttributes", IsJson = true)]
        public CableCutParam? CuttingParam { get; set; }
        public string? Status { get; set; }

    }
}
