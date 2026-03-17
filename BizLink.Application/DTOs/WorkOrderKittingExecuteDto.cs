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
    public class WorkOrderKittingExecuteDto : IMapFrom<V_WorkOrderKittingExecute>
    {
        public int WorkOrderProcessId { get; set; }

        public string WorkOrderNo { get; set; }

        public int FactoryId { get; set; }

        public string MaterialCode { get; set; }

        public string? MaterialDesc { get; set; }

        public decimal Quantity { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime DispatchDate { get; set; }

        public string ProfitCenter { get; set; }

        public int LabelCount { get; set; }

        public string Status { get; set; }

        public DateTime? KittingTime { get; set; }

        public DateTime? RequestTime { get; set; }

        public DateTime? GroupTime { get; set; }

        public string? OperateUser { get; set; }

        public string? GroupCode { get; set; }

        public string? ShotRemark { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<V_WorkOrderKittingExecute, WorkOrderKittingExecuteDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }

    public class WorkOrderKittingExecuteCreateDto : IMapFrom<V_WorkOrderKittingExecute> { }

    public class WorkOrderKittingExecuteUpdateDto : IMapFrom<V_WorkOrderKittingExecute> { }

}
