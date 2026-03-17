using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_WorkOrderKittingExecute")]

    public class V_WorkOrderKittingExecute
    {

        public int WorkOrderProcessId { get; set; }

        public string WorkOrderNo { get; set; }


        public int FactoryId { get; set; }

        public string MaterialCode { get; set; }

        public string? MaterialDesc { get; set; }

        public decimal Quantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DispatchDate { get; set; }

        public string ProfitCenter { get; set; }

        public int LabelCount { get; set; }

        public string Status { get; set; }

        public DateTime? KittingTime { get; set; }

        public DateTime? RequestTime { get; set; }

        public DateTime? GroupTime { get; set; }

        public string? OperateUser { get; set; }

        public string? GroupCode { get; set; }

        public string? ShotRemark { get; set; }
    }
}
