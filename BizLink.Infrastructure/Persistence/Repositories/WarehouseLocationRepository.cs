using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WarehouseLocationRepository : GenericRepository<WarehouseLocation>, IWarehouseLocationRepository
    {
        public WarehouseLocationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<WarehouseLocation>> GetAllBinAsync()
        {
            return await _db.Queryable<WarehouseLocation>().Where(x => (int)x.LocationType == 4).ToListAsync();
        }

        public async Task<List<WarehouseLocation>> GetLocationTreeAsync()
        {
            // 1. 为了获得最佳性能，首先一次性查询出所有需要构建树的数据
            //    可以根据需要添加 .Where() 条件进行过滤
            var allLocations = await _db.Queryable<WarehouseLocation>()
                                        .OrderBy(it => it.SortOrder) // 按排序码排序
                                        .ToListAsync();

            // 2. 调用 ToTree 方法将列表转换成树形结构
            var locationTree = await  _db.Queryable<WarehouseLocation>()
                                  .ToTreeAsync(
                                      it => it.Children,   
                                      it => it.ParentId,   
                                      0,                   
                                      it => allLocations.Select(x=>x.Id).ToList().Contains(it.Id)
                                  );

            return locationTree;
        }
    }
}
