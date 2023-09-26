﻿using Microsoft.Extensions.Configuration;
using MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces;
using System;
using System.Data;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Extensions
{
    public class DbConfigurationSettings : IDbConfigurationSettings, ISetDbConfigurationSettings
	{
        public virtual string CommandSelectQuerySql { get; set; }
        public virtual string ConfigurationKeyColumn { get; set; }
        public virtual string ConfigurationValueColumn { get; set; }
        public Func<IConfiguration, IDbConnection> DbConnectionFactory { get; set; }

        public virtual IDbConnection CreateDbConnection(IConfiguration configuration)
        {
			return DbConnectionFactory.Invoke(configuration);
		}

		IDbConnection IDbConfigurationSettings.CreateDbConnection(IConfiguration configuration)
        {
            return CreateDbConnection(configuration);
        }

        public override string ToString() => $"'{CommandSelectQuerySql}' - (KeyColumn: {ConfigurationKeyColumn}, ValueColumn: {ConfigurationValueColumn})";
    }
}