using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories.Common
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, new()
    {
        // 1. 存储 IUnitOfWork, 而不是 ISqlSugarClient
        protected readonly IUnitOfWork _unitOfWork;

        // 2. 关键：创建一个动态属性来获取 DbClient
        //    这确保每次访问 Db 属性时, 都会从 UoW 获取一个 *当前可用* 的 Client 实例
        protected ISqlSugarClient _db => _unitOfWork.DbClient;

        // 3. 构造函数现在只存储 UoW
        public GenericRepository(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _db.Queryable<TEntity>().InSingleAsync(id);
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _db.Queryable<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            // Insertable会返回插入的对象，如果主键是自增的，会自动填充
            return await _db.Insertable(entity).ExecuteReturnEntityAsync();
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            // 假设实体有主键
            return await _db.Updateable(entity).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandHasChangeAsync();
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            return await _db.Deleteable<TEntity>().In(id).ExecuteCommandHasChangeAsync();
        }

        public virtual async Task<List<int>> AddBulkAsync(List<TEntity> entities)
        {
            return await _db.Insertable(entities).ExecuteReturnPkListAsync<int>();
        }

        public virtual async Task<List<TEntity>> GetByIdsAsync(List<int> ids)
        {
            return await _db.Queryable<TEntity>().In(ids).ToListAsync();
        }

        public virtual async Task<bool> UpdateBulkAsync(List<TEntity> entities)
        {
            return await _db.Updateable(entities).IgnoreColumns(ignoreAllNullColumns: true).ExecuteCommandHasChangeAsync();
        }

        public virtual async Task<bool> DeleteBulkAsync(List<int> ids)
        {
            return await  _db.Deleteable<TEntity>().In(ids).ExecuteCommandHasChangeAsync();
        }
    }
}
