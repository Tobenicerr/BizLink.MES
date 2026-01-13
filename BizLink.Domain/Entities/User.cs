using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Entities
{
    /// <summary>
    /// 用户实体 (对应 Sys_Users 表)
    /// </summary>
    [SugarTable("Sys_Users", IsDisabledUpdateAll = true)] // 明确指定表名
    public class User
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        public string EmployeeId { get; set; }

        [SugarColumn(IsNullable = true)] // 允许 DomainAccount 为空
        public string DomainAccount { get; set; }
        [SugarColumn(ColumnDataType = "nvarchar", Length = 50)]
        public string UserName { get; set; }

        [SugarColumn(IsNullable = true)] // 允许 DomainAccount 为空

        public string PasswordHash { get; set; }

        [SugarColumn(IsNullable = true)]
        public int FactoryId { get; set; }

        [SugarColumn(IsNullable = true)]
        public bool IsActive { get; set; } = true;

        [SugarColumn(IsNullable = true)]
        public bool IsDelete { get; set; } = false;

        [SugarColumn(IsNullable = true)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [SugarColumn(IsIgnore = true)] // 告诉SqlSugar这个字段不映射到数据库
        public string FactoryName { get; set; }
    }

    /// <summary>
    /// 角色实体 (对应 Sys_Roles 表)
    /// </summary>
    [SugarTable("Sys_Roles")]
    public class Role
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        [SugarColumn(IsNullable = true)]
        public string RoleName { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 50)]
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 菜单/权限实体 (对应 Sys_Menus 表)
    /// </summary>
    [SugarTable("Sys_Menus")]
    public class Menu
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        [SugarColumn(IsNullable = true)]
        public int ParentId { get; set; }
        [SugarColumn(ColumnDataType = "nvarchar", Length = 100)]
        public string MenuName { get; set; }

        [SugarColumn(IsNullable = true)]

        public string PermissionCode { get; set; }

        public int MenuType { get; set; }

        [SugarColumn(IsNullable = true)]

        public string Route { get; set; }

        [SugarColumn(IsNullable = true)]

        public string Icon { get; set; }
        [SugarColumn(IsNullable = true)]

        public int SortOrder { get; set; }

        public bool IsVisible { get; set; }
    }

    /// <summary>
    /// 工厂实体 (对应 Sys_Factories 表)
    /// </summary>
    [SugarTable("Sys_Factories")]
    public class Factory
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }

        /// <summary>
        /// 工厂代码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "nvarchar", Length = 100)]
        public string Address { get; set; }

        /// <summary>
        /// 是否激活 (用于控制业务上的启用/禁用)
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// 逻辑删除标记 (用于数据软删除)
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// 用户-角色关联实体 (对应 Sys_UserRoles 表)
    /// </summary>
    [SugarTable("Sys_UserRoles")]
    public class UserRole
    {
        [SugarColumn(IsPrimaryKey = true)] // 联合主键的一部分
        public int UserId { get; set; }

        [SugarColumn(IsPrimaryKey = true)] // 联合主键的一部分
        public int RoleId { get; set; }
    }

    /// <summary>
    /// 角色-菜单关联实体 (对应 Sys_RoleMenus 表)
    /// </summary>
    [SugarTable("Sys_RoleMenus")]
    public class RoleMenu
    {
        [SugarColumn(IsPrimaryKey = true)] // 联合主键的一部分
        public int RoleId { get; set; }

        [SugarColumn(IsPrimaryKey = true)] // 联合主键的一部分
        public int MenuId { get; set; }
    }
}
