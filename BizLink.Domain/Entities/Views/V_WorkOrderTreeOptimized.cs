using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_WorkOrderTree_Optimized")]

    public class V_WorkOrderTreeOptimized
    {
        public int WorkOrderId
        {
            get; set;
        }

        

        public int WorkOrderProcessId
        {
            get; set;
        }

        

        public string? RootLeadingOrder
        {
            get; set;
        }

        

        public int Level
        {
            get; set;
        }

        

        public string? WorkOrderNo
        {
            get; set;
        }

        

        public string? SuperiorOrder
        {
            get; set;
        }


        

        public string? Path
        {
            get; set;
        }

        

        public string? VisualTree
        {
            get; set;
        }

        

        public int FactoryId
        {
            get; set;
        }

        

        public string? FactoryCode
        {
            get; set;
        }

        

        public string? MaterialCode
        {
            get; set;
        }

        

        public string? MaterialDesc
        {
            get; set;
        }

 
        

        public DateTime? DispatchDate
        {
            get; set;
        }

        

        public string? ProfitCenter
        {
            get; set;
        }

        

        public string? OrderStatus
        {
            get; set;
        }

        

        public string? StorageLocation
        {
            get; set;
        }

        

        public string? OpStatus
        {
            get; set;
        }

        

        public string? Operation
        {
            get; set;
        }

        

        public string? WorkCenter
        {
            get; set;
        }

        

        public decimal? Quantity
        {
            get; set;
        }

        

        public decimal? CompletedQuantity
        {
            get; set;
        }

        

        public string? ControlKey
        {
            get; set;
        }

    }
}
