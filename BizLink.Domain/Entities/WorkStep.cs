using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkStep")]
    public class WorkStep
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        [SugarColumn(IsNullable = true,Length = 50)]
        public string? StepCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(100)")]
        public string? StepName
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length =20)]
        public string? StepType
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        } = "1";

        public bool? IsDelete
        {
            get; set;
        } = false;

        [SugarColumn(IsNullable = true,ColumnName = "CreatedAt")]
        public DateTime? CreateOn
        {
            get; set;
        } = DateTime.Now; // 创建时间

        [SugarColumn(IsNullable = true)]
        public string? CreateBy
        {
            get; set;
        } // 创建人

        [SugarColumn(IsNullable = true,ColumnName = "UpdatedAt")]
        public DateTime? UpdateOn
        {
            get; set;
        } // 更新时间

        [SugarColumn(IsNullable = true)]
        public string? UpdateBy
        {
            get; set;
        } // 更新人
    }
}
