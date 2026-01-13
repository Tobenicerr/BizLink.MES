using AutoMapper;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Domain.Entities;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class RoleDto : IMapFrom<Role>
    {

        public int Id
        {
            get; set;
        }
        public string RoleName
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }

        public DateTime CreatedAt
        {
            get; set;
        }
    }

    public class RoleCreateDto : IMapFrom<Role>
    {
        public string RoleName
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }

        public DateTime CreatedAt
        {
            get; set;
        }
    }

    public class RoleUpdateDto : IMapFrom<Role>
    {
        public string RoleName
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
    }
}
