using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Sys_SerialRule", IsDisabledUpdateAll = true)]
    public class SerialRule
    {
        // <summary>
        /// 规则的唯一名称 (例如, "WorkOrder", "BatchNumber")。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, Length = 100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 序列号的静态前缀 (例如, "WO", "B")。
        /// </summary>
        [SugarColumn(Length = 50)]
        public string Prefix { get; set; } = string.Empty;

        /// <summary>
        /// 序列号中日期部分的格式 (例如, "yyyyMMdd", "yyMM")。
        /// 如果为 null 或空，则不包含日期部分。
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? DateFormat
        {
            get; set;
        }

        /// <summary>
        /// 流水号部分的长度 (例如, 5 代表 00001-99999 的序列)。
        /// </summary>
        public int SequenceLength { get; set; } = 5;

        /// <summary>
        /// 对此规则的描述。
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 50)]
        public string? Description
        {
            get; set;
        }
    }
}
