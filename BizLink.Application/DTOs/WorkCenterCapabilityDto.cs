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
    public class WorkCenterCapabilityDto : IMapFrom<WorkCenterCapability>
    {
        public int Id
        {
            get; set;
        }

        public int WorkCenterGroupId
        {
            get; set;
        }

        public int TaskCategoryId
        {
            get; set;
        }

        public string? TaskCategoryCode
        {
            get; set;
        }

        public decimal? StandardCapacity
        {
            get; set;
        }

        public int Priority
        {
            get; set;
        }

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
            profile.CreateMap<WorkCenterCapability, WorkCenterCapabilityDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkCenterCapabilityCreateDto : IMapFrom<WorkCenterCapability>
    {

        public int WorkCenterGroupId
        {
            get; set;
        }

        public int TaskCategoryId
        {
            get; set;
        }

        public string? TaskCategoryCode
        {
            get; set;
        }

        public decimal? StandardCapacity
        {
            get; set;
        }

        public int Priority
        {
            get; set;
        }


        public string? CreatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkCenterCapabilityCreateDto, WorkCenterCapability>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkCenterCapabilityUpdateDto : IMapFrom<WorkCenterCapability>
    {
        public int Id
        {
            get; set;
        }

        public int WorkCenterGroupId
        {
            get; set;
        }

        public int TaskCategoryId
        {
            get; set;
        }

        public string? TaskCategoryCode
        {
            get; set;
        }

        public decimal? StandardCapacity
        {
            get; set;
        }

        public int Priority
        {
            get; set;
        }

        public bool IsActive
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
            profile.CreateMap<WorkCenterCapability, WorkCenterCapabilityDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
