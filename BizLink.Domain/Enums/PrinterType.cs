using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public enum PrinterType
    {
        [Description("网络打印")]
        NetworkPrinter = 1,
        [Description("本地打印")]
        LocalPrinter = 2
    }
}
