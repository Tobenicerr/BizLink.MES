using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{

    [SugarTable("Mes_WiApprovalLogs")]
    public class WiApprovalLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? DocumentId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true ,Length = 50)]
        public string? Operator
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? ActionType
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar(200)")]
        public string? Comment
        {
            get; set;
        }


        [SugarColumn(IsNullable = true)]
        public DateTime? LogTime
        {
            get; set;
        } = DateTime.Now;
    }
}
