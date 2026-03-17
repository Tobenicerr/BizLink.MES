using AntdUI;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
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


        /// <summary>
        /// 将单列数据拆分为多列导出，适配 A4 纸打印
        /// </summary>
        /// <param name="dataList">数据源列表</param>
        /// <param name="filePath">保存文件的完整路径 (例如 "D:\\Result.xlsx")</param>
        /// <param name="headerText">每组数据的列头标题 (小标题)</param>
        /// <param name="mainTitle">表格顶部的大标题 (如 "2023年度检测报告")，为空则不显示</param>
        /// <param name="maxRowsPerColumn">每栏最大行数 (建议 40-50 行适配 A4)</param>
        /// <param name="startRow">数据写入的起始行 (预留给标题)</param>
        /// <param name="gapColCount">分栏之间的间隔列数</param>
        public static void ExportSplitColumnsToA4(
            Form form,
            List<string> dataList,
            string headerText = "导出结果",
            string mainTitle = null,
            int maxRowsPerColumn = 45,
            int startRow = 2,
            int gapColCount = 1)
        {

            // 弹出文件保存对话框，让用户选择保存路径
            using (var sfd = new SaveFileDialog()) 
            {

                sfd.Filter = "Excel Files (*.xlsx)|*.xlsx";
                sfd.FileName = $"{mainTitle}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK) 
                {
                    try
                    {
                        // 核心配置参数
                        const int DataColCount = 1; // 目前仅支持单列数据的拆分

                        // 如果有大标题，确保数据至少从第3行开始 (1行大标题 + 1行小标题)
                        if (!string.IsNullOrEmpty(mainTitle) && startRow < 3)
                        {
                            startRow = 3;
                        }

                        using (var workbook = new XLWorkbook())
                        {
                            var ws = workbook.Worksheets.Add("sheet1");

                            // 1. 循环写入数据 (蛇形分列算法)
                            for (int i = 0; i < dataList.Count; i++)
                            {
                                // 计算当前数据应该在哪一栏 (Group)
                                int groupIndex = i / maxRowsPerColumn;

                                // 计算当前数据在该栏内的行索引
                                int rowIndexInGroup = i % maxRowsPerColumn;

                                // 计算实际写入的 Excel 行号
                                int excelRow = startRow + rowIndexInGroup;

                                // 计算实际写入的 Excel 列号
                                // 逻辑：(组索引 * (数据列数 + 间隔列数)) + 1
                                int excelCol = 1 + (groupIndex * (DataColCount + gapColCount));

                                // 写入数据
                                ws.Cell(excelRow, excelCol).Value = dataList[i];

                                // --- 处理列头 (小标题) 和样式 (每组的第一行触发) ---
                                if (rowIndexInGroup == 0)
                                {
                                    // 设置列标题
                                    ws.Cell(startRow - 1, excelCol).Value = headerText;
                                    ws.Cell(startRow - 1, excelCol).Style.Font.Bold = true;
                                    ws.Cell(startRow - 1, excelCol).Style.Fill.BackgroundColor = XLColor.LightGray;
                                    ws.Cell(startRow - 1, excelCol).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                                    // 给整列数据加边框
                                    // 注意：如果是最后一组，可能行数不足 maxRowsPerColumn，需要计算实际结束行
                                    int currentGroupRowCount = Math.Min(maxRowsPerColumn, dataList.Count - (groupIndex * maxRowsPerColumn));
                                    var range = ws.Range(excelRow, excelCol, excelRow + currentGroupRowCount - 1, excelCol);
                                    range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                                }
                            }

                            // 2. 添加大标题 (如果提供了 mainTitle)
                            if (!string.IsNullOrEmpty(mainTitle) && dataList.Count > 0)
                            {
                                // 计算总列数，用于合并单元格
                                int totalGroups = (int)Math.Ceiling((double)dataList.Count / maxRowsPerColumn);
                                int lastColIndex = 1 + ((totalGroups - 1) * (DataColCount + gapColCount));

                                // 合并第一行所有涉及的列
                                var titleRange = ws.Range(1, 1, 1, lastColIndex);
                                titleRange.Merge();
                                titleRange.Value = mainTitle;

                                // 设置大标题样式：加粗、加大、居中
                                titleRange.Style.Font.Bold = true;
                                titleRange.Style.Font.FontSize = 16;
                                titleRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                titleRange.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                            }

                            // 3. A4 打印设置 (关键)
                            var pageSetup = ws.PageSetup;

                            // 设置纸张大小
                            pageSetup.PaperSize = XLPaperSize.A4Paper;

                            // 设置页边距
                            pageSetup.Margins.Left = 0.5;
                            pageSetup.Margins.Right = 0.5;
                            pageSetup.Margins.Top = 0.5;
                            pageSetup.Margins.Bottom = 0.5;

                            // 强制将所有列缩放到一页宽 (高度自动，不限制页数)
                            pageSetup.FitToPages(1, 0);

                            // 设置打印居中
                            pageSetup.CenterHorizontally = true;

                            // 4. 调整列宽
                            ws.Columns().AdjustToContents();

                            // 调整间隔列的宽度 (让它变窄一点，作为视觉分隔)
                            int totalGroupsForWidth = (int)Math.Ceiling((double)dataList.Count / maxRowsPerColumn);
                            for (int g = 0; g < totalGroupsForWidth - 1; g++)
                            {
                                int gapColIndex = 1 + DataColCount + (g * (DataColCount + gapColCount));
                                ws.Column(gapColIndex).Width = 2; // 间隔列宽设小一点
                            }

                            // 5. 保存
                            workbook.SaveAs(sfd.FileName);
                        }

                        // 询问用户是否立即打开文件
                        if (AntdUI.Modal.open(form, "导出成功", "Excel 文件已成功导出！\n是否立即打开？", TType.Success) == DialogResult.OK)
                        {
                            // 使用默认程序打开文件
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
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
