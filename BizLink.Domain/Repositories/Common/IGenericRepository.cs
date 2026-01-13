using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories.Common
{
    /// <summary>
    /// 定义了针对数据库实体的通用 CRUD 操作接口
    /// </summary>
    /// <typeparam name="TEntity">数据库实体类型</typeparam>
    public interface IGenericRepository<TEntity> where TEntity : class, new()
    {
        Task<TEntity> GetByIdAsync(int id);

        Task<List<TEntity>> GetByIdsAsync(List<int> ids);

        Task<List<TEntity>> GetAllAsync();

        Task<TEntity> AddAsync(TEntity entity);

        Task<List<int>> AddBulkAsync(List<TEntity> entities);

        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> UpdateBulkAsync(List<TEntity> entities);

        Task<bool> DeleteAsync(int id);

        Task<bool> DeleteBulkAsync(List<int> ids);
    }
}
