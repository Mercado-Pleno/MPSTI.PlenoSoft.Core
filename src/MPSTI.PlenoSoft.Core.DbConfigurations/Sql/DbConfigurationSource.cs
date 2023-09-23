using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
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

        void IDbConfigurationSource.ExecuteQueryAndFillDataSource(IDictionary<string, string> dataSource)
        {
            using var dbConnection = GetConnection();
            try
            {
                using var dbCommand = dbConnection.CreateCommand(CommandSelectQuerySql);
                using var dataReader = dbCommand.ExecuteReader();
                try
                {
                    var keyIndex = dataReader.GetOrdinal(ConfigurationKeyColumn);
                    var valueIndex = dataReader.GetOrdinal(ConfigurationValueColumn);
                    while (dataReader.Read())
                    {
                        var key = dataReader.GetString(keyIndex);
                        var value = dataReader.GetString(valueIndex);
                        dataSource[key] = value;
                    }
                }
                finally
                {
                    dataReader.Close();
                }
            }
            finally
            {
                dbConnection.Close();
            }
        }

        private IDbConnection GetConnection()
        {
            var configuration = BuildPartialConfiguration();
            var dbConnection = CreateDbConnection(configuration);

            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            return dbConnection;
        }

        private IConfiguration BuildPartialConfiguration()
        {
            var configurationProviders = _configurationBuilder.Sources
                .Where(x => x is not IDbConfigurationSource)
                .Select(x => x.Build(_configurationBuilder))
                .ToArray();

            foreach (var configurationProvider in configurationProviders)
                configurationProvider.Load();

            return new ConfigurationRoot(configurationProviders);
        }
    }
}