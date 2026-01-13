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
    public class WorkStationDto : IMapFrom<WorkStation>
    {
        public int Id
        {
            get; set;
        }

        public int? FactoryId
        {
            get; set;
        }

        public int? WorkAreaId
        {
            get; set;
        }

        public int WorkCenterId
        {
            get; set;
        }

        public string? WorkStationCode
        {
            get; set;
        }


        public string? WorkStationName
        {
            get; set;
        }



        public string? Status
        {
            get; set;
        }



        public string? PrintType
        {
            get; set;
        }



        public string? PrinterName
        {
            get; set;
        }

        public bool IsStartStep
        {
            get; set;
        }

        public bool IsEndStep
        {
            get; set;
        }

        public bool IsDelete
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
    }

    public class WorkStationCreateDto : IMapFrom<WorkStation>
    {

        public int WorkCenterId
        {
            get; set;
        }

        public int? FactoryId
        {
            get; set;
        }

        public int? WorkAreaId
        {
            get; set;
        }
        public string? WorkStationCode
        {
            get; set;
        }


        public string? WorkStationName
        {
            get; set;
        }



        public string? Status
        {
            get; set;
        }



        public string? PrintType
        {
            get; set;
        }



        public string? PrinterName
        {
            get; set;
        }

        public bool IsStartStep
        {
            get; set;
        }

        public bool IsEndStep
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkStationCreateDto, WorkStation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }

    public class WorkStationUpdateDto : IMapFrom<WorkStation>
    {


        public string? WorkStationName
        {
            get; set;
        }


        public string? Status
        {
            get; set;
        }



        public string? PrintType
        {
            get; set;
        }



        public string? PrinterName
        {
            get; set;
        }

        public bool IsDelete
        {
            get; set;
        }

        public bool IsStartStep
        {
            get; set;
        }

        public bool IsEndStep
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
            profile.CreateMap<WorkStationUpdateDto, WorkStation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
