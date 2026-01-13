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
    public  class WorkOrderDto : IMapFrom<WorkOrder>
    {
        public int Id
        {
            get; set;
        }


        public int FactoryId
        {
            get; set;
        } // 工厂ID

        public string? FactoryCode
        {
            get; set;
        }

        public string? OrderNumber
        {
            get; set;
        } // 订单号


        public string? MaterialCode
        {
            get; set;
        } // 产品代码


        public string? MaterialDesc
        {
            get; set;
        } // 产品名称


        public decimal? Quantity
        {
            get; set;
        } // 订单数量


        public DateTime? DispatchDate
        {
            get; set;
        } // 发货日期


        public string? Status
        {
            get; set;
        } // 订单状态


        public DateTime? ScheduledStartDate
        {
            get; set;
        } // 计划开始日期


        public DateTime? ScheduledFinishDate
        {
            get; set;
        } // 计划完成日期


        public DateTime? BasicStartDate
        {
            get; set;
        } // 基本开始日期


        public DateTime? BasicFinishDate
        {
            get; set;
        } // 基本完成日期

        public string? PlannerRemark
        {
            get; set;
        } // 计划员备注


        public string? CollectiveOrder
        {
            get; set;
        } // 集合订单


        public string? SuperiorOrder
        {
            get; set;
        } // 上级订单


        public string? LeadingOrder
        {
            get; set;
        } // 前置订单


        public string? LeadingOrderMaterial
        {
            get; set;
        } // 前置订单物料


        public string? ComponentMainMaterial
        {
            get; set;
        } // 组件主物料


        public int? ConfirmNo
        {
            get; set;
        }

        public int? ReservationNo
        {
            get; set;
        }


        public string? PlanningPeriod
        {
            get; set;
        } // 计划周期


        public string? StorageLocation
        {
            get; set;
        } // 存储位置


        public string? ProfitCenter
        {
            get; set;
        } // 利润中心


        public int? LabelCount
        {
            get; set;
        } // 标签数量
        public int? ProcessCardPrintCount
        {
            get; set;
        } // 打印次数

        public DateTime? CreateOn
        {
            get; set;
        } // 创建时间


        public string? CreateBy
        {
            get; set;
        } // 创建人
        public int? CableCount
        {
            get; set;
        }

        public DateTime? UpdateOn
        {
            get; set;
        }
        public string? UpdateBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrder, WorkOrderDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderCreateDto : IMapFrom<WorkOrder>
    {
        public int FactoryId
        {
            get; set;
        } // 工厂ID


        public string? OrderNumber
        {
            get; set;
        } // 订单号


        public string? MaterialCode
        {
            get; set;
        } // 产品代码


        public string? MaterialDesc
        {
            get; set;
        } // 产品名称


        public decimal? Quantity
        {
            get; set;
        } // 订单数量


        public DateTime? DispatchDate
        {
            get; set;
        } // 发货日期


        public string? Status
        {
            get; set;
        } // 订单状态


        public DateTime? ScheduledStartDate
        {
            get; set;
        } // 计划开始日期


        public DateTime? ScheduledFinishDate
        {
            get; set;
        } // 计划完成日期


        public DateTime? BasicStartDate
        {
            get; set;
        } // 基本开始日期


        public DateTime? BasicFinishDate
        {
            get; set;
        } // 基本完成日期

        public string? PlannerRemark
        {
            get; set;
        } // 计划员备注


        public string? CollectiveOrder
        {
            get; set;
        } // 集合订单


        public string? SuperiorOrder
        {
            get; set;
        } // 上级订单


        public string? LeadingOrder
        {
            get; set;
        } // 前置订单


        public string? LeadingOrderMaterial
        {
            get; set;
        } // 前置订单物料


        public string? ComponentMainMaterial
        {
            get; set;
        } // 组件主物料

        public int? ConfirmNo
        {
            get; set;
        }

        public int? ReservationNo
        {
            get; set;
        }



        public string? PlanningPeriod
        {
            get; set;
        } // 计划周期


        public string? StorageLocation
        {
            get; set;
        } // 存储位置


        public string? ProfitCenter
        {
            get; set;
        } // 利润中心


        public int? LabelCount
        {
            get; set;
        } // 标签数量

        public string? CreateBy
        {
            get; set;
        } // 创建人

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderCreateDto, WorkOrder>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderUpdateDto : IMapFrom<WorkOrder>
    {
        public int Id
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        } // 订单状态

        public decimal? Quantity
        {
            get; set;
        } // 订单数量


        public DateTime? DispatchDate
        {
            get; set;
        } // 发货日期

        public DateTime? ScheduledStartDate
        {
            get; set;
        } // 计划开始日期


        public DateTime? ScheduledFinishDate
        {
            get; set;
        } // 计划完成日期


        public DateTime? BasicStartDate
        {
            get; set;
        } // 基本开始日期


        public DateTime? BasicFinishDate
        {
            get; set;
        } // 基本完成日期

        public string? PlannerRemark
        {
            get; set;
        } // 计划员备注


        public string? CollectiveOrder
        {
            get; set;
        } // 集合订单


        public string? SuperiorOrder
        {
            get; set;
        } // 上级订单


        public string? LeadingOrder
        {
            get; set;
        } // 前置订单


        public string? LeadingOrderMaterial
        {
            get; set;
        } // 前置订单物料


        public string? ComponentMainMaterial
        {
            get; set;
        } // 组件主物料

        public int? ConfirmNo
        {
            get; set;
        }

        public int? ReservationNo
        {
            get; set;
        }



        public string? PlanningPeriod
        {
            get; set;
        } // 计划周期


        public string? StorageLocation
        {
            get; set;
        } // 存储位置


        public string? ProfitCenter
        {
            get; set;
        } // 利润中心


        public int? LabelCount
        {
            get; set;
        } // 标签数量

        public DateTime? UpdateOn
        {
            get; set;
        }
        public string? UpdateBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderUpdateDto, WorkOrder>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }
}
