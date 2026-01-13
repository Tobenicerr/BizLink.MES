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
    // 角色仓储实现
    //================================================================
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<IEnumerable<Menu>> GetMenusAsync(int roleId)
        {
            return await _db.Queryable<Menu, RoleMenu>((m, rm) => m.Id == rm.MenuId)
                            .Where((m, rm) => rm.RoleId == roleId)
                            .Select((m, rm) => m)
                            .ToListAsync();
        }

        public async Task AssignMenusAsync(int roleId, IEnumerable<int> menuIds)
        {
            await _db.Ado.UseTranAsync(async () =>
            {
                // 1. 删除角色旧的菜单关系
                await _db.Deleteable<RoleMenu>().Where(rm => rm.RoleId == roleId).ExecuteCommandAsync();

                // 2. 插入角色新的菜单关系
                if (menuIds != null && menuIds.Any())
                {
                    var roleMenus = menuIds.Select(menuId => new RoleMenu { RoleId = roleId, MenuId = menuId });
                    await _db.Insertable(roleMenus.ToList()).ExecuteCommandAsync();
                }
            });
        }
    }

}
