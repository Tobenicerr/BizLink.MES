using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class PendingOperationTaskDto :IMapFrom<V_PendingOperationTask>
    {
        public int? TaskId { get; set; }

        public int WorkOrderId { get; set; }

        public int FactoryId { get; set; }

        public int? PrevProcessId { get; set; }

        public string WorkOrderNo { get; set; }

        public int WorkOrderProcessId { get; set; }

        public string Operation { get; set; }

        public string ProfitCenter { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialDesc { get; set; }

        public DateTime? DispatchDate { get; set; }

        public string? LeadingOrderMaterial { get; set; }


        public decimal? Quantity { get; set; }

        public decimal? CompletedQuantity { get; set; }

        public decimal? PrevCompletedQuantity { get; set; }

        public decimal? YieldQuantity { get; set; }

        public string? WorkCenter { get; set; }

        public string? NextWorkCenter { get; set; }

        public string? ControlKey { get; set; }

        public string? Status { get; set; }

        public string? PrevProcessStatus { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<V_PendingOperationTask, PendingOperationTaskDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class PendingOperationTaskCreateDto : IMapFrom<V_PendingOperationTask>
    {
    }

    public class PendingOperationTaskUpdateDto : IMapFrom<V_PendingOperationTask>
    {
    }
}
