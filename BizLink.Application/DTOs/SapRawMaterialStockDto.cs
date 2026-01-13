using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Attributes;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class SapRawMaterialStockDto : IMapFrom<SapRawMaterialStock>
    {
        public string? MaterialCode
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }

        public string? LocationCode
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }
        public string? BatchCode
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }

        public string? BatchCodeNew
        {
            get; set;
        }

        public string? Remark
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SapRawMaterialStock, SapRawMaterialStockDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
