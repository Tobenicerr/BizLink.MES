using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkTaskCategoryService : IGenericService<WorkTaskCategoryDto, WorkTaskCategoryCreateDto, WorkTaskCategoryUpdateDto>
    {
        Task<WorkTaskCategoryDto> GetByCategoryCodeAsync(string categoryCode);

        Task<List<WorkTaskCategoryDto>> GetListByStepTypeAsync(string stepType);
    }
}
