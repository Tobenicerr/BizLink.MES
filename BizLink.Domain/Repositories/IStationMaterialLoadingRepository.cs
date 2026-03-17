using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Repositories
{
    public interface IStationMaterialLoadingRepository : IGenericRepository<StationMaterialLoading>
    {
        Task<List<StationMaterialLoading>> GetListByStationIdAsync(int stationId, string? materialCode = null, string? status = MaterialLoadingStatus.Active);

        Task<List<StationMaterialLoading>> GetListByFactoryIdAsync(int factoryId, string? materialCode = null, string? status = MaterialLoadingStatus.Active);

        Task<int> UpdateWithOptLockAsync (StationMaterialLoading update);

        Task<int> UpdateWithOptLockAsync(List<StationMaterialLoading> updates);
    }
}
