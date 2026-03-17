using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{


    [SugarTable("Mes_WorkOrderKittingItem")]

    public class WorkOrderKittingItem
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public int WorkOrderId { get; set; }

        [SugarColumn(Length = 50, IsNullable = false)]
        public string WorkOrderNo { get; set; }

        public int WorkOrderProcessId { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string Operation { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string BomItem { get; set; }


        public int ReservationItem { get; set; }

        /// <summary>
        /// 物料号
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = false)]
        public string MaterialCode { get; set; }

        [SugarColumn(Length = 200, IsNullable = true)]
        public string? MaterialDesc { get; set; }

        [SugarColumn(Length = 10, IsNullable = true)]
        public string BaseUnit { get; set; }

        /// <summary>
        /// 批次号 (原材料批次 或 半成品虚拟批次)
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = true)]
        public string? BatchCode { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string? BarCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        public string? ConsumptionType { get; set; }

        public int? MaterialConsumeType { get; set; }

        [SugarColumn(Length = 200, IsNullable = true,ColumnDataType = "NVARCHAR")]
        public string? ConsumptionRemark { get; set; }



        [SugarColumn(Length = 200, IsNullable = true, ColumnDataType = "NVARCHAR")]
        public string? Remark { get; set; }

        [SugarColumn(Length = 10, IsNullable = true)]
        public string? Status { get; set; } = "1";

        [SugarColumn(IsNullable = true)]
        public DateTime? CreatedOn
        {
            get; set;
        } = DateTime.Now;

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? CreatedBy
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdatedOn
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? UpdateBy
        {
            get; set;
        }

    }
}
