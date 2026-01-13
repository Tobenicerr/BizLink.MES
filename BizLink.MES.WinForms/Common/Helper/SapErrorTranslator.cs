using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BizLink.MES.WinForms.Common.Helper
{
    public static class SapErrorTranslator
    {
        // 定义翻译规则字典
        // Key: 英文错误信息的正则表达式 (使用 @"" 避免转义麻烦)
        // Value: 中文翻译模版 ($1 代表第一个括号匹配的内容，$2 代表第二个，以此类推)
        private static readonly Dictionary<string, string> TranslationRules = new Dictionary<string, string>
    {
        // --- 1. 处理您图片中的特定错误 ---
        // 英文: Order 3643764 is already being processed by FUNC11519
        // 解释: (\w+) 匹配任何单词字符（包括数字和字母）
        { @"^Order\s+(\w+)\s+is already being processed by\s+(\w+)", "订单 $1 正在被 $2 处理中，请先从SAP中退出后再重新操作！" },
        // 前置工序未确认 ---
        // 英文: Preceding operation 0300 of sequence 0 not yet confirmed;
        // 解释: (\w+) 捕获工序号 (0300) 和 序列号 (0)
        // 中文: 序列 0 的前置工序 0300 尚未确认
        { @"^Preceding operation\s+(\w+)\s+of sequence\s+(\w+)\s+not yet confirmed", " 前置子订单的工序 $1 尚未报工确认，请先将前道工序 $1 报工后再进行操作！" },

        // --- 新增规则 2: 前置工序确认数量不足 ---
        // 英文: Up until now only 9 PCS confirmed for predecessors 0100 / 0;
        // 解释: 
        //   ([\d\.]+) 捕获数量 (9)，支持小数
        //   (\w+) 捕获前置工序号 (0100)
        //   (\w+) 捕获前置序列号 (0)
        // 中文: 前置工序 0100 / 0 目前仅确认了 9 件
        { @"^Up until now only\s+([\d\.]+)\s+PCS confirmed for predecessors\s+(\w+)\s+/\s+(\w+)", "前置工序 $2 / $3 目前仅确认了 $1 件，数量不足，当前工序无法超量报工！" },

  // --- 新增规则 3: 库存不足 (Deficit of ...) ---
        // 场景 A: 带批次 (例如: Deficit of BA Unrestricted-use 38,798 M : 808828 CN11 2200 0010994877)
        // 解释: 
        //   $1 (\w+)       : 库存类型 (BA/SL)
        //   $2 ([\d\.,]+)  : 数量 (支持逗号分隔，如 38,798)
        //   $3 (\w+)       : 单位 (PCS/M)
        //   $4-$7          : 物料、工厂、库位、批次
        { @"^Deficit of (\w+) Unrestricted-use\s+([\d\.,]+)\s+(\w+)\s*:\s*(\w+)\s+(\w+)\s+(\w+)\s+(\w+)", "$1 非限制使用库存不足 $2 $3。详情: 物料 $4 / 工厂 $5 / 库位 $6 / 批次 $7" },

       // 场景 B: 不带批次 (例如: Deficit of SL Unrestricted-use 8 PCS : 848894 CN11 1100)
        // 更新: 同样支持带横杠的物料号
        { @"^Deficit of (\w+) Unrestricted-use\s+([\d\.,]+)\s+(\w+)\s*:\s*([\w-]+)\s+(\w+)\s+(\w+)(?!\s+\w)", "$1 非限制使用库存不足 $2 $3。详情: 物料 $4 / 工厂 $5 / 库位 $6" },



        // --- 新增规则 4: 批次不存在 (Batch ... does not exist) ---
        // 英文: Batch 842112 CN11 2200 0011004503 does not exist
        { @"^Batch\s+(\w+)\s+(\w+)\s+(\w+)\s+(\w+)\s+does not exist", "指定批次信息不存在 (物料: $1, 工厂: $2, 库位: $3, 批次: $4)" },

        // --- 新增规则 5: 请输入批次 (Enter Batch) ---
        // 英文: Enter Batch
        { @"^Enter Batch", "请输入批次号" },

        // --- 新增规则 6: 物料号不存在 (Material number ... does not exist) ---
        // 英文: Material number  808323 does not exist
        // 注意: 原文中数字前有两个空格，使用 \s+ 可以兼容一个或多个空格
        { @"^Material number\s+(\w+)\s+does not exist", "当前物料 $1 不存在非限制使用库存" },

        // --- 新增规则 7: 用户状态激活限制 (User status ... is active) ---
        // 英文: User status NMVT is active (ORD 9860707)
        // 解释: $1 状态码(NMVT), $2 订单号(9860707)
        { @"^User status\s+(\w+)\s+is active\s+\(ORD\s+(\w+)\)", "订单 $2 当前处于用户状态 $1（关单状态），请联系计划重开订单" },

        // --- 新增规则 8: 不允许已多重分类 (Multiple classification not allowed) ---
        // 英文: Multiple classification not allowed
        { @"^Multiple classification not allowed", "不允许进行多重分类操作，请在SAP中修改物料批次的属性分类！" },

        // --- 新增规则 9: 需要分配成本对象 (Account ... requires an assignment to a CO object) ---
        // 英文: Account 41050500 requires an assignment to a CO object
        { @"^Account\s+(\w+)\s+requires an assignment to a CO object", "科目 $1 需要分配成本对象 (CO Object)" },

        // --- 新增规则 10: 前置工序确认数量不足 (格式 B) ---
        // 英文: Only                 2 PCS for previous operation 0300 / 000000 from order 9861240 has been confirmed
        // 解释: 
        //   $1 ([\d\.,]+) : 数量 (2)
        //   $2 (\w+)      : 工序号 (0300)
        //   $3 (\w+)      : 序列号 (000000)
        //   $4 (\w+)      : 订单号 (9861240)
        { @"^Only\s+([\d\.,]+)\s+PCS\s+for\s+previous\s+operation\s+(\w+)\s+/\s+(\w+)\s+from\s+order\s+(\w+)\s+has\s+been\s+confirmed", "订单 $4 的前置工序 $2 / $3 仅确认了 $1 件，数量不足，无法继续报工！" },

    };

        /// <summary>
        /// 将英文错误信息转换为中文
        /// </summary>
        /// <param name="englishMessage">原始英文消息</param>
        /// <returns>翻译后的中文，如果未匹配到规则则返回原句</returns>
        public static string ToChinese(string englishMessage)
        {
            if (string.IsNullOrWhiteSpace(englishMessage))
                return englishMessage;

            foreach (var rule in TranslationRules)
            {
                // RegexOptions.IgnoreCase: 忽略大小写
                // RegexOptions.CultureInvariant: 忽略区域性差异
                if (Regex.IsMatch(englishMessage, rule.Key, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
                {
                    // 执行替换：将 $1, $2 替换为实际捕获的值
                    try
                    {
                        return Regex.Replace(englishMessage, rule.Key, rule.Value, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                    }
                    catch
                    {
                        // 防止正则替换出错，回退到原文本
                        return englishMessage;
                    }
                }
            }

            // 如果没有匹配到任何规则，直接返回原始英文
            return englishMessage;
        }
    }
}
