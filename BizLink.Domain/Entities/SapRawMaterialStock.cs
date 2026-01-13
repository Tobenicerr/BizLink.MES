using BizLink.MES.Domain.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    public class SapRawMaterialStock
    {

        [SapFieldName("MATNR")]
        public string? MaterialCode
        {
            get; set;
        }

        [SapFieldName("WERKS")]
        public string? FactoryCode
        {
            get; set;
        }

        [SapFieldName("LGORT")]
        public string? LocationCode
        {
            get; set;
        }

        [SapFieldName("CHARG")]
        public string? BatchCode
        {
            get; set;
        }

        [SapFieldName("CLABS")]
        public decimal? Quantity
        {
            get; set;
        }

        public string? BaseUnit
        {
            get; set;
        }

        public string? BatchCodeNew
        {
            get; set;
        }
        [SapFieldName("ATWRT")]
        public string? Remark
        {
            get; set;
        }

    }
}
