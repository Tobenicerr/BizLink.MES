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
    public class SapOrderScrapDeclarationDto :IMapFrom<SapOrderScrapDeclaration>
    {
        public int Id
        {
            get; set;
        }
        public string? FactoryCode
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        }

        public string? OperationNo
        {
            get; set;
        }

        public string? WorkCenterCode
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        public string? SuperiorOrder
        {
            get; set;
        }

        public string? LeadingOrder
        {
            get; set;
        }

        public string? LeadingMaterial
        {
            get; set;
        }
        public string? ScrapBomItem
        {
            get; set;
        }

        public string? ScrapMaterialType
        {
            get; set;
        }

        public string? ScrapMaterialCode
        {
            get; set;
        }

        public string? ScrapMaterialDesc
        {
            get; set;
        }

        public decimal? RequireQuantity
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public decimal? ComponentScrap
        {
            get; set;
        }

        public decimal? ScrapQuantity
        {
            get; set;
        }

        public string? ScrapReason
        {
            get; set;
        }

        public string? Remark
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

        public string? UpdatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SapOrderScrapDeclaration, SapOrderScrapDeclarationDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class SapOrderScrapDeclarationCreateDto : IMapFrom<SapOrderScrapDeclaration>
    {

        public string? FactoryCode
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        }

        public string? OperationNo
        {
            get; set;
        }

        public string? WorkCenterCode
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        public string? SuperiorOrder
        {
            get; set;
        }

        public string? LeadingOrder
        {
            get; set;
        }

        public string? LeadingMaterial
        {
            get; set;
        }
        public string? ScrapBomItem
        {
            get; set;
        }

        public string? ScrapMaterialType
        {
            get; set;
        }

        public string? ScrapMaterialCode
        {
            get; set;
        }

        public string? ScrapMaterialDesc
        {
            get; set;
        }

        public decimal? RequireQuantity
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public decimal? ComponentScrap
        {
            get; set;
        }

        public decimal? ScrapQuantity
        {
            get; set;
        }

        public string? ScrapReason
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
            profile.CreateMap<SapOrderScrapDeclarationCreateDto, SapOrderScrapDeclaration>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }


    public class SapOrderScrapDeclarationUpdateDto : IMapFrom<SapOrderScrapDeclaration>
    {
        public int Id
        {
            get; set;
        }
     
        public decimal? ScrapQuantity
        {
            get; set;
        }

        public string? ScrapReason
        {
            get; set;
        }

        public string? Remark
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

        public string? UpdatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SapOrderScrapDeclarationUpdateDto, SapOrderScrapDeclaration>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
