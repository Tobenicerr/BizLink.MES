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
    public class AutoMaterialStockDto : IMapFrom<V_AutoMaterialStock>
    {
        public string MaterialCode
        {
            get; set;
        }

        public string MaterialDesc
        {
            get; set;
        }

        public string BaseUnit
        {
            get; set;
        } = "ST";

        public decimal Quantity
        {
            get; set;
        }

        public string BatchCode
        {
            get; set;
        }

        public string BarCode
        {
            get; set;
        }

        public string LocationCode
        {
            get; set;
        }

        public bool IsLocked
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<V_AutoMaterialStock, AutoMaterialStockDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class AutoMaterialStockCreateDto : IMapFrom<V_AutoMaterialStock>
    {
    }

    public class AutoMaterialStockUpdateDto : IMapFrom<V_AutoMaterialStock>
    {
    }
}
