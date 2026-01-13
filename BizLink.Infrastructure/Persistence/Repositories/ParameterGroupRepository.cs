using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class ParameterGroupRepository : GenericRepository<ParameterGroup>, IParameterGroupRepository
    {
        public ParameterGroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<ParameterGroup> GetGroupWithItemsAsync(string groupKey)
        {
            // 使用导航查询 IncludeMany 一次性加载分组和其下的所有明细项
            return await _db.Queryable<ParameterGroup>()
                            .Includes(g => g.Items.Where(i => i.IsEnabled).OrderBy(i => i.SortOrder).ToList())
                            .Where(g => g.IsEnabled && g.Key == groupKey)
                            .OrderBy(g => g.SortOrder)
                            .FirstAsync();
        }

        public async Task<List<ParameterGroup>> GetParameterTreeAsync()
        {
            // 使用 ToTree 方法构建树形结构，非常方便
            var allGroups = _db.Queryable<ParameterGroup>()
                                    .Includes(g => g.Items.Where(i => i.IsEnabled).OrderBy(i => i.SortOrder).ToList())
                                    .Where(g => g.IsEnabled)
                                     .OrderBy(g => g.SortOrder)
                                     .ToList();

            // 如果您的数据结构支持父子关系，可以使用 ToTree
            // 这里假设您需要一个扁平列表，每个列表项包含其子项
            return allGroups;
        }
    }
}
