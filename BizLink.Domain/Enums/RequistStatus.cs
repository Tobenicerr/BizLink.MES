using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public enum RequistStatus
    {
        [Description("未领料")]
        No = 0,

        [Description("已领料")]
        Yes = 1,

        [Description("部分领料")]
        Partial = 2,

    }
}
