using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.DbConfigurations.Sql.Interfaces
{
	public interface IDbConfigurationSource : IConfigurationSource
	{
		void FillDataSource(IDictionary<string, string> dataSource);
	}
}