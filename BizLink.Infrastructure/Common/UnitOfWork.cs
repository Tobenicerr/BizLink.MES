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
        // 注入的 SqlSugarClient 必须是 Scoped (WebAPI) 或在 Scope 中创建 (WinForms)
        // 且初始化时必须传入 List<ConnectionConfig> 以支持多库
        private readonly ISqlSugarClient _db;
        private bool _hasActiveTransaction;

        public UnitOfWork(ISqlSugarClient db)
        {
            _db = db;
        }

        // 1. 获取默认数据库连接
        public ISqlSugarClient DbClient => _db;

        // 2. 获取指定 ConfigId 的数据库连接 (多库支持)
        // 官方文档：自动换库 -> 通过 ConfigId 获取对应的 Client
        public ISqlSugarClient GetDbClient(string configId)
        {
            // AsTenant() 是 SqlSugar 处理多租户/多库的核心入口
            // GetConnection(configId) 会自动切换到对应的库，且如果事务已开启，会自动加入事务
            return _db.AsTenant().GetConnection(configId);
        }

        public async Task BeginTransactionAsync()
        {
            // 官方文档：事务嵌套/多库事务
            // 使用 AsTenant().BeginTran() 可以同时开启主库和所有从库的事务上下文
            // 这样无论后续 Repository 操作哪个库，只要是在同一个 Scope 内，都会受此事务控制
            await _db.AsTenant().BeginTranAsync();
        }

        public async Task CommitAsync()
        {
            try
            {
                // 提交所有库的事务
                await _db.AsTenant().CommitTranAsync();
            }
            catch
            {
                // 提交失败则回滚
                await _db.AsTenant().RollbackTranAsync();
                throw;
            }
        }

        public async Task RollbackAsync()
        {
            // 回滚所有库的事务
            await _db.AsTenant().RollbackTranAsync();
        }

        public void Dispose()
        {
            // 官方文档：禁止用 db.Rollback，工作单元内只要 throw 会自动回滚
            // 但在我们封装的模式下，为了安全起见，Dispose 时如果事务未提交，执行回滚
            // 使用本地标记替代 _db.AsTenant().IsAnyTran
            if (_hasActiveTransaction)
            {
                _db.AsTenant().RollbackTran();
                _hasActiveTransaction = false;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hasActiveTransaction)
            {
                await _db.AsTenant().RollbackTranAsync();
                _hasActiveTransaction = false;
            }
        }
    }
}
