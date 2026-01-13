using BizLink.MES.Domain.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_MaterialTransferLog", IsDisabledUpdateAll = true)]
    public class MaterialTransferLog
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("WMS_NUMBER")]

        public string? TransferNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("EVENT_TYPE")]
        public string? TransferType
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("PSTNG_DATE")]
        public DateTime? PostingDate
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("DOC_DATE")]
        public DateTime? DocumentDate
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int? WorkOrderId
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("ORDERID")]
        public string? WorkOrderNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("MATERIAL")]
        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("PLANT")]
        public string? FactoryCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("STGE_LOC")]
        public string? FromLocationCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("BATCH")]
        public string? BatchCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("MOVE_TYPE")]
        public string? MovementType
        {
            get; set;
        }

        [SugarColumn(IsNullable = true,Length = 20)]
        [SapFieldName("MOVE_REAS")]
        public string? MovementReason
        {
            get; set;
        }
        /// <summary>
        /// 数量
        /// </summary>
        [SugarColumn(ColumnDataType = "decimal(18, 3)")]
        [SapFieldName("ENTRY_QNT")]
        public decimal? Quantity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("ENTRY_UOM")]
        public string? BaseUnit
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("MOVE_MAT")]
        public string? ReceivingMaterialCode
        {
            get; set;
        }

        /// <summary>
        /// 关联的库存记录ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int? FromStockId
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("MOVE_STLOC")]
        public string? ToLocationCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("MOVE_PLANT")]
        public string? ToFactoryCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("MOVE_BATCH")]
        public string? ReceivingBatchCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public string? Status
        {
            get; set;
        } = "0";

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(IsNullable = true)] 
        [SapFieldName("HEADER_TXT")]
        public string? Remark
        {
            get; set;
        }

        [SugarColumn(IsNullable = true,Length = 50)]
        [SapFieldName("COSTCENTER")]
        public string? CostCenterCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("MSG")]
        public string? Message  
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("MSG_TYPE")]
        public string? MessageType
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public int? StockLogId
        {
            get; set;
        }
        public DateTime CreatedAt
        {
            get; set;
        } = DateTime.Now;

        [SugarColumn(IsNullable = true)]
        public string? CreatedBy
        {
            get; set;
        }
        public DateTime UpdatedAt
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public string? UpdatedBy
        {
            get; set;
        }
    }
}
