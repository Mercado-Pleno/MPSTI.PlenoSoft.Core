﻿using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace MPSTI.PlenoSoft.Core.Configurations.DbSql.Interfaces
{
    public interface IDbConfigurationSettings
    {
        string CommandSelectQuerySql { get; }
        string ConfigurationKeyColumn { get; }
        string ConfigurationValueColumn { get; }
        Func<IConfiguration, IDbConnection> DbConnectionFactory { get; }
        IConfigurationSource CreateConfigurationSource();
    }
}