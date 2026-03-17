using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkTaskCategory")]
    public class WorkTaskCategory 
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? CategoryCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? CategoryName
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? CategoryDescription
        {
            get; set;
        }


        public string? StepType
        {
            get; set;
        }

        /// <summary>
        /// 是否是里程碑工步？ (1=是，0=否)
        /// 只有里程碑工步的完成，才会向上累加 OperationTask 和 Process 的完成数量
        /// </summary>
        public bool IsMilestone { get; set; } = false;

        /// <summary>
        /// 是否触发【前置工序】的 SAP 报工？ (1=是，0=否)
        /// 例如 "材料准备(MAT_CHECK)" 设为 1
        /// </summary>
        public bool IsSapPrevTrigger { get; set; } = false;

        /// <summary>
        /// 是否触发【当前工序】的 SAP 报工？ (1=是，0=否)
        /// 例如 "装配完工(ASSY_END)" 设为 1
        /// </summary>
        public bool IsSapCurrentTrigger { get; set; } = false;

        public int SortNo { get; set; }

        [SugarColumn(IsNullable = true)]

        public bool IsActive
        {
            get; set;
        } = true;

        [SugarColumn(IsNullable = true)]

        public DateTime? CreatedOn
        {
            get; set;
        } = DateTime.Now;
        [SugarColumn(IsNullable = true)]

        public string? CreatedBy
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public DateTime? UpdatedOn
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
