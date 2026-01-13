using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Common
{
    /// <summary>
    /// 定义了通用 CRUD 操作的服务接口
    /// </summary>
    /// <typeparam name="TEntityDto">数据传输对象 (DTO)</typeparam>
    /// <typeparam name="TCreateDto">用于创建的 DTO</typeparam>
    /// <typeparam name="TUpdateDto">用于更新的 DTO</typeparam>
    public interface IGenericService<TEntityDto, TCreateDto, TUpdateDto>
    {
        Task<TEntityDto> GetByIdAsync(int id);
        Task<IEnumerable<TEntityDto>> GetAllAsync();
        Task<TEntityDto> CreateAsync(TCreateDto createDto);

        Task<List<int>> CreateBatchAsync(List<TCreateDto> createDto);
        Task<bool> UpdateAsync(TUpdateDto updateDto);
        Task<bool> DeleteAsync(int id);
    }
}
