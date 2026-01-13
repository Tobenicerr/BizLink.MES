using BizLink.MES.Domain.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Sap_OrderOperation", IsDisabledUpdateAll = true)]
    public class SapOrderOperation
    {
        public int Id { get; set; }
        private string? _orderNo;
        [SapFieldName("ORDER_NUMBER")]
        public string?  OrderNo {
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

        [SapFieldName("COUNTER")]
        public int?  Counter { get; set; }

        [SapFieldName("PLANT")]
        public string?  PlantCode { get; set; }

        [SapFieldName("DISPATCH_DATE")]
        public string?  DispatchDate { get; set; }

        [SapFieldName("WORK_CENTER")]
        public string?  WorkCenter { get; set; }

        [SapFieldName("OPERATION_NUMBER")]
        public string?  OperationNo { get; set; }

        private string? _materialCode;

        [SapFieldName("MATERIAL")]
        public string?  MaterialCode
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

        [SapFieldName("TARGET_QUANTITY")]
        public decimal?  TargetQuantity { get; set; }

        [SapFieldName("PRODUCTION_FINISH_DATE")]
        public string?  ProductFinish { get; set; }

        [SapFieldName("PRODUCTION_START_DATE")]
        public string?  ProductStart { get; set; }

        [SapFieldName("FINISH_DATE")]
        public string? FinishDate
        {
            get; set;
        }

        [SapFieldName("START_DATE")]
        public string? StartDate
        {
            get; set;
        }

        [SapFieldName("REMARK_PLANNER")]
        public string? PlannerRemark
        {
            get; set;
        }

        [SapFieldName("COLLECTIVE_ORDER")]
        public string? CollectiveOrder
        {
            get; set;
        }

        private string? _superiorOrder;
        [SapFieldName("SUPERIOR_ORDER")]
        public string? SuperiorOrder
        {
            get
            {
                return _superiorOrder;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _superiorOrder = value;
                }
                else
                {
                    _superiorOrder = value.TrimStart('0');
                }
            }
        }
        private string? _leadingOrder;

        [SapFieldName("LEADING_ORDER")]
        public string? LeadingOrder
        {
            get
            {
                return _leadingOrder;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _leadingOrder = value;
                }
                else
                {
                    _leadingOrder = value.TrimStart('0');
                }
            }
        }

        private string? _leadingMaterial;

        [SapFieldName("LEAD_MATNR")]
        public string? LeadingMaterial
        {
            get
            {
                return _leadingMaterial;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _leadingMaterial = value;
                }
                else
                {
                    _leadingMaterial = value.TrimStart('0');
                }
            }
        }

        [SapFieldName("PVC_MATERIAL")]
        public string? ComponentMaterial
        {
            get; set;
        }

        [SapFieldName("CONF_NO")]
        public int? ConfirmNo
        {
            get; set;
        }

        [SapFieldName("RESERVATION_NUMBER")]
        public int? ReservationNo
        {
            get; set;
        }

        [SapFieldName("ORDER_STATUS")]
        public string? OrderStatus
        {
            get; set;
        }

        [SapFieldName("OPERATION_STATUS")]
        public string? OperationStatus
        {
            get; set;
        }

        [SapFieldName("STATUS_MES")]
        public string? MesStatus
        {
            get; set;
        }

        [SapFieldName("TRANSFER_STATUS")]
        public string? TransferStatus
        {
            get; set;
        }

        [SapFieldName("DISPATCH_STATUS")]
        public string? DispatchStatus
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

        [SapFieldName("OPR_CNTRL_KEY")]
        public string? ControlKey
        {
            get; set;
        }

        [SapFieldName("PLAN_PERIOD")]
        public string? PlanPeriod
        {
            get; set;
        }

        [SapFieldName("NEXT_WORK_CENTER")]
        public string? NextWorkCenter
        {
            get; set;
        }

        [SapFieldName("LGORT")]
        public string? StorageLocation
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

        [SapFieldName("INSMK")]
        public string? StockType
        {
            get; set;
        }

        [SapFieldName("VGW_SETUP1")]
        public decimal? SetupTime1
        {
            get; set;
        }

        [SapFieldName("VGW_SETUP2")]
        public decimal? SetupTime2
        {
            get; set;
        }

        [SapFieldName("VGW_MACHINE1")]
        public decimal? MachineTime1
        {
            get; set;
        }

        [SapFieldName("VGW_MACHINE2")]
        public decimal? MachineTime2
        {
            get; set;
        }

        [SapFieldName("CONF_ACTI_UNIT1")]
        public string? ConfActiUnit1
        {
            get; set;
        }

        [SapFieldName("CONF_ACTI_UNIT2")]
        public string? ConfActiUnit2
        {
            get; set;
        }

        [SapFieldName("CONF_ACTI_UNIT3")]
        public string? ConfActiUnit3
        {
            get; set;
        }

        [SapFieldName("CONF_ACTI_UNIT4")]
        public string? ConfActiUnit4
        {
            get; set;
        }

        [SapFieldName("PRCTR")]
        public string? ProfitCenter
        {
            get; set;
        }

        [SapFieldName("LABEL_COUNT")]
        public int? LabelCount
        {
            get; set;
        }

        [SapFieldName("SENDD")]
        public string? LastOperationFinish
        {
            get; set;
        }
    }
}
