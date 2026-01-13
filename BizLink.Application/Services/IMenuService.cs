using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    /// <summary>
    /// 定义了用于处理菜单相关业务逻辑的服务接口。
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// 异步获取所有菜单项，并将其组织成树形结构。
        /// </summary>
        /// <returns>根菜单项的集合。</returns>
        Task<IEnumerable<MenuDto>> GetMenusAsTreeAsync();
    }
}
