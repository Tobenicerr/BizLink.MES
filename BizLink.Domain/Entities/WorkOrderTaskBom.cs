using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderTaskBom", IsDisabledUpdateAll = true)]
    public class WorkOrderTaskBom
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        public int? OrderId
        {
            get; set;
        }

        public int? OrderProcessId
        {
            get; set;
        }

        public int? TaskId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? MaterialCode
        {
            get; set;
        } // 物料代码

        [SugarColumn(IsNullable = true)]
        public string? MaterialDesc
        {
            get; set;
        } // 物料名称

        [SugarColumn(IsNullable = true)]
        public decimal? RequiredQuantity
        {
            get; set;
        } // 需求数量

        [SugarColumn(IsNullable = true)]
        public string? Unit
        {
            get; set;
        } // 单位


        [SugarColumn(IsNullable = true)]
        public string? BomItem
        {
            get; set;
        } // BOM项目

        [SugarColumn(IsNullable = true)]
        public string? Status
        {
            get; set;
        } = "0";
        [SugarColumn(IsNullable = true)]
        public string? CreatedAt
        {
            get; set;
        } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
