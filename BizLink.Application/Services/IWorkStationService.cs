using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IWorkStationService
    {

        Task<WorkStationDto> GetByIdAsync(int id);

        Task<List<WorkStationDto>> GetByIdAsync(List<int> id);

        Task<WorkStationDto> GetByCodeAsync(string code);

        Task<List<WorkStationDto>> GetByWorkcenterIdAsync(int workcenterId);

        Task<List<WorkStationDto>> GetByWorkcenterGroupCodeAsync(string groupcode);

        Task<List<WorkStationDto>> GetAllAsync(int factoryid);
        Task CreateAsync(WorkStationCreateDto WorkStation);
        Task UpdateAsync(WorkStationUpdateDto WorkStation);

        Task DeleteAsync(int id);
    }
}
