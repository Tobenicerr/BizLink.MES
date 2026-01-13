using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class WorkOrderViewDto : IMapFrom<V_WorkOrder>
    {
        public int Id
        {
            get; set;
        }
        public string? FactoryCode
        {
            get; set;
        }
        public string? OrderNumber
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
        public decimal? Quantity
        {
            get; set;
        }
        public DateTime? DispatchDate
        {
            get; set;
        }

        public DateTime? StartTime
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }
        public string? PlannerRemark
        {
            get; set;
        }
        public string? ProfitCenter
        {
            get; set;
        }

        public string? ProductCode
        {
            get; set;
        }

        public int? LabelCount
        {
            get; set;
        }
    }

    public class WorkOrderViewCreateDto : IMapFrom<V_WorkOrder>
    {
        public int Id
        {
            get; set;
        }
        public string? FactoryCode
        {
            get; set;
        }
        public string? OrderNumber
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
        public decimal? Quantity
        {
            get; set;
        }
        public DateTime? DispatchDate
        {
            get; set;
        }

        public DateTime? StartTime
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }
        public string? PlannerRemark
        {
            get; set;
        }
        public string? ProfitCenter
        {
            get; set;
        }

        public string? ProductCode
        {
            get; set;
        }

        public int? LabelCount
        {
            get; set;
        }
    }

    public class WorkOrderViewUpdateDto : IMapFrom<V_WorkOrder>
    {
        public int Id
        {
            get; set;
        }
        public string? FactoryCode
        {
            get; set;
        }
        public string? OrderNumber
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
        public decimal? Quantity
        {
            get; set;
        }
        public DateTime? DispatchDate
        {
            get; set;
        }

        public DateTime? StartTime
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }
        public string? PlannerRemark
        {
            get; set;
        }
        public string? ProfitCenter
        {
            get; set;
        }

        public string? ProductCode
        {
            get; set;
        }

        public int? LabelCount
        {
            get; set;
        }
    }
}
