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
    public class InventoryModuleFacade : BaseAppFacade
    {
        // --- 模块特有服务 ---
        public IRawLinesideStockService RawStock
        {
            get;
        }
        public IRawLinesideStockLogService StockLog
        {
            get;
        }
        public IMaterialTransferLogService SapTransferLog
        {
            get;
        }

        public ISerialHelperService SerialHelperService
        {
            get;
        }

        public IWarehouseLocationService Location
        {
            get;
        }
        public IMaterialViewService MaterialView
        {
            get;
        }
        public IJyApiClient JyApi
        {
            get;
        }

        // 任务相关 (部分库存操作可能需要回写任务状态)
        public IWorkOrderTaskService Task
        {
            get;
        }
        public IWorkOrderTaskMaterialAddService MaterialAdd
        {
            get;
        }

        // 注意：ApiSettings 属性已在 BaseAppFacade 中定义，此处直接继承使用

        public InventoryModuleFacade(
            // 1. 模块特有服务
            IRawLinesideStockService rawStock,
            IRawLinesideStockLogService stockLog,
            IMaterialTransferLogService sapTransferLog,
            IWarehouseLocationService location,
            IMaterialViewService materialView,
            IJyApiClient jyApi,
            IWorkOrderTaskService task,
            IWorkOrderTaskMaterialAddService materialAdd,
            ISerialHelperService serialHelperService,

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
            RawStock = rawStock;
            StockLog = stockLog;
            Location = location;
            MaterialView = materialView;
            JyApi = jyApi;
            Task = task;
            MaterialAdd = materialAdd;
            SapTransferLog = sapTransferLog;
            SerialHelperService = serialHelperService;
        }
    }
}
