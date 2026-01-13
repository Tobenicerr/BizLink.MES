using BizLink.MES.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs.Request
{
    public  class TransferSapRequest
    {
        public string FactoryCode { get; set;}

        public string? EmployeeId { get; set;}

        public MaterialTransferType? TransferType { get; set; }

        public int? WorkOrderId { get; set; }
        public string? WorkOrderNo { get; set;}

        public ConsumptionType? ConsumptionType
        {
            get; set;
        }

        public List<TransferStock> Stocks { get; set;} = new List<TransferStock>();
        public string FromLocation { get; set;}

        public string? ToLocation { get; set;}

        public string? CostCenterCode
        {
            get; set;
        } = null;
        

        public string? MovementReason
        {
            get; set;
        } = null;
    }

    public class TransferStock
    {
        public int? StockId
        {
            get; set;
        }

        public int? StockLogId
        {
            get; set;
        }

        public string MaterialCode
        {
            get; set;
        }

        public decimal Quantity
        {
            get; set;
        }

        public string BaseUnit
        {
            get; set;
        }

        public string BatchCode
        {
            get; set;
        }

        public string? BatchCodeRecive
        {
            get; set;
        } = null ;

    }
}
