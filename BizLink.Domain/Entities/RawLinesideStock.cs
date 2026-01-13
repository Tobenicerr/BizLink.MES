using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_RawLinesideStock")]
    //[SugarTable("Mes_RawLinesideStock", IsDisabledUpdateAll = true)]
    public class RawLinesideStock
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public int FactoryId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int MaterialId
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

        [SugarColumn(IsNullable = true)]

        public string? BaseUnit
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? BatchCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? BarCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public decimal? Quantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public decimal? LastQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? Status
        {
            get; set;
        } = "1";

        [SugarColumn(IsNullable = true,Length = 10)]
        public string? SapStatus
        {
            get; set;
        } = "1";

        [SugarColumn(IsNullable = true)]
        public int? LocationId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? LocationCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true,ColumnDataType = "nvarchar", Length = 50)]
        public string? LocationDesc
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
