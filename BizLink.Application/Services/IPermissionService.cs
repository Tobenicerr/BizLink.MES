using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public  interface IPermissionService
    {
        /// <summary>
        /// 获取指定用户的菜单树，用于UI展示
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>构建好的菜单树 DTO 列表</returns>
        Task<List<MenuDto>> GetMenuTreeForUserAsync(int userId);

        /// <summary>
        /// 检查用户是否拥有特定的操作权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionCode">权限代码 (对应 Menu 表的 PermissionCode 字段)</param>
        /// <returns>如果拥有权限则为 true，否则为 false</returns>
        Task<bool> HasPermissionAsync(int userId, string permissionCode);
    }
}
