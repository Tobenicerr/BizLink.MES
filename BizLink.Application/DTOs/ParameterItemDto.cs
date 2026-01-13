using AutoMapper;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class ParameterItemDto
    {
        public int Id
        {
            get; set;
        }
        public int GroupId
        {
            get; set;
        }
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string? Value
        {
            get; set;
        }
        public string? Type
        {
            get; set;
        }
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
    }

    public class ParameterItemCreateDto
    {
        [Required(ErrorMessage = "必须指定所属分组")]
        public int GroupId
        {
            get; set;
        }

        [Required(ErrorMessage = "参数名称不能为空")]
        [StringLength(100, ErrorMessage = "参数名称长度不能超过100个字符")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "参数键不能为空")]
        [StringLength(100, ErrorMessage = "参数键长度不能超过100个字符")]
        public string Key { get; set; } = string.Empty;

        public string? Value
        {
            get; set;
        }

        [StringLength(50, ErrorMessage = "类型长度不能超过50个字符")]
        public string? Type
        {
            get; set;
        }

        public bool IsEnabled { get; set; } = true;

        public int SortOrder { get; set; } = 0;

        [StringLength(500, ErrorMessage = "备注长度不能超过500个字符")]
        public string? Remark
        {
            get; set;
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<ParameterItem, ParameterItemDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    public class ParameterItemUpdateDto
    {
        [Required]
        public int Id
        {
            get; set;
        }
    }
}
