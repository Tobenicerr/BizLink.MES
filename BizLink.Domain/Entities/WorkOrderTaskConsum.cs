using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderTaskConsum", IsDisabledUpdateAll = true)]
    public class WorkOrderTaskConsum
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        public int ConfirmId
        {
            get; set;
        }

        public string? MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? BatchCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? BarCode
        {
            get; set;
        }

        public decimal? EntryQuantity
        {
            get; set;
        }

        public string? EntryUnitCode
        {
            get; set;
        }

        public string? MovementType
        {
            get; set;
        }
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 255)]

        public string? MovementRemark
        {
            get; set;
        }
    }
}
