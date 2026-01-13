using AutoMapper;
using BizLink.MES.Application.Mappings;
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
    public class MaterialViewDto : IMapFrom<V_Material>
    {
        public int Id
        {
            get; set;
        }

        public string FactoryCode
        {
            get; set;
        }


        public string MaterialCode
        {
            get; set;
        }


        public string MaterialName
        {
            get; set;
        }


        public string BaseUnit
        {
            get; set;
        }

        public int? ConsumeType
        {
            get; set;
        }

        public int? LabelId
        {
            get; set;
        }

        public string? LabelName
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<V_Material, MaterialViewDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
