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
    public class WorkOrderTaskDto : IMapFrom<WorkOrderTask>
    {
        public int Id
        {
            get; set;
        }

        public int OrderId
        {
            get; set;
        }

        public int OrderProcessId
        {
            get; set;
        }

        public string OrderNumber
        {
            get; set;
        }

        public string? TaskNumber
        {
            get; set;
        }

        public int? WorkStationId
        {
            get; set;
        }

        public string? WorkStationCode
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

        
        public string? MaterialItem
        {
            get; set;
        }

        
        public decimal? Quantity
        {
            get; set;
        }

        
        public decimal? CompletedQty
        {
            get; set;
        }

        
        public string? ProfitCenter
        {
            get; set;
        }

        
        public string? Status
        {
            get; set;
        }

        
        public DateTime? StartTime
        {
            get; set;
        }

        
        public DateTime? DispatchDate
        {
            get; set;
        }

        
        public string? WorkCenter
        {
            get; set;
        }
        public string? NextWorkCenter
        {
            get; set;
        }
        public string? Operation
        {
            get; set;
        }

        public string? Remark
        {
            get; set;
        }

        public string? ProductionRemark
        {
            get; set;
        }


        public decimal? CableLength
        {
            get; set;
        }

        
        public decimal? CableLengthUsl
        {
            get; set;
        }

        
        public decimal? CableLengthDsl
        {
            get; set;
        }
        public DateTime? CreateOn
        {
            get; set;
        }

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
    }

    public class WorkOrderTaskCreateDto : IMapFrom<WorkOrderTask>
    {
        public int OrderId
        {
            get; set;
        }

        public int OrderProcessId
        {
            get; set;
        }

        public string OrderNumber
        {
            get; set;
        }

        public string? TaskNumber
        {
            get; set;
        }

        public int? WorkStationId
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


        public string? MaterialItem
        {
            get; set;
        }


        public decimal? Quantity
        {
            get; set;
        }


        public decimal? CompletedQty
        {
            get; set;
        }


        public string? ProfitCenter
        {
            get; set;
        }


        public string? Status
        {
            get; set;
        }


        public DateTime? StartTime
        {
            get; set;
        }


        public DateTime? DispatchDate
        {
            get; set;
        }


        public string? WorkCenter
        {
            get; set;
        }

        public string? NextWorkCenter
        {
            get; set;
        }
        public string? Operation
        {
            get; set;
        }
        public string? Remark
        {
            get; set;
        }


        public decimal? CableLength
        {
            get; set;
        }


        public decimal? CableLengthUsl
        {
            get; set;
        }


        public decimal? CableLengthDsl
        {
            get; set;
        }
        public string? ProductionRemark
        {
            get; set;
        }

        public string? CreateBy
        {
            get; set;
        } // 创建人

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderTaskCreateDto, WorkOrderTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderTaskUpdateDto : IMapFrom<WorkOrderTask>
    {


        public int Id
        {
            get; set;
        }
        public decimal? CompletedQty
        {
            get; set;
        }


        public string? Status
        {
            get; set;
        }


        public DateTime? StartTime
        {
            get; set;
        }


        public DateTime? DispatchDate
        {
            get; set;
        }


        public string? WorkCenter
        {
            get; set;
        }


        public string? Remark
        {
            get; set;
        }


        public decimal? CableLength
        {
            get; set;
        }


        public decimal? CableLengthUsl
        {
            get; set;
        }


        public decimal? CableLengthDsl
        {
            get; set;
        }
        public string? ProductionRemark
        {
            get; set;
        }
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
            profile.CreateMap<WorkOrderTaskUpdateDto, WorkOrderTask>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
