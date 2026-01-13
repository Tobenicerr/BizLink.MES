using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public enum LocationType
    {
        /// <summary>
        /// 区域 (如：原材料区, 成品区)
        /// </summary>
        Area = 1,

        /// <summary>
        /// 产线/工作中心
        /// </summary>
        Line = 2,

        /// <summary>
        /// 货架
        /// </summary>
        Shelf = 3,

        /// <summary>
        /// 货位/储位 (最小存储单元)
        /// </summary>
        Bin = 4,

        /// <summary>
        /// 虚拟库位 (用于逻辑分组或临时存放)
        /// </summary>
        Virtual = 99
    }
}
