using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.Configurations.DbSql.Interfaces;
using System.Data;

namespace MPSTI.PlenoSoft.Core.Configurations.DbSql
{
    public class DbConfigurationSourceProxy : DbConfigurationSource
    {
        private readonly IDbConfigurationSettings _dbConfigurationSettings;
        protected override string CommandSelectQuerySql => _dbConfigurationSettings.CommandSelectQuerySql;
        protected override string ConfigurationKeyColumn => _dbConfigurationSettings.ConfigurationKeyColumn;
        protected override string ConfigurationValueColumn => _dbConfigurationSettings.ConfigurationValueColumn;
        protected override IDbConnection CreateDbConnection(IConfiguration configuration) => _dbConfigurationSettings.DbConnectionFactory.Invoke(configuration);

        public DbConfigurationSourceProxy(IDbConfigurationSettings dbConfigurationSettings)
        {
            _dbConfigurationSettings = dbConfigurationSettings;
        }
    }
}