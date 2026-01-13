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
    public class WorkCenterGroupStepConfigDto : IMapFrom<WorkCenterGroupStepConfig>
    {
        public int Id
        {
            get; set;
        }

        public int GroupId
        {
            get; set;
        }

        public int StepId
        {
            get; set;
        }

        public string? StepCategory
        {
            get; set;
        }

        public int StepSequence
        {
            get; set;
        }
        public string? IsStartStep
        {
            get; set;
        }
        public string? IsEndStep
        {
            get; set;
        }

        public string? IsCriticalPath
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
            profile.CreateMap<WorkCenterGroupStepConfig, WorkCenterGroupStepConfigDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkCenterGroupStepConfigCreateDto : IMapFrom<WorkCenterGroupStepConfig>
    {
        public int GroupId
        {
            get; set;
        }

        public int StepId
        {
            get; set;
        }

        public string? StepCategory
        {
            get; set;
        }

        public int StepSequence
        {
            get; set;
        }
        public string? IsStartStep
        {
            get; set;
        }
        public string? IsEndStep
        {
            get; set;
        }

        public string? IsCriticalPath
        {
            get; set;
        }

        public string? CreateBy
        {
            get; set;
        } // 创建人

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkCenterGroupStepConfigCreateDto, WorkCenterGroupStepConfig>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }


    public class WorkCenterGroupStepConfigUpdateDto : IMapFrom<WorkCenterGroupStepConfig>
    {
        public int Id
        {
            get; set;
        }

        public int GroupId
        {
            get; set;
        }

        public int StepId
        {
            get; set;
        }

        public string? StepCategory
        {
            get; set;
        }

        public int StepSequence
        {
            get; set;
        }
        public string? IsStartStep
        {
            get; set;
        }
        public string? IsEndStep
        {
            get; set;
        }

        public string? IsCriticalPath
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
            profile.CreateMap<WorkCenterGroupStepConfigUpdateDto, WorkCenterGroupStepConfig>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
