using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_MaterialLineStock")]

    public class V_MaterialLineStock
    {
        [SugarColumn(IsNullable = true)]
        public string? MaterialCode { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? MaterialDesc { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? Quantity { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? BatchCode { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? BarCode { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? LocationCode { get; set; }

    }
}
