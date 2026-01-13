using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    public class ActivityLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable = true)]
        public int? UserId { get; set; }
        [SugarColumn(IsNullable = true,Length = 50, ColumnDataType = "nvarchar")]
        public string? UserName { get; set; }

        [SugarColumn(IsNullable = true,Length = 50)]
        public string? LogType { get; set; } // "Action", "PageView", "Error"

        [SugarColumn(IsNullable = true, Length = 255,ColumnDataType = "nvarchar")]
        public string? LogContent { get; set; }

        [SugarColumn(IsNullable = true, Length = 255,ColumnDataType = "nvarchar")]
        public string? Details { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? Timestamp { get; set; } = DateTime.Now;
    }
}
