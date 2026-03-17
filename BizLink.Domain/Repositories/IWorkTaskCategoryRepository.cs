using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IWorkTaskCategoryRepository : IGenericRepository<WorkTaskCategory>
    {

        Task<WorkTaskCategory> GetByCategoryCodeAsync(string categoryCode);

        Task<List<WorkTaskCategory>> GetByCategoryCodeAsync(List<string> categoryCodes);

        Task<List<WorkTaskCategory>> GetListByStepTypeAsync(string stepType);
    }
}
