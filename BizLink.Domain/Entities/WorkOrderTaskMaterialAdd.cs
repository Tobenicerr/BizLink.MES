using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderTaskMaterialAdd")]
    //[SugarTable("Mes_WorkOrderTaskMaterialAdd", IsDisabledUpdateAll = true)]
    public  class WorkOrderTaskMaterialAdd
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id {get; set;}

        [SugarColumn(IsNullable = true)]
        public int TaskId {get; set;}

        [SugarColumn(IsNullable = true)]
        public int? WorkStationId {get; set;}

        [SugarColumn(IsNullable = true)]
        public string? MaterialCode {get; set;}

        [SugarColumn(IsNullable = true)]
        public string? MaterialDesc {get; set;}

        [SugarColumn(IsNullable = true)]
        public string? BomItem {get; set;}

        [SugarColumn(IsNullable = true)]
        public decimal? Quantity {get; set;}

        [SugarColumn(IsNullable = true)]
        public string? BatchCode {get; set;}
        [SugarColumn(IsNullable = true)]

        public string? BarCode {get; set;}
        [SugarColumn(IsNullable = true)]

        public decimal? LastQuantity { get; set; } = 0;
        [SugarColumn(IsNullable = true)]

        public string? Status { get; set; } = "1";

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
