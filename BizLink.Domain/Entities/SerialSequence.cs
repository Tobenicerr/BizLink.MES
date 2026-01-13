using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Sys_SerialSequence", IsDisabledUpdateAll = true)]

    public class SerialSequence
    {
        /// <summary>
        /// 序列的唯一键 (例如, "WorkOrder-20250829")。
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, Length = 100)]
        public string SequenceKey { get; set; } = string.Empty;

        /// <summary>
        /// 该序列最后使用的值。
        /// </summary>
        public int LastValue
        {
            get; set;
        }

        /// <summary>
        /// 最后更新时间。
        /// </summary>
        public DateTime UpdatedAt
        {
            get; set;
        }
    }
}
