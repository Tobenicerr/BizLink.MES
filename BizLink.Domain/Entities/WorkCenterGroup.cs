using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    [SugarTable("Mes_WorkCenterGroup", IsDisabledUpdateAll = true)]
    public  class WorkCenterGroup
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
        [SugarColumn(IsNullable = false)]

        public string? GroupCode
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? GroupName
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? GroupDesc
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string? GroupType
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]

        public string? Status
        {
            get; set;
        } = "1";

        [SugarColumn(IsNullable = true)]

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
