using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkCenterService
    {
        Task<WorkCenterDto> GetByIdAsync(int id);

        Task<WorkCenterDto> GetByCodeAsync(string code);

        Task<List<WorkCenterDto>> GetBygroupIdAsync(int groupid);

        Task<List<WorkCenterDto>> GetListByGroupCodeAsync(string groupcode);

        Task<List<WorkCenterDto>> GetListByGroupCodeAsync(List<string> groupcode);

        Task<List<WorkCenterDto>> GetAllAsync(int factoryid);
        Task CreateAsync(WorkCenterCreateDto workCenter);
        Task UpdateAsync(WorkCenterUpdateDto workCenter);

        Task DeleteAsync(int id);
    }
}
