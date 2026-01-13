using BizLink.MES.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Common
{
    public static class StatusQueryRules
    {
        public static readonly List<string> ActiveWorkOrderStatus = new List<string>
        {
            ((int)WorkOrderStatus.New).ToString(), // "1"
            ((int)WorkOrderStatus.OnGoing).ToString(), // "2"
            ((int)WorkOrderStatus.Paused).ToString(),   // "3"
            ((int)WorkOrderStatus.Finished).ToString()   // "3"
        };
    }
}
