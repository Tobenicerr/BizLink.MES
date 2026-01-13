using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities.Views;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class RawMaterialInventoryViewDto : IMapFrom<V_RawMaterialInventory>
    {
        public int Id
        {
            get; set;
        }


        public string? FactoryCode
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


        public decimal? Quantity
        {
            get; set;
        }

        public decimal? LastQuantity
        {
            get; set;
        }

        public string? BaseUnit
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


        public string? RawStockName
        {
            get; set;
        }


        public string? RawLocationName
        {
            get; set;
        }
    }
}
