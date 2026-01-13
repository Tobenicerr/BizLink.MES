using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    //================================================================
    // 工厂仓储实现
    //================================================================
    public class FactoryRepository : GenericRepository<Factory>, IFactoryRepository
    {
        public FactoryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<(IEnumerable<Factory> Factorys, int TotalCount)> GetPagedListAsync(int pageIndex, int pageSize, string keyword, bool? isActive)
        {

            var query = _db.Queryable<Factory>()
                .WhereIF(!string.IsNullOrWhiteSpace(keyword), u => u.FactoryCode.Contains(keyword))
                .WhereIF(isActive.HasValue, u => u.IsActive == isActive.Value)
                .Where(u => u.IsDelete == false);
            var totalCount = await query.CountAsync();
            var factorys = await query.ToPageListAsync(pageIndex, pageSize);

            return (factorys, totalCount);

        }

        //public async Task AddAsync(Factory factory)
        //{
        //    await _db.Insertable(factory).ExecuteCommandAsync();
        //}

        //public async Task UpdateAsync(Factory factory)
        //{
        //    await _db.Updateable(factory).ExecuteCommandAsync();
        //}

        //public async Task DeleteAsync(int id)
        //{
        //    // 将 IsActive 字段更新为 false，而不是物理删除记录
        //    await _db.Updateable<Factory>()
        //             .SetColumns(it => new Factory { IsDelete = true }) // 仅更新 IsActive 字段
        //             .Where(it => it.Id == id)
        //             .ExecuteCommandAsync();
        //}
    }
}
