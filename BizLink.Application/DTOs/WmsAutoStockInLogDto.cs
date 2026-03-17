using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class WmsAutoStockInLogDto : IMapFrom<WmsAutoStockInLog>
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
            profile.CreateMap<WmsAutoStockInLog, WmsAutoStockInLogDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WmsAutoStockInLogCreateDto : IMapFrom<WmsAutoStockInLog>
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
            profile.CreateMap<WmsAutoStockInLogCreateDto, WmsAutoStockInLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WmsAutoStockInLogUpdateDto : IMapFrom<WmsAutoStockInLog>
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
            profile.CreateMap<WmsAutoStockInLogUpdateDto, WmsAutoStockInLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
