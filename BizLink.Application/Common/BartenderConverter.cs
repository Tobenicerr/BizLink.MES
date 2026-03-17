using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BizLink.MES.Application.Common
{
    public static class BartenderConverter
    {

        /// <summary>
        /// 生成 BarTender 打印用的 XML 脚本
        /// </summary>
        /// <typeparam name="T">数据对象的类型</typeparam>
        /// <param name="data">包含数据的对象实例</param>
        /// <param name="layoutPath">BTW 模板文件的完整路径</param>
        /// <param name="printerName">打印机名称</param>
        /// <param name="copies">打印份数 (默认1)</param>
        /// <returns>完整的 XML 字符串</returns>
        public static string GenerateXml<T>(T data, string layoutPath, string printerName, int copies = 1)
        {
            // 1. 先将对象转换为符合要求的 CSV 格式 (带双引号)
            string csvContent = ConvertToQuotedCsv(data);

            // 2. 构建 XML 结构
            // 使用 XDocument 可以自动处理格式、缩进和特殊字符
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("XMLScript",
                    new XAttribute("Version", "2.0"),
                    new XElement("Command",
                        new XAttribute("Name", "Report Label"),
                        new XElement("Print",
                            // 模板路径
                            new XElement("Format", layoutPath),

                            // 打印设置
                            new XElement("PrintSetup",
                                new XElement("Printer", printerName),
                                new XElement("IdenticalCopiesOfLabel", copies)
                            ),

                            // 数据记录集设置
                            new XElement("RecordSet",
                                new XAttribute("Type", "btTextFile"),
                                new XElement("Delimitation", "btDelimQuoteAndComma"),
                                new XElement("UseFieldNamesFromFirstRecord", "true"),

                                // 关键部分：使用 XCData 来生成 <![CDATA[ ... ]]>
                                new XElement("TextData", new XCData(csvContent))
                            )
                        )
                    )
                )
            );

            // 返回字符串 (包含声明头)
            return doc.Declaration + Environment.NewLine + doc.ToString();
        }
        /// <summary>
        /// 将对象转换为带双引号的 CSV 格式字符串 (两行：表头+数据)
        /// </summary>
        /// 
        /// <summary>
        /// 重载：生成支持多条数据的 BarTender XML 脚本
        /// </summary>
        public static string GenerateXml<T>(IEnumerable<T> dataList, string layoutPath, string printerName, int copies = 1)
        {
            // 1. 将集合转换为多行 CSV 格式
            string csvContent = ConvertListToQuotedCsv(dataList);

            // 2. 构建 XML 结构 (逻辑与单体一致)
            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", null),
                new XElement("XMLScript",
                    new XAttribute("Version", "2.0"),
                    new XElement("Command",
                        new XAttribute("Name", "Report Labels"),
                        new XElement("Print",
                            new XElement("Format", layoutPath),
                            new XElement("PrintSetup",
                                new XElement("Printer", printerName),
                                new XElement("IdenticalCopiesOfLabel", copies)
                            ),
                            new XElement("RecordSet",
                                new XAttribute("Type", "btTextFile"),
                                new XElement("Delimitation", "btDelimQuoteAndComma"),
                                new XElement("UseFieldNamesFromFirstRecord", "true"),
                                new XElement("TextData", new XCData(csvContent))
                            )
                        )
                    )
                )
            );

            return doc.Declaration + Environment.NewLine + doc.ToString();
        }
        private static string ConvertToQuotedCsv<T>(this T obj)
        {
            if (obj == null) return string.Empty;

            var properties = typeof(T).GetProperties();

            // 1. 生成表头 (Header)
            // 格式: "Material","Description","Quantity"...
            var header = string.Join(",", properties.Select(p => $"\"{p.Name}\""));

            // 2. 生成数据 (Data)
            // 格式: "309341","6FX5002...","20"...
            var values = properties.Select(p =>
            {
                var val = p.GetValue(obj);

                // 特殊处理：如果是日期，根据图片格式转为 "yyyy.MM.dd" (如果需要)
                if (val is DateTime dt)
                {
                    return $"\"{dt.ToString("yyyy.MM.dd")}\"";
                }

                // 处理 null 为空字符串，并转义内部可能存在的双引号
                var strVal = val?.ToString() ?? "";

                // 如果内容里本身有双引号，通常需要转义（变成""），根据你的系统要求调整
                // strVal = strVal.Replace("\"", "\"\""); 

                return $"\"{strVal}\"";
            });

            var dataRow = string.Join(",", values);

            // 3. 拼接结果 (表头 + 换行 + 数据)
            // 注意：图片中看起来是直接拼接的，这里用了 Environment.NewLine
            return $"{header}\r\n{dataRow}";
        }

        /// <summary>
        /// 将对象集合转换为带双引号的多行 CSV 字符串
        /// </summary>
        private static string ConvertListToQuotedCsv<T>(IEnumerable<T> dataList)
        {
            if (dataList == null || !dataList.Any()) return string.Empty;

            var properties = typeof(T).GetProperties();
            var sb = new StringBuilder();

            // 1. 生成表头
            var header = string.Join(",", properties.Select(p => $"\"{p.Name}\""));
            sb.AppendLine(header);

            // 2. 遍历集合生成数据行
            foreach (var item in dataList)
            {
                var values = properties.Select(p =>
                {
                    var val = p.GetValue(item);

                    // 时间处理
                    if (val is DateTime dt)
                    {
                        return $"\"{dt.ToString("yyyy.MM.dd")}\"";
                    }

                    // 处理 null 及引号转义
                    var strVal = val?.ToString() ?? "";
                    strVal = strVal.Replace("\"", "\"\""); // CSV 标准转义：双引号变两个双引号

                    return $"\"{strVal}\"";
                });

                sb.AppendLine(string.Join(",", values));
            }

            return sb.ToString().TrimEnd(); // 去掉最后多余的换行
        }
    }

}
