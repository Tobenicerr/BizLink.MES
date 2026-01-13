using BizLink.MES.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Drawing.Printing.PrinterSettings;

namespace BizLink.MES.WinForms.Common.Helper
{
    public static class PrinterHelper
    {
        public static List<object> GetPrinters()
        {
            // PrinterSettings.InstalledPrinters 会返回一个 StringCollection
            StringCollection printerNames = PrinterSettings.InstalledPrinters;
            List<object> printers = new List<object>();
            if (printerNames.Count == 0)
                return new List<object>();
            foreach (string printerName in printerNames)
            {
                try
                {
                    PrinterSettings settings = new PrinterSettings();
                    settings.PrinterName = printerName; // 指定要查询的打印机

                    if (settings.IsValid)
                    {
                        //Console.WriteLine($"  - 是否有效: {settings.IsValid}");
                        //Console.WriteLine($"  - 是否为默认打印机: {settings.IsDefaultPrinter}");

                        // 4. 判断是本地打印机还是网络打印机
                        // 通常，网络打印机的名称是以 "\\" 开头的 UNC 路径
                        if (printerName.StartsWith(@"\\"))
                        {
                            printers.Add(new
                            {
                                PrinterName = printerName,
                                printerType = PrinterType.NetworkPrinter,
                            });
                        }
                        else
                        {
                            printers.Add(new
                            {
                                PrinterName = printerName,
                                printerType = PrinterType.LocalPrinter,
                            });
                        }
                    }


                }
                catch (Exception exDetail)
                {
                    return new List<object>();
                }
            }

            return printers;


        }
    }
}
