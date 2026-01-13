using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Attributes;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class MaterialDto : IMapFrom<Material>
    {
        public int Id
        {
            get; set;
        }
        public string? MaterialCode
        {
            get; set;
        }

        public string? MaterialDescription
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }

        public string? MaterialType
        {
            get; set;
        }

        public string? MaterialGroup
        {
            get; set;
        }

        public string? SpecialProcurement
        {
            get; set;
        }


        public string? BatchManagementRequired
        {
            get; set;
        }

        public string? ProcurementType
        {
            get; set;
        }

        public int? ConsumeType
        {
            get; set;
        }

        public string? IsCableMaterial
        {
            get; set;
        }

        public string? IsConsumption
        {
            get; set;
        }

        public bool IsActive
        {
            get; set;
        }

        public bool IsDelete { get; set; }

        public DateTime? ExpiredDate
        {
            get; set;
        }

        public string? CreateBy
        {
            get; set;
        } // 创建人
        public DateTime? CreateAt
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
            profile.CreateMap<Material, MaterialDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class MaterialCreateDto : IMapFrom<Material>
    {

        public string? MaterialCode
        {
            get; set;
        }

        public string? MaterialDescription
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }


        public int? ConsumeType
        {
            get; set;
        }

        public string? CreateBy
        {
            get; set;
        } // 创建人


        public void Mapping(Profile profile)
        {
            profile.CreateMap<MaterialCreateDto, Material>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class MaterialUpdateDto : IMapFrom<Material>
    {
        public int Id
        {
            get; set;
        }

        public string? MaterialDescription
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }

        

        public int? ConsumeType
        {
            get; set;
        }

      

        public bool IsDelete
        {
            get; set;
        }

        public bool IsActive
        {
            get; set;
        }
        public DateTime? ExpiredDate
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
            profile.CreateMap<MaterialUpdateDto, MaterialDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
