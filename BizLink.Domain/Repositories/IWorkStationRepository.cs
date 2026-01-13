using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkStationRepository : IGenericRepository<WorkStation>
    {


        Task<WorkStation> GetByCodeAsync(string code);

        Task<List<WorkStation>> GetByWorkcenterIdAsync(int workcenterid);

        Task<List<WorkStation>> GetAllAsync(int factoryid);

        Task<List<WorkStation>> GetByWorkcenterGroupCodeAsync(string groupcode);

        Task<List<WorkStation>> GetByIdAsync(List<int> id);

    }
}
