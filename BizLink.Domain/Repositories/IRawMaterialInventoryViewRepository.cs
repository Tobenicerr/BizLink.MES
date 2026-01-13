using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IRawMaterialInventoryViewRepository : IGenericRepository<V_RawMaterialInventory>
    {
        Task<List<V_RawMaterialInventory>> GetListByMaterialCodeAsync(string factorycode, string materialCode);

        Task<List<V_RawMaterialInventory>> GetListByMaterialCodeAsync(string factorycode, List<string> materialCode);
    }
}
