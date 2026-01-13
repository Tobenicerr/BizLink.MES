using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_WorkOrderProcess")]
    public class V_WorkOrderProcess
    {
        [SugarColumn(ColumnName = "OrderId",IsNullable = true)]

        public int WorkOrderId { get; set; }

        [SugarColumn(ColumnName = "OrderNo", IsNullable = true)]

        public string? WorkOrder { get; set; }


        [SugarColumn(IsNullable = true)]
        public string Operation { get; set; } // 行号


        public string? FactoryCode { get; set; }

        [SugarColumn(IsNullable = true)]
        public int OperationId { get; set; }


        [SugarColumn(ColumnName = "WorkCenterCode", IsNullable = true)]
        public string? WorkCenter { get; set; } // 工作中心

        [SugarColumn(ColumnName = "WorkCenterName", IsNullable = true)]
        public string? WorkCenterName { get; set; } // 工作中心


        [SugarColumn(ColumnName = "MESStatus", IsNullable = true)]
        public string? Status { get; set; } // 工序状态


        [SugarColumn(IsNullable = true)]

        public DateTime? StartTime { get; set; } // 计划开始时间

        [SugarColumn(IsNullable = true)]
        public DateTime? EndTime { get; set; } // 计划结束时间

        [SugarColumn(IsNullable = true)]
        public DateTime? ActStartTime { get; set; } // 实际开始时间

        [SugarColumn(IsNullable = true)]
        public DateTime? ActEndTime { get; set; } // 实际结束时间


    }
}
