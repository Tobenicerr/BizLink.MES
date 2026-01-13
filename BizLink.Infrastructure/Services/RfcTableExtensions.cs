using BizLink.MES.Domain.Attributes;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Services
{
    /// <summary>
    /// 包含通用转换方法的静态类
    /// </summary>
    public static class RfcTableExtensions
    {
        /// <summary>
        /// [扩展方法] 将 IRfcTable 通用地转换为强类型的 List<T>
        /// </summary>
        /// <typeparam name="T">要转换的目标类型，必须有一个无参数的构造函数</typeparam>
        /// <param name="rfcTable">SAP NCo 的 IRfcTable 对象</param>
        /// <returns>一个包含映射后数据的 List<T></returns>
        public static List<T> ToList<T>(this IRfcTable rfcTable) where T : new()
        {
            var resultList = new List<T>();

            // 使用反射获取目标类型 T 的所有公共属性
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite) // 只选择可写的属性
                .ToList();

            // 遍历 IRfcTable 的每一行
            foreach (var row in rfcTable)
            {
                var item = new T(); // 创建目标类型的新实例

                // 遍历目标类型的每一个属性
                foreach (var prop in properties)
                {
                    // 查找属性上定义的 SapFieldNameAttribute
                    var attribute = prop.GetCustomAttribute<SapFieldNameAttribute>();

                    // 如果定义了特性，则使用特性中指定的 SAP 字段名；否则忽略该属性
                    if (attribute?.Name != null)
                    {
                        string sapFieldName = attribute?.Name;

                        try
                        {
                            // 从当前 SAP 行中获取值
                            object sapValue = row.GetValue(sapFieldName);

                            // 进行类型转换并设置属性值
                            // 注意：这里需要更复杂的类型处理逻辑来应对 DBNull.Value 和不同类型之间的转换
                            if (sapValue != null && sapValue != DBNull.Value)
                            {
                                // 将 sapValue 转换为属性的类型
                                object convertedValue = ChangeTypeExtended(sapValue, prop.PropertyType);
                                prop.SetValue(item, convertedValue);
                            }
                        }
                        catch (RfcInvalidParameterException)
                        {
                            // 如果 SAP 表中不存在该字段，则忽略，避免程序崩溃
                        }
                    }
 

                }
                resultList.Add(item);
            }
            return resultList;
        }

        /// <summary>
        /// 一个更强大的 ChangeType 版本，支持 Nullable<T>、Enums 和 Guids。
        /// </summary>
        /// <param name="value">要转换的值</param>
        /// <param name="conversionType">目标类型</param>
        /// <returns>转换后的对象</returns>
        public static object ChangeTypeExtended(object value, Type conversionType)
        {
            // 如果值是 null 或 DBNull，直接返回 null
            if (value == null || value == DBNull.Value)
            {
                // 只有当目标类型是引用类型或 Nullable<> 时，才能返回 null
                if (!conversionType.IsValueType || (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    return null;
                }
                // 否则，对于非空值类型，返回其默认值
                return Activator.CreateInstance(conversionType);
            }

            // --- 处理 Nullable<T> ---
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // 获取 Nullable<T> 的基础类型 (例如, 从 int? 获取 int)
                Type underlyingType = Nullable.GetUnderlyingType(conversionType);
                // 递归调用，将值转换为基础类型
                return ChangeTypeExtended(value, underlyingType);
            }

            // --- 处理枚举 Enum ---
            if (conversionType.IsEnum)
            {
                // 可以从字符串或整数转换
                return Enum.Parse(conversionType, value.ToString());
            }

            // --- 处理 GUID ---
            if (conversionType == typeof(Guid))
            {
                return Guid.Parse(value.ToString());
            }

            // --- 回退到标准的 Convert.ChangeType 处理其他基础类型 ---
            return Convert.ChangeType(value, conversionType);
        }

        /// <summary>
        /// 将一个 C# 对象列表映射并填充到一个 IRfcTable 中。
        /// </summary>
        /// <typeparam name="T">C# 对象的类型，其属性应使用 [SapFieldName] 标记。</typeparam>
        /// <param name="sourceList">包含源数据的 C# 对象列表。</param>
        /// <param name="destinationTable">从 RFC 获取的、待填充的空 IRfcTable。</param>
        public static void MapToRfcTable<T>(IEnumerable<T> sourceList, IRfcTable destinationTable) where T : class
        {
            if (sourceList == null)
                throw new ArgumentNullException(nameof(sourceList));
            if (destinationTable == null)
                throw new ArgumentNullException(nameof(destinationTable));

            // 性能优化：缓存属性与SAP字段名的映射关系
            var propertyMap = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p => new
                {
                    Property = p,
                    SapAttribute = p.GetCustomAttribute<SapFieldNameAttribute>()
                })
                .Where(p => p.SapAttribute != null) // 只处理标记了 SapFieldName 的属性
                .ToDictionary(p => p.Property, p => p.SapAttribute.Name);

            // 遍历源数据列表
            foreach (var sourceItem in sourceList)
            {
                destinationTable.Append(); // 在 SAP 表中创建一个新行
                IRfcStructure currentRow = destinationTable.CurrentRow;

                // 遍历所有需要映射的属性
                foreach (var entry in propertyMap)
                {
                    var property = entry.Key;
                    var sapFieldName = entry.Value;

                    // 获取属性的值
                    object value = property.GetValue(sourceItem);

                    // 如果值不为 null，则设置到 SAP 表的对应字段
                    if (value != null)
                    {
                        try
                        {
                            currentRow.SetValue(sapFieldName, value);
                        }
                        catch (RfcAbapException ex)
                        {
                            // 处理可能的类型不匹配或字段不存在的错误
                            Console.WriteLine($"无法设置字段 '{sapFieldName}' 的值 '{value}': {ex.Message}");
                            // 根据需要可以选择抛出异常或继续
                        }
                    }
                }
            }
        }
    }
}
