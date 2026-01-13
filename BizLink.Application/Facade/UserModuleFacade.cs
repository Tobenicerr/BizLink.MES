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
    // 自动会被 Program.cs 中的 AddProjectServices 扫描注册
    public class UserModuleFacade : BaseAppFacade
    {
        // 目前 User 模块所需的所有核心服务（UserService, FactoryService 等）都已由基类 BaseAppFacade 提供。
        // 因此这里不需要定义额外的属性。
        // 如果未来有 User 模块独有的服务（例如 IUserImportService），可以在这里添加。

        public UserModuleFacade(
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
            // 构造函数体为空，因为所有赋值都在基类完成了
        }
    }
}
