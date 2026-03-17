using Azure.Core;
using BizLink.MES.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Application.DTOs
{
    public class SapIntegrationService : ISapIntegrationService
    {
        private readonly IMaterialTransferLogService _materialTransferLogService;
        private readonly IParameterGroupService _parameterGroupService;
        private readonly ISapRfcService _sapRfcService;
        private const string SAP_TRANSFER_CONFIG_GROUP = "TransferSAP";
        private const string SAP_CONFIG_KEY_ENABLED = "IsEnabled";
        public SapIntegrationService(IMaterialTransferLogService materialTransferLogService, IParameterGroupService parameterGroupService, ISapRfcService sapRfcService) 
        {
            _materialTransferLogService = materialTransferLogService;
            _parameterGroupService = parameterGroupService;
            _sapRfcService = sapRfcService;
        }

        public async Task TransferToSapAsync(List<MaterialTransferLogCreateDto> createDtos, string employeeId)
        {
            if (createDtos == null || !createDtos.Any()) return;

            // 1. 【持久化】先批量创建本地日志 (Status = Pending)
            // 获取回写的 ID，这对于后续更新至关重要
            var transferIds = await _materialTransferLogService.CreateBatchAsync(createDtos);

            // 重新查出带 ID 的完整记录
            var transferLogs = await _materialTransferLogService.GetListByIdsAsync(transferIds);

            // 2. 【检查开关】
            bool isSapEnabled = await CheckSapEnabledAsync();

            List<MaterialTransferLogDto> finalResults = new List<MaterialTransferLogDto>();

            // 3. 【无事务区间】调用 SAP 接口
            try
            {
                if (isSapEnabled)
                {
                    // 调用外部 RFC 服务
                    var sapResponse = await _sapRfcService.MaterialStockTransferToSAPAsync(transferLogs);

                    if (sapResponse != null && sapResponse.Any())
                    {
                        finalResults = sapResponse;
                    }
                    else
                    {
                        // 接口返回空列表，视为异常
                        finalResults = GenerateErrorResults(transferLogs, "调用SAP接口返回空结果，未获取到处理状态");
                    }
                }
                else
                {
                    // 模拟模式：直接标记成功
                    finalResults = transferLogs.Select(log =>
                    {
                        // 创建副本或直接修改，确保 ID 存在
                        log.MessageType = "S";
                        log.Message = "Simulation: SAP Call Disabled";
                        return log;
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // 捕获异常，确保本地数据库能更新为"失败"状态
                finalResults = GenerateErrorResults(transferLogs, $"SAP接口调用异常: {ex.Message}");
            }

            // 4. 【短事务区间 B】回写结果
            // 关键：这里必须确保 finalResults 里的对象拥有正确的 Id
            var updateDtos = finalResults.Select(item => new MaterialTransferLogUpdateDto
            {
                Id = item.Id, // 确保 RFC Service 传回了这个 ID
                Message = item.Message,
                MessageType = item.MessageType,
                // 根据 SAP 返回的 Type 判断状态：S=成功(1), E/A/X=失败(-1)
                Status = string.Equals(item.MessageType, "S", StringComparison.OrdinalIgnoreCase) ? "1" : "-1",
                UpdatedBy = employeeId,
                UpdatedAt = DateTime.Now
            }).ToList();

            if (updateDtos.Any())
            {
                await _materialTransferLogService.UpdateListAsync(updateDtos);
            }
        }

        /// <summary>
        /// 辅助方法：安全读取配置开关
        /// </summary>
        private async Task<bool> CheckSapEnabledAsync()
        {
            try
            {
                var group = await _parameterGroupService.GetGroupWithItemsAsync(SAP_TRANSFER_CONFIG_GROUP);
                var param = group?.Items?.FirstOrDefault(x => x.Key == SAP_CONFIG_KEY_ENABLED);

                // 默认策略：如果没配置，默认开启还是关闭？这里假设没配置则开启(与原代码逻辑一致)
                if (param == null) return true;

                // 安全解析 bool
                if (bool.TryParse(param.Value, out bool result))
                {
                    return result;
                }

                // 兼容 "1"/"0"
                if (param.Value == "1") return true;

                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        /// <summary>
        /// 辅助方法：批量生成错误结果
        /// </summary>
        private List<MaterialTransferLogDto> GenerateErrorResults(List<MaterialTransferLogDto> sources, string errorMsg)
        {
            return sources.Select(log =>
            {
                // 注意：不要直接修改 sources 里的引用，最好 clone，但这里直接修改 DTO 通常也可以
                log.MessageType = "E";
                log.Message = errorMsg;
                return log;
            }).ToList();
        }
    }
}
