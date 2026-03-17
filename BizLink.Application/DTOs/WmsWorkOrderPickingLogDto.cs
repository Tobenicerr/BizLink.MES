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
    public class WmsWorkOrderPickingLogDto : IMapFrom<V_WmsWorkOrderPickingLog>
    {
        public string WorkOrderNo { get; set; }

        public string MaterialCode { get; set; }

        public string MaterialDesc { get; set; }

        public string? BatchCode { get; set; }
        public string BarCode { get; set; }

        public string TaskStatus { get; set; }
        public string WmsStockName { get; set; }
        public string WmsTaskCode { get; set; }
        public string BaseUnit { get; set; }

        public decimal Quantity { get; set; }

        public int WmsSourceId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<V_WmsWorkOrderPickingLog, WmsWorkOrderPickingLogDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class WmsWorkOrderPickingLogCreateDto : IMapFrom<V_WmsWorkOrderPickingLog>
    {
    }

    public class WmsWorkOrderPickingLogUpdateDto : IMapFrom<V_WmsWorkOrderPickingLog>
    {
    }
}
