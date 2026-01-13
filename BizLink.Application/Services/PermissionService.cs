using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public PermissionService(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuDto>> GetMenuTreeForUserAsync(int userId)
        {
            var allMenus = await _menuRepository.GetMenusByUserIdAsync(userId);
            var allMenuDtos =  _mapper.Map<List<MenuDto>>(allMenus);

            // 将扁平列表构建成树状结构
            var menuDict = allMenuDtos.ToDictionary(m => m.Id);
            var rootMenus = new List<MenuDto>();

            foreach (var menu in allMenuDtos)
            {
                // ParentId 为 null 的是根菜单
                if (menu.ParentId == null)
                {
                    rootMenus.Add(menu);
                }
                else
                {
                    // 找到父菜单并添加到其 Children 列表中
                    if (menuDict.TryGetValue(menu.ParentId.Value, out var parentMenu))
                    {
                        parentMenu.Children.Add(menu);
                    }
                }
            }
            return rootMenus;
        }

        public async Task<bool> HasPermissionAsync(int userId, string permissionCode)
        {
            if (string.IsNullOrWhiteSpace(permissionCode))
                return true; // 如果未指定权限码，则默认有权限

            var allMenus = await _menuRepository.GetMenusByUserIdAsync(userId);

            // 检查用户的权限列表中是否包含指定的权限码
            return allMenus.Any(m => m.PermissionCode == permissionCode);
        }
    }
}
