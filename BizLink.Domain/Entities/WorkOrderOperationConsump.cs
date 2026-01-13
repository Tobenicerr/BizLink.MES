using BizLink.MES.Domain.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderOperationConsump", IsDisabledUpdateAll = true)]
    public class WorkOrderOperationConsump
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        public int OperationConfirmId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 10)]
        [SapFieldName("CONF_NO")]

        public int? SapConfirmationNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("ORDERID")]
        public string? WorkOrderNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("SEQUENCE")]
        public string? ConfirmSequence
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("RESERV_NO")]
        public int? ReservationNo
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("RES_ITEM")]
        public int? ReservationItem
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("MATERIAL")]
        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("BATCH")]
        public string? BatchCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("PLANT")]
        public string? FactoryCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("STGE_LOC")]
        public string? FromLocationCode
        {
            get; set;
        }



        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("MOVE_TYPE")]
        public string? MovementType
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

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("ENTRY_UOM")]
        public string? BaseUnit
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("MOVE_REAS")]
        public string? MovementReason
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 20)]
        public string? Status
        {
            get; set;
        } = "1";

        [SugarColumn(IsNullable = true)]
        public DateTime CreatedAt
        {
            get; set;
        } = DateTime.Now;
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? CreatedBy
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdatedAt
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 50)]
        public string? UpdatedBy
        {
            get; set;
        }

    }
}
