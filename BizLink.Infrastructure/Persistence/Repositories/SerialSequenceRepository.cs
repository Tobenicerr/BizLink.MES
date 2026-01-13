using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Entities;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.Repositories.Common;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.Repositories
{
    public class SerialSequenceRepository : GenericRepository<SerialSequence>, ISerialSequenceRepository
    {

        private readonly ConcurrentDictionary<string, SerialRule> _ruleCache = new ConcurrentDictionary<string, SerialRule>();
        public SerialSequenceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }


        /// <summary>
        /// 根据配置的规则生成下一个唯一的序列号。
        /// </summary>
        /// <param name="ruleName">用于生成的规则的名称。</param>
        /// <returns>一个新的、唯一的序列号字符串。</returns>
        public string GenerateNext(string ruleName)
        {
            // 从缓存或数据库中获取规则
            var rule = GetRule(ruleName);
            if (rule == null)
            {
                throw new ArgumentException($"未在数据库中找到序列号规则 '{ruleName}'。", nameof(ruleName));
            }

            var sb = new StringBuilder();

            // 1. 添加静态前缀
            if (!string.IsNullOrEmpty(rule.Prefix))
            {
                sb.Append(rule.Prefix);
            }

            // 2. 添加日期部分
            string datePart = string.Empty;
            if (!string.IsNullOrEmpty(rule.DateFormat))
            {
                datePart = DateTime.Now.ToString(rule.DateFormat);
                sb.Append(datePart);
            }

            // 3. 从数据库获取并更新序列号
            string sequenceKey = $"{rule.Name}{datePart}";

            int nextSequence = GetNextSequenceValue(sequenceKey);

            sb.Append(nextSequence.ToString($"D{rule.SequenceLength}"));

            return sb.ToString();
        }

        public SerialRule? GetRule(string ruleName)
        {
            // 尝试从缓存获取
            if (_ruleCache.TryGetValue(ruleName, out var rule))
            {
                return rule;
            }
            // 如果缓存中没有，则从数据库查询
            rule = _db.Queryable<SerialRule>().InSingle(ruleName);
            if (rule != null)
            {
                // 存入缓存
                _ruleCache.TryAdd(ruleName, rule);
            }
            return rule;
        }

        /// <summary>
        /// 通过数据库事务安全地获取并增加序列值。
        /// </summary>
        private int GetNextSequenceValue(string sequenceKey)
        {
            try
            {
                // 开启数据库事务
                _db.Ado.BeginTran();

                // 查询并锁定行，防止其他客户端同时读取。
                // 这相当于 SQL 中的 "SELECT ... FOR UPDATE"。
                var sequence = _db.Queryable<SerialSequence>()
                                  .Where(s => s.SequenceKey == sequenceKey)
                                  .TranLock()
                                  .First();

                if (sequence != null)
                {
                    // 如果存在记录，则递增
                    sequence.LastValue++;
                    sequence.UpdatedAt = DateTime.Now;
                    _db.Updateable(sequence).ExecuteCommand();
                }
                else
                {
                    // 如果不存在，则创建新记录
                    sequence = new SerialSequence
                    {
                        SequenceKey = sequenceKey,
                        LastValue = 1,
                        UpdatedAt = DateTime.Now
                    };
                    _db.Insertable(sequence).ExecuteCommand();
                }

                // 提交事务
                _db.Ado.CommitTran();

                return sequence.LastValue;
            }
            catch (Exception)
            {
                // 如果发生任何错误，则回滚事务
                _db.Ado.RollbackTran();
                throw;
            }
        }
    }
}
