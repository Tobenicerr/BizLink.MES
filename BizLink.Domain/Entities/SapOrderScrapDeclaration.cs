using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Sap_OrderScrapDeclaration")]
    public class SapOrderScrapDeclaration
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 10)]
        public string? FactoryCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? WorkOrderNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        public string? OperationNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? WorkCenterCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length =20)]
        public string? SuperiorOrder
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? LeadingOrder
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? LeadingMaterial
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? ScrapBomItem
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? ScrapMaterialType
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? ScrapMaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 100)]
        public string? ScrapMaterialDesc
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? RequireQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true,Length = 10)]
        public string? BaseUnit
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? ComponentScrap
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18,3)")]
        public decimal? ScrapQuantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true,ColumnDataType = "nvarchar(200)")]
        public string? ScrapReason
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(200)")]
        public string? Remark
        {
            get; set;
        }


        public bool IsActive
        {
            get; set;
        } = true;

        [SugarColumn(IsNullable = true)]
        public DateTime? CreatedOn
        {
            get; set;
        } = DateTime.Now;

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
        public string? UpdatedBy
        {
            get; set;
        }
    }
}
