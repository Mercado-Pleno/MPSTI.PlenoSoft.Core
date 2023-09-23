using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
{
    public class DbConfigurationProvider : ConfigurationProvider
    {
        private readonly List<Exception> _exceptions = new();
        private readonly IDbConfigurationSource _dbConfigurationSource;
        public Exception LastException => _exceptions.LastOrDefault();

        public DbConfigurationProvider(IDbConfigurationSource dbConfigurationSource)
        {
            _dbConfigurationSource = dbConfigurationSource;
        }

        public override void Load()
        {
            try
            {
                _dbConfigurationSource.ExecuteQueryAndFillDataSource(Data);
            }
            catch (Exception exception)
            {
                _exceptions.Add(exception);
            }
        }
    }
}