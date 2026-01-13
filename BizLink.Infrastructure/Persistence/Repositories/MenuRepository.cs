using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    //================================================================
    // 菜单仓储实现
    //================================================================
    public class MenuRepository : GenericRepository<Menu>, IMenuRepository
    {
        public MenuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Task<IEnumerable<Menu>> GetMenusAsTreeAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Menu>> GetMenusByRoleIdsAsync(IEnumerable<int> roleIds)
        {
            return await _db.Queryable<Menu, RoleMenu>((m, rm) => m.Id == rm.MenuId)
                            .Where((m, rm) => roleIds.Contains(rm.RoleId))
                            .Select((m, rm) => m)
                            .Distinct() // 去重，因为多个角色可能有相同菜单
                            .ToListAsync();
        }

        public async Task<List<Menu>> GetMenusByUserIdAsync(int userId)
        {
            // 使用 SqlSugar 的 Navigate (导航) 查询属性，需要实体类中配置 [Navigate] 特性
            // 或者使用 Join 查询
            var menuList = await _db.Queryable<UserRole>()
                .InnerJoin<RoleMenu>((ur, rm) => ur.RoleId == rm.RoleId)
                .InnerJoin<Menu>((ur, rm, m) => rm.MenuId == m.Id)
                .Where((ur, rm, m) => ur.UserId == userId && m.IsVisible)
                .Select((ur, rm, m) => m).MergeTable()
                .OrderBy(x => x.SortOrder)
                .Distinct() // 去除因为多角色导致的重复菜单
                .ToListAsync();

            return menuList;
        }
    }
}
