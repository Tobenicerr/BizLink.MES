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
    public class WmsMaterialStockDto : IMapFrom<V_WmsMaterialStock>
    {
        public int Id
        {
            get; set;
        }

        public string MaterialCode
        {
            get; set;
        }

        public string MaterialDesc
        {
            get; set;
        }

        public string BaseUnit
        {
            get; set;
        }

        public decimal Quantity
        {
            get; set;
        }

        public string BatchCode
        {
            get; set;
        }

        public string BarCode
        {
            get; set;
        }

        public string StockCode
        {
            get; set;
        }

        public string ShelveCode
        {
            get; set;
        }

        public string ShelveName
        {
            get; set;
        }
        public string StockName
        {
            get; set;
        }

        public string LocationName
        {
            get; set;
        }

        public string FactoryCode
        {
            get; set;
        }

        public decimal LockQuantity
        {
            get; set;
        }

        public decimal DownQuantity
        {
            get; set;
        }

        public decimal? UsageQuantity
        {
            get; set;
        }

        public int? ConsumeType
        {
            get; set;
        }
    }

    public class WmsMaterialStockCreateDto : IMapFrom<V_WmsMaterialStock>
    {
    }

    public class WmsMaterialStockUpdateDto : IMapFrom<V_WmsMaterialStock>
    {
    }
}
