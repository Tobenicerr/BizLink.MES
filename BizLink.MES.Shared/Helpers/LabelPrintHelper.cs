using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using Dm.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Shared.Helpers
{
    /// <summary>
    /// 提供标签打印功能的静态帮助类.
    /// 该类不依赖任何第三方SDK, 直接通过Windows GDI/WinSpool API与打印机通信.
    /// </summary>
    public static class LabelPrintHelper
    {
        // 使用 winspool.drv 中的 Windows API 函数
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        private class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        /// <summary>
        /// 获取系统中所有已安装的打印机名称列表.
        /// </summary>
        /// <returns>包含打印机名称的字符串数组.</returns>
        /// 

        public static string[] GetInstalledPrinters()
        {
            var printerList = new System.Collections.Generic.List<string>();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                printerList.Add(printer);
            }
            return printerList.ToArray();
        }


        // 建议：后续将此路径移至配置文件 (appsettings.json)
        private const string BartenderScanPath = @"\\svcn5bartp01\scan\";
        private const string LayoutBasePath = @"D:\bartender\layout\";
        public static bool ProcessLabelPrinter(string materialCode,string materialDesc,decimal quantity,string orderNo,string operationNo,string leadingMaterial,string workCenter,string nextWorkCenter,int labelCount,DateTime dispatchDate, string bagflag, string linesidelocation, PrinterType printerType, string printerName)
        {
            try
            {
                // 准备数据字段
                string materialCodeE = materialCode.StartsWith("E") ? materialCode.Substring(1) : materialCode;
                //string dispatchDate = dispatchDate.ToString("yyyy-MM-dd") ?? "";

                if (printerType == PrinterType.NetworkPrinter)
                {
                    // 构造 CSV 数据行
                    string csvData = $"\"{materialCode}\",\"{materialDesc}\",\"{Convert.ToInt32(quantity)}\",\"PCS\",\"\",\"{orderNo}-{operationNo}\",\"{orderNo}\",\"\",\"\",\"\",\"{workCenter}{bagflag}\",\"{nextWorkCenter}({linesidelocation})\",\"{leadingMaterial}\",\"{dispatchDate:d}\",\"{labelCount}\"";

                    // 生成 XML
                    string xmlContent = GenerateBartenderXml(
                        $"{LayoutBasePath}BCN_CN11_PROCESSCARD.btw",
                        printerName,
                        csvData
                    );

                    // 保存文件触发打印
                    string filename = $"{materialCode}-{operationNo}-{DateTime.Now:yyyyMMddHHmmss}.xml";
                    return SaveXmlToNetwork(filename, xmlContent);
                }
                else if (printerType == PrinterType.LocalPrinter)
                {
                    // ZPL 插值
                    string zpl = $@"^XA
^CI28
^MMT
^PW799
^LL599
^LS0
^LH20,0^FS
^FO50,50^A0N,35,35^FDOrderNo^FS
^FO300,50^A0N,35,35^FB200,1,0,L,0^FD{orderNo}^FS
^FO470,50^BY1.5,2^BCN,40,N,N,N,A^FD{orderNo}^FS
^FO50,120^A0N,35,35^FDMaterialCode^FS
^FO300,120^A0N,35,35^FB500,1,0,L,0^FD{materialCode}^FS
^FO300,165^BY1.5,2^BCN,40,N,N,N,A^FD{materialCodeE}^FS
^FO50,235^A0N,35,35^FDPrepareWC^FS
^FO300,235^A0N,35,35,E:SIMSUN.FNT^FD{workCenter}{bagflag}^FS
^FO50,290^A0N,35,35^FDAssemblyWC^FS
^FO300,280^A@N,45,40,E:SIMSUN.FNT^FD{nextWorkCenter}({linesidelocation})^FS
^FO50,350^A0N,35,35^FDProductDate^FS
^FO300,350^A0N,35,35^FD{dispatchDate}^FS
^FO50,410^A0N,35,35^FDProductCode^FS
^FO300,410^A0N,35,35^FD{leadingMaterial}^FS
^FO50,470^A0N,35,35^FDOrderQty^FS
^FO300,470^A0N,35,35^FD{Convert.ToInt32(quantity)}^FS
^FO50,530^A0N,35,35^FDLabelCount^FS
^FO300,530^A0N,35,35^FD{labelCount}^FS
^FO520,330^BQN,2,5^FDQA,{orderNo}-{operationNo}^FS
^FO500,480^A0N,30,30^FD{orderNo}-{operationNo}^FS
^FO470,570^A0N,50,70^FD BizLink^FS
^XZ";
                    return RawPrinterHelper.SendStringToPrinter(printerName, zpl);
                }
                return true;
            }
            catch (Exception ex)
            {
                // 建议集成 ILogger 记录 ex
                Console.WriteLine($"Print Error: {ex.Message}");
                return false;
            }
        }


        /// <summary>
        /// Cutting报工标签打印
        /// </summary>
        public static bool CuttingLabelPrinter(string materialCode,string materialDesc,decimal targetQuantity,decimal cuttingLength, string profitCenter,string workOrderNo, string batchCode, string barCode,decimal confirmQuantity, PrinterType printerType, string printerName)
        {
            try
            {
                string cableLength = (cuttingLength / 1000).ToString("0.000");
                //string taskBaseNumber = task.TaskNumber.Split('-')[0];

                if (printerType == PrinterType.NetworkPrinter)
                {
                    string csvData = $"\"{materialCode}\",\"{materialDesc}\",\"{targetQuantity.ToString("F0")}\",\"PCS {profitCenter}\",\"{batchCode}\",\"{barCode}\",\"{workOrderNo}\",\"\",\"{cableLength}\",\"{Convert.ToInt32(confirmQuantity.ToString("F0"))}\"";

                    string xmlContent = GenerateBartenderXml(
                        $"{LayoutBasePath}BCN_CN11_CUT.btw",
                        printerName,
                        csvData
                    );

                    string filename = $"{workOrderNo}CUT{DateTime.Now:yyyyMMddHHmmss}.xml";
                    return SaveXmlToNetwork(filename, xmlContent);
                }
                else if (printerType == PrinterType.LocalPrinter)
                {
                    string zpl = $@"^XA
^PW800
^LL400
^LH80,50
^FO0,0^A0N,55,55^FD{materialCode}^FS
^FO280,0^A0N,55,55^FD{workOrderNo}^FS
^FO0,65^A0N,30,30^FD{materialDesc}^FS
^FO0,115^A0N,40,40^FD{batchCode}^FS
^FO220,115^A0N,40,40^FD{cableLength} M^FS
^FO0,190^A0N,50,50^FD{confirmQuantity.ToString("F0")} / {targetQuantity.ToString("F0")} PCS{profitCenter}^FS
^FO480,70^BQN,2,5^FDQA,{barCode}^FS
^FO480,210^A0N,25,25^FD  {barCode}^FS
^XZ";
                    return RawPrinterHelper.SendStringToPrinter(printerName, zpl);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Print Error: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 断线首检标签
        /// </summary>
        /// <param name="materialCode"></param>
        /// <param name="materialDesc"></param>
        /// <param name="workOrderNo"></param>
        /// <param name="batchCode"></param>
        /// <param name="barCode"></param>
        /// <param name="cuttingLength"></param>
        /// <param name="minCuttingLength"></param>
        /// <param name="maxCuttingLength"></param>
        /// <param name="printerType"></param>
        /// <param name="printerName"></param>
        /// <returns></returns>
        public static bool CuttingInspectLabelPrinter(string materialCode,string materialDesc, string workOrderNo, decimal cuttingLength,decimal minCuttingLength,decimal maxCuttingLength, PrinterType printerType, string printerName)
        {
            try
            {

                if (printerType == PrinterType.NetworkPrinter)
                {
                    string csvData = $"\"{materialCode}\",\"{materialDesc}\",\"{maxCuttingLength.ToString("F1")} mm\",\"\",\"\",\"\",\"{workOrderNo}\",\"\",\"{(cuttingLength / 1000).ToString("F3")} \",\"{minCuttingLength.ToString("F1")} mm\"";

                    string xmlContent = GenerateBartenderXml(
                        $"{LayoutBasePath}BCN_CN11_CUT.btw",
                        printerName,
                        csvData
                    );

                    string filename = $"{workOrderNo}CUT{DateTime.Now:yyyyMMddHHmmss}.xml";
                    return SaveXmlToNetwork(filename, xmlContent);
                }
                else if (printerType == PrinterType.LocalPrinter)
                {
                    string zpl = $@"^XA
^PW800
^LL400
^LH80,50
^FO0,0^A0N,55,55^FD{materialCode}^FS
^FO280,0^A0N,55,55^FD{workOrderNo}^FS
^FO0,65^A0N,30,30^FD{materialDesc}^FS
^FO0,115^A0N,40,40^FD^FS
^FO220,115^A0N,40,40^FD{(cuttingLength / 1000).ToString("F3")} M^FS
^ FO0,190^A0N,50,50^FD{minCuttingLength.ToString("F1")} mm / {maxCuttingLength.ToString("F1")} mm^FS
^FO480,70^BQN,2,5^FDQA,^FS
^FO480,210^A0N,25,25^FD  ^FS
^XZ";
                    return RawPrinterHelper.SendStringToPrinter(printerName, zpl);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Print Error: {ex.Message}");
                return false;
            }
        }


        public static bool ShipmentLabelPrinter(PrinterType printerType, string printerName, string materialCode, string materialDesc, string batchCode, string locationCode, decimal cuttingLength, string barCode = null)
        {
            try
            {
                string cutLenStr = cuttingLength.ToString("0.000");

                if (printerType == PrinterType.NetworkPrinter)
                {
                    string csvData = $"\"{materialCode}\",\"{materialDesc}\",\"\",\"\",\"{batchCode}\",\"{barCode ?? ""}\",\"\",\"{locationCode}\",\"{cutLenStr}\",\"\"";

                    string xmlContent = GenerateBartenderXml(
                        $"{LayoutBasePath}BCN_CN11_CUT.btw",
                        printerName,
                        csvData
                    );

                    string filename = $"{batchCode}{DateTime.Now:yyyyMMddHHmmss}.xml";
                    return SaveXmlToNetwork(filename, xmlContent);
                }
                else if (printerType == PrinterType.LocalPrinter)
                {
                    string zpl = $@"^XA
^PW800
^LL400
^LH80,50
^FO0,0^A0N,55,55^FD{materialCode}^FS
^FO280,0^A0N,55,55^FD{locationCode}^FS
^FO0,65^A0N,30,30^FD{materialDesc}^FS
^FO0,115^A0N,40,40^FD{batchCode}^FS
^FO220,115^A0N,40,40^FD{cutLenStr} M^FS
^FO0,190^A0N,50,50^FD PCS^FS
^FO480,70^BQN,2,5^FDQA,{barCode ?? ""}^FS
^FO480,210^A0N,25,25^FD  {barCode ?? ""}^FS
^XZ";
                    return RawPrinterHelper.SendStringToPrinter(printerName, zpl);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Print Error: {ex.Message}");
                return false;
            }
        }

        #region Private Helpers

        /// <summary>
        /// 生成 BarTender 集成 XML (Script 2.0)
        /// </summary>
        private static string GenerateBartenderXml(string layoutPath, string targetPrinter, string csvRecordData)
        {
            // 使用 StringBuilder 避免大字符串内存分配
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sb.AppendLine("<XMLScript Version=\"2.0\">");
            sb.AppendLine("  <Command Name=\"Report Label\">");
            sb.AppendLine("    <Print>");
            sb.AppendLine($"      <Format>{layoutPath}</Format>");
            sb.AppendLine("      <PrintSetup>");
            sb.AppendLine($"        <Printer>{targetPrinter}</Printer>");
            sb.AppendLine("        <IdenticalCopiesOfLabel>1</IdenticalCopiesOfLabel>");
            sb.AppendLine("      </PrintSetup>");
            sb.AppendLine("      <RecordSet Type=\"btTextFile\">");
            sb.AppendLine("        <Delimitation>btDelimQuoteAndComma</Delimitation>");
            sb.AppendLine("        <UseFieldNamesFromFirstRecord>true</UseFieldNamesFromFirstRecord>");
            //sb.AppendLine("        <TextData><![CDATA[\"Material\",\"Description\",\"Quantity\",\"Unit\",\"Batch\",\"PackageID\",\"OrderID\",\"RecLoc\",\"Length\",\"ConfQuantity\""); // Header
            sb.AppendLine("        <TextData><![CDATA[\"Material\",\"Description\",\"Quantity\",\"Unit\",\"Batch\",\"PackageID\",\"OrderID\",\"RecLoc\",\"Length\",\"ConfQuantity\",\"PrepareWC\",\"AssemblyWC\",\"FinishProduct\",\"FinishDate\",\"LabelCount\""); // Header
            sb.AppendLine(csvRecordData); // Data Row
            sb.AppendLine("]]></TextData>");
            sb.AppendLine("      </RecordSet>");
            sb.AppendLine("    </Print>");
            sb.AppendLine("  </Command>");
            sb.AppendLine("</XMLScript>");

            return sb.ToString();
        }

        /// <summary>
        /// 将内容保存到网络路径 (BarTender Scan Folder)
        /// </summary>
        private static bool SaveXmlToNetwork(string filename, string content)
        {
            try
            {
                string fullPath = Path.Combine(BartenderScanPath, filename);

                // 优化：File.WriteAllText 自动处理流的打开与关闭
                File.WriteAllText(fullPath, content, Encoding.UTF8);

                return true;
            }
            catch (IOException ioEx)
            {
                // 处理文件占用等 IO 异常
                throw new Exception($"写入网络路径失败: {ioEx.Message}", ioEx);
            }
            catch (UnauthorizedAccessException authEx)
            {
                // 处理权限异常
                throw new Exception($"无权访问网络路径: {authEx.Message}", authEx);
            }
        }

        #endregion

        /// <summary> 
        /// 将原始的打印命令字符串 (如 ZPL, EPL) 发送到指定的打印机.
        /// 作废
        /// </summary>
        /// <param name="printerName">目标打印机的完整名称.</param>
        /// <param name="commandString">要发送的原始命令字符串.</param>
        /// <returns>如果发送成功, 返回 true; 否则返回 false.</returns>
        /// <exception cref="Exception">当无法打开打印机或写入数据时抛出.</exception>
        public static bool SendStringToPrinter(string printerName, string commandString)
        {
            IntPtr pBytes;
            int dwCount;
            int dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false;

            di.pDocName = "ZPL Document";
            di.pDataType = "RAW";

            // 将字符串转换为非托管内存字节数组
            dwCount = commandString.Length;
            // 使用UTF-8编码以支持更广泛的字符集
            byte[] commandBytes = Encoding.UTF8.GetBytes(commandString);
            pBytes = Marshal.AllocCoTaskMem(commandBytes.Length);
            Marshal.Copy(commandBytes, 0, pBytes, commandBytes.Length);
            dwCount = commandBytes.Length;


            if (OpenPrinter(printerName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    if (StartPagePrinter(hPrinter))
                    {
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }

            Marshal.FreeCoTaskMem(pBytes);

            if (!bSuccess)
            {
                int errorCode = Marshal.GetLastWin32Error();
                throw new Exception($"写入打印机失败, 错误代码: {errorCode}");
            }

            return bSuccess;
        }
        /// <summary>
        ///  作废
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="commandString"></param>
        /// <returns></returns>
        public static bool SaveXMLToBartenderPath(string filename,string commandString)
        {
            bool bSuccess = false;
            try
            {
                string path = @"\\svcn5bartp01\scan\" + filename;
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    fs.SetLength(0);
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(commandString);
                        sw.Close();
                    }
                    fs.Close();
                    bSuccess = true;
                }
            }
            catch (Exception)
            {

                bSuccess = false;
            }

            return bSuccess;

        }

        /// <summary>
        /// CN11流转卡打印
        /// 作废
        /// </summary>
        public static bool ProcessCardPrinter(WorkOrderDto order, WorkOrderProcessDto process,string bagflag, string linesidelocation, PrinterType printerType, string printername)
        {
            try
            {
                string processStr = string.Empty;
                if (printerType == PrinterType.NetworkPrinter)
                {
                    processStr = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
	                     <XMLScript Version=""2.0"">
	                      <Command Name=""Report Label"">
		                    <Print>
		                      <Format>D:\bartender\layout\BCN_CN11_PROCESSCARD.btw</Format>
		                      <PrintSetup>
			                    <Printer>{10}</Printer>
			                    <IdenticalCopiesOfLabel>1</IdenticalCopiesOfLabel>
		                      </PrintSetup>
		                      <RecordSet Type=""btTextFile"">
			                    <Delimitation>btDelimQuoteAndComma</Delimitation>
			                    <UseFieldNamesFromFirstRecord>true</UseFieldNamesFromFirstRecord>
			                    <TextData><![CDATA[""Material"",""Description"",""Quantity"",""Unit"",""Batch"",""PackageID"",""OrderID"",""RecLoc"",""Length"",""ConfQuantity"",""PrepareWC"",""AssemblyWC"",""FinishProduct"",""FinishDate"",""LabelCount""
                               ""{0}"",""{1}"",""{2}"",""PCS"","""",""{3}"",""{4}"","""","""","""",""{5}"",""{6}"",""{7}"",""{8}"",""{9}""]]></TextData>
		                      </RecordSet>
		                    </Print>
	                      </Command>
	                    </XMLScript>", order.MaterialCode, order.MaterialDesc, Convert.ToInt32(order.Quantity).ToString(), order.OrderNumber + "-" + process.Operation, order.OrderNumber, process.WorkCenter + bagflag, $"{process.NextWorkCenter}({linesidelocation})", order.LeadingOrderMaterial, ((DateTime)order.DispatchDate).ToString("yyyy-MM-dd"), order.LabelCount.ToString(), printername);

                    var filename = order.OrderNumber + "-" + process.Operation + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";

                    if (!string.IsNullOrWhiteSpace(processStr))
                        SaveXMLToBartenderPath(filename, processStr);
                }

                else if (printerType == PrinterType.LocalPrinter)
                {
                    processStr = string.Format("^XA  ^CI28  ^MMT  ^PW799  ^LL599  ^LS0    ^LH20,0^FS    ^FO50,50^A0N,35,35^FDOrderNo^FS  ^FO300,50^A0N,35,35^FB200,1,0,L,0^FD{0}^FS  ^FO470,50^BY1.5,2^BCN,40,N,N,N,A^FD{0}^FS    ^FO50,120^A0N,35,35^FDMaterialCode^FS  ^FO300,120^A0N,35,35^FB500,1,0,L,0^FD{1}^FS  ^FO300,165^BY1.5,2^BCN,40,N,N,N,A^FD{2}^FS    ^FO50,235^A0N,35,35^FDPrepareWC^FS  ^FO300,235^A0N,35,35,E:SIMSUN.FNT^FD{3}^FS    ^FO50,290^A0N,35,35^FDAssemblyWC^FS  ^FO300,280^A@N,45,40,E:SIMSUN.FNT^FD{4}^FS    ^FO50,350^A0N,35,35^FDProductDate^FS  ^FO300,350^A0N,35,35^FD{5}^FS    ^FO50,410^A0N,35,35^FDProductCode^FS  ^FO300,410^A0N,35,35^FD{6}^FS    ^FO50,470^A0N,35,35^FDOrderQty^FS  ^FO300,470^A0N,35,35^FD{7}^FS    ^FO50,530^A0N,35,35^FDLabelCount^FS  ^FO300,530^A0N,35,35^FD{8}^FS    ^FO520,330^BQN,2,5^FDQA,{9}^FS  ^FO500,480^A0N,30,30^FD{9}^FS    ^FO470,570^A0N,50,70^FD BizLink^FS  ^XZ", order.OrderNumber, order.MaterialCode, order.MaterialCode.startsWith("E")? order.MaterialCode.Substring(1): order.MaterialCode, process.WorkCenter + bagflag, $"{process.NextWorkCenter}({linesidelocation})", ((DateTime)order.DispatchDate).ToString("yyyy-MM-dd"), order.LeadingOrderMaterial, Convert.ToInt32(order.Quantity).ToString(), order.LabelCount.ToString(), order.OrderNumber + "-" + process.Operation);
                    LabelPrintHelper.SendStringToPrinter(printername, processStr);

                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="task"></param>
        /// <param name="confirm"></param>
        /// <param name="profitCenter"></param>
        /// <param name="batchcode"></param>
        /// <param name="printerType"></param>
        /// <param name="printername"></param>
        /// <returns></returns>
        public static bool CuttingReportPrinter(WorkOrderTaskDto task,WorkOrderTaskConfirmDto confirm,string profitCenter ,string batchcode, PrinterType printerType, string printername)
        {
            var reportStr = string.Empty;
            try
            {
                if (printerType == PrinterType.NetworkPrinter)
                {
                    reportStr = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                     <XMLScript Version=""2.0"">
                      <Command Name=""Report Label"">
                        <Print>
                          <Format>D:\bartender\layout\BCN_CN11_CUT.btw</Format>
                          <PrintSetup>
                            <Printer>{9}</Printer>
                            <IdenticalCopiesOfLabel>1</IdenticalCopiesOfLabel>
                          </PrintSetup>
                          <RecordSet Type=""btTextFile"">
                            <Delimitation>btDelimQuoteAndComma</Delimitation>
                            <UseFieldNamesFromFirstRecord>true</UseFieldNamesFromFirstRecord>
		                    <TextData><![CDATA[""Material"",""Description"",""Quantity"",""Unit"",""Batch"",""PackageID"",""OrderID"",""RecLoc"",""Length"",""ConfQuantity""
		                    ""{0}"",""{1}"",""{2}"",""PCS {3}"",""{4}"",""{5}"",""{6}"","""",""{7}"",""{8}""]]></TextData>
                          </RecordSet>
                        </Print>
                      </Command>
                    </XMLScript>", task.MaterialCode, task.MaterialDesc, Convert.ToInt32(task.Quantity).ToString(), profitCenter, batchcode, confirm.ConfirmNumber, task.TaskNumber.split("-")[0], (task.CableLength.GetValueOrDefault() / 1000).ToString("0.000"), Convert.ToInt32(confirm.ConfirmQuantity).toString(), printername);
                    var filename = task.TaskNumber + "CUT" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";

                    if (!string.IsNullOrWhiteSpace(reportStr))
                        LabelPrintHelper.SaveXMLToBartenderPath(filename, reportStr);
                }
                else if (printerType == PrinterType.LocalPrinter)
                {
                    reportStr = string.Format("^XA  ^PW800  ^LL400  ^LH80,50  ^FO0,0^A0N,55,55^FD{0}^FS  ^FO280,0^A0N,55,55^FD{1}^FS  ^FO0,65^A0N,30,30^FD{2}^FS  ^FO0,115^A0N,40,40^FD{3}^FS  ^FO220,115^A0N,40,40^FD{4} M^FS  ^FO0,190^A0N,50,50^FD{5} PCS{7}^FS  ^FO480,70^BQN,2,5^FDQA,{6}^FS  ^FO480,210^A0N,25,25^FD  {6}^FS  ^XZ", task.MaterialCode, task.TaskNumber.split("-")[0], task.MaterialDesc, batchcode, (task.CableLength.GetValueOrDefault() / 1000).ToString("0.000"), Convert.ToInt32(confirm.ConfirmQuantity).toString() + " / " + Convert.ToInt32(task.Quantity).ToString(), confirm.ConfirmNumber, profitCenter);

                    if (!string.IsNullOrWhiteSpace(reportStr))
                        LabelPrintHelper.SendStringToPrinter(printername, reportStr);
                }

                return true;
            }
            catch (Exception)
            {

                return false;   
            }

        }
        /// <summary>
        /// 作废
        /// </summary>
        /// <param name="printerType"></param>
        /// <param name="printername"></param>
        /// <param name="materialCode"></param>
        /// <param name="materialDesc"></param>
        /// <param name="batchCode"></param>
        /// <param name="locationCode"></param>
        /// <param name="cuttingLength"></param>
        /// <param name="barCode"></param>
        /// <returns></returns>
        public static bool ShipmentOfRawMaterialPrinter(PrinterType printerType, string printername,string materialCode,string materialDesc,string batchCode,string locationCode,decimal cuttingLength,string? barCode = null )
        {
            var reportStr = string.Empty;
            try
            {
                if (printerType == PrinterType.NetworkPrinter)
                {
                    reportStr = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                     <XMLScript Version=""2.0"">
                      <Command Name=""Report Label"">
                        <Print>
                          <Format>D:\bartender\layout\BCN_CN11_CUT.btw</Format>
                          <PrintSetup>
                            <Printer>{9}</Printer>
                            <IdenticalCopiesOfLabel>1</IdenticalCopiesOfLabel>
                          </PrintSetup>
                          <RecordSet Type=""btTextFile"">
                            <Delimitation>btDelimQuoteAndComma</Delimitation>
                            <UseFieldNamesFromFirstRecord>true</UseFieldNamesFromFirstRecord>
		                    <TextData><![CDATA[""Material"",""Description"",""Quantity"",""Unit"",""Batch"",""PackageID"",""OrderID"",""RecLoc"",""Length"",""ConfQuantity""
		                   ""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",""{5}"",""{6}"","""",""{7}"",""{8}""]]></TextData>
                          </RecordSet>
                        </Print>
                      </Command>
                    </XMLScript>", materialCode, materialDesc, string.Empty, string.Empty, batchCode, barCode ?? string.Empty, string.Empty, cuttingLength.ToString("0.000"), locationCode, printername);
                    var filename = batchCode + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xml";

                    if (!string.IsNullOrWhiteSpace(reportStr))
                        LabelPrintHelper.SaveXMLToBartenderPath(filename, reportStr);
                }
                else if (printerType == PrinterType.LocalPrinter)
                {
                    reportStr = string.Format("^XA  ^PW800  ^LL400  ^LH80,50  ^FO0,0^A0N,55,55^FD{0}^FS  ^FO280,0^A0N,55,55^FD{1}^FS  ^FO0,65^A0N,30,30^FD{2}^FS  ^FO0,115^A0N,40,40^FD{3}^FS  ^FO220,115^A0N,40,40^FD{4} M^FS  ^FO0,190^A0N,50,50^FD{5} PCS{7}^FS  ^FO480,70^BQN,2,5^FDQA,{6}^FS  ^FO480,210^A0N,25,25^FD  {6}^FS  ^XZ", materialCode, locationCode, materialDesc, batchCode, cuttingLength.ToString("0.000"), string.Empty, barCode ?? string.Empty, string.Empty);

                    if (!string.IsNullOrWhiteSpace(reportStr))
                        LabelPrintHelper.SendStringToPrinter(printername, reportStr);
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
