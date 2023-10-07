using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces
{
	public interface IGetDbConfigurationSettings
	{
		TimeSpan CheckChangeInterval { get; }
		string CommandSelectQuerySql { get; }
		string ConfigurationKeyColumn { get; }
		string ConfigurationValueColumn { get; }
		IDbConnection CreateDbConnection(IConfiguration configuration);
	}
}