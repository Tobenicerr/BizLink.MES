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
    public class FinishedGoodsReceiptDto : IMapFrom<FinishedGoodsReceipt>
    {
        public int Id
        {
            get;
            set;
        }

        public int WorkOrderId
        {
            get;
            set;
        }

        public int FactoryId
        {
            get;
            set;
        }

        public string? FactoryCode
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        } // 物料代码

        public int WorkOrderProcessId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get;
            set;
        } // 物料代码

        public string? MaterialDesc
        {
            get;
            set;
        } // 物料名称

        public decimal? Quantity 
        { 
            get; 
            set; 
        } // 数量

        public string? BaseUnit 
        { 
            get; 
            set; 
        } // 单位

        public string? StorageLocation
        {
            get;
            set;
        } // 库存地点

        public string? WorkCenterCode
        {
            get; set;
        }

        public string? SapTransferNo
        {
            get;
            set;
        }

        public string? SapStatus
        {
            get;
            set;
        }

        public string? SapBatchNo
        {
            get;
            set;
        }

        public string? SapMessageType
        {
            get;
            set;
        }

        public string? SapMessage
        {
            get;
            set;
        }

        public bool? IsPrinted
        {
            get;
            set;
        }

        public int? PrintCount 
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
            var map = profile.CreateMap<FinishedGoodsReceipt, FinishedGoodsReceiptDto>();
            map.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            profile.CreateMap<FinishedGoodsReceiptDto, FinishedGoodsReceipt>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class FinishedGoodsReceiptCreateDto : IMapFrom<FinishedGoodsReceipt>
    {

        public int WorkOrderId
        {
            get;
            set;
        }

        public int FactoryId
        {
            get;
            set;
        }

        public string? FactoryCode
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        } // 物料代码

        public int WorkOrderProcessId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get;
            set;
        } // 物料代码

        public string? MaterialDesc
        {
            get;
            set;
        } // 物料名称

        public decimal? Quantity
        {
            get;
            set;
        } // 数量

        public string? BaseUnit
        {
            get;
            set;
        } // 单位

        public string? StorageLocation
        {
            get;
            set;
        } // 库存地点

        public string? WorkCenterCode
        {
            get; set;
        }
        public string? SapTransferNo
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
            profile.CreateMap<FinishedGoodsReceiptCreateDto, FinishedGoodsReceipt>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }

    public class FinishedGoodsReceiptUpdateDto : IMapFrom<FinishedGoodsReceipt>
    {
        public int Id
        {
            get;
            set;
        }

       
        public string? SapStatus
        {
            get;
            set;
        }

        public string? SapBatchNo
        {
            get;
            set;
        }

        public string? SapMessageType
        {
            get;
            set;
        }

        public string? SapMessage
        {
            get;
            set;
        }

        public bool? IsPrinted
        {
            get;
            set;
        }

        public int? PrintCount
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
    }

}
