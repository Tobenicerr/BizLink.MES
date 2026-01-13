using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Shared.Extensions
{
    //public static class EnumExtensions
    //{
    //    public static string GetDescription(this Enum value)
    //    {
    //        // 获取枚举成员
    //        FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
    //        if (fieldInfo == null)
    //            return value.ToString();

    //        // 获取该成员的 Description 特性
    //        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

    //        // 如果有 Description 特性，则返回其内容，否则返回枚举成员的名称
    //        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    //    }
    //}
}
