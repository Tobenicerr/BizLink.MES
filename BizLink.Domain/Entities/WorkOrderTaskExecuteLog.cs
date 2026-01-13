using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderTaskExecuteLog")]

    public class WorkOrderTaskExecuteLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }
        [SugarColumn(IsNullable = true,Length = 10)]
        public string? TaskLevel
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? TaskId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? BatchCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? CompletedQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? WorkStationId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? WorkStationCode
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

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? EmployerCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(255)")]
        public string? Remark
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
