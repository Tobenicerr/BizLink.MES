using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class WorkOrderTaskConfirmDto : IMapFrom<WorkOrderTaskConfirm>
    {

        public int Id
        {
            get; set;
        }

        public int TaskId
        {
            get; set;
        }

        public int WorkStationId
        {
            get; set;
        }
        public string? ConfirmNumber
        {
            get; set;
        }

        public DateTime? ConfirmDate
        {
            get; set;
        }

        public decimal? ConfirmQuantity
        {
            get; set;
        }

        public string? EmployerCode
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }


        public string? Remark
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderTaskConfirm, WorkOrderTaskConfirmDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderTaskConfirmCreateDto: IMapFrom<WorkOrderTaskConfirm>
    {
        public int TaskId
        {
            get; set;
        }

        public int WorkStationId
        {
            get; set;
        }
        public string? ConfirmNumber
        {
            get; set;
        }

        public DateTime? ConfirmDate
        {
            get; set;
        }

        public decimal? ConfirmQuantity
        {
            get; set;
        }

        public string? EmployerCode
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }


        public string? Remark
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderTaskConfirmCreateDto, WorkOrderTaskConfirm>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderTaskConfirmUpdateDto : IMapFrom<WorkOrderTaskConfirm>
    {

        public int Id
        {
            get; set;
        }


        public int WorkStationId
        {
            get; set;
        }

        public DateTime? ConfirmDate
        {
            get; set;
        }


        public string? EmployerCode
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }


        public string? Remark
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderTaskConfirmUpdateDto, WorkOrderTaskConfirm>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
