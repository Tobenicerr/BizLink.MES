using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    /// <summary>
    /// 订单工序表 (OrderProcess)
    /// </summary>
    /// 
    [SugarTable("Mes_WorkOrderProcess")]
    //[SugarTable("Mes_WorkOrderProcess", IsDisabledUpdateAll = true)]
    public class WorkOrderProcess
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = true)]
        public int? WorkOrderId { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? WorkOrderNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? ConfirmNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? Status { get; set; } // 工序状态

        [SugarColumn(IsNullable = true)]
        public DateTime? StartTime { get; set; } // 计划开始时间

        [SugarColumn(IsNullable = true)]
        public DateTime? EndTime { get; set; } // 计划结束时间

        [SugarColumn(IsNullable = true)]
        public decimal? Quantity
        {
            get; set;
        } // 数量

        [SugarColumn(IsNullable = true)]
        public decimal? CompletedQuantity
        {
            get; set;
        }  = 0; // 完成数量

        [SugarColumn(IsNullable = true)]
        public DateTime? ActStartTime { get; set; } // 实际开始时间

        [SugarColumn(IsNullable = true)]
        public DateTime? ActEndTime { get; set; } // 实际结束时间

        [SugarColumn(IsNullable = true)]
        public string? WorkCenter { get; set; } // 工作中心

        [SugarColumn(IsNullable = true)]
        public string? Operation { get; set; } // 操作

        [SugarColumn(IsNullable = true)]
        public string? ControlKey { get; set; } // 控制码

        [SugarColumn(IsNullable = true)]
        public string? NextWorkCenter { get; set; } // 下一工作中心

        [SugarColumn(IsNullable = true)]
        public int? ProcessCardPrintCount
        {
            get; set;
        } = 0;

        [SugarColumn(IsNullable = true)]
        public decimal? SetupTime { get; set; } // 设置时间

        [SugarColumn(IsNullable = true)]
        public string? SetupTimeUnit { get; set; } // 设置时间单位

        [SugarColumn(IsNullable = true)]
        public decimal? MachineTime { get; set; } // 机器时间

        [SugarColumn(IsNullable = true)]
        public string? MachineTimeUnit { get; set; } // 机器时间单位

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateOn { get; set; } = DateTime.Now; // 创建时间

        [SugarColumn(IsNullable = true)]
        public string? CreateBy { get; set; } // 创建人

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdateOn { get; set; } // 更新时间

        [SugarColumn(IsNullable = true)]
        public string? UpdateBy { get; set; } // 更新人

        [SugarColumn(IsNullable = true)]
        public bool? IsActive { get; set; } = true; // 软删除标志

        [SugarColumn(IsNullable = true)]
        public int? OperationVersion
        {
            get; set;
        } = 1; // 工艺版本
    }
}
