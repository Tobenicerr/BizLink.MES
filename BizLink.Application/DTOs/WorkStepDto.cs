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
    public class WorkStepDto: IMapFrom<WorkStep>
    {
        public int Id
        {
            get; set;
        }

        public string? StepCode
        {
            get; set;
        }

        public string? StepName
        {
            get; set;
        }

        public string? StepType
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public bool? IsDelete
        {
            get; set;
        }

        public DateTime? CreateOn
        {
            get; set;
        }

        public string? CreateBy
        {
            get; set;
        } // 创建人

        public DateTime? UpdateOn
        {
            get; set;
        } // 更新时间

        public string? UpdateBy
        {
            get; set;
        } // 更新人

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkStep, WorkStepDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkStepCreateDto : IMapFrom<WorkStep>
    {
        public string? StepCode
        {
            get; set;
        }

        public string? StepName
        {
            get; set;
        }

        public string? StepType
        {
            get; set;
        }


        public string? CreateBy
        {
            get; set;
        } // 创建人


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkStepDto, WorkStepCreateDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkStepUpdateDto : IMapFrom<WorkStep>
    {

        public int Id
        {
            get; set;
        }

        public string? StepCode
        {
            get; set;
        }

        public string? StepName
        {
            get; set;
        }

        public string? StepType
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public bool? IsDelete
        {
            get; set;
        }

        public DateTime? UpdateOn
        {
            get; set;
        } // 更新时间

        public string? UpdateBy
        {
            get; set;
        } // 更新人


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkStepDto, WorkStepUpdateDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
