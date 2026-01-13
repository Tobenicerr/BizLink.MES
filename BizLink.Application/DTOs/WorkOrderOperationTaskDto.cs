using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class WorkOrderOperationTaskDto :IMapFrom<WorkOrderOperationTask>
    {
        public int Id
        {
            get; set;
        }

        public int? WorkOrderId
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        }

        public int? WorkOrderProcessId
        {
            get; set;
        }

        public string? Operation
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }

        public int? WorkCenterId
        {
            get; set;
        }

        public DateTime? StartTime
        {
            get; set;
        }

        public DateTime? EndTime
        {
            get; set;
        }

        public decimal? CompletedQuantity
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public DateTime? CreatedOn
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        }

        public DateTime? UpdatedOn
        {
            get; set;
        }

        public string? UpdateBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationTask, WorkOrderOperationTaskDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderOperationTaskCreateDto : IMapFrom<WorkOrderOperationTask>
    {

        public int? WorkOrderId
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        }

        public int? WorkOrderProcessId
        {
            get; set;
        }

        public string? Operation
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }

        public int? WorkCenterId
        {
            get; set;
        }

        public DateTime? StartTime
        {
            get; set;
        }

        public DateTime? EndTime
        {
            get; set;
        }

        public decimal? CompletedQuantity
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }


        public string? CreatedBy
        {
            get; set;
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationTaskCreateDto, WorkOrderOperationTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }


    public class WorkOrderOperationTaskUpdateDto : IMapFrom<WorkOrderOperationTask>
    {
        public int Id
        {
            get; set;
        }

        public int? WorkOrderId
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        }

        public int? WorkOrderProcessId
        {
            get; set;
        }

        public string? Operation
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }

        public int? WorkCenterId
        {
            get; set;
        }

        public DateTime? StartTime
        {
            get; set;
        }

        public DateTime? EndTime
        {
            get; set;
        }

        public decimal? CompletedQuantity
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }


        public DateTime? UpdatedOn
        {
            get; set;
        }

        public string? UpdateBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationTaskUpdateDto, WorkOrderOperationTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
