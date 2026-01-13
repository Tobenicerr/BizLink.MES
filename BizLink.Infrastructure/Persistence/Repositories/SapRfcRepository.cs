using Azure.Core;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class SapRfcRepository : ISapRfcRepository
    {
        private readonly RfcDestination _destination;

        public SapRfcRepository(IConfiguration configuration)
        {
            var rfcConfig = new RfcConfigParameters();
            // ... (此处省略了从 configuration 添加参数的代码, 与原指南相同) ...
            rfcConfig.Add(RfcConfigParameters.Name, configuration["Connections:SapRfcConnection:Name"]);
            rfcConfig.Add(RfcConfigParameters.AppServerHost, configuration["Connections:SapRfcConnection:ApplicationServer"]);
            rfcConfig.Add(RfcConfigParameters.SystemNumber, configuration["Connections:SapRfcConnection:SystemNumber"]);
            rfcConfig.Add(RfcConfigParameters.User, configuration["Connections:SapRfcConnection:User"]);
            rfcConfig.Add(RfcConfigParameters.Password, configuration["Connections:SapRfcConnection:Password"]);
            rfcConfig.Add(RfcConfigParameters.Client, configuration["Connections:SapRfcConnection:Client"]);
            rfcConfig.Add(RfcConfigParameters.Language, configuration["Connections:SapRfcConnection:Language"]);
            rfcConfig.Add(RfcConfigParameters.PoolSize, configuration["Connections:SapRfcConnection:PoolSize"]);

            _destination = RfcDestinationManager.GetDestination(rfcConfig);
        }

        public async Task<WorkOrderOperationConfirm?> ConfirmOrderCompletionToSAPAsync(WorkOrderOperationConfirm confirm)
        {
            return await Task.Run(() => 
            {
                try
                {
                    IRfcFunction rfcFunction = _destination.Repository.CreateFunction("ZC_XX_MESDATACONF");
                    rfcFunction.SetValue("FUNC", "F");
                    IRfcTable confirmtable = rfcFunction.GetTable("CONFDATA");
                    IRfcTable consumptable = rfcFunction.GetTable("CONSUMPDATA");
                    RfcTableExtensions.MapToRfcTable(new List<WorkOrderOperationConfirm>() { confirm }, confirmtable);
                    if (confirm.Consumps != null && confirm.Consumps.Count() > 0)
                    {
                        RfcTableExtensions.MapToRfcTable(confirm.Consumps, consumptable);
                    }
                    rfcFunction.Invoke(_destination);
                    return RfcTableExtensions.ToList<WorkOrderOperationConfirm>(rfcFunction.GetTable("CONFDATA")).FirstOrDefault();
                }
                catch (RfcCommunicationException ex) { throw new Exception("无法连接到 SAP 系统。", ex); }
                catch (RfcLogonException ex) { throw new Exception("SAP 登录失败。", ex); }
                catch (Exception ex) { throw new Exception("调用 SAP RFC 时发生未知错误。", ex); }
            });
        }

        public async Task<List<CableCutParam>> GetCableCutParamByMaterialsAsync(List<string> semimaterialcode)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IRfcFunction rfcFunction = _destination.Repository.CreateFunction("ZC_XX_PPMASTERDATA");
                    rfcFunction.SetValue("FUNC", "C");
                    rfcFunction.SetValue("ERDAT_START", DateTime.MinValue.ToString("yyyy-MM-dd"));
                    rfcFunction.SetValue("ERDAT_END", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                    rfcFunction.SetValue("AEDAT_START", DateTime.MinValue.ToString("yyyy-MM-dd"));
                    rfcFunction.SetValue("AEDAT_END", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                    IRfcTable tSAP = rfcFunction.GetTable("CUTTINGDATA");
                    foreach (var item in semimaterialcode)
                    {
                        tSAP.Append();
                        tSAP.CurrentRow.SetValue("SEMINUMBER", item.StartsWith('E')? item: item.PadLeft(18, '0'));
                    }
                    rfcFunction.Invoke(_destination);
                    return RfcTableExtensions.ToList<CableCutParam>(rfcFunction.GetTable("CUTTINGDATA"));
   
                }
                catch (RfcCommunicationException ex) { throw new Exception("无法连接到 SAP 系统。", ex); }
                catch (RfcLogonException ex) { throw new Exception("SAP 登录失败。", ex); }
                catch (Exception ex) { throw new Exception("调用 SAP RFC 时发生未知错误。", ex); }

            });


        }

        public async Task<List<Material>> GetSAPMaterialAsync(string factoryCode, List<string>? materialCodes, DateTime? startTime, DateTime? endTime)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IRfcFunction rfcFunction = _destination.Repository.CreateFunction("ZC_XX_PPMASTERDATA");
                    rfcFunction.SetValue("FUNC", "M");
                    IRfcTable tSAP = rfcFunction.GetTable("MATERIALDATA");
                    List<Material> result = new List<Material>();
                    if (materialCodes == null || materialCodes.Count() == 0)
                    {

                        //按创建日期范围查询
                        rfcFunction.SetValue("ERDAT_START", ((DateTime)startTime).ToString("yyyy-MM-dd"));
                        rfcFunction.SetValue("ERDAT_END", (endTime ?? DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd"));
                        rfcFunction.SetValue("AEDAT_START", DateTime.MinValue.ToString("yyyy-MM-dd"));
                        rfcFunction.SetValue("AEDAT_END", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));

                        tSAP.Append();
                        tSAP.CurrentRow.SetValue("WERKS", factoryCode);

                        rfcFunction.Invoke(_destination);
                        result.AddRange(RfcTableExtensions.ToList<Material>(rfcFunction.GetTable("MATERIALDATA")));

                        tSAP.Clear();
                        //按修改日期范围查询
                        rfcFunction.SetValue("ERDAT_START", DateTime.MinValue.ToString("yyyy-MM-dd"));
                        rfcFunction.SetValue("ERDAT_END", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                        rfcFunction.SetValue("AEDAT_START", ((DateTime)startTime).ToString("yyyy-MM-dd"));
                        rfcFunction.SetValue("AEDAT_END", (endTime ?? DateTime.Now.AddDays(1)).ToString("yyyy-MM-dd"));

                        tSAP.Append();
                        tSAP.CurrentRow.SetValue("WERKS", factoryCode);

                        rfcFunction.Invoke(_destination);
                        result.AddRange(RfcTableExtensions.ToList<Material>(rfcFunction.GetTable("MATERIALDATA")));

                    }
                    else
                    {

                        rfcFunction.SetValue("ERDAT_START", DateTime.MinValue.ToString("yyyy-MM-dd"));
                        rfcFunction.SetValue("ERDAT_END", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                        rfcFunction.SetValue("AEDAT_START", DateTime.MinValue.ToString("yyyy-MM-dd"));
                        rfcFunction.SetValue("AEDAT_END", DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"));
                        foreach (var item in materialCodes)
                        {
                            tSAP.Append();
                            tSAP.CurrentRow.SetValue("MATNR", item.StartsWith('E') ? item : item.PadLeft(18, '0'));
                            tSAP.CurrentRow.SetValue("WERKS", factoryCode);
                        }

                        rfcFunction.Invoke(_destination);
                        result.AddRange(RfcTableExtensions.ToList<Material>(rfcFunction.GetTable("MATERIALDATA")));
                    }
                    return result;
                }
                catch (RfcCommunicationException ex) { throw new Exception("无法连接到 SAP 系统。", ex); }
                catch (RfcLogonException ex) { throw new Exception("SAP 登录失败。", ex); }
                catch (Exception ex) { throw new Exception("调用 SAP RFC 时发生未知错误。", ex); }
            });
        }

        public async Task<(List<SapOrderOperation> sapOperation, List<SapOrderBom> sapBom)> GetWorkOrdersAsync(string plantcode,DateTime? dispatchdate,List<string> orders = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IRfcFunction rfcFunction = _destination.Repository.CreateFunction("ZC_XX_PROORDDATA");
                    rfcFunction.SetValue("Plant", plantcode);
                    if (orders == null || orders.Count == 0)
                        rfcFunction.SetValue("OP_Latest_Finish", ((DateTime)dispatchdate).ToString("yyyy-MM-dd"));
                    else
                    {
                        IRfcTable tSAP = rfcFunction.GetTable("ORDER");
                        foreach (var order in orders)
                        {
                            tSAP.Append();
                            tSAP.CurrentRow.SetValue("AUFNR", order);
                        }

                    }
                    //tSAP.CurrentRow.SetValue("DISPATCH_DATE", DateTime.Now.ToString("yyyy-MM-dd"));


                    rfcFunction.Invoke(_destination);

                    var operations
                    = RfcTableExtensions.ToList<SapOrderOperation>(rfcFunction.GetTable("ORDEROP"));
                    var boms = RfcTableExtensions.ToList<SapOrderBom>(rfcFunction.GetTable("ORDERBOM"));

                    return (operations, boms);
                }
                catch (RfcCommunicationException ex) { throw new Exception("无法连接到 SAP 系统。", ex); }
                catch (RfcLogonException ex) { throw new Exception("SAP 登录失败。", ex); }
                catch (Exception ex) { throw new Exception("调用 SAP RFC 时发生未知错误。", ex); }
            });
        }

        public async Task<(List<SapOrderOperation> sapOperation, List<SapOrderBom> sapBom)> GetCN10WorkOrdersAsync(string plantcode, DateTime? dispatchdate, List<string> workcentercode,List<string> orders = null)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IRfcFunction rfcFunction = _destination.Repository.CreateFunction("ZC_XX_MESDATATRANSFER");
                    rfcFunction.SetValue("FUNC", "O");
                    IRfcTable tSAP = rfcFunction.GetTable("ORDDATA");
                    if (orders == null || orders.Count == 0)
                    {
                        foreach (var workcenter in workcentercode)
                        {
                            tSAP.Append();
                            tSAP.CurrentRow.SetValue("PLANT", plantcode);
                            tSAP.CurrentRow.SetValue("DISPATCH_DATE", dispatchdate);
                            tSAP.CurrentRow.SetValue("WORK_CENTER", workcenter);
                        }
  
                    }
                    else
                    {
                        foreach (var order in orders)
                        {
                            tSAP.Append();
                            tSAP.CurrentRow.SetValue("PLANT", plantcode);
                            tSAP.CurrentRow.SetValue("ORDER_NUMBER", order);
                        }

                    }
                    //tSAP.CurrentRow.SetValue("DISPATCH_DATE", DateTime.Now.ToString("yyyy-MM-dd"));


                    rfcFunction.Invoke(_destination);

                    var operations
                    = RfcTableExtensions.ToList<SapOrderOperation>(rfcFunction.GetTable("ORDDATA"));
                    var boms = RfcTableExtensions.ToList<SapOrderBom>(rfcFunction.GetTable("RESDATA"));

                    return (operations, boms);
                }
                catch (RfcCommunicationException ex) { throw new Exception("无法连接到 SAP 系统。", ex); }
                catch (RfcLogonException ex) { throw new Exception("SAP 登录失败。", ex); }
                catch (Exception ex) { throw new Exception("调用 SAP RFC 时发生未知错误。", ex); }
            });
        }

        public async Task<List<MaterialTransferLog>> MaterialStockTransferToSAPAsync(List<MaterialTransferLog> input)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IRfcFunction rfcFunction = _destination.Repository.CreateFunction("ZM_XX_WMSDATA");
                    rfcFunction.SetValue("FUNC", "M");
                    IRfcTable tSAP = rfcFunction.GetTable("GMDATA");

                    RfcTableExtensions.MapToRfcTable(input, tSAP);
                    rfcFunction.Invoke(_destination);
                    var result =  RfcTableExtensions.ToList<MaterialTransferLog>(rfcFunction.GetTable("GMDATA"));
                    return result;
                }
                catch (RfcCommunicationException ex) { throw new Exception("无法连接到 SAP 系统。", ex); }
                catch (RfcLogonException ex) { throw new Exception("SAP 登录失败。", ex); }
                catch (Exception ex) { throw new Exception("调用 SAP RFC 时发生未知错误。", ex); }
            });

        }

        public async Task<List<SapRawMaterialStock>> GetRawMaterialStockFromSapAsync(List<SapRawMaterialStock> materialCodes)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IRfcFunction rfcFunction = _destination.Repository.CreateFunction("ZC_XX_PPMASTERDATA");
                    rfcFunction.SetValue("FUNC", "I");
                    IRfcTable tSAP = rfcFunction.GetTable("STOCKSDATA");
                    foreach (var item in materialCodes)
                    {
                        tSAP.Append();
                        tSAP.CurrentRow.SetValue("MATNR", item.MaterialCode.StartsWith('E') ? item.MaterialCode : item.MaterialCode.PadLeft(18, '0'));
                        tSAP.CurrentRow.SetValue("WERKS", item.FactoryCode);
                        if (!string.IsNullOrWhiteSpace(item.LocationCode))
                        {
                            tSAP.CurrentRow.SetValue("LGORT", item.LocationCode);

                        }
                        if (!string.IsNullOrWhiteSpace(item.BatchCode))
                        {
                            tSAP.CurrentRow.SetValue("CHARG", item.BatchCode);
                        }
                    }
                    rfcFunction.Invoke(_destination);
                    return RfcTableExtensions.ToList<SapRawMaterialStock>(rfcFunction.GetTable("STOCKSDATA"));
                }
                catch (RfcCommunicationException ex) { throw new Exception("无法连接到 SAP 系统。", ex); }
                catch (RfcLogonException ex) { throw new Exception("SAP 登录失败。", ex); }
                catch (Exception ex) { throw new Exception("调用 SAP RFC 时发生未知错误。", ex); }
            });
        }

        public async Task<List<MaterialTransferLog>> RawMaterialInventoryAdjustmentAsync(List<MaterialTransferLog> input)
        {
            return await Task.Run(() =>
            {
                try
                {
                    IRfcFunction rfcFunction = _destination.Repository.CreateFunction("ZM_XX_WMSDATA");
                    rfcFunction.SetValue("FUNC", "C");
                    IRfcTable tSAP = rfcFunction.GetTable("GMDATA");

                    RfcTableExtensions.MapToRfcTable(input, tSAP);
                    rfcFunction.Invoke(_destination);
                    var result = RfcTableExtensions.ToList<MaterialTransferLog>(rfcFunction.GetTable("GMDATA"));
                    return result;
                }
                catch (RfcCommunicationException ex) { throw new Exception("无法连接到 SAP 系统。", ex); }
                catch (RfcLogonException ex) { throw new Exception("SAP 登录失败。", ex); }
                catch (Exception ex) { throw new Exception("调用 SAP RFC 时发生未知错误。", ex); }
            });

        }
    }
}
