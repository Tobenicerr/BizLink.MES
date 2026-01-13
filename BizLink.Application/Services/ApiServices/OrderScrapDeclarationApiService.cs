using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services.ApiServices
{
    public  class OrderScrapDeclarationApiService : IOrderScrapDeclarationApiService
    {
        private readonly ISapOrderScrapDeclarationService _sapOrderScrapDeclarationService;
        public OrderScrapDeclarationApiService(ISapOrderScrapDeclarationService sapOrderScrapDeclarationService)
        {
            _sapOrderScrapDeclarationService = sapOrderScrapDeclarationService;
        }

        public async Task<List<SapOrderScrapDeclarationDto>> GetOrderScrapDeclarationsAsync(SapOrderScrapDeclarationRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.FactoryCode))
                throw new Exception("工厂代码不能为空！");

            if (request.WorkOrderNos == null || request.WorkOrderNos.Count() == 0)
            {
                if(request.WorkCenterCodes == null || request.WorkCenterCodes.Count() == 0 || request.StartDate == null || request.EndDate == null)
                    throw new Exception("订单号清单为空时，必须传入工作中心列表及开始结束时间！");
            }
            return await _sapOrderScrapDeclarationService.GetListByWorkCenterAsync(request.FactoryCode, request.WorkCenterCodes, request.StartDate, request.EndDate, request.WorkOrderNos);
        }
    }
}
