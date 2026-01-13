using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WiDocuments")]

    public class WiDocument
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? FactoryId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? FactoryCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? MaterialCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 100)]
        public string? MaterialDesc
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(100)")]
        public string? DocVersion
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(100)")]
        public string? DocumentName
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(max)")]
        public string? DocumentPath
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 10)]
        public string? Status
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(200)")]
        public string? Remark
        {
            get; set;
        }

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
        public DateTime? ApprovedOn
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? ApprovedBy
        {
            get; set;
        }
    }
}
