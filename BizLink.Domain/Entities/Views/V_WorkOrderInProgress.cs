using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_WorkOrderInProgress")]
    public class V_WorkOrderInProgress
    {
        public int FactoryId
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }
        public int OrderId { get; set; }
        public int OrderProcessId { get; set; }

        public int? PrevProcessId
        {
            get; set;
        }
        public string? ProfitCenter { get; set; }
        public string? OrderNumber { get; set; }

        public string? Operation { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialDesc { get; set; }

        public decimal? Quantity { get; set; }
        public string? ControlKey
        {
            get; set;
        }
        public decimal? RequiredQuantity
        {
            get; set;
        }
        public int? TaskId
        {
            get; set;
        }

        public decimal? TaskQuantity
        {
            get; set;
        }

        public decimal? TaskCompletedQty
        {
            get; set;
        }
        public int? CablePcs
        {
            get; set;
        }
        public string? CableItem { get; set; }
        public string? CableMaterial { get; set; }

        public string? CableMaterialDesc
        {
            get; set;
        }
        public string? LeadingOrderMaterial
        {
            get; set;
        }
        
        public decimal? CompletedQty { get; set; }
        public decimal? CableLength { get; set; }

        public decimal? CableLengthUsl { get; set; }

        public decimal? CableLengthDsl { get; set; }
        public DateTime? DispatchDate { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? ActStartTime
        {
            get; set;
        }

        public DateTime? ActEndTime
        {
            get; set;
        }
        public string? Status { get; set; }
        public string? PlannerRemark { get; set; }

        public int? WorkCenterId
        {
            get; set;
        }

        public int? WorkStationId
        {
            get; set;
        }
        public string? WorkCenter { get; set; } // 工作中心

        public string? WorkStationCode
        {
            get; set;
        } // 工作中心

        public string? NextWorkCenter { get; set; } // 工作中心
    }
}
