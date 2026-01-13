using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    /// <summary>
    /// 订单主表 (WorkOrder)
    /// </summary>
    /// 
    [SugarTable("Mes_WorkOrder")]
    //[SugarTable("Mes_WorkOrder", IsDisabledUpdateAll = true)]
    public class WorkOrder
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = true)]
        public int FactoryId { get; set; } // 工厂ID
        [SugarColumn(IsIgnore = true)]
        public string? FactoryCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true,Length = 20)]
        public string? OrderNumber { get; set; } // 订单号

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? MaterialCode { get; set; } // 产品代码

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? MaterialDesc { get; set; } // 产品名称

        [SugarColumn(IsNullable = true)]
        public decimal? Quantity { get; set; } // 订单数量

        [SugarColumn(IsNullable = true)]
        public DateTime? DispatchDate { get; set; } // 发货日期

        [SugarColumn(IsNullable = true, Length = 10)]
        public string? Status { get; set; } // 订单状态

        [SugarColumn(IsNullable = true)]
        public DateTime? ScheduledStartDate { get; set; } // 计划开始日期

        [SugarColumn(IsNullable = true)]
        public DateTime? ScheduledFinishDate { get; set; } // 计划完成日期

        [SugarColumn(IsNullable = true)]
        public DateTime? BasicStartDate { get; set; } // 基本开始日期

        [SugarColumn(IsNullable = true)]
        public DateTime? BasicFinishDate { get; set; } // 基本完成日期

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 255)]
        public string? PlannerRemark { get; set; } // 计划员备注

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? CollectiveOrder { get; set; } // 集合订单

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? SuperiorOrder { get; set; } // 上级订单

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? LeadingOrder { get; set; } // 前置订单

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? LeadingOrderMaterial { get; set; } // 前置订单物料

        [SugarColumn(IsNullable = true, Length = 100)]
        public string? ComponentMainMaterial { get; set; } // 组件主物料

        [SugarColumn(IsNullable = true)]
        public int? ConfirmNo
        {
            get; set;
        } // 预留

        [SugarColumn(IsNullable = true)]
        public int? ReservationNo { get; set; } // 预留

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? PlanningPeriod { get; set; } // 计划周期

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? StorageLocation { get; set; } // 存储位置

        [SugarColumn(IsNullable = true, Length = 20)]
        public string? ProfitCenter { get; set; } // 利润中心

        [SugarColumn(IsNullable = true)]
        public int? LabelCount { get; set; } // 标签数量

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateOn { get; set; } = DateTime.Now; // 创建时间

        [SugarColumn(IsNullable = true)]
        public string? CreateBy { get; set; } // 创建人
        [SugarColumn(IsIgnore = true)]
        public int? CableCount
        {
            get; set;
        }

        [SugarColumn(IsIgnore = true)]
        public int? ProcessCardPrintCount
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdateOn { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? UpdateBy
        {
            get; set;
        }

    }
}
