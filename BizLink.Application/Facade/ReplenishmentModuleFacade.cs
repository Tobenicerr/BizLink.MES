using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Entities;
using BizLink.MES.WinForms.Common;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Facade
{
    public class ReplenishmentModuleFacade : BaseAppFacade
    {
        public IMaterialViewService MaterialView
        {
            get;
        }
        public IAutoMaterialStockService AutoStock
        {
            get;
        }

        // 专门用于报表计算的 DTO，放在 Facade 或 DTOs 命名空间下
        public class ReplenishmentPlanView
        {
            public string MaterialCode
            {
                get; set;
            }
            public string MaterialDesc
            {
                get; set;
            }
            public decimal PlanQuantity
            {
                get; set;
            }
            public string BaseUnit
            {
                get; set;
            }
            public string ConsumeType
            {
                get; set;
            }
            public decimal UsageQuantity
            {
                get; set;
            }
            public decimal DiffQuantity
            {
                get; set;
            }
            public decimal ReplenisQuantity
            {
                get; set;
            }
        }

        public ReplenishmentModuleFacade(
            IMaterialViewService materialView,
            IAutoMaterialStockService autoStock,

            // 2. 基础服务 (透传给基类 BaseAppFacade)
            IParameterGroupService paramsService,
            IMesApiClient mesApi,
            IOptions<Dictionary<string, ServiceEndpointSettings>> apiSettings,
            IFactoryService factoryService,
            IWorkCenterGroupService workCenterGroupService,
            IWorkCenterService workCenterService,
            IWorkStationService workStationService,
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
                workCenterService,
                workStationService,
                workOrderService,
                workOrderProcessService,
                workOrderBomItemService,
                userService,
                menuService,
                permissionService,
                roleService,
                activityLogService)
        {
            MaterialView = materialView;
            AutoStock = autoStock;
        }

        /// <summary>
        /// 核心业务逻辑：计算补料计划
        /// </summary>
        public async Task<List<ReplenishmentPlanView>> GenerateReplenishmentReportAsync(string factoryCode ,DateTime startDate)
        {
            var sapBoms = new List<SapOrderBom>();
            var tempdate = startDate.Date.AddDays(1);
            var endDate = startDate.Date.AddDays(3);

            // 1. 循环获取未来 3 天的工单
            while (tempdate < endDate)
            {

                var requestUrl = $"{ApiSettings["MesApi"].Endpoints["GetWorkOrdersByDispatchDate"]}?factoryCode={factoryCode}&dispatchDate={tempdate}";


                var result = await MesApi.GetAsync<SapOrderDto>(requestUrl);
                if (result.IsSuccess && result.Data.sapOrderBoms.Any())
                {
                    sapBoms.AddRange(result.Data.sapOrderBoms);
                }
                tempdate = tempdate.AddDays(1);
            }

            if (!sapBoms.Any())
                return new List<ReplenishmentPlanView>();

            // 2. 获取基础数据
            var distinctMaterials = sapBoms.Select(x => x.MaterialCode).Distinct().ToList();

            var materialsTask = MaterialView.GetListByCodesAsync(factoryCode, distinctMaterials);
            var autoStocksTask = AutoStock.GetListByMaterialCodeAsync(distinctMaterials);

            await Task.WhenAll(materialsTask, autoStocksTask);

            var materials = materialsTask.Result;
            var antostocks = autoStocksTask.Result;

            // 3. 内存计算 (LINQ)
            var joinBoms = sapBoms.Join(materials, b => b.MaterialCode, m => m.MaterialCode, (b, m) => new
            {
                b.MaterialCode,
                b.MaterialDesc,
                PlanQuantity = (decimal)b.RequireQuantity,
                b.BaseUnit,
                ConsumeType = m.LabelName
            });

            var groupStock = antostocks.GroupBy(x => x.MaterialCode).Select(g => new
            {
                MaterialCode = g.Key,
                UsageQuantity = g.Sum(x => x.Quantity)
            }).ToList();

            var groupMat = joinBoms
                .Where(b => b.ConsumeType == "自动仓物料")
                .GroupBy(x => x.MaterialCode)
                .Select(g => new ReplenishmentPlanView
                {
                    MaterialCode = g.Key,
                    MaterialDesc = g.First().MaterialDesc,
                    PlanQuantity = g.Sum(x => x.PlanQuantity),
                    BaseUnit = g.First().BaseUnit,
                    ConsumeType = g.First().ConsumeType,
                }).ToList();

            var finalList = groupMat.GroupJoin(groupStock,
                m => m.MaterialCode,
                s => s.MaterialCode,
                (m, s) => new { Mat = m, Stock = s.FirstOrDefault() })
                .Select(x =>
                {
                    var m = x.Mat;
                    m.UsageQuantity = x.Stock?.UsageQuantity ?? 0;
                    m.DiffQuantity = m.PlanQuantity - m.UsageQuantity;
                    m.ReplenisQuantity = m.DiffQuantity > 0 ? m.DiffQuantity : 0;
                    return m;
                })
                .Where(x => x.ReplenisQuantity > 0)
                .ToList();

            return finalList;
        }
    }
}
