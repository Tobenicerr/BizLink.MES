using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public  enum MaterialTransferType
    {
        [Description("03")]
        GoodsIssue = 1,

        [Description("04")]
        MaterialReturn = 2,

        [Description("05")]
        RawMaterialScrapAgainstOrder = 3,

        [Description("15")]
        GoodsIssuetoCostCenter = 4,

        [Description("16")]
        ReversalofGoodsIssuetoCostCenter = 5,
    }
}
