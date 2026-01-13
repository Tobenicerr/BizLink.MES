using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public interface IMaterialTransferLogService : IGenericService<MaterialTransferLogDto, MaterialTransferLogCreateDto, MaterialTransferLogUpdateDto>
    {

        Task<MaterialTransferLogDto> GetByTransferNoAsync(string transferno, string materialcode,string batchcode);

        Task<List<MaterialTransferLogDto>> GetByTransferNoAsync(string transferno);

        Task<List<MaterialTransferLogDto>> GetByTransferNoAsync(List<string> transfernos);

        Task<List<MaterialTransferLogDto>> GetListByStatusAsync();

        Task<List<MaterialTransferLogDto>> GetListByIdsAsync(List<int> transferIds);

        Task<List<MaterialTransferLogDto>> GetListByStockIdAsync(int stockid);

        Task<List<MaterialTransferLogDto>> GetListByStockLogIdAsync(List<int> stockid);

        Task<bool> UpdateListAsync(List<MaterialTransferLogUpdateDto> updateDto);

        //Task<List<int>> CreateBatchAsync(List<MaterialTransferLogUpdateDto> createDtos);

        Task<PagedResultDto<MaterialTransferLogDto>> GetPagedListAsync(int pageIndex, int pageSize, string? keyword, string? status, DateTime? createdStart, DateTime? createdEnd);

    }
}
