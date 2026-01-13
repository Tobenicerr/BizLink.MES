using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Domain.Common
{
    public interface IUnitOfWork : IDisposable
    {
        // 添加这个属性
        //bool IsTransactionCompleted
        //{
        //    get;
        //}
        // 获取默认的数据库客户端
        ISqlSugarClient DbClient
        {
            get;
        }

        // 根据连接ID获取指定的数据库客户端
        ISqlSugarClient GetDbClient(string configId = "Default");

        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }

}
