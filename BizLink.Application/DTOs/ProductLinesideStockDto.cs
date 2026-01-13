using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class ProductLinesideStockDto : IMapFrom<ProductLinesideStock>
    {
        public int Id
        {
            get; set;
        }


        public int? WorkOrderId
        {
            get; set;
        }
        public string? WorkOrderNo
        {
            get; set;
        }

        public string? BomItem
        {
            get; set;
        }

        public int? WorkOrderProcessId
        {
            get; set;
        }

        public string? Operation
        {
            get; set;
        }


        public int? WorkCenterId
        {
            get; set;
        }
        

        public string? WorkCenterCode
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

        public string? BaseUnit
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }


        public string? Remark
        {
            get; set;
        }

        public DateTime? CreatedAt
        {
            get; set;
        } = DateTime.Now;
        

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
    }

    public class ProductLinesideStockCreateDto : IMapFrom<ProductLinesideStock>
    {

        public int? WorkOrderId
        {
            get; set;
        }
        public string? WorkOrderNo
        {
            get; set;
        }

        public string? BomItem
        {
            get; set;
        }

        public int? WorkOrderProcessId
        {
            get; set;
        }

        public int? WorkCenterId
        {
            get; set;
        }


        public string? WorkCenterCode
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

        public string? BaseUnit
        {
            get; set;
        }



        public string? CreatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ProductLinesideStockCreateDto, ProductLinesideStock>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }

    public class ProductLinesideStockUpdateDto : IMapFrom<ProductLinesideStock>
    {

        public int Id
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

        public string? Status
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
            profile.CreateMap<ProductLinesideStockUpdateDto, ProductLinesideStock>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
