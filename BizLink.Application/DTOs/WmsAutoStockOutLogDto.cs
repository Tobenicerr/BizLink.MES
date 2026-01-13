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
    public class WmsAutoStockOutLogDto : IMapFrom<WmsAutoStockOutLog>
    {
        public int Id
        {
            get; set;
        }


        public string? BillNo
        {
            get; set;
        }


        public string? MaterialCode
        {
            get; set;
        }


        public string? BatchCode
        {
            get; set;
        }


        public decimal Quantity
        {
            get; set;
        }


        public string? BarCode
        {
            get; set;
        }


        public string? BarCodeNew
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }


        public int ProcessFlag
        {
            get; set;
        }

        public string? Message
        {
            get; set;
        }


        public DateTime? CreateTime
        {
            get; set;
        }

        public DateTime? UpdateTime
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WmsAutoStockOutLog, WmsAutoStockOutLogDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WmsAutoStockOutLogCreateDto : IMapFrom<WmsAutoStockOutLog>
    {

        public string? BillNo
        {
            get; set;
        }


        public string? MaterialCode
        {
            get; set;
        }


        public string? BatchCode
        {
            get; set;
        }


        public decimal Quantity
        {
            get; set;
        }


        public string? BarCode
        {
            get; set;
        }


        public string? BarCodeNew
        {
            get; set;
        }

        public string? FactoryCode
        {
            get; set;
        }


        public int ProcessFlag
        {
            get; set;
        }

        public string? Message
        {
            get; set;
        }


        public DateTime? CreateTime
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WmsAutoStockOutLogCreateDto, WmsAutoStockOutLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }

    public class WmsAutoStockOutLogUpdateDto : IMapFrom<WmsAutoStockOutLog>
    {
        public int Id
        {
            get; set;
        }

        public int ProcessFlag
        {
            get; set;
        }

        public string? Message
        {
            get; set;
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WmsAutoStockOutLogUpdateDto, WmsAutoStockOutLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
