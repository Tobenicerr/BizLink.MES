using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class WorkOrderOperationConsumptionRecordDto : IMapFrom<WorkOrderOperationConsumptionRecord>
    {
        public int Id
        {
            get; set;
        }
        public int WorkOrderId
        {
            get; set;
        }
        public int WorkOrderProcessId
        {
            get; set;
        }

        public int? ReservationItem
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
        public decimal? Quantity
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }
        public int ConsumptionType
        {
            get; set;
        }
        public string? ConsumptionRemark
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

        public DateTime? UpdatedAt
        {
            get; set;
        }

        public string? UpdatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationConsumptionRecord, WorkOrderOperationConsumptionRecordDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderOperationConsumptionRecordCreateDto : IMapFrom<WorkOrderOperationConsumptionRecord>
    {

        public int WorkOrderId
        {
            get; set;
        }
        public int WorkOrderProcessId
        {
            get; set;
        }

        public int? ReservationItem
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
        public decimal? Quantity
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }
        public int ConsumptionType
        {
            get; set;
        }
        public string? ConsumptionRemark
        {
            get; set;
        }

        public string? CreatedBy
        {
            get; set;
        }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationConsumptionRecordCreateDto, WorkOrderOperationConsumptionRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderOperationConsumptionRecordUpdateDto : IMapFrom<WorkOrderOperationConsumptionRecord>
    {
        public int Id
        {
            get; set;
        }
       
        public decimal? Quantity
        {
            get; set;
        }
        public int ConsumptionType
        {
            get; set;
        }
        public string? ConsumptionRemark
        {
            get; set;
        }

        public DateTime? UpdatedAt
        {
            get; set;
        }

        public string? UpdatedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderOperationConsumptionRecordUpdateDto, WorkOrderOperationConsumptionRecord>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

}
