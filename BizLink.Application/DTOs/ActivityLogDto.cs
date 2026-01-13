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
    public class ActivityLogDto :IMapFrom<ActivityLog>
    {
        public int Id
        {
            get; set;
        }

        public int? UserId
        {
            get; set;
        }
        public string? UserName
        {
            get; set;
        }

        public string? LogType
        {
            get; set;
        } // "Action", "PageView", "Error"

        public string? LogContent
        {
            get; set;
        }
        public string? Details
        {
            get; set;
        }
        public DateTime? Timestamp { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ActivityLog, ActivityLogDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class ActivityLogCreateDto : IMapFrom<ActivityLog>
    {

        public int? UserId
        {
            get; set;
        }
        public string? UserName
        {
            get; set;
        }

        public string? LogType
        {
            get; set;
        } // "Action", "PageView", "Error"

        public string? LogContent
        {
            get; set;
        }
        public string? Details
        {
            get; set;
        }
        public DateTime? Timestamp { get; set; } = DateTime.Now;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ActivityLogCreateDto, ActivityLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }


    public class ActivityLogUpdateDto : IMapFrom<ActivityLog>
    {

    }
}
