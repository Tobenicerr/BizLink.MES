using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public enum ConsumptionType
    {
        [Description("正常消耗")]
        Consumption = 261,

        [Description("物料移库")]
        MaterialTransfer = 311,

        [Description("报废")]
        Scrap = 903,
        [Description("报废取消")]
        ScrapReversal = 904,

        [Description("盘盈")]
        AdjustStockGain = 922,

        [Description("盘亏")]
        AdjustStockLoss = 921,

        [Description("成本中心领料")]
        GoodsIssuetoCostCenter = 201,

        [Description("成本中心退料")]
        ReversalofGoodsIssuetoCostCenter = 202,

    }
}
