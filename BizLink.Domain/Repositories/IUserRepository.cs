using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    // ... IUserRepository 接口定义 ...
    public interface IUserRepository : IGenericRepository<User>
    {

        Task<User> GetByEmployeeIdAsync(string EmployeeId);
        Task<User> GetByDomainAccountAsync(string domainAccount);
        Task<IEnumerable<Role>> GetRolesAsync(int userId);
        Task AssignRolesAsync(int userId, IEnumerable<int> roleIds);
        Task<(IEnumerable<User> Users, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, string keyword, bool? isActive);

        Task<List<User>> GetByEmployeeIdAsync(List<string> EmployeeIds);
    }



}
