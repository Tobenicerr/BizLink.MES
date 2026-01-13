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
    /// 断线任务报表模块外观
    /// </summary>
    public class CableTaskModuleFacade : BaseAppFacade
    {
        // --- 模块特有服务 ---
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
        public IWorkOrderTaskConsumService Consum
        {
            get;
        }
        public IWorkOrderTaskMaterialAddService MaterialAdd
        {
            get;
        }
        public CableTaskModuleFacade(
            // 模块特有
            IWorkOrderTaskService task,
            IWorkOrderInProgressViewService view,
            IWorkOrderTaskConfirmService confirm,
            IWorkOrderTaskConsumService consum,
            IWorkOrderTaskMaterialAddService materialAdd,
            IWorkCenterService workCenter,

            // 2. 基础服务 (透传给基类 BaseAppFacade)
            IParameterGroupService paramsService,
            IMesApiClient mesApi,
            IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings,
            IFactoryService factoryService,
            IWorkCenterGroupService workCenterGroup,
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
                workCenterGroup,
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
            Consum = consum;
            MaterialAdd = materialAdd;
        }
    }
}
