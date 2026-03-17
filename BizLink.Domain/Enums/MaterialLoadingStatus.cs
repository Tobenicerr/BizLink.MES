using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public static class MaterialLoadingStatus
    {
        public const string Active = "1";     // 使用中
        public const string Depleted = "2";     // 已耗尽
        public const string Unloaded = "3";     // 已卸载

    }
}
