using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IUserService : IGenericService<UserDto, UserCreateDto, UserUpdateDto>
    {

        Task<UserDto> GetByDomainAccountAsync(string domainAccount);

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        Task<PagedResultDto<UserDto>> GetPagedListAsync(int pageIndex, int pageSize, string keyword, bool? isActive);

        Task<UserDto> GetByEmployeeIdAsync(string employeeId);

        Task<List<UserDto>> GetByEmployeeIdAsync(List<string> employeeIds);

    }
}
