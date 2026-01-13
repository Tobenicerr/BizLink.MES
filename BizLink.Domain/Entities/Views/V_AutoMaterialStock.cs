using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("View_Material")]
    public class V_AutoMaterialStock
    {
        [SugarColumn(ColumnName = "MaterialCode")]
        public string MaterialCode
        {
            get; set;
        }

        [SugarColumn(ColumnName = "MaterialName")]
        public string MaterialDesc
        {
            get; set;
        }

        [SugarColumn(IsIgnore = true)]
        public string BaseUnit
        {
            get; set;
        } = "ST";

        [SugarColumn(ColumnName = "Quantity")]
        public decimal Quantity
        {
            get; set;
        }

        [SugarColumn(ColumnName = "Batch")]
        public string BatchCode
        {
            get; set;
        }

        [SugarColumn(ColumnName = "Consumer_MaterialId")]
        public string BarCode
        {
            get; set;
        }

        [SugarColumn(ColumnName = "BoxPositionCode")]
        public string LocationCode
        {
            get; set;
        }

        [SugarColumn(ColumnName = "Islock")]
        public bool IsLocked
        {
            get; set;
        }
    }
}
