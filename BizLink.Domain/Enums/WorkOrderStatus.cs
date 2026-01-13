using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public enum WorkOrderStatus
    {
        [Description("已删除")]
        Deleted = -1,
        [Description("未准备")]
        UnKnow = 0,
        [Description("已排产")]
        New = 1,
        [Description("执行中")]
        OnGoing = 2,
        [Description("已挂起")]
        Paused = 3,
        [Description("已完成")]
        Finished = 4,
    }
}
