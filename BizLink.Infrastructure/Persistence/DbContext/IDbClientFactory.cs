using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Infrastructure.Persistence.DbContext
{
    /// <summary>
    /// Defines a factory for creating and managing different database clients.
    /// </summary>
    public interface IDbClientFactory
    {
        /// <summary>
        /// Gets a SqlSugar client for the specified connection key.
        /// </summary>
        /// <param name="connectionKey">The connection string key from appsettings.json.</param>
        /// <returns>An instance of ISqlSugarClient.</returns>
        ISqlSugarClient GetDbClient(string connectionKey);

        ISqlSugarClient GetDefaultClient();

        void Dispose();
    }
}
