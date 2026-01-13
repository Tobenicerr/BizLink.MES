using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class WorkOrderComponentsTransferDto : IMapFrom<V_WorkOrderComponentsTransfer>
    {
        public int FactoryId
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
        }

        public int WorkOrderProcessId
        {
            get; set;
        }

        public DateTime? ScheduledStartDate
        {
            get; set;
        }

        public DateTime? ScheduledFinishDate
        {
            get; set;
        }
        public string? ProcessStatus
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

        public string? BaseUnit
        {
            get; set;
        }
        public decimal? RequiredQuantity
        {
            get; set;
        }

        public DateTime? PostingDate
        {
            get; set;
        }

        public string? BatchCode
        {
            get; set;
        }

        public string? ReceivingBatchCode
        {
            get; set;
        }

        public decimal? TransferredQuantity
        {
            get; set;
        }

        public string? FromLocationCode
        {
            get; set;
        }

        public string? ToLocationCode
        {
            get; set;
        }

        public string? TransferStatus
        {
            get; set;
        }

        public string? Status
        {
            get; set;
        }
        public string? TransferMessageType
        {
            get; set;
        }

        public string? TransferMessage
        {
            get; set;
        }

        public string? ConfirmStatus
        {
            get; set;
        }

        public string? ConfirmMessageType
        {
            get; set;
        }

        public string? ConfirmMessage
        {
            get; set;
        }

        public decimal? ConfirmQuantity
        {
            get; set;
        }


        public string? CreatedAt
        {
            get; set;
        }

        private string? CratedBy
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<V_WorkOrderComponentsTransfer, WorkOrderComponentsTransferDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WorkOrderComponentsTransferCreateDto : IMapFrom<V_WorkOrderComponentsTransfer>
    {

    }

    public class WorkOrderComponentsTransferUpdateDto : IMapFrom<V_WorkOrderComponentsTransfer>
    {

    }
}
