using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    /// <summary>
    /// 用于显示和传输的工厂数据对象
    /// </summary>
    public class FactoryDto : IMapFrom<Factory>
    {
        public int Id { get; set; }
        public string? FactoryCode { get; set; }
        public string? FactoryName { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }

        public bool IsDelete { get; set; }

        public DateTime CreatedAt {get; set;}

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Factory, FactoryDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }

    /// <summary>
    /// 用于创建新工厂的数据对象
    /// </summary>
    public class FactoryCreateDto : IMapFrom<Factory>
    {
        public string FactoryCode { get; set; }
        public string? FactoryName { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; } = true;
    }

    /// <summary>
    /// 用于更新工厂的数据对象
    /// </summary>
    public class FactoryUpdateDto : IMapFrom<Factory>
    {
        public int Id { get; set; }
        public string? FactoryCode { get; set; }
        public string? FactoryName { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
    }
}
