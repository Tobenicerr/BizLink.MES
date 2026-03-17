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
    public class WorkTaskCategoryDto : IMapFrom<WorkTaskCategory>
    {
        public int Id
        {
            get; set;
        }

        public string? CategoryCode
        {
            get; set;
        }

        public string? CategoryName
        {
            get; set;
        }

        public string? CategoryDescription
        {
            get; set;
        }

        public string? StepType
        {
            get; set;
        }

        public bool IsMilestone { get; set; }

        public bool IsSapPrevTrigger { get; set; }

        public bool IsSapCurrentTrigger { get; set; }

        public int SortNo { get; set; }

        public bool IsActive
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
            profile.CreateMap<WorkTaskCategory, WorkTaskCategoryDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkTaskCategoryCreateDto : IMapFrom<WorkTaskCategory>
    {
        public string? CategoryCode
        {
            get; set;
        }

        public string? CategoryName
        {
            get; set;
        }

        public string? StepType
        {
            get; set;
        }

        public bool IsMilestone { get; set; }

        public bool IsSapPrevTrigger { get; set; }

        public bool IsSapCurrentTrigger { get; set; }

        public int SortNo { get; set; }

        public string? CategoryDescription
        {
            get; set;
        }
        public string? CreatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkTaskCategoryCreateDto, WorkTaskCategory>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkTaskCategoryUpdateDto : IMapFrom<WorkTaskCategory>
    {
        public int Id
        {
            get; set;
        }

        public string? CategoryCode
        {
            get; set;
        }

        public string? CategoryName
        {
            get; set;
        }

        public string? CategoryDescription
        {
            get; set;
        }

        public string? StepType
        {
            get; set;
        }

        public bool IsMilestone { get; set; }

        public bool IsSapPrevTrigger { get; set; }

        public bool IsSapCurrentTrigger { get; set; }

        public int SortNo { get; set; }
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
            profile.CreateMap<WorkTaskCategoryUpdateDto, WorkTaskCategory>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
