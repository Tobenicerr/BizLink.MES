using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderTaskExecuteConsump")]

    public class WorkOrderTaskExecuteConsump
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int ExeLogId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? BatchCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? BarCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? ConsumedQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        public string? BaseUnit
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? MovementType
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length =100)]
        public string? MovementReason
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
