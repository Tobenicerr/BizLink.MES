using BizLink.MES.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_RawLinesideStockLog", IsDisabledUpdateAll = true)]
    public class RawLinesideStockLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        /// <summary>
        /// 关联的库存记录ID
        /// </summary>
        /// 
        [SugarColumn(IsNullable = true)]
        public int? RawLinesideStockId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? WorkOrderId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? WorkOrderNo
        {
            get; set;
        }

        public InOutStatus InOutStatus
        {
            get; set;
        }

        /// <summary>
        /// 操作类型 (补料/盘盈/盘亏/移库/退库)
        /// </summary>
        public StockOperationType OperationType
        {
            get; set;
        }

        /// <summary>
        /// 变更数量 (正数表示增加, 负数表示减少)
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18, 4)")]
        public decimal ChangeQuantity
        {
            get; set;
        }

        /// <summary>
        /// 操作前数量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18, 4)")]
        public decimal QuantityBefore
        {
            get; set;
        }

        /// <summary>
        /// 操作后数量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18, 4)")]
        public decimal QuantityAfter
        {
            get; set;
        }

        /// <summary>
        /// 关联的业务单号 (如：补料单号, 盘点单号, 移库单号)
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string? SourceDocumentCode
        {
            get; set;
        }

        /// <summary>
        /// 物料编码 (冗余字段，便于追溯)
        /// </summary>
        [SugarColumn(Length = 50)]
        public string? MaterialCode
        {
            get; set;
        }

        /// <summary>
        /// 批次号 (冗余字段，便于追溯)
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string? BatchCode
        {
            get; set;
        }

        /// <summary>
        /// 条码 (冗余字段，便于追溯)
        /// </summary>
        [SugarColumn(Length =50, IsNullable = true)]
        public string? BarCode
        {
            get; set;
        }

        [SugarColumn(Length = 10, IsNullable = true)]
        public string? BaseUnit
        {
            get; set;
        }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string? TransferReason
        {
            get; set;
        }

        /// <summary>
        /// 库位ID (操作发生的库位)
        /// </summary>
        public int LocationId
        {
            get; set;
        }

        /// <summary>
        /// 库位编码 (冗余字段)
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string? LocationCode
        {
            get; set;
        }

        [SugarColumn(Length = 10, IsNullable = true)]
        public string? SapStatus
        {
            get; set;
        } = "0";

        /// <summary>
        /// 操作人
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string? CreateBy
        {
            get; set;
        }

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateOn { get; set; } = DateTime.Now;

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true, ColumnDataType = "nvarchar")]
        public string? Remark
        {
            get; set;
        }
    }
}
