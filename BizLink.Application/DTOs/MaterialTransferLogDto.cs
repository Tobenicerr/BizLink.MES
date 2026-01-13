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
    public class MaterialTransferLogDto : IMapFrom<MaterialTransferLog>
    {
        public int Id
        {
            get; set;
        }

        public string? TransferNo
        {
            get; set;
        }


        public string? TransferType
        {
            get; set;
        }


        public DateTime? PostingDate
        {
            get; set;
        }


        public DateTime? DocumentDate
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


        public string? MaterialCode
        {
            get; set;
        }


        public string? FactoryCode
        {
            get; set;
        }


        public string? FromLocationCode
        {
            get; set;
        }


        public string? BatchCode
        {
            get; set;
        }


        public string? MovementType
        {
            get; set;
        }

        public string? MovementReason
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


        public string? ReceivingMaterialCode
        {
            get; set;
        }

        /// <summary>
        /// 关联的库存记录ID
        /// </summary>

        public int? FromStockId
        {
            get; set;
        }

        public string? ToLocationCode
        {
            get; set;
        }
        public string? ToFactoryCode
        {
            get; set;
        }

        public string? ReceivingBatchCode
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>

        public string? Remark
        {
            get; set;
        }

        public string? CostCenterCode
        {
            get; set;
        }

        public string? Message
        {
            get; set;
        }


        public string? MessageType
        {
            get; set;
        }

        public int? StockLogId
        {
            get; set;
        }
        public DateTime CreatedAt
        {
            get; set;
        }


        public string? CreatedBy
        {
            get; set;
        }
        public DateTime UpdatedAt
        {
            get; set;
        }

        public string? UpdatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            var map = profile.CreateMap<MaterialTransferLog, MaterialTransferLogDto>();
            map.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            profile.CreateMap<MaterialTransferLogDto, MaterialTransferLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class MaterialTransferLogCreateDto : IMapFrom<MaterialTransferLog>
    {

        public string? TransferNo
        {
            get; set;
        }


        public string? TransferType
        {
            get; set;
        }


        public DateTime? PostingDate
        {
            get; set;
        }


        public DateTime? DocumentDate
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


        public string? MaterialCode
        {
            get; set;
        }


        public string? FactoryCode
        {
            get; set;
        }


        public string? FromLocationCode
        {
            get; set;
        }


        public string? BatchCode
        {
            get; set;
        }


        public string? MovementType
        {
            get; set;
        }

        public string? MovementReason
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


        public string? ReceivingMaterialCode
        {
            get; set;
        }

        /// <summary>
        /// 关联的库存记录ID
        /// </summary>

        public int? FromStockId
        {
            get; set;
        }

        public string? ToLocationCode
        {
            get; set;
        }

        public string? ToFactoryCode
        {
            get; set;
        }


        public string? ReceivingBatchCode
        {
            get; set;
        }

        public int? StockLogId
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>

        public string? Remark
        {
            get; set;
        }

        public string? CostCenterCode
        {
            get; set;
        }
        public string? CreatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MaterialTransferLogCreateDto, MaterialTransferLog>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class MaterialTransferLogUpdateDto : IMapFrom<MaterialTransferLog>
    {
        public int Id
        {
            get; set;
        }

        public string? TransferNo
        {
            get; set;
        }
        public DateTime? PostingDate
        {
            get; set;
        }


        public DateTime? DocumentDate
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


        public string? MaterialCode
        {
            get; set;
        }



        public string? BatchCode
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


        public string? ReceivingMaterialCode
        {
            get; set;
        }

        /// <summary>
        /// 关联的库存记录ID
        /// </summary>

        public int? FromStockId
        {
            get; set;
        }


        public string? ReceivingBatchCode
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>

        public string? Remark
        {
            get; set;
        }

        public string? CostCenterCode
        {
            get; set;
        }

        public string? Message
        {
            get; set;
        }


        public string? MessageType
        {
            get; set;
        }
        public DateTime UpdatedAt
        {
            get; set;
        }

        public string? UpdatedBy
        {
            get; set;
        }



        public void Mapping(Profile profile)
        {
            // 1. 配置 Entity -> DTO (跳过 null)
            var map = profile.CreateMap<MaterialTransferLogUpdateDto, MaterialTransferLog>();
            map.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // 2. 配置 DTO -> Entity (也要跳过 null)
            //    在 .ReverseMap() 之后，继续链式调用 ForAllMembers
            map.ReverseMap()
               .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
