using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
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
    /// 基础外观服务
    /// 聚合了所有模块都可能用到的通用底层服务
    /// </summary>
    public class BaseAppFacade
    {
        // --- 通用基础设施服务 ---
        public IParameterGroupService Params
        {
            get;
        }
        public IMesApiClient MesApi
        {
            get;
        }

        public Dictionary<string, ServiceEndpointSettings> ApiSettings
        {
            get;
        }

        // --- 通用业务服务 ---
        public IFactoryService FactoryService
        {
            get;
        }

        public IWorkCenterGroupService WorkCenterGroup
        {
            get;
        }

        public IWorkCenterService WorkCenter
        {
            get;
        }
        public IWorkStationService WorkStation
        {
            get;
        }
        public IWorkOrderService WorkOrderService
        {
            get;
        }
        public IWorkOrderProcessService WorkOrderProcessService
        {
            get;
        }
        public IWorkOrderBomItemService WorkOrderBomItemService
        {
            get;
        }

        // --- 【新增】核心安全/权限服务 ---
        public IUserService UserService
        {
            get;
        }
        public IMenuService MenuService
        {
            get;
        }
        public IPermissionService PermissionService
        {
            get;
        }
        public IRoleService RoleService
        {
            get;
        } // 【新增】角色服务

        public IActivityLogService ActivityLogService 
        {
            get;
        }

        public BaseAppFacade(
            IParameterGroupService paramsService,
            IMesApiClient mesApi,
            IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings,
            IFactoryService factoryService,
            IWorkCenterGroupService workCenterGroup,
            IWorkCenterService workCenter,
            IWorkStationService workStation,
            IWorkOrderService workOrderService,
            IWorkOrderProcessService workOrderProcessService,
            IWorkOrderBomItemService workOrderBomItemService,
            // 新增注入
            IUserService userService,
            IMenuService menuService,
            IPermissionService permissionService,
            IRoleService roleService,
            IActivityLogService activityLogService)
        {
            Params = paramsService;
            MesApi = mesApi;
            ApiSettings = apiSettings.Value;
            FactoryService = factoryService;
            WorkCenterGroup = workCenterGroup;
            WorkCenter = workCenter;
            WorkStation = workStation;
            WorkOrderService = workOrderService;
            WorkOrderProcessService = workOrderProcessService;
            WorkOrderBomItemService = workOrderBomItemService;

            // 赋值
            UserService = userService;
            MenuService = menuService;
            PermissionService = permissionService;
            RoleService = roleService;
            ActivityLogService = activityLogService;
        }
    }
}
