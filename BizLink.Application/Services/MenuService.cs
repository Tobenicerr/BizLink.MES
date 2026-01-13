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
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MenuDto>> GetMenusAsTreeAsync()
        {
            var allMenus = await _menuRepository.GetAllAsync();

            // 1. 过滤掉不可见的菜单
            // 2. 映射到 DTO
            // 3. 按 SortOrder 排序
            var allMenuDtos = _mapper.Map<List<MenuDto>>(allMenus.Where(m => m.IsVisible))
                                     .OrderBy(m => m.SortOrder)
                                     .ToList();

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
    }
}
