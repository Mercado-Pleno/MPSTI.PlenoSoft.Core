using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System.Data;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
{
    public class DbConfigurationSourceProxy : DbConfigurationSource
    {
        private readonly IDbConfigurationSettings _dbConfigurationSettings;
        protected override string CommandSelectQuerySql => _dbConfigurationSettings.CommandSelectQuerySql;
        protected override string ConfigurationKeyColumn => _dbConfigurationSettings.ConfigurationKeyColumn;
        protected override string ConfigurationValueColumn => _dbConfigurationSettings.ConfigurationValueColumn;

        protected override IDbConnection CreateDbConnection(IConfiguration configuration)
        { 
            return _dbConfigurationSettings.DbConnectionFactory?.Invoke(configuration);
        }

        public DbConfigurationSourceProxy(IDbConfigurationSettings dbConfigurationSettings)
        {
            _dbConfigurationSettings = dbConfigurationSettings;
        }
    }
}