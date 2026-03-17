using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class PendingMaterialTaskCuttingDto : IMapFrom<V_PendingMaterialTaskCutting>
    {
        public int WorkOrderId { get; set; }
        public string WorkOrderNo { get; set; }
        public string ProfitCenter { get; set; }
        public DateTime ScheduledStartDate { get; set; }
        public decimal Quantity { get; set; }
        public int WorkOrderProcessId { get; set; }
        public string WorkCenter { get; set; }
        public string Operation { get; set; }
        public int BomId { get; set; }
        public string MaterialCode { get; set; }
        public string BomItem { get; set; }
        public string MaterialDesc { get; set; }
        public decimal RequiredQuantity { get; set; }
        public string Unit { get; set; }
        public int MaterialTaskId { get; set; }
        public int StepTaskId { get; set; }
        public decimal? TargetQuantity { get; set; }
        public decimal? TargetValue { get; set; }
        public string? TargetUnit { get; set; }

        public decimal? CompletedQuantity { get; set; }

        public CableCutParamDto? CuttingParam { get; set; }
        public string? Status { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<V_PendingMaterialTaskCutting, PendingMaterialTaskCuttingDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class PendingMaterialTaskCuttingCreateDto : IMapFrom<V_PendingMaterialTaskCutting> 
    {
    
    }

    public class PendingMaterialTaskCuttingUpdateDto : IMapFrom<V_PendingMaterialTaskCutting>
    {

    }
}
