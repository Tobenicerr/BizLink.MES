using AutoMapper;
using BizLink.MES.Application.Common;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class CableCutParamService : ICableCutParamService
    {
        private readonly ICableCutParamRepository _cableCutParamRepository;
        private readonly IMapper _mapper; // 2. 声明 IMapper
        private readonly IUnitOfWork _unitOfWork;



        public CableCutParamService(ICableCutParamRepository cableCutParamRepository, IMapper mapper , IUnitOfWork unitOfWork)
        {
            _cableCutParamRepository = cableCutParamRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<CableCutParamDto> CreateAsync(CableCutParamCreateDto createDto)
        {
            var entity = _mapper.Map<CableCutParam>(createDto);
            var rtn =  await _cableCutParamRepository.AddAsync(entity);
            return _mapper.Map<CableCutParamDto>(rtn);
        }

        public async Task<bool> CreateBatchAsync(List<CableCutParamCreateDto> input)
        {
            var rtn = false;

            try
            {
                var entities = await _cableCutParamRepository.GetListBySimiMaterialCodeAsync(input.Select(x => x.SemiMaterialCode).ToList());
                await _unitOfWork.BeginTransactionAsync();
                if (entities != null && entities.Count > 0)
                {
                    rtn = await _cableCutParamRepository.DeleteByIdsAsync(entities.Where(x => input.Select(i => i.PositionItem).Contains(x.PositionItem)).Select(x => x.Id).ToList());
                    //if (!rtn)
                    //    throw new Exception();
                }

                rtn = await _cableCutParamRepository.AddBatchAsync(input.Select(x => _mapper.Map<CableCutParam>(x)).ToList());
                if (!rtn)
                    throw new Exception();


                await _unitOfWork.CommitAsync();
                return rtn;

            }
            catch (Exception ex)
            {

                await _unitOfWork.RollbackAsync();
                return rtn;

            }
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteByIdsAsync(List<int> ids)
        {
            return await _cableCutParamRepository.DeleteByIdsAsync(ids);
        }

        public Task<IEnumerable<CableCutParamDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CableCutParamDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CableCutParamDto>> GetListBySimiMaterialCodeAsync(string semiMaterialCode)
        {
            var entities = await _cableCutParamRepository.GetListBySimiMaterialCodeAsync(semiMaterialCode);
            return entities.Select(x => _mapper.Map<CableCutParamDto>(x)).ToList();
        }

        public async Task<List<CableCutParamDto>> GetListBySimiMaterialCodeAsync(List<string> semiMaterialCode)
        {
            var entities = await _cableCutParamRepository.GetListBySimiMaterialCodeAsync(semiMaterialCode);
            return entities.Select(x => _mapper.Map<CableCutParamDto>(x)).ToList();
        }

        public Task<bool> UpdateAsync(CableCutParamUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }

        Task<List<int>> IGenericService<CableCutParamDto, CableCutParamCreateDto, CableCutParamUpdateDto>.CreateBatchAsync(List<CableCutParamCreateDto> createDto)
        {
            throw new NotImplementedException();
        }
    }
}
