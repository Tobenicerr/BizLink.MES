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
    public class WorkOrderKittingItemDto : IMapFrom<WorkOrderKittingItem>
    {

        public int Id { get; set; }

        public int WorkOrderId { get; set; }

        public string WorkOrderNo { get; set; }

        public int WorkOrderProcessId { get; set; }

        public string Operation { get; set; }

        public string BomItem { get; set; }

        public int ReservationItem { get; set; }

        public string MaterialCode { get; set; }

        public string? MaterialDesc { get; set; }

        public string BaseUnit { get; set; }

        public string? BatchCode { get; set; }

        public string? BarCode { get; set; }

        public decimal Quantity { get; set; }

        public string? ConsumptionType { get; set; }

        public int? MaterialConsumeType { get; set; }

        public string? ConsumptionRemark { get; set; }

        public string? Remark { get; set; }

        public string? Status { get; set; }

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

        public string? UpdateBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderKittingItem, WorkOrderKittingItemDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderKittingItemCreateDto : IMapFrom<WorkOrderKittingItem>
    {

        public int WorkOrderId { get; set; }

        public string WorkOrderNo { get; set; }

        public int WorkOrderProcessId { get; set; }

        public string Operation { get; set; }

        public string BomItem { get; set; }

        public int ReservationItem { get; set; }

        public string MaterialCode { get; set; }

        public string? MaterialDesc { get; set; }

        public string BaseUnit { get; set; }

        public string? BatchCode { get; set; }

        public string? BarCode { get; set; }

        public decimal Quantity { get; set; }

        public string? ConsumptionType { get; set; }

        public int? MaterialConsumeType { get; set; }

        public string? ConsumptionRemark { get; set; }

        public string? Remark { get; set; }


        public string? CreatedBy
        {
            get; set;
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderKittingItemCreateDto, WorkOrderKittingItem>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderKittingItemUpdateDto : IMapFrom<WorkOrderKittingItem>
    {

        public int Id { get; set; }

        public string? BatchCode { get; set; }

        public string? BarCode { get; set; }

        public decimal Quantity { get; set; }

        public string? ConsumptionType { get; set; }

        public int? MaterialConsumeType { get; set; }

        public string? ConsumptionRemark { get; set; }

        public string? Remark { get; set; }

        public string? Status { get; set; }

        public DateTime? UpdatedOn
        {
            get; set;
        }

        public string? UpdateBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderKittingItemUpdateDto, WorkOrderKittingItem>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
