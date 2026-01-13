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
    public interface IWorkCenterGroupStepConfigService :IGenericService<WorkCenterGroupStepConfigDto, WorkCenterGroupStepConfigCreateDto, WorkCenterGroupStepConfigUpdateDto>
    {
    }
}
