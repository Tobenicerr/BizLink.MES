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
    public class WorkOrderTaskMaterialAddDto : IMapFrom<WorkOrderTaskMaterialAdd>
    {

        public int Id
        {
            get; set;
        }


        public int TaskId
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


        public string? BomItem
        {
            get; set;
        }


        public decimal? Quantity
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


        public decimal? LastQuantity
        {
            get; set;
        }


        public string? Status
        {
            get; set;
        }
        public DateTime? CreateOn
        {
            get; set;
        } // 更新时间


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

    public class WorkOrderTaskMaterialAddCreateDto : IMapFrom<WorkOrderTaskMaterialAdd>
    {
        public int TaskId
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


        public string? BomItem
        {
            get; set;
        }


        public decimal? Quantity
        {
            get; set;
        }

        public decimal? LastQuantity
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

        public string? CreateBy
        {
            get; set;
        } // 创建人

        public void Mapping(Profile profile)
        {
            profile.CreateMap<WorkOrderTaskMaterialAddCreateDto, WorkOrderTaskMaterialAdd>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }

    public class WorkOrderTaskMaterialAddUpdateDto : IMapFrom<WorkOrderTaskMaterialAdd>
    {
        public int Id
        {
            get; set;
        }
        public decimal? LastQuantity
        {
            get; set;
        }


        public string? Status
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
            profile.CreateMap<WorkOrderTaskMaterialAddUpdateDto, WorkOrderTaskMaterialAdd>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }

    }
}
