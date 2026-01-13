using BizLink.MES.Infrastructure.Persistence.DbContext;
using Dm;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Common
{
    public class UnitOfWork : IUnitOfWork, IAsyncDisposable, IDisposable
    {
        private readonly IDbClientFactory _dbClientFactory;
        private readonly ConcurrentDictionary<string, ISqlSugarClient> _activeClients;
        private volatile bool _isDisposed;

        // 状态管理
        private volatile bool _isTransactionActive;
        private volatile bool _isTransactionCompleted;

        public UnitOfWork(IDbClientFactory dbClientFactory)
        {
            _dbClientFactory = dbClientFactory;
            _activeClients = new ConcurrentDictionary<string, ISqlSugarClient>();
        }

        public ISqlSugarClient DbClient => GetDbClient();

        public ISqlSugarClient GetDbClient(string configId = "Default")
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            // 1. 获取或创建 Client
            var client = _activeClients.GetOrAdd(configId, (key) =>
            {
                return _dbClientFactory.GetDbClient(key);
            });

            // 2. 【关键】延迟加入事务逻辑
            // 如果 UoW 认为当前应该在事务中，但该 Client 还没开启事务，则强制开启
            if (_isTransactionActive && !_isTransactionCompleted)
            {
                // 乐观检查：SqlSugarClient 的 Ado.Transaction 为 null 表示未开启
                if (client.Ado.Transaction == null)
                {
                    lock (client)
                    {
                        if (client.Ado.Transaction == null)
                        {
                            try
                            {
                                client.Ado.BeginTran();
                            }
                            catch (Exception ex)
                            {
                                // 这里的异常通常是因为 SqlSugarClient 被多线程并发使用了
                                throw new InvalidOperationException($"无法为配置 '{configId}' 挂载事务。请确保不要在多线程中并发使用同一个 DbClient 实例。", ex);
                            }
                        }
                    }
                }
            }

            return client;
        }

        public async Task BeginTransactionAsync()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            // 防止重复开启
            if (_isTransactionActive)
            {
                return;
                // 或者抛出异常：throw new InvalidOperationException("Transaction already started.");
            }

            // 防止复用已完成的 UoW（重要！避免状态混乱）
            if (_isTransactionCompleted)
            {
                throw new InvalidOperationException("Cannot restart a transaction on a completed UnitOfWork. Please create a new Scope/UnitOfWork.");
            }

            // 开启当前已存在的 Client 的事务
            foreach (var client in _activeClients.Values)
            {
                if (client.Ado.Transaction == null)
                {
                    await client.Ado.BeginTranAsync();
                }
            }

            _isTransactionActive = true;
        }

        public async Task CommitAsync()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            // 如果事务没开启或者已经完成了，直接报错
            if (!_isTransactionActive || _isTransactionCompleted)
            {
                throw new InvalidOperationException("No active transaction to commit.");
            }

            try
            {
                // 提交所有连接
                // 【警告】如果涉及多个不同库的 Client，这里非强一致性事务。如果第2个失败，第1个无法回滚。
                foreach (var client in _activeClients.Values)
                {
                    if (client.Ado.Transaction != null)
                    {
                        await client.Ado.CommitTranAsync();
                    }
                }
            }
            catch (Exception commitEx)
            {
                // 提交失败，尝试回滚（Best Effort）
                try
                {
                    await RollbackInternalAsync();
                }
                catch (Exception rollbackEx)
                {
                    // 记录聚合异常，方便排查
                    throw new AggregateException("Transaction Commit failed, and subsequent Rollback also failed.", commitEx, rollbackEx);
                }
                throw; // 抛出原始提交异常
            }
            finally
            {
                // 标记为完成，禁止后续操作
                _isTransactionCompleted = true;
                // _isTransactionActive = false; // 保持为 true 或 false 均可，依靠 Completed 标记来阻断
            }
        }

        public async Task RollbackAsync()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(UnitOfWork));

            // 如果事务没开启，或者已经完成了，无需回滚
            if (!_isTransactionActive || _isTransactionCompleted)
            {
                return;
            }

            try
            {
                await RollbackInternalAsync();
            }
            finally
            {
                _isTransactionCompleted = true;
            }
        }

        private async Task RollbackInternalAsync()
        {
            var exceptions = new List<Exception>();

            foreach (var client in _activeClients.Values)
            {
                try
                {
                    if (client.Ado.Transaction != null)
                    {
                        await client.Ado.RollbackTranAsync();
                    }
                }
                catch (Exception ex)
                {
                    // 忽略连接已关闭等无效状态错误，但记录其他错误
                    var msg = ex.Message.ToLower();
                    if (!msg.Contains("completed") && !msg.Contains("closed") && !msg.Contains("zombie"))
                    {
                        exceptions.Add(ex);
                    }
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException("Rollback failed with unexpected errors.", exceptions);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return;

            // 自动回滚未提交的事务
            if (_isTransactionActive && !_isTransactionCompleted)
            {
                try
                {
                    await RollbackInternalAsync();
                }
                catch { /* 吞掉 Dispose 中的异常 */ }
            }

            _activeClients.Clear();
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            if (_isTransactionActive && !_isTransactionCompleted)
            {
                try
                {
                    // 同步回滚逻辑
                    foreach (var client in _activeClients.Values)
                    {
                        try
                        {
                            if (client.Ado.Transaction != null)
                                client.Ado.RollbackTran();
                        }
                        catch { }
                    }
                }
                catch { }
            }

            _activeClients.Clear();
            _isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
