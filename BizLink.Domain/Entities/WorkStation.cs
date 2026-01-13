using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkStation", IsDisabledUpdateAll = true)]
    public class WorkStation
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        /// <summary>
        /// 工厂Id
        /// </summary>
        public int? FactoryId
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        /// <summary>
        /// 工厂Id
        /// </summary>
        public int? WorkAreaId
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public int WorkCenterId
        {
            get; set;
        }

        public string? WorkStationCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 50)]
        public string? WorkStationName
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? Status
        {
            get; set;
        } = "1";

        [SugarColumn(IsNullable = true)]

        public string? PrintType
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? PrinterName
        {
            get; set;
        }

        public bool IsStartStep
        {
            get; set;
        } = false;

        public bool IsEndStep
        {
            get; set;
        } = false;

        public bool IsDelete
        {
            get; set;
        } = false;

        [SugarColumn(IsNullable = true)]

        public DateTime? CreatedAt
        {
            get; set;
        } = DateTime.Now;
        [SugarColumn(IsNullable = true)]

        public string? CreatedBy
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public DateTime? UpdatedAt
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? UpdateBy
        {
            get; set;
        }
    }
}
