using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkCenterGroupService : IGenericService<WorkCenterGroupDto, WorkCenterGroupCreateDto, WorkCenterGroupUpdateDto>
    {
        Task<List<WorkCenterGroupDto>> GetListByGroupTypeAsync(int factoryid,string grouptype);
    }
}
