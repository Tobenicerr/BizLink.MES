using BizLink.MES.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Wms_Location", IsDisabledUpdateAll = true)]
    public class WarehouseLocation
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        /// <summary>
        /// 父级库位ID (用于构建树形结构, 0表示根节点)
        /// </summary>
        public int ParentId
        {
            get; set;
        }

        /// <summary>
        /// 库位编码 (需保证同一父级下唯一)
        /// </summary>
        [SugarColumn(Length = 50)]
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 库位名称/描述
        /// </summary>
        [SugarColumn(Length = 100,ColumnDataType = "nvarchar")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 库位类型 (区域/货架/货位等)
        /// </summary>
        public LocationType LocationType
        {
            get; set;
        }

        /// <summary>
        /// 是否启用 (true: 启用, false: 禁用)
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 排序码 (用于同级显示排序)
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// 所属工厂ID
        /// </summary>
        public int FactoryId
        {
            get; set;
        }

        /// <summary>
        /// 最大承重 (单位: kg)
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18, 4)")]
        public decimal? MaxWeight
        {
            get; set;
        }

        /// <summary>
        /// 最大体积 (单位: 立方米)
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "decimal(18, 4)")]
        public decimal? MaxVolume
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? Remark
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        [SugarColumn(Length = 50, IsNullable = true)]
        public string? CreatedBy
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdatedAt
        {
            get; set;
        }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string? UpdatedBy
        {
            get; set;
        }

        /// <summary>
        /// 子节点 (用于构建树形结构)
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<WarehouseLocation> Children { get; set; } = new List<WarehouseLocation>();
    }
}
