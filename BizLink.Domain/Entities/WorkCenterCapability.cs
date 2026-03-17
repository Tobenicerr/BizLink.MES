using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkCenterCapability", IsDisabledUpdateAll = true)]
    public class WorkCenterCapability
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int WorkCenterGroupId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int TaskCategoryId
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? TaskCategoryCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public decimal? StandardCapacity
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public int Priority
        {
            get; set;
        }

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
