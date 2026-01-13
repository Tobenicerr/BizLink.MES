using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class AutoStockOutDto : IMapFrom<V_AutoStockOut>
    {
        public string? WorkOrderNo
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        public decimal? Quantity
        {
            get; set;
        }


        public decimal? PickingQuantity
        {
            get; set;
        }
        public decimal? LastQuantity
        {
            get; set;
        }

        public decimal? UseageQuantity
        {
            get; set;
        }

        public string? Status
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
            profile.CreateMap<V_AutoStockOut, AutoStockOutDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }
    public class AutoStockOutCreateDto : IMapFrom<V_AutoStockOut>
    {
    }

    public class AutoStockOutUpdateDto : IMapFrom<V_AutoStockOut>
    {
    }
}
