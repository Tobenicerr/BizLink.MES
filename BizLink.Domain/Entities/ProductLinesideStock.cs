using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_ProductLinesideStock")]
    //[SugarTable("Mes_ProductLinesideStock", IsDisabledUpdateAll = true)]
    public class ProductLinesideStock
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

        [SugarColumn(IsNullable = true, Length = 20)]

        public string? BomItem
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public int? WorkOrderProcessId
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 20)]

        public string? Operation
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public int? WorkCenterId
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 10)]

        public string? WorkCenterCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]

        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]

        public string? MaterialDesc
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
        [SugarColumn(IsNullable = true)]

        public decimal? Quantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]

        public string? BaseUnit
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        public string? Status
        {
            get; set;
        } = "1";

        [SugarColumn(IsNullable = true,ColumnDataType = "nvarchar",Length = 200)]

        public string? Remark
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public int? SourceId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public DateTime? CreatedAt
        {
            get; set;
        } = DateTime.Now;
        [SugarColumn(IsNullable = true, Length = 20)]

        public string? CreatedBy
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public DateTime? UpdatedAt
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 20)]

        public string? UpdateBy
        {
            get; set;
        }

    }
}
