using Microsoft.Extensions.Configuration;
using System;
using System.Data;

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2376:Write-only properties should not be used", Justification = "this is a interface")]

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