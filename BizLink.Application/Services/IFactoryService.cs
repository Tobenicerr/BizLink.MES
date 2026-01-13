using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IFactoryService
    {
        Task<FactoryDto> GetByIdAsync(int userId);

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        Task<PagedResultDto<FactoryDto>> GetPagedListAsync(int pageIndex, int pageSize, string keyword, bool? isActive);

        Task<List<FactoryDto>> GetAllAsync();

        /// <summary>
        /// 创建新用户
        /// </summary>
        Task CreateAsync(FactoryCreateDto input);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        Task UpdateAsync(FactoryUpdateDto input);

        /// <summary>
        /// 删除用户 (逻辑删除)
        /// </summary>
        Task DeleteAsync(int id);
    }
}
