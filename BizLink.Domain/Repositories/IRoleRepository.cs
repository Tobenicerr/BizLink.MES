using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{

    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<IEnumerable<Menu>> GetMenusAsync(int roleId);
        Task AssignMenusAsync(int roleId, IEnumerable<int> menuIds);
    }
}
