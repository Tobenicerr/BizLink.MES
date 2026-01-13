using BizLink.MES.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderOperationConsumptionRecord", IsDisabledUpdateAll = true)]
    public class WorkOrderOperationConsumptionRecord
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }
        public int WorkOrderId
        {
            get; set;
        }

        public int WorkOrderProcessId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? ReservationItem
        {
            get; set;
        }


        [SugarColumn(IsNullable = true, Length = 50)]
        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? BatchCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string ? BarCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18, 4)")]
        public decimal? Quantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        public string? BaseUnit
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public ConsumptionType ConsumptionType
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(200)")]
        public string? ConsumptionRemark
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime CreatedAt
        {
            get; set;
        } = DateTime.Now;
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? CreatedBy
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdatedAt
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? UpdatedBy
        {
            get; set;
        }
    }
}
