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
    public class WorkCenterDto : IMapFrom<WorkCenter>
    {
        public int Id
        {
            get; set;
        }

        /// <summary>
        /// 工厂Id
        /// </summary>
        public int? FactoryId
        {
            get; set;
        }

        public int? WorkAreaId
        {
            get; set;
        }
        public string? WorkCenterCode
        {
            get; set;
        }

        public string? WorkCenterName
        {
            get; set;
        }

        public string? WorkCenterDesc
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public int? ParentId
        {
            get; set;
        }

        public bool IsGroup
        {
            get; set;
        }
        public int? LineStockId
        {
            get; set;
        }
        public DateTime? CreatedAt
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        }

        public DateTime? UpdatedAt
        {
            get; set;
        }

        public string? UpdateBy
        {
            get; set;
        }

        public bool IsDelete
        {
            get; set;
        }
    }

    public class WorkCenterCreateDto : IMapFrom<WorkCenter>
    {
        /// <summary>
        /// 工厂Id
        /// </summary>
        public int? FactoryId
        {
            get; set;
        }

        public int? WorkAreaId 
        { 
            get; set;
        }
        public string? WorkCenterCode
        {
            get; set;
        }

        public string? WorkCenterName
        {
            get; set;
        }

        public string? WorkCenterDesc
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public int? ParentId
        {
            get; set;
        }

        public bool IsGroup
        {
            get; set;
        }


        public string? CreatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkCenterCreateDto, WorkCenter>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkCenterUpdateDto : IMapFrom<WorkCenter>
    {
        public int? WorkAreaId
        {
            get; set;
        }

        public string? WorkCenterCode
        {
            get; set;
        }

        public string? WorkCenterName
        {
            get; set;
        }

        public string? WorkCenterDesc
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public int? ParentId
        {
            get; set;
        }

        public bool IsGroup
        {
            get; set;
        }

        public DateTime? UpdatedAt
        {
            get; set;
        }

        public string? UpdateBy
        {
            get; set;
        }

        public bool IsDelete
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkCenterUpdateDto, WorkCenter>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }
}
