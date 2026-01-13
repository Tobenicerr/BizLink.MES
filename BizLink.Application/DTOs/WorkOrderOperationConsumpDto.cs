using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Attributes;
using BizLink.MES.Domain.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class WorkOrderOperationConsumpDto : IMapFrom<WorkOrderOperationConsump>
    {
        public int Id
        {
            get; set;
        }

        public int OperationConfirmId
        {
            get; set;
        }

        public int? SapConfirmationNo
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        }

        public string? ConfirmSequence
        {
            get; set;
        }

        public int? ReservationNo
        {
            get; set;
        }

        public int? ReservationItem
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

        public string? FactoryCode
        {
            get; set;
        }

        public string? FromLocationCode
        {
            get; set;
        }

        public string? MovementType
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }

        public string? BaseUnit
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

        public DateTime CreatedAt
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        }

        public DateTime? UpdatedAt
        {
            get; set;
        }

        public string? UpdatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationConsump, WorkOrderOperationConsumpDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderOperationConsumpCreateDto : IMapFrom<WorkOrderOperationConsump>
    {

        public int OperationConfirmId
        {
            get; set;
        }

        public int? SapConfirmationNo
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        }

        public string? ConfirmSequence
        {
            get; set;
        }

        public int? ReservationNo
        {
            get; set;
        }

        public int? ReservationItem
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

        public string? FactoryCode
        {
            get; set;
        }

        public string? FromLocationCode
        {
            get; set;
        }

        public string? MovementType
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? MovementReason
        {
            get; set;
        }


        public string? CreatedBy
        {
            get; set;
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationConsumpCreateDto, WorkOrderOperationConsump>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderOperationConsumpUpdateDto : IMapFrom<WorkOrderOperationConsump>
    {
        public int Id
        {
            get; set;
        }

        public string? FromLocationCode
        {
            get; set;
        }

        public string? MovementType
        {
            get; set;
        }

        public decimal? Quantity
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

        public DateTime? UpdatedAt
        {
            get; set;
        }

        public string? UpdatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationConsumpUpdateDto, WorkOrderOperationConsump>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
