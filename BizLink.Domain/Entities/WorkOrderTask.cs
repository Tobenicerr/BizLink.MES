using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderTask", IsDisabledUpdateAll = true)]
    public class WorkOrderTask
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public int? OrderId { get; set; }

        public int? OrderProcessId { get; set; }

        public string? OrderNumber { get; set; }

        public string? TaskNumber
        {
            get; set;
        }

        public int? WorkStationId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? MaterialCode { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? MaterialDesc { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? MaterialItem { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? Quantity { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? CompletedQty { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? ProfitCenter { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? Status { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? StartTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public DateTime? DispatchDate { get; set; }

        [SugarColumn(IsNullable = true)]
        public string? WorkCenter { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 50)]
        public string? NextWorkCenter { get; set; }
        [SugarColumn(IsNullable = true)]
        public string? Operation { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 255)]
        public string? Remark { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? CableLength { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? CableLengthUsl { get; set; }

        [SugarColumn(IsNullable = true)]
        public decimal? CableLengthDsl { get; set; }

        [SugarColumn(IsIgnore = true)]
        public string? WorkStationCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? ProductionRemark
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreateOn
        {
            get; set;
        } = DateTime.Now; // 创建时间

        [SugarColumn(IsNullable = true)]
        public string? CreateBy
        {
            get; set;
        } // 创建人

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdateOn
        {
            get; set;
        } // 更新时间

        [SugarColumn(IsNullable = true)]
        public string? UpdateBy
        {
            get; set;
        } // 更新人
    }
}
