using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public enum ConsumeType
    {
        [Description("电缆")]
        CableMaterial = 0,
        [Description("一次性领料")]
        BulkMaterial =1,
        [Description("按单领料")]
        OrderBasedMaterial = 2
    }
}
