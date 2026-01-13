using AntdUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.WinForms.Common.Helper
{
    public static class CellTagHelper
    {
        public static CellTag BuildWorkOrderStatusTag(string status)
        {
            switch (status)
            {
                case "0":
                    return new AntdUI.CellTag("未准备", AntdUI.TTypeMini.Error);

                case "1":
                    return new AntdUI.CellTag("已排产", AntdUI.TTypeMini.Default);

                case "2":
                    return new AntdUI.CellTag("执行中", AntdUI.TTypeMini.Info);

                case "3":
                    return new AntdUI.CellTag("已挂起", AntdUI.TTypeMini.Warn);

                case "4":
                    return new AntdUI.CellTag("已完成", AntdUI.TTypeMini.Success);

                default:
                    return new AntdUI.CellTag("未知", AntdUI.TTypeMini.Default);
            }
        }

        public static CellTag BuildConsumeTypeTag(string type)
        {
            switch (type)
            {
                case "电缆":
                    return new AntdUI.CellTag("电缆", AntdUI.TTypeMini.Primary);
                case "一次性领料":
                    return new AntdUI.CellTag("一次性领料", AntdUI.TTypeMini.Error);
                case "按单领料":
                    return new AntdUI.CellTag("按单领料", AntdUI.TTypeMini.Success);
                default:
                    return new AntdUI.CellTag("未知", AntdUI.TTypeMini.Default) { BorderWidth = 1};
            }
        }
    }
}
