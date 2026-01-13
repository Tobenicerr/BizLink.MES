using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities.Views
{
    [SugarTable("V_Biz_Material")]
    public class V_Material
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int Id
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]

        public string FactoryCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string MaterialCode
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string MaterialName
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string BaseUnit
        {
            get; set;
        }
        [SugarColumn(IsNullable = true)]
        public int? LabelId
        {
            get; set;
        }


        [SugarColumn(IsNullable = true)]
        public int? ConsumeType
        {
            get; set;
        }

        [SugarColumn(IsNullable = true)]
        public string? LabelName
        {
            get; set;
        }
    }
}
