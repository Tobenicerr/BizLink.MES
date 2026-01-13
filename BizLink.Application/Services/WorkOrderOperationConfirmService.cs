using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Common;
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
    public class WorkOrderOperationConfirmService : IWorkOrderOperationConfirmService
    {
        private readonly IWorkOrderOperationConfirmRepository _workOrderOperationConfirmRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper
        private readonly IUnitOfWork _unitOfWork;

        public WorkOrderOperationConfirmService(IWorkOrderOperationConfirmRepository workOrderOperationConfirmRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _workOrderOperationConfirmRepository = workOrderOperationConfirmRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;

        }

        public async Task<WorkOrderOperationConfirmDto> CreateAsync(WorkOrderOperationConfirmCreateDto createDto)
        {
            var entity = _mapper.Map<WorkOrderOperationConfirm>(createDto); //使用 IMapper 进行映射
            var result = await _workOrderOperationConfirmRepository.AddAsync(entity);
            return _mapper.Map<WorkOrderOperationConfirmDto>(result);
        }

        public Task<List<int>> CreateBatchAsync(List<WorkOrderOperationConfirmCreateDto> createDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<WorkOrderOperationConfirmDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<WorkOrderOperationConfirmDto> GetByIdAsync(int id)
        {
            var entity = await _workOrderOperationConfirmRepository.GetByIdAsync(id);
            return _mapper.Map<WorkOrderOperationConfirmDto>(entity);
        }

        public async Task<WorkOrderOperationConfirmDto> GetConfirmWitemConsumeptionAsync(int confirmid)
        {
            var entity = await _workOrderOperationConfirmRepository.GetConfirmWitemConsumeptionAsync(confirmid);
            return _mapper.Map<WorkOrderOperationConfirmDto>(entity);
        }

        public async Task<List<WorkOrderOperationConfirmDto>> GetListByProcessIdAsync(int processid)
        {
            var entity =  await _workOrderOperationConfirmRepository.GetListByProcessIdAsync(processid);
            return _mapper.Map<List<WorkOrderOperationConfirmDto>>(entity);
        }

        public async Task<List<WorkOrderOperationConfirmDto>> GetListByProcessIdAsync(List<int> processids)
        {
            var entities = await _workOrderOperationConfirmRepository.GetListByProcessIdAsync(processids);
            return _mapper.Map<List<WorkOrderOperationConfirmDto>>(entities);
        }

        public async Task<PagedResultDto<WorkOrderOperationConfirmDto>> GetOperationConfirmPageListAsync(int pageIndex, int pageSize, List<string>? orders, string? status, List<string>? operation)
        {
            var (entities,totalCount) = await _workOrderOperationConfirmRepository.GetOperationConfirmPageListAsync(pageIndex, pageSize,orders, status, operation);

            return new PagedResultDto<WorkOrderOperationConfirmDto> { Items = _mapper.Map<List<WorkOrderOperationConfirmDto>>(entities), TotalCount = totalCount };
        }

        public async Task<bool> SplitOperationConfirmAsync(int confirmId,decimal splitQuantity,string confirmSeq)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var entity = await _workOrderOperationConfirmRepository.GetByIdAsync(confirmId);

                var updateDto = new WorkOrderOperationConfirmUpdateDto()
                {
                    Id = entity.Id,
                    YieldQuantity = entity.YieldQuantity - splitQuantity
                };
                _mapper.Map(updateDto, entity);
                var result = await _workOrderOperationConfirmRepository.UpdateAsync(entity);
                entity.YieldQuantity = splitQuantity;
                entity.ConfirmSequence = confirmSeq;
                entity.CompletedFlag = string.Empty;
                var newEntity = await _workOrderOperationConfirmRepository.AddAsync(entity);
                await _unitOfWork.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
            
        }

        public async Task<bool> UpdateAsync(WorkOrderOperationConfirmUpdateDto updateDto)
        {
            var entity = await _workOrderOperationConfirmRepository.GetByIdAsync(updateDto.Id);
            _mapper.Map(updateDto, entity);
            return await _workOrderOperationConfirmRepository.UpdateAsync(entity);
        }
    }
}
