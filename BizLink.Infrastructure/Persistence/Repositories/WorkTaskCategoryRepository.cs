using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class WorkTaskCategoryRepository : GenericRepository<WorkTaskCategory>, IWorkTaskCategoryRepository
    {
        public WorkTaskCategoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<WorkTaskCategory> GetByCategoryCodeAsync(string categoryCode)
        {
            return await _db.Queryable<WorkTaskCategory>().Where(c => c.CategoryCode == categoryCode).FirstAsync();
        }

        public async Task<List<WorkTaskCategory>> GetByCategoryCodeAsync(List<string> categoryCodes)
        {
            return await _db.Queryable<WorkTaskCategory>().Where(c => categoryCodes.Contains(c.CategoryCode)).ToListAsync();
        }

        public async Task<List<WorkTaskCategory>> GetListByStepTypeAsync(string stepType)
        {
            return await _db.Queryable<WorkTaskCategory>().Where(c => c.StepType == stepType).ToListAsync();
        }
    }
}
