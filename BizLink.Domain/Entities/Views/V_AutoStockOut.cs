using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("V_Biz_WorkOrderAutoPickOut")]
    public class V_AutoStockOut
    {
        [SugarColumn(IsNullable = true, Length = 20)]
        public string? WorkOrderNo
        {
            get; set;
        }


        [SugarColumn(IsNullable = true,Length = 50)]
        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? Quantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? PickingQuantity
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? LastQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? UseageQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? Status
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public DateTime? CreatedAt
        {
            get; set;
        } = DateTime.Now;
        [SugarColumn(IsNullable = true)]

        public string? CreatedBy
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public DateTime? UpdatedAt
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? UpdateBy
        {
            get; set;
        }



    }
}
