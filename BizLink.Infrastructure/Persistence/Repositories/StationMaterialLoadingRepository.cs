using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Entities.Views;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class StationMaterialLoadingRepository : GenericRepository<StationMaterialLoading>, IStationMaterialLoadingRepository
    {
        public StationMaterialLoadingRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<List<StationMaterialLoading>> GetListByFactoryIdAsync(int factoryId, string? materialCode = null, string? status = "1")
        {
            return await _db.Queryable<StationMaterialLoading>().Where(l => l.FactoryId == factoryId)
                .WhereIF(!string.IsNullOrEmpty(status), l => l.Status == status)
                .WhereIF(!string.IsNullOrEmpty(materialCode), l => l.MaterialCode == materialCode)
                .ToListAsync();
        }

        public async Task<List<StationMaterialLoading>> GetListByStationIdAsync(int stationId,string?materialCode = null, string? status = "1")
        {
            return await _db.Queryable<StationMaterialLoading>().Where(l => l.WorkStationId == stationId)
                .WhereIF(!string.IsNullOrEmpty(status), l => l.Status == status)
                .WhereIF(!string.IsNullOrEmpty(materialCode), l => l.MaterialCode == materialCode)
                .ToListAsync();
        }

        public async Task<int> UpdateWithOptLockAsync(StationMaterialLoading update)
        {
            return await _db.Updateable(update).ExecuteCommandWithOptLockAsync(true);
        }

        public async Task<int> UpdateWithOptLockAsync(List<StationMaterialLoading> updates)
        {
            return await _db.Updateable(updates).ExecuteCommandWithOptLockAsync(true);
        }
    }
}
