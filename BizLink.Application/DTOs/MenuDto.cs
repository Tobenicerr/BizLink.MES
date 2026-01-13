using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class MenuDto : IMapFrom<Menu>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; } // 使用可空 int 来表示根节点
        public string FormName { get; set; }
        public int SortOrder { get; set; }
        public string Icon { get; set; }
        public bool IsVisible { get; set; }

        public List<MenuDto> Children { get; set; } = new List<MenuDto>();

        // 实现 IMapFrom 接口的 Mapping 方法，用于自定义映射规则
        public void Mapping(AutoMapper.Profile profile)
        {
            profile.CreateMap<Menu, MenuDto>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.MenuName))
                .ForMember(d => d.FormName, opt => opt.MapFrom(s => s.Route))
                // 如果 ParentId 为 0，则在 DTO 中映射为 null，便于处理根节点
                .ForMember(d => d.ParentId, opt => opt.MapFrom(s => s.ParentId == 0 ? (int?)null : s.ParentId));
        }
    }
}
