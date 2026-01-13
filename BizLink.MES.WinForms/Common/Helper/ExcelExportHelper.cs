using AntdUI;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.WinForms.Common.Helper
{
    public static class ExcelExportHelper
    {
        /// <summary>
        /// 将一个对象列表导出到 Excel 文件
        /// </summary>
        /// <typeparam name="T">要导出的对象类型</typeparam>
        /// <param name="data">包含数据的 IEnumerable 集合</param>
        /// <param name="sheetName">Excel 工作表的名称</param>
        public static void ExportToExcel<T>(Form form,IEnumerable<T> data, string sheetName = "Sheet1")
        {
            // 弹出文件保存对话框，让用户选择保存路径
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                sfd.FileName = $"{sheetName}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var workbook = new XLWorkbook())
                        {
                            // 创建一个 DataTable
                            var dt = new DataTable(sheetName);

                            // 使用 FastMember 或者简单的反射来填充 DataTable
                            // 这里提供一个简单的反射实现
                            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                            foreach (PropertyDescriptor prop in props)
                            {
                                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                            }
                            foreach (T item in data)
                            {
                                DataRow row = dt.NewRow();
                                foreach (PropertyDescriptor prop in props)
                                {
                                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                                }
                                dt.Rows.Add(row);
                            }

                            // 从 DataTable 创建工作表
                            workbook.Worksheets.Add(dt); // 注意这里的变化
                            var worksheet = workbook.Worksheet(sheetName); // 获取刚创建的工作表
                            worksheet.Columns().AdjustToContents();
                            // (可选) 自动调整所有列的宽度以适应内容
                            worksheet.Columns().AdjustToContents();

                            // 保存 Excel 文件
                            workbook.SaveAs(sfd.FileName);

                            // 询问用户是否立即打开文件
                            if (AntdUI.Modal.open(form, "导出成功", "Excel 文件已成功导出！\n是否立即打开？", TType.Success) == DialogResult.OK)
                            {
                                // 使用默认程序打开文件
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        AntdUI.Message.error(form, $"导出 Excel 时发生错误: {ex.Message}");
                    }
                }
            }
        }
    }
}
