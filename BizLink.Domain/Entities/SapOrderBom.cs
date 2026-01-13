using BizLink.MES.Domain.Attributes;
using Microsoft.VisualBasic;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Sap_OrderBom", IsDisabledUpdateAll = true)]

    public class SapOrderBom
    {
        public int Id
        {
            get; set;
        }
        private string? _orderNo;

        [SapFieldName("ORDER_NUMBER")]
        public string? OrderNo
        {
            get
            {
                return _orderNo;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _orderNo = value;
                }
                else
                {
                    _orderNo = value.TrimStart('0');
                }
            }
        }

        [SapFieldName("RESERVATION_NUMBER")]
        public int? ReservationNo
        {
            get; set;
        }

        [SapFieldName("RESERVATION_ITEM")]
        public int? ReservationItem
        {
            get; set;
        }

        [SapFieldName("RESERVATION_TYPE")]
        public string? ReservationType
        {
            get; set;
        }
        private string? _materialCode;

        [SapFieldName("MATERIAL")]
        public string? MaterialCode
        {
            get
            {
                return _materialCode;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _materialCode = value;
                }
                else
                {
                    _materialCode = value.TrimStart('0');
                }
            }
        }

        [SapFieldName("MATERIAL_DESCRIPTION")]
        public string? MaterialDesc
        {
            get; set;
        }

        [SapFieldName("REQ_QUAN")]
        public decimal? RequireQuantity
        {
            get; set;
        }

        [SapFieldName("BASE_UOM")]
        public string? BaseUnit
        {
            get; set;
        }

        [SapFieldName("REQ_DATE")]
        public string? RequireDate
        {
            get; set;
        }

        [SapFieldName("WITHDRAWN_QUANTITY")]
        public decimal? WithdrawnQuantity
        {
            get; set;
        }

        [SapFieldName("FIX_INDICATOR")]
        public string? FixIndicator
        {
            get; set;
        }

        [SapFieldName("COMPONENT_SCRAP")]
        public decimal? ComponentScrap
        {
            get; set;
        }

        [SapFieldName("ITEM_NUMBER")]
        public string? BomItem
        {
            get; set;
        }

        [SapFieldName("OPERATION")]
        public string? Operation
        {
            get; set;
        }

        [SapFieldName("PROD_PLANT")]
        public string? FactoryCode
        {
            get; set;
        }

        [SapFieldName("TRANSFER_STATUS")]
        public string? TransferStatus
        {
            get; set;
        }

        [SapFieldName("COMPLETION_STATUS")]
        public string? ComletionStatus
        {
            get; set;
        }

        [SapFieldName("MSG")]
        public string? Message
        {
            get; set;
        }

        [SapFieldName("DATA_TIME")]
        public string? DataTimestamp
        {
            get; set;
        }

        [SapFieldName("XWAOK")]
        public string? AllowedMovement
        {
            get; set;
        }

        [SapFieldName("KDAUF")]
        public string? SalesOrderNo
        {
            get; set;
        }

        [SapFieldName("KDPOS")]
        public int? SalesOrderItem
        {
            get; set;
        }

        [SapFieldName("BAUGR")]
        public string? SuperMaterialCode
        {
            get; set;
        }

        public int? ConsumeType
        {
            get; set;
        }
    }
}
