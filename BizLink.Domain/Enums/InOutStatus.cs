using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public enum InOutStatus
    {
        [Description("入库")]
        In = 1,
        [Description("出库")]
        Out = -1,
    }
}
