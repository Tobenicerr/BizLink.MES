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
    public class UserDto : IMapFrom<User>
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string DomainAccount { get; set; }
        public string UserName { get; set; }
        public string FactoryName { get; set; } // 关联查询出的工厂名
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    // 用于分页查询的通用结果DTO
    //public class PagedResultDto<T>
    //{
    //    public IEnumerable<T> Items { get; set; }
    //    public int TotalCount { get; set; }
    //}


    // 用于创建用户的DTO
    public class UserCreateDto : IMapFrom<User>
    {
        public string EmployeeId { get; set; }
        public string DomainAccount { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; } // 接收明文密码
        public int FactoryId { get; set; }
        public bool IsActive { get; set; } = true;

        public bool IsDelete { get; set; } = false;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.PasswordHash)))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));
            // 其他映射...
        }
    }

    // 用于更新用户的DTO
    public class UserUpdateDto : IMapFrom<User>
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public string DomainAccount { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; } // 接收明文密码
        public int FactoryId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UserUpdateDto, User>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
