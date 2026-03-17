using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_WmsWorkOrderPickingLog")]

    public class V_WmsWorkOrderPickingLog
    {
        public string WorkOrderNo { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialDesc { get; set; }

        public string? BatchCode { get; set; }
        public string BarCode { get; set; }

        public string WmsStockName { get; set; }
        public string TaskStatus { get; set; }
        public string WmsTaskCode { get; set; }
        public string BaseUnit { get; set; }

        public decimal Quantity { get; set; }

        public int WmsSourceId { get; set; }

    }
}
