using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IAssemblyOrderProgressService : IGenericService<AssemblyOrderProgressDto, AssemblyOrderProgressCreateDto, AssemblyOrderProgressUpdateDto>
    {
        Task<PagedResultDto<AssemblyOrderProgressDto>> GetPageListAsync(int pageIndex, int pageSize, string factoryCode, List<string>? orderNumber, List<string>? workCenter, DateTime? dispatchdateStart, DateTime? dispatchdateEnd, DateTime? confirmDateStart, DateTime? confirmDateEnd);
    }
}
