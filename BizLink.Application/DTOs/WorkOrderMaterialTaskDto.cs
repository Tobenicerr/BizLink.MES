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
    public class WorkOrderMaterialTaskDto : IMapFrom<WorkOrderMaterialTask>
    {
        public int Id
        {
            get; set;
        }
        public int? StepTaskId
        {
            get; set;
        }

        public int? SourceBomId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        public string? MaterialDesc
        {
            get; set;
        }

        public decimal? TargetQuantity
        {
            get; set;
        }

        public decimal? TargetValue
        {
            get; set;
        }

        public string? TargetUnit
        {
            get; set;
        }

        public string? ExtAttributes
        {
            get; set;
        }

        /// <summary>
        /// 任务子类型
        /// </summary>
        public string? TaskSubType
        {
            get; set;
        }

        /// <summary>
        /// 参考来源ID
        /// </summary>
        public int? RefSourceId
        {
            get; set;
        }

        /// <summary>
        /// 参考原因
        /// </summary>
        public string? RefReasonCode
        {
            get; set;
        }

        /// <summary>
        /// 任务优先级
        /// </summary>
        public int? Priority
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
            profile.CreateMap<WorkOrderMaterialTask, WorkOrderMaterialTaskDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderMaterialTaskCreateDto : IMapFrom<WorkOrderMaterialTask>
    {
        public int? StepTaskId
        {
            get; set;
        }

        public int? SourceBomId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        public string? MaterialDesc
        {
            get; set;
        }

        public decimal? TargetQuantity
        {
            get; set;
        }

        public decimal? TargetValue
        {
            get; set;
        }

        public string? TargetUnit
        {
            get; set;
        }

        public string? ExtAttributes
        {
            get; set;
        }

        /// <summary>
        /// 任务子类型
        /// </summary>
        public string? TaskSubType
        {
            get; set;
        }

        /// <summary>
        /// 参考来源ID
        /// </summary>
        public int? RefSourceId
        {
            get; set;
        }

        /// <summary>
        /// 参考原因
        /// </summary>
        public string? RefReasonCode
        {
            get; set;
        }

        /// <summary>
        /// 任务优先级
        /// </summary>
        public int? Priority
        {
            get; set;
        } = 0;

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
            profile.CreateMap<WorkOrderMaterialTaskCreateDto, WorkOrderMaterialTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderMaterialTaskUpdateDto : IMapFrom<WorkOrderMaterialTask>
    {
        public int Id
        {
            get; set;
        }
        public int? StepTaskId
        {
            get; set;
        }

        public int? SourceBomId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        public string? MaterialDesc
        {
            get; set;
        }

        public decimal? TargetQuantity
        {
            get; set;
        }

        public decimal? TargetValue
        {
            get; set;
        }

        public string? TargetUnit
        {
            get; set;
        }

        public string? ExtAttributes
        {
            get; set;
        }

        /// <summary>
        /// 任务子类型
        /// </summary>
        public string? TaskSubType
        {
            get; set;
        }

        /// <summary>
        /// 参考来源ID
        /// </summary>
        public int? RefSourceId
        {
            get; set;
        }

        /// <summary>
        /// 参考原因
        /// </summary>
        public string? RefReasonCode
        {
            get; set;
        }

        /// <summary>
        /// 任务优先级
        /// </summary>
        public int? Priority
        {
            get; set;
        } = 0;

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
            profile.CreateMap<WorkOrderMaterialTaskUpdateDto, WorkOrderMaterialTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }


}
