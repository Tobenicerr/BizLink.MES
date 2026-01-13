using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_WorkOrderBom")]
    public class V_WorkOrderBom
    {
        [SugarColumn(ColumnName = "OrderId")]
        public int WorkOrderId
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? OrderNumber
        {
            get; set;
        }


        [SugarColumn(IsNullable = true)]

        public string? BomItem
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? Operation
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? MaterialDesc
        {
            get; set;
        }
        [SugarColumn(IsNullable = true,ColumnName = "ReqQty")]

        public decimal? RequiredQuantity
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? BaseUnit
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public int ConsumeType
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public string? ConsumeTypeDesc
        {
            get; set;
        }

    }
}
