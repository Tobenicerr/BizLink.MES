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
    public class WorkOrderBomItemDto : IMapFrom<WorkOrderBomItem>
    {
        public int Id
        {
            get; set;
        }


        public int WorkOrderId
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

        public int? MaterialId
        {
            get; set;
        } // 物料代码

        public string? MaterialCode
        {
            get; set;
        } // 物料代码


        public string? MaterialDesc
        {
            get; set;
        } // 物料名称


        public decimal? RequiredQuantity
        {
            get; set;
        } // 需求数量


        public string? Unit
        {
            get; set;
        } // 单位


        public int? ReservationItem
        {
            get; set;
        } // 预留项目


        public decimal? ComponentScrap
        {
            get; set;
        } // 组件废料


        public string? BomItem
        {
            get; set;
        } // BOM项目


        public string? Operation
        {
            get; set;
        } // 操作


        public bool? MovementAllowed
        {
            get; set;
        } // 允许移动


        public bool? QuantityIsFixed
        {
            get; set;
        } // 数量已固定

        public int? ConsumeType
        {
            get; set;
        }

        public int? SyncWMSStatus
        {
            get; set;
        }

        public string? SuperMaterialCode
        {
            get; set;
        } // 操作
        public bool? IsActive
        {
            get; set;
        }

        public int? BomVersion
        {
            get; set;
        } // 工艺版本

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderBomItem, WorkOrderBomItemDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderBomItemCreateDto : IMapFrom<WorkOrderBomItem>
    {

        public int WorkOrderId
        {
            get; set;
        }

        public string WorkOrderNo
        {
            get; set;
        }
        public int WorkOrderProcessId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        } // 物料代码


        public string? MaterialDesc
        {
            get; set;
        } // 物料名称


        public decimal? RequiredQuantity
        {
            get; set;
        } // 需求数量


        public string? Unit
        {
            get; set;
        } // 单位


        public int? ReservationItem
        {
            get; set;
        } // 预留项目


        public decimal? ComponentScrap
        {
            get; set;
        } // 组件废料


        public string? BomItem
        {
            get; set;
        } // BOM项目


        public string? Operation
        {
            get; set;
        } // 操作


        public bool? MovementAllowed
        {
            get; set;
        } // 允许移动


        public bool? QuantityIsFixed
        {
            get; set;
        } // 数量已固定

        public int? ConsumeType
        {
            get; set;
        }

        public int? SyncWMSStatus
        {
            get; set;
        }
        public string? CreateBy
        {
            get; set;
        } // 创建人

        public string? SuperMaterialCode
        {
            get; set;
        } // 操作
        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderBomItemCreateDto, WorkOrderBomItem>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderBomItemUpdateDto : IMapFrom<WorkOrderBomItem>
    {
        public int Id
        {
            get; set;
        }

        public decimal? RequiredQuantity
        {
            get; set;
        } // 需求数量


        public string? Unit
        {
            get; set;
        } // 单位


        public int? ReservationItem
        {
            get; set;
        } // 预留项目


        public decimal? ComponentScrap
        {
            get; set;
        } // 组件废料


        public string? BomItem
        {
            get; set;
        } // BOM项目


        public string? Operation
        {
            get; set;
        } // 操作


        public bool? MovementAllowed
        {
            get; set;
        } // 允许移动


        public bool? QuantityIsFixed
        {
            get; set;
        } // 数量已固定

        public int? ConsumeType
        {
            get; set;
        }

        public int? SyncWMSStatus
        {
            get; set;
        }

        public string? SuperMaterialCode
        {
            get; set;
        } // 操作

        public bool? IsActive
        {
            get; set;
        }

        public int? BomVersion
        {
            get; set;
        } // 工艺版本

        public DateTime? UpdateOn
        {
            get; set;
        } // 更新时间

        public string? UpdateBy
        {
            get; set;
        } // 更新人

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderBomItemUpdateDto, WorkOrderBomItem>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
