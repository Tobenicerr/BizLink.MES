using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    /// <summary>
    /// 参数配置主表 (分组)
    /// </summary>
    [SugarTable("Sys_ParameterGroup", IsDisabledUpdateAll = true)]
    public class ParameterGroup
    {
        
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        /// <summary>
        /// 父级ID，0表示根节点
        /// </summary>
        public int ParentId
        {
            get; set;
        }

        /// <summary>
        /// 分组名称 (用于UI显示)
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100)]
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 分组键 (用于程序内部调用，同一父级下应唯一)
        /// </summary>
        [SugarColumn(Length = 100)]
        public string Key
        {
            get; set;
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 排序号 (同级节点排序)
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

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 导航属性，包含的参数明细项
        /// </summary>
        [Navigate(NavigateType.OneToMany, nameof(ParameterItem.GroupId))]
        public List<ParameterItem>? Items
        {
            get; set;
        }
    }
}
