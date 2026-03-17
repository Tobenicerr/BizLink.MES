using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    public class FinishedGoodsReceiptDetail
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public int ReceiptId
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public string? SapRequestJson
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public string? SapResponseJson
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public string? PrintTemplate
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public string? PrintData
        {
            get;
            set;
        }

        [SugarColumn(IsNullable = true)]
        public DateTime? CreatedOn
        {
            get; set;
        } = DateTime.Now; // 创建时间

        [SugarColumn(IsNullable = true)]
        public string? CreatedBy
        {
            get; set;
        } // 创建人

        [SugarColumn(IsNullable = true)]
        public DateTime? UpdatedOn
        {
            get; set;
        } // 更新时间

        [SugarColumn(IsNullable = true)]
        public string? UpdatedBy
        {
            get; set;
        } // 更新人
    }
}
