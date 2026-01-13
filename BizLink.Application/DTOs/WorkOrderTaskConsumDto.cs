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
    public class WorkOrderTaskConsumDto : IMapFrom<WorkOrderTaskConsum>
    {

        public int Id
        {
            get; set;
        }

        public int ConfirmId
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

        public decimal? EntryQuantity
        {
            get; set;
        }

        public string? EntryUnitCode
        {
            get; set;
        }

        public string? MovementType
        {
            get; set;
        }
        public string? MovementRemark
        {
            get; set;
        }
    }

    public class WorkOrderTaskConsumCreateDto : IMapFrom<WorkOrderTaskConsum>
    {
        public int? ConfirmId
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

        public decimal? EntryQuantity
        {
            get; set;
        }

        public string? EntryUnitCode
        {
            get; set;
        }

        public string? MovementType
        {
            get; set;
        }
        public string? MovementRemark
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderTaskConsumCreateDto, WorkOrderTaskConsum>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
