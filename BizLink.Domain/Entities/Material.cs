using BizLink.MES.Domain.Attributes;
using Dm.util;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("MDM_Material")]
    public class Material
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }


        [SugarColumn(IsNullable = true,ColumnName = "ItemNo",Length = 50)]
        [SapFieldName("MATNR")]
        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnName = "ItemName", Length = 100,ColumnDataType = "nvarchar(100)")]
        [SapFieldName("MAKTX_EN")]
        public string? MaterialDescription
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnName = "BaseUnit", Length = 20)]
        [SapFieldName("MEINS")]
        public string? BaseUnit
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnName = "PlantCode", Length = 10)]
        [SapFieldName("WERKS")]
        public string? FactoryCode
        {
            get; set;
        }

        [SugarColumn(IsIgnore = true)]
        [SapFieldName("MTART")]
        public string? MaterialType
        {
            get; set;
        }

        [SugarColumn(IsIgnore = true)]
        [SapFieldName("MATKL")]
        public string? MaterialGroup
        {
            get; set;
        }

        [SugarColumn(IsIgnore = true)]
        [SapFieldName("XCHPF")]
        public string? BatchManagementRequired
        {
            get; set;
        }

        [SugarColumn(IsIgnore = true)]
        [SapFieldName("BESKZ")]
        public string? ProcurementType
        {
            get; set;
        }

        [SugarColumn(IsIgnore = true)]
        [SapFieldName("SOBSL")]
        public string? SpecialProcurement
        {
            get; set;
        }


        [SugarColumn(IsNullable = true, ColumnName = "ConsumeType")]
        public int? ConsumeType
        {
            get; set;
        }

        [SugarColumn(IsIgnore = true)]
        [SapFieldName("ISCABLE")]
        public string? IsCableMaterial
        {
            get; set;
        }

        [SugarColumn(IsIgnore = true)]
        [SapFieldName("VBRMAT")]
        public string? IsConsumption
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnName = "IsDelete")]

        public bool IsDelete { get; set; } = false;

        [SugarColumn(IsNullable = true, ColumnName = "BActive")]

        public bool IsActive { get; set; } = true;

        [SugarColumn(IsNullable = true, ColumnName = "BProduce")]

        public bool IsProduce { get; set; } = true;

        [SugarColumn(IsNullable = true)]
        public DateTime? ExpiredDate
        {
            get; set;
        } = DateTime.Now.AddYears(100).Date;
        [SugarColumn(IsNullable = true, ColumnName = "InDate")]
        public DateTime? CreateAt 
        { get; set; } = DateTime.Now; // 创建时间

        [SugarColumn(IsNullable = true, ColumnName = "InUser",Length = 50)]
        public string? CreateBy
        {
            get; set;
        } // 创建人

        [SugarColumn(IsNullable = true,ColumnName = "LastEditDate")]

        public DateTime? UpdatedAt
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnName = "LastEditUser", Length = 50)]

        public string? UpdateBy
        {
            get; set;
        }

    }
}
