using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Shared.Helpers
{
    /// <summary>
    /// 底层打印机通信帮助类 (P/Invoke)
    /// </summary>
    public static class RawPrinterHelper
    {
        #region Win32 API
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)] public string pDataType;
        }

        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        private static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, int dwCount, out int dwWritten);
        #endregion

        /// <summary>
        /// 获取已安装打印机列表
        /// </summary>
        public static string[] GetInstalledPrinters()
        {
            // 优化：直接使用 Linq 转换，更简洁
            return System.Linq.Enumerable.ToArray(PrinterSettings.InstalledPrinters.Cast<string>());
        }

        /// <summary>
        /// 发送原始字符串(ZPL/EPL)到本地打印机
        /// </summary>
        public static bool SendStringToPrinter(string printerName, string commandString)
        {
            IntPtr hPrinter = IntPtr.Zero;
            IntPtr pBytes = IntPtr.Zero;

            try
            {
                // 1. 打开打印机
                if (!OpenPrinter(printerName.Normalize(), out hPrinter, IntPtr.Zero))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error(), $"无法打开打印机: {printerName}");
                }

                // 2. 准备数据 (放在 try 块内以确保 finally 能释放)
                byte[] commandBytes = Encoding.UTF8.GetBytes(commandString);
                int dwCount = commandBytes.Length;
                pBytes = Marshal.AllocCoTaskMem(dwCount);
                Marshal.Copy(commandBytes, 0, pBytes, dwCount);

                // 3. 开始打印文档
                var di = new DOCINFOA { pDocName = "MES RAW Document", pDataType = "RAW" };

                if (!StartDocPrinter(hPrinter, 1, di))
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "StartDocPrinter Failed");

                // 4. 开始打印页
                if (!StartPagePrinter(hPrinter))
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "StartPagePrinter Failed");

                // 5. 写入数据
                if (!WritePrinter(hPrinter, pBytes, dwCount, out int dwWritten))
                    throw new Win32Exception(Marshal.GetLastWin32Error(), "WritePrinter Failed");

                // 6. 结束页和文档
                EndPagePrinter(hPrinter);
                EndDocPrinter(hPrinter);

                return true;
            }
            finally
            {
                // 确保非托管资源一定被释放
                if (hPrinter != IntPtr.Zero) ClosePrinter(hPrinter);
                if (pBytes != IntPtr.Zero) Marshal.FreeCoTaskMem(pBytes);
            }
        }
    }
}
