using Microsoft.Extensions.Configuration;
using System.Data;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces
{
	public interface IDbConfigurationSettings
	{
		string CommandSelectQuerySql { get; }
		string ConfigurationKeyColumn { get; }
		string ConfigurationValueColumn { get; }
		IDbConnection CreateDbConnection(IConfiguration configuration);
	}
}