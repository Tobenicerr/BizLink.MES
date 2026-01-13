using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderStepTask")]
    public class WorkOrderStepTask
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public int? OperationTaskId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? StepId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true,Length = 50)]
        public string? StepCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? WorkCenterGroupId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? WorkCenterGroupCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? ActualWorkCenterId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? ActualWorkCenterCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? ActualWorkStationId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? ActualWorkStationCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? Quantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? CompletedQuantity
        {
            get; set;
        }

        /// <summary>
        /// NORMAL, REWORK(返工), SAMPLE(抽检), TRIAL(试制)
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 20)]
        public string? TaskCategory
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 255)]
        public string? PreStepIds
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        public string? Status
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreatedOn
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? CreatedBy
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdatedOn
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? UpdateBy
        {
            get; set;
        }
    }
}
