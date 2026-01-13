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
    public class WorkOrderTaskExecuteLogDto :IMapFrom<WorkOrderTaskExecuteLog>
    {
        public int Id
        {
            get; set;
        }
        public string? TaskLevel
        {
            get; set;
        }

        public int? TaskId
        {
            get; set;
        }

        public string? BatchCode
        {
            get; set;
        }

        public decimal? CompletedQuantity
        {
            get; set;
        }

        public int? WorkStationId
        {
            get; set;
        }

        public string? WorkStationCode
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

        public string? EmployerCode
        {
            get; set;
        }

        public string? Remark
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
            profile.CreateMap<WorkOrderTaskExecuteLog, WorkOrderTaskExecuteLogDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderTaskExecuteLogCreateDto : IMapFrom<WorkOrderTaskExecuteLog>
    {

        public string? TaskLevel
        {
            get; set;
        }

        public int? TaskId
        {
            get; set;
        }

        public string? BatchCode
        {
            get; set;
        }

        public decimal? CompletedQuantity
        {
            get; set;
        }

        public int? WorkStationId
        {
            get; set;
        }

        public string? WorkStationCode
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

        public string? EmployerCode
        {
            get; set;
        }

        public string? Remark
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
            profile.CreateMap<WorkOrderTaskExecuteLogCreateDto, WorkOrderTaskExecuteLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderTaskExecuteLogUpdateDto : IMapFrom<WorkOrderTaskExecuteLog>
    {
        public int Id
        {
            get; set;
        }
        public string? TaskLevel
        {
            get; set;
        }

        public int? TaskId
        {
            get; set;
        }

        public string? BatchCode
        {
            get; set;
        }

        public decimal? CompletedQuantity
        {
            get; set;
        }

        public int? WorkStationId
        {
            get; set;
        }

        public string? WorkStationCode
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

        public string? EmployerCode
        {
            get; set;
        }

        public string? Remark
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
            profile.CreateMap<WorkOrderTaskExecuteLogUpdateDto, WorkOrderTaskExecuteLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
