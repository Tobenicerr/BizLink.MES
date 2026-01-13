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
    public class WorkOrderTaskExecuteConsumpDto:IMapFrom<WorkOrderTaskExecuteConsump>
    {
        public int Id
        {
            get; set;
        }

        public int ExeLogId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        public string? BatchCode
        {
            get; set;
        }

        public string? BarCode
        {
            get; set;
        }

        public decimal? ConsumedQuantity
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? MovementType
        {
            get; set;
        }

        public string? MovementReason
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
            profile.CreateMap<WorkOrderTaskExecuteConsump, WorkOrderTaskExecuteConsumpDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderTaskExecuteConsumpCreateDto : IMapFrom<WorkOrderTaskExecuteConsump>
    {
        public int Id
        {
            get; set;
        }

        public int ExeLogId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        public string? BatchCode
        {
            get; set;
        }

        public string? BarCode
        {
            get; set;
        }

        public decimal? ConsumedQuantity
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? MovementType
        {
            get; set;
        }

        public string? MovementReason
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
            profile.CreateMap<WorkOrderTaskExecuteConsumpCreateDto, WorkOrderTaskExecuteConsump>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderTaskExecuteConsumpUpdateDto : IMapFrom<WorkOrderTaskExecuteConsump>
    {
        public int Id
        {
            get; set;
        }

        public int ExeLogId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        public string? BatchCode
        {
            get; set;
        }

        public string? BarCode
        {
            get; set;
        }

        public decimal? ConsumedQuantity
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? MovementType
        {
            get; set;
        }

        public string? MovementReason
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
            profile.CreateMap<WorkOrderTaskExecuteConsumpUpdateDto, WorkOrderTaskExecuteConsump>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
