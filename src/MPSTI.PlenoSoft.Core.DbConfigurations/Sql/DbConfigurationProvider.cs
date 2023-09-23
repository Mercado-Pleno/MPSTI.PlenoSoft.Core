using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
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
            _dbConfigurationSource.ExecuteQueryAndFillDataSource(Data);
        }
    }
}