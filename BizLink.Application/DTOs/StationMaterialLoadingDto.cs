using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class StationMaterialLoadingDto : IMapFrom<StationMaterialLoading>
    {
        public int Id { get; set; }

        public int FactoryId { get; set; }


        public int WorkStationId { get; set; }

        public string WorkStationCode { get; set; }

        public int LineSideInventoryId { get; set; }

        public string MaterialCode { get; set; }


        public string MaterialDesc { get; set; }

        public string BatchCode { get; set; }

        public string BarCode { get; set; }

        public decimal Quantity { get; set; }

        public decimal LastQuantity { get; set; }

        public string BaseUnit { get; set; }

        public string Status { get; set; }

        public SupplyMode SupplyMode { get; set; }

        public long Version { get; set; }

        public string? Remark { get; set; }


        public string? BoundWorkOrderNo { get; set; }

        public DateTime LoadedTime { get; set; }

        public string LoadedBy { get; set; }
        public DateTime? UnloadedTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StationMaterialLoading, StationMaterialLoadingDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class StationMaterialLoadingCreateDto : IMapFrom<StationMaterialLoading>
    {

        public int FactoryId { get; set; }


        public int WorkStationId { get; set; }

        public string WorkStationCode { get; set; }

        public int LineSideInventoryId { get; set; }

        public string MaterialCode { get; set; }


        public string MaterialDesc { get; set; }

        public string BatchCode { get; set; }

        public string BarCode { get; set; }

        public decimal Quantity { get; set; }


        public string BaseUnit { get; set; }

        public string? BoundWorkOrderNo { get; set; }

        public string LoadedBy { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StationMaterialLoadingCreateDto, StationMaterialLoading>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class StationMaterialLoadingUpdateDto : IMapFrom<StationMaterialLoading>
    {
        public int Id { get; set; }

        public decimal LastQuantity { get; set; }

        public string Status { get; set; }

        public SupplyMode SupplyMode { get; set; }

        public long Version { get; set; }

        public string? Remark { get; set; }


        public string? BoundWorkOrderNo { get; set; }

        public DateTime? UnloadedTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<StationMaterialLoadingUpdateDto, StationMaterialLoading>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
