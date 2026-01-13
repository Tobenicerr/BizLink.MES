using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public enum RawPickStatus
    {
        [Description("未拣配")]

        PendingPick = 0,
        [Description("部分拣配")]

        PartiallyPicked = 1,
        [Description("拣配完成")]

        Picked = 2,
        [Description("待合箱")]

        PendingConsolidation = 3,
        [Description("已合箱")]

        Consolidated = 4
    }
}
