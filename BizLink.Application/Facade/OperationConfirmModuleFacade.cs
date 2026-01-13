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
    public class OperationConfirmModuleFacade : BaseAppFacade
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
        public IWorkOrderOperationConfirmService OperationConfirm
        {
            get;
        }
        public IWorkOrderOperationConsumpService OperationConsump
        {
            get;
        } // 【新增】
        public IProductLinesideStockService ProductStock
        {
            get;
        }      // 【新增】
        public IWorkOrderTaskMaterialAddService MaterialAdd
        {
            get;
        }
        public ISerialHelperService Serial
        {
            get;
        }
        public ISapRfcService SapRfc
        {
            get;
        }
        public IMaterialViewService MaterialView
        {
            get;
        }

        public OperationConfirmModuleFacade(
            // 1. 模块特有服务
            IWorkOrderTaskService task,
            IWorkOrderInProgressViewService view,
            IWorkOrderTaskConfirmService confirm,
            IWorkOrderOperationConfirmService operationConfirm,
            IWorkOrderOperationConsumpService operationConsump, // 【新增注入】
            IProductLinesideStockService productStock,          // 【新增注入】
            IWorkOrderTaskMaterialAddService materialAdd,
            ISerialHelperService serial,
            ISapRfcService sapRfc,
            IMaterialViewService materialView,


            // 2. 基础服务 (透传给基类 BaseAppFacade)
            IParameterGroupService paramsService,
            IMesApiClient mesApi,
            IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings,
            IFactoryService factoryService,
            IWorkCenterService workCenter,
            IWorkCenterGroupService workCenterGroupService,
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
                apiSettings, // 这里如果不需要 ApiSettings 可以传 null，或者补充注入
                factoryService,
                workCenterGroupService,
                workCenter,
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
            Task = task;
            View = view;
            Confirm = confirm;
            OperationConfirm = operationConfirm;
            OperationConsump = operationConsump; // 【赋值】
            ProductStock = productStock;         // 【赋值】
            MaterialAdd = materialAdd;
            Serial = serial;
            SapRfc = sapRfc;
            MaterialView = materialView;
        }
    }
}
