using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System;
using System.Data;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql
{
    public class DbConfigurationSettings : IDbConfigurationSettings
    {
        public string CommandSelectQuerySql { get; set; }
        public string ConfigurationKeyColumn { get; set; }
        public string ConfigurationValueColumn { get; set; }
        public Func<IConfiguration, IDbConnection> DbConnectionFactory { get; set; }

        IConfigurationSource IDbConfigurationSettings.CreateConfigurationSource()
        {
            return new DbConfigurationSourceProxy(this);
        }
    }
}