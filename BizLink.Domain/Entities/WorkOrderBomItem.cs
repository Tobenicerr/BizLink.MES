using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    /// <summary>
    /// 订单物料清单 (OrderBomItem)
    /// </summary>
    [SugarTable("Mes_WorkOrderBomItem")]
    //[SugarTable("Mes_WorkOrderBomItem", IsDisabledUpdateAll = true)]
    public class WorkOrderBomItem
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = true)]
        public int WorkOrderId { get; set; }
        [SugarColumn(IsIgnore = true)]
        public string? WorkOrderNo
        {
            get; set;
        } // 物料代码

        [SugarColumn(IsNullable = true)]
        public int WorkOrderProcessId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? MaterialCode { get; set; } // 物料代码

        [SugarColumn(IsNullable = true)]
        public string? MaterialDesc { get; set; } // 物料名称

        [SugarColumn(IsNullable = true)]
        public decimal? RequiredQuantity { get; set; } // 需求数量

        [SugarColumn(IsNullable = true)]
        public string? Unit { get; set; } // 单位

        [SugarColumn(IsNullable = true)]
        public int? ReservationItem { get; set; } // 预留项目

        [SugarColumn(IsNullable = true)]
        public decimal? ComponentScrap { get; set; } // 组件废料

        [SugarColumn(IsNullable = true)]
        public string? BomItem { get; set; } // BOM项目

        [SugarColumn(IsNullable = true)]
        public string? Operation { get; set; } // 操作

        [SugarColumn(IsNullable = true)]
        public bool? MovementAllowed { get; set; } // 允许移动

        [SugarColumn(IsNullable = true)]
        public bool? QuantityIsFixed { get; set; } // 数量已固定

        [SugarColumn(IsNullable = true)]
        public int? ConsumeType { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? SyncWMSStatus { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? SuperMaterialCode
        {
            get; set;
        } // 操作

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateOn
        {
            get; set;
        } = DateTime.Now; // 创建时间

        [SugarColumn(IsNullable = true)]
        public string? CreateBy
        {
            get; set;
        } // 创建人

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdateOn
        {
            get; set;
        } // 更新时间

        [SugarColumn(IsNullable = true)]
        public string? UpdateBy
        {
            get; set;
        } // 更新人
        [SugarColumn(IsNullable = true)]

        public bool? IsActive
        {
            get; set;
        } = true; // 是否有效

        [SugarColumn(IsNullable = true)]

        public int? BomVersion
        {
            get; set;
        } = 1;// 工艺版本
    }
}
