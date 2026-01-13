using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{

    public interface IMenuRepository : IGenericRepository<Menu>
    {
        Task<IEnumerable<Menu>> GetMenusByRoleIdsAsync(IEnumerable<int> roleIds);

        /// <summary>
        /// 根据用户ID获取其所有被授权的菜单/权限项
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>菜单/权限项列表</returns>
        Task<List<Menu>> GetMenusByUserIdAsync(int userId);
    }
}
