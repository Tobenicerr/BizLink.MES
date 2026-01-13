using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class ParameterGroupDto : IMapFrom<ParameterGroup>
    {
        public int Id
        {
            get; set;
        }
        public int ParentId
        {
            get; set;
        }
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public bool IsEnabled
        {
            get; set;
        }
        public int SortOrder
        {
            get; set;
        }
        public string? Remark
        {
            get; set;
        }

        /// <summary>
        /// 包含的参数明细项
        /// </summary>
        public List<ParameterItemDto> Items { get; set; } = new();

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ParameterGroup, ParameterGroupDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            profile.CreateMap<ParameterItem, ParameterItemDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class ParameterGroupCreateDto : IMapFrom<ParameterGroup>
    {
        public int ParentId
        {
            get; set;
        }

        [Required(ErrorMessage = "分组名称不能为空")]
        [StringLength(100, ErrorMessage = "分组名称长度不能超过100个字符")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "分组键不能为空")]
        [StringLength(100, ErrorMessage = "分组键长度不能超过100个字符")]
        public string Key { get; set; } = string.Empty;

        public bool IsEnabled { get; set; } = true;

        public int SortOrder { get; set; } = 0;

        [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
        public string? Remark
        {
            get; set;
        }
    }

    public class ParameterGroupUpdateDto : IMapFrom<ParameterGroup>
    {
        [Required]
        public int Id
        {
            get; set;
        }
    }

}
