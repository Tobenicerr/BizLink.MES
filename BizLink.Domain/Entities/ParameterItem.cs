using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    /// <summary>
    /// 参数配置明细表 (键值项)
    /// </summary>
    [SugarTable("Sys_ParameterItem", IsDisabledUpdateAll = true)]
    public class ParameterItem
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        /// <summary>
        /// 所属分组ID
        /// </summary>
        public int GroupId
        {
            get; set;
        }

        /// <summary>
        /// 参数名称 (用于UI显示)
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100)]
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 参数键 (用于程序内部调用，同一分组下应唯一)
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Key
        {
            get; set;
        }

        /// <summary>
        /// 参数值
        /// </summary>
        [SugarColumn(Length = -1,ColumnDataType = "nvarchar")] // 对应NVARCHAR(MAX)
        public string? Value
        {
            get; set;
        }

        /// <summary>
        /// 参数类型 (例如: string, int, bool, json)
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string? Type
        {
            get; set;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 排序号 (同组内排序)
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 500, ColumnDataType = "nvarchar", IsNullable = true)]
        public string? Remark
        {
            get; set;
        }
    }
}
