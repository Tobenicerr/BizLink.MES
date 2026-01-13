using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    public class WorkCenterGroupStepConfig
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        public int GroupId
        {
            get; set;
        }

        public int StepId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true, Length = 50)]
        public string? StepCategory
        {
            get; set;
        }

        public int StepSequence
        {
            get; set;
        }
        public string? IsStartStep
        {
            get; set;
        } = "N";
        public string? IsEndStep
        {
            get; set;
        } = "N";

        public string? IsCriticalPath
        {
            get; set;
        } = "N";

        [SugarColumn(IsNullable = true, ColumnName = "CreatedAt")]
        public DateTime? CreateOn
        {
            get; set;
        } = DateTime.Now; // 创建时间

        [SugarColumn(IsNullable = true)]
        public string? CreateBy
        {
            get; set;
        } // 创建人

        [SugarColumn(IsNullable = true, ColumnName = "UpdatedAt")]
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
