using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IStationMaterialLoadingService : IGenericService<StationMaterialLoadingDto, StationMaterialLoadingCreateDto, StationMaterialLoadingUpdateDto>
    {

        Task<List<StationMaterialLoadingDto>> GetListByStationIdAsync(int stationId, string? materialCode = null, string? status = MaterialLoadingStatus.Active);

        Task<List<StationMaterialLoadingDto>> GetListByFactoryIdAsync(int factoryId, string? materialCode = null, string? status = MaterialLoadingStatus.Active);

    }
}
