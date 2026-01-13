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
    public class RawLinesideStockLogDto : IMapFrom<RawLinesideStockLog>
    {
        public int Id
        {
            get; set;
        }

        public int? RawLinesideStockId
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

        public InOutStatus InOutStatus
        {
            get; set;
        }
        public StockOperationType OperationType
        {
            get; set;
        }

        public decimal ChangeQuantity
        {
            get; set;
        }

        public decimal QuantityBefore
        {
            get; set;
        }

        public decimal QuantityAfter
        {
            get; set;
        }

        public string? SourceDocumentCode
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

        public string? BarCode
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? TransferReason
        {
            get; set;
        }

        public int LocationId
        {
            get; set;
        }

        public string? LocationCode
        {
            get; set;
        }

        public string? SapStatus
        {
            get; set;
        }
        public string? CreateBy
        {
            get; set;
        }

        public DateTime CreateOn { get; set; } = DateTime.Now;

        public string? Remark
        {
            get; set;
        }
    }

    public class RawLinesideStockLogCreateDto : IMapFrom<RawLinesideStockLog>
    {
        public int? RawLinesideStockId
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
        public InOutStatus InOutStatus
        {
            get; set;
        }
        public StockOperationType OperationType
        {
            get; set;
        }

        public decimal ChangeQuantity
        {
            get; set;
        }

        public decimal QuantityBefore
        {
            get; set;
        }

        public decimal QuantityAfter
        {
            get; set;
        }

        public string? SourceDocumentCode
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

        public string? BarCode
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? TransferReason
        {
            get; set;
        }

        public int LocationId
        {
            get; set;
        }

        public string? LocationCode
        {
            get; set;
        }

        public string? CreateBy
        {
            get; set;
        }

        public string? Remark
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RawLinesideStockLogCreateDto, RawLinesideStockLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class RawLinesideStockLogUpdateDto : IMapFrom<RawLinesideStockLog>
    {
        public int Id
        {
            get; set;
        }

        public string? SapStatus
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RawLinesideStockLogUpdateDto, RawLinesideStockLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
