using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Helper;
using BizLink.MES.Application.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Facade
{
    /// <summary>
    /// 切割/断线模块外观服务
    /// 聚合了该模块所需的所有特定业务服务
    /// </summary>
    public class CutModuleFacade : BaseAppFacade
    {
        // --- 核心任务服务 ---
        public IWorkOrderTaskService Task
        {
            get;
        }
        public IWorkOrderInProgressViewService View
        {
            get;
        }
        public IWorkOrderTaskConfirmService Confirm
        {
            get;
        }

        // --- 模块特有配套服务 ---
        public ICableCutParamService CutParam
        {
            get;
        }           // 断线参数
        public IRawLinesideStockService RawStock
        {
            get;
        }        // 线边库存(条码扫描)
        public IWorkOrderTaskMaterialAddService MaterialAdd
        {
            get;
        } // 上料记录
        public IWorkOrderTaskConsumService Consum
        {
            get;
        }       // 消耗记录
        public ISerialHelperService Serial
        {
            get;
        }              // 序列号生成
        public IWorkCenterService WorkCenter
        {
            get;
        }            // 工作中心
        public IWarehouseLocationService Location
        {
            get;
        }       // 库位服务
        public IRawLinesideStockLogService StockLog
        {
            get;
        }     // 库存日志

        public IMaterialTransferLogService SapTransferLog
        {
            get;
        }           // 断线参数

        public CutModuleFacade(
            // 1. 模块特有服务
            IWorkOrderTaskService task,
            IWorkOrderInProgressViewService view,
            IWorkOrderTaskConfirmService confirm,
            ICableCutParamService cutParam,
            IRawLinesideStockService rawStock,
            IWorkOrderTaskMaterialAddService materialAdd,
            IWorkOrderTaskConsumService consum,
            ISerialHelperService serial,
            IWorkCenterService workCenter,
            IWarehouseLocationService location,
            IRawLinesideStockLogService stockLog,
            IMaterialTransferLogService sapTransferLog,

           // 2. 基础服务 (透传给基类 BaseAppFacade)
            IParameterGroupService paramsService,
            IMesApiClient mesApi,
            IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings,
            IFactoryService factoryService,
            IWorkCenterGroupService workCenterGroupService,
            IWorkCenterService workCenterService,
            IWorkStationService workStation,
            IWorkOrderService workOrderService,
            IWorkOrderProcessService workOrderProcessService,
            IWorkOrderBomItemService workOrderBomItemService,
            IUserService userService,
            IMenuService menuService,
            IPermissionService permissionService,
            IRoleService roleService,
            IActivityLogService activityLogService
            ) : base(
                paramsService,
                mesApi,
                apiSettings,
                factoryService,
                workCenterGroupService,
                workCenterService,
                workStation,
                workOrderService,
                workOrderProcessService,
                workOrderBomItemService,
                userService,
                menuService,
                permissionService,
                roleService,
                activityLogService)
        {
            // 赋值模块特有服务
            Task = task;
            View = view;
            Confirm = confirm;
            CutParam = cutParam;
            RawStock = rawStock;
            MaterialAdd = materialAdd;
            Consum = consum;
            Serial = serial;
            WorkCenter = workCenter;
            Location = location;
            StockLog = stockLog;
            SapTransferLog = sapTransferLog;
        }
    }
}
