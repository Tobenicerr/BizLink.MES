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
    public class WorkOrderProcessDto : IMapFrom<WorkOrderProcess>
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

        public int? ConfirmNo
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        } // 工序状态


        public DateTime? StartTime
        {
            get; set;
        } // 计划开始时间


        public DateTime? EndTime
        {
            get; set;
        } // 计划结束时间

        public decimal? Quantity
        {
            get; set;
        } // 数量

        public decimal? CompletedQuantity
        {
            get; set;
        }

        public DateTime? ActStartTime
        {
            get; set;
        } // 实际开始时间


        public DateTime? ActEndTime
        {
            get; set;
        } // 实际结束时间


        public string? WorkCenter
        {
            get; set;
        } // 工作中心


        public string? Operation
        {
            get; set;
        } // 操作



        public string? ControlKey
        {
            get; set;
        } // 控制码


        public string? NextWorkCenter
        {
            get; set;
        } // 下一工作中心

        public int? ProcessCardPrintCount
        {
            get; set;
        }
        public decimal? SetupTime
        {
            get; set;
        } // 设置时间


        public string? SetupTimeUnit
        {
            get; set;
        } // 设置时间单位


        public decimal? MachineTime
        {
            get; set;
        } // 机器时间


        public string? MachineTimeUnit
        {
            get; set;
        } // 机器时间单位

        public bool? IsActive 
        { 
            get; set; 
        }

        public int? OperationVersion
        {
            get; set;
        } // 工艺版本
        public DateTime? CreateOn
        {
            get; set;
        } // 创建时间


        public string? CreateBy
        {
            get; set;
        } // 创建人


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
            profile.CreateMap<WorkOrderProcess, WorkOrderProcessDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderProcessCreateDto : IMapFrom<WorkOrderProcess>
    {

        public int? WorkOrderId
        {
            get; set;
        }

        public string? WorkOrderNo
        {
            get; set;
        }

        public int? ConfirmNo
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        } // 工序状态


        public DateTime? StartTime
        {
            get; set;
        } // 计划开始时间


        public DateTime? EndTime
        {
            get; set;
        } // 计划结束时间

        public decimal? Quantity
        {
            get; set;
        } // 数量

        public DateTime? ActStartTime
        {
            get; set;
        } // 实际开始时间


        public DateTime? ActEndTime
        {
            get; set;
        } // 实际结束时间


        public string? WorkCenter
        {
            get; set;
        } // 工作中心


        public string? Operation
        {
            get; set;
        } // 操作


        public string? ControlKey
        {
            get; set;
        } // 控制码


        public string? NextWorkCenter
        {
            get; set;
        } // 下一工作中心


        public decimal? SetupTime
        {
            get; set;
        } // 设置时间


        public string? SetupTimeUnit
        {
            get; set;
        } // 设置时间单位


        public decimal? MachineTime
        {
            get; set;
        } // 机器时间


        public string? MachineTimeUnit
        {
            get; set;
        } // 机器时间单位


        public string? CreateBy
        {
            get; set;
        } // 创建人

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderProcessCreateDto, WorkOrderProcess>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }

    public class WorkOrderProcessUpdateDto : IMapFrom<WorkOrderProcess>
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
        public string? Operation
        {
            get; set;
        } // 操作
        public decimal? Quantity
        {
            get; set;
        } // 数量

        public DateTime? StartTime
        {
            get; set;
        } // 计划开始时间

        public DateTime? EndTime
        {
            get; set;
        } // 计划结束时间
        public string? Status
        {
            get; set;
        } // 工序状态
        public decimal? CompletedQuantity
        {
            get; set;
        }
        public string? ControlKey
        {
            get; set;
        } // 控制码
        public string? NextWorkCenter
        {
            get; set;
        } // 下一工作中心
        public DateTime? ActStartTime
        {
            get; set;
        } // 实际开始时间


        public DateTime? ActEndTime
        {
            get; set;
        } // 实际结束时间

        public string? WorkCenter
        {
            get; set;
        } // 工作中心

        public int? ConfirmNo
        {
            get; set;
        }
        public int? ProcessCardPrintCount
        {
            get; set;
        }

        public decimal? SetupTime
        {
            get; set;
        } // 设置时间

        public string? SetupTimeUnit
        {
            get; set;
        } // 设置时间单位

        public decimal? MachineTime
        {
            get; set;
        } // 机器时间

        public string? MachineTimeUnit
        {
            get; set;
        } // 机器时间单位

        public bool? IsActive
        {
            get; set;
        }

        public int? OperationVersion
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
            profile.CreateMap<WorkOrderProcessUpdateDto, WorkOrderProcess>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
