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
    public class WorkOrderOperationConfirmDto : IMapFrom<WorkOrderOperationConfirm>
    {
        public int Id
        {
            get; set;
        }
        public int? WorkOrderId
        {
            get; set;
        }
        public int? ProcessId
        {
            get; set;
        }
        public int? TaskConfirmId
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

        public string? CompletedFlag
        {
            get; set;
        }

        public string? OperationNo
        {
            get; set;
        }

        public string? WorkCenterCode
        {
            get; set;
        }

        public DateTime? PostingDate
        {
            get; set;
        }


        public string? Remark
        {
            get; set;
        }

        public string? EmployeeId
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public decimal? YieldQuantity
        {
            get; set;
        }

        public decimal? ScrapQuantity
        {
            get; set;
        }

        public string? ActStartDate
        {
            get; set;
        }

        public string? ActStartTime
        {
            get; set;
        }

        public string? ActFinishDate
        {
            get; set;
        }

        public string? ActFinishTime
        {
            get; set;
        }

        public string? Message
        {
            get; set;
        }

        public string? MessageType
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        } = "0";

        public DateTime CreatedAt
        {
            get; set;
        } = DateTime.Now;

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


        public List<WorkOrderOperationConsumpDto> Consumps { get; set; } = new();
        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationConfirm, WorkOrderOperationConfirmDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            profile.CreateMap<WorkOrderOperationConsump, WorkOrderOperationConsumpDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderOperationConfirmCreateDto : IMapFrom<WorkOrderOperationConfirm>
    {
        public int? WorkOrderId
        {
            get; set;
        }
        public int? ProcessId
        {
            get; set;
        }

        public int? TaskConfirmId
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

        public string? CompletedFlag
        {
            get; set;
        }

        public string? OperationNo
        {
            get; set;
        }

        public string? WorkCenterCode
        {
            get; set;
        }

        public DateTime? PostingDate
        {
            get; set;
        }


        public string? Remark
        {
            get; set;
        }

        public string? EmployeeId
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public decimal? YieldQuantity
        {
            get; set;
        }

        public decimal? ScrapQuantity
        {
            get; set;
        }

        public string? ActStartDate
        {
            get; set;
        }

        public string? ActStartTime
        {
            get; set;
        }

        public string? ActFinishDate
        {
            get; set;
        }

        public string? ActFinishTime
        {
            get; set;
        }


        public string? CreatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationConfirmCreateDto, WorkOrderOperationConfirm>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderOperationConfirmUpdateDto : IMapFrom<WorkOrderOperationConfirm>
    {
        public int Id
        {
            get; set;
        }
       
        public string? CompletedFlag
        {
            get; set;
        }

        public string? OperationNo
        {
            get; set;
        }

        public string? WorkCenterCode
        {
            get; set;
        }

        public DateTime? PostingDate
        {
            get; set;
        }


        public string? Remark
        {
            get; set;
        }

        public string? EmployeeId
        {
            get; set;
        }


        public decimal? YieldQuantity
        {
            get; set;
        }

        public decimal? ScrapQuantity
        {
            get; set;
        }

        public string? ActStartDate
        {
            get; set;
        }

        public string? ActStartTime
        {
            get; set;
        }

        public string? ActFinishDate
        {
            get; set;
        }

        public string? ActFinishTime
        {
            get; set;
        }

        public string? Message
        {
            get; set;
        }

        public string? MessageType
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
            profile.CreateMap<WorkOrderOperationConfirmUpdateDto, WorkOrderOperationConfirm>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
