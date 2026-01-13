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
    public class WiDocumentDto : IMapFrom<WiDocument>
    {
        public int Id
        {
            get; set;
        }
        public int? FactoryId
        {
            get; set;
        }

        public string? FactoryCode
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
        public string? DocVersion
        {
            get; set;
        }
        public string? DocumentName
        {
            get; set;
        }
        public string? DocumentPath
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }
        public string? Remark
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

        public DateTime? ApprovedOn
        {
            get; set;
        }

        public string? ApprovedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WiDocument, WiDocumentDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WiDocumentCreateDto : IMapFrom<WiDocument>
    {

        public int? FactoryId
        {
            get; set;
        }

        public string? FactoryCode
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
        public string? DocVersion
        {
            get; set;
        }
        public string? DocumentName
        {
            get; set;
        }
        public string? DocumentPath
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }
        public string? Remark
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WiDocumentCreateDto, WiDocument>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WiDocumentUpdateDto : IMapFrom<WiDocument>
    {
        public int Id
        {
            get; set;
        }
        public int? FactoryId
        {
            get; set;
        }

        public string? FactoryCode
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
        public string? DocVersion
        {
            get; set;
        }
        public string? DocumentName
        {
            get; set;
        }
        public string? DocumentPath
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }
        public string? Remark
        {
            get; set;
        }

        public DateTime? ApprovedOn
        {
            get; set;
        }

        public string? ApprovedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WiDocumentUpdateDto, WiDocument>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
