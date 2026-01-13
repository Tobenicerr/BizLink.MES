using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IRawMaterialInventoryViewService
    {
        Task<List<RawMaterialInventoryViewDto>> GetListByMaterialCodeAsync(string factorycode, string materialCode);

        Task<List<RawMaterialInventoryViewDto>> GetListByMaterialCodeAsync(string factorycode, List<string> materialCode);
    }
}
