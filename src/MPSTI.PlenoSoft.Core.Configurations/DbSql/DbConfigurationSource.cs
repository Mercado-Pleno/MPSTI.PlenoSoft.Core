using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.Configurations.DbSql.Extensions;
using MPSTI.PlenoSoft.Core.Configurations.DbSql.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.Configurations.DbSql
{
    public abstract class DbConfigurationSource : IDbConfigurationSource
    {
        private IConfigurationBuilder _configurationBuilder;
        protected abstract string CommandSelectQuerySql { get; }
        protected abstract string ConfigurationKeyColumn { get; }
        protected abstract string ConfigurationValueColumn { get; }
        protected abstract IDbConnection CreateDbConnection(IConfiguration configuration);

        IConfigurationProvider IConfigurationSource.Build(IConfigurationBuilder builder)
        {
            _configurationBuilder = builder;
            return new DbConfigurationProvider(this);
        }

        void IDbConfigurationSource.ExecuteQueryAndLoadData(IDictionary<string, string> dataSource)
        {
            using var dbConnection = GetConnection();
            using var dbCommand = dbConnection.CreateCommand(CommandSelectQuerySql);
            using var dataReader = dbCommand.ExecuteReader();
            var keyIndex = dataReader.GetOrdinal(ConfigurationKeyColumn);
            var valueIndex = dataReader.GetOrdinal(ConfigurationValueColumn);
            while (dataReader.Read())
            {
                var key = dataReader.GetString(keyIndex);
                var value = dataReader.GetString(valueIndex);
                dataSource[key] = value;
            }
            dataReader.Close();
            dbConnection.Close();
        }

        private IDbConnection GetConnection()
        {
            var configuration = BuildPartialConfiguration();
            var dbConnection = CreateDbConnection(configuration);
            dbConnection.Open();
            return dbConnection;
        }

        private IConfiguration BuildPartialConfiguration()
        {
            var configurationProviders = _configurationBuilder.Sources
                .Where(x => x is not IDbConfigurationSource)
                .Select(x => x.Build(_configurationBuilder)).ToArray();

            foreach (var configurationProvider in configurationProviders)
                configurationProvider.Load();

            return new ConfigurationRoot(configurationProviders);
        }
    }
}