using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizLink.MES.Application.Services
{
    public class SapRfcService : ISapRfcService
    {
        private readonly ISapRfcRepository _sapRfcRepository;
        private readonly IMaterialTransferLogRepository _materialTransferLogRepository;
        private readonly IMaterialRepository _materialRepository;

        private readonly IWorkOrderOperationConfirmRepository _workOrderOperationConfirmRepository;

        private readonly IMapper _mapper; // 2. 声明 IMapper
        public SapRfcService(ISapRfcRepository sapRfcRepository, IMapper mapper, IMaterialTransferLogRepository materialTransferLogRepository, IWorkOrderOperationConfirmRepository workOrderOperationConfirmRepository, IMaterialRepository materialRepository)
        {
            _sapRfcRepository = sapRfcRepository;
            _mapper = mapper;
            _materialTransferLogRepository = materialTransferLogRepository;
            _workOrderOperationConfirmRepository = workOrderOperationConfirmRepository;
            _materialRepository = materialRepository;
        }

        public async Task<WorkOrderOperationConfirmDto> ConfirmOrderCompletionToSAPAsync(int sapconfirmid)
        {
            var entity = await _workOrderOperationConfirmRepository.GetConfirmWitemConsumeptionAsync(sapconfirmid);
            entity.Message = string.Empty;
            var result = await _sapRfcRepository.ConfirmOrderCompletionToSAPAsync(entity);
            return _mapper.Map<WorkOrderOperationConfirmDto>(result);
        }

        public async Task<List<CableCutParamCreateDto>> GetCableCutParamByMaterialsAsync(List<string> semimaterialcode)
        {
            var entities = await _sapRfcRepository.GetCableCutParamByMaterialsAsync(semimaterialcode);
            return entities.Select(x => new CableCutParamCreateDto() 
            {
               CuttoLeranceId =  x.CuttoLeranceId,
                SemiMaterialCode = x.SemiMaterialCode,
                CableMaterialCode = x.CableMaterialCode,
                CableType = x.CableType,
                DrawingCode = x.DrawingCode,
                PositionItem = x.PositionItem,
                CablePcs = x.CablePcs,
                PostionNo = x.PostionNo,
                BomLength = x.BomLength,
                UpTol = x.UpTol,
                DownTol = x.DownTol,
                AlphaFactor = x.AlphaFactor, 
                BetaFactor = x.BetaFactor,
                CuttingLength = x.CuttingLength,
                CuttingTime = x.CuttingTime,
                ReelCode = x.ReelCode,
                Remark =    x.Remark,
                Status = x.Status,
                CreateDate = x.CreateDate,
                CreateTime = x.CreateTime,
                UpdateDate = x.UpdateDate,
                UpdateTime = x.UpdateTime
            }).ToList();
        }

        public async Task<SapOrderDto> GetCN10WorkOrdersAsync(string plantcode, DateTime? dispatchdate, List<string> workcentercode, List<string> orders = null)
        {
            var (operations, bom) = await _sapRfcRepository.GetCN10WorkOrdersAsync(plantcode, dispatchdate, workcentercode, orders);
            return new SapOrderDto()
            {
                sapOrderOperations = operations,
                sapOrderBoms = bom,
            };
        }

        public async Task<List<SapRawMaterialStockDto>> GetRawMaterialStockFromSapAsync(List<SapRawMaterialStockDto> materialCodes)
        {
            var entities = materialCodes.Select(x => new SapRawMaterialStock() 
            {
                MaterialCode = x.MaterialCode,
                FactoryCode = x.FactoryCode,
                BatchCode = x.BatchCode,
                LocationCode = x.LocationCode
            }).ToList();

            var result = await _sapRfcRepository.GetRawMaterialStockFromSapAsync(entities);
            return _mapper.Map<List<SapRawMaterialStockDto>>(result);

        }

        public async Task<SapOrderDto> GetWorkOrdersAsync(string plantcode, DateTime? dispatchdate, List<string> orders = null)
        {
            var (operations,bom) =  await _sapRfcRepository.GetWorkOrdersAsync(plantcode, dispatchdate, orders);
            return new SapOrderDto()
            {
                sapOrderOperations = operations,
                sapOrderBoms = bom,
            };
        }

        public async Task<List<MaterialTransferLogDto>> MaterialStockTransferToSAPAsync(List<MaterialTransferLogDto> input)
        {
            List<MaterialTransferLog> entityList = _mapper.Map<List<MaterialTransferLog>>(input);
            var result = await _sapRfcRepository.MaterialStockTransferToSAPAsync(entityList);

            var updateDict = new Dictionary<(string TransferNo, string MaterialCode), MaterialTransferLog>();
            foreach (var update in result)
            {
                // 使用 (update.TransferNo, update.MaterialCode) 元组作为键
                updateDict[(update.TransferNo, update.MaterialCode)] = update;
            }

            // 2. 遍历主列表并使用复合键进行更新
            foreach (var entity in entityList)
            {
                // 尝试使用相同的复合键 (entity.TransferNo, entity.MaterialCode) 从字典中获取更新
                if (updateDict.TryGetValue((entity.TransferNo, entity.MaterialCode), out MaterialTransferLog newlog))
                {
                    // 找到了匹配项，更新库存
                    entity.Status = newlog.MessageType.ToUpper() == "S" ? "1" : "-1";
                    entity.Message = newlog.Message;
                    entity.MessageType = newlog.MessageType;
                }
                // else: 字典中没有这个 (TransferNo, MaterialCode) 组合的更新，保持 entity 原样
            }
            return entityList.Select(x => _mapper.Map<MaterialTransferLogDto>(x)).ToList();
        }

        public async Task<List<MaterialTransferLogDto>> RawMaterialInventoryAdjustmentAsync(List<MaterialTransferLogDto> input)
        {
            List<MaterialTransferLog> entityList = _mapper.Map<List<MaterialTransferLog>>(input);
            var result = await _sapRfcRepository.RawMaterialInventoryAdjustmentAsync(entityList);

            var updateDict = new Dictionary<(string TransferNo, string MaterialCode), MaterialTransferLog>();
            foreach (var update in result)
            {
                // 使用 (update.TransferNo, update.MaterialCode) 元组作为键
                updateDict[(update.TransferNo, update.MaterialCode)] = update;
            }

            // 2. 遍历主列表并使用复合键进行更新
            foreach (var entity in entityList)
            {
                // 尝试使用相同的复合键 (entity.TransferNo, entity.MaterialCode) 从字典中获取更新
                if (updateDict.TryGetValue((entity.TransferNo, entity.MaterialCode), out MaterialTransferLog newlog))
                {
                    // 找到了匹配项，更新库存
                    entity.Status = newlog.MessageType.ToUpper() == "S" ? "1" : "-1";
                    entity.Message = newlog.Message;
                    entity.MessageType = newlog.MessageType;
                }
                // else: 字典中没有这个 (TransferNo, MaterialCode) 组合的更新，保持 entity 原样
            }
            return entityList.Select(x => _mapper.Map<MaterialTransferLogDto>(x)).ToList();
        }

        public async Task<bool> SyncMaterialFromSAPAsync(string factoryCode, List<string>? materialCodes, DateTime? startTime, DateTime? endTime)
        {
            var materialsap = (await _sapRfcRepository.GetSAPMaterialAsync(factoryCode, materialCodes, startTime, endTime)).GroupBy(x => x.MaterialCode).Select(g => g.First());
            if (materialsap == null || materialsap.Count() == 0)
                throw new Exception("未查询到物料信息，无法同步！");
            else
            {
                var existingMaterials = await _materialRepository.GetListByMaterialCodesAsync(materialsap.Select(m => m.MaterialCode.TrimStart('0')).ToList());
                var materialsToUpdate = new List<Material>();
                var materialsToInsert = new List<Material>();
                foreach (var sapMaterial in materialsap)
                {
                    if ((sapMaterial.IsConsumption ?? "").Contains("X"))
                        sapMaterial.ConsumeType = 1;
                    else
                    {
                        if (!(sapMaterial.IsCableMaterial ?? "").Contains("X"))
                        {
                            if((sapMaterial.BaseUnit??"").Contains("ST") && (sapMaterial.MaterialCode??"").StartsWith("0000000000008"))
                                sapMaterial.ConsumeType = 2;

                            //按单超市料
                            if ((sapMaterial.BaseUnit ?? "").Contains("ST") && (sapMaterial.MaterialCode ?? "").StartsWith("E") && string.IsNullOrWhiteSpace(sapMaterial.SpecialProcurement) && (sapMaterial.ProcurementType ?? "").Contains("E"))
                                sapMaterial.ConsumeType = 2;
                        }
                            
                    }

                    if ((sapMaterial.IsCableMaterial ?? "").Contains("X"))
                        sapMaterial.ConsumeType = 0;

                    var existingMaterial = existingMaterials.FirstOrDefault(m => m.MaterialCode == sapMaterial.MaterialCode.TrimStart('0'));
                    if (existingMaterial != null)
                    {
                        // 更新现有物料
                        existingMaterial.MaterialDescription = sapMaterial.MaterialDescription;
                        existingMaterial.ConsumeType = sapMaterial.ConsumeType;
                        existingMaterial.BaseUnit = sapMaterial.BaseUnit;
                        existingMaterial.IsActive = true;
                        existingMaterial.IsProduce = true;
                        existingMaterial.UpdatedAt = DateTime.Now;
                        materialsToUpdate.Add(existingMaterial);
                    }
                    else
                    {
                        // 新增物料
                        sapMaterial.MaterialCode = sapMaterial.MaterialCode.TrimStart('0');
                        sapMaterial.CreateAt = DateTime.Now;
                        materialsToInsert.Add(sapMaterial);
                    }
                }
                if (materialsToUpdate.Count > 0)
                {
                    await _materialRepository.UpdateBatchAsync(materialsToUpdate);
                }
                if (materialsToInsert.Count > 0)
                {
                    await _materialRepository.CreateBatchAsync(materialsToInsert);
                }
                return true;
            }
        }


    }
}
