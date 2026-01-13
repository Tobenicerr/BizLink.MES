using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkCenterRepository : IGenericRepository<WorkCenter>
    {

        Task<WorkCenter> GetByCodeAsync(string code);

        Task<List<WorkCenter>> GetBygroupIdAsync(int groupid);

        Task<List<WorkCenter>> GetAllAsync(int factoryid);

        Task<List<WorkCenter>> GetListByGroupCodeAsync(string groupcode);

        Task<List<WorkCenter>> GetListByGroupCodeAsync(List<string> groupcode);


    }
}
