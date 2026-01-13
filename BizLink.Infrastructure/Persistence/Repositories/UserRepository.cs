using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    //================================================================
    // 用户仓储实现
    //================================================================
    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<User> GetByEmployeeIdAsync(string EmployeeId)
        {
            return await _db.Queryable<User>().LeftJoin<Factory>((u,f) => u.FactoryId==f.Id)
            .Where(u => u.EmployeeId == EmployeeId)
            .Where(u => u.IsDelete == false)
            .Select((u,f) => new User 
            { 
                Id = u.Id,
                UserName = u.UserName,
                DomainAccount = u.DomainAccount,
                EmployeeId = u.EmployeeId,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                PasswordHash = u.PasswordHash,
                FactoryId = f.Id, // 动态添加FactoryName属性
                FactoryName = f.FactoryName // 动态添加FactoryName属性

            }).FirstAsync();

        }
        public async Task<User> GetByDomainAccountAsync(string domainAccount)
        {
            var res = await _db.Queryable<User>().LeftJoin<Factory>((u, f) => u.FactoryId == f.Id)
            .Where(u => u.DomainAccount == domainAccount)
            .Where(u => u.IsDelete == false).ToListAsync();
            return await _db.Queryable<User>().LeftJoin<Factory>((u, f) => u.FactoryId == f.Id)
            .Where(u => u.DomainAccount == domainAccount)
            .Where(u => u.IsDelete == false)
            .Select((u, f) => new User
            {
                Id = u.Id,
                UserName = u.UserName,
                DomainAccount = u.DomainAccount,
                EmployeeId = u.EmployeeId,
                IsActive = u.IsActive,
                CreatedAt = u.CreatedAt,
                PasswordHash = u.PasswordHash,
                FactoryName = f.FactoryName // 动态添加FactoryName属性

            }).FirstAsync();
        }

        public async Task<IEnumerable<Role>> GetRolesAsync(int userId)
        {
            return await _db.Queryable<Role, UserRole>((r, ur) => r.Id == ur.RoleId)
                            .Where((r, ur) => ur.UserId == userId)
                            .Select((r, ur) => r)
                            .ToListAsync();
        }

        public async Task AssignRolesAsync(int userId, IEnumerable<int> roleIds)
        {
            await _db.Ado.UseTranAsync(async () =>
            {
                // 1. 删除用户旧的角色关系
                await _db.Deleteable<UserRole>().Where(ur => ur.UserId == userId).ExecuteCommandAsync();

                // 2. 插入用户新的角色关系
                if (roleIds != null && roleIds.Any())
                {
                    var userRoles = roleIds.Select(roleId => new UserRole { UserId = userId, RoleId = roleId });
                    await _db.Insertable(userRoles.ToList()).ExecuteCommandAsync();
                }
            });
        }

        public async Task<(IEnumerable<User> Users, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, string keyword, bool? isActive)
        {
            var query = _db.Queryable<User>().LeftJoin<Factory>((u, f) => u.FactoryId == f.Id)
                .Where(u => u.IsDelete == false)
                .WhereIF(!string.IsNullOrWhiteSpace(keyword), u => u.UserName.Contains(keyword) || u.DomainAccount.Contains(keyword))
                .WhereIF(isActive.HasValue, u => u.IsActive == isActive.Value)
                .Select((u, f) => new User { FactoryName = f.FactoryName, Id = u.Id, EmployeeId = u.EmployeeId, DomainAccount = u.DomainAccount, UserName = u.UserName, IsActive = u.IsActive, CreatedAt = u.CreatedAt }); // 动态添加FactoryName属性

            var totalCount = await query.CountAsync();
            var users = await query.ToPageListAsync(pageIndex, pageSize);

            return (users, totalCount);
        }

        public async Task<List<User>> GetByEmployeeIdAsync(List<string> EmployeeIds)
        {
            return await _db.Queryable<User>().Where(u => EmployeeIds.Contains(u.EmployeeId) && u.IsDelete == false).ToListAsync();

        }
    }
}
