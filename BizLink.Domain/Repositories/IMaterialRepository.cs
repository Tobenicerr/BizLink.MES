using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IMaterialRepository : IGenericRepository<Material>
    {
        Task<int> CreateBatchAsync(List<Material> materials);

        Task<int> UpdateBatchAsync(List<Material> materials);

        Task<List<Material>> GetListByMaterialCodesAsync(List<string> materialCodes);
    }
}
