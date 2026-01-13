using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderOperationTask")]

    public class WorkOrderOperationTask
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? WorkOrderId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true,Length = 50)]
        public string? WorkOrderNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? WorkOrderProcessId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        public string? Operation
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? Quantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? WorkCenterId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? StartTime
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? EndTime
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? CompletedQuantity
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
