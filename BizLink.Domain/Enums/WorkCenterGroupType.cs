using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public enum WorkCenterGroupType
    {
        [Description("仓库组")]
        WareHouseGroup = 1,

        [Description("装配组")]
        AssemblyGroup = 2,

        [Description("检验组组")]
        InspectionGroup = 3,
    }
}
