using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly IMapper _mapper;

        public ActivityLogService(IActivityLogRepository activityLogRepository, IMapper mapper)
        {
            _activityLogRepository = activityLogRepository;
            _mapper = mapper;
        }


        public async Task<ActivityLogDto> CreateAsync(ActivityLogCreateDto createDto)
        {
            var entity = _mapper.Map<ActivityLog>(createDto);
            var result = await _activityLogRepository.AddAsync(entity);
            return _mapper.Map<ActivityLogDto>(result);
        }

        public async Task<List<int>> CreateBatchAsync(List<ActivityLogCreateDto> createDto)
        {
            var entities = _mapper.Map<List<ActivityLog>>(createDto);
            var result = await _activityLogRepository.AddBulkAsync(entities);
            return result;
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ActivityLogDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ActivityLogDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(ActivityLogUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
