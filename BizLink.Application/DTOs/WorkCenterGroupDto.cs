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
    public class WorkCenterGroupDto :IMapFrom<WorkCenterGroup>
    {
        public int Id
        {
            get; set;
        }

        public int? FactoryId
        {
            get; set;
        }

        public string? GroupCode
        {
            get; set;
        }


        public string? GroupName
        {
            get; set;
        }


        public string? GroupDesc
        {
            get; set;
        }


        public string? GroupType
        {
            get; set;
        }



        public string? Status
        {
            get; set;
        }



        public bool IsDelete
        {
            get; set;
        } = false;



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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkCenterGroup, WorkCenterGroupDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkCenterGroupCreateDto : IMapFrom<WorkCenterGroup>
    {

        public int? FactoryId
        {
            get; set;
        }

        public string? GroupCode
        {
            get; set;
        }


        public string? GroupName
        {
            get; set;
        }


        public string? GroupDesc
        {
            get; set;
        }


        public string? GroupType
        {
            get; set;
        }



        public string? Status
        {
            get; set;
        }



        public bool IsDelete
        {
            get; set;
        } = false;





        public string? CreatedBy
        {
            get; set;
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkCenterGroupCreateDto, WorkCenterGroup>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkCenterGroupUpdateDto : IMapFrom<WorkCenterGroup>
    {
        public int Id
        {
            get; set;
        }

        public int? FactoryId
        {
            get; set;
        }

        public string? GroupCode
        {
            get; set;
        }


        public string? GroupName
        {
            get; set;
        }


        public string? GroupDesc
        {
            get; set;
        }


        public string? GroupType
        {
            get; set;
        }



        public string? Status
        {
            get; set;
        }



        public bool IsDelete
        {
            get; set;
        } = false;





        public DateTime? UpdatedAt
        {
            get; set;
        }

        public string? UpdateBy
        {
            get; set;
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkCenterGroupUpdateDto, WorkCenterGroup>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }


}
