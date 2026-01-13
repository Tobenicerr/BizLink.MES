using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class WarehouseLocationDto : IMapFrom<WarehouseLocation>
    {
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
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 库位名称/描述
        /// </summary>
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
        public decimal? MaxWeight
        {
            get; set;
        }

        /// <summary>
        /// 最大体积 (单位: 立方米)
        /// </summary>
        public decimal? MaxVolume
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark
        {
            get; set;
        }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedBy
        {
            get; set;
        }

        public DateTime? UpdatedAt
        {
            get; set;
        }

        public string? UpdatedBy
        {
            get; set;
        }

        /// <summary>
        /// 子节点 (用于构建树形结构)
        /// </summary>
        public List<WarehouseLocation> Children { get; set; } = new List<WarehouseLocation>();
    }

    public class WarehouseLocationCreateDto : IMapFrom<WarehouseLocation>
    {

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
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// 库位名称/描述
        /// </summary>
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
        public decimal? MaxWeight
        {
            get; set;
        }

        /// <summary>
        /// 最大体积 (单位: 立方米)
        /// </summary>
        public decimal? MaxVolume
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark
        {
            get; set;
        }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public string? CreatedBy
        {
            get; set;
        }
    }

    public class WarehouseLocationUpdateDto : IMapFrom<WarehouseLocation>
    {
        /// <summary>
        /// 库位名称/描述
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用 (true: 启用, false: 禁用)
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 排序码 (用于同级显示排序)
        /// </summary>
        public int SortOrder { get; set; } = 0;

        /// <summary>
        /// 最大承重 (单位: kg)
        /// </summary>
        public decimal? MaxWeight
        {
            get; set;
        }

        /// <summary>
        /// 最大体积 (单位: 立方米)
        /// </summary>
        public decimal? MaxVolume
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark
        {
            get; set;
        }

        public DateTime? UpdatedAt
        {
            get; set;
        }

        public string? UpdatedBy
        {
            get; set;
        }

    }
}
