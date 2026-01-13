using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.DbContext
{
    /// <summary>
    /// Implements the IDbClientFactory to provide ISqlSugarClient instances for different databases.
    /// </summary>
    public class DbClientFactory : IDbClientFactory
    {
        private readonly IConfiguration _configuration;
        private readonly ConcurrentDictionary<string, ISqlSugarClient> _clients = new ConcurrentDictionary<string, ISqlSugarClient>();

        public DbClientFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ISqlSugarClient GetDbClient(string connectionKey)
        {
            // 如果客户端已被创建和缓存，则直接返回
            if (_clients.TryGetValue(connectionKey, out var client))
            {
                return client;
            }
            // 1. 获取指定 key 下的整个连接配置节
            var connectionSection = _configuration.GetSection($"Connections:{connectionKey}");
            if (!connectionSection.Exists())
            {
                throw new ArgumentException($"在 appsettings.json 的 'Connections' 中未找到名为 '{connectionKey}' 的配置节。");
            }
            // 2. 从配置节中分别读取连接字符串和数据库类型
            var connectionString = connectionSection["ConnectionString"];
            var dbTypeString = connectionSection["Type"];

            //var connectionString = _configuration.GetConnectionString(connectionKey);
            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(dbTypeString))
            {
                throw new ArgumentException($"连接配置 '{connectionKey}' 必须同时包含 'ConnectionString' 和 'Type' 属性。");
            }

            // 3. 将字符串解析为 SqlSugar 的 DbType 枚举
            if (!Enum.TryParse<DbType>(dbTypeString, true, out var dbType))
            {
                throw new ArgumentException($"无法识别的数据库类型: '{dbTypeString}'。");
            }

            var newClient = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = dbType, // 4. 使用动态解析出的数据库类型
                IsAutoCloseConnection = true, // 建议在Web应用中设置为 true，SqlSugar会处理好连接的生命周期
                ConfigureExternalServices = new ConfigureExternalServices()
                {
                    EntityNameService = (type, entity) =>
                    {
                        entity.IsDisabledUpdateAll = true;
                    },
                    EntityService = (type, entity) =>
                    {
                        entity.IsDisabledAlterColumn = true;
                    },
                }
            },
            db =>
            {
                db.Ado.CommandTimeOut = 300;//单位秒
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    // 在这里，你可以将 SQL 和参数打印到任何你需要的地方
                    // 比如控制台、日志文件等

                    Console.WriteLine("----------- Sugar SQL Start -----------");
                    Console.WriteLine("SQL语句:");
                    Console.WriteLine(sql); // 打印生成的SQL

                    Console.WriteLine("参数:");
                    // pars 是一个 SqlParameter 数组，可以遍历打印
                    foreach (var p in pars)
                    {
                        Console.WriteLine($"  {p.ParameterName}: {p.Value}");
                    }

                    Console.WriteLine("------------ Sugar SQL End ------------");
                    Console.WriteLine(); // 换行，让日志更清晰
                };
            }

            );


            // 将新客户端存入缓存并返回
            _clients.TryAdd(connectionKey, newClient);
            return newClient;
        }

        public ISqlSugarClient GetDefaultClient()
        {
            // "Default" 是您在 appsettings.json 中配置的主数据库连接Id
            return GetDbClient("Default");
        }

        public void Dispose()
        {
            foreach (var client in _clients.Values)
            {
                client.Dispose();
            }
            _clients.Clear();
        }
    }
}
