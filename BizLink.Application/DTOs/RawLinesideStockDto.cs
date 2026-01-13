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
    public class RawLinesideStockDto : IMapFrom<RawLinesideStock>
    {
        public int Id
        {
            get; set;
        }

        public int FactoryId
        {
            get; set;
        }
        public int MaterialId
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
        public string? BaseUnit
        {
            get; set;
        }

        public string? BatchCode
        {
            get; set;
        }
        

        public string? BarCode
        {
            get; set;
        }
        

        public decimal? Quantity
        {
            get; set;
        }

        

        public decimal? LastQuantity
        {
            get; set;
        }

        
        public string? Status
        {
            get; set;
        }


        public string? SapStatus
        {
            get; set;
        }

        public int? LocationId
        {
            get; set;
        }

        
        public string? LocationCode
        {
            get; set;
        }

        public string? LocationDesc
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

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RawLinesideStock, RawLinesideStockDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }

    public class RawLinesideStockCreateDto : IMapFrom<RawLinesideStock>
    {

        public int FactoryId
        {
            get; set;
        }
        public int MaterialId
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

        public string? BaseUnit
        {
            get; set;
        }
        public string? BatchCode
        {
            get; set;
        }


        public string? BarCode
        {
            get; set;
        }


        public decimal? Quantity
        {
            get; set;
        }



        public decimal? LastQuantity
        {
            get; set;
        }

        public int? LocationId
        {
            get; set;
        }


        public string? LocationCode
        {
            get; set;
        }


        public string? LocationDesc
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RawLinesideStockCreateDto, RawLinesideStock>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }

    public class RawLinesideStockUpdateDto : IMapFrom<RawLinesideStock>
    {
        public int Id
        {
            get; set;
        }

        public decimal? LastQuantity
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        public string? SapStatus
        {
            get; set;
        }

        public int? LocationId
        {
            get; set;
        }


        public string? LocationCode
        {
            get; set;
        }


        public string? LocationDesc
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
            profile.CreateMap<RawLinesideStockUpdateDto, RawLinesideStock>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
