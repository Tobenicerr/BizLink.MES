using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    /// <summary>
    /// 线边库库存操作日志类型
    /// </summary>
    public enum StockOperationType
    {
        /// <summary>
        /// 补料入库
        /// </summary>
        [Description("补料")]
        Replenishment = 1,

        /// <summary>
        /// 盘点盈利
        /// </summary>
        [Description("904报废冲销")]
        StockGain = 2,

        /// <summary>
        /// 盘点亏损
        /// </summary>
        /// 
        [Description("903报废")]
        StockLoss = 3,

        /// <summary>
        /// 移库（出库）
        /// </summary>
        /// 
        [Description("移出库")]
        TransferOut = 4,

        /// <summary>
        /// 移库（入库）
        /// </summary>
        /// 
        [Description("移入库")]
        TransferIn = 5,

        /// <summary>
        /// 退库
        /// </summary>
        ///
        [Description("退库")]
        Return = 6,

        [Description("原材料外发")]
        ShipmentOfRawMaterials = 7,

        [Description("外发退回")]
        ShipmentReturn = 8,

        [Description("921盘亏")]
        AdjustStockLoss = 9,

        [Description("922盘盈")]
        AdjustStockGain = 10,

        [Description("成本中心领料")]
        GoodsIssuetoCostCenter = 11,

        [Description("成本中心退料")]
        ReversalofGoodsIssuetoCostCenter = 12,


        [Description("2100移库")]
        ProductLineStockTransfer = 13,

        [Description("Mes库存增加")]
        MesStockAdd = 14,

        [Description("Mes库存减少")]
        MesStockReduce = 15,
    }
}
