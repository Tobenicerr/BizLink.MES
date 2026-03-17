using BizLink.MES.Domain.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Extensions
{
    public static class SqlSugarSetupExtensions
    {
        /// <summary>
        /// 注册 SqlSugar 及 UnitOfWork 服务
        /// </summary>
        public static IServiceCollection AddSqlSugarSetup(this IServiceCollection services, IConfiguration configuration)
        {
            // 1. 注册 ISqlSugarClient (Scoped)
            services.AddScoped<ISqlSugarClient>(s =>
            {
                var listConfig = new List<ConnectionConfig>();

                // A. 动态读取 "Connections" 节点下的所有数据库配置
                var connectionsSection = configuration.GetSection("Connections");
                var connectionItems = connectionsSection.GetChildren();

                if (!connectionItems.Any())
                {
                    // 兼容旧配置：如果没找到 Connections 节点，尝试读取默认 ConnectionStrings
                    var defaultConn = configuration.GetConnectionString("Default");
                    if (!string.IsNullOrWhiteSpace(defaultConn))
                    {
                        listConfig.Add(new ConnectionConfig
                        {
                            ConfigId = "Default",
                            ConnectionString = defaultConn,
                            DbType = DbType.SqlServer,
                            IsAutoCloseConnection = true,
                            InitKeyType = InitKeyType.Attribute
                        });
                    }
                    else
                    {
                        throw new Exception("未在配置文件中找到 'Connections' 节点或默认连接字符串。");
                    }
                }
                else
                {
                    foreach (var item in connectionItems)
                    {
                        var configId = item.Key; // 节点名称作为 ConfigId (如 "Default", "JyConnection")
                        var connectionString = item["ConnectionString"];
                        var dbTypeString = item["Type"];

                        if (string.IsNullOrWhiteSpace(connectionString)) continue;

                        // 解析数据库类型，默认为 SqlServer
                        if (!Enum.TryParse(dbTypeString, true, out DbType dbType))
                        {
                            dbType = DbType.SqlServer;
                        }

                        listConfig.Add(new ConnectionConfig
                        {
                            ConfigId = configId,
                            ConnectionString = connectionString,
                            DbType = dbType,
                            IsAutoCloseConnection = true,
                            InitKeyType = InitKeyType.Attribute,
                            // 如果是从库，可以设置 SlaveConnectionConfigs
                        });
                    }
                }

                if (listConfig.Count == 0)
                {
                    throw new Exception("未加载到任何有效的数据库连接配置。");
                }

                // C. 创建实例 (使用 SqlSugarScope)
                var db = new SqlSugarScope(listConfig, scope =>
                {
                    // 遍历所有配置，为每个 Tenant 独立绑定事件
                    foreach (var config in listConfig)
                    {
                        var tenantDb = scope.GetConnection(config.ConfigId);

                        tenantDb.Aop.OnLogExecuting = (sql, pars) =>
                        {
                            Console.WriteLine(sql); // 输出原生带有 @ 的 SQL

#if DEBUG
                            // 获取无参数化、直接可运行的 SQL 语句
                            var rawSql = UtilMethods.GetSqlString(config.DbType, sql, pars);
                            Console.WriteLine(rawSql);
                            System.Diagnostics.Debug.WriteLine($"[SQL RAW - {config.ConfigId}]: {rawSql}");
#endif
                        };

                        tenantDb.Aop.OnError = (exp) =>
                        {
                            System.Diagnostics.Debug.WriteLine($"[SQL ERROR - {config.ConfigId}]: \r\n{exp.Sql} \r\n{exp.Message}");
                        };
                    }
                });

                return db;
            });

            // 2. 注册 UnitOfWork (必须是 Scoped，与 SqlSugarClient 保持一致)
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
