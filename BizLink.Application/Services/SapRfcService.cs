using AutoMapper;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Enums;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using Dm.util;
using Polly;
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
        private readonly IFinishedGoodsReceiptRepository _finishedGoodsReceiptRepository;
        private readonly IFinishedGoodsReceiptDetailRepository _finishedGoodsReceiptDetailRepository;


        private readonly IWorkOrderOperationConfirmRepository _workOrderOperationConfirmRepository;

        private readonly IMapper _mapper; // 2. 声明 IMapper
        public SapRfcService(ISapRfcRepository sapRfcRepository, IMapper mapper, IMaterialTransferLogRepository materialTransferLogRepository, IWorkOrderOperationConfirmRepository workOrderOperationConfirmRepository, IMaterialRepository materialRepository, IFinishedGoodsReceiptDetailRepository finishedGoodsReceiptDetailRepository, IFinishedGoodsReceiptRepository finishedGoodsReceiptRepository)
        {
            _sapRfcRepository = sapRfcRepository;
            _mapper = mapper;
            _materialTransferLogRepository = materialTransferLogRepository;
            _workOrderOperationConfirmRepository = workOrderOperationConfirmRepository;
            _materialRepository = materialRepository;
            _finishedGoodsReceiptDetailRepository = finishedGoodsReceiptDetailRepository;
        }

        public async Task<WorkOrderOperationConfirmDto> ConfirmOrderCompletionToSAPAsync(int sapconfirmid)
        {
            var entity = await _workOrderOperationConfirmRepository.GetConfirmWitemConsumeptionAsync(sapconfirmid);
            entity.Message = string.Empty;
            var result = await _sapRfcRepository.ConfirmOrderCompletionToSAPAsync(entity);
            // 更新状态
            if (result != null) 
            {
                var isSuccess = result.MessageType?.ToUpper() == "S" || (result.Message?.Contains("该报工盘号已处理完成") ?? false);
                entity.Message = result.Message?.Length > 100 ? result.Message.Substring(0, 100) : result.Message;
                entity.MessageType = result.MessageType;
                entity.Status = isSuccess ? "1" : "-1";
                entity.UpdatedAt = DateTime.Now;
                await _workOrderOperationConfirmRepository.UpdateAsync(entity);

            }
            return _mapper.Map<WorkOrderOperationConfirmDto>(result);
        }

        public async Task<List<WorkOrderOperationConfirmDto>> ConfirmBatchOrderCompletionToSAPAsync(List<int> sapconfirmids)
        {
            // 1. 用于收集需要更新的本地实体，以便最后一次性批量更新数据库
            var entitiesToUpdate = new List<WorkOrderOperationConfirm>();

            // 2. 定义 Polly 重试策略：专门应对 SAP 的并发锁定报错
            var sapRetryPolicy = Policy
                .Handle<Exception>(ex =>
                    ex.Message.ToLower().Contains("already being processed") ||
                    ex.Message.Contains("lock") ||
                    ex.Message.Contains("锁定"))
                .WaitAndRetryAsync(
                    3, // 遇到锁定重试 3 次
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // 等待 2s, 4s, 8s
                );

            // 3. 在 Service 内部循环处理
            foreach (var Id in sapconfirmids.Distinct())
            {
                var entity = await _workOrderOperationConfirmRepository.GetConfirmWitemConsumeptionAsync(Id);

                entity.Message = string.Empty;

                try
                {
                    // 4. 使用 Polly 包裹单次 SAP 调用
                    var result = await sapRetryPolicy.ExecuteAsync(async () =>
                    {
                        var res = await _sapRfcRepository.ConfirmOrderCompletionToSAPAsync(entity);

                        // 手动检测返回值中的锁定信息并抛出异常，以激活 Polly 重试
                        if (res != null && !string.IsNullOrEmpty(res.Message))
                        {
                            string msgLower = res.Message.ToLower();
                            if (msgLower.Contains("already being processed") || msgLower.Contains("锁定"))
                            {
                                throw new Exception(res.Message);
                            }
                        }
                        return res;
                    });

                    // 5. 记录单条调用结果
                    if (result != null)
                    {
                        var isSuccess = result.MessageType?.ToUpper() == "S" || (result.Message?.Contains("该报工盘号已处理完成") ?? false);
                        entity.Message = result.Message?.Length > 100 ? result.Message.Substring(0, 100) : result.Message;
                        entity.MessageType = result.MessageType;
                        entity.Status = isSuccess ? "1" : "-1";
                        entity.UpdatedAt = DateTime.Now;
                        entitiesToUpdate.Add(entity);
                    }
                }
                catch (Exception ex)
                {
                    // 捕获重试耗尽或其他异常，保证一条失败不影响下一条的取消
                    entity.Message = $"报工异常: {ex.Message}";
                    entity.MessageType = "E";
                    entity.UpdatedAt = DateTime.Now;
                    entitiesToUpdate.Add(entity);
                }

                // 6. ★ 核心：主动延迟！
                // 每次发完一条指令后，强行休眠 1~1.5 秒，给 SAP 后台进程留出解锁的时间
                // 这能极大程度避免触发 Polly 重试，提升整体成功率
                await Task.Delay(1000);
            }

            // 7. 循环结束后，一次性批量更新本地数据库的状态
            if (entitiesToUpdate.Any())
            {
                // 请确保您的仓储支持 UpdateBulkAsync 或类似的批量修改方法
                await _workOrderOperationConfirmRepository.UpdateBulkAsync(entitiesToUpdate);
            }

            var entities = await _workOrderOperationConfirmRepository.GetByIdsAsync(sapconfirmids);
            return _mapper.Map<List<WorkOrderOperationConfirmDto>>(entities);
        }

        public async Task<List<WorkOrderOperationConfirmDto>> CancelBatchConfirmToSAPAsync(List<int> sapconfirmids)
        {
            //var results = new List<WorkOrderOperationConfirmDto>();

            // 1. 用于收集需要更新的本地实体，以便最后一次性批量更新数据库
            var entitiesToUpdate = new List<WorkOrderOperationConfirm>();

            // 2. 定义 Polly 重试策略：专门应对 SAP 的并发锁定报错
            var sapRetryPolicy = Policy
                .Handle<Exception>(ex =>
                    ex.Message.ToLower().Contains("already being processed") ||
                    ex.Message.Contains("lock") ||
                    ex.Message.Contains("锁定"))
                .WaitAndRetryAsync(
                    3, // 遇到锁定重试 3 次
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // 等待 2s, 4s, 8s
                );

            // 3. 在 Service 内部循环处理
            var entities = await _workOrderOperationConfirmRepository.GetByIdsAsync(sapconfirmids);
            foreach (var entity in entities)
            {
                entity.Message = string.Empty;

                try
                {
                    // 4. 使用 Polly 包裹单次 SAP 调用
                    var sapResult = await sapRetryPolicy.ExecuteAsync(async () =>
                    {
                        var res = await _sapRfcRepository.CancelConfirmToSAPAsync(entity);

                        // 手动检测返回值中的锁定信息并抛出异常，以激活 Polly 重试
                        if (res != null && !string.IsNullOrEmpty(res.Message))
                        {
                            string msgLower = res.Message.ToLower();
                            if (msgLower.Contains("already being processed") || msgLower.Contains("锁定"))
                            {
                                throw new Exception(res.Message);
                            }
                        }
                        return res;
                    });

                    // 5. 记录单条调用结果
                    if (sapResult != null)
                    {
                        entity.Message = sapResult.Message;
                        entity.MessageType = sapResult.MessageType;
                        entity.Status = sapResult.MessageType?.ToUpper() == "S" ? "-2" : entity.Status;
                        entity.UpdatedAt = DateTime.Now;

                        entitiesToUpdate.Add(entity);
                        //results.Add(_mapper.Map<WorkOrderOperationConfirmDto>(sapResult));
                    }
                }
                catch (Exception ex)
                {
                    // 捕获重试耗尽或其他异常，保证一条失败不影响下一条的取消
                    entity.Message = $"取消异常: {ex.Message}";
                    entity.MessageType = "E";
                    entity.UpdatedAt = DateTime.Now;
                    entitiesToUpdate.Add(entity);
                }

                // 6. ★ 核心：主动延迟！
                // 每次发完一条指令后，强行休眠 1~1.5 秒，给 SAP 后台进程留出解锁的时间
                // 这能极大程度避免触发 Polly 重试，提升整体成功率
                await Task.Delay(1000);
            }

            // 7. 循环结束后，一次性批量更新本地数据库的状态
            if (entitiesToUpdate.Any())
            {
                // 请确保您的仓储支持 UpdateBulkAsync 或类似的批量修改方法
                await _workOrderOperationConfirmRepository.UpdateBulkAsync(entitiesToUpdate);
            }

            var result = await _workOrderOperationConfirmRepository.GetByIdsAsync(sapconfirmids);
            return _mapper.Map<List<WorkOrderOperationConfirmDto>>(result);
        }

        public async Task<FinishedGoodsReceiptDto> FinishedGoodsReceiptToSapAsync(FinishedGoodsReceiptDto dto)
        {
            List<MaterialTransferLog> transferLogs = new List<MaterialTransferLog>() 
            {
                new MaterialTransferLog()
                {
                    TransferNo = dto.SapTransferNo,
                    TransferType = MaterialTransferType.FinishedGoodsReceipt.GetDescription(),
                    PostingDate = DateTime.Now.Date,
                    DocumentDate = DateTime.Now.Date,
                    WorkOrderId = dto.WorkOrderId,
                    WorkOrderNo = dto.WorkOrderNo,
                    //WorkCenterCode = dto.WorkCenterCode,
                    MaterialCode = dto.MaterialCode,
                    FactoryCode = dto.FactoryCode,
                    FromLocationCode = dto.StorageLocation,
                    MovementType = ((int)ConsumptionType.FinishedGoodsReceipt).ToString(),
                    Quantity = dto.Quantity,
                    BaseUnit = dto.BaseUnit,
                }
            };
            var result = await _sapRfcRepository.MaterialStockTransferToSAPAsync(transferLogs);
            // 2. ★ 核心修复：安全局部更新，彻底避免报错与数据清空
            if (result != null && result.Any())
            {
                var transfer = result.First();


                // 必须先从数据库中查询出完整的原有实体！
                var receiptToUpdate = await _finishedGoodsReceiptRepository.GetByIdAsync(dto.Id);

                if (receiptToUpdate != null)
                {
                    // 仅修改我们想更新的这几个字段
                    receiptToUpdate.SapBatchNo = transfer.BatchCode;
                    receiptToUpdate.SapMessageType = transfer.MessageType;
                    receiptToUpdate.SapMessage = transfer.Message;
                    receiptToUpdate.SapStatus = transfer.MessageType == "S" ? "1" : "-1";
                    receiptToUpdate.UpdatedOn = DateTime.Now;

                    // 此时 UpdateAsync 拥有完整的对象上下文，不会报错也不会丢失其他字段
                    await _finishedGoodsReceiptRepository.UpdateAsync(receiptToUpdate);
                }
            }

            // 3. 映射并返回最新数据
            var entity = await _finishedGoodsReceiptRepository.GetByIdAsync(dto.Id);
            return _mapper.Map<FinishedGoodsReceiptDto>(entity);

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

        public async Task<SapLabelDataComponentDto> GetSapLabelDataByMaterialCodeAsync(string factoryCode, string materialCode)
        {
            var entity =  await _sapRfcRepository.GetSapLabelDataByMaterialCodeAsync(factoryCode, materialCode);
            return _mapper.Map<SapLabelDataComponentDto>(entity);
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
                        existingMaterial.UpdateBy = "SAP";
                        existingMaterial.UpdatedAt = DateTime.Now;
                        materialsToUpdate.Add(existingMaterial);
                    }
                    else
                    {
                        // 新增物料
                        sapMaterial.MaterialCode = sapMaterial.MaterialCode.TrimStart('0');
                        sapMaterial.CreateAt = DateTime.Now;
                        sapMaterial.CreateBy = "SAP";
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
