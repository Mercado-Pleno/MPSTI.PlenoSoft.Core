using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.Configurations.DbSql.Interfaces;

namespace MPSTI.PlenoSoft.Core.Configurations.DbSql
{
    public class DbConfigurationProvider : ConfigurationProvider
    {
        private readonly IDbConfigurationSource _dbConfigurationSource;

        public DbConfigurationProvider(IDbConfigurationSource dbConfigurationSource)
        {
            _dbConfigurationSource = dbConfigurationSource;
        }

        public override void Load()
        {
            _dbConfigurationSource.ExecuteQueryAndLoadData(Data);
        }
    }
}