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
    public class FinishedGoodsReceiptDetailDto : IMapFrom<FinishedGoodsReceiptDetail>
    {
        public int Id
        {
            get;
            set;
        }

        public int ReceiptId
        {
            get;
            set;
        }

        public string? SapRequestJson
        {
            get;
            set;
        }

        public string? SapResponseJson
        {
            get;
            set;
        }

        public string? PrintTemplate
        {
            get;
            set;
        }

        public string? PrintData
        {
            get;
            set;
        }

        public DateTime? CreatedOn
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        } // 创建人

        public DateTime? UpdatedOn
        {
            get; set;
        } // 更新时间

        public string? UpdatedBy
        {
            get; set;
        } // 更新人


        public void Mapping(Profile profile)
        {
            profile.CreateMap<FinishedGoodsReceiptDetail, FinishedGoodsReceiptDetailDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class FinishedGoodsReceiptDetailCreateDto : IMapFrom<FinishedGoodsReceiptDetail>
    {

        public int ReceiptId
        {
            get;
            set;
        }

        public string? SapRequestJson
        {
            get;
            set;
        }

        public string? SapResponseJson
        {
            get;
            set;
        }

        public string? PrintTemplate
        {
            get;
            set;
        }

        public string? PrintData
        {
            get;
            set;
        }


        public string? CreatedBy
        {
            get; set;
        } // 创建人


        public void Mapping(Profile profile)
        {
            profile.CreateMap<FinishedGoodsReceiptDetailCreateDto, FinishedGoodsReceiptDetail>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class FinishedGoodsReceiptDetailUpdateDto : IMapFrom<FinishedGoodsReceiptDetail>
    {
        public int Id
        {
            get;
            set;
        }


        public string? SapRequestJson
        {
            get;
            set;
        }

        public string? SapResponseJson
        {
            get;
            set;
        }

        public string? PrintTemplate
        {
            get;
            set;
        }

        public string? PrintData
        {
            get;
            set;
        }


        public DateTime? UpdatedOn
        {
            get; set;
        } // 更新时间

        public string? UpdatedBy
        {
            get; set;
        } // 更新人


        public void Mapping(Profile profile)
        {
            profile.CreateMap<FinishedGoodsReceiptDetailUpdateDto, FinishedGoodsReceiptDetail>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
