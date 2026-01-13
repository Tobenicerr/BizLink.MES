using BizLink.MES.Domain.Attributes;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_CableCutParam")]
    public class CableCutParam
    {

        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("CUTTOLERANCEID")]

        public int? CuttoLeranceId
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("SEMINUMBER")]

        public string? SemiMaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("CABLENUMBER")]
        public string? CableMaterialCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("CABLETYPE")]

        public string? CableType
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 50)]
        [SapFieldName("DRAWINGNUMBER")]

        public string? DrawingCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("POSNR")]

        public string? PositionItem
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("CABLEPCS")]

        public int? CablePcs
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("POSITIONNO")]

        public string? PostionNo
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("BOMLENGTH")]

        public decimal? BomLength
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("UPTOL")]

        public decimal? UpTol
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("DOWNTOL")]

        public decimal? DownTol
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("ALPHAFACTOR")]

        public decimal? AlphaFactor
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("BETAFACTOR")]

        public decimal? BetaFactor
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("CUTLENGTH")]

        public decimal? CuttingLength
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("CUTTIME")]

        public decimal? CuttingTime
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 100)]
        [SapFieldName("REELNUMBER")]

        public string? ReelCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true,ColumnDataType = "nvarchar",Length = 100)]
        [SapFieldName("REMARK")]

        public string? Remark
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, Length = 10)]
        [SapFieldName("STATUS")]

        public string? Status
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("ERDAT")]
        public string? CreateDate
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("ERZET")]
        public string? CreateTime
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("ERNAM")]
        public string? CreateBy
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        [SapFieldName("AEDAT")]

        public string? UpdateDate
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        [SapFieldName("AEZET")]

        public string? UpdateTime
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 20)]
        [SapFieldName("AENAM")]

        public string? UpdateBy
        {
            get; set;
        }

    }
}