using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Enums
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            // 获取枚举成员
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null)
                return value.ToString();

            // 获取该成员的 Description 特性
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // 如果有 Description 特性，则返回其内容，否则返回枚举成员的名称
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// 根据 Description 获取对应的枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="description">Description 的字符串值 (例如 "03")</param>
        /// <returns>对应的枚举值</returns>
        public static T GetEnumByDescription<T>(string description) where T : Enum
        {
            // 获取枚举类型的所有字段
            FieldInfo[] fields = typeof(T).GetFields();

            foreach (FieldInfo field in fields)
            {
                // 获取字段上的 DescriptionAttribute
                var attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

                // 如果找到了属性，并且描述内容匹配
                if (attribute != null && attribute.Description == description)
                {
                    // 返回该字段对应的枚举值
                    return (T)field.GetValue(null);
                }
            }

            // 如果没找到，抛出异常或返回默认值 (这里选择报错提示)
            throw new ArgumentException($"在枚举 {typeof(T).Name} 中未找到描述为 '{description}' 的项。", nameof(description));

        }
    }
}
