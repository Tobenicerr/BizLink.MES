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
    public class RawLinesideStockLogService : IRawLinesideStockLogService
    {

        private readonly IRawLinesideStockLogRepository _rawLinesideStockLogRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper


        public RawLinesideStockLogService(IRawLinesideStockLogRepository rawLinesideStockLogRepository, IMapper mapper)
        {
            _rawLinesideStockLogRepository = rawLinesideStockLogRepository;
            _mapper = mapper;
        }
        public async Task<RawLinesideStockLogDto> CreateAsync(RawLinesideStockLogCreateDto createDto)
        {
            var entity = _mapper.Map<RawLinesideStockLog>(createDto);
            var rtn =  await _rawLinesideStockLogRepository.AddAsync(entity);
            return _mapper.Map<RawLinesideStockLogDto>(rtn);
        }

        public Task<List<int>> CreateBatchAsync(List<RawLinesideStockLogCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RawLinesideStockLogDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<RawLinesideStockLogDto> GetByIdAsync(int id)
        {
            var entity = await _rawLinesideStockLogRepository.GetByIdAsync(id);
            return _mapper.Map<RawLinesideStockLogDto>(entity);
        }

        public async Task<List<RawLinesideStockLogDto>> GetListByKeywordAsync(List<string> keywords)
        {
            var entities = await  _rawLinesideStockLogRepository.GetListByKeywordAsync(keywords);
            return _mapper.Map<List<RawLinesideStockLogDto>>(entities);
        }

        public async Task<bool> UpdateAsync(RawLinesideStockLogUpdateDto updateDto)
        {
            var entity = await _rawLinesideStockLogRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _rawLinesideStockLogRepository.UpdateAsync(entity);
        }
    }
}
