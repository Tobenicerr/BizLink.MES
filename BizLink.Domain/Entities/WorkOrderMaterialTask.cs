using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderMaterialTask")]
    public class WorkOrderMaterialTask
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public int? StepTaskId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? SourceBomId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 200)]
        public string? MaterialDesc
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? TargetQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? TargetValue
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        public string? TargetUnit
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = " nvarchar(max)")]
        public string? ExtAttributes
        {
            get; set;
        }

        /// <summary>
        /// 任务子类型
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 20)]
        public string? TaskSubType
        {
            get; set;
        }

        /// <summary>
        /// 参考来源ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? RefSourceId
        {
            get; set;
        }

        /// <summary>
        /// 参考原因
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? RefReasonCode
        {
            get; set;
        }

        /// <summary>
        /// 任务优先级
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? Priority
        {
            get; set;
        } = 0;

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
