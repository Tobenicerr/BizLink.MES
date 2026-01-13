using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("VWMSMaterialListLocation")]
    public class V_WmsMaterialStock
    {
        public int Id { get; set; }

        [SugarColumn(ColumnName = "MATERIALNO")]
        public string MaterialCode { get; set; }

        [SugarColumn(ColumnName = "MATERIALNAME")]
        public string MaterialDesc { get; set; }

        [SugarColumn(ColumnName = "DW")]
        public string BaseUnit { get; set; }

        [SugarColumn(ColumnName = "NUM")]
        public decimal Quantity { get; set; }

        [SugarColumn(ColumnName = "BATNO")]
        public string BatchCode { get; set; }

        [SugarColumn(ColumnName = "BARCODE")]
        public string BarCode { get; set; }

        [SugarColumn(ColumnName = "stockcode")]
        public string StockCode
        {
            get; set;
        }

        [SugarColumn(ColumnName = "shelveCode")]
        public string ShelveCode
        {
            get; set;
        }

        [SugarColumn(ColumnName = "StockAreaName")]
        public string ShelveName
        {
            get; set;
        }
        [SugarColumn(ColumnName = "STOCKNAME")]
        public string StockName { get; set; }

        [SugarColumn(ColumnName = "LOCATIONNAME")]
        public string LocationName { get; set; }

        [SugarColumn(ColumnName = "PLANT")]
        public string FactoryCode { get; set; }

        [SugarColumn(ColumnName = "LockNum")]
        public decimal LockQuantity { get; set; }

        [SugarColumn(ColumnName = "DownNum")]
        public decimal DownQuantity { get; set; }

        [SugarColumn(ColumnName = "CanUseNum")]
        public decimal? UsageQuantity { get; set; }

        [SugarColumn(IsIgnore = true)]

        public int? ConsumeType
        {
            get; set;
        }

    }
}
