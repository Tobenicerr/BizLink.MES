using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class FinishedGoodsReceiptService : IFinishedGoodsReceiptService
    {
        private readonly IFinishedGoodsReceiptRepository _finishedGoodsReceiptRepository;
        private readonly IMapper _mapper;
        public FinishedGoodsReceiptService(IFinishedGoodsReceiptRepository finishedGoodsReceiptRepository, IMapper mapper) 
        {
            _finishedGoodsReceiptRepository = finishedGoodsReceiptRepository;
            _mapper = mapper;
        }
        public async Task<FinishedGoodsReceiptDto> CreateAsync(FinishedGoodsReceiptCreateDto createDto)
        {
            var entity = _mapper.Map<FinishedGoodsReceipt>(createDto);
            var result = await _finishedGoodsReceiptRepository.AddAsync(entity);
            return _mapper.Map<FinishedGoodsReceiptDto>(result);
        }

        public Task<List<int>> CreateBatchAsync(List<FinishedGoodsReceiptCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<FinishedGoodsReceiptDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<FinishedGoodsReceiptDto> GetByIdAsync(int id)
        {
            var entity = await _finishedGoodsReceiptRepository.GetByIdAsync(id);
            return _mapper.Map<FinishedGoodsReceiptDto>(entity);
        }

        public async Task<List<FinishedGoodsReceiptDto>> GetListByWorkOrderProcessIdAsync(int processId)
        {
            var entities = await _finishedGoodsReceiptRepository.GetListByWorkOrderProcessIdAsync(processId);
            return _mapper.Map<List<FinishedGoodsReceiptDto>>(entities);
        }

        public async Task<List<FinishedGoodsReceiptDto>> GetListByWorkOrderProcessIdAsync(List<int> processIds)
        {
            var entities = await _finishedGoodsReceiptRepository.GetListByWorkOrderProcessIdAsync(processIds);
            return _mapper.Map<List<FinishedGoodsReceiptDto>>(entities);
        }

        public Task<bool> UpdateAsync(FinishedGoodsReceiptUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
