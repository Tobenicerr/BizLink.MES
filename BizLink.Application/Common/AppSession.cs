using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.WinForms.Common
{
    public static class AppSession
    {
        /// <summary>
        /// 获取或设置当前登录的用户信息。
        /// </summary>
        public static UserDto CurrentUser { get; set; }

        /// <summary>
        /// 获取或设置当前用户拥有的所有菜单（树形结构），用于生成左侧菜单。
        /// </summary>
        public static List<MenuDto> UserMenus { get; set; }

        /// <summary>
        /// 获取或设置当前用户拥有的所有权限代码集合。
        /// 使用 HashSet 是为了快速查找 (Contains 方法)。
        /// </summary>
        public static HashSet<string> UserPermissions { get; set; }

        /// <summary>
        /// 获取或设置当前选择的工厂ID。
        /// </summary>
        public static int CurrentFactoryId { get; set; }

        /// <summary>
        /// 判断用户是否已登录。
        /// </summary>
        public static bool IsLoggedIn => CurrentUser != null;

        /// <summary>
        /// 清除所有会话信息，用于注销登录。
        /// </summary>
        public static void Clear()
        {
            CurrentUser = null;
            UserMenus = null;
            UserPermissions = null;
            CurrentFactoryId = 0;
        }
    }

}
