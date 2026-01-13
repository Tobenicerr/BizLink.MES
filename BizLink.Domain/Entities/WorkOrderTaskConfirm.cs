using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkOrderTaskConfirm", IsDisabledUpdateAll = true)]
    public class WorkOrderTaskConfirm
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        public int TaskId
        {
            get; set;
        }

        public int WorkStationId
        {
            get; set;
        }

        public string? ConfirmNumber
        {
            get; set;
        }

        public DateTime? ConfirmDate
        {
            get; set;
        }

        public decimal? ConfirmQuantity
        {
            get; set;
        }

        public string? EmployerCode
        {
            get; set;
        }
        public string? Status
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 255)]
        public string? Remark
        {
            get; set;
        }

    }
}
