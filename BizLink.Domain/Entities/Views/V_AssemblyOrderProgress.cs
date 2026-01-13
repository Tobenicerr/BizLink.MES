using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_AssemblyOrderProgress")]
    public class V_AssemblyOrderProgress
    {
        public int Id
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }

        public string? ProfitCenter
        {
            get; set;
        }
        public string? OrderNumber
        {
            get; set;
        }

        public string? Operation
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

        public string? SuperiorOrder
        {
            get; set;
        }
        public string? LeadingOrder
        {
            get; set;
        }

        public DateTime? EndTime
        {
            get; set;
        }

        public string? Status
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
        public decimal? MachineTime
        {
            get; set;
        }
        public decimal? WorkingTime
        {
            get; set;
        }

        public string? WorkCenter
        {
            get; set;
        } // 工作中心

        public decimal? ConfirmedQuantity
        {
            get; set;
        }

        public decimal? UnConfirmedQuantity
        {
            get; set;
        }

        public DateTime? ConfirmDate
        {
            get; set;
        }

    }
}
