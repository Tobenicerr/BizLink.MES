using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_WorkOrder")]
    public class V_WorkOrder
    {
        [SugarColumn(IsPrimaryKey = true, ColumnName = "OrderId")]
        public int Id { get; set; }
        public string? FactoryCode { get; set; }
        public string? OrderNumber { get; set; }
        public string? MaterialCode { get; set; }
        public string? MaterialDesc { get; set; }
        public decimal? Quantity { get; set; }
        public DateTime? DispatchDate { get; set; }

        public DateTime? StartTime { get; set; }
        public string? Status { get; set; }
        public string? PlannerRemark { get; set; }
        public string? ProfitCenter { get; set; }

        public string? ProductCode { get; set; }

        public int? LabelCount { get; set; }
    }
}
