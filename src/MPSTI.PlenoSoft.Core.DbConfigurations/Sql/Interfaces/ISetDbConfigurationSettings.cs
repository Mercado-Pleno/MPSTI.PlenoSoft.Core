using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces
{
	public interface ISetDbConfigurationSettings
	{
		string CommandSelectQuerySql { set; }
		string ConfigurationKeyColumn { set; }
		string ConfigurationValueColumn { set; }
		Func<IConfiguration, IDbConnection> DbConnectionFactory { set; }
	}
}