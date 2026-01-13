using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_CableCutParam")]

    public class V_CableCutParam
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable = true)]

        public string? SemiMaterialCode { get; set; }
        [SugarColumn(IsNullable = true)]

        public string? SemiMaterialDesc { get; set; }
        [SugarColumn(IsNullable = true)]

        public string? CableItem { get; set; }
        [SugarColumn(IsNullable = true)]

        public string? CableMaterialCode { get; set; }
        [SugarColumn(IsNullable = true)]

        public string? CableMaterialDesc { get; set; }
        [SugarColumn(IsNullable = true)]

        public int CutQuantity { get; set; }
        [SugarColumn(IsNullable = true)]

        public decimal BomLength { get; set; }
        [SugarColumn(IsNullable = true)]

        public decimal CableUsl
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public decimal CableDsl
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public decimal AlphaFactor
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public decimal BeatFactor
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public decimal CuttingLength
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public decimal StandardCuttingTime
        {
            get; set;
        }


    }
}
