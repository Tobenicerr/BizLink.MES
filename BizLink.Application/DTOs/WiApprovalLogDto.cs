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
    public class WiApprovalLogDto :IMapFrom<WiApprovalLog>
    {
        public int Id
        {
            get; set;
        }

        public int? DocumentId
        {
            get; set;
        }

        public string? Operator
        {
            get; set;
        }

        public string? ActionType
        {
            get; set;
        }

        public string? Comment
        {
            get; set;
        }


        public DateTime? LogTime
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WiApprovalLog, WiApprovalLogDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WiApprovalLogCreateDto : IMapFrom<WiApprovalLog>
    {

        public int? DocumentId
        {
            get; set;
        }

        public string? Operator
        {
            get; set;
        }

        public string? ActionType
        {
            get; set;
        }

        public string? Comment
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WiApprovalLogCreateDto, WiApprovalLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WiApprovalLogUpdateDto : IMapFrom<WiApprovalLog>
    {
        public int Id
        {
            get; set;
        }

        public int? DocumentId
        {
            get; set;
        }

        public string? Operator
        {
            get; set;
        }

        public string? ActionType
        {
            get; set;
        }

        public string? Comment
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WiApprovalLogUpdateDto, WiApprovalLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
